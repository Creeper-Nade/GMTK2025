using UnityEngine;
using UnityEngine.UI;

public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance;

    public Image cursorImage;         // 拖入 UI Image
    public Vector2 offset = Vector2.zero;
    public float normalScale = 1f;    // 正常大小
    public float largeScale = 1.5f;   // 放大倍数

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        Cursor.visible = false;
    }

    void Start()
    {
        //Cursor.visible = false; // 隐藏系统鼠标
        SetCursorNormal();
    }

    void Update()
    {
        Vector2 mousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            cursorImage.canvas.transform as RectTransform,
            Input.mousePosition, cursorImage.canvas.worldCamera, out mousePos);

        cursorImage.rectTransform.anchoredPosition = mousePos + offset;
    }

    public void SetCursorNormal()
    {
        cursorImage.rectTransform.localScale = Vector3.one * normalScale;
    }

    public void SetCursorLarge()
    {
        cursorImage.rectTransform.localScale = Vector3.one * largeScale;
    }
}
