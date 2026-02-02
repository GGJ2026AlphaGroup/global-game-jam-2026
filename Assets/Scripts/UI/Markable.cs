using UnityEngine;

public class Markable : Hoverable
{
    public FillSuspectInfo target;

    private void Update()
    {
        if (!isHovered) return;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            target.character.isMarkedGreen = !target.character.isMarkedGreen;
            target.character.RegisterChange();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            target.character.isMarkedOrange = !target.character.isMarkedOrange;
            target.character.RegisterChange();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            target.character.isMarkedRed = !target.character.isMarkedRed;
            target.character.RegisterChange();
        }
    }
}
