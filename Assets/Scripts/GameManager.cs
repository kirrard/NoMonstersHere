using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public PlayerAction playerAct;
    public Map currentMap;

    #region Singleton
    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    #endregion

    private void Start()
    {
        ResourcesCache.Load("CG");
        ResourcesCache.Load("Portrait");
        DialogController.instance.MakeSpriteGameObj();
        InitUI();
    }

    public void InitUI()
    {
        FadeController.instance.root.SetActive(false);
        DialogController.instance.spriteRoot.SetActive(false);
        DialogController.instance.panelRoot.SetActive(false);
    }
}
