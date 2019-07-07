using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; set; }

    public Transform Player;

    public Vector3 runOffset = new Vector3(0, 2.5f, 1.5f);
    public Vector3 Offset = new Vector3(0, 2.5f, -3.3f);

    private void Start()
    {
        Instance = this;
        transform.position = Player.position + Offset;
    }

    private void Update()
    {

    }

    private void LateUpdate()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        if (GameManager.Instance._hasGameStarted)
        {
            Vector3 desiredPosition = Player.position + runOffset;
            //desiredPosition.x = ;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime);
        }
    }

    public void SnapToPlayerOnStart()
    {
        Vector3 newOffset = new Vector3(0, 0, 1.32f);
        transform.position = Player.position + (runOffset - newOffset);
    }
}
