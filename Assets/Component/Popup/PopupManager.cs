using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PopupManager : MonoBehaviour
{
    #region Singelton
    private static PopupManager _instance;
    public static PopupManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PopupManager>();
                if (FindObjectsOfType<PopupManager>().Length > 1)
                {
                    Debug.LogError("[Singleton] Something went really wrong " +
                        " - there should never be more than 1 singleton!" +
                        " Reopening the scene might fix it.");
                    return _instance;
                }

                if (_instance == null)
                {
                    GameObject go = new GameObject("PopupManager");
                    _instance = go.AddComponent<PopupManager>();
                }
            }
            return _instance;
        }
    }
    #endregion

    #region lifeCycle
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SetCanvas();
        NotificationCenter.Instance.AddObserver(OnNotification, ENotiMessage.ChangeSceneState);

        foreach (var popupbase in _popupStack)
        {
            popupbase.Init();
        }
    }

    private void Update()
    {
        Debug.Log(_popupStack.Count);
        foreach (var popupbase in _popupStack)
        {
            popupbase.AdvanceTime(Time.deltaTime);
        }
    }
    #endregion

    #region private
    private void InitInstance(GameObject go)
    {
        PopupBase _popupInstance = go.GetComponent<PopupBase>();
        _popupInstance.Init();
        _popupInstance.Set();
        _popupStack.Push(_popupInstance);
    }

    private GameObject CreatePopup(EPrefabsType type, string name, Transform layer)
    {
        go = PoolManager.Instance.GrabPrefabs(type, name, _canvas.transform);
        go.transform.position = _canvas.transform.position;
        return go;
    }

    private Canvas _canvas = null;
    private Stack<PopupBase> _popupStack = new Stack<PopupBase>();
    private GameObject go = null;

    private void OnNotification(Notification noti)
    {
        switch (noti.msg)
        {
            case ENotiMessage.ChangeSceneState:
                _canvas = null;
                DeleteAll();
                SetCanvas();
                if (_canvas == null) Debug.LogError("[Self] expected PopupCanvas");
                break;
        }
    }
    #endregion

    private void SetCanvas()
    {
        _canvas = GameObject.Find("PopupCanvas").GetComponent<Canvas>();
        if (_canvas != null) return;
        _canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }

    public GameObject CreatePopup(EPrefabsType type, string name)
    {
        if (_canvas == null)
        {
            Debug.LogError("[Self] expected canvas");
            return null;
        }

        go = CreatePopup(type, name,  _canvas.transform);
        InitInstance(go);
        return go;
    }

    public void DeleteHead()
    {
        if (_popupStack.Count == 0)
        {
            Debug.LogError("[Self] stack count zero");
            return;
        }
        PopupBase _head = _popupStack.Pop();
        _head.Dispose();
    }

    public void DeleteAll()
    {
        while (_popupStack.Count > 0)
        {
            DeleteHead();
        }
    }
}
