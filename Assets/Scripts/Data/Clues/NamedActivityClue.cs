using System.Collections.Generic;
using Unity.VisualScripting;

public class NamedActivityClue : Clue
{
    public Character subject;
    public bool isNegated;
    public Activity activity;

    public NamedActivityClue(Character subject, bool isNegated, bool isLie)
    {
        this.subject = subject;
        this.isNegated = isNegated;
        isAbsoloute = !isNegated;
        this.isLie = isLie;

        if ((!isNegated) ^ isLie)
        {
            activity = subject.activity;
        }
        else
        {
            activity = PuzzleManager.Instance.GetRandomActiveActivity(subject.activity);
        }
    }

    public override bool IsConnectionValid(Character namedCharacter, Character propertiesCharacter, List<Clue> existingClues)
    {
        // we are not checking this clue against the subject character
        if (namedCharacter != subject)
        {
            return true;
        }

        // if the action matches
        if (propertiesCharacter.activity == activity)
        {
            return !isNegated;
        }

        return isNegated;
    }

    public override string GetClueText(Character speaker)
    {
        return $"{(subject == speaker ? "I am" : Character.GetNameDisplayName(subject.name) + " is")} {(isNegated ? "not " : "")}{Character.GetActivityDisplayName(activity)}";
    }

    public override bool DoesReferenceCharacter(Character character)
    {
        return character == subject;
    }

    public override bool IsEqual(Clue clue)
    {
        if (clue is not NamedActivityClue)
        {
            return false;
        }

        NamedActivityClue typeClue = clue as NamedActivityClue;

        return typeClue.subject == subject && typeClue.isNegated == isNegated && typeClue.activity == activity && typeClue.isLie == isLie;
    }
}
