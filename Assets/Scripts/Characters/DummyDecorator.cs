using UnityEngine;

public class DummyDecorator : MonoBehaviour
{
    public SkinnedMeshRenderer smr;

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

    public GameObject chickenMask;
    public GameObject catMask;
    public GameObject dogMask;
    public GameObject rabbitMask;
    public GameObject ratMask;

    public void Decorate(Character character)
    {
        SetMat(character.clothing, character.outfitType);
        SetMask(character.mask);
    }

    public void RandomDecorate()
    {
        SetMat((Clothing)Random.Range(1, 7), Random.Range(0, 2));
        SetMask((Mask)Random.Range(1, 7));
    }

    public void KillerDecorate()
    {
        SetMat(PuzzleManager.Instance.killer.clothing, PuzzleManager.Instance.killer.outfitType);
        SetMask(PuzzleManager.Instance.killer.mask);
    }

    void SetMat(Clothing clothes, int outfitType)
    {
        switch (clothes)
        {
            case Clothing.White:
                smr.SetMaterials(new() { smr.materials[0], outfitType == 0 ? whiteMatM : whiteMatF });
                break;
            case Clothing.Red:
                smr.SetMaterials(new() { smr.materials[0], outfitType == 0 ? redMatM : redMatF });
                break;
            case Clothing.Blue:
                smr.SetMaterials(new() { smr.materials[0], outfitType == 0 ? blueMatM : blueMatF });
                break;
            case Clothing.Green:
                smr.SetMaterials(new() { smr.materials[0], outfitType == 0 ? greenMatM : greenMatF });
                break;
            case Clothing.Yellow:
                smr.SetMaterials(new() { smr.materials[0], outfitType == 0 ? yellowMatM : yellowMatF });
                break;
        }
    }

    void SetMask(Mask mask)
    {
        switch (mask)
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
    }
}
