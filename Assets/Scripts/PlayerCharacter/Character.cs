using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Aim Reticle")]
    [SerializeField] private float aimReticleRange = 7f;

    [Header("Camera")]
    [SerializeField] private float cameraRadius = 10f;

    [Header("References")]
    [SerializeField] private SpriteRenderer blushSpriteRend;
    [SerializeField] private SpriteRenderer faceSpriteRend;
    [SerializeField] private SpriteRenderer feetSpriteRend;
    [SerializeField] private SpriteRenderer goggleFrameSpriteRend;
    [SerializeField] private SpriteRenderer goggleLensSpriteRend;
    [SerializeField] private SpriteRenderer handsSpriteRend;
    [SerializeField] private SpriteRenderer robesSpriteRend;


    public Controller Controller { get; private set; }

    public string ProfileName { get; set; }
    public float AimReticleRange => aimReticleRange;

    private Vector3 direction;

    private Rigidbody2D rb2D;
    private Animator animator;
    public Health Health { get; private set; }


    private bool isInitialized;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        Health = GetComponent<Health>();

        isInitialized = false;
    }



    private void Start()
    {
        CameraTargetGroupAdder.Instance.AddTarget(this.transform, 1f, cameraRadius);
    }

    private void OnEnable()
    {
        Health.onDamaged += TakeDamage;
        Health.onComboed += TakeCombo;
        Health.onKilled += Kill;
    }

    private void TakeDamage()
    {
        Debug.Log("Do something");
    }
    private void TakeCombo()
    {

        Debug.Log("Do something unfortunate");
    }
    private void Kill()
    {
        Debug.Log("Do something mega unfortunate");
    }

    private void FixedUpdate()
    {
        if (!isInitialized) return;

        HandleMovement();
    }

    private void Update()
    {
        if (!isInitialized) return;
        HandleAnimation();
    }

    private void HandleMovement()
    {
        direction = Vector3.ClampMagnitude(Controller.GetDirection(), 1);
        rb2D.MovePosition(transform.position + (direction * Time.deltaTime * moveSpeed));
    }

    private void HandleAnimation()
    {
        if (direction.magnitude != 0)
        {
            setSpritesFlipX(direction.x < 0);
            animator.SetFloat("Direction", Vector2.Dot(direction.normalized, transform.up));
        }
        animator.SetFloat("Speed", direction.magnitude);
    }

    internal void Initialize(Player player)
    {
        this.Controller = player.Controller;
        gameObject.name = $"(P{player.PlayerNumber}: {player.ProfileData.profileName}) Character";

        robesSpriteRend.color = player.ProfileData.robesColor;

        isInitialized = true;
    }

    private void setSpritesFlipX(bool on)
    {
        blushSpriteRend.flipX = on;
        faceSpriteRend.flipX = on;
        feetSpriteRend.flipX = on;
        goggleFrameSpriteRend.flipX = on;
        goggleLensSpriteRend.flipX = on;
        handsSpriteRend.flipX = on;
        robesSpriteRend.flipX = on;
    }
}
