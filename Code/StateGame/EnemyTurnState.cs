using System;
using System.Threading.Tasks;
using Godot;

public partial class EnemyTurnState : State
{
    public async override void Enter()
    {
        game.GetNode<CardManager>("CardManager").ProcessMode = ProcessModeEnum.Disabled;
        await EnemyTurn();
        game.lastStateWasPlayer = false;
        game.ChangeState("CheckEnd");
    }

    public async Task EnemyTurn()
    {
        int playerPower = game.GetTotalPlayerPower();
        int enemyPower = game.GetTotalEnemyPower();

        var battleManager = GetNode<BattleManager>("../BattleManager");
        battleManager.endTurnButton.Disabled = true;
        battleManager.endTurnButton.Visible = false;

        game.turnLabel.ShowTurnLabel("Enemy's turn");
        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        await ToSignal(game.turnLabel.animationPlayer, AnimationPlayer.SignalName.AnimationFinished);

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

        CardScene nextCard = new CardScene();
        bool choiceIsMade = false;
        
        //play spy
        foreach (CardScene card in enemyHand)
        {
            if (card.ability is Spy && !choiceIsMade)
            {
                nextCard = card;
                choiceIsMade = true;
                break;
            }
        }


        //play weather for opponents highest power row
        RowScene strongestRow = game.playerRows[0];
        for (int i = 1; i < game.playerRows.Length; i++)
        {
            if (strongestRow.power < game.playerRows[i].power)
            {
                strongestRow = game.playerRows[i];
            }
        }

        foreach (CardScene card in enemyHand)
        {
            if (card.row == 4 && strongestRow.power != 0 && !RowHasCardWithSameName(nextCard))
            {
                switch (strongestRow.row)
                {
                    case 1:
                        if (card.ability is BitingFrost)
                        {
                            nextCard = card;
                            choiceIsMade = true;
                        }
                        break;
                    case 2:
                        if (card.ability is ImpenetrableFog)
                        {
                            nextCard = card;
                            choiceIsMade = true;
                        }
                        break;
                    case 3:
                        if (card.ability is TorrentialRain)
                        {
                            nextCard = card;
                            choiceIsMade = true;
                        }
                        break;
                }
            }
        }

        Random rnd = new Random();
        int count = 0;
        while (!choiceIsMade && count < 5)
        {
            nextCard = enemyHand[rnd.Next(0, enemyHand.Count)];
            count++;
            if (count == 4 && !RowHasCardWithSameName(nextCard))
                choiceIsMade = true;
 
            if (nextCard.row != 4)
                choiceIsMade = true;
        }

        if (!choiceIsMade)
        {
            game.enemyPassed = true;
            game.turnLabel.ShowTurnLabel("Enemy passed");
            await ToSignal(game.turnLabel.animationPlayer, AnimationPlayer.SignalName.AnimationFinished);
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
