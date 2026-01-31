using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public Character character;

    public GameObject selectionImage;

    private void Start()
    {
        selectionImage.SetActive(false);
    }

    public void SetupVisual()
    {

    }

    public void SetHovered(bool isHovered)
    {
        selectionImage.SetActive(isHovered);
    }
}
