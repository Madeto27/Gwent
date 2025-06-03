using System;
using Godot;

public partial class EnemyTurnState : State
{
    //private Timer _enemyTimer;

    public override void Enter()
    {
        game.GetNode<CardManager>("CardManager").ProcessMode = ProcessModeEnum.Disabled;
        //OnEnemyActionFinished();
        //play card
        EnemyTurn();
        game.ChangeState("CheckEnd");
    }

    
    public async void EnemyTurn()
    {
        int playerPower = game.GetTotalPlayerPower();
        int enemyPower = game.GetTotalEnemyPower();

        var battleManager = GetNode<BattleManager>("../BattleManager");
        battleManager.endTurnButton.Disabled = true;
        battleManager.endTurnButton.Visible = false;
        battleManager.battleTimer.Start();

        var enemyHand = GetNode<EnemyHand>("../EnemyHand").enemyHand;
        if (enemyHand.Count == 0)
        {
            GetNode<Game>("..").enemyPassed = true;
            return;
        }

        if (GetNode<Game>("..").playerPassed && playerPower < enemyPower)
        {
            GetNode<Game>("..").enemyPassed = true;
            return;
        }

        Random rnd = new Random();
        battleManager.PlayCard(enemyHand[rnd.Next(0, enemyHand.Count)]);

        await ToSignal(battleManager.battleTimer, Timer.SignalName.Timeout);

        GetNode<Game>("..").ChangeState("CheckEnd");
    }

    private void OnEnemyActionFinished()
    {
        //if (enemyChoseToPass)
        //{ game.enemyPassed = true; }

        game.ChangeState("CheckEnd");
    }
}
