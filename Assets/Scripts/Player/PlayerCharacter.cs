using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
    [Header("玩家特有")]
    public float maxPower;
    public float currentPower;
    public float autoPowerAddSpeed;
    public GameObject playerStateBar;
    public GameObject cimera;
    private void Awake()
    {
        currentPower=maxPower;
    }
    protected override void Update()
    {
        base.Update();
        if (currentPower < maxPower)
        {
            currentPower += Time.deltaTime*autoPowerAddSpeed;
            playerStateBar.GetComponent<PlayerStateBar>().OnPowerChange(currentPower / maxPower);
        }
    }
    public override void TakeDamage(Attack attack)
    {
        if (isInvulnerable)
            return;
        //Debug.Log(attack.damage);
        if (currentHealth - attack.damage > 0)
        {
            currentHealth -= attack.damage;
            TriggerInvulnerable();
            //执行受伤
            OnTakeDamage?.Invoke(attack.transform);
            cimera.GetComponent<CameraController>().OnCameraShakeEvent();
        }
        else
        {
            currentHealth = 0;
            //触发死亡
            OnDie?.Invoke();
        }
        playerStateBar.GetComponent<PlayerStateBar>().OnHealthChange(currentHealth / maxHealth);
    }
    public void OnPowerCost(float power)
    {
        currentPower -= power;
        playerStateBar.GetComponent<PlayerStateBar>().OnPowerChange(currentPower / maxPower);
    }
    public void OnPowerAdd(float power)
    {
        currentPower += power;
        playerStateBar.GetComponent<PlayerStateBar>().OnPowerChange(currentPower / maxPower);
    }
}
