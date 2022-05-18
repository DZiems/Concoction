using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class CircularStrikeZone : MonoBehaviour
{

    private Animator animator;

    private bool strikeFlag;
    private int damage;



    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        if (animator == null) Debug.LogError("strike zone animator null");
        strikeFlag = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!strikeFlag) 
            return;

        Character character = collision.GetComponent<Character>();

        if (character == null)
            return;

        character.Health.TakeDamage(damage);

        strikeFlag = false;
        Debug.Log($"Strike flag: {strikeFlag}, from trigger enter");
    }

    //need to wait 2 frames for some reason
    private IEnumerator StopStrike()
    {
        yield return null;
        yield return null;
        strikeFlag = false;
        Debug.Log($"Strike flag: {strikeFlag}, from stop coroutine");
    }

    public void Initialize(float diameter, int damage)
    {
        transform.localScale = new Vector3(diameter, diameter, 1f);
        this.damage = damage;

        gameObject.SetActive(false);
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    private void OnAnimFinishHide()
    {
        gameObject.SetActive(false);
    }

    public void Indicate()
    {
        gameObject.SetActive(true);
        animator.SetTrigger("Indicate");
    }

    public void Strike()
    {
        strikeFlag = true;
        gameObject.SetActive(true);
        animator.SetTrigger("Strike");
        Debug.Log($"Strike flag: {strikeFlag}, from strike");

        StartCoroutine(StopStrike());
    }


}
