using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimReticle : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float maxDistFromPlayer = 12f;
    public Controller Controller { get; private set; }
    public bool IsInitialized => Controller != null && playerTransform != null;

    private Vector3 direction;
    private Transform playerTransform;
    private float maxSqDistFromPlayer;
    private float sqDistFromPlayer;

    private void Awake()
    {
        maxSqDistFromPlayer = maxDistFromPlayer * maxDistFromPlayer;
    }

    private void Start()
    {
        CameraTargetGroupAdder.Instance.AddTarget(this.transform, 1f, 2f);
    }

    private void Update()
    {
        if (!IsInitialized) return;

        HandleMovement();
    }

    private void HandleMovement()
    {
        direction = Vector3.ClampMagnitude(Controller.GetAimDirection(), 1);
        transform.position += direction * Time.deltaTime * moveSpeed;

        ClampWithinMaxPlayerDist();
    }

    private void ClampWithinMaxPlayerDist()
    {
        //vector pointing in the direction of reticle
        Vector3 delta = transform.position - playerTransform.position;
        sqDistFromPlayer = (delta.x * delta.x) + (delta.y * delta.y);

        if (sqDistFromPlayer > maxSqDistFromPlayer)
        {
            var position = delta;
        }
    }


    internal void Initialize(Controller controller, Transform playerTransform, int playerNumber)
    {
        this.Controller = controller;
        this.playerTransform = playerTransform;
        gameObject.name = $"P{playerNumber} Aim Reticle";
    }

}
