using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//UI ��ҵ��� Root Object�� �ش� ��ũ��Ʈ�� �߰��ϸ� safe area�� �°� Anchor�� ����
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
