using UnityEngine;
using System.Collections.Generic;

public class NamedClothingClue : Clue
{
    public Character subject;
    public bool isNegated;
    public Clothing clothing;

    public NamedClothingClue(Character subject, bool isNegated, bool isLie)
    {
        this.subject = subject;
        this.isNegated = isNegated;
        isAbsoloute = !isNegated;
        this.isLie = isLie;

        if ((!isNegated) ^ isLie)
        {
            clothing = subject.clothing;
        }
        else
        {
            clothing = PuzzleManager.Instance.GetRandomActiveClothing(subject.clothing);
        }
    }

    public override bool IsConnectionValid(Character namedCharacter, Character propertiesCharacter, List<Clue> existingClues)
    {
        // we are not checking this clue against the subject character
        if (namedCharacter != subject)
        {
            return true;
        }

        // if the clothing matches
        if (propertiesCharacter.clothing == clothing)
        {
            return !isNegated;
        }

        return isNegated;
    }

    public override string ClueText { get { return $"{subject.name} is {(isNegated ? "not " : "")}wearing {Character.GetClothingDisplayName(clothing)}"; } }

    public override bool DoesReferenceCharacter(Character character)
    {
        return character == subject;
    }

    public override bool IsEqual(Clue clue)
    {
        if (clue is not NamedClothingClue)
        {
            return false;
        }

        NamedClothingClue typeClue = clue as NamedClothingClue;

        return typeClue.subject == subject && typeClue.isNegated == isNegated && typeClue.clothing == clothing && typeClue.isLie == isLie;
    }
}
