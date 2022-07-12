using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InGameApp : BaseApplication
{
    [SerializeField]
    private InGameApp _inGameApp;
    [SerializeField]
    private List<BaseApplication> _popupList;
    [SerializeField]
    private GameObject _popupInstanciate;
    [SerializeField]
    private Canvas _canvas;

    public override void Init()
    {
        PopupManager.Instance.SetCanvas(_canvas);
    }

    public override void Set()
    {
    }

    public override void AdvanceTime(float dt_sec)
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            PopupManager.Instance.CreatePopup(EPrefabsType.Popup, "DestroyOnClickPopup");
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            PopupManager.Instance.CreatePopup(EPrefabsType.Popup, "NormalPopup");
        }
    }

    public override void Dispose()
    {
    }
}
