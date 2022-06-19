using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimReticle : MonoBehaviour
{
    //TODO: draw arc of circle when close to max range boundary
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float boostedMoveSpeed = 20f;

    [Header("Camera")]
    [SerializeField] private float cameraRadius = 2f;


    private Controller controller;
    private bool isInitialized;

    private Vector3 direction;

    //track position from player
    private PlayerCharacter character;
    private Vector3 deltaFromPlayer;
    private float maxSqRange;
    private float currentSqRange;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        isInitialized = false;
    }

    private void Start()
    {
        CameraTargetGroupAdder.Instance.AddTarget(this.transform, 1f, cameraRadius);
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
            var clampedPosition = deltaFromPlayer.normalized * character.AimReticleRange;
            transform.position = character.transform.position + clampedPosition;
        }
    }

    private void UpdateDeltaFromPlayer()
    {
        deltaFromPlayer = transform.position - character.transform.position;
        currentSqRange =
            Mathf.Pow(deltaFromPlayer.x, 2) +
            Mathf.Pow(deltaFromPlayer.y, 2);
    }

    internal void Initialize(Player player)
    {
        //player
        this.controller = player.Controller;
        this.character = player.Character;
        gameObject.name = $"({player.profileData.profileName}) Aim Reticle";
        transform.position = character.transform.position + Vector3.up;

        //cached values
        maxSqRange = character.AimReticleRange * character.AimReticleRange;

        //profile data
        spriteRenderer.color = player.profileData.playerCharacterData.aimReticleColor;

        isInitialized = true;
    }

}
