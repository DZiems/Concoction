using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimReticle : MonoBehaviour
{
    //TODO: draw arc of circle when close to max range boundary
    [SerializeField] private float moveSpeed = 20f;
    [SerializeField] private float maxRange = 8f;
    private Controller controller;
    private bool isInitialized;

    private Vector3 direction;

    //track position from player
    private Transform characterTransform;
    private Vector3 deltaFromPlayer;
    private float maxSqRange;
    private float currentSqRange;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        maxSqRange = maxRange * maxRange;
        spriteRenderer = GetComponent<SpriteRenderer>();

        isInitialized = false;
    }

    private void Start()
    {
        CameraTargetGroupAdder.Instance.AddTarget(this.transform, 1f, 2f);
    }

    private void Update()
    {
        if (!isInitialized) return;

        HandleMovement();

    }

    private void HandleMovement()
    {
        direction = Vector3.ClampMagnitude(controller.GetAimDirection(), 1);
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
            transform.position = characterTransform.position + clampedPosition;
        }
    }

    private void UpdateDeltaFromPlayer()
    {
        deltaFromPlayer = transform.position - characterTransform.position;
        currentSqRange =
            Mathf.Pow(deltaFromPlayer.x, 2) +
            Mathf.Pow(deltaFromPlayer.y, 2);
    }

    internal void Initialize(Player player)
    {
        //player
        this.controller = player.Controller;
        this.characterTransform = player.Character.transform;
        gameObject.name = $"(P{player.PlayerNumber}: {player.ProfileData.profileName}) Aim Reticle";
        transform.position = characterTransform.position + Vector3.up;

        //profile data
        spriteRenderer.color = player.ProfileData.aimReticleColor;

        isInitialized = true;
    }

    void OnDrawGizmos()
    {
        if (!isInitialized) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(characterTransform.position, maxRange);
    }
}
