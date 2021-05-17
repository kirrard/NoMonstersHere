using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapChangePoint : MonoBehaviour
{
    public Transform targetPoint;
    public Map targetMap;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            GameManager.instance.TransferMap(targetMap, collision, targetPoint);
        }
    }
}
