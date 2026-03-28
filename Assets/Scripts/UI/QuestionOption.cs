using UnityEngine;

public class QuestionOption : MonoBehaviour
{
    public Character character;

    public void Select()
    {
        QTargetScreenController.Instance.OpenScreen(character);
        QuestionScreenHolder.Instance.CloseScreen();
    }
}
