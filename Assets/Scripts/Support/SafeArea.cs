using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//UI 요소들의 Root Object에 해당 스크립트를 추가하면 safe area에 맞게 Anchor를 조정
public class SafeArea : MonoBehaviour
{
    RectTransform rt;
        // Start is called before the first frame update
    void Start()
    {
        rt = GetComponent<RectTransform>();
        SetAnchor();
    }

    private void SetAnchor()
    {
        Rect safeArea = Screen.safeArea;

        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;

        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        rt.anchorMin = anchorMin;
        rt.anchorMax = anchorMax;
    }
}
