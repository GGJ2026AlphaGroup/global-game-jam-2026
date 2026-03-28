using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FillSuspectInfo : MonoBehaviour
{
    public Character character;

    [Header("Suspect Fields")]
    public TextMeshProUGUI characterNameText;
    public TextMeshProUGUI maskGuessText;
    public TextMeshProUGUI clothesGuessText;
    public TextMeshProUGUI activityGuessText;
    public TextMeshProUGUI traitText;
    public TextMeshProUGUI personalityText;
    public TextMeshProUGUI personalityDescription;
    public Transform clueHolder;
    public GameObject cluePrefab;
    public HoverText traitHoverText;

    public TMP_Dropdown maskGuess;
    public TMP_Dropdown clothesGuess;
    public TMP_Dropdown activityGuess;

    public GameObject greenTab;
    public GameObject orangeTab;
    public GameObject redTab;

    public Image face;

    void Start()
    {
        character.OnCharacterChanged += RebuildLayout;

        RebuildLayout();
    }

    private void OnDestroy()
    {
        character.OnCharacterChanged -= RebuildLayout;
    }

    private List<FillClueInfo> clues = new();

    bool locked = true;

    public void RebuildLayout()
    {
        if (character == null) return;

        locked = true;

        face.sprite = Character.GetFaceSprite(character.name);

        characterNameText.text = Character.GetNameDisplayName(character.name);
        if (maskGuessText != null) maskGuessText.text = Character.GetMaskDisplayName(character.guessedMask);
        if (clothesGuessText != null) clothesGuessText.text = Character.GetClothingDisplayName(character.guessedClothing);
        if (activityGuessText != null) activityGuessText.text = Character.GetActivityDisplayName(character.guessedActivity);

        if (traitText != null)
        {
            traitText.text = Character.GetTraitDisplayName(character.trait);
        }

        if (personalityText != null)
        {
            personalityText.text = Character.GetPersonalityDisplayName(character.personality);
        }

        if (character.isRevealed)
        {
            if (maskGuess != null)
            {
                maskGuess.options = new List<TMP_Dropdown.OptionData>() { new TMP_Dropdown.OptionData(Character.GetMaskDisplayName(character.mask)) };
                maskGuess.value = 0;
            }
            if (clothesGuess != null)
            {
                clothesGuess.options = new List<TMP_Dropdown.OptionData>() { new TMP_Dropdown.OptionData(Character.GetClothingDisplayName(character.clothing)) };
                clothesGuess.value = 0;
            }
            if (activityGuess != null)
            {
                activityGuess.options = new List<TMP_Dropdown.OptionData>() { new TMP_Dropdown.OptionData(Character.GetActivityDisplayName(character.activity)) };
                activityGuess.value = 0;
            }
        }
        else
        {
            if (maskGuess != null)
            {
                List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>() { new TMP_Dropdown.OptionData(Character.GetMaskDisplayName(Mask.None)) };
                int i = 1;
                int j = 0;
                foreach (Mask mask in PuzzleManager.Instance.GetAllActiveMasks())
                {
                    options.Add(new TMP_Dropdown.OptionData(Character.GetMaskDisplayName(mask)));
                    if (mask == character.guessedMask) j = i;
                    i++;
                }
                maskGuess.options = options;
                maskGuess.value = j;
            }
            if (clothesGuess != null)
            {
                List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>() { new TMP_Dropdown.OptionData(Character.GetClothingDisplayName(Clothing.None)) };
                int i = 1;
                int j = 0;
                foreach (Clothing clothes in PuzzleManager.Instance.GetAllActiveClothings())
                {
                    options.Add(new TMP_Dropdown.OptionData(Character.GetClothingDisplayName(clothes)));
                    if (clothes == character.guessedClothing) j = i;
                    i++;
                }
                clothesGuess.options = options;
                clothesGuess.value = j;
            }
            if (activityGuess != null)
            {
                List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>() { new TMP_Dropdown.OptionData(Character.GetActivityDisplayName(Activity.None)) };
                int i = 1;
                int j = 0;
                foreach (Activity activity in PuzzleManager.Instance.GetAllActiveActivities())
                {
                    options.Add(new TMP_Dropdown.OptionData(Character.GetActivityDisplayName(activity)));
                    if (activity == character.guessedActivity) j = i;
                    i++;
                }
                activityGuess.options = options;
                activityGuess.value = j;
            }
        }

        if (traitHoverText != null) traitHoverText.text = character.trait switch
        {
            Trait.None => "There is nothing special about this character.",
            Trait.Honest => "Even if they are an accomplice or the\nkiller, this character's information is always true.",
            Trait.Confused => "This character's information is always incorrect.",
            Trait.Innocent => "This character is not the killer.",
            _ => "???"
        };
        if (personalityDescription != null)
        {
            if (character.questionClue != null)
            {
                personalityDescription.text = character.questionClue.GetClueText(character);
            }
            else personalityDescription.text = character.personality switch
            {
                Personality.Perceptive => "I can tell you what mask a suspect is wearing.",
                Personality.Fashionable => "I can tell you what colour clothes a suspect is wearing.",
                Personality.Socialite => "I can tell you what activity a suspect is doing.",
                Personality.Astute => "I can tell you if a suspect is lying.",
                _ => "???"
            };
        }

        greenTab.SetActive(character.isMarkedGreen);
        orangeTab.SetActive(character.isMarkedOrange);
        redTab.SetActive(character.isMarkedRed);

        if (clueHolder != null && clues.Count != character.clues.Count)
        {
            BuildClues();
        }

        locked = false;
    }

    void BuildClues()
    {
        foreach (FillClueInfo clue in clues)
        {
            Destroy(clue.gameObject);
        }

        clues.Clear();

        foreach (Clue clue in character.clues)
        {
            FillClueInfo newClue = Instantiate(cluePrefab, clueHolder).GetComponent<FillClueInfo>();

            newClue.clue = clue;

            clues.Add(newClue);
        }
    }

    public void SetGuess()
    {
        if (locked) return;

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
