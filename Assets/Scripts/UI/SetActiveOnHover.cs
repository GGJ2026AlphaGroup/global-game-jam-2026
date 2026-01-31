using UnityEngine;

public class SetActiveOnHover : Hoverable
{
    public GameObject objectToActivate;

    public override void SetHovered(bool isHovered)
    {
        objectToActivate.SetActive(isHovered);
    }
}
