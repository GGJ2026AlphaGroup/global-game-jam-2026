using System.Collections.Generic;
using UnityEngine;

public class QTargetScreenPopulator : MonoBehaviour
{
    public GameObject row;
    public GameObject option;

    Transform[] spawnedRows;

    public void Populate()
    {
        if (spawnedRows != null)
        {
            foreach (Transform t in spawnedRows)
            {
                Destroy(t.gameObject);
            }
        }

        List<Character> characters = new(PuzzleManager.Instance.characters);

        for (int i = characters.Count - 1; i >= 0; i--)
        {
            if (characters[i] == QTargetScreenController.Instance.subject)
            {
                characters.RemoveAt(i);
            }
        }

        int rows = Mathf.CeilToInt(characters.Count / 5f);

        spawnedRows = new Transform[rows];

        for (int i = 0; i < rows; i++)
        {
            spawnedRows[i] = Instantiate(row, transform).transform;
        }

        for (int i = 0; i < characters.Count; i++)
        {
            Character character = characters[i];

            int row = Mathf.FloorToInt(i / 5f);

            FillSuspectInfo newOption = Instantiate(option, spawnedRows[row]).GetComponent<FillSuspectInfo>();
            newOption.character = character;
            newOption.GetComponent<QTargetOption>().character = character;
        }
    }
}
