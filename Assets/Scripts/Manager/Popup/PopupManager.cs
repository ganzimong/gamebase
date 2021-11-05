using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PopupCloseReason
{
    Ok,
    Cancel,
    ForcedClose,
}

/// <summary>
/// ����
/// 1. ���ε� ���̰ų� �˸� �˾��� �� �ִ� ��쿡�� ������ ���ƾ� �Ѵ�.
/// 2. 
/// 
/// </summary>
public class PopupManager : MonoSingleton<PopupManager>
{
    [SerializeField]
    private string resourcePath = "Popups/";
    //[SerializeField]
    //private string popupAssetName = "popups";
    [SerializeField]
    private int popupBaseDepth = 100;
    [SerializeField]
    private int popupDepthInterval = 10;

    public delegate void CloseCallback(int state);

    //�˾��� �ε� ���̰ų� �˾��� �ϳ��� ���� �ִٸ�
    public bool IsOpenedAnyPopup { get { return CurrentPopup != null; } }
    public bool IsEnableOpenPopup { get { return false; } }

    public PopupBase CurrentPopup { get; private set; }

    private List<PopupBase> _popupList = new List<PopupBase>();

#if UNITY_ANDROID || UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            if (IsOpenedAnyPopup)
            {
                CurrentPopup.OnClickBackButton();
            }
        }
    }
#endif

    public void Open<T>(IPopupMessage message = null, CloseCallback closeCallback = null) where T : PopupBase
    {
        string popupName = typeof(T).Name;

        //TODO:Prefab�� Resource�� �ٿ�ε���� ���¹��鿡�� ã�´�.
        var prefab = Resources.Load<GameObject>(resourcePath + popupName);

        if (prefab == null)
        {
            Debug.LogErrorFormat("No exist {0} asset", popupName);
            return;
        }

        var obj = Instantiate(prefab);
        obj.transform.SetParent(transform);

        var popup = obj.GetComponent<T>();

        //Set sorting order
        int sortingOrder = CurrentPopup != null ? CurrentPopup.SortingOrder + popupDepthInterval : popupBaseDepth;
        popup.CoInit(message, sortingOrder);

        _popupList.Add(popup);
        CurrentPopup = popup;
    }

    public void Close<T>() where T : PopupBase
    {
        var popup = _popupList.Find(p => p is T);
        Close(popup);
    }

    public void Close<T>(T popup) where T : PopupBase
    {
        if (popup == null)
        {
            Debug.LogErrorFormat("Not found {0}", typeof(T).Name);
            return;
        }

        //popup.Close();

        _popupList.Remove(popup);
    }

    public void CloseAll()
    {
        if (_popupList.Count > 0)
        {
            foreach (var popup in _popupList)
            {
                popup.Close(PopupCloseReason.ForcedClose);
            }

            _popupList.Clear();
            CurrentPopup = null;
        }
    }

    private IEnumerator CoOpen<T>(T popup, IPopupMessage message) where T : PopupBase
    {
        //Set sorting order
        int sortingOrder = CurrentPopup != null ? CurrentPopup.SortingOrder + popupDepthInterval : popupBaseDepth;

        _popupList.Add(popup);
        CurrentPopup = popup;

        yield return popup.CoInit(message, sortingOrder);

        yield return popup.CoShow();
    }
}