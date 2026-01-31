using System;
using System.Collections.Generic;

public enum Name
{
    None,
    Kori,
    Wiktor,
    Toby,
    Jamie,
    Jonathan,
    Safiya,
    Max,
    Lewis,
    Abby,
    Alex,
    Katy,
    Phoebe,
    Jack,
    Ethan,
    Lenna,
    Hannah,

}

public enum Mask
{
    None,
    Bunny,
    Dog,
    Cat,
    Rat,
    Bird,
}

public enum Clothing
{
    None,
    Red,
    Blue,
    Green,
    Yellow,
    Orange,
}

public enum Activity
{
    None,
    Drinking,
    Walking,
    Smoking,
    Dancing,
    Talking,
}

public enum Trait
{
    None,
    Honest,
    Confused,
    Innocent,
    Perceptive,
    Fashionable,
    Socialite,
}

public class Character
{
    public Name name;
    public Name guessedName;

    public Mask mask;
    public Mask guessedMask;

    public Clothing clothing;
    public Clothing guessedClothing;

    public Activity activity;
    public Activity guessedActivity;

    public bool isKiller;
    public bool isLiar;

    public Trait trait;

    public List<Clue> clues = new();

    public int id;

    public event Action OnCharacterChanged;

    public void RegisterChange()
    {
        OnCharacterChanged?.Invoke();
    }

    public static string GetNameDisplayName(Name name)
    {
        switch (name)
        {
            case Name.None:
                return "???";
            case Name.Kori:
                return "Kori";
            case Name.Wiktor:
                return "Wiktor";
            case Name.Toby:
                return "Toby";
            case Name.Jamie:
                return "Jamie";
            case Name.Jonathan:
                return "Jonathan";
            case Name.Safiya:
                return "Safiya";
            case Name.Max:
                return "Max";
            case Name.Lewis:
                return "Lewis";
            case Name.Abby:
                return "Abby";
            case Name.Alex:
                return "Alex";
            case Name.Katy:
                return "Katy";
            case Name.Phoebe:
                return "Phoebe";
            case Name.Jack:
                return "Jack";
            case Name.Ethan:
                return "Ethan";
            case Name.Lenna:
                return "Lenna";
            case Name.Hannah:
                return "Hannah";
            default:
                return "???";
        }
    }

    public static string GetMaskDisplayName(Mask mask)
    {
        switch (mask)
        {
            case Mask.None:
                return "???";
            case Mask.Bunny:
                return "Bunny";
            case Mask.Dog:
                return "Dog";
            case Mask.Cat:
                return "Cat";
            case Mask.Rat:
                return "Rat";
            case Mask.Bird:
                return "Bird";
            default:
                return "???";
        }
    }

    public static string GetClothingDisplayName(Clothing colour)
    {
        switch (colour)
        {
            case Clothing.None:
                return "???";
            case Clothing.Red:
                return "Red";
            case Clothing.Blue:
                return "Blue";
            case Clothing.Green:
                return "Green";
            case Clothing.Yellow:
                return "Yellow";
            case Clothing.Orange:
                return "Orange";
            default:
                return "???";
        }
    }

    public static string GetActivityDisplayName(Activity activity)
    {
        switch (activity)
        {
            case Activity.None:
                return "???";
            case Activity.Drinking:
                return "Drinking";
            case Activity.Walking:
                return "Walking";
            case Activity.Smoking:
                return "Smoking";
            case Activity.Dancing:
                return "Dancing";
            case Activity.Talking:
                return "Talking";
            default:
                return "???";
        }
    }

    public static string GetTraitDisplayName(Trait trait)
    {
        switch (trait)
        {
            case Trait.None:
                return "Boring";
            case Trait.Honest:
                return "Honest";
            case Trait.Confused:
                return "Confused";
            case Trait.Innocent:
                return "Innocent";
            case Trait.Perceptive:
                return "Perceptive";
            case Trait.Fashionable:
                return "Fashionable";
            case Trait.Socialite:
                return "Socialite";
            default:
                return "Boring";
        }
    }
}