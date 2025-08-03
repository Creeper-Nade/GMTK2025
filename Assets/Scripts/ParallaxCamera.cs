using UnityEngine;

[ExecuteInEditMode]
public class ParallaxCamera : Singleton<ParallaxCamera>
{
    //parallax variables
    public delegate void ParallaxCameraDelegate(Vector2 deltaMovement); // Changed to Vector2
    public ParallaxCameraDelegate onCameraTranslate;
    [SerializeField] private Camera MainCamera;
    private Vector2 oldPosition;

    private void Start() {
        oldPosition = MainCamera.transform.position;
    }
    private void Update() {
        
        Vector2 currentpos = MainCamera.transform.position;
         if (currentpos != oldPosition) // Check both axes
        {
            if (onCameraTranslate != null)
            {
                Vector2 delta = new Vector2(
                    oldPosition.x - currentpos.x,
                    oldPosition.y - currentpos.y // Added vertical delta
                );
                onCameraTranslate(delta);
            }
            oldPosition = currentpos;
        }
    }


}
