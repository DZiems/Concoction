using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPressAnyPrompt : MonoBehaviour
{
    private UIHoverableText button;
    bool isGameStart;

    private void Awake()
    {
        button = GetComponent<UIHoverableText>();
    }
    private void OnEnable()
    {
        isGameStart = true;

        if (PlayerManager.Instance != null)
        {
            isGameStart = false;
            PlayerManager.Instance.onPlayerJoined += Disappear;
        }
    }
    private void Start()
    {
        button.Hover();

        if (isGameStart)
        {
            PlayerManager.Instance.onPlayerJoined += Disappear;
            isGameStart = false;
        }
    }

    private void OnDisable()
    {
        PlayerManager.Instance.onPlayerJoined -= Disappear;
    }

    private void Disappear()
    {
        gameObject.SetActive(false);
    }
}
