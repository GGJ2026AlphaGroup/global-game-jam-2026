using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PuzzleManager : MonoSingleton<PuzzleManager>
{
    public Character[] characters;

    public Name[] namePool;
    public Mask[] maskPool;
    public Clothing[] clothingPool;
    public Activity[] activityPool;
    public Trait[] traitPool;

    protected override void Awake()
    {
        base.Awake();
        SetUpPuzzle(1);
    }

    public void SetUpPuzzle(int difficulty)
    {
        int characterCount = Mathf.Clamp(difficulty + 2, 3, 15);
        int liarCount = characterCount / 5;
        int propertyCount = Mathf.Min(Mathf.Max(characterCount / 2, 2), 5);

        int lastName = Name.GetValues(typeof(Name)).Cast<int>().Last();
        int lastMask = Mask.GetValues(typeof(Mask)).Cast<int>().Last();
        int lastClothes = Clothing.GetValues(typeof(Clothing)).Cast<int>().Last();
        int lastActivity = Activity.GetValues(typeof(Activity)).Cast<int>().Last();
        int lastTrait = Trait.GetValues(typeof(Trait)).Cast<int>().Last();

        List<Name> currentNames = new List<Name>();

        for (int i = 0; i < characterCount; i++)
        {
            Name newName = Name.None;
            while (newName == Name.None || currentNames.Contains(newName))
            {
                newName = (Name)Random.Range(1, lastName + 1);
            }

            currentNames.Add(newName);
        }

        List<Mask> currentMasks = new List<Mask>();
        List<Clothing> currentClothings = new List<Clothing>();
        List<Activity> currentActivities = new List<Activity>();

        for (int i = 0; i < propertyCount; i++)
        {
            Mask newMask = Mask.None;
            Clothing newClothing = Clothing.None;
            Activity newActivity = Activity.None;

            while (newMask == Mask.None || currentMasks.Contains(newMask))
            {
                newMask = (Mask)Random.Range(1, lastMask + 1);
            }
            while (newClothing == Clothing.None || currentClothings.Contains(newClothing))
            {
                newClothing = (Clothing)Random.Range(1, lastClothes + 1);
            }
            while (newActivity == Activity.None || currentActivities.Contains(newActivity))
            {
                newActivity = (Activity)Random.Range(1, lastActivity + 1);
            }

            currentMasks.Add(newMask);
            currentClothings.Add(newClothing);
            currentActivities.Add(newActivity);
        }

        traitPool = new Trait[lastTrait];
        for (int i = 0; i < traitPool.Length; i++)
        {
            traitPool[i] = (Trait)(i + 1);
        }

        namePool = currentNames.ToArray();
        maskPool = currentMasks.ToArray();
        clothingPool = currentClothings.ToArray();
        activityPool = currentActivities.ToArray();

        characters = new PuzzleGenerator().GeneratePuzzle(characterCount, 1);
    }

    public Name GetRandomActiveName(Name excluding = Name.None)
    {
        Name currentName = Name.None;
        while (currentName == Name.None || currentName == excluding)
        {
            currentName = namePool[Random.Range(0, namePool.Length)];
        }

        return currentName;
    }

    public Mask GetRandomActiveMask(Mask excluding = Mask.None)
    {
        Mask currentMask = Mask.None;
        while (currentMask == Mask.None || currentMask == excluding)
        {
            currentMask = maskPool[Random.Range(0, maskPool.Length)];
        }

        return currentMask;
    }

    public Clothing GetRandomActiveClothing(Clothing excluding = Clothing.None)
    {
        Clothing currentClothing = Clothing.None;
        while (currentClothing == Clothing.None || currentClothing == excluding)
        {
            currentClothing = clothingPool[Random.Range(0, clothingPool.Length)];
        }

        return currentClothing;
    }

    public Activity GetRandomActiveActivity(Activity excluding = Activity.None)
    {
        Activity currentActivity = Activity.None;
        while (currentActivity == Activity.None || currentActivity == excluding)
        {
            currentActivity = activityPool[Random.Range(0, activityPool.Length)];
        }

        return currentActivity;
    }

    public Trait GetRandomActiveTrait()
    {
        if (Random.value > 0.75f)
        {
            return Trait.None;
        }

        if (traitPool.Length == 0)
        {
            return Trait.None;
        }

        Trait trait = traitPool[Random.Range(0, traitPool.Length)];

        return trait;
    }

    public Name[] GetAllActiveNames() 
    { 
        return namePool;
    }

    public Mask[] GetAllActiveMasks()
    {
        return maskPool;
    }

    public Clothing[] GetAllActiveClothings()
    {
        return clothingPool;
    }

    public Activity[] GetAllActiveActivities()
    {
        return activityPool;
    }
}
