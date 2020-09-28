using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushHitBox : MonoBehaviour
{
    [SerializeField] float hPushForce = .05f;
    [SerializeField] float vPushForce = .05f;
    [SerializeField] GameObject player;

    private float pushForceMultiplier;

    Transform playerPosition;

    //direction of push
    Vector3 direction;

    private void Start()
    {
        player = GameObject.Find("Third_Person_Player");
    }

    //on collision with interactable objects apply a force to those obje
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Interactable")
        {
            Rigidbody _rb = collision.gameObject.GetComponent<Rigidbody>();

            playerPosition = player.GetComponent<Transform>();
            ThirdPersonMovement playerController = player.GetComponent<ThirdPersonMovement>();

            //calculate the direction of the pushc
            direction = collision.gameObject.transform.position - playerPosition.position;

            _rb.AddForce(collision.gameObject.transform.up.normalized * vPushForce * playerController.currentPushStrength);
            _rb.AddRelativeForce(direction.normalized * hPushForce * playerController.currentPushStrength);
        }
    }
}
