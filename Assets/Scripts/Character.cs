using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private AimReticle aimReticlePrefab;
    [SerializeField] private float moveSpeed = 5f;
    public Controller Controller { get; private set; }
    public int PlayerNumber { get; private set; }
    public bool HasController => Controller != null;

    private Vector3 direction;

    private Animator animator;
    private SpriteRenderer spriteRenderer;


    private Rigidbody2D rb2D;
    private AimReticle aimReticle;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();

        if (aimReticlePrefab == null)
        {
            Debug.LogError("Player Awake(): Aim reticle prefab is unassigned!");
            return;
        }

        aimReticle = Instantiate(aimReticlePrefab);
        aimReticle.Initialize(this.Controller, this.transform, PlayerNumber);
    }

    private void Start()
    {
        CameraTargetGroupAdder.Instance.AddTarget(this.transform, 1f, 5f);

    }

    private void FixedUpdate()
    {
        if (!HasController) return;

        HandleMovement();
    }

    private void Update()
    {
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
            spriteRenderer.flipX = direction.x < 0;
            animator.SetFloat("Direction", Vector2.Dot(direction.normalized, transform.up));
        }
        animator.SetFloat("Speed", direction.magnitude);
    }

    internal void SetController(Controller controller)
    {
        this.Controller = controller;
    }
    internal void SetPlayerNumber(int playerNumber)
    {
        PlayerNumber = playerNumber;
    }
}