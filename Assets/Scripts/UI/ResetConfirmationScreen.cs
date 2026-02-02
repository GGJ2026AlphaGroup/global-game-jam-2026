using UnityEngine;

public class ResetConfirmationScreen : MonoBehaviour
{
    public GameObject holder;

    public void OpenScreen()
    {
        holder.SetActive(true);
    }

    public void CloseScreen()
    {
        holder.SetActive(false);
    }

    public void ConfirmReset()
    {
        PuzzleManager.Instance.ResetPuzzle();
        CloseScreen();
    }
}
