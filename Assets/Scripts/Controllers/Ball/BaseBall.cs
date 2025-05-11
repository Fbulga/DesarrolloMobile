using System;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class BaseBall : MonoBehaviour
{
    [SerializeField] protected CollisionCheck collisionCheck;
    [SerializeField] protected BallData ballData;
    protected Collider2D[] colliders = new Collider2D[5];
    protected CircleCollider2D circleCollider2D;

    protected Vector2 direction;
    public Vector2 Direction => direction;
    
    
    [Header("Configuración de Escala")]
    public Vector3 escalaRebote = new Vector3(1.2f, 0.8f, 1.0f);
    public float duracionAnimacion = 0.2f;

    [Header("Configuración de Color")]
    public Color colorRebote = Color.yellow;

    [Header("Efectos Visuales")]
    public GameObject efectoParticulas;

    private Vector3 escalaOriginal;
    private Color colorOriginal;
    private SpriteRenderer spriteRenderer;
    private bool animando = false;

    

    protected virtual void Start()
    {
        circleCollider2D = GetComponent<CircleCollider2D>();
        circleCollider2D.radius = ballData.CollisionRadius;
        direction = new Vector2(Random.Range(-1f, 1f), -1f);
        
        ///animacion
        escalaOriginal = transform.localScale;
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            colorOriginal = spriteRenderer.color;
        }
        EjecutarRebote();
    }

    protected virtual void Update()
    {
        MoveBall();
        Physics2D.OverlapCircleNonAlloc(transform.position, ballData.CollisionRadius, colliders, ballData.ObstacleLayer);
        CheckCollisions();
    }

    protected void MoveBall()
    {
        transform.position += (Vector3)(GetSpeed() * direction * Time.deltaTime);
    }
    
    protected void Reflect(CollisionResponseDto response)
    {
        Vector2 normal = response.collisionNormal;
        direction = Vector2.Reflect(direction, normal).normalized;
        transform.position = response.closestPoint + normal * (circleCollider2D.radius + 0.025f);

        if (Mathf.Abs(direction.x) < 0.01f)
            direction.x = Random.Range(-0.5f, 0.5f);

        if (Mathf.Abs(direction.y) < 0.01f)
            direction.y = Random.Range(-0.5f, 0.5f);

        
        EjecutarRebote();
        direction = direction.normalized;
        
    }
    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    protected abstract float GetSpeed();
    protected abstract void CheckCollisions();
    
    /// <summary>
    /// Animacion
    /// </summary>
    public void EjecutarRebote()
    {
        if (!animando && gameObject.activeInHierarchy)
        {
            StartCoroutine(AnimacionRebote());
        }
    }

    private System.Collections.IEnumerator AnimacionRebote()
    {
        animando = true;

        // Cambiar color
        spriteRenderer.color = colorRebote;

        // Reproducir efecto de partículas si está asignado
        if (ballData.ParticleBouncePrefab != null)
        {
            //PoolManager.Instance.GetPowerUp(ballData.ParticleBouncePrefab,transform.position);
            Instantiate(ballData.ParticleBouncePrefab, transform.position, Quaternion.identity);
        }

        float mitadDuracion = duracionAnimacion / 2f;

        yield return Escalar(escalaOriginal, escalaOriginal/2f, mitadDuracion);
        
        // Escalar hacia escalaRebote
        yield return Escalar(escalaOriginal/2f, escalaRebote, mitadDuracion);

        // Volver a la escala original
        yield return Escalar(escalaRebote, escalaOriginal, mitadDuracion);
        
        // Restaurar color original
        spriteRenderer.color = colorOriginal;

        animando = false;
    }

    private System.Collections.IEnumerator Escalar(Vector3 desde, Vector3 hasta, float duracion)
    {
        float tiempoTranscurrido = 0f;
        while (tiempoTranscurrido < duracion)
        {
            transform.localScale = Vector3.Lerp(desde, hasta, tiempoTranscurrido / duracion);
            tiempoTranscurrido += Time.deltaTime;
            yield return null;
        }
        transform.localScale = hasta;
    }

    private void OnDisable(){
        StopCoroutine(AnimacionRebote());
    }
    
}

