using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameOverScreen : MonoBehaviour
{
#region Serialized Fields
    [SerializeField] private Image flashImage;
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private RunnerPlayer player;
    [SerializeField] private ObstacleSpawner spawner;
    [SerializeField] private GameObject gameplayRoot;

    [SerializeField, Min(0.01f)] private float flashDuration = 0.2f;
    [SerializeField, Min(0f)] private float textHoldDuration = 0.5f;
    [SerializeField, Min(0.01f)] private float textFadeDuration = 1f;
#endregion

#region Private Fields
    private bool playing;
#endregion

#region Game Over Logic
    public void Play()
    {
        if (playing)
        {
            return;
        }

        playing = true;

        spawner.CancelInvoke();
        gameplayRoot.SetActive(false);

        StartCoroutine(PlaySequence());
    }

    private IEnumerator PlaySequence()
    {
        gameOverText.gameObject.SetActive(false);
        flashImage.gameObject.SetActive(true);

        yield return FadeOut(flashImage, flashDuration);

        flashImage.gameObject.SetActive(false);

        gameOverText.alpha = 1f;
        gameOverText.gameObject.SetActive(true);

        yield return new WaitForSeconds(textHoldDuration);
        yield return FadeOut(gameOverText, textFadeDuration);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
#endregion

#region Fade Logic
    private IEnumerator FadeOut(Graphic graphic, float duration)
    {
        duration = Mathf.Max(0.01f, duration);

        Color color = graphic.color;
        float startAlpha = color.a;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float t = elapsed / duration;
            color.a = startAlpha * (1f - Ease.OutQuad(t));
            graphic.color = color;

            yield return null;
        }
    }
#endregion
}