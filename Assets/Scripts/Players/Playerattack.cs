using UnityEngine;
using UnityEngine.InputSystem;

//bridges player input and the active class's attack.
//the Player has Hero and Mage components
public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private InputActionReference attackAction;

    private Character[] characters;

    private void Awake()
    {
        characters = GetComponents<Character>();
    }

    //IMPORTANT: subscribe in OnEnable and unsubscribe in OnDisable (same pathern for pause)
    private void OnEnable()
    {
        attackAction.action.Enable();
        attackAction.action.performed += OnAttack;
    }

    private void OnDisable()
    {
        attackAction.action.performed -= OnAttack;
        attackAction.action.Disable();
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        //input callbacks still fire while paused (timeScale = 0), so block attacks manually.
        if (Time.timeScale == 0f) return;

        foreach (Character character in characters)
        {
            if (character.enabled)
            {
                character.Attack();
                break; // Only one class is active at a time
            }
        }
    }
}