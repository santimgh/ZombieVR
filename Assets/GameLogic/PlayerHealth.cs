using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int health = 2; // Player can take 2 hits before dying
    public Image hitEffect; // UI Image for red screen effect
    public GameObject deathScreen; // UI Death Screen

    private bool isDead = false;

    void Start()
    {
        // Ensure the hit effect is invisible at the start
        if (hitEffect != null)
        {
            hitEffect.color = new Color(hitEffect.color.r, hitEffect.color.g, hitEffect.color.b, 0f);
        }

        // Hide death screen at the start
        if (deathScreen != null)
        {
            deathScreen.SetActive(false);
        }
    }

    public void TakeDamage()
    {
        if (isDead) return; // Ignore damage if already dead

        health--;

        // Show red hit effect
        if (hitEffect != null)
        {
            StartCoroutine(FadeHitEffect());
        }

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        Time.timeScale = 0; // Freeze the game
        if (deathScreen != null)
        {
            deathScreen.SetActive(true);
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1; // Resume game
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Restart the current scene
    }

    IEnumerator FadeHitEffect()
    {
        if (hitEffect == null) yield break;

        // Set alpha to 30% (show the red effect)
        hitEffect.color = new Color(hitEffect.color.r, hitEffect.color.g, hitEffect.color.b, 0.3f);

        yield return new WaitForSecondsRealtime(0.5f); // Use unscaled time so it works even when the game is paused

        // Smoothly fade out over 0.5 seconds
        float duration = 0.5f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime; // Use unscaled time to work even when Time.timeScale = 0
            float alpha = Mathf.Lerp(0.3f, 0f, elapsedTime / duration); // Gradually decrease alpha
            hitEffect.color = new Color(hitEffect.color.r, hitEffect.color.g, hitEffect.color.b, alpha);
            yield return null; // Wait for the next frame
        }

        // Ensure it fully disappears
        hitEffect.color = new Color(hitEffect.color.r, hitEffect.color.g, hitEffect.color.b, 0f);
    }

}
