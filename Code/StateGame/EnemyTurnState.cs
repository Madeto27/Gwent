using System;
using Godot;

public partial class EnemyTurnState : State
{
    public override void Enter()
    {
        game.GetNode<CardManager>("CardManager").ProcessMode = ProcessModeEnum.Disabled;
        EnemyTurn();
        game.lastStateWasPlayer = false;
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
            game.enemyPassed = true;
            return;
        }

        if (game.playerPassed && playerPower < enemyPower)
        {
            game.enemyPassed = true;
            return;
        }

        Random rnd = new Random();
        bool choiceIsMade = false;
        CardScene nextCard = new CardScene();
        int count = 0;
        while (!choiceIsMade && count < 5)
        {
            nextCard = enemyHand[rnd.Next(0, enemyHand.Count)];
            count++;

            if (nextCard.row != 4 || !RowHasCardWithSameName(nextCard))
            {
                choiceIsMade = true;
            }
        }

        if (!choiceIsMade)
        {
            game.enemyPassed = true;
            GD.Print("ENEMY PASSED");
            return;
        }

        await ToSignal(battleManager.battleTimer, Timer.SignalName.Timeout);

        if (!game.enemyPassed)
        {
            battleManager.PlayCard(nextCard);
        }
    }
    public bool RowHasCardWithSameName(CardScene excludeCard)
    {
        var game = GetNode<Game>("..");
        foreach (CardScene child in game.weatherRow.children)
        {
            if (child.name == excludeCard.name)
            {
                return true;
            }
        }
        return false;
    }
}
