using UnityEngine;

public enum Name
{
    None,
    Kori,
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

public enum Colour
{

}

public enum Activity
{

}

public class Character
{
    public Name name;
    public Name guessedName;

    public Mask mask;
    public Mask guessedMask;

    public Colour colour;
    public Colour guessedColour;

    public Activity activity;
    public Activity guessedActivity;

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

    public static string GetColourDisplayName(Mask mask)
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

    public static string GetColourDisplayName(Colour colour)
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
}
