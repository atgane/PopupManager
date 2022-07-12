using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public abstract class PopupBase : BaseApplication
{
    #region static variables
    #endregion

    #region constant
    #endregion

    #region attributes
    [SerializeField]
    private Button _closeBtn; // 닫는 버튼
    [SerializeField]
    private RectTransform _rectTransform;
    #endregion

    #region [get, set]
    #endregion

    public override void Init()
    {
        if (_closeBtn == null)
        {
            Debug.LogError("[self] expected close button");
        }
        if (_rectTransform == null)
        {
            Debug.LogError("[self] expected rect transform");
        }
        _rectTransform.offsetMin = new Vector2(0, _rectTransform.offsetMin.y);
        _rectTransform.offsetMax = new Vector2(0, _rectTransform.offsetMax.y);
        _rectTransform.offsetMax = new Vector2(_rectTransform.offsetMax.x, 0);
        _rectTransform.offsetMin = new Vector2(_rectTransform.offsetMin.x, 0);
        _closeBtn.onClick.AddListener(Dispose);
    }

    public override void Dispose()
    {
        _closeBtn.onClick.RemoveAllListeners();
    }
}