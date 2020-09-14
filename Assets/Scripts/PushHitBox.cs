using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushHitBox : MonoBehaviour
{
    [SerializeField] float hPushForce = 10f;
    [SerializeField] float vPushForce = 10f;

    //on collision with interactable objects apply a force to those obje
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Interactable")
        {
            Rigidbody _rb = collision.gameObject.GetComponent<Rigidbody>();

         
            _rb.AddForce(collision.gameObject.transform.forward * hPushForce);
            _rb.AddForce(collision.gameObject.transform.up * vPushForce);
        }
    }
}
