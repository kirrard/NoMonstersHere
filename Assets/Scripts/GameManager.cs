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
        ResourcesCache.Load("Images/CG");
        ResourcesCache.Load("Images/BG");
        ResourcesCache.Load("Images/Portrait");
        ResourcesCache.Load("Audio/Sfx");
        DialogController.instance.MakeSpriteGameObj();
        InitUI();
    }

    public void InitUI()
    {
        FadeController.instance.blackImg.gameObject.SetActive(false);
        FadeController.instance.whiteImg.gameObject.SetActive(false);
        DialogController.instance.spriteRoot.SetActive(false);
        DialogController.instance.dialogPanelRoot.SetActive(false);
        DialogController.instance.miniDialogPanelRoot.SetActive(false);
    }
}
