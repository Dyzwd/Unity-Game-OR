using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;

public class Sign : MonoBehaviour
{
    private bool canPress;
    public GameObject signSprite;
    public Transform playerTransform;
    public PlayerInputControl inputControl;
    private IInteractable targetItem;
    private void Awake()
    {
        inputControl = new PlayerInputControl();
        inputControl.Enable();
        inputControl.Gameplay.Confirm.started += OnComfirm;
    }
    private void OnEnable()
    {
        inputControl.Enable();
    }

    private void OnDisable()
    {
        inputControl.Disable();
    }

    private void OnComfirm(InputAction.CallbackContext context)
    {
        if (canPress)
        {
            targetItem.TriggerAciton();
        }
    }

    private void Update()
    {
        signSprite.SetActive(canPress);
        signSprite.transform.localScale=playerTransform.localScale;
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Interactable"))
        {
            canPress = true;
            targetItem=other.GetComponent<IInteractable>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        canPress = false;
    }
}
