using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public GameObject container;
    public InputActionReference pauseMenuAction;

    private void OnEnable()
    {
        pauseMenuAction.action.Enable();
        pauseMenuAction.action.performed += OnPause;
    }

    private void OnDisable()
    {
        pauseMenuAction.action.performed -= OnPause;
        pauseMenuAction.action.Disable();
    }

    private void OnPause(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
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

    public void ResumeBt()
    {
        container.SetActive(false);
        Time.timeScale = 1;
    }
}
