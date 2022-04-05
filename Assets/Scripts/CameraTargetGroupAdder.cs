using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTargetGroupAdder : MonoBehaviour
{
    public static CameraTargetGroupAdder Instance;

    [SerializeField] float defaultTargetRadius = 10f;
    [SerializeField] float defaultTargetWeight = 1f;

    private CinemachineTargetGroup targetGroup;


    private void Awake()
    {
        Instance = this;
        targetGroup = GetComponent<CinemachineTargetGroup>();
    }

    public void AddTarget(Transform target)
    {
        targetGroup.AddMember(target, defaultTargetWeight, defaultTargetRadius);
    }
    public void AddTarget(Transform target, float weight, float radius)
    {
        targetGroup.AddMember(target, weight, radius);
    }
}
