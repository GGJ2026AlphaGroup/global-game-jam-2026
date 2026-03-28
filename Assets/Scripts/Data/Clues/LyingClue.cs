using System.Collections.Generic;
using UnityEngine.UI;

public class LyingClue : Clue
{
    Character subject;
    bool isNegated;

    public LyingClue(Character subject)
    {
        this.subject = subject;

        isAbsoloute = false;

        isNegated = !subject.isLiar;
    }

    public override bool IsConnectionValid(Character namedCharacter, Character propertiesCharacter, List<Clue> existingClues)
    {
        return true;
    }

    public override string GetClueText(Character speaker)
    {
        return $"{(subject == speaker ? "I am" : Character.GetNameDisplayName(subject.name) + " is")} {(isNegated ? "telling the truth" : "lying")}";
    }

    public override bool DoesReferenceCharacter(Character character)
    {
        return character == subject;
    }

    public override bool IsEqual(Clue clue)
    {
        if (clue is not LyingClue)
        {
            return false;
        }

        return clue.DoesReferenceCharacter(subject) && ((LyingClue)clue).isNegated == isNegated;
    }
}
