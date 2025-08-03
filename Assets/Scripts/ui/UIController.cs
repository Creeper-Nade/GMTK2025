using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    private int _ExitHash = Animator.StringToHash("Exit");
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void OnRegress()
    {
        StartCoroutine(RegressCoroutine());
    }
    private IEnumerator RegressCoroutine()
    {
        animator.SetTrigger(_ExitHash);
        yield return new WaitForSecondsRealtime(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void OnBreak()
    {
        StartCoroutine(BreakCoroutine());
    }
    private IEnumerator BreakCoroutine()
    {
        animator.SetTrigger(_ExitHash);
        yield return new WaitForSecondsRealtime(2);
        Debug.Log("To Title Screen");
    }
}
