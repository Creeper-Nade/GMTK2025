using System.Collections;
using UnityEngine;

public class MiscIngredients : AbstractInteractables, IHauntAction
{
    public GameObject GameObject => gameObject;
    private bool is_haunted=false;
    public bool Is_Haunted => is_haunted;
    private enum Haunt_Types
    {
        none,
        vibrate,
        WrongDisplay
    }
    private Haunt_Types _haunt_Type;

    private void OnEnable() {
        if (is_haunted && _haunt_Type==Haunt_Types.vibrate)
        {
            StartCoroutine(Vibrate());
        }
    }

    private IEnumerator Vibrate()
    {
        float speed = 10.0f;
        float intensity = 0.1f;
        float timer = 0f;
        float MaximumTime = 0.2f;
        while (timer < MaximumTime)
        {
            timer += Time.deltaTime;
            transform.localPosition = intensity * new Vector2(
                Mathf.PerlinNoise(speed * Time.time, 1),
                Mathf.PerlinNoise(speed * Time.time, 2)
            );
            yield return null;
        }
    }

    public override void OnInteraction()
    {
        if (_haunt_Type == Haunt_Types.WrongDisplay)
        {
            Debug.Log("Give item with its properties messed up");
        }
        else
        {
            Debug.Log("give normal misc item");
        }    
        //implement logic of giving
    }

    public void ExitHaunt()
    {
        is_haunted = false;
        _haunt_Type = Haunt_Types.none;
    }

    public void Haunt()
    {
        int index = Random.Range(0, 2);
        Debug.Log("Haunt ingredients");
        //int index = 0;
        is_haunted = true;
        //Debug.Log(gameObject + "is ahunted");
        switch (index)
        {
            case 0:
                _haunt_Type = Haunt_Types.vibrate;
                break;
            case 1:
                //Debug.Log("Species change");
                _haunt_Type = Haunt_Types.WrongDisplay;
                
                break;

        }
    }

    

}
