public class NamedActionClue : Clue
{
    Character subject;
    bool isNegated;
    Activity activity;

    public NamedActionClue(Character subject, bool isNegated, bool isLie)
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

    public override bool IsConnectionValid(Character namedCharacter, Character propertiesCharacter)
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

    public override string ClueText { get { return $"{subject.name} is {(isNegated ? "not " : "")}{Character.GetActivityDisplayName(activity)}"; } }

    public override bool DoesReferenceCharacter(Character character)
    {
        return character == subject;
    }

    public override bool IsEqual(Clue clue)
    {
        if (clue is not NamedActionClue)
        {
            return false;
        }

        NamedActionClue typeClue = clue as NamedActionClue;

        return typeClue.subject == subject && typeClue.isNegated == isNegated && typeClue.activity == activity && typeClue.isLie == isLie;
    }
}
