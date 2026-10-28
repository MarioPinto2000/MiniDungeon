using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public GameObject container; //pause UI
    public InputActionReference pauseMenuAction; //action that toggles pause "p"

    //subscribe to the input event when object becomes active
    private void OnEnable()
    {
        pauseMenuAction.action.Enable();
        pauseMenuAction.action.performed += OnPause;
    }

    //always unsubscribe to avoid duplicates callbacks
    private void OnDisable()
    {
        pauseMenuAction.action.performed -= OnPause;
        pauseMenuAction.action.Disable();
    }

    //toggle, when pressing the key again it resumes
    private void OnPause(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (GameOverUI.IsGameOver) return;

        if (container.activeSelf)
        {
            ResumeBt();
        }
        else
        {
            container.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    //public so it can be wired to the Resume UI button's OnClick event
    public void ResumeBt()
    {
        container.SetActive(false);
        Time.timeScale = 1;
    }
}
