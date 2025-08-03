using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{

    public Vector2 parallaxFactor; // Changed to Vector2

    public void Move(Vector2 delta) // Now accepts Vector2
    {
        Vector3 newPos = transform.localPosition;
        newPos.x -= delta.x * parallaxFactor.x;
        newPos.y -= delta.y * parallaxFactor.y; // Added vertical movement
        
        transform.localPosition = newPos;
    }
}