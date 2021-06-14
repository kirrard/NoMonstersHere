using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    public GameObject camTarget;
    public float camSpeed;
    public BoxCollider2D bound;

    private Camera cam;

    private Vector3 targetPos;

    private Vector3 minBound;
    private Vector3 maxBound;

    private float halfWidth;
    private float halfHeight;

    // transfrom 캐싱
    private Transform camTargetTf;
    private Transform camTf;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        cam = GetComponent<Camera>();
        camTf = GetComponent<Transform>();
        camTargetTf = camTarget.GetComponent<Transform>();
        CamInit();
    }

    public void CamInit()
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
            targetPos.Set(camTargetTf.position.x, camTargetTf.position.y, camTf.position.z);

            //transform.position = Vector3.Lerp(transform.position, targetPos, camSpeed * Time.deltaTime);
            camTf.position = targetPos;

            float clampedX = Mathf.Clamp(camTf.position.x, minBound.x + halfWidth, maxBound.x - halfWidth);
            float clampedY = Mathf.Clamp(camTf.position.y, minBound.y + halfHeight, maxBound.y - halfHeight);

            camTf.position = new Vector3(clampedX, clampedY, camTf.position.z);
        }
    }
}
