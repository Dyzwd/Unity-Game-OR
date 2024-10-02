using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Processors;

public class Enemy : MonoBehaviour
{
    [HideInInspector]public Rigidbody2D rb;
    [HideInInspector] public Animator anim;
    [HideInInspector] public PhysicsCheck physicsCheck;
    public GameObject Player;
    [Header("基本参数")]
    public float normalSpeed;
    public float chaseSpeed;
    [HideInInspector] public float currentSpeed;

    [Header("撞墙休息")]
    public Vector3 faceDir;
    public float waitTime;
    public float waitCounter;
    public bool wait;
    [Header("受击")]
    public bool isHurt;
    public float hurtForce;
    public bool isDead;
    public float powerRewards;

    [Header("检测")]
    public Vector2 centerOffset;
    public Vector2 checkSize;
    public float checkDistance;
    public LayerMask attackLayer;

    [Header("追击")]
    public float lostTimeCounter;
    public float lostTime;
    public bool isRun;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        physicsCheck = GetComponent<PhysicsCheck>();
 
        currentSpeed=normalSpeed;
        waitCounter = waitTime;
        anim.SetBool("isWalk", true);
    }
    private void Update()
    {
        faceDir = new Vector3(-transform.localScale.x, 0);
        //碰到悬崖或墙壁
        if ((physicsCheck.touchWall || !physicsCheck.touchCliff) && !wait && !isRun && physicsCheck.isGround && !isHurt)
        {
            wait = true;
            anim.SetBool("isWalk", false);
        }
        //发现敌人
        if (FoundPlayer())
        {
            wait=false;
            isRun = true;
            currentSpeed=chaseSpeed;
            lostTimeCounter = lostTime;
            anim.SetBool("isRun", true);
        }
        if (isRun)
        {
            if ((physicsCheck.touchWall || !physicsCheck.touchCliff) && physicsCheck.isGround && !isHurt)
            {
                physicsCheck.frontOffset.x = -physicsCheck.frontOffset.x;
                physicsCheck.bottomOffset.x = -physicsCheck.bottomOffset.x;
                physicsCheck.frontBottomOffset.x = -physicsCheck.frontBottomOffset.x;
                transform.localScale = new Vector3(faceDir.x, 1, 1);
            }
        }
        TimeCounter();
    }
    private void FixedUpdate()
    {
        if(!wait && !isHurt && !isDead && physicsCheck.isGround)
            Move();
    }
    protected virtual void Move()
    {
        rb.velocity = new Vector2(currentSpeed * faceDir.x * Time.deltaTime, rb.velocity.y);
    }
    public void TimeCounter()
    {
        if (wait)
        {
            waitCounter -= Time.deltaTime;
            if (waitCounter <= 0)
            {
                wait = false;
                anim.SetBool("isWalk", true);
                waitCounter = waitTime;
                physicsCheck.frontOffset.x = -physicsCheck.frontOffset.x;
                physicsCheck.bottomOffset.x = -physicsCheck.bottomOffset.x;
                physicsCheck.frontBottomOffset.x = -physicsCheck.frontBottomOffset.x;
                transform.localScale = new Vector3(faceDir.x, 1, 1);
            }
        }
        if (!FoundPlayer() && isRun)
        {
            lostTimeCounter-= Time.deltaTime;
            if(lostTimeCounter <= 0)
            {
                isRun=false;
                currentSpeed=normalSpeed;
                anim.SetBool("isWalk", true);
                anim.SetBool("isRun", false);
            }
        }
    }
    public void OnTakeDamage(Transform attacker)
    {
        Player.GetComponent<PlayerCharacter>().OnPowerAdd(powerRewards);
        //转身
        if (attacker.position.x - transform.position.x > 0)
            transform.localScale=new Vector3(-1,1,1);
        if (attacker.position.x - transform.position.x < 0)
            transform.localScale = new Vector3(1, 1, 1);

        //受伤击退
        isHurt = true;
        anim.SetTrigger("hurt");
        Vector2 dir=new Vector2(transform.position.x -attacker.position.x,0.5f).normalized;
        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
        physicsCheck.frontOffset.x = Mathf.Abs(physicsCheck.frontOffset.x)*-transform.localScale.x;
        physicsCheck.bottomOffset.x = Mathf.Abs(physicsCheck.bottomOffset.x) * -transform.localScale.x;
        physicsCheck.frontBottomOffset.x = Mathf.Abs(physicsCheck.frontBottomOffset.x) *-transform.localScale.x;
        //StartCoroutine(OnHurt(dir));
    }

    public void hrutFinish()
    {

    }
    //private IEnumerator OnHurt(Vector2 dir)
    //{
    //    rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
    //    gameObject.layer = 2;
    //    //携程-延时器
    //    yield return new WaitForSeconds(0.5f);
    //    isHurt=false;
    //    gameObject.layer = 8;
    //}
    public void OnDie()
    {
        gameObject.layer = 2;
        anim.SetBool("isDead", true);
        isDead = true;
    }
    public void DestoryAfterAnimation()
    {
        Destroy(this.gameObject);
    }

    public bool FoundPlayer()
    {
        return Physics2D.BoxCast(transform.position+(Vector3)centerOffset,checkSize,0,faceDir,checkDistance,attackLayer);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + (Vector3)centerOffset + new Vector3(checkDistance*-transform.localScale.x, 0, 0),0.2f);
    }
}
