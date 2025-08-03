using TMPro;
using UnityEngine;

public class ClockController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;


    // Update is called once per frame
    void Update()
    {
         float gameTime = GlobalDataManager.Instance._RoundTimer / 3f;
    
        // Calculate minutes and seconds from scaled game time
        int minutes = Mathf.FloorToInt(gameTime / 60);
        int seconds = Mathf.FloorToInt(gameTime % 60);
        timerText.text = string.Format("{0:00}:{1:00} AM", minutes, seconds);
    }
}
