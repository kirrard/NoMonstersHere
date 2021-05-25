using UnityEngine;

public class MapChangePoint : MonoBehaviour
{
    public Transform targetPoint;
    public Map targetMap;
    public bool isDoor;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (isDoor)
            {
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    MapController.instance.TransferMap(targetMap, collision, targetPoint);
                }
            }
            else
            {
                MapController.instance.TransferMap(targetMap, collision, targetPoint);
            }
        }
    }
}