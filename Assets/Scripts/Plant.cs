using UnityEngine;

public class Plant : AbstractInteractables
{

    public override void OnInteraction()
    {
        Debug.Log("You've got me!");
    }
}
