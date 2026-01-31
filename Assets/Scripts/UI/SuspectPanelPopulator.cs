using UnityEngine;

public class SuspectPanelPopulator : MonoBehaviour
{
    public GameObject suspectPrefab;

    public void Start()
    {
        foreach (Character character in PuzzleManager.Instance.characters)
        {
            FillSuspectInfo newSuspect = Instantiate(suspectPrefab, transform).GetComponent<FillSuspectInfo>();

            newSuspect.character = character;
        }
    }
}
