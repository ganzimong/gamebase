using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class PopupBase : MonoBehaviour
{
    [SerializeField]
    private Button okButton;
    [SerializeField]
    private Button cancelButton;

    public int SortingOrder { get { return _canvas.sortingOrder; } }
    public bool IsEnabledPopup
    {
        get
        {
            return _canvasGroup.alpha == 1f && _canvasGroup.interactable;
        }
    }

    private Canvas _canvas;
    private CanvasGroup _canvasGroup;
    private IPopupMessage popupMessage;

    private void Awake()
    {
        if (okButton != null)
        {
            okButton.onClick.AddListener(OnClickOk);
        }

        if (cancelButton != null)
        {
            cancelButton.onClick.AddListener(OnClickCancel);
        }
    }

    /// <summary>
    /// Don't call this method. It's called automatically in PopupManager
    /// </summary>
    public virtual IEnumerator CoInit(IPopupMessage message, int sortingOrder)
    {
        popupMessage = message;

        SetSortingOrder(sortingOrder);
        SetEnableCanvas(false);

        yield return OnInit();
    }

    /// <summary>
    /// Don't call this method. It's called automatically in PopupManager
    /// </summary>
    public virtual IEnumerator CoShow()
    {
        SetEnableCanvas(true);

        yield return OnShow();
    }

    public void Close(PopupCloseReason reason)
    {
        PopupManager.Instance.Close(this);
    }

    /// <summary>
    /// Initialize settings of popup. e.g. Loading data from server, Send analytics log, ...
    /// </summary>
    /// <returns></returns>
    protected abstract IEnumerator OnInit();

    protected abstract IEnumerator OnShow();

    protected abstract void OnClose();

    protected virtual void OnClickOk()
    {
        PlayButtonSound();
        Close(PopupCloseReason.Ok);
    }

    protected virtual void OnClickCancel()
    {
        PlayButtonSound();
        Close(PopupCloseReason.Cancel);
    }

    //Android Device���� Back Key �̺�Ʈ�� �߻��� ���
    public virtual void OnClickBackButton()
    {
        if (cancelButton != null && IsEnabledPopup)
        {
            OnClickCancel();
        }
    }

    protected virtual void PlayButtonSound()
    {
        //TODO:��ư Ŭ�� �̺�Ʈ �߻� �� ���� ���
        throw new NotImplementedException();
    }

    private void SetSortingOrder(int sortingOrder)
    {
        if (_canvas == null)
        {
            _canvas = GetComponentInChildren<Canvas>();
        }

        _canvas.overrideSorting = false;
        _canvas.sortingOrder = sortingOrder;

        //TODO:set renderer sorting order
    }

    private void SetEnableCanvas(bool enable)
    {
        if (_canvasGroup == null)
        {
            _canvasGroup.GetComponentInChildren<CanvasGroup>();
        }

        _canvasGroup.interactable = enable;
        _canvasGroup.alpha = enable ? 1f : 0f;
    }
}