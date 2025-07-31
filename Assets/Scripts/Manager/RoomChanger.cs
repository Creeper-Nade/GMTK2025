using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomChanger : Singleton<RoomChanger>
{

    [SerializeField] private RoomBase _DefaultRoom;
    [SerializeField] private List<RoomBase> _RoomList;
    [SerializeField] private List<GameObject> _ButtonList;


    //Screen slide variables
    [SerializeField] private Animator _ScreenSlideTransitAnimator;
    private int _SlideLeftHash = Animator.StringToHash("SlideLeft");
    private int _SlideRightHash = Animator.StringToHash("SlideRight");
    private int _SlideDownHash = Animator.StringToHash("SlideDown");
    private int _SlideUpHash = Animator.StringToHash("SlideUp");

    private RoomBase _currentRoom;
    protected override void Awake()
    {
        base.Awake();
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

    private IEnumerator ExitRoom(RoomBase TargetRoom, int AnimHash)
    {
        _ScreenSlideTransitAnimator.SetTrigger(AnimHash);
        yield return new WaitForSeconds(0.1f);
        
        _currentRoom.gameObject.SetActive(false);
        foreach (GameObject obj in _ButtonList)
            obj.SetActive(false);

        _currentRoom = TargetRoom;
        _currentRoom.gameObject.SetActive(true);
    }

}
