public class NamedMaskClue : Clue
{
    Character subject;
    bool isNegated;
    Mask mask;

    public NamedMaskClue(Character subject, bool isNegated, bool isLie)
    {
        this.subject = subject;
        this.isNegated = isNegated;
        this.isLie = isLie;

        isAbsoloute = !isNegated;

        if ((!isNegated) ^ isLie)
        {
            mask = subject.mask;
        }
        else
        {
            mask = PuzzleManager.Instance.GetRandomActiveMask(subject.mask);
        }
    }

    public override bool IsConnectionValid(Character namedCharacter, Character propertiesCharacter)
    {
        // we are not checking this clue against the subject character
        if (namedCharacter != subject)
        {
            return true;
        }

        // if the mask matches
        if (propertiesCharacter.mask == mask)
        {
            return !isNegated;
        }

        return isNegated;
    }

    public override string ClueText { get { return $"{subject.name} is {(isNegated ? "not " : "")}wearing a {Character.GetMaskDisplayName(mask)} mask"; } }

    public override bool DoesReferenceCharacter(Character character)
    {
        return character == subject;
    }

    public override bool IsEqual(Clue clue)
    {
        if (clue is not NamedMaskClue)
        {
            return false;
        }

        NamedMaskClue typeClue = clue as NamedMaskClue;

        return typeClue.subject == subject && typeClue.isNegated == isNegated && typeClue.mask == mask && typeClue.isLie == isLie;
    }
}
