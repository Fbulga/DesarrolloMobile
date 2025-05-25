using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public class BallVisuals : MonoBehaviour
{
    
    [Header("Visual References")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    public SpriteRenderer SpriteRenderer => spriteRenderer;
    [SerializeField] private TrailRenderer trailRenderer;
    public TrailRenderer TrailRenderer => trailRenderer;
    
    
    [Header("Bounce Config")]
    [SerializeField] private PrefabsType particlesType;
    [SerializeField] private float animationLength = 0.2f;
    [SerializeField] private Vector3 bounceScale;
    [SerializeField] private SFXType bounceSFX;
    [SerializeField] private Color bounceColor;

    private Vector3 visualScale;
    private Color originalColor;
    private bool isAnimating;

    
    
    
    // Start is called before the first frame update
    void Start()
    {
        HandlePlatform();
     
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        trailRenderer = gameObject.GetComponent<TrailRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
        visualScale = transform.localScale;
        
        PlayBounceEffect();
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
    
    private IEnumerator BounceAnimationRoutine()
    {
        isAnimating = true;

        // Cambiar color
        spriteRenderer.color = bounceColor;

        GameObject ballParticles = PoolManager.Instance.GetPowerUp(particlesType,transform.position);
        Particles particles = ballParticles.GetComponent<Particles>();
        particles.prefabType = particlesType;
        

        float mitadDuracion = animationLength/2f;
        
        yield return Scale(visualScale, visualScale/2f, mitadDuracion/1.5f);
        
        // Escalar hacia escalaRebote
        yield return Scale(visualScale/2f, bounceScale, animationLength);
        
        // Volver a la escala original
        yield return Scale(bounceScale, visualScale, mitadDuracion);
        
        // Restaurar color original
        
        spriteRenderer.color = originalColor;
        
        isAnimating = false;
    }
    private IEnumerator Scale(Vector3 desde, Vector3 hasta, float duracion)
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

    public void HandleBounceEffect()
    {
        PlayBounceEffect();
    }
    
    private void OnDisable()
    {
        StopCoroutine(BounceAnimationRoutine());
    }
}
