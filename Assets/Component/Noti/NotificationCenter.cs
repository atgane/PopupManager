using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static NotificationCenter;

public class NotificationCenter : MonoBehaviour
{
    #region Singelton
    private static bool _appIsClosing = false;
    private static NotificationCenter _instance;
    public static NotificationCenter Instance
    {
        get
        {
            if (_appIsClosing)
            {
                return null;
            }

            if (_instance == null)
            {
                _instance = FindObjectOfType<NotificationCenter>();
                if (FindObjectsOfType<NotificationCenter>().Length > 1)
                {
                    Debug.LogError("[Singleton] Something went really wrong " +
                        " - there should never be more than 1 singleton!" +
                        " Reopening the scene might fix it.");
                    return _instance;
                }

                if (_instance == null)
                {
                    GameObject go = new GameObject("Default Notification Center");
                    _instance = go.AddComponent<NotificationCenter>();
                }
            }

            return _instance;
        }
    }
    void OnApplicationQuit()
    {
        // release reference on exit
        _appIsClosing = true;
    }
    #endregion

    Hashtable _notifications = new Hashtable();

    public delegate void DelFunction(Notification noti);

    #region Add Observer

    public void AddObserver(DelFunction observer, ENotiMessage msg/*, Component sender*/)
    {
        if (_notifications[msg] == null)
        {
            _notifications[msg] = new List<DelFunction>();
        }

        List<DelFunction> notifyList = _notifications[msg] as List<DelFunction>;

        if (!notifyList.Contains(observer)) { notifyList.Add(observer); }
    }
    #endregion

    #region Remove Observer
    public void RemoveObserver(DelFunction observer, ENotiMessage msg)
    {
        List<DelFunction> notifyList = (List<DelFunction>)_notifications[msg];

        if (notifyList != null)
        {
            if (notifyList.Contains(observer))
            {
                notifyList.Remove(observer);
            }

            if (notifyList.Count == 0)
            {
                _notifications.Remove(msg);
            }
        }
    }
    #endregion

    #region Post Notification
    public void PostNotification(ENotiMessage aMsg)
    {
        PostNotification(aMsg, null);
    }

    public void PostNotification(ENotiMessage aMsg, Hashtable aData)
    {
        //PostNotification(new Notification(aMsg, aData));
        PostNotification(Notification.Instantiate(aMsg, aData));
    }

    public void PostNotification(Notification aNotification)
    {
        List<DelFunction> notifyList = (List<DelFunction>)_notifications[aNotification.msg];
        if (notifyList == null)
        {
            Debug.Log("Notify list not found in PostNotification: " + aNotification.msg);
            return;
        }

        List<DelFunction> observersToRemove = new List<DelFunction>();

        foreach (DelFunction observer in notifyList)
        {
            if (observer == null)
            {
                observersToRemove.Add(observer);
            }
            else
            {
                observer(aNotification);
            }
        }

        foreach (DelFunction observer in observersToRemove)
        {
            notifyList.Remove(observer);
        }
    }
    #endregion

}
public class Notification
{
    public ENotiMessage msg;
    public Hashtable data;
    private static Notification _instance = new Notification(ENotiMessage.UNKNOWN);

    private Notification(ENotiMessage aMsg)
    {
        data = new Hashtable();
        msg = aMsg;
    }

    //public Notification(ENotiMessage aMsg)
    //{
    //    msg = aMsg;
    //    data = null;
    //}

    //public Notification(ENotiMessage aMsg, Hashtable aData)
    //{
    //    msg = aMsg;
    //    data = aData;
    //}

    public static Notification Instantiate(ENotiMessage aMsg, Hashtable aData)
    {
        _instance.msg = aMsg;
        if (aData != null)
        {
            _instance.data.Clear();
            _instance.data = aData;
        }
        return _instance;
    }

    public static Notification Instantiate<T>(ENotiMessage aMsg, EDataParamKey dataParam, T data)
    {
        _instance.msg = aMsg;

        if (data != null)
        {
            _instance.data.Clear();
            _instance.data.Add(dataParam, data);
        }
        return _instance;
    }

    public static Notification Instantiate(ENotiMessage aMsg)
    {
        _instance.msg = aMsg;
        _instance.data.Clear();
        return _instance;
    }

}

public enum ENotiMessage
{
    UNKNOWN = 0,

    //500 ~ 1000 GameTabChange 관련
    ChangeSceneState = 500,
    GameTabChange,
    //1000 ~ 1199 GuideCollision관련
    OnGuideCellCollisionStart = 1000,
    OnGuideCellCollisionEnd,
    //1200~2000 Cell 관련
    CellCreateSuccess = 1200,
    OnClickCellCreateBtn,
    OnClickConnectorCreateBtn,

}

public enum EDataParamKey
{
    //숫자관련
    Integer, //일반 int 변수형 낱개로 보낼때
    Vector3, //vector3
    //
    GameObject, //게임 오브젝트 보낼때
    State,
}