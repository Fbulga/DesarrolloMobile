using System;
using Enum;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public abstract class BaseBall : MonoBehaviour
{
    [SerializeField] protected CollisionCheck collisionCheck;
    [SerializeField] protected BallData ballData;
    protected Collider2D[] colliders = new Collider2D[5];
    protected CircleCollider2D circleCollider2D;

    protected Vector2 direction;
    public Vector2 Direction => direction;
    
    
    [Header("Bounce Config")]
    private Vector3 originalScale;
    [SerializeField] private Vector3 bounceScale = new Vector3(0.5f, 0.5f, 0.25f);
    [SerializeField] private Color bounceColor = Color.white;
    private Color originalColor;
    [SerializeField] private float animationLenght = 0.2f;
    [SerializeField] private GameObject bounceParticles;
    private SpriteRenderer spriteRenderer;
    private bool isAnimating = false;
    [SerializeField] private SFXType bounceSFX; 

    protected virtual void Start()
    {
        circleCollider2D = GetComponent<CircleCollider2D>();
        circleCollider2D.radius = ballData.CollisionRadius;
        direction = new Vector2(Random.Range(-1f, 1f), -1f);
        
        ///animacion
        originalScale = transform.localScale;
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
        EjecutarRebote();
    }

    protected virtual void Update()
    {
        Physics2D.OverlapCircleNonAlloc(transform.position, ballData.CollisionRadius, colliders, ballData.ObstacleLayer);
        CheckCollisions();
        MoveBall();
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
        SFXManager.Instance.PlaySFXClip(bounceSFX);
        StatManager.Instance.IncreaseStat(Stat.TotalBallBounces,1f);
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
        if (!isAnimating && gameObject.activeInHierarchy)
        {
            StartCoroutine(AnimacionRebote());
        }
    }

    private System.Collections.IEnumerator AnimacionRebote()
    {
        isAnimating = true;

        // Cambiar color
        spriteRenderer.color = bounceColor;

        // Reproducir efecto de partículas si está asignado
        if (ballData.ParticleBouncePrefab != null)
        {
            PoolManager.Instance.GetPowerUp(ballData.ParticleBouncePrefab,transform.position);
            //Instantiate(ballData.ParticleBouncePrefab, transform.position, Quaternion.identity);
        }

        float mitadDuracion = animationLenght / 2f;

        yield return Escalar(originalScale, originalScale/2f, mitadDuracion);
        
        // Escalar hacia escalaRebote
        yield return Escalar(originalScale/2f, bounceScale, mitadDuracion);

        // Volver a la escala original
        yield return Escalar(bounceScale, originalScale, mitadDuracion);
        
        // Restaurar color original
        spriteRenderer.color = originalColor;

        isAnimating = false;
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

