using Godot;
using System;
using System.Threading.Tasks;

public partial class BattleManager : Node2D
{
    //draw a card from opponent deck
    //wait 1 second

    public Timer battleTimer;
    public Button endTurnButton;
    public Button endRoundButton;

    public override void _Ready()
    {
        base._Ready();
        endTurnButton = GetNode<Button>("../EndTurnButton");
        endTurnButton.Pressed += OnEndTurnButtonPressed;

        endRoundButton = GetNode<Button>("../EndRoundButton");
        endRoundButton.Pressed += OnEndRoundButtonPressed;

        battleTimer = GetNode<Timer>("../BattleTimer");
        battleTimer.OneShot = true;
        battleTimer.WaitTime = 1.0;
    }

    public void OnEndRoundButtonPressed()
    {
        if (GetNode<CardManager>("../CardManager").cardPlayedThisTurn) return;
        var game = GetNode<Game>("..");
        game.playerPassed = true;

        game.GetNode<CardManager>("CardManager").ProcessMode = ProcessModeEnum.Disabled;
        endTurnButton.Disabled = true;
        endTurnButton.Visible = false;
        endRoundButton.Disabled = true;
        endRoundButton.Visible = false;

        game.lastStateWasPlayer = true;

        GetNode<Game>("..").ChangeState("CheckEnd");
    }

    public void OnEndTurnButtonPressed()
    {
        if (!GetNode<CardManager>("../CardManager").cardPlayedThisTurn) return;
        GetNode<Game>("..").ChangeState("EnemyTurn");
        //EnemyTurn();
    }

    public async void EnemyTurn()
    {
        endTurnButton.Disabled = true;
        endTurnButton.Visible = false;
        battleTimer.Start();

        var enemyHand = GetNode<EnemyHand>("../EnemyHand").enemyHand;
        if (enemyHand.Count == 0)
        {
            return;
        }

        Random rnd = new Random();
        PlayCard(enemyHand[rnd.Next(0, enemyHand.Count)]);

        //place card 

        await ToSignal(battleTimer, Timer.SignalName.Timeout);

        GetNode<Game>("..").ChangeState("EnemyTurn");
    }

    public void PlayCard(CardScene cardScene)
    {
        var game = GetNode<Game>("..");
        var enemyHand = GetNode<EnemyHand>("../EnemyHand");
        enemyHand.RemoveCardFromHand(cardScene);
        cardScene.GetParent().RemoveChild(cardScene);

        game.enemyRows[cardScene.row - 1].Add(cardScene);
        /*
        switch (cardScene.row)
        {
            case 1:
                game.row1Enemy.Add(cardScene);
                break;
            case 2:
                game.row2Enemy.Add(cardScene);
                break;
            case 3:
                game.row3Enemy.Add(cardScene);
                break;
        }*/

        cardScene.GetNode<AnimationPlayer>("AnimationPlayer").Play("card_flip");
    }

    public void EndEnemyTurn()
    {

    }
}


//at first play cards with low power
        //then place cards with abilites
        //place cards with highest power
        //after 1 second play a card