using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    private SpriteRenderer spriteRenderer;
    public Sprite openSprite;
    public Sprite closeSprite;
    public bool isDone;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
        spriteRenderer.sprite = isDone ? openSprite : closeSprite;
    }
    public void TriggerAciton()
    {
        if (!isDone )
        {
            OpenChest();
        }
    }
    private void OpenChest()
    {
        isDone = true;
        spriteRenderer.sprite = openSprite;
        this.gameObject.tag = "Untagged";
    }
}
