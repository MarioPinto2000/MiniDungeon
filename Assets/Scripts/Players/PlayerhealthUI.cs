using TMPro;
using UnityEngine;
using UnityEngine.UI;

//shows player HP bar
public class PlayerhealthUI : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Slider healthBar;
    [SerializeField] private TMP_Text healthText;

    private Character[] characters;

    private void Awake()
    {
        characters = player.GetComponents<Character>();
    }

    private void Update()
    {
        // player is destroyed when its HP reaches 0 so the references become null.
        if (player == null)
        {
            Show(0f, healthBar != null ? healthBar.maxValue : 0f);
            return;
        }

        foreach (Character character in characters)
        {
            if (character != null && character.enabled)
            {
                Show(character.CurrentHp, character.MaxHP);
                return;
            }
        }
    }

    private void Show(float current, float max)
    {
        current = Mathf.Max(0f, current); // HP can go below 0 on the last hit

        if (healthBar != null)
        {
            healthBar.maxValue = max;
            healthBar.value = current;
        }

        if (healthText != null)
        {
            healthText.text = $"HP {Mathf.CeilToInt(current)} / {Mathf.CeilToInt(max)}";
        }
    }
}
