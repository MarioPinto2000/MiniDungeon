using UnityEngine;
using UnityEngine.SceneManagement;

//shows game over when character dies
public class GameOverUI : MonoBehaviour
{
    public static bool IsGameOver { get; private set; }

    [SerializeField] private GameObject player;         //player
    [SerializeField] private GameObject gameOverPanel;  //game over ui panel

    private Character[] characters;

    private void Awake()
    {
        //reset every time the scene starts
        IsGameOver = false;
        gameOverPanel.SetActive(false);
        characters = player.GetComponents<Character>();
    }

    //player has both a Hero and a Mage, so listen to both
    //only the enabled one can take damage, but listening to both costs nothing
    private void OnEnable()
    {
        foreach (Character character in characters)
        {
            character.Died += OnPlayerDied;
        }
    }

    private void OnDisable()
    {
        foreach (Character character in characters)
        {
            if (character != null) character.Died -= OnPlayerDied;
        }
    }

    private void OnPlayerDied()
    {
        IsGameOver = true;
        gameOverPanel.SetActive(true);

        //freezes physics
        Time.timeScale = 0f;
    }

    public void RestartBt()
    {
        //restore time before loading, otherwise the new scene starts frozen
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
