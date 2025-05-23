using System;
using Enum;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public abstract class BaseBall : MonoBehaviour
{
    [SerializeField] protected CollisionCheck collisionCheck;
    [SerializeField] protected BallData ballData;
    protected Collider2D[] colliders = new Collider2D[2];
    protected CircleCollider2D circleCollider2D;

    protected Vector2 direction;
    public Vector2 Direction => direction;
    
    
    [Header("Bounce Config")]
    private Vector3 originalScale;
    [SerializeField] private Vector3 bounceScale;
    [SerializeField] private PrefabsType particlesType;
    [SerializeField] private Color bounceColor;
    private Color originalColor;
    [SerializeField] private float animationLenght = 0.2f;
    private SpriteRenderer spriteRenderer;
    private bool isAnimating = false;
    [SerializeField] private SFXType bounceSFX; 

    protected virtual void Start()
    {
        circleCollider2D = GetComponent<CircleCollider2D>();
        circleCollider2D.radius = ballData.CollisionRadius;
        direction = new Vector2(Random.Range(-1f, 1f), -1f);

        if (Application.isMobilePlatform)
        {
            gameObject.GetComponent<GhostSprites>().enabled = false;
            gameObject.GetComponent<TrailRenderer>().enabled = true;
        }
        else
        {
            gameObject.GetComponent<GhostSprites>().enabled = true;
            gameObject.GetComponent<TrailRenderer>().enabled = false;
        }
        
        ///animacion
        originalScale = transform.localScale;
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
        EjecutarRebote();
    }

 

    protected virtual void FixedUpdate()
    {
        Physics2D.OverlapCircleNonAlloc(transform.position, ballData.CollisionRadius, colliders, ballData.ObstacleLayer);
        CheckCollisions();
    }

    private void Update()
    {
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
        {
            direction.x = Random.Range(-0.5f, 0.5f);
        }

        if (Mathf.Abs(direction.y) < 0.01f)
        {
            direction.y = Random.Range(-0.5f, 0.5f);
        }
        
        EjecutarRebote();
        direction = direction.normalized;
        
    }
    public void SetDirection(Vector2 dir)
    {
        direction = dir;
    }

    protected abstract float GetSpeed();
    protected abstract void CheckCollisions();
    
    /// <summary>
    /// Animacion
    /// </summary>
    public void EjecutarRebote()
    {
        SFXManager.Instance.PlaySFXClip(bounceSFX);
        StatManager.Instance.IncreaseStat(Stat.TotalBallBounces,1f);
        VibrationManager.VibrateLight();
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

        GameObject ballParticles = PoolManager.Instance.GetPowerUp(particlesType,transform.position);
        Particles particles = ballParticles.GetComponent<Particles>();
        particles.prefabType = particlesType;
        

        float mitadDuracion = animationLenght/2f;

        yield return Escalar(originalScale, originalScale/2f, mitadDuracion);
        
        // Escalar hacia escalaRebote
        yield return Escalar(originalScale/2f, bounceScale, animationLenght);

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

