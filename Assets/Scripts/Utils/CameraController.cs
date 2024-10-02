using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

//获取边界、清楚缓存
public class CameraController : MonoBehaviour
{
    private CinemachineConfiner2D confiner2D;
    public CinemachineImpulseSource impulseSource;

    public void OnCameraShakeEvent()
    {
        impulseSource.GenerateImpulse();
    }

    private void Awake()
    {
        confiner2D=GetComponent<CinemachineConfiner2D>();
    }

    private void Start()
    {
        GetNewCameraBounds();
    }

    private void GetNewCameraBounds()
    {
        var obj = GameObject.FindGameObjectWithTag("Bounds");
        if (obj == null)
            return;
        confiner2D.m_BoundingShape2D = obj.GetComponent<Collider2D>();
        confiner2D.InvalidateCache();
    }
}
