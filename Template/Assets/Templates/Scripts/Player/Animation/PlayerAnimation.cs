using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public enum PLAYER_STATE
{
    PLAYER_IDLE = 0,
    PLAYER_RUN,
    PLAYER_STOP,
    PLAYER_JUMP
}

[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    [HideInInspector] public Animator _animator;

    PlayerMovement _movement;

    PLAYER_STATE _current_state;

    void Awake()
    {
        _animator = this.GetComponent<Animator>();

        if (this.GetComponent<PlayerMovement>() != null)
            _movement = this.GetComponent<PlayerMovement>();

        _movement.OnMovement += ChangePlayerState;
    }

    // Start is called before the first frame update
    void Start()
    {
        //SET STATE TO IDLE
        ChangePlayerState(0);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ChangePlayerState(int state)
    {
        PLAYER_STATE _change_state = (PLAYER_STATE)Enum.ToObject(typeof(PLAYER_STATE), state);

        if (_current_state == _change_state) 
            return;

        _current_state = _change_state;

        _animator.Play(_change_state.ToString().ToLower());
    }
}
