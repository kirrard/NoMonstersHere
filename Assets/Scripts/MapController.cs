using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapController : MonoBehaviour
{
    #region Singleton
    public static MapController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    #endregion

    public FadeController fadeCtrl;
    public GameManager gameManager;
    public Text mapNameUIText;

    public void TransferMap(Map map)
    {
        gameManager.currentMap = map;
        StartCoroutine(TransferEffectsCoroutine(map));
    }

    public void TransferMap(Map map, Collider2D collision, Transform targetPoint)
    {
        gameManager.currentMap = map;
        StartCoroutine(TransferEffectsCoroutine(map, collision, targetPoint));
    }

    IEnumerator TransferEffectsCoroutine(Map map)
    {
        gameManager.playerAct.canMove = false;
        gameManager.playerAct.rigid.velocity = Vector2.zero;

        yield return StartCoroutine(fadeCtrl.FadeOutCoroutine(0.5f));

        CameraController.instance.bound = map.bound;
        CameraController.instance.CamInit();

        yield return StartCoroutine(fadeCtrl.FadeInCoroutine(0.5f));

        gameManager.playerAct.canMove = true;

        StartCoroutine(ShowMapNameCoroutine());
    }

    IEnumerator TransferEffectsCoroutine(Map map, Collider2D collision, Transform targetPoint)
    {
        gameManager.playerAct.canMove = false;
        gameManager.playerAct.rigid.velocity = Vector2.zero;

        yield return StartCoroutine(fadeCtrl.FadeOutCoroutine(0.5f));

        collision.gameObject.transform.position = targetPoint.position;

        CameraController.instance.bound = map.bound;
        CameraController.instance.CamInit();

        yield return StartCoroutine(fadeCtrl.FadeInCoroutine(0.5f));

        gameManager.playerAct.canMove = true;

        StartCoroutine(ShowMapNameCoroutine());
    }

    IEnumerator ShowMapNameCoroutine()
    {
        mapNameUIText.text = gameManager.currentMap.mapName;

        Color color = mapNameUIText.color;

        while (color.a <= 1)
        {
            color.a += Time.deltaTime / 0.3f;
            mapNameUIText.color = color;

            yield return null;
        }

        yield return new WaitForSeconds(1f);

        while (color.a >= 0)
        {
            color.a -= Time.deltaTime / 0.3f;
            mapNameUIText.color = color;

            yield return null;
        }
    }
}
