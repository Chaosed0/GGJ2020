using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public static class UIUtil
{
    public static void setCanvasGroupActive(CanvasGroup canvasGroup, bool active)
    {
        if (canvasGroup == null)
        {
            return;
        }

        canvasGroup.alpha = (active ? 1.0f : 0.0f);
        canvasGroup.blocksRaycasts = active;
        canvasGroup.interactable = active;
    }

    public static List<Dropdown.OptionData> ToOptionData<T>(IList<T> things)
    {
        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>(things.Count);
        for (int i = 0; i < things.Count; i++)
        {
            Dropdown.OptionData option = new Dropdown.OptionData(things[i].ToString());
            options.Add(option);
        }
        return options;
    }

    public static Vector3 WorldToCanvas(Canvas canvas, Vector3 worldPosition)
    {
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
        return ScreenToCanvas(canvas, screenPosition);
    }

    public static Vector3 ScreenToCanvas(Canvas canvas, Vector3 screenPosition)
    {
        RectTransform canvasRectTransform = canvas.transform as RectTransform;
        Vector2 normalizedScreenPosition = screenPosition / new Vector2(Screen.width, Screen.height);
        Vector2 proportionalPosition = new Vector2(normalizedScreenPosition.x * canvasRectTransform.sizeDelta.x, normalizedScreenPosition.y * canvasRectTransform.sizeDelta.y);
        return proportionalPosition;
    }
}
