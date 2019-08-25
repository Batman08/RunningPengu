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
        if (other.tag == "Player")
        {
            GameManager.Instance.GetCoin();
            _anim.SetTrigger("Collected");
            //Destroy(transform.parent.gameObject, 2f);  p13 21.25
        }
    }
}
