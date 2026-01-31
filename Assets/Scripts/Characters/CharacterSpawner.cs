using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    public GameObject characterPrefab;

    public Transform[] generalLocations;
    public Transform[] walkingLocations;

    public void SpawnCharacters(Character[] characters)
    {
        List<Transform> generalLocationsPool = new(generalLocations);
        List<Transform> walkingLocationsPool = new(walkingLocations);

        foreach (Character character in characters)
        {
            GameObject newCharacter = Instantiate(characterPrefab);

            Transform location;

            if (character.activity == Activity.Walking)
            {
                location = walkingLocationsPool[Random.Range(0, walkingLocationsPool.Count)];
            }
            else
            {
                location = generalLocationsPool[Random.Range(0, generalLocationsPool.Count)];
            }

            newCharacter.transform.position = location.position;

            CharacterController newController = newCharacter.GetComponent<CharacterController>();
            newController.character = character;
            newController.SetupVisual();
        }
    }
}
