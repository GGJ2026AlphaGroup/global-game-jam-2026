using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public Character character;

    public GameObject selectionImage;

    public GameObject chickenMask;
    public GameObject catMask;
    public GameObject dogMask;
    public GameObject rabbitMask;
    public GameObject ratMask;

    public Animation anim;

    public AnimationClip dancing;
    public AnimationClip walking;
    public AnimationClip drinking;
    public AnimationClip talking;
    public AnimationClip smoking;

    public Material whiteMatF;
    public Material blueMatF;
    public Material greenMatF;
    public Material redMatF;
    public Material yellowMatF;
    public Material whiteMatM;
    public Material blueMatM;
    public Material greenMatM;
    public Material redMatM;
    public Material yellowMatM;

    public SkinnedMeshRenderer smr;

    public GameObject killerIdentifier;
    public GameObject ciggarette;
    public GameObject alcohol;

    private void Start()
    {
        selectionImage.SetActive(false);
    }

    public void SetupVisual()
    {
        switch (character.mask)
        {
            case Mask.Chicken:
                chickenMask.SetActive(true);
                break;
            case Mask.Cat:
                catMask.SetActive(true);
                break;
            case Mask.Dog:
                dogMask.SetActive(true);
                break;
            case Mask.Rabbit:
                rabbitMask.SetActive(true);
                break;
            case Mask.Rat:
                ratMask.SetActive(true);
                break;
        }

        switch (character.activity)
        {
            case Activity.Dancing:
                anim.AddClip(dancing, dancing.name);
                anim.clip = dancing;
                anim[dancing.name].time = Random.value;
                anim[dancing.name].speed = Random.Range(0.8f,1.2f);
                break;
            /*case Activity.Walking:
                anim.AddClip(walking, walking.name);
                anim.clip = walking;
                anim[walking.name].time = Random.value;
                anim[walking.name].speed = Random.Range(0.8f, 1.2f);
                break;*/
            case Activity.Drinking:
                anim.AddClip(drinking, drinking.name);
                anim.clip = drinking;
                anim[drinking.name].time = Random.value;
                anim[drinking.name].speed = Random.Range(0.8f, 1.2f);
                break;
            case Activity.Talking:
                anim.AddClip(talking, talking.name);
                anim.clip = talking;
                anim[talking.name].time = Random.value;
                anim[talking.name].speed = Random.Range(0.8f, 1.2f);
                break;
            case Activity.Smoking:
                anim.AddClip(smoking, smoking.name);
                anim.clip = smoking;
                anim[smoking.name].time = Random.value;
                anim[smoking.name].speed = Random.Range(0.8f, 1.2f);
                break;
        }


        switch (character.clothing)
        {
            case Clothing.White:
                smr.SetMaterials(new() { smr.materials[0], character.outfitType == 0 ? whiteMatM : whiteMatF });
                break;
            case Clothing.Red:
                smr.SetMaterials(new() { smr.materials[0], character.outfitType == 0 ? redMatM : redMatF });
                break;
            case Clothing.Blue:
                smr.SetMaterials(new() { smr.materials[0], character.outfitType == 0 ? blueMatM : blueMatF });
                break;
            case Clothing.Green:
                smr.SetMaterials(new() { smr.materials[0], character.outfitType == 0 ? greenMatM : greenMatF });
                break;
            case Clothing.Yellow:
                smr.SetMaterials(new() { smr.materials[0], character.outfitType == 0 ? yellowMatM : yellowMatF });
                break;
        }

        if (!character.isKiller) killerIdentifier.SetActive(false);
        if (character.activity != Activity.Smoking) ciggarette.SetActive(false);
        if (character.activity != Activity.Drinking) alcohol.SetActive(false);

        anim.Play();
    }

    public void SetHovered(bool isHovered)
    {
        selectionImage.SetActive(isHovered);
    }
}
