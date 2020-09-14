using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GroundCheck : MonoBehaviour
{
    public bool _hitingGround = false;
    public bool _inAir = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Ground")
        {
            _hitingGround = true;
            _inAir = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Ground")
        {
            _inAir = true;
            _hitingGround = false;
        }
    }
}
