using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ControllerManager : MonoBehaviour
{
    private Controller[] _controllers;
    private bool _canAssignController; //make this a property of GameManager, which can track whether on an adventure, or in combat, or something

    private void Awake()
    {
        _canAssignController = true;
        _controllers = FindObjectsOfType<Controller>();
    }

    private void Start()
    {
        var index = 1;
        foreach (var controller in _controllers)
        {
            controller.SetIndex(index);
            index++;
        }
    }

    private void Update()
    {
        if (_canAssignController)
            HandleUnassignedControllers();
    }

    private void HandleUnassignedControllers()
    {
        foreach (var controller in _controllers)
        {
            if (ControllerShouldBeAssigned(controller))
            {
                AssignController(controller);
            }
        }

    }

    private bool ControllerShouldBeAssigned(Controller controller)
    {
        return !controller.IsAssigned && controller.InteractDown;
    }

    private void AssignController(Controller controller)
    {
        controller.IsAssigned = true;

        PlayerManager.Instance.AddPlayerToGame(controller);
    }
}
