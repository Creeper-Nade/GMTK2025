using UnityEngine;

public interface IHauntAction
{
    public void Haunt();
    public void ExitHaunt();
    public GameObject GameObject { get; }
    public bool Is_Haunted{ get; }
}
