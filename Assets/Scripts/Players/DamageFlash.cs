using UnityEngine;
using System.Collections;

//makes a sprite blink in a flash colour when it takes damage.
public class DamageFlash : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color flashColor = Color.red; //red by default
    [SerializeField, Min(0.01f)] private float flashDuration = 0.15f; //flash time
    [SerializeField, Min(0.01f)] private float blinkInterval = 0.08f; //time between flashs

    private Color originalColor;
    private Coroutine routine;

    private void Awake()
    {
        if (spriteRenderer == null) spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        originalColor = spriteRenderer.color; //sprite returns to ITS colour
    }

    public void Flash()
    {
        Flash(flashDuration);
    }

    //player passes its invincibility time, so it blinks for exactly as long as it cannot be hurt.
    public void Flash(float duration)
    {
        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(FlashRoutine(Mathf.Max(duration, blinkInterval)));
    }

    private IEnumerator FlashRoutine(float duration)
    {
        float elapsed = 0f;
        bool showFlashColor = true;

        while (elapsed < duration)
        {
            spriteRenderer.color = showFlashColor ? flashColor : originalColor;
            showFlashColor = !showFlashColor;

            yield return new WaitForSeconds(blinkInterval);
            elapsed += blinkInterval;
        }

        spriteRenderer.color = originalColor;
        routine = null;
    }

    //never leave the sprite tinted if the object is disabled in the middle of a blink.
    private void OnDisable()
    {
        if (spriteRenderer != null) spriteRenderer.color = originalColor;
        routine = null;
    }

}
