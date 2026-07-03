using UnityEngine;
using UnityEngine.UI;

public class ScalePointer : MonoBehaviour
{
    public static ScalePointer main;

    private void Awake()
    {
        main = this;
    }

    public CanvasScaler canvasScaler;
}
