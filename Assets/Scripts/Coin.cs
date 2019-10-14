using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private Animator _anim;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _anim.SetTrigger("Spawn");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponent<BoxCollider>().enabled = false;
            GameManager.Instance.GetCoin();
            _anim.SetTrigger("Collected");
            //Destroy(transform.parent.gameObject, 2f);  //p13 21.25
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.collider.CompareTag("Player"))
    //    {
    //        gameObject.GetComponent<BoxCollider>().isTrigger = true;
    //        GameManager.Instance.GetCoin();
    //        _anim.SetTrigger("Collected");
    //        Debug.LogWarning("Collected the fuckin coin");
    //    }
    //}

    //private void OnControllerColliderHit(ControllerColliderHit hit)
    //{
    //    if (hit.collider.CompareTag("Player"))
    //    {
    //        //GetComponent<BoxCollider>().enabled = false;
    //        GameManager.Instance.GetCoin();
    //        _anim.SetTrigger("Collected");
    //    }
    //}
}
