using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RunnerHealth : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private GameOverScreen gameOverScreen;
    [SerializeField] private Image damageFlash;
    [SerializeField, Min(0.01f)] private float damageFlashDuration = 0.15f;

    private float damageFlashElapsed;
    private bool dead;
    private int health = 100;
    private float regenerationTimer;

    private void Start()
    {
        RefreshText();
    }

    private void Update()
    {
        if (dead) return;

        UpdateDamageFlash();
        regenerationTimer += Time.deltaTime;

        while (regenerationTimer >= 1f)
        {
            regenerationTimer -= 1f;

            if (health < 100)
            {
                health++;
                RefreshText();
            }
        }
    }

    public void TakeDamage()
    {
        if (dead) return;

        health = Mathf.Max(0, health - 5);
        RefreshText();

        if (health == 0)
        {
            dead = true;
            damageFlash.gameObject.SetActive(false);
            gameOverScreen.Play();
            return;
        }

        damageFlashElapsed = 0f;

        Color color = damageFlash.color;
        color.a = 0.5f;
        damageFlash.color = color;

        damageFlash.gameObject.SetActive(true);
    }
    
    private void RefreshText()
    {
        healthText.text = $"HP: {health}/100";
    }

    private void UpdateDamageFlash()
    {
        if (!damageFlash.gameObject.activeSelf)
        {
            return;
        }

        damageFlashElapsed += Time.deltaTime;

        float t = damageFlashElapsed / Mathf.Max(0.01f, damageFlashDuration);

        Color color = damageFlash.color;
        color.a = 0.5f * (1f - Ease.OutQuad(t));
        damageFlash.color = color;

        if (t >= 1f)
        {
            damageFlash.gameObject.SetActive(false);
        }
    }
}