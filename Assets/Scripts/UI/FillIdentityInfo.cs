using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FillIdentityInfo : MonoBehaviour
{
    public Character character;

    [Header("Identity Fields")]
    public TextMeshProUGUI characterNameGuessText;
    public TextMeshProUGUI maskText;
    public TextMeshProUGUI clothesText;
    public TextMeshProUGUI activityText;

    public TMP_Dropdown guess;
    
    void Start()
    {
        BuildLayout();
        RebuildLayout();
    }

    public void BuildLayout()
    {
        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>() { new TMP_Dropdown.OptionData(Character.GetNameDisplayName(Name.None)) };
        int i = 1;
        foreach (Name name in PuzzleManager.Instance.GetAllActiveNames())
        {
            options.Add(new TMP_Dropdown.OptionData(Character.GetNameDisplayName(name)));
            if (name == character.guessedName) guess.value = i;
            i++;
        }
        guess.options = options;
    }

    public void RebuildLayout()
    {
        if (character == null) return;

        characterNameGuessText.text = Character.GetNameDisplayName(character.guessedName);
        maskText.text = Character.GetMaskDisplayName(character.mask);
        clothesText.text = Character.GetClothingDisplayName(character.clothing);
        activityText.text = Character.GetActivityDisplayName(character.activity);
    }

    public void SetGuess()
    {
        if (guess.value == 0)
        {
            character.guessedName = Name.None;
        }
        else
        {
            character.guessedName = PuzzleManager.Instance.GetAllActiveNames()[guess.value - 1];
        }
    }
}
