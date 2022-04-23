using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPressAnyPrompt : MonoBehaviour
{
    bool subscribeInStart;
    private void OnEnable()
    {
        subscribeInStart = true;
        GetComponent<Animator>().SetBool("IsHovered", true);
        if (PlayerManager.Instance != null)
        {
            subscribeInStart = false;
            PlayerManager.Instance.onPlayerOneJoined += Disappear;
        }
    }
    private void Start()
    {
        if (subscribeInStart)
        {
            PlayerManager.Instance.onPlayerOneJoined += Disappear;
            subscribeInStart = false;
        }
    }

    private void OnDisable()
    {
        PlayerManager.Instance.onPlayerOneJoined -= Disappear;
    }

    private void Disappear()
    {
        gameObject.SetActive(false);
    }
}
