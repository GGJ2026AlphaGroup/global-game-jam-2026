using NUnit.Framework.Internal.Commands;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class PuzzleGenerator
{
    private static readonly string[] characterNames = new string[] { "Sam", "Ben", "James", "Lenna", "Luke", "Hannah" };
    private static readonly string[] clothes = new string[] { "red", "green", "blue" };
    private static readonly string[] masks = new string[] { "fox", "wolf", "dragon" };
    private static readonly string[] actions = new string[] { "talking", "smoking", "drinking" };
    private static readonly string[] traits = new string[] { "honest", "confused", "innocent" };

    private class Character
    {
        public int id;
        public string name;
        public int clothing;
        public int mask;
        public int action;
        public bool isKiller;
        public bool isLying;

        public string GetPropertiesDescription()
        {
            return $"character {id} is wearing {clothes[clothing]} and a {masks[mask]} mask, and is {actions[action]}.{(isKiller ? " This is the killer!" : "")}";
        }

        public string trait;

        public List<Clue> clues;
    }

    private abstract class Clue
    {
        public abstract string ClueText { get; }

        public bool isAbsoloute; // true if another clue of the same type can never give additional information when referencing the same character

        public abstract bool IsConnectionValid(Character namedCharacter, Character propertiesCharacter);

        public abstract bool DoesReferenceCharacter(Character character);

        public abstract bool IsEqual(Clue clue);
    }

    class NamedClothingClue : Clue
    {
        Character subject;
        bool isNegated;
        int clothing;

        public NamedClothingClue(Character subject, bool isNegated, bool isLie)
        {
            this.subject = subject;
            this.isNegated = isNegated;
            isAbsoloute = !isNegated;
            if ((!isNegated) ^ isLie)
            {
                clothing = subject.clothing;
            }
            else
            {
                clothing = (subject.clothing + Random.Range(1, clothes.Length)) % clothes.Length; ;
            }
        }

        public override bool IsConnectionValid(Character namedCharacter, Character propertiesCharacter)
        {
            // we are not checking this clue against the subject character
            if (namedCharacter != subject)
            {
                return true;
            }

            // if the clothing matches
            if (propertiesCharacter.clothing == clothing)
            {
                return !isNegated;
            }

            return isNegated;
        }

        public override string ClueText { get { return $"{subject.name} is {(isNegated ? "not " : "")}wearing {clothes[clothing]}"; } }

        public override bool DoesReferenceCharacter(Character character)
        {
            return character == subject;
        }

        public override bool IsEqual(Clue clue)
        {
            if (clue is not NamedClothingClue)
            {
                return false;
            }

            NamedClothingClue typeClue = clue as NamedClothingClue;

            return typeClue.subject == subject && typeClue.isNegated == isNegated && typeClue.clothing == clothing;
        }
    }

    class NamedMaskClue : Clue
    {
        Character subject;
        bool isNegated;
        int mask;

        public NamedMaskClue(Character subject, bool isNegated, bool isLie)
        {
            this.subject = subject;
            this.isNegated = isNegated;
            isAbsoloute = !isNegated;

            if ((!isNegated) ^ isLie)
            {
                mask = subject.mask;
            }
            else
            {
                mask = (subject.mask + Random.Range(1, masks.Length)) % masks.Length;
            }
        }

        public override bool IsConnectionValid(Character namedCharacter, Character propertiesCharacter)
        {
            // we are not checking this clue against the subject character
            if (namedCharacter != subject)
            {
                return true;
            }

            // if the mask matches
            if (propertiesCharacter.mask == mask)
            {
                return !isNegated;
            }

            return isNegated;
        }

        public override string ClueText { get { return $"{subject.name} is {(isNegated ? "not " : "")}wearing a {masks[mask]} mask"; } }

        public override bool DoesReferenceCharacter(Character character)
        {
            return character == subject;
        }

        public override bool IsEqual(Clue clue)
        {
            if (clue is not NamedMaskClue)
            {
                return false;
            }

            NamedMaskClue typeClue = clue as NamedMaskClue;

            return typeClue.subject == subject && typeClue.isNegated == isNegated && typeClue.mask == mask;
        }
    }

    class NamedActionClue : Clue
    {
        Character subject;
        bool isNegated;
        int action;

        public NamedActionClue(Character subject, bool isNegated, bool isLie)
        {
            this.subject = subject;
            this.isNegated = isNegated;
            isAbsoloute = !isNegated;

            if ((!isNegated) ^ isLie)
            {
                action = subject.action;
            }
            else
            {
                action = (subject.action + Random.Range(1, actions.Length)) % actions.Length;
            }
        }

        public override bool IsConnectionValid(Character namedCharacter, Character propertiesCharacter)
        {
            // we are not checking this clue against the subject character
            if (namedCharacter != subject)
            {
                return true;
            }

            // if the action matches
            if (propertiesCharacter.action == action)
            {
                return !isNegated;
            }

            return isNegated;
        }

        public override string ClueText { get { return $"{subject.name} is {(isNegated ? "not " : "")}{actions[action]}"; } }

        public override bool DoesReferenceCharacter(Character character)
        {
            return character == subject;
        }

        public override bool IsEqual(Clue clue)
        {
            if (clue is not NamedActionClue)
            {
                return false;
            }

            NamedActionClue typeClue = clue as NamedActionClue;

            return typeClue.subject == subject && typeClue.isNegated == isNegated && typeClue.action == action;
        }
    }

    delegate Clue ClueSpawner();

    public void GeneratePuzzle(int characterCount, int liarCount)
    {
        Character[] characters = new Character[characterCount];
        List<Clue> clues = new List<Clue>();

        Character killer = null;

        Clue DrawRandomClue(Character subject, bool isLie)
        {
            ClueSpawner[] potentialClues = new ClueSpawner[]
            {
                () => new NamedClothingClue(subject, Random.value > 0.5f, isLie),
                () => new NamedMaskClue(subject, Random.value > 0.5f, isLie),
                () => new NamedActionClue(subject, Random.value > 0.5f, isLie),
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
                else if (existingClue is NamedActionClue && existingClue.DoesReferenceCharacter(subject))
                {
                    clueTypeCount[2]++;
                    if (existingClue.isAbsoloute)
                    {
                        clueTypeCount[2] += 1000;
                    }
                }
            }

            // spawn a random clue of a type with the lowest count
            int lowestCount = int.MaxValue;

            foreach (int count in clueTypeCount)
            {
                lowestCount = Mathf.Min(lowestCount, count);
            }

            List<ClueSpawner> cluesToDraw = new List<ClueSpawner();

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

                        bool isValid = clue.IsConnectionValid(namedCharacter, propertiesCharacter);

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

            Debug.Log($"Puzzle is of order {highestOrder}");

            return;
        }

        // create characters

        List<int> namePool = new List<int>();

        for (int i = 0; i < characterNames.Length; i++)
        {
            namePool.Add(i);
        }

        List<int> clothesPool = new List<int>();
        List<int> maskPool = new List<int>();
        List<int> actionPool = new List<int>();

        void PopulatePools()
        {
            clothesPool.Clear();
            maskPool.Clear();
            actionPool.Clear();

            for (int i = 0; i < clothes.Length; i++)
            {
                for (int j = 0; j < Mathf.Max(1, Mathf.FloorToInt(1.5f * characterCount / clothes.Length)); j++)
                {
                    clothesPool.Add(i);
                }
            }

            for (int i = 0; i < masks.Length; i++)
            {
                for (int j = 0; j < Mathf.Max(1, Mathf.FloorToInt(1.5f * characterCount / masks.Length)); j++)
                {
                    maskPool.Add(i);
                }
            }

            for (int i = 0; i < actions.Length; i++)
            {
                for (int j = 0; j < Mathf.Max(1, Mathf.FloorToInt(1.5f * characterCount / actions.Length)); j++)
                {
                    actionPool.Add(i);
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
                           characters[j].action == actionPool[actionID])
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
                name = characterNames[namePool[nameID]],
                clothing = clothesPool[clothesID],
                mask = maskPool[maskID],
                action = actionPool[actionID],
                isKiller = false
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

        // add traits

        foreach (Character character in characters)
        {
            if (Random.value < 0.15f)
            {
                character.trait = traits[Random.Range(0, traits.Length)];
            }
        }

        if (liarCount > 0)
        {
            killer.isLying = true;
            liarCount--;
        }

        // add liars
        int failures = 0;

        while (liarCount > 0)
        {
            if (failures > 30)
            {
                continue;
            }

            Character randomCharacter = characters[Random.Range(0, characters.Length)];

            if (randomCharacter.isLying)
            {
                failures++;
                continue;
            }

            randomCharacter.isLying = true;
            liarCount--;
        }

        // assign clues to characters
        List<Clue> unclaimedClues = new List<Clue>();
        foreach (Clue clue in clues)
        {
            unclaimedClues.Add(clue);
        }

        foreach (Character character in characters)
        {
            if (character.isLying)
            {
                continue;
            }

            foreach (Clue clue in unclaimedClues)
            {
                if (clue.DoesReferenceCharacter(character))
                {
                    character.clues.Add(clue);
                    unclaimedClues.Remove(clue);
                    continue;
                }
            }
        }

        int characterID = 0;
        int liesCount = 1;

        foreach (Clue clue in clues)
        {
            while (characters[characterID].isLying)
            {
                characterID++;
                if (characterID >= characters.Length)
                {
                    characterID = 0;
                    liesCount++;
                }
            }
            
            characters[characterID].clues.Add(clue);
            unclaimedClues.Remove(clue);

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
            if (!character.isLying)
            {
                continue;
            }

            for (int i = 0; i < liesCount; i++)
            {
                Clue newClue = DrawRandomClue(character, true);
                character.clues.Add(newClue);
                clues.Add(newClue);
            }
        }

        // final puzzle is ready

        for (int i = 0; i < characterCount; i++)
        {
            Debug.Log(characters[i].GetPropertiesDescription());
        }

        for (int i = 0; i < characterCount; i++)
        {
            Debug.Log(characters[i].name);
        }

        foreach (Clue clue in clues)
        {
            Debug.Log(clue.ClueText);
        }
    }
}
