using System;
using System.Collections.Generic;

public abstract class Clue
{
    public abstract string GetClueText(Character speaker);

    public Character owner;

    public bool isAbsoloute; // true if another clue of the same type can never give additional information when referencing the same character
    protected bool isLie;

    public abstract bool IsConnectionValid(Character namedCharacter, Character propertiesCharacter, List<Clue> existingClues);

    public abstract bool DoesReferenceCharacter(Character character);

    public abstract bool IsEqual(Clue clue);

    public bool IsPinned { get; private set; }

    public void SetIsPinned(bool isPinned)
    {
        if (IsPinned == isPinned) return;

        IsPinned = isPinned;
        OnClueChanged?.Invoke();
    }

    public event Action OnClueChanged;
}
