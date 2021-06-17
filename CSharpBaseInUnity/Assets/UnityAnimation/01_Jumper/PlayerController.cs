using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    // 유한상태머신
    // - 여러개의 상태중 한개만 가지고 있음.

    // 애니메이터 컨트롤러
    // - 유한 상태 머신을 사용해 재생할 애니메이션을 결정하는 상태도를 표현한 에셋
    // - 애니메이터 컨트롤러는 애셋, 애니메이터는 컴포넌트,

    // - 2D 애니메이션을 사용할땐, 애니메이션을 사용할 오브젝트에
    //   애니메이터, Sprite Renderer 컴포넌트 두개가 다 있어야한다.

    public float jumpForce = 700f;

    private int jumpCount = 0;
    private bool isGrounded = false;
    private bool isDead = false;

    private Rigidbody playerRigidBody2D;
    private Animator animator;

    public void Start()
    {
        playerRigidBody2D = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }


    private void Update()
    {
        if (isDead) return;

        if(Input.GetMouseButtonDown(0) && jumpCount<2)
        {
            jumpCount++;
            playerRigidBody2D.velocity = Vector2.zero;
            playerRigidBody2D.AddForce(new Vector2(0, jumpForce));
        }
        else if(Input.GetMouseButtonUp(0) && playerRigidBody2D.velocity.y > 0)
        {
            playerRigidBody2D.velocity *= 0.5f;
        }

        animator.SetBool("Grounded", isGrounded);
    }


    private void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;
        jumpCount = 0;
    }


    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Collider 에서 isTrigger On 하면 검출됨.
        // 단, 트리거를 활성화 시키면 마찰이 일어나지 않음(관통함)
        Debug.Log("트리거 감지");
    }
}
