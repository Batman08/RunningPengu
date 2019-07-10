using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; set; }

    public Transform Player;
    public Vector3 Offset = new Vector3(0, 0.5f, -10);

    public Vector3 PlayerXOffset = new Vector3(0, 0, 0);

    private void Start()
    {
        Instance = this;
        transform.position = Player.position + Offset;
    }

    private void LateUpdate()
    {
        FollowPlayer();
        CameraXPos();
    }

    private void FollowPlayer()
    {
        if (GameManager.Instance._hasGameStarted)
        {
            Vector3 newOffset = new Vector3(0, 0, 3.25f);
            Vector3 desiredPosition = Player.position + Offset + newOffset;
            //desiredPosition.x = Player.position.x + PlayerXOffset.x;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime);
        }

        //CameraXPos();
    }

    private void CameraXPos()
    {
        Vector3 CameraTransform = transform.position;
        CameraTransform.x = Player.position.x /*+ PlayerXOffset.x*/;
    }
}
