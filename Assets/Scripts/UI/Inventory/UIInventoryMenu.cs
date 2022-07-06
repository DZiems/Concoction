using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryMenu : MonoBehaviour
{
    [SerializeField] private UIInventoryItemTable itemsTable;
    [SerializeField] private UIIngredientDetailsPage ingredientDetailsPage;

    private CanvasGroup canvasGroup;

    private Controller controller;
    private Inventory inventory;

    private const int numPages = 5;
    private const int numItemsInTable = 10;

    private int currentPageIndex;

    private bool HasController => controller != null;

    //states
    private UIFiniteStateMachine stateMachine;
    public UsingInventoryState UsingState { get; private set; }
    public NotUsingInventoryState NotUsingState { get ; private set; }


    private void Awake()
    {
        controller = PlayerManager.Instance.Player.Controller;

        currentPageIndex = 0;
        
        canvasGroup = GetComponent<CanvasGroup>();

        stateMachine = new UIFiniteStateMachine();
        //initialize all ui states
        UsingState = new UsingInventoryState(this, controller, stateMachine);
        NotUsingState = new NotUsingInventoryState(this, controller, stateMachine);
        //initialize state machine
        stateMachine.Initialize(NotUsingState);
    }

    public void Initialize(Inventory inventory)
    {
        this.inventory = inventory;


        itemsTable.Initialize(numItemsInTable);
        inventory.onChanged += BuildPage;
    }

    private void OnDisable()
    {
        inventory.onChanged -= BuildPage;
    }

    private void Update()
    {
        if (HasController)
            stateMachine.CurrentState.LogicUpdate();
    }

    public void Hide()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
    public void Show()
    {
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }


    public void BuildPage()
    {
        Debug.Log("BUILD PAGE CALLED FROM UIINVENTORYMENU");
        itemsTable.Build(inventory.Ingredients.GetRange(0, numItemsInTable));
    }

    public void ScrollUp()
    {
        itemsTable.ScrollUp();
    }
    public void ScrollDown()
    {
        itemsTable.ScrollDown();
    }

    private void DisplayCurrentItemDetails()
    {
        throw new NotImplementedException();
    }
}


public class UsingInventoryState : UIState
{
    private UIInventoryMenu menu;
    private Controller controller;
    public UsingInventoryState(UIInventoryMenu menu, Controller controller, UIFiniteStateMachine stateMachine) : base(stateMachine)
    {
        this.menu = menu;
        this.controller = controller;
    }
    public override void Enter()
    {
        base.Enter();
        menu.Show();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (controller.BackPress)
        {
            stateMachine.ChangeState(menu.NotUsingState);
        }
        if (controller.VerticalUpPress)
        {
            menu.ScrollUp();
        }
        else if (controller.VerticalDownPress)
        {
            menu.ScrollDown();
        }
    }
}

public class NotUsingInventoryState : UIState
{
    private UIInventoryMenu menu;
    private Controller controller;
    public NotUsingInventoryState(UIInventoryMenu menu, Controller controller, UIFiniteStateMachine stateMachine) : base(stateMachine)
    {
        this.menu = menu;
        this.controller = controller;
    }

    public override void Enter()
    {
        base.Enter();
        menu.Hide();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (controller.BackPress)
        {
            stateMachine.ChangeState(menu.UsingState);
        }
    }
}
