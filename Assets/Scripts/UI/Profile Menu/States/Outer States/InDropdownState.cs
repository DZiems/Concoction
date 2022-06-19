using System.Linq;
using UnityEngine;

public class InDropdownState : ProfileMenuState
{
    private MyDropdown dropdown;
    public InDropdownState(ProfileMenu profileMenu, Controller controller, UIFiniteStateMachine stateMachine, MyDropdown dropdown) : base(profileMenu, controller, stateMachine)
    {
        this.dropdown = dropdown;

        if (dropdown == null) Debug.LogError("InDropdownState dropdown object is null");
    }

    public override void Enter()
    {
        base.Enter();

        dropdown.Show();
    }

    public override void Exit()
    {
        base.Exit();

        dropdown.Hide();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (controller.VerticalDownPress)
        {
            dropdown.ScrollDown();
        }
        else if (controller.VerticalUpPress)
        {
            dropdown.ScrollUp();
        }
        else if (controller.InteractPress)
        {
            var result = dropdown.Select();
            if (result == MyDropdown.SelectResult.CreateNew)
            {
                stateMachine.ChangeState(profileMenu.createProfileState);
            }
            else
            {
                Debug.Log($"Selected: {profileMenu.CurrentSelectedProfile}");
                stateMachine.ChangeState(profileMenu.mainState);

                //weird case, but calls after mainState.Enter(), which means it works after mainState.innerStateMachine has initialized. This just jumps us to hovering confirm.
                profileMenu.mainState.innerStateMachine.ChangeState(profileMenu.mainState.hoveringConfirmState);
            }
        }
        else if (controller.Slot0Press)
        {
            stateMachine.ChangeState(profileMenu.mainState);
        }
        else if (controller.Slot1Press)
        {
            string deletedItem = dropdown.TryDelete();
            if (!string.IsNullOrEmpty(deletedItem))
            {
                profileMenu.HandleDeleteProfile(deletedItem);
            }
        }
    }

}
