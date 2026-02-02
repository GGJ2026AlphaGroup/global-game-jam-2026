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
        TheLightingCrew.Off();
        mainCamera.gameObject.SetActive(false);
        CutsceneDirector cutscene = Instantiate(victoryCutscene, new Vector3(0, -100, 0), Quaternion.identity).GetComponent<CutsceneDirector>();
        cutscene.GlamTheFuckUp(killer);
        UIDisabler.SetUIEnabled(false);

        AudioManager.Instance.StopAllAudio();
        AudioManager.Instance.PlayAudio("audio_victory");

        yield return new WaitForSeconds(5.5f);

        TextDisplay.DisplayText("Guilty!", 2f);

        yield return new WaitForSeconds(3.833f);

        RunManager.Instance.Victory();

        yield return new WaitForSeconds(2);
        Destroy(cutscene.gameObject);
        TheLightingCrew.On();
    }

    public void DefeatSequence(Character character, Character killer)
    {
        StartCoroutine(DefeatSequenceCutscene(character));
    }

    IEnumerator DefeatSequenceCutscene(Character character)
    {
        TheLightingCrew.Off();
        mainCamera.gameObject.SetActive(false);
        CutsceneDirector cutscene = Instantiate(defeatCutscene, new Vector3(0, -100, 0), Quaternion.identity).GetComponent<CutsceneDirector>();
        cutscene.GlamTheFuckUp(character);
        UIDisabler.SetUIEnabled(false);

        AudioManager.Instance.StopAllAudio();
        AudioManager.Instance.PlayAudio("audio_defeat");

        yield return new WaitForSeconds(5.5f);

        TextDisplay.DisplayText("Innocent!", 2f);

        yield return new WaitForSeconds(7.5f);
        RunManager.Instance.Defeat();

        yield return new WaitForSeconds(2);
        Destroy(cutscene.gameObject);
        TheLightingCrew.On();
    }

    public void PlayIncorrectGuessCutscene(Character character)
    {
        StartCoroutine(IncorrectGuessCutscene(character));
    }

    IEnumerator IncorrectGuessCutscene(Character character)
    {
        TheLightingCrew.Off();
        mainCamera.gameObject.SetActive(false);
        CutsceneDirector cutscene = Instantiate(incorrectGuessCutscene, new Vector3(0, -100, 0), Quaternion.identity).GetComponent<CutsceneDirector>();
        cutscene.GlamTheFuckUp(character);
        UIDisabler.SetUIEnabled(false);

        AudioManager.Instance.StopAllAudio();
        AudioManager.Instance.PlayAudio("audio_incorrect");

        yield return new WaitForSeconds(5.5f);

        TextDisplay.DisplayText("Innocent!", 2f);

        yield return new WaitForSeconds(3.833f);

        character.isRevealed = true;
        character.guessedMask = character.mask;
        character.guessedClothing = character.clothing;
        character.guessedActivity = character.activity;
        character.guessedName = character.name;

        character.RegisterChange();

        FadeScreenManager.Instance.FadeOut(1f);

        yield return new WaitForSeconds(1f);

        Destroy(cutscene.gameObject);
        mainCamera.gameObject.SetActive(true);
        UIDisabler.Instance.SetUIEnabled(true);
        TheLightingCrew.On();

        FadeScreenManager.Instance.FadeIn();
    }

    public void PlayIntroCutscene(Character killer)
    {
        StartCoroutine(IntroCutscene(killer));
    }

    IEnumerator IntroCutscene(Character killer)
    {
        TheLightingCrew.Off();
        mainCamera.gameObject.SetActive(false);
        CutsceneDirector cutscene = Instantiate(introCutscene, new Vector3(0, -100, 0), Quaternion.identity).GetComponent<CutsceneDirector>();
        cutscene.GlamTheFuckUp(killer);
        UIDisabler.SetUIEnabled(false);

        AudioManager.Instance.StopAllAudio();
        AudioManager.Instance.PlayAudio("audio_intro");

        yield return new WaitForSeconds(introCutsceneLength);

        FadeScreenManager.Instance.FadeOut(1f);

        yield return new WaitForSeconds(1f);

        Destroy(cutscene.gameObject);
        mainCamera.gameObject.SetActive(true);
        UIDisabler.Instance.SetUIEnabled(true);
        TheLightingCrew.On();

        FadeScreenManager.Instance.FadeIn();

        yield return new WaitForSeconds(0.5f);

        TextDisplay.DisplayText("Find the killer's name!", 2f);

        AudioManager.Instance.PlayAudio("audio_ballroom", true);

        int people = PuzzleManager.Instance.characters.Length;
        if (people <= 5) AudioManager.Instance.PlayAudio("audio_small_crowd", true);
        else if (people > 5 && people < 10) AudioManager.Instance.PlayAudio("audio_mid_crowd", true);
        else AudioManager.Instance.PlayAudio("audio_large_crowd", true);
    }
}
