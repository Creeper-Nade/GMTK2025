using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomChanger : Singleton<RoomChanger>
{

    [SerializeField] private RoomBase _DefaultRoom;
    [SerializeField] private List<RoomBase> _RoomList;
    [SerializeField] private List<GameObject> _ButtonList;
    [SerializeField] private GameObject Inventory;
    [SerializeField] private GameObject potionSubmitPanel;



    //Screen slide variables
    [SerializeField] private Animator _ScreenSlideTransitAnimator;
    private int _SlideLeftHash = Animator.StringToHash("SlideLeft");
    private int _SlideRightHash = Animator.StringToHash("SlideRight");
    private int _SlideDownHash = Animator.StringToHash("SlideDown");
    private int _SlideUpHash = Animator.StringToHash("SlideUp");

    [SerializeField] private List<GameObject> DefaultRoomObjects;


    private RoomBase _currentRoom;
    private void Start()
    {
        //reset and init rooms
        _currentRoom = _DefaultRoom;
        foreach (RoomBase room in _RoomList)
        {
            room.gameObject.SetActive(false);
        }
        foreach (GameObject buttom in _ButtonList)
        {
            buttom.SetActive(false);
        }
        if (potionSubmitPanel != null)
            potionSubmitPanel.SetActive(false);

        _currentRoom.gameObject.SetActive(true);
        _currentRoom.init();
    }

    public void OnRightButtonClick()
    {
        if (_currentRoom.RightRoom != null)
            StartCoroutine(ExitRoom(_currentRoom.RightRoom, _SlideLeftHash));

    }
    public void OnLeftButtonClick()
    {
        if (_currentRoom.LeftRoom != null)
            StartCoroutine(ExitRoom(_currentRoom.LeftRoom, _SlideRightHash));
    }
    public void OnUpButtonClick()
    {
        if (_currentRoom.FrontRoom != null)
            StartCoroutine(ExitRoom(_currentRoom.FrontRoom, _SlideDownHash));
    }
    public void OnDownButtonClick()
    {
        if (_currentRoom.BackRoom != null)
            StartCoroutine(ExitRoom(_currentRoom.BackRoom, _SlideUpHash));
    }

    private IEnumerator ExitRoom(RoomBase targetRoom, int animHash)
    {
        _ScreenSlideTransitAnimator.SetTrigger(animHash);
        yield return new WaitForSeconds(0.17f);

        _currentRoom.gameObject.SetActive(false);
        Inventory.SetActive(false);
        foreach (GameObject obj in _ButtonList)
            obj.SetActive(false);

        // 隐藏 FrontRoom 专属物体
        foreach (GameObject obj in DefaultRoomObjects)
            obj.SetActive(false);

        _currentRoom = targetRoom;
        _currentRoom.gameObject.SetActive(true);

        // 如果是 FrontRoom，激活 FrontRoom 专属物体
        if (_currentRoom == _DefaultRoom)  
        {
            foreach (GameObject obj in DefaultRoomObjects)
                obj.SetActive(true);
        }

        // 如果是 SubmissionRoom，激活 SubmissionRoom 专属物体
        if (_currentRoom.name == "SubmissionRoom")
        {
            if (potionSubmitPanel != null)
                potionSubmitPanel.SetActive(true);
        }
        else
        {
            if (potionSubmitPanel != null)
                potionSubmitPanel.SetActive(false);
        }

        _currentRoom.init();


    }


}
