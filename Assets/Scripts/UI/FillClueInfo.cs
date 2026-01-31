using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FillClueInfo : MonoBehaviour
{
    public Clue clue;

    [Header("Clue Fields")]
    public TextMeshProUGUI clueText;
    public TextMeshProUGUI ownerText;
    public Image pinImage;

    public Sprite pinnedImage;
    public Sprite unpinnedImage;

    public PinnedClue pinnedClue;

    void Start()
    {
        clue.OnClueChanged += UpdateClue;
        UpdateClue();
    }

    private void OnDestroy()
    {
        clue.OnClueChanged -= UpdateClue;
    }

    public void UpdateClue()
    {
        if (clue == null) return;

        clueText.text = clue.GetClueText(clue.owner);

        if (ownerText != null)
        {
            ownerText.text = Character.GetNameDisplayName(clue.owner.name);
        }

        if (!clue.IsPinned)
        {
            if (pinnedClue != null)
            {
                Destroy(pinnedClue.gameObject);
            }
        }
    }

    public void TogglePinnedClue()
    {
        if (clue == null) return;

        clue.SetIsPinned(!clue.IsPinned);

        if (clue.IsPinned)
        {
            PinnedCluesManager.Instance.PinClue(clue);
        }
    }
}
