using UnityEngine;

public class InCreateProfileState : ProfileMenuState
{
    private AlphabetFieldPrompt alphabetFieldPrompt;
    public InCreateProfileState(ProfileMenu profileMenu, Controller controller, UIFiniteStateMachine stateMachine, AlphabetFieldPrompt alphabetFieldPrompt) : base(profileMenu, controller, stateMachine)
    {
        this.alphabetFieldPrompt = alphabetFieldPrompt;

        if (this.alphabetFieldPrompt == null) Debug.LogError("InCreateProfileState alphabetTextField object is null");
    }

    public override void Enter()
    {
        base.Enter();

        alphabetFieldPrompt.Show();
    }

    public override void Exit()
    {
        base.Exit();

        alphabetFieldPrompt.Hide();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (controller.Slot0Press)
        {
            stateMachine.ChangeState(profileMenu.dropdownState);
        }
        else if (controller.StartPress)
        {
            if(!string.IsNullOrEmpty(alphabetFieldPrompt.CurrentName))
            {
                profileMenu.HandleCreateProfile(alphabetFieldPrompt.CurrentName);
                stateMachine.ChangeState(profileMenu.dropdownState);
            }
            else
            {
                Debug.Log("Cannot confirm an empty profile name!");
            }
        }
        else if (controller.VerticalDownPress)
        {
            alphabetFieldPrompt.ScrollDown();
        }
        else if (controller.VerticalUpPress)
        {
            alphabetFieldPrompt.ScrollUp();
        }
        if (controller.HorizontalLeftPress)
        {
            alphabetFieldPrompt.ScrollLeft();
        }
        else if (controller.HorizontalRightPress)
        {
            alphabetFieldPrompt.ScrollRight();
        }
        else if (controller.InteractPress)
        {
            alphabetFieldPrompt.SelectLetter();
        }
        else if (controller.Slot1Press)
        {
            alphabetFieldPrompt.DeleteLetter();
        }
        else if (controller.SpecialPress || controller.DashPress)
        {
            alphabetFieldPrompt.ShiftCasing();
        }
    }

}