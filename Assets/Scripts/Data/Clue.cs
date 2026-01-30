public abstract class Clue
{
    public abstract string ClueText { get; }

    public bool isAbsoloute; // true if another clue of the same type can never give additional information when referencing the same character
    protected bool isLie;

    public abstract bool IsConnectionValid(Character namedCharacter, Character propertiesCharacter);

    public abstract bool DoesReferenceCharacter(Character character);

    public abstract bool IsEqual(Clue clue);
}
