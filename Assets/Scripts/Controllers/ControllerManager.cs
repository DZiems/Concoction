using System;
using UnityEngine;

public class ControllerManager : MonoBehaviour
{
    public static ControllerManager Instance { get; private set; }

    Controller controller;


    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Found more than one ControllerManager in the scene. Destroying the newest one");
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);

        controller = GetComponentInChildren<Controller>();
    }


    private void Update()
    {
        HandleUnassignedControllers();
    }

    private void HandleUnassignedControllers()
    {
        if (ControllerNeedsAssign())
            AssignController();
    }

    private bool ControllerNeedsAssign()
    {
        return !controller.IsAssigned && controller.AnyButtonDown();
    }

    private void AssignController()
    {
        controller.IsAssigned = true;

        Debug.Log($"ControllerManager: assigning new player Controller {controller.Id}");

        PlayerManager.Instance.AssignController(controller);
    }

    internal void UnassignController()
    {
        Debug.Log($"Unassigning Controller.");
        controller.IsAssigned = false;
    }
}
