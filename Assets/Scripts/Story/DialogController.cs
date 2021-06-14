using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogController : MonoBehaviour
{
    public GameObject spriteRoot;
    public GameObject panelRoot;
    public GameObject dialogPanel;
    public GameObject namePanel;
    public TextMeshProUGUI dialogText;
    public TextMeshProUGUI nameText;

    public GameObject characterPrefab;

    private Animator namePanelAnimator;
    private Animator dialogPanelAnimator;

    private string isTalking = "isTalking";
    private bool isDialogActivated;

    private Dictionary<CharacterPos, GameObject> characterSpritesDic = new Dictionary<CharacterPos, GameObject>();
    private Dictionary<CharacterPos, Image> characterSpritesImageDic = new Dictionary<CharacterPos, Image>();

    public static DialogController instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        namePanelAnimator = namePanel.GetComponent<Animator>();
        dialogPanelAnimator = dialogPanel.GetComponent<Animator>();
    }

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

    public void MakeSpriteGameObj()
    {
        for (int i = 0; i < posDic.Count; i++)
        {
            characterSpritesDic.Add((CharacterPos)i, Instantiate(characterPrefab, spriteRoot.transform));
            // Instantiate에서 position을 설정하면 부모가 기준이 아니기 때문에 엉뚱하게 배치된다.
            characterSpritesDic[(CharacterPos)i].transform.localPosition = posDic[(CharacterPos)i];
            characterSpritesDic[(CharacterPos)i].SetActive(false);
        }

        CharacterPos[] tempPos = characterSpritesDic.Keys.ToArray();
        GameObject[] tempGobj = characterSpritesDic.Values.ToArray();

        // Getcomponent 자주 사용시 성능 저하되므로 캐싱 필요
        for (int i = 0; i < characterSpritesDic.Values.Count; i++)
        {
            characterSpritesImageDic.Add(tempPos[i], tempGobj[i].GetComponent<Image>());
        }
    }

    public void ShowDialog(DialogRoot dialog, DialogEffectRoot dialogEffect)
    {
        if (!isDialogActivated)
            StartCoroutine(ShowDialogCoroutine(dialog.dialogList, dialogEffect.dialogEffectList));
    }

    IEnumerator ShowDialogCoroutine(List<Dialog> dialogList, List<DialogEffect> dialogEffectList)
    {
        spriteRoot.SetActive(true);
        panelRoot.SetActive(true);

        // 애니메이션 재생 방지
        if (dialogList[0].teller.Equals(""))
            namePanel.SetActive(false);

        yield return StartCoroutine(ShowPanelAnimCoroutine(true));

        for (int i = 0; i < dialogList.Count; i++)
        {
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

            if (!dialogEffectList[i].bg.Equals(""))
            {
                // 배경 변경
            }

            if (!dialogEffectList[i].sfx.Equals(""))
            {
                // 소리 재생
            }

            if (dialogEffectList[i].characterList.Count != 0)
            {
                ShowCharacters(dialogEffectList[i].characterList);
            }

            yield return StartCoroutine(ShowTypingEffectCoroutine(dialogList[i].content));
            dialogText.text = "";
        }

        // foreach보다 성능이 좋음
        var enumerator = characterSpritesDic.GetEnumerator();
        while (enumerator.MoveNext())
        {
            enumerator.Current.Value.SetActive(false);
        }

        yield return StartCoroutine(ShowPanelAnimCoroutine(false));
        panelRoot.SetActive(false);
        spriteRoot.SetActive(false);
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

    IEnumerator ShowPanelAnimCoroutine(bool condition)
    {
        isDialogActivated = condition;
        dialogText.text = "";
        dialogPanelAnimator.SetBool(isTalking, condition);
        namePanelAnimator.SetBool(isTalking, condition);
        yield return new WaitForSeconds(0.2f);
    }

    IEnumerator ShowTypingEffectCoroutine(string content)
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
