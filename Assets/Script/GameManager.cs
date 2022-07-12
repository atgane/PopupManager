using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private InGameApp _inGameApp;

    void Start()
    {
        _inGameApp.Init();
    }

    void Update()
    {
        _inGameApp.AdvanceTime(Time.deltaTime);
    }
}
