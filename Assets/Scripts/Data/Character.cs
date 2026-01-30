using UnityEngine;

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

public enum Colour
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

    public static string GetColourDisplayName(Colour colour)
    {
        switch (colour)
        {
            case Colour.None:
                return "???";
            case Colour.Red:
                return "Red";
            case Colour.Blue:
                return "Blue";
            case Colour.Green:
                return "Green";
            case Colour.Yellow:
                return "Yellow";
            case Colour.Orange:
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
}
