using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HoverBox : MonoBehaviour
{
    public static Hoverable currentHover;

    public CanvasGroup holder;

    public TextMeshProUGUI text;

    float targetAlpha = 0f;
    float alpha = 0f;

    bool signX = false;
    bool signY = false;

    public static void StartHovering(HoverText hoverable)
    {
        if (currentHover == hoverable)
        {
            OnUpdateHoverText?.Invoke(hoverable);
            return;
        }
        currentHover = hoverable;
        OnHoverChanged?.Invoke(hoverable);
    }

    public static void StopHovering(Hoverable hoverable)
    {
        if (currentHover != hoverable)
        {
            return;
        }
        currentHover = null;
        OnHoverChanged?.Invoke(null);
    }

    public static event Action<HoverText> OnHoverChanged;
    public static event Action<HoverText> OnUpdateHoverText;

    void Awake()
    {
        OnHoverChanged += HoverChanged;
        OnUpdateHoverText += UpdateHoverText;

        targetAlpha = 0f;
        alpha = targetAlpha;
        holder.alpha = alpha;
    }

    void OnDestroy()
    {
        OnHoverChanged -= HoverChanged;
        OnUpdateHoverText -= UpdateHoverText;
    }

    private void LateUpdate()
    {
        alpha = Mathf.MoveTowards(alpha, targetAlpha, Time.deltaTime * 8f);
        holder.alpha = alpha;

        holder.transform.position = Input.mousePosition + new Vector3(
            signX ? -10f : 20f,
            signY ? -20f : 10f,
            0f
        );
    }

    void HoverChanged(HoverText hoverable)
    {
        if (hoverable == null)
        {
            targetAlpha = 0f;
        }
        else
        {
            targetAlpha = 1f;

            signX = Input.mousePosition.x > Screen.width / 2;
            signY = Input.mousePosition.y > Screen.height / 2;

            (transform as RectTransform).pivot = new Vector2(
                signX ? 1f : 0f,
                signY ? 1f : 0f
            );

            UpdateHoverText(hoverable);
        }
    }

    void UpdateHoverText(HoverText hoverable)
    {

            text.text = hoverable.text;
            LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
        
    }
}
