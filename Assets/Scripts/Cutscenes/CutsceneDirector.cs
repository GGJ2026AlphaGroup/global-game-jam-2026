using UnityEngine;

public class CutsceneDirector : MonoBehaviour
{
    public DummyDecorator starring;
    public DummyDecorator[] extras;
    public DummyDecorator theEvilVillain;

    public bool isFaceRevealed;

    public void GlamTheFuckUp(Character character)
    {
        if (theEvilVillain != null)
        {
            theEvilVillain.KillerDecorate(isFaceRevealed);
        }

        if (starring != null)
        {
            starring.Decorate(character, isFaceRevealed);
        }

        foreach (DummyDecorator extra in extras)
        {
            extra.RandomDecorate();
        }
    }
}
