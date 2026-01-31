using UnityEngine;

public class PuzzleTrigger : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        new PuzzleGenerator().GeneratePuzzle(6, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
