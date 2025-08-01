using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{

    public void OnLookInput(InputAction.CallbackContext context)
    {
        Debug.Log("moving");
    }
}
