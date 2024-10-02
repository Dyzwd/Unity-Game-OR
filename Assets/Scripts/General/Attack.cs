using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int damage;
    public float attackRange;
    public float attackRate;

    private void OnTriggerStay2D(Collider2D other)
    {
        //other被碰撞的物体,this是碰撞者/攻击者
        //？：有就触发，没有无事
        //PlayerCharacter?
        other.GetComponent<Character>()?.TakeDamage(this);
    }
}
