using Godot;
using System;
using System.Threading.Tasks;

public partial class BattleManager : Node2D
{
    //draw a card from opponent deck
    //wait 1 second

    Timer battleTimer;
    Button endTurnButton; 

    public override void _Ready()
    {
        base._Ready();
        endTurnButton = GetNode<Button>("../EndTurnButton");
        endTurnButton.Pressed += OnEndTurnButtonPressed;
        battleTimer = GetNode<Timer>("../BattleTimer");
        battleTimer.OneShot = true;
        battleTimer.WaitTime = 1.0;
    }

    public void OnEndTurnButtonPressed()
    {
        if (!GetNode<CardManager>("../CardManager").cardPlayedThisTurn) return;
        EnemyTurn();
    }

    public async void EnemyTurn()
    {
        endTurnButton.Disabled = true;
        endTurnButton.Visible = false;
        battleTimer.Start();
        await ToSignal(battleTimer, Timer.SignalName.Timeout);

        GetNode<Game>("..").ChangeState("EnemyTurn");
    }
}


//at first play cards with low power
        //then place cards with abilites
        //place cards with highest power
        //after 1 second play a card