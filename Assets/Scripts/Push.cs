using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Push : Ability
{
    [SerializeField] GameObject _pushHitBox;
    [SerializeField] AudioClip _pushSound;
    


    public override void Use(Transform origin, Transform target)
    {
        Debug.Log("Force Push");
        AudioHelper.PlayClip2D(_pushSound, 0.15f);
        GameObject pushBox = Instantiate(_pushHitBox, origin.position, origin.rotation);

        Destroy(pushBox, 1f);
    }
}
