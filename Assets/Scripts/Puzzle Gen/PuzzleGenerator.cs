using System.Collections.Generic;
using UnityEngine;

public class PuzzleGenerator
{
    delegate Clue ClueSpawner();

    public Character[] GeneratePuzzle(int characterCount, int liarCount)
    {
        Character[] characters = new Character[characterCount];
        List<Clue> clues = new List<Clue>();

        Character killer = null;

        Character GetRandomCharacter(Character excluding)
        {
            Character currentCharacter = null;
            while (currentCharacter == null || currentCharacter == excluding)
            {
                currentCharacter = characters[Random.Range(0, characters.Length)];
            }

            return currentCharacter;
        }

        Clue DrawRandomClue(Character subject, bool isLie)
        {
            ClueSpawner[] potentialClues = new ClueSpawner[]
            {
                () => new NamedClothingClue(subject, Random.value > 0.5f, isLie),
                () => new NamedMaskClue(subject, Random.value > 0.5f, isLie),
                () => new NamedActivityClue(subject, Random.value > 0.5f, isLie),
                () => new RelationalClothingClue(subject, GetRandomCharacter(subject), isLie),
                () => new RelationalMaskClue(subject, GetRandomCharacter(subject), isLie),
                () => new RelationalActivityClue(subject, GetRandomCharacter(subject), isLie),
            };

            int[] clueTypeCount = new int[potentialClues.Length];

            // count how many clues of each type already reference this character
            foreach (Clue existingClue in clues)
            {
                if (existingClue is NamedClothingClue && existingClue.DoesReferenceCharacter(subject))
                {
                    clueTypeCount[0]++;
                    if (existingClue.isAbsoloute)
                    {
                        clueTypeCount[0] += 1000;
                    }
                }
                else if (existingClue is NamedMaskClue && existingClue.DoesReferenceCharacter(subject))
                {
                    clueTypeCount[1]++;
                    if (existingClue.isAbsoloute)
                    {
                        clueTypeCount[1] += 1000;
                    }
                }
                else if (existingClue is NamedActivityClue && existingClue.DoesReferenceCharacter(subject))
                {
                    clueTypeCount[2]++;
                    if (existingClue.isAbsoloute)
                    {
                        clueTypeCount[2] += 1000;
                    }
                }
                else if (existingClue is RelationalClothingClue && existingClue.DoesReferenceCharacter(subject))
                {
                    clueTypeCount[3]++;
                }
                else if (existingClue is RelationalMaskClue && existingClue.DoesReferenceCharacter(subject))
                {
                    clueTypeCount[4]++;
                }
                else if (existingClue is RelationalActivityClue && existingClue.DoesReferenceCharacter(subject))
                {
                    clueTypeCount[5]++;
                }
            }

            // spawn a random clue of a type with the lowest count
            int lowestCount = int.MaxValue;

            foreach (int count in clueTypeCount)
            {
                lowestCount = Mathf.Min(lowestCount, count);
            }

            List<ClueSpawner> cluesToDraw = new List<ClueSpawner>();

            for (int i = 0; i < clueTypeCount.Length; i++)
            {
                if (clueTypeCount[i] == lowestCount)
                {
                    cluesToDraw.Add(potentialClues[i]);
                }
            }

            int iterations = 0;

        redrawClue:
            Clue drawnClue = cluesToDraw[Random.Range(0, cluesToDraw.Count)]();

            iterations++;

            if (iterations > 20)
            {
                goto skip;
            }

            foreach (Clue clue in clues)
            {
                if (clue.IsEqual(drawnClue))
                {
                    goto redrawClue;
                }
            }

        skip:
            return drawnClue;
        }

        void AddNewClue(Character subject)
        {
            Clue drawnClue = DrawRandomClue(subject, false);

            clues.Add(drawnClue);

            // if it's an absoloute clue, remove any previous clues of the same type referencing the same character (as they will now be redundant)
            if (drawnClue.isAbsoloute)
            {
                for (int i = 0; i < clues.Count - 1; i++)
                {
                    Clue clue = clues[i];
                    
                    if (clue != drawnClue && clue.DoesReferenceCharacter(subject) && clue.GetType() == drawnClue.GetType())
                    {
                        clues.RemoveAt(i);
                        i--;
                    }
                }
            }
        }

        void VerifyPuzzle()
        {
            Dictionary<Character, List<Character>> namedPossibilities = new Dictionary<Character, List<Character>>(); // key is named character, value is list of possible property characters
            Dictionary<Character, List<Character>> propertiesPossibilities = new Dictionary<Character, List<Character>>(); // key is properties character, value is list of possible named characters

            // populate possibility dicts

            foreach (Character A in characters)
            {
                namedPossibilities[A] = new List<Character>();
                propertiesPossibilities[A] = new List<Character>();
                foreach (Character B in characters)
                {
                    namedPossibilities[A].Add(B);
                    propertiesPossibilities[A].Add(B);
                }
            }

            // eliminate first-order impossibilities

            foreach (Clue clue in clues)
            {
                foreach (Character namedCharacter in characters)
                {
                    for (int i = 0; i < namedPossibilities[namedCharacter].Count; i++)
                    {
                        Character propertiesCharacter = namedPossibilities[namedCharacter][i];

                        bool isValid = clue.IsConnectionValid(namedCharacter, propertiesCharacter, clues);

                        // this connection is impossible
                        if (!isValid)
                        {
                            namedPossibilities[namedCharacter].Remove(propertiesCharacter);
                            propertiesPossibilities[propertiesCharacter].Remove(namedCharacter);
                            i--;
                        }
                    }
                }
            }

            // have we made any deductions?

            int[] deductions = new int[characterCount]; // index is character id, value is the order of deduction (0 is not deduced).
            int currentOrder = 1;
            bool moreDeductionsToEliminate = true;

            while (moreDeductionsToEliminate)
            {
                moreDeductionsToEliminate = false;

                // find new deductions
                for (int i = 0; i < characterCount; i++)
                {
                    if (deductions[i] > 0)
                    {
                        continue; // already deduced
                    }

                    Character character = characters[i];

                    if (namedPossibilities[character].Count == 1 || propertiesPossibilities[character].Count == 1)
                    {
                        deductions[i] = currentOrder;
                        moreDeductionsToEliminate = true;
                    }
                }

                currentOrder++;

                if (!moreDeductionsToEliminate)
                {
                    break;
                }

                // eliminate deduced possibilities
                foreach (Character namedCharacter in characters)
                {
                    for (int i = 0; i < namedPossibilities[namedCharacter].Count; i++)
                    {
                        Character propertiesCharacter = namedPossibilities[namedCharacter][i];

                        if (deductions[propertiesCharacter.id] > 0)
                        {
                            namedPossibilities[namedCharacter].Remove(propertiesCharacter);
                            propertiesPossibilities[propertiesCharacter].Remove(namedCharacter);
                            i--;
                        }
                    }
                }
            }

            // check if puzzle has been solved
            bool puzzleValid = true;

            for (int i = 0; i < characterCount; i++)
            {
                if (deductions[i] == 0)
                {
                    puzzleValid = false;
                }
            }

            if (!puzzleValid)
            {
                // find character with highest possibility count to add a clue for
                int highestPossibilityCount = 0;
                Character highestPossibilityCharacter = characters[0];

                foreach (Character namedCharacter in characters)
                {
                    if (namedPossibilities[namedCharacter].Count > highestPossibilityCount)
                    {
                        highestPossibilityCount = namedPossibilities[namedCharacter].Count;
                        highestPossibilityCharacter = namedCharacter;
                    }
                }

                foreach (Character propertiesCharacter in characters)
                {
                    if (propertiesPossibilities[propertiesCharacter].Count > highestPossibilityCount)
                    {
                        highestPossibilityCount = propertiesPossibilities[propertiesCharacter].Count;
                        highestPossibilityCharacter = propertiesCharacter;
                    }
                }

                AddNewClue(highestPossibilityCharacter);

                namedPossibilities.Clear();
                propertiesPossibilities.Clear();

                VerifyPuzzle();
                return;
            }

            // set the highest order deduction character as the killer

            int highestOrder = 0;
            Character highestOrderCharacter = characters[0];

            for (int i = 0; i < characterCount; i++)
            {
                if (deductions[i] > highestOrder)
                {
                    highestOrder = deductions[i];
                    highestOrderCharacter = characters[i];
                }
            }

            killer = highestOrderCharacter;
            highestOrderCharacter.isKiller = true;

            return;
        }

        // create characters

        List<Name> namePool = new List<Name>(PuzzleManager.Instance.GetAllActiveNames());

        List<Clothing> clothesPool = new List<Clothing>();
        List<Mask> maskPool = new List<Mask>();
        List<Activity> actionPool = new List<Activity>();

        void PopulatePools()
        {
            clothesPool.Clear();
            maskPool.Clear();
            actionPool.Clear();

            Clothing[] allClothes = PuzzleManager.Instance.GetAllActiveClothings();
            Mask[] allMasks = PuzzleManager.Instance.GetAllActiveMasks();
            Activity[] allActivities = PuzzleManager.Instance.GetAllActiveActivities();

            for (int i = 0; i < allClothes.Length; i++)
            {
                for (int j = 0; j < Mathf.Max(1, Mathf.CeilToInt(1.5f * characterCount / allClothes.Length)); j++)
                {
                    clothesPool.Add(allClothes[i]);
                }
            }

            for (int i = 0; i < allMasks.Length; i++)
            {
                for (int j = 0; j < Mathf.Max(1, Mathf.CeilToInt(1.5f * characterCount / allMasks.Length)); j++)
                {
                    maskPool.Add(allMasks[i]);
                }
            }

            for (int i = 0; i < allActivities.Length; i++)
            {
                for (int j = 0; j < Mathf.Max(1, Mathf.CeilToInt(1.5f * characterCount / allActivities.Length)); j++)
                {
                    actionPool.Add(allActivities[i]);
                }
            }
        }
    AbortGeneration:
        PopulatePools();

        for (int i = 0; i < characterCount; i++)
        {
            int nameID = Random.Range(0, namePool.Count);
            int clothesID = 0;
            int maskID = 0;
            int actionID = 0;

            bool needsRerolling = true;

            int rerollAttempts = 0;

            while (needsRerolling)
            {
                if (rerollAttempts == 10)
                {
                    // repopulate pools and start over
                    PopulatePools();
                }
                if (rerollAttempts > 100)
                {
                    // something is very wrong, abort
                    Debug.Log("Character generation restarted: too many reroll attempts.");
                    goto AbortGeneration;
                }

                clothesID = Random.Range(0, clothesPool.Count);
                maskID = Random.Range(0, maskPool.Count);
                actionID = Random.Range(0, actionPool.Count);

                needsRerolling = false;

                for (int j = 0; j < i; j++)
                {
                    // ensure no duplicate characters
                    if (characters[j].clothing == clothesPool[clothesID] &&
                           characters[j].mask == maskPool[maskID] &&
                           characters[j].activity == actionPool[actionID])
                    {
                        needsRerolling = true;
                        rerollAttempts++;
                        break;
                    }
                }
            }

            characters[i] = new Character()
            {
                id = i,
                name = namePool[nameID],
                clothing = clothesPool[clothesID],
                mask = maskPool[maskID],
                activity = actionPool[actionID],
                outfitType = Random.Range(0, 2)
            };

            namePool.RemoveAt(nameID);
            clothesPool.RemoveAt(clothesID);
            maskPool.RemoveAt(maskID);
            actionPool.RemoveAt(actionID);
        }

        // add initial clues

        for (int i = 0; i < characterCount; i++)
        {
            AddNewClue(characters[i]);
        }

        // create solvable puzzle

        VerifyPuzzle();

        // shift IDs

        foreach (Character character in characters)
        {
            if (character.isKiller)
            {
                character.id = 0;
                break;
            }

            character.id++;
        }

        // add traits

        bool addedConfused = false;

        foreach (Character character in characters)
        {
            character.trait = PuzzleManager.Instance.GetRandomActiveTrait();

            while ((character.trait == Trait.Innocent && character.isKiller) || (addedConfused && character.trait == Trait.Confused))
            {
                character.trait = PuzzleManager.Instance.GetRandomActiveTrait();
            }

            if (character.trait == Trait.Confused)
            {
                addedConfused = true;
            }
        }

        if (liarCount > 0)
        {
            killer.isLiar = true;
            killer.isAccomplice = true;
            liarCount--;
        }

        // add accomplices
        int failures = 0;

        while (liarCount > 0)
        {
            if (failures > 30)
            {
                continue;
            }

            Character randomCharacter = characters[Random.Range(0, characters.Length)];

            if (randomCharacter.isAccomplice)
            {
                failures++;
                continue;
            }

            randomCharacter.isLiar = true;
            randomCharacter.isAccomplice = true;
            liarCount--;
        }

        foreach (Character character in characters)
        {
            if (character.trait == Trait.Honest)
            {
                character.isLiar = false;
            }
            if (character.trait == Trait.Confused)
            {
                character.isLiar = true;
            }
        }

        // assign clues to characters
        List<Clue> unclaimedClues = new List<Clue>();
        foreach (Clue clue in clues)
        {
            unclaimedClues.Add(clue);
        }

        // grab initial clue
        foreach (Character character in characters)
        {
            if (character.isLiar)
            {
                continue;
            }

            foreach (Clue clue in unclaimedClues)
            {
                if (clue.DoesReferenceCharacter(character))
                {
                    character.clues.Add(clue);
                    clue.owner = character;
                    unclaimedClues.Remove(clue);
                    break;
                }
            }
        }

        int characterID = 0;
        int liesCount = 1;

        // assign remaining clues
        while (unclaimedClues.Count > 0)
        {
            while (characters[characterID].isLiar)
            {
                characterID++;
                if (characterID >= characters.Length)
                {
                    characterID = 0;
                    liesCount++;
                }
            }
            Clue clue = unclaimedClues[0];
            
            characters[characterID].clues.Add(clue);
            clue.owner = characters[characterID];
            unclaimedClues.RemoveAt(0);

            characterID++;
            if (characterID >= characters.Length)
            {
                characterID = 0;
                liesCount++;
            }
        }

        // generate lying clues for liars

        foreach (Character character in characters)
        {
            if (!character.isLiar)
            {
                continue;
            }

            for (int i = 0; i < liesCount; i++)
            {
                Clue newClue = DrawRandomClue(character, true);
                newClue.owner = character;
                character.clues.Add(newClue);
                clues.Add(newClue);
            }
        }

        return characters;
    }
}
