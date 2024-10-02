using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    [Header("��������")]
    public float maxHealth;
    public float currentHealth;


    [Header("�����޵�")]
    public float invulnerableDuration;
    private float invulnerableCounter;
    public bool isInvulnerable;
    public UnityEvent<Transform> OnTakeDamage;
    public UnityEvent OnDie;

    private void Start()
    {
        currentHealth=maxHealth;
    }
    virtual protected void Update()
    {
        if (isInvulnerable) { 
            invulnerableCounter-=Time.deltaTime;
            if (invulnerableCounter <= 0) { 
                isInvulnerable = false;
            }
        }
    }
    virtual public void TakeDamage(Attack attack)
    {
        if (isInvulnerable) 
            return;
        //Debug.Log(attack.damage);
        if (currentHealth - attack.damage > 0)
        {
            currentHealth-=attack.damage;
            TriggerInvulnerable();
            //ִ������
            OnTakeDamage?.Invoke(attack.transform);
        }
        else
        {
            currentHealth = 0;
            //��������
            OnDie?.Invoke();
        }
    }
    protected void TriggerInvulnerable()
    {
        if (!isInvulnerable)
        {
            isInvulnerable = true;
            invulnerableCounter = invulnerableDuration;
        }
    }

}
