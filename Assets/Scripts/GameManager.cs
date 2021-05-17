using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Text mapNameUIText;
    private IEnumerator coroutine;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void TransferMap(Map map)
    {
        CameraManager.instance.bound = map.bound;
        CameraManager.instance.Init();

        if(coroutine!=null)
            StopCoroutine(coroutine);
        coroutine = ShowMapNameCoroutine(map.mapName);
        StartCoroutine(coroutine);
    }
    
    public void TransferMap(Map map, Collision2D collision, Transform targetPoint)
    {
        collision.gameObject.transform.position = targetPoint.position;
        CameraManager.instance.bound = map.bound;
        CameraManager.instance.Init();

        if (coroutine != null)
            StopCoroutine(coroutine);
        coroutine = ShowMapNameCoroutine(map.mapName);
        StartCoroutine(coroutine);
    }

    IEnumerator ShowMapNameCoroutine(string mapName)
    {
        mapNameUIText.text = mapName;

        Color color = mapNameUIText.color;

        while (color.a <= 1)
        {
            yield return null;

            yield return new WaitForSeconds(0.001f);
            color.a += 0.02f;
            mapNameUIText.color = color;
        }

        yield return new WaitForSeconds(1f);

        while (color.a >= 0)
        {
            yield return null;

            yield return new WaitForSeconds(0.001f);
            color.a -= 0.02f;
            mapNameUIText.color = color;
        }
    }
}
