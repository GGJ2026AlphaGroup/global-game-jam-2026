using UnityEngine;

public class AccusationScreenPopulator : MonoBehaviour
{
    public GameObject row;
    public GameObject option;

    private void Start()
    {
        Populate();
    }

    public void Populate()
    {
        Character[] characters = PuzzleManager.Instance.characters;
        int rows = Mathf.CeilToInt(characters.Length / 5f);

        Transform[] spawnedRows = new Transform[rows];

        for (int i = 0; i < rows; i++)
        {
            spawnedRows[i] = Instantiate(row, transform).transform;
        }

        for (int i = 0; i < characters.Length; i++)
        {
            Character character = characters[i];

            int row = Mathf.FloorToInt(i / 5f);

            FillSuspectInfo newOption = Instantiate(option, spawnedRows[row]).GetComponent<FillSuspectInfo>();
            newOption.character = character;
            newOption.GetComponent<AccusationOption>().character = character;
        }
    }
}
