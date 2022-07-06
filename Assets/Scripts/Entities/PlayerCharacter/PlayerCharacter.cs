using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Entity
{
    [Header("Aim Reticle")]
    [SerializeField] private float aimReticleRange = 7f;

    [Header("Camera")]
    [SerializeField] private float cameraRadius = 10f;

    [Header("References")]
    [SerializeField] private SpriteRenderer blushSpRend;
    [SerializeField] private SpriteRenderer faceSpRend;
    [SerializeField] private SpriteRenderer feetSpRend;
    [SerializeField] private SpriteRenderer goggleFrameSpRend;
    [SerializeField] private SpriteRenderer goggleLensSpRend;
    [SerializeField] private SpriteRenderer handsSpRend;
    [SerializeField] private SpriteRenderer robesSpRend;

    //states
    public PlayerCharacterMoveState moveState { get; private set; }
    public PlayerCharacterIdleState idleState { get; private set; }

    //references
    public Controller Controller { get; private set; }

    public float AimReticleRange => aimReticleRange;


    public Rigidbody2D Rb2D { get; private set; }
    private bool isInitialized;

    public void Initialize(Player player)
    {
        Controller = player.Controller;

        if (!player.HasProfile)
        {
            gameObject.name = $"(TEST MODE) Character";
            isInitialized = true;
            return;
        }

        gameObject.name = $"({player.profileData.profileName}) Character";

        robesSpRend.color = player.profileData.playerCharacterData.robesColor;
        faceSpRend.color = player.profileData.playerCharacterData.skinColor;
        goggleLensSpRend.color = player.profileData.playerCharacterData.gogglesLensColor;

        isInitialized = true;
    }


    protected override void Awake()
    {
        base.Awake();

        Rb2D = GetComponent<Rigidbody2D>();
        if (Rb2D == null) Debug.LogError($"{gameObject.name} rigidbody2D is null!");

        moveState = new PlayerCharacterMoveState(this, stateMachine);
        idleState = new PlayerCharacterIdleState(this, stateMachine);

        isInitialized = false;

    }


    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);

        CameraTargetGroupAdder.Instance.AddTarget(this.transform, 1f, cameraRadius);
    }


    protected override void FixedUpdate()
    {
        if (!isInitialized) return;

        base.FixedUpdate();
    }

    protected override void Update()
    {
        if (!isInitialized) return;

        base.Update();
    }

    public void SetVelocity(float speed)
    {
        Rb2D.velocity = FacingDirection * speed;
    }



    public void SetSpritesFlipX(bool on)
    {
        blushSpRend.flipX = on;
        faceSpRend.flipX = on;
        feetSpRend.flipX = on;
        goggleFrameSpRend.flipX = on;
        goggleLensSpRend.flipX = on;
        handsSpRend.flipX = on;
        robesSpRend.flipX = on;
    }
}
