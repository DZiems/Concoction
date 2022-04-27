using System;
using UnityEngine;

public class ControllerManager : MonoBehaviour
{
    public static ControllerManager Instance { get; private set; }
    private Controller[] controllers;


    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Found more than one ControllerManager in the scene. Destroying the newest one");
            Destroy(gameObject);
            return;
        }

        Instance = this;

        controllers = GetComponentsInChildren<Controller>();

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        var index = 1;
        foreach (var controller in controllers)
        {
            controller.SetId(index);
            index++;
        }
    }

    private void Update()
    {
        HandleUnassignedControllers();
    }

    private void HandleUnassignedControllers()
    {
        foreach (var controller in controllers)
            if (ControllerNeedsAssign(controller))
                AssignController(controller);
    }

    private bool ControllerNeedsAssign(Controller controller)
    {
        return !controller.IsAssigned && controller.AnyButtonDown();
    }

    private void AssignController(Controller controller)
    {
        controller.IsAssigned = true;

        Debug.Log($"ControllerManager: assigning new player Controller {controller.Id}");

        PlayerManager.Instance.AssignController(controller);
    }

    internal void UnassignController(int id)
    {
        int index = id - 1;
        Debug.Log($"Unassigning Controller: {controllers[index].gameObject.name}");
        controllers[index].IsAssigned = false;
    }
}
