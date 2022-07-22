using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryMenu : MonoBehaviour
{
    [SerializeField] private UIInventoryIngredientsTable ingredientsTable;
    [SerializeField] private UIIngredientDetailsPage ingredientDetailsPage;

    private CanvasGroup canvasGroup;

    private Controller controller;
    public Inventory Inventory { get; private set; }

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
        this.Inventory = inventory;
        Debug.Log(inventory.gameObject.name);

        ingredientsTable.Initialize(numItemsInTable);

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
        ingredientsTable.Build(Inventory);
        DisplayCurrentItemDetails();

        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void ScrollUp()
    {
        ingredientsTable.ScrollUp();
        DisplayCurrentItemDetails();
    }
    public void ScrollDown()
    {
        ingredientsTable.ScrollDown();
        DisplayCurrentItemDetails();
    }

    private void DisplayCurrentItemDetails()
    {
        ingredientDetailsPage.DisplayIngredientDetails(ingredientsTable.CurrentHoveredIngredient);
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
