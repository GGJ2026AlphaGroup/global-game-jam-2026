using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class FillSuspectInfo : MonoBehaviour
{
    public Character character;

    [Header("Suspect Fields")]
    public TextMeshProUGUI characterNameText;
    public TextMeshProUGUI maskGuessText;
    public TextMeshProUGUI clothesGuessText;
    public TextMeshProUGUI activityGuessText;
    public TextMeshProUGUI traitText;
    public Transform clueHolder;
    public GameObject cluePrefab;

    public TMP_Dropdown maskGuess;
    public TMP_Dropdown clothesGuess;
    public TMP_Dropdown activityGuess;

    void Start()
    {
        character.OnCharacterChanged += RebuildLayout;

        BuildLayout();
        RebuildLayout();
    }

    private void OnDestroy()
    {
        character.OnCharacterChanged -= RebuildLayout;
    }

    void BuildLayout()
    {
        if (clueHolder != null)
        {
            foreach (Clue clue in character.clues)
            {
                FillClueInfo newClue = Instantiate(cluePrefab, clueHolder).GetComponent<FillClueInfo>();

                newClue.clue = clue;
            }
        }

        if (maskGuess != null)
        {
            List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>() { new TMP_Dropdown.OptionData(Character.GetMaskDisplayName(Mask.None)) };
            int i = 1;
            foreach (Mask mask in PuzzleManager.Instance.GetAllActiveMasks())
            {
                options.Add(new TMP_Dropdown.OptionData(Character.GetMaskDisplayName(mask)));
                if (mask == character.guessedMask) maskGuess.value = i;
                i++;
            }
            maskGuess.options = options;
        }
        if (clothesGuess != null)
        {
            List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>() { new TMP_Dropdown.OptionData(Character.GetClothingDisplayName(Clothing.None)) };
            int i = 1;
            foreach (Clothing clothes in PuzzleManager.Instance.GetAllActiveClothings())
            {
                options.Add(new TMP_Dropdown.OptionData(Character.GetClothingDisplayName(clothes)));
                if (clothes == character.guessedClothing) clothesGuess.value = i;
                i++;
            }
            clothesGuess.options = options;
        }
        if (activityGuess != null)
        {
            List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>() { new TMP_Dropdown.OptionData(Character.GetActivityDisplayName(Activity.None)) };
            int i = 1;
            foreach (Activity activity in PuzzleManager.Instance.GetAllActiveActivities())
            {
                options.Add(new TMP_Dropdown.OptionData(Character.GetActivityDisplayName(activity)));
                if (activity == character.guessedActivity) activityGuess.value = i;
                i++;
            }
            activityGuess.options = options;
        }
    }

    public void RebuildLayout()
    {
        if (character == null) return;

        characterNameText.text = Character.GetNameDisplayName(character.name);
        if (maskGuessText != null) maskGuessText.text = Character.GetMaskDisplayName(character.guessedMask);
        if (clothesGuessText != null) clothesGuessText.text = Character.GetClothingDisplayName(character.guessedClothing);
        if (activityGuessText != null) activityGuessText.text = Character.GetActivityDisplayName(character.guessedActivity);

        if (traitText != null)
        {
            traitText.text = Character.GetTraitDisplayName(character.trait);
        }
    }

    public void SetGuess()
    {
        if (maskGuess.value == 0)
        {
            character.guessedMask = Mask.None;
        }
        else
        {
            character.guessedMask = PuzzleManager.Instance.GetAllActiveMasks()[maskGuess.value - 1];
        }

        if (clothesGuess.value == 0)
        {
            character.guessedClothing = Clothing.None;
        }
        else
        {
            character.guessedClothing = PuzzleManager.Instance.GetAllActiveClothings()[clothesGuess.value - 1];
        }

        if (activityGuess.value == 0)
        {
            character.guessedActivity = Activity.None;
        }
        else
        {
            character.guessedActivity = PuzzleManager.Instance.GetAllActiveActivities()[activityGuess.value - 1];
        }

        character.RegisterChange();
    }

    public void SpawnDetailsScreen()
    {
        WindowHolder.Instance.SpawnSuspectScreen(character, (Vector2)transform.position + new Vector2(450, 0));
    }
}
