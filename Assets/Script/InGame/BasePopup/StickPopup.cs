using UnityEngine;
using TMPro;
using System.Collections;

public class StickPopup : PopupBase
{
    [SerializeField]
    public TextMeshProUGUI text;

    public override void Init()
    {
        base.Init();
        text.text = "Popup1";
    }

    public override void Set()
    {
    }

    public override void AdvanceTime(float dt_sec)
    {

    }

    public override void Dispose()
    {
        base.Dispose();



        PoolManager.Instance.DespawnObject(EPrefabsType.Popup, gameObject);
    }
}
