using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // RigidBody
    // Angular Drag : 이값을 높이면 물체가 잘회전하지 못함.

    // Animation Controller
    // - Layers를 활용하면 유한 상태머신을 병렬로 처리할 수 있음
    //   (여러개의 상태를 동시에 가질 수 있음)
    // - Layers는 에니메이터 UI에 나와 있는 순서대로 중첩됨.
    // - 애니메이터의 레이어별로 부위를 다르게 적용하려면 아마타 마스크가 필요함.

    // - Blend Tree를 활용하면 가중치에 따라 애니메이션을 부드럽게 처리 할 수 있음.



    public float moveSpeed = 5f;
    public float rotateSpeed = 180f;

    private PlayerInput _playerInput;
    private Rigidbody   _playerRigidbody;
    private Animator    _playerAnimater;

    private void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _playerRigidbody = GetComponent<Rigidbody>();
        _playerAnimater = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Rotate();
        Move();

        _playerAnimater.SetFloat("Move", _playerInput.move);
    }

    private void Move()
    {
        Vector3 moveDistance;
    }

    private void Rotate()
    {

    }
}
