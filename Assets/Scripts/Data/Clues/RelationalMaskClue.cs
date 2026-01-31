using System.Collections.Generic;
using UnityEngine.UI;

public class RelationalMaskClue : Clue
{
    Character subject1;
    Character subject2;
    bool isNegated;

    public RelationalMaskClue(Character subject1, Character subject2, bool isLie)
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
            if (clue is not NamedMaskClue)
            {
                continue;
            }

            if (clue.DoesReferenceCharacter(unnamedCharacter))
            {
                // if the mask matches

                if (!(clue as NamedMaskClue).isNegated)
                {
                    if (propertiesCharacter.mask == unnamedCharacter.mask)
                    {
                        return !isNegated;
                    }

                    return isNegated;
                }
                else
                {
                    if (propertiesCharacter.mask != (clue as NamedMaskClue).mask)
                    {
                        return !isNegated;
                    }

                    return isNegated;
                }
            }
        }

        return true;
    }


    public override string GetClueText(Character speaker)
    {
        return $"{(subject1 == speaker ? "I am" : Character.GetNameDisplayName(subject1.name) + " is")} {(isNegated ? "not " : "")}wearing the same mask as {(subject2 == speaker ? "me" : Character.GetNameDisplayName(subject2.name))}";
    }

    public override bool DoesReferenceCharacter(Character character)
    {
        return character == subject1 || character == subject2;
    }

    public override bool IsEqual(Clue clue)
    {
        if (clue is not RelationalMaskClue)
        {
            return false;
        }

        RelationalMaskClue typeClue = clue as RelationalMaskClue;

        return typeClue.subject1 == subject1 && typeClue.subject2 == subject2 && typeClue.isNegated == isNegated && typeClue.isLie == isLie;
    }
}
