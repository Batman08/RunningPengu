using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform Player;
    public Vector3 Offset = new Vector3(0, 0.5f, -10);

    private void Start()
    {
        transform.position = Player.position + Offset;
    }

    private void LateUpdate()
    {
        Vector3 desiredPosition = Player.position + Offset;
        desiredPosition.x = 0;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime);
    }
}
