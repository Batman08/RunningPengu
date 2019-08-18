using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; set; }

    public Transform Player;
    public Vector3 Offset = new Vector3(0, 0.5f, -10);

    private void Awake()
    {
        transform.position = Player.position + Offset;
    }

    private void LateUpdate()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        Vector3 desiredPosition = Player.position + Offset;
        desiredPosition.x = 0;
        //desiredPosition.y = 0;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime);
    }
}
