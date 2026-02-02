using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    public GameObject characterPrefab;

    public void SpawnCharacters(Character[] characters)
    {
        List<Transform> takenLocations = new();

        List<GameObject> collection = new(GameObject.FindGameObjectsWithTag("Spawner"));

        foreach (Character character in characters)
        {
            GameObject newCharacter = Instantiate(characterPrefab);

            Transform location = null;
            int i = 0;
            bool isLocationValid = false;
            while (location == null || takenLocations.Contains(location) || !isLocationValid)
            {
                i++;
                location = collection[Random.Range(0, collection.Count)].transform;

                // bit of social distancing mate
                isLocationValid = !Physics.CheckSphere(location.position, 0.75f);

                if (i > 50)
                {
                    Debug.LogWarning("Could not find valid spawn location for character: " + character.id);
                    break;
                }
            }

            collection.Remove(location.gameObject);
            takenLocations.Add(location);

            newCharacter.transform.position = location.position;
            newCharacter.transform.rotation = location.rotation * Quaternion.Euler(0f, 180f, 0f);

            CharacterController newController = newCharacter.GetComponent<CharacterController>();
            newController.character = character;
            newController.SetupVisual();
        }
    }
}
