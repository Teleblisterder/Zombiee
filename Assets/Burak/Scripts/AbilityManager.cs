using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class AbilityManager : MonoBehaviour
{
    public static AbilityManager Instance;

    [Header("Yetenek Ayarları")]
    public float cooldown = 15f;
    public float pushDistance = 8f;
    public float damage = 2f;
    private float nextReadyTime;

    [Header("Patlama Alanı")]
    public Vector2 boxSize = new Vector2(5f, 8f);
    public Vector2 boxOffset = new Vector2(2f, -4f);
    public LayerMask enemyLayer;

    [Header("UI & Tutorial")]
    public Slider skillSlider;
    public GameObject tutorialText;
    private bool tutorialShown = false; 
    private bool isPausedForTutorial = false;

    void Awake() { Instance = this; }

    void Start()
    {
        nextReadyTime = Time.time;
        if (skillSlider != null) skillSlider.maxValue = cooldown;
        if (tutorialText != null) tutorialText.SetActive(false);
    }

    void Update()
    {
       
        if (skillSlider != null)
        {
           
            float progress = Mathf.Clamp(Time.time - (nextReadyTime - cooldown), 0, cooldown);
            skillSlider.value = progress;
        }


        if (Input.GetKeyDown(KeyCode.Space))
        {
           
            if (isPausedForTutorial)
            {
                EndTutorial();
            }

            if (Time.time >= nextReadyTime)
            {
                UseShockwave();
            }
        }
    }

    public void StartTutorial()
    {
        if (tutorialShown) return; 

        tutorialShown = true;
        isPausedForTutorial = true;
        
        if (tutorialText != null) tutorialText.SetActive(true);
        
        Time.timeScale = 0.1f; 
        Time.fixedDeltaTime = 0.02f * Time.timeScale; 
    }

    void EndTutorial()
    {
        isPausedForTutorial = false;
        if (tutorialText != null) tutorialText.SetActive(false);
        
        Time.timeScale = 1f; 
        Time.fixedDeltaTime = 0.02f;
    }

    void UseShockwave()
    {
        Vector2 spawnPos = (Vector2)transform.position + boxOffset;
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(spawnPos, boxSize, 0f, enemyLayer);

        if (hitEnemies.Length > 0)
        {
            foreach (Collider2D enemyCollider in hitEnemies)
            {
                Zombie z = enemyCollider.GetComponent<Zombie>();
                if (z != null)
                {
                    z.TakeDamage(damage);
                    z.GetPushedBack(pushDistance);
                }
            }

            nextReadyTime = Time.time + cooldown;
            if (CameraShake.Instance != null) CameraShake.Instance.Shake(0.4f, 0.4f);
            if (AudioManager.Instance != null) AudioManager.Instance.Play("ShockwavePulse");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector2 spawnPos = (Vector2)transform.position + boxOffset;
        Gizmos.DrawWireCube(spawnPos, boxSize);
    }
}