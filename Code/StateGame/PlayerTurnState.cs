using Godot;

public partial class PlayerTurnState : State
{
    //when enter -> give player access to move cards
    //when "enter" or button pressed -> exit, turn off card access and go to OpponentTurnState
    public override void Enter()
    {
        game.turnLabel.ShowTurnLabel("Your turn");
        game.GetNode<CardManager>("CardManager").ProcessMode = ProcessModeEnum.Inherit;
        game.GetNode<CardManager>("CardManager").cardPlayedThisTurn = false;
        game.GetNode<Button>("MarginContainer/VBoxContainer/EndTurnButton").Disabled = false;
        game.GetNode<Button>("MarginContainer/VBoxContainer/EndTurnButton").Visible = true;
        
        game.GetNode<Button>("MarginContainer/VBoxContainer/EndRoundButton").Disabled = false;
        game.GetNode<Button>("MarginContainer/VBoxContainer/EndRoundButton").Visible = true;
    }

    public override void Exit()
    {
        game.GetNode<Button>("MarginContainer/VBoxContainer/EndTurnButton").Disabled = true;
        game.GetNode<Button>("MarginContainer/VBoxContainer/EndTurnButton").Visible = false;

        game.GetNode<Button>("MarginContainer/VBoxContainer/EndRoundButton").Disabled = true;
        game.GetNode<Button>("MarginContainer/VBoxContainer/EndRoundButton").Visible = false;
    }
}