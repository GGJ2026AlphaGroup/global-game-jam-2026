using System.Collections;
using UnityEngine;

public class CutsceneManager : MonoSingleton<CutsceneManager>
{
    public Camera mainCamera;

    public GameObject introCutscene;
    public float introCutsceneLength;

    public GameObject victoryCutscene;
    public float victoryCutsceneLength;

    public GameObject defeatCutscene;
    public float defeatCutsceneLength;

    public GameObject incorrectGuessCutscene;
    public float incorrectGuessCutsceneLength;

    public UIDisabler UIDisabler;

    public void VictorySequence(Character killer)
    {
        StartCoroutine(VictorySequenceCutscene(killer));
    }

    IEnumerator VictorySequenceCutscene(Character killer)
    {
        mainCamera.gameObject.SetActive(false);
        CutsceneDirector cutscene = Instantiate(victoryCutscene, new Vector3(0, -100, 0), Quaternion.identity).GetComponent<CutsceneDirector>();
        cutscene.GlamTheFuckUp(killer);
        UIDisabler.SetUIEnabled(false);

        yield return new WaitForSeconds(victoryCutsceneLength);

        RunManager.Instance.Victory();

        yield return new WaitForSeconds(2);
        Destroy(cutscene.gameObject);
    }

    public void DefeatSequence(Character character, Character killer)
    {
        StartCoroutine(DefeatSequenceCutscene(character));
    }

    IEnumerator DefeatSequenceCutscene(Character character)
    {
        mainCamera.gameObject.SetActive(false);
        CutsceneDirector cutscene = Instantiate(defeatCutscene, new Vector3(0, -100, 0), Quaternion.identity).GetComponent<CutsceneDirector>();
        cutscene.GlamTheFuckUp(character);
        UIDisabler.SetUIEnabled(false);

        yield return new WaitForSeconds(defeatCutsceneLength);

        RunManager.Instance.Defeat();

        yield return new WaitForSeconds(2);
        Destroy(cutscene.gameObject);
    }

    public void PlayIncorrectGuessCutscene(Character character)
    {
        StartCoroutine(IncorrectGuessCutscene(character));
    }

    IEnumerator IncorrectGuessCutscene(Character character)
    {
        mainCamera.gameObject.SetActive(false);
        CutsceneDirector cutscene = Instantiate(incorrectGuessCutscene, new Vector3(0, -100, 0), Quaternion.identity).GetComponent<CutsceneDirector>();
        cutscene.GlamTheFuckUp(character);
        UIDisabler.SetUIEnabled(false);

        yield return new WaitForSeconds(incorrectGuessCutsceneLength);

        character.isRevealed = true;
        character.guessedMask = character.mask;
        character.guessedClothing = character.clothing;
        character.guessedActivity = character.activity;
        character.guessedName = character.name;

        FadeScreenManager.Instance.FadeOut(1f);

        yield return new WaitForSeconds(1f);

        Destroy(cutscene.gameObject);
        mainCamera.gameObject.SetActive(true);
        UIDisabler.Instance.SetUIEnabled(true);

        FadeScreenManager.Instance.FadeIn();
    }

    public void PlayIntroCutscene(Character killer)
    {
        StartCoroutine(IntroCutscene(killer));
    }

    IEnumerator IntroCutscene(Character killer)
    {
        mainCamera.gameObject.SetActive(false);
        CutsceneDirector cutscene = Instantiate(introCutscene, new Vector3(0, -100, 0), Quaternion.identity).GetComponent<CutsceneDirector>();
        cutscene.GlamTheFuckUp(killer);
        UIDisabler.SetUIEnabled(false);

        yield return new WaitForSeconds(introCutsceneLength);

        FadeScreenManager.Instance.FadeOut(1f);

        yield return new WaitForSeconds(1f);

        Destroy(cutscene.gameObject);
        mainCamera.gameObject.SetActive(true);
        UIDisabler.Instance.SetUIEnabled(true);

        FadeScreenManager.Instance.FadeIn();
    }
}
