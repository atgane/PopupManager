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

    private void Awake()
    {
        foreach (var popupbase in _popupStack)
        {
            popupbase.Init();
        }
    }

    private void Start()
    {
        foreach (var popupbase in _popupStack)
        {
            popupbase.Set();
        }
    }

    private void Update()
    {
        foreach (var popupbase in _popupStack)
        {
            popupbase.AdvanceTime(Time.deltaTime);
        }
    }

    private Canvas _canvas = null;
    private Stack<PopupBase> _popupStack = new Stack<PopupBase>();

    // Popup이 올라갈 canvas 설정
    public void SetCanvas(Canvas canvas)
    {
        _canvas = canvas;
    }

    // 설정한 canvas위에 popup생성
    public GameObject CreatePopup(EPrefabsType type, string name)
    {
        if (_canvas == null)
        {
            Debug.LogError("[Self] expected canvas");
            return null;
        }
        GameObject go = PoolManager.Instance.GrabPrefabs(type, name, _canvas.transform);
        go.transform.position = _canvas.transform.position;
        PopupBase _popupInstance = go.GetComponent<PopupBase>();
        _popupInstance.Init();
        _popupInstance.Set();
        _popupStack.Push(_popupInstance);
        return go;
    }

    // 맨 앞 popup닫기
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

    // 모든 popup닫기
    public void DeleteAll()
    {
        while (_popupStack.Count > 0)
        {
            DeleteHead();
        }
    }
}
