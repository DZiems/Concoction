using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimReticle : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 20f;
    [SerializeField] private float maxRange = 8f;
    public Controller Controller { get; private set; }
    public bool IsInitialized => Controller != null && playerTransform != null;

    private Vector3 direction;
    //track position from player
    private Transform playerTransform;
    private Vector3 deltaFromPlayer;
    private float maxSqRange;
    private float currentSqRange;

    private void Awake()
    {
        maxSqRange = maxRange * maxRange;
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

    //if outside range of maxDist, clamp to maxDist away
    private void ClampWithinMaxPlayerDist()
    {
        UpdateDeltaFromPlayer();
        if (currentSqRange > maxSqRange)
        {
            var clampedPosition = deltaFromPlayer.normalized * maxRange;
            transform.position = playerTransform.position + clampedPosition;
        }
    }

    private void UpdateDeltaFromPlayer()
    {
        deltaFromPlayer = transform.position - playerTransform.position;
        currentSqRange =
            Mathf.Pow(deltaFromPlayer.x, 2) +
            Mathf.Pow(deltaFromPlayer.y, 2);
    }

    internal void Initialize(Controller controller, Transform playerTransform, int playerNumber)
    {
        this.Controller = controller;
        this.playerTransform = playerTransform;
        gameObject.name = $"P{playerNumber} Aim Reticle";
        transform.position = playerTransform.position + Vector3.up;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(playerTransform.position, maxRange);
    }
}
