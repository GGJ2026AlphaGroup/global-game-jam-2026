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

    public Material[] faces;
    public Sprite[] faceSprites;

    public CharacterSpawner characterSpawner;
    public CutsceneManager cutsceneManager;

    public Character killer;

    public GameObject[] smallMaps;
    public GameObject[] mediumMaps;
    public GameObject[] largeMaps;

    public int guessesRemaining = 2;
    public int startGuesses = 2;

    protected override void Awake()
    {
        base.Awake();
        SetUpPuzzle(RunManager.Instance.level);
    }

    public void Accuse(Character character)
    {
        AccusationScreenController.Instance.CloseScreen();

        if (character.isKiller)
        {
            CutsceneManager.Instance.VictorySequence(character);
        }

        else if (guessesRemaining > 1)
        {
            CutsceneManager.Instance.PlayIncorrectGuessCutscene(character);
            guessesRemaining--;
        }

        else
        {
            CutsceneManager.Instance.DefeatSequence(character, killer);
        }
    }

    public void SetUpPuzzle(int difficulty)
    {
        int characterCount = Mathf.Clamp(difficulty + 2, 3, 15);

        if (characterCount <= 5)
        {
            Instantiate(smallMaps[Random.Range(0, smallMaps.Length)], Vector3.zero, Quaternion.Euler(0, 180, 0));
        }

        else if (characterCount <= 10)
        {
            Instantiate(mediumMaps[Random.Range(0, mediumMaps.Length)], Vector3.zero, Quaternion.Euler(0, 180, 0));
        }

        else
        {
            Instantiate(largeMaps[Random.Range(0, largeMaps.Length)], Vector3.zero, Quaternion.Euler(0, 180, 0));
        }

        startGuesses = Mathf.Clamp(Mathf.CeilToInt(characterCount / 4f), 2, 4);
        guessesRemaining = startGuesses;

        int liarCount = Mathf.CeilToInt(characterCount / 5f);
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

        if (difficulty > 1)
        {
            traitPool = new Trait[difficulty == 2 ? lastTrait - 1 : lastTrait];
            int j = 0;
            for (int i = 0; i < (difficulty == 2 ? traitPool.Length + 1 : traitPool.Length); i++)
            {
                if ((Trait)(i + 1) == Trait.Confused && difficulty == 2)
                {
                    j = -1;
                    continue;
                }

                traitPool[i + j] = (Trait)(i + 1 + j);
            }
        }
        else
        {
            traitPool = new Trait[0];
        }

        namePool = currentNames.ToArray();
        maskPool = currentMasks.ToArray();
        clothingPool = currentClothings.ToArray();
        activityPool = currentActivities.ToArray();

        characters = new PuzzleGenerator().GeneratePuzzle(characterCount, liarCount);

        characterSpawner.SpawnCharacters(characters);

        foreach (Character character in characters)
        {
            if (character.isKiller)
            {
                killer = character;
                cutsceneManager.PlayIntroCutscene(character);
                break;
            }
        }

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
        if (traitPool.Length == 0)
        {
            return Trait.Honest;
        }

        if (Random.value > 0.33f)
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
