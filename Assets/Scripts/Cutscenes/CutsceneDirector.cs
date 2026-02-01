using UnityEngine;

public class CutsceneDirector : MonoBehaviour
{
    public DummyDecorator starring;
    public DummyDecorator[] extras;
    public DummyDecorator theEvilVillain;

    public void GlamTheFuckUp(Character character)
    {
        if (theEvilVillain != null)
        {
            theEvilVillain.KillerDecorate();
        }

        if (starring != null)
        {
            starring.Decorate(character);
        }

        foreach (DummyDecorator extra in extras)
        {
            extra.RandomDecorate();
        }
    }
}
