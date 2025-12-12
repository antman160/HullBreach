using UnityEngine;
using UnityEngine.UI;

public class UILine : MonoBehaviour
{
    public RectTransform startPoint;
    public RectTransform endPoint;
    public Image lineImage;

    public bool showLine = false; 
    void Update()
    {
        if (!showLine)
        {
            lineImage.enabled = false;
            return;
        }

        lineImage.enabled = true;

        Vector2 dir = endPoint.anchoredPosition - startPoint.anchoredPosition;
        float distance = dir.magnitude;

        // Resize line to match distance
        lineImage.rectTransform.sizeDelta = new Vector2(distance, lineImage.rectTransform.sizeDelta.y);

        // Position halfway
        lineImage.rectTransform.anchoredPosition = startPoint.anchoredPosition + dir / 2f;

        // Rotate
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        lineImage.rectTransform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
