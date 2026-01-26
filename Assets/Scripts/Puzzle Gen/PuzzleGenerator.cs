using System.Collections.Generic;
using UnityEngine;

public class PuzzleGenerator
{
    private static readonly string[] characterNames = new string[] { "Sam", "Ben", "James", "Lenna", "Luke", "Hannah" };
    private static readonly string[] clothes = new string[] { "red", "green", "blue" };
    private static readonly string[] masks = new string[] { "fox", "wolf", "dragon" };
    private static readonly string[] actions = new string[] { "talking", "smoking", "drinking" };

    private class Character
    {
        public int id;
        public string name;
        public int clothing;
        public int mask;
        public int action;

        public bool isKiller;

        public string GetPropertiesDescription()
        {
            return $"character {id} is wearing {clothes[clothing]} and a {masks[mask]} mask, and is {actions[action]}.{(isKiller ? " This is the killer!" : "")}";
        }
    }

    private abstract class Clue
    {
        public abstract string ClueText { get; }

        public abstract bool IsConnectionValid(Character namedCharacter, Character propertiesCharacter);
    }

    class NamedClothingClue : Clue
    {
        Character subject;
        bool isNegated;
        int clothing;

        public NamedClothingClue(Character subject, bool isNegated)
        {
            this.subject = subject;
            this.isNegated = isNegated;
            if (!isNegated)
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
    }

    class NamedMaskClue : Clue
    {
        Character subject;
        bool isNegated;
        int mask;

        public NamedMaskClue(Character subject, bool isNegated)
        {
            this.subject = subject;
            this.isNegated = isNegated;

            if (!isNegated)
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
    }

    class NamedActionClue : Clue
    {
        Character subject;
        bool isNegated;
        int action;

        public NamedActionClue(Character subject, bool isNegated)
        {
            this.subject = subject;
            this.isNegated = isNegated;
            if (!isNegated)
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
    }

    public void GeneratePuzzle(int characterCount, int liarCount)
    {
        Character[] characters = new Character[characterCount];
        List<Clue> clues = new List<Clue>();

        void AddNewClue(Character subject, bool subjectIsNamedSubject) // if false, subject is properties subject
        {
            switch (Random.Range(0, 3))
            {
                case 0:
                    clues.Add(new NamedClothingClue(subject, UnityEngine.Random.value > 0.5f));
                    break;
                case 1:
                    clues.Add(new NamedMaskClue(subject, UnityEngine.Random.value > 0.5f));
                    break;
                case 2:
                    clues.Add(new NamedActionClue(subject, UnityEngine.Random.value > 0.5f));
                    break;
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
                bool characterAsNamedCharacter = true;

                foreach (Character namedCharacter in characters)
                {
                    if (namedPossibilities[namedCharacter].Count > highestPossibilityCount)
                    {
                        highestPossibilityCount = namedPossibilities[namedCharacter].Count;
                        highestPossibilityCharacter = namedCharacter;
                        characterAsNamedCharacter = true;
                    }
                }

                foreach (Character propertiesCharacter in characters)
                {
                    if (propertiesPossibilities[propertiesCharacter].Count > highestPossibilityCount)
                    {
                        highestPossibilityCount = propertiesPossibilities[propertiesCharacter].Count;
                        highestPossibilityCharacter = propertiesCharacter;
                        characterAsNamedCharacter = false;
                    }
                }

                AddNewClue(highestPossibilityCharacter, characterAsNamedCharacter);
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

            highestOrderCharacter.isKiller = true;
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
                    if (characters[j].clothing == clothesID &&
                           characters[j].mask == maskID &&
                           characters[j].action == actionID)
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
            AddNewClue(characters[i], true);
        }

        // create solvable puzzle

        VerifyPuzzle();

        // add liars

        // assign clues to characters

        // generate lying clues for liars

        // add abilities

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
