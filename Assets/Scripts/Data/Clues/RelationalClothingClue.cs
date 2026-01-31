using System.Collections.Generic;

public class RelationalClothingClue : Clue
{
    Character subject1;
    Character subject2;
    bool isNegated;
    Clothing clothing;

    public RelationalClothingClue(Character subject1, Character subject2, bool isLie)
    {
        this.subject1 = subject1;
        this.subject2 = subject2;
        this.isLie = isLie;

        isAbsoloute = false;

        if (subject1.mask == subject2.mask)
        {
            isNegated = isLie;
        }
        else
        {
            isNegated = !isLie;
        }
    }

    public override bool IsConnectionValid(Character namedCharacter, Character propertiesCharacter, List<Clue> existingClues)
    {
        if (namedCharacter == propertiesCharacter)
        {
            // obviously true
            return true;
        }

        if (namedCharacter != subject1 && namedCharacter != subject2)
        {
            // we do not care
            return true;
        }

        if ((namedCharacter == subject1 && propertiesCharacter == subject2) || (namedCharacter == subject2 && propertiesCharacter == subject1))
        {
            // definitionally true by the setup of the clue
            return true;
        }

        Character unnamedCharacter;
        if (namedCharacter == subject1)
        {
            unnamedCharacter = subject2;
        }
        else
        {
            unnamedCharacter = subject1;
        }

        foreach (Clue clue in existingClues)
        {
            if (clue is not NamedClothingClue)
            {
                continue;
            }

            if (clue.DoesReferenceCharacter(unnamedCharacter))
            {
                // if the clothing matches

                if (!(clue as NamedClothingClue).isNegated)
                {
                    if (propertiesCharacter.clothing == unnamedCharacter.clothing)
                    {
                        return !isNegated;
                    }

                    return isNegated;
                }
                else
                {
                    if (propertiesCharacter.clothing != (clue as NamedClothingClue).clothing)
                    {
                        return !isNegated;
                    }

                    return isNegated;
                }
            }
        }

        return true;
    }

    public override string ClueText { get { return $"{subject1.name} is {(isNegated ? "not " : "")}wearing the same colour clothes as {subject2.name}"; } }

    public override bool DoesReferenceCharacter(Character character)
    {
        return character == subject1 || character == subject2;
    }

    public override bool IsEqual(Clue clue)
    {
        if (clue is not RelationalClothingClue)
        {
            return false;
        }

        RelationalClothingClue typeClue = clue as RelationalClothingClue;

        return typeClue.subject1 == subject1 && typeClue.subject2 == subject2 && typeClue.isNegated == isNegated && typeClue.isLie == isLie;
    }
}
