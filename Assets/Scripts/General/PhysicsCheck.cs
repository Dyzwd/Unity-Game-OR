using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    [Header("������")]
    //public bool manual;
    public float checkRaduis;
    public Vector2 bottomOffset;
    public Vector2 frontOffset;
    public Vector2 frontBottomOffset;
    public LayerMask groundLayer;
    private CapsuleCollider2D coll;
    [Header("״̬")]
    public bool isGround;
    public bool touchWall;
    public bool touchCliff;
    private void Awake()
    {
        coll = GetComponent<CapsuleCollider2D>();
    }
    void Update()
    {
        Check();
    }

    public void Check()
    {
        //������
        isGround = Physics2D.OverlapCircle((Vector2)transform.position+bottomOffset, checkRaduis, groundLayer);

        // ǽ����
        touchWall = Physics2D.OverlapCircle((Vector2)transform.position + frontOffset, checkRaduis, groundLayer);

        //���¼��
        touchCliff = Physics2D.OverlapCircle((Vector2)transform.position + frontBottomOffset, checkRaduis, groundLayer);
    }
    //���Ƶ���������
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, checkRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + frontOffset, checkRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + frontBottomOffset, checkRaduis);
    }
}
