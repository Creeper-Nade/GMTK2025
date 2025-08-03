using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public Texture2D customCursorTexture;  // ‘⁄ Inspector ÷–Õœ»Î Sprite.texture
    public Vector2 hotSpot = Vector2.zero;

    void Start()
    {
        Cursor.SetCursor(customCursorTexture, hotSpot, CursorMode.Auto);
        Cursor.visible = true;
    }
}
