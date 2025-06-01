using Godot;

public partial class PlayerTurnState : State
{
    //when enter -> give player access to move cards
    //when "enter" or button pressed -> exit, turn off card access and go to OpponentTurnState
    public override void Enter()
    {
        game.GetNode<CardManager>("CardManager").ProcessMode = ProcessModeEnum.Inherit;
        game.GetNode<CardManager>("CardManager").cardPlayedThisTurn = false;
        game.GetNode<Button>("EndTurnButton").Disabled = false;
        game.GetNode<Button>("EndTurnButton").Visible = true;
        
        game.GetNode<Button>("EndRoundButton").Disabled = false;
        game.GetNode<Button>("EndRoundButton").Visible = true;
    }

    public override void Exit()
    {
        game.GetNode<Button>("EndTurnButton").Disabled = true;
        game.GetNode<Button>("EndTurnButton").Visible = false;

        game.GetNode<Button>("EndRoundButton").Disabled = true;
        game.GetNode<Button>("EndRoundButton").Visible = false;
    }
}