using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomChanger : Singleton<RoomChanger>
{
    [SerializeField] private PlayerControl playerControl;
    [SerializeField] private RoomBase _DefaultRoom;
    [SerializeField] private List<RoomBase> _RoomList;
    [SerializeField] private List<GameObject> _ButtonList;
    [SerializeField] private GameObject Inventory;
    [SerializeField] private GameObject potionSubmitPanel;
    //[SerializeField] private GameObject orderPanel;

    //Screen slide variables
    [SerializeField] private Animator _ScreenSlideTransitAnimator;
    private int _SlideLeftHash = Animator.StringToHash("SlideLeft");
    private int _SlideRightHash = Animator.StringToHash("SlideRight");
    private int _SlideDownHash = Animator.StringToHash("SlideDown");
    private int _SlideUpHash = Animator.StringToHash("SlideUp");




    private RoomBase _currentRoom;
    private void Start()
    {
        StartCoroutine(InitCouroutine());
    }
    private IEnumerator InitCouroutine()
    {
        yield return null;
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
        potionSubmitPanel.SetActive(false);

        playerControl.maxDistance = 0.2f;
        playerControl.speed = 5;
        foreach (GameObject obj in _ButtonList)
            obj.SetActive(false);
            
        _currentRoom = targetRoom;
        //open submit panel if room is submission room; placeholder
        if (_currentRoom.BackRoom!=null)
        {
            if (potionSubmitPanel != null)
                potionSubmitPanel.SetActive(true);
        }
        //checks if current room is the warehouse.
        if (_currentRoom.FrontRoom == null && _currentRoom.LeftRoom == null && _currentRoom.BackRoom == null)
        {
            playerControl.maxDistance = 5;
            playerControl.speed = 10;
        }
        _currentRoom.gameObject.SetActive(true);


    }


}
