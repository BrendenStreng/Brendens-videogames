using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerCharacterAnimator : MonoBehaviour
{

    [SerializeField] ThirdPersonMovement _thirdPersonMovement = null;
    [SerializeField] Health _health = null;
    [SerializeField] Player _player = null;

    //names align with animator nodes
    const string IdleState = "Idle";
    const string RunState = "Run";
    const string JumpState = "Jumping";
    const string FallState = "Falling";
    const string LandState = "FallingToLanding";
    const string SprintState = "Sprinting";
    const string PushState = "PushAbilityActivate";
    const string DyingState = "Dying";

    Animator _animator = null;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void OnIdle()
    {
        _animator.CrossFadeInFixedTime(IdleState, .2f);
    }

    private void OnStartRunning()
    {
        _animator.CrossFadeInFixedTime(RunState, .2f);
    }

    private void OnSprinting()
    {
        _animator.CrossFadeInFixedTime(SprintState, .2f);
    }

    private void OnJump()
    {
        _animator.CrossFadeInFixedTime(JumpState, .2f);
    }

    private void OnFalling()
    {
        _animator.CrossFadeInFixedTime(FallState, .2f);
    }

    private void OnLanding()
    {
        _animator.CrossFadeInFixedTime(LandState, .2f);
    }

    private void OnPush()
    {
        _animator.CrossFadeInFixedTime(PushState, .2f);
    }

    private void OnDeath()
    {
        _animator.CrossFadeInFixedTime(DyingState, .2f);
    }

    private void OnEnable()
    {
        _thirdPersonMovement.Idle += OnIdle;
        _thirdPersonMovement.StartRunning += OnStartRunning;
        _thirdPersonMovement.StartJump += OnJump;
        _thirdPersonMovement.Falling += OnFalling;
        _thirdPersonMovement.Landing += OnLanding;
        _thirdPersonMovement.Sprinting += OnSprinting;
        _player.Pushing += OnPush;
        _health.Dying += OnDeath;
    }

    private void OnDisable()
    {
        _thirdPersonMovement.Idle -= OnIdle;
        _thirdPersonMovement.StartRunning -= OnStartRunning;
        _thirdPersonMovement.StartJump -= OnJump;
        _thirdPersonMovement.Falling -= OnFalling;
        _thirdPersonMovement.Landing -= OnLanding;
        _thirdPersonMovement.Sprinting -= OnSprinting;
        _player.Pushing -= OnPush;
        _health.Dying -= OnDeath;

    }
}
