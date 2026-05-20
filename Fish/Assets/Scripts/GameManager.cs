using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI Elements")]
    public Transform bgTransform;
    public Transform wave1Transform;
    public Transform wave2Transform;
    public Transform wave3Transform;
    public Transform floatTransform;

    [Header("Animation Settings")]
    public float waveInterval = 1f;
    public float floatAnimationSpeed = 0.1f;

    private SpriteRenderer[] waveRenderers;
    private SpriteRenderer floatRenderer;
    private Sprite[] floatSprites;
    private int currentFloatFrame = 0;
    private float waveTimer = 0f;
    private int currentWave = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        InitializeWaveRenderers();
        InitializeFloatAnimation();
        SetupUIElements();
    }

    void InitializeWaveRenderers()
    {
        waveRenderers = new SpriteRenderer[3];
        if (wave1Transform != null) waveRenderers[0] = wave1Transform.GetComponent<SpriteRenderer>();
        if (wave2Transform != null) waveRenderers[1] = wave2Transform.GetComponent<SpriteRenderer>();
        if (wave3Transform != null) waveRenderers[2] = wave3Transform.GetComponent<SpriteRenderer>();

        foreach (var renderer in waveRenderers)
        {
            if (renderer != null)
            {
                renderer.enabled = false;
            }
        }
    }

    void InitializeFloatAnimation()
    {
        if (floatTransform != null)
        {
            floatRenderer = floatTransform.GetComponent<SpriteRenderer>();
            floatSprites = Resources.LoadAll<Sprite>("float");
            
            if (floatSprites.Length > 0 && floatRenderer != null)
            {
                floatRenderer.sprite = floatSprites[0];
            }
        }
    }

    void SetupUIElements()
    {
        Camera.main.aspect = 9f / 16f;
        Camera.main.orthographic = true;
        Camera.main.orthographicSize = 5f;
    }

    void Update()
    {
        UpdateFloatAnimation();
        UpdateWaveAnimation();
    }

    void UpdateFloatAnimation()
    {
        if (floatSprites != null && floatSprites.Length > 0 && floatRenderer != null)
        {
            currentFloatFrame++;
            if (currentFloatFrame >= floatSprites.Length * (1 / floatAnimationSpeed))
            {
                currentFloatFrame = 0;
            }
            floatRenderer.sprite = floatSprites[Mathf.FloorToInt(currentFloatFrame * floatAnimationSpeed)];
        }
    }

    void UpdateWaveAnimation()
    {
        waveTimer += Time.deltaTime;
        
        if (waveTimer >= waveInterval)
        {
            waveTimer = 0f;
            
            foreach (var renderer in waveRenderers)
            {
                if (renderer != null)
                {
                    renderer.enabled = false;
                }
            }
            
            if (waveRenderers[currentWave] != null)
            {
                waveRenderers[currentWave].enabled = true;
            }
            
            currentWave = (currentWave + 1) % 3;
        }
    }
}