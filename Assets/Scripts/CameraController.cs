using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; set; }

    public Transform Player;
    public Vector3 Offset = new Vector3(0, 0.5f, -10); //x = 0, y = 6, z = -7
    public Vector3 Rotation = new Vector3(35, 0, 0);

    public bool IsMoving { get; set; }

    //private void Awake()
    //{
    //    transform.position = Player.position + Offset;
    //}

    private void LateUpdate()
    {
        if (!IsMoving)
            return;

        FollowPlayer();
    }// camera roation 13.5

    private void FollowPlayer()
    {
        Vector3 desiredPosition = Player.position + Offset;
        desiredPosition.x = 0;
        desiredPosition.y = Offset.y /*6.14f*/;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, 0.1f); //Time.DeltaTime
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(Rotation), 0.1f);
    }
}
