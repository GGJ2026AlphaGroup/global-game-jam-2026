using UnityEngine;
using UnityEngine.EventSystems;

public class Hoverable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool isHovered;

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
        SetHovered(true);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
        SetHovered(false);
    }

    public virtual void SetHovered(bool isHovered)
    {

    }

    private void OnDisable()
    {
        isHovered = false;
        SetHovered(false);
    }
}
