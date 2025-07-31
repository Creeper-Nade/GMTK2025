using System.Collections.Generic;
using UnityEngine;

public class RoomBase : MonoBehaviour
{
    public RoomBase FrontRoom;
    public RoomBase BackRoom;
    public RoomBase LeftRoom;
    public RoomBase RightRoom;

    [SerializeField] private List<GameObject> DirectionButtons;
    private void OnEnable()
    {
        init();
    }
    public void init()
    {
        //enable all the directional buttons needed this scene
        foreach (GameObject gameObject in DirectionButtons)
        {
            gameObject.SetActive(true);
        }
    }
}
