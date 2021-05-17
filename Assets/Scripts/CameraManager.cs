using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    public GameObject camTarget;
    public float camSpeed;
    public BoxCollider2D bound;

    private Camera cam;

    private Vector3 targetPos;

    private Vector3 minBound;
    private Vector3 maxBound;

    private float halfWidth;
    private float halfHeight;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        cam = GetComponent<Camera>();
        Init();
    }

    public void Init()
    {
        minBound = bound.bounds.min;
        maxBound = bound.bounds.max;
        halfHeight = cam.orthographicSize;
        halfWidth = halfHeight * Screen.width / Screen.height;
    }

    void LateUpdate()
    {
        if (camTarget != null)
        {
            targetPos.Set(camTarget.transform.position.x, camTarget.transform.position.y, transform.position.z);

            //transform.position = Vector3.Lerp(transform.position, targetPos, camSpeed * Time.deltaTime);
            transform.position = targetPos;

            float clampedX = Mathf.Clamp(transform.position.x, minBound.x + halfWidth, maxBound.x - halfWidth);
            float clampedY = Mathf.Clamp(transform.position.y, minBound.y + halfHeight, maxBound.y - halfHeight);

            transform.position = new Vector3(clampedX, clampedY, transform.position.z);
        }
    }
}
