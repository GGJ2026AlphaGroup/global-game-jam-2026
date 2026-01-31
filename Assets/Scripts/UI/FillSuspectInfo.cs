using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class FillSuspectInfo : MonoBehaviour
{
    public Character character;

    [Header("Suspect Fields")]
    public TextMeshProUGUI characterNameText;
    public TextMeshProUGUI maskGuessText;
    public TextMeshProUGUI clothesGuessText;
    public TextMeshProUGUI activityGuessText;
    public TextMeshProUGUI traitText;
    public Image suspectFaceImage;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        character = new Character()
        {
            name = Name.Kori,
            mask = Mask.Rat,
            clothing = Clothing.Green,
            activity = Activity.Dancing,
            trait = Trait.Innocent,
        };

        characterNameText.text = Character.GetNameDisplayName(character.name);
        RebuildLayout();
    }

    public void RebuildLayout()
    {
        if (character == null) return;

        characterNameText.text = Character.GetNameDisplayName(character.name);
        maskGuessText.text = Character.GetMaskDisplayName(character.mask);
        clothesGuessText.text = Character.GetClothingDisplayName(character.clothing);
        activityGuessText.text = Character.GetActivityDisplayName(character.activity);
        traitText.text = Character.GetTraitDisplayName(character.trait);
    }
}
