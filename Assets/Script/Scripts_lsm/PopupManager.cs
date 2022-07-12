using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    private Dictionary<string, PopupWindow> popupDict = new Dictionary<string, PopupWindow>();
    private List<PopupWindow> activePopupList = new List<PopupWindow>();

    public static PopupManager Instance
    {
        get
        {
            return instance;
        }
    }
    private static PopupManager instance;

    private void Awake() {
        if (instance)
        {
            DestroyImmediate(gameObject);
            return;
        }
        instance = this;
    }

    private void Start() {
        foreach (PopupWindow popup in popupDict.Values())
        {
            popup.gameObject.SetActive(false);
        }
    }

    public void ShowPopup(string name)
    {
        PopupWindow popup = popupDict[name];
        popup.gameObject.SetActive(true);
        activePopupList.Add(popup);
    }

    public void SelectPopup(string name)
    {
        PopupWindow selectedPopup = popupDict[name];
    }

    public void HidePopup(string name)
    {
        PopupWindow popup = popupDict[name];
        popup.gameObject.SetActive(false);
        activePopupList.Remove(popup);
    }
}
