using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogController : MonoBehaviour
{
    public GameObject spriteRoot;
    public GameObject dialogPanelRoot;
    public GameObject dialogPanel;
    public GameObject namePanel;
    public GameObject bg;
    public TextMeshProUGUI dialogText;
    public TextMeshProUGUI nameText;

    public GameObject miniDialogPanelRoot;
    public GameObject miniDialogPanel;
    public GameObject portrait;
    public GameObject miniDlgArea;
    public GameObject miniDlgAreaNoPrt;

    public GameObject characterPrefab;

    private Animator namePanelAnimator;
    private Animator dialogPanelAnimator;

    private TextMeshProUGUI miniDlgText;
    private TextMeshProUGUI miniDlgTextNoPrt;

    private Image portraitImageComponent;
    private Image bgImageComponent;

    private readonly string isTalking = "isTalking";
    private bool isDialogActivated;

    private readonly Dictionary<CharacterPos, Vector3> posDic = new Dictionary<CharacterPos, Vector3>
    {
        { CharacterPos.L1, new Vector3(-720f, -243f, 0f) },
        { CharacterPos.L2, new Vector3(-480f, -243f, 0f) },
        { CharacterPos.L3, new Vector3(-240f, -243f, 0f) },
        { CharacterPos.C, new Vector3(0f, -243f, 0f) },
        { CharacterPos.R1, new Vector3(720f, -243f, 0f) },
        { CharacterPos.R2, new Vector3(480f, -243f, 0f) },
        { CharacterPos.R3, new Vector3(240f, -243f, 0f) },
    };
    private readonly Dictionary<MiniDlgPanelPos, Vector3> panelPosDic = new Dictionary<MiniDlgPanelPos, Vector3>
    {
        { MiniDlgPanelPos.Up, new Vector3(0f, 300f, 0f) },
        { MiniDlgPanelPos.Center, new Vector3(0f, 0f, 0f) },
        { MiniDlgPanelPos.Down, new Vector3(0f, -300f, 0f) }
    };

    private Dictionary<CharacterPos, GameObject> characterSpritesDic = new Dictionary<CharacterPos, GameObject>();
    private Dictionary<CharacterPos, Image> characterSpritesImageDic = new Dictionary<CharacterPos, Image>();

    public static DialogController instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        miniDlgText = miniDlgArea.GetComponent<TextMeshProUGUI>();
        miniDlgTextNoPrt = miniDlgAreaNoPrt.GetComponent<TextMeshProUGUI>();
        namePanelAnimator = namePanel.GetComponent<Animator>();
        dialogPanelAnimator = dialogPanel.GetComponent<Animator>();
        portraitImageComponent = portrait.GetComponent<Image>();
        bgImageComponent = bg.GetComponent<Image>();
    }

    public void MakeSpriteGameObj()
    {
        for (int i = 0; i < posDic.Count; i++)
        {
            characterSpritesDic.Add((CharacterPos)i, Instantiate(characterPrefab, spriteRoot.transform));
            characterSpritesDic[(CharacterPos)i].transform.localPosition = posDic[(CharacterPos)i];
            // Instantiate에서 position을 설정하면 부모가 기준이 아니기 때문에 엉뚱하게 배치된다.
            characterSpritesDic[(CharacterPos)i].SetActive(false);
        }

        CharacterPos[] tempPos = characterSpritesDic.Keys.ToArray();
        GameObject[] tempGobj = characterSpritesDic.Values.ToArray();

        // Getcomponent 자주 사용시 성능 저하되므로 캐싱 필요
        for (int i = 0; i < characterSpritesDic.Values.Count; i++)
            characterSpritesImageDic.Add(tempPos[i], tempGobj[i].GetComponent<Image>());
    }

    public void ShowDialog(DialogRoot dialog, DialogEffectRoot dialogEffect)
    {
        if (!isDialogActivated)
            StartCoroutine(ShowDialogCoroutine(dialog.dialogList, dialogEffect.dialogEffectList));
    }

    public void ShowMiniDialog(MiniDialogRoot miniDialogRoot, MiniDialogEffectRoot miniDialogEffectRoot)
    {
        if (!isDialogActivated)
            StartCoroutine(ShowMiniDialogCoroutine(miniDialogRoot.dialogList, miniDialogEffectRoot.dialogEffectList));
    }

    IEnumerator ShowDialogCoroutine(List<Dialog> dialogList, List<DialogEffect> dialogEffectList)
    {
        GameManager.instance.playerAct.SetCanMove(false);
        GameManager.instance.playerAct.SetVelocity(Vector2.zero);

        dialogText.text = "";
        isDialogActivated = true;
        spriteRoot.SetActive(true);
        dialogPanelRoot.SetActive(true);

        // 애니메이션 재생 방지
        if (dialogList[0].teller.Equals(""))
            namePanel.SetActive(false);

        yield return StartCoroutine(ShowPanelAnimCoroutine(true));

        for (int i = 0; i < dialogList.Count; i++)
        {
            int index = -1;

            //teller가 없으면 namePanel 없음
            if (dialogList[i].teller.Equals(""))
            {
                namePanel.SetActive(false);
            }
            else
            {
                namePanel.SetActive(true);
                namePanelAnimator.SetBool(isTalking, true);
                nameText.text = dialogList[i].teller;
            }

            for (int j = 0; j < dialogEffectList.Count; j++)
            { 
                if(dialogEffectList[j].id.Equals(i))
                {
                    index = j;
                    break;
                }
            }

            if (index != -1)
                ShowEffects(dialogEffectList[index]);

            yield return StartCoroutine(ShowTypingEffectCoroutine(dialogList[i].content, dialogText));
            dialogText.text = "";
        }

        // foreach보다 성능이 좋음
        var enumerator = characterSpritesDic.GetEnumerator();
        while (enumerator.MoveNext())
            enumerator.Current.Value.SetActive(false);

        AudioController.instance.StopAudio();
        yield return StartCoroutine(ShowPanelAnimCoroutine(false));

        Color color = bgImageComponent.color;
        color.a = 0f;
        bgImageComponent.color = color;

        isDialogActivated = false;
        dialogPanelRoot.SetActive(false);
        spriteRoot.SetActive(false);
        dialogText.text = "";

        GameManager.instance.playerAct.SetCanMove(true);
    }

    IEnumerator ShowMiniDialogCoroutine(List<MiniDialog> dialogList, List<MiniDialogEffect> dialogEffectList)
    {
        GameManager.instance.playerAct.SetCanMove(false);
        GameManager.instance.playerAct.SetVelocity(Vector2.zero);

        miniDlgText.text = "";
        miniDlgTextNoPrt.text = "";

        isDialogActivated = true;
        miniDialogPanelRoot.SetActive(true);
        miniDialogPanel.SetActive(true);

        bool hasPortrait = false;

        for (int i = 0; i < dialogList.Count; i++)
        {
            int index = -1;

            for (int j = 0; j < dialogEffectList.Count; j++)
            {
                if (dialogEffectList[j].id.Equals(i))
                {
                    index = j;
                    break;
                }
            }

            if (index != -1)
                hasPortrait = ShowEffects(dialogEffectList[index]);

            if (hasPortrait)
            {
                miniDlgArea.SetActive(true);
                miniDlgAreaNoPrt.SetActive(false);
                yield return StartCoroutine(ShowTypingEffectCoroutine(dialogList[i].content, miniDlgText));
            }
            else
            {
                miniDlgArea.SetActive(false);
                miniDlgAreaNoPrt.SetActive(true);
                yield return StartCoroutine(ShowTypingEffectCoroutine(dialogList[i].content, miniDlgTextNoPrt));
            }
            miniDlgText.text = "";
            miniDlgTextNoPrt.text = "";
        }

        AudioController.instance.StopAudio();
        miniDialogPanelRoot.SetActive(false);
        miniDialogPanel.SetActive(false);
        isDialogActivated = false;

        GameManager.instance.playerAct.SetCanMove(true);
    }

    void ShowEffects(DialogEffect dialogEffect)
    {
        if (!dialogEffect.bg.src.Equals("None"))
        {
            ShowBG(dialogEffect.bg.mode, dialogEffect.bg.src);
        }
        else
        {
            bgImageComponent.sprite = null;
            bg.SetActive(false);
        }

        if (!dialogEffect.sfx.src.Equals("None"))
        {
            AudioController.instance.SetAudioSource(dialogEffect.sfx.src);
            AudioController.instance.SetLoop(dialogEffect.sfx.loop);
            AudioController.instance.PlayAudio();
        }
        else
        {
            AudioController.instance.StopAudio();
        }

        if (dialogEffect.characterList.Count != 0)
        {
            ShowCharacters(dialogEffect.characterList);
        }
    }

    bool ShowEffects(MiniDialogEffect dialogEffect)
    {
        miniDialogPanelRoot.transform.localPosition = panelPosDic[dialogEffect.pos];

        if (!dialogEffect.sfx.src.Equals("None"))
        {
            AudioController.instance.SetAudioSource(dialogEffect.sfx.src);
            AudioController.instance.SetLoop(dialogEffect.sfx.loop);
            AudioController.instance.PlayAudio();
        }
        else
        {
            AudioController.instance.StopAudio();
        }

        if (!dialogEffect.portrait.Equals("None"))
        {
            portrait.SetActive(true);
            portraitImageComponent.sprite = ResourcesCache.GetObject(dialogEffect.portrait) as Sprite;
            return true;
        }
        else
        {
            portrait.SetActive(false);
            portraitImageComponent.sprite = null;
            return false;
        }
    }

    void ShowCharacters(List<Character> characterList)
    {
        for (int i = 0; i < characterList.Count; i++)
        {
            characterSpritesDic[characterList[i].pos].SetActive(true);
            characterSpritesImageDic[characterList[i].pos].sprite = ResourcesCache.GetObject(characterList[i].sprite) as Sprite;

            Color color = characterSpritesImageDic[characterList[i].pos].color;

            if (!characterList[i].telling)
            {
                color.r = 0.5f;
                color.g = 0.5f;
                color.b = 0.5f;
            }
            else
            {
                color.r = 1f;
                color.g = 1f;
                color.b = 1f;
            }

            characterSpritesImageDic[characterList[i].pos].color = color;
        }
    }

    void ShowBG(BgMode mode, string bgName)
    {
        bg.SetActive(true);
        bgImageComponent.sprite = ResourcesCache.GetObject(bgName) as Sprite;

        switch (mode)
        {
            case BgMode.FadeIn:
                StartCoroutine(FadeController.instance.FadeInCoroutine(0.7f, bgImageComponent));
                break;
            case BgMode.FadeOut:
                StartCoroutine(FadeController.instance.FadeOutCoroutine(0.7f, bgImageComponent));
                break;
            case BgMode.None:
                Color color = bgImageComponent.color;
                color.a = 1f;
                bgImageComponent.color = color;
                break;
        }
    }

    IEnumerator ShowPanelAnimCoroutine(bool condition)
    {
        dialogPanelAnimator.SetBool(isTalking, condition);
        namePanelAnimator.SetBool(isTalking, condition);
        yield return new WaitForSeconds(0.2f);
    }

    IEnumerator ShowTypingEffectCoroutine(string content, TextMeshProUGUI dialogText)
    {
        for (int i = 0; i < content.Length; i++)
        {
            if (Input.GetKey(KeyCode.X))
            {
                dialogText.text = content;
                break;
            }
            else
            {
                dialogText.text += content[i];
                yield return new WaitForSeconds(0.05f);
            }
        }

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Z) && dialogText.text.Equals(content));
    }
}
