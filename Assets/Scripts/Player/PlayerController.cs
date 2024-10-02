using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    public PlayerInputControl inputControl;
    public PhysicsCheck physicsCheck;
    public Vector2 inputDirection;
    private Rigidbody2D rb;
    private CapsuleCollider2D coll;
    private PlayerAnimation playerAnimation;
    private PlayerCharacter playerCharacter;
    [Header("�������")]
    public float speed;
    public float runSpeed;
    public float slideSpeed;
    //"=>":ÿ������ǰ��ִ��һ��
    private float walkSpeed=>speed/2.5f;
    public float jumpForce;
    public int jumpCount;
    private Vector2 originalSize;
    private Vector2 originalOffset;
    [Header("�������")]
    public PhysicsMaterial2D normal;
    public PhysicsMaterial2D wall;
    [Header("״̬")]
    public bool isCroush;
    public bool isHuet;
    public float hurtForce;
    public bool isDead;
    public bool isAttack;
    public bool canSlide;
    //�������ں���
    private void Awake()
    {
        inputControl = new PlayerInputControl();
        //��ȡ�����������ʽ��
        //1.GetComponent����ȡ����
        //2.public��ק��ͨ�ã���Ϸһ��ʼ�͸�ֵ����ˢ�����׶�ʧ
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        coll = GetComponent<CapsuleCollider2D>();
        playerAnimation = GetComponent<PlayerAnimation>();
        playerCharacter=GetComponent<PlayerCharacter>();
        //+=��ע�ắ����
        //started-����,performed-��ס,cancel-̧��
        inputControl.Gameplay.Jump.started += Jump;
        #region ǿ����·
        runSpeed = speed;
        inputControl.Gameplay.WalkButton.performed += ctx =>
        {
            if (physicsCheck.isGround)
                speed = walkSpeed;
        };
        inputControl.Gameplay.WalkButton.canceled += ctx =>
        {
            if (physicsCheck.isGround)
                speed = runSpeed;
        };
        #endregion
        originalOffset=coll.offset;
        originalSize=coll.size;
        inputControl.Gameplay.Attack.started += PlayerAttack;
        inputControl.Gameplay.Slide.started += Slide;
        canSlide = true;
    }

    private void Slide(InputAction.CallbackContext context)
    {
        if (physicsCheck.isGround && playerCharacter.currentPower>=20 && canSlide)
        {
            playerCharacter.OnPowerCost(20);
            gameObject.layer = 2;
            playerAnimation.PlaySlide();
            speed=slideSpeed;
            canSlide=false;
            StartCoroutine(OnSlide());
        }
    }

    private IEnumerator OnSlide()
    {
        yield return new WaitForSeconds(0.5f);
        canSlide = true;
    }

    private void OnEnable()
    {
        inputControl.Enable();
    }

    private void OnDisable()
    {
        inputControl.Disable();
    }

    private void Update()
    {
        inputDirection = inputControl.Gameplay.Move.ReadValue<Vector2>();

        CheckState();
    }

    private void FixedUpdate()
    {
        if(!isHuet && !isAttack) 
            Move();
    }

    //����
    //private void OnTriggerStay2D(Collider2D other)
    //{
    //    Debug.Log(other.name);
    //}

    public void Move()
    {
        //�����ƶ�
        //ʱ��������1/֡����s��
        if(!isCroush)
            rb.velocity = new Vector2(inputDirection.x*speed*Time.deltaTime, rb.velocity.y);
        
        int faceDir=(int)transform.localScale.x;
        if(inputDirection.x < 0 )
            faceDir = -1;
        if(inputDirection.x > 0 )
            faceDir = 1;
        //���﷭ת
        //��������Render��Filp����
        transform.localScale=new Vector3(faceDir, 1, 1);

        //�¶�
        isCroush=inputDirection.y<-0.5f && physicsCheck.isGround;
        if (isCroush)
        {
            //�޸���ײ����С
            coll.offset = new Vector3(-0.05f, 0.85f);
            coll.size = new Vector3(0.7f, 1.7f);
        }
        else
        {
            //��ԭ��ײ����С
            coll.offset=originalOffset;
            coll.size=originalSize;
        }
    }
    private void Jump(InputAction.CallbackContext context)
    {
        //Debug.Log("JUMP");
        if(jumpCount>0)
        {
            jumpCount--;
            playerAnimation.PlayJump();
            rb.AddForce(transform.up*jumpForce,ForceMode2D.Impulse);
        }
    }

    public void GetHurt(Transform attacker)
    {
        isHuet = true;
        playerAnimation.PlayHurt();
        rb.velocity=Vector2.zero;
        Vector2 dir = new Vector2(transform.position.x-attacker.position.x,0.5f).normalized;
        rb.AddForce(dir*hurtForce,ForceMode2D.Impulse);
    }
    public void PlyaerDead()
    {
        isDead = true;
        inputControl.Gameplay.Disable();
    }
    private void PlayerAttack(InputAction.CallbackContext context)
    {
        isAttack = true;
        playerAnimation.PlayAttack();
    }
    private void CheckState()
    {
        coll.sharedMaterial = physicsCheck.isGround ? normal : wall;
        if (physicsCheck.isGround) { 
            jumpCount=1;
        }
    }
}
