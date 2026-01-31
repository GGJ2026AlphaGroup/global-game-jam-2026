using TMPro;
using UnityEngine;

public class FillClueInfo : MonoBehaviour
{
    public Clue clue;
    public Character character;

    [Header("Suspect Fields")]
    public TextMeshProUGUI clueText;

    void Awake()
    {
        character = new Character();

        clue = new NamedClothingClue(character, false, false);
        {
            
        }

        UpdateClue();
    }
    void UpdateClue()
    {
        if (clue == null) return;

        clueText.text = clue.ClueText;
    }
}
