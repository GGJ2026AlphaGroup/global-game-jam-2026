using UnityEngine;

public class AccusationOption : MonoBehaviour
{
    public Character character;

    public void Select()
    {
        PuzzleManager.Instance.Accuse(character);
    }
}
