using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    [SerializeField] AbilityLoadout _abilityLoadout;
    [SerializeField] Ability _startingAbility;

    public event Action Pushing = delegate { };

    public Transform CurrentTarget { get; private set; }


    private void Awake()
    {
        if(_startingAbility != null)
        {
            _abilityLoadout?.EquipAbility(_startingAbility);
        }
        SetTarget(transform);
    }

    private void Update()
    {
        //TODO in reality, Inputs would be detected elsewhere
        // and passed into the player class. We're doing it here
        // for simplification of example
        if (Input.GetMouseButtonDown(0))
        {
            Pushing?.Invoke();
            _abilityLoadout.UseEquippedAbility(CurrentTarget);
        }
    }

    public void SetTarget(Transform newTarget)
    {
        CurrentTarget = newTarget;
    }
}
