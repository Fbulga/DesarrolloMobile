using System.Collections;
using Enum;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class BaseBall : MonoBehaviour
{
    [Header("Visual References")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform visualTransform;
    
    [Header("Ball Settings")]
    [SerializeField] private float minRandomX = -1f, maxRandomX = 1f;
    [SerializeField] protected BallData ballData;
    
    [Header("Bounce Config")]
    [SerializeField] private PrefabsType particlesType;
    [SerializeField] private float animationLength = 0.2f;
    [SerializeField] private Vector3 bounceScale;
    [SerializeField] private SFXType bounceSFX;
    [SerializeField] private Color bounceColor;

    public Vector2 Direction => direction;
    
    protected float currentSpeed;
    protected Vector2 direction;
    
    private RaycastHit2D[] raycastHits = new RaycastHit2D[4];
    private CircleCollider2D circleCollider2D;
    private Vector2 initialDirection;
    private Vector3 visualScale;
    private Color originalColor;
    private bool isAnimating;
    
    // Unity Methods, acostumbrate a dejarlos juntos
    
    private void Awake()
    {
        circleCollider2D = GetComponent<CircleCollider2D>();
    }

    protected virtual void Start()
    {
        Initialize();
        HandlePlatform();
        PlayBounceEffect();
    }

    private void Update()
    {
        MoveBall();
    }
    
    protected virtual void FixedUpdate()
    {
        float distance = currentSpeed * Time.fixedDeltaTime;
        if (TryGetCollisionHit(out RaycastHit2D hit, distance))
        {
            HandleCollision(hit);
        }
        Debug.DrawRay(transform.position, direction * 3f, Color.magenta);
    }
    
    private void OnDisable()
    {
        StopCoroutine(BounceAnimationRoutine());
    }

    // Metodos! 
    public void SetDirection(Vector2 dir)
    {
        direction = dir;
    }
    
    private void Initialize()
    {
        circleCollider2D.radius = ballData.CollisionRadius;
        direction = GetInitialDirection();
        currentSpeed = ballData.Speed;
        visualScale = visualTransform.localScale;

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }
    private bool TryGetCollisionHit(out RaycastHit2D hit, float distance)
    {
        float scaledRadius = GetScaledCollisionRadius();

        int hitCount = Physics2D.CircleCastNonAlloc(
            transform.position,
            scaledRadius,
            direction,
            raycastHits,
            distance,
            ballData.ObstacleLayer
        );

        for (int i = 0; i < hitCount; i++)
        {
            if (raycastHits[i].collider != null)
            {
                hit = raycastHits[i];
                return true;
            }
        }

        hit = default;
        return false;
    }

    private float GetScaledCollisionRadius()
    {
        float visualScale = Mathf.Max(transform.lossyScale.x, transform.lossyScale.y);
        float scaledRadius = circleCollider2D.radius * visualScale;
        return scaledRadius;
    }
    
    protected Vector2 CalculatePaddleBounce(Vector2 ballPosition, Transform paddleTransform, float maxBounceAngle = 75f)
    {
        // PUNTO DE IMPACTO LOCAL en coordenadas de la paleta
        Vector2 localHitPoint = paddleTransform.InverseTransformPoint(ballPosition);

        // Medimos el ancho real de la paleta (seguro y escalado)
        BoxCollider2D box = paddleTransform.GetComponent<BoxCollider2D>();
        float halfWidth = box.size.x * 0.5f * paddleTransform.lossyScale.x;

        // Cuánto se desvió a izquierda o derecha (-1 a 1)
        float normalizedX = Mathf.Clamp(localHitPoint.x / halfWidth, -1f, 1f);

        // Convertimos a ángulo (rad) según el máximo
        float angleRad = normalizedX * maxBounceAngle * Mathf.Deg2Rad;

        // Calculamos nueva dirección
        Vector2 newDir = new Vector2(Mathf.Sin(angleRad), Mathf.Cos(angleRad));

        // Siempre hacia arriba
        newDir.y = Mathf.Abs(newDir.y);

        return newDir.normalized;
    }
    protected virtual void HandleCollision(RaycastHit2D hit)
    {
        Reflect(hit); // -> Abajo le puedes meter un evento OnHit?.Invoke y que lo escuche el hijo visual para hacer el efecto de escalarse
    }
    
    private void MoveBall()
    {
        transform.position += (Vector3)(direction * (currentSpeed * Time.deltaTime));
    }

    private void Reflect(RaycastHit2D hit)
    {
        Vector2 normal = hit.normal;
        direction = Vector2.Reflect(direction, normal).normalized;
        float visualScale = Mathf.Max(transform.lossyScale.x, transform.lossyScale.y);
        float scaledRadius = circleCollider2D.radius * visualScale;
        transform.position = hit.point + hit.normal * (scaledRadius + 0.025f);

        /*if (Mathf.Abs(direction.x) < 0.01f)
        {
            direction.x = Random.Range(-0.5f, 0.5f);
        }

        if (Mathf.Abs(direction.y) < 0.01f)
        {
            direction.y = Random.Range(-0.5f, 0.5f);
        }*/
        
        // El de abajo evita que quede horizontal, en vez de tirarle un ramdom como arriba
        //EnforceMinimumAngle(ref direction, 15f);
        
        PlayBounceEffect();
        direction.Normalize();
    }
    private Vector3 GetInitialDirection()
    {
        float x = Random.Range(minRandomX, maxRandomX);
        float y = -1f;
        initialDirection = new Vector2(x, y);
        return initialDirection;
    }
    //No usado pero puede evitar que la bota quede horizontal. Ni puta idea como funciona o si funciona jajaja
    private void EnforceMinimumAngle(ref Vector2 dir, float minAngleDegrees = 15f)
    {
        float angle = Vector2.Angle(dir, Vector2.up);
        if (angle < minAngleDegrees || angle > (180f - minAngleDegrees))
        {
            float sign = Mathf.Sign(dir.x);
            float angleRad = minAngleDegrees * Mathf.Deg2Rad;
            dir = new Vector2(Mathf.Sin(angleRad) * sign, Mathf.Cos(angleRad));
        }
    }
    //Este metodo y los de abajo se los puedes lanzar a la clase en el hijo visual y se suscriba al evento OnHit. No deberian estar aqui
    private void PlayBounceEffect()
    {
        SFXManager.Instance.PlaySFXClip(bounceSFX);
        StatManager.Instance.IncreaseStat(Stat.TotalBallBounces,1f);
        VibrationManager.VibrateLight();
        if (!isAnimating && gameObject.activeInHierarchy)
        {
            StartCoroutine(BounceAnimationRoutine());
        }
    }
    //Este tambien
    private IEnumerator BounceAnimationRoutine()
    {
        isAnimating = true;

        // Cambiar color
        spriteRenderer.color = bounceColor;

        GameObject ballParticles = PoolManager.Instance.GetPowerUp(particlesType,transform.position);
        Particles particles = ballParticles.GetComponent<Particles>();
        particles.prefabType = particlesType;
        

        float mitadDuracion = animationLength/2f;

        yield return Scale(visualScale, visualScale/2f, mitadDuracion);
        
        // Escalar hacia escalaRebote
        yield return Scale(visualScale/2f, bounceScale, animationLength);

        // Volver a la escala original
        yield return Scale(bounceScale, visualScale, mitadDuracion);
        
        // Restaurar color original
        spriteRenderer.color = originalColor;

        isAnimating = false;
    }
    //Y este
    private IEnumerator Scale(Vector3 desde, Vector3 hasta, float duracion)
    {
        float tiempoTranscurrido = 0f;
        while (tiempoTranscurrido < duracion)
        {
            visualTransform.localScale = Vector3.Lerp(desde, hasta, tiempoTranscurrido / duracion);
            tiempoTranscurrido += Time.deltaTime;
            yield return null;
        }
        visualTransform.localScale = hasta;
    }
    private void HandlePlatform()
    {
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
    }
    
    //Gizmos para debug! Son super utiles
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying || direction == Vector2.zero || circleCollider2D == null)
            return;

        Gizmos.color = Color.cyan;

        float castDistance = currentSpeed * Time.fixedDeltaTime;
        Vector3 origin = transform.position;
        Vector3 castEnd = origin + (Vector3)(direction.normalized * castDistance);

        // 🔧 Escala real del collider usando el lossyScale
        float scaledRadius = circleCollider2D.radius * Mathf.Max(transform.lossyScale.x, transform.lossyScale.y);

        // ✅ Usa el radio escalado para representar visualmente el tamaño real
        
        //Te dejo los emojis de mi pana Chatgpt
        Gizmos.DrawWireSphere(origin, scaledRadius);
        Gizmos.DrawWireSphere(castEnd, scaledRadius);

        Gizmos.DrawLine(origin, castEnd);

        for (int i = 0; i < raycastHits.Length; i++)
        {
            if (raycastHits[i].collider != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(raycastHits[i].point, 0.05f);
            }
        }
    }
}