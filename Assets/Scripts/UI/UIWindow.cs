using System;
using UnityEngine;
using UnityEngine.UI;

public class UIWindow : MonoBehaviour
{
    public void CloseWindow()
    {
        if (highlight != null) highlight.SetWindowHovered(false);
        Destroy(gameObject);
    }

    bool isDragged = false;

    Vector2 mouseOffset = Vector2.zero;

    public Vector2 positiveExtents;
    public Vector2 negativeExtents;

    public Hoverable hoverableTab;
    public Hoverable[] excludingHoverables;

    public event Action OnWindowBeginDragging;

    public CharacterController highlight;

    public CanvasScaler canvasScaler;

    private void Update()
    {
        bool isHovered = hoverableTab.isHovered;

        foreach (Hoverable tab in excludingHoverables)
        {
            if (tab.isHovered)
            {
                isHovered = false;
            }
        }

        if (Input.GetMouseButton(1) && isHovered)
        {
            CloseWindow();
            return;
        }

        if (Input.GetMouseButtonDown(0) && isHovered)
        {
            if (!isDragged)
            {
                mouseOffset = transform.position - Input.mousePosition;
                isDragged = true;
                transform.SetAsLastSibling();
                OnWindowBeginDragging?.Invoke();
            }
        }

        if (highlight != null)
        {
            highlight.SetWindowHovered(isHovered);
        }

        if (isDragged)
        {
            Vector2 targetPosition = mouseOffset + (Vector2)Input.mousePosition;

            if (targetPosition.x + positiveExtents.x > Screen.width)
            {
                targetPosition.x = Screen.width - (positiveExtents.x * canvasScaler.scaleFactor);
            }
            else if (targetPosition.x - negativeExtents.x < 0)
            {
                targetPosition.x = negativeExtents.x * canvasScaler.scaleFactor;
            }
            if (targetPosition.y + positiveExtents.y > Screen.height)
            {
                targetPosition.y = Screen.height - (positiveExtents.y * canvasScaler.scaleFactor);
            }
            else if (targetPosition.y - negativeExtents.y < 0)
            {
                targetPosition.y = negativeExtents.y * canvasScaler.scaleFactor;
            }

            transform.position = targetPosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragged = false;
        }
    }
}
