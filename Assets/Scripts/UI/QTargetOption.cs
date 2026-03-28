using UnityEngine;

public class QTargetOption : MonoBehaviour
{
    public Character character;

    public void Select()
    {
        PuzzleManager.Instance.questionsRemaining--;
        Character subject = QTargetScreenController.Instance.subject;
        subject.questionClue = subject.personality switch
        {
            Personality.Perceptive => new NamedMaskClue(character, false, subject.isLiar),
            Personality.Fashionable => new NamedClothingClue(character, false, subject.isLiar),
            Personality.Socialite => new NamedActivityClue(character, false, subject.isLiar),
            Personality.Astute => new LyingClue(character),
            _ => null
        };
        subject.clues.Add(subject.questionClue);
        subject.RegisterChange();
        QResultScreenController.Instance.OpenScreen(subject);
        QTargetScreenController.Instance.CloseScreen();
        QuestionScreenHolder.Instance.CloseScreen();
    }
}
