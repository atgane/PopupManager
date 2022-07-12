using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class InGameApp : BaseApplication
{
    [SerializeField]
    private InGameApp _inGameApp;
    [SerializeField]
    private List<BaseApplication> _popupList;
    [SerializeField]
    private GameObject _popupInstanciate;
    [SerializeField]
    private Button _sceneEventBtn;

    public override void Init()
    {
        _sceneEventBtn.onClick.AddListener(OnClickSceneChangeEvent);
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
        if (Input.GetKeyUp(KeyCode.F))
        {
            PopupManager.Instance.CreatePopup(EPrefabsType.Popup, "FullscreenPopup");
        }
    }

    public override void Dispose()
    {
    }

    public void OnClickSceneChangeEvent()
    {
        NotificationCenter.Instance.PostNotification(ENotiMessage.ChangeSceneState);
        SceneManager.LoadScene("SubScene");
    }
}

