using System.Collections.Generic;
using System.Numerics;
using Godot;

public partial class RoundEndState : State
{
    //somehow AI placed 2nd forst bite
    public async override void Enter()
    {
        game.turnLabel.ShowTurnLabel("Round End");
        await ToSignal(game.turnLabel.animationPlayer, AnimationPlayer.SignalName.AnimationFinished);
        int playerPower = game.GetTotalPlayerPower();
        int enemyPower = game.GetTotalEnemyPower();

        if (playerPower > enemyPower)
        {
            game.score[0]++;
        }
        else if (playerPower < enemyPower)
        {
            game.score[1]++;
        }
        else
        {
            game.score[0]++;
            game.score[1]++;
        }

        switch (game.score[1])
        {
            case 1:
                GetNode<Sprite2D>("../PlayerHP/1HP").Visible = false;
                break;
            case 2:
                GetNode<Sprite2D>("../PlayerHP/2HP").Visible = false;
                game.turnLabel.ShowTurnLabel("You Lost");
                await ToSignal(game.turnLabel.animationPlayer, AnimationPlayer.SignalName.AnimationFinished);
                GetTree().ChangeSceneToFile("res://menu.tscn");
                break;
        }

        switch (game.score[0])
        {
            case 1:
                GetNode<Sprite2D>("../EnemyHP/1HP").Visible = false;
                break;
            case 2:
                GetNode<Sprite2D>("../EnemyHP/2HP").Visible = false;
                game.turnLabel.ShowTurnLabel("You Won");
                await ToSignal(game.turnLabel.animationPlayer, AnimationPlayer.SignalName.AnimationFinished);
                GetTree().ChangeSceneToFile("res://menu.tscn");
                break;
        }

        //relocate all cards to each discard pile
        MoveCardsToDiscard();

        var playerDeck = game.GetNode<PlayerDeck>("PlayerDeck");
        var enemyDeck = game.GetNode<EnemyDeck>("EnemyDeck");
        var playerHand = game.GetNode<PlayerHand>("PlayerHand");
        var enemyHand = game.GetNode<EnemyHand>("EnemyHand");

        playerHand.DrawCard(playerDeck);
        enemyHand.DrawCard(enemyDeck);

        game.playerPassed = false;
        game.enemyPassed = false;

        game.ChangeState("PlayerTurn");
    }

    void MoveCardsToDiscard()
    {
        var playerDiscard = GetNode<PlayerDiscard>("../PlayerDiscard");
        var enemyDiscard = GetNode<EnemyDiscard>("../EnemyDiscard");

        WeatherManager.Instance.ClearWeather(1);
        WeatherManager.Instance.ClearWeather(2);
        WeatherManager.Instance.ClearWeather(3);

        foreach (RowScene rowScene in game.playerRows)
        {
            var cards = new List<CardScene>(rowScene.children);
            foreach (CardScene cardScene in cards)
            {
                Godot.Vector2 originalPosition = cardScene.GlobalPosition;

                cardScene.ResetPower();
                cardScene.UpdatePowerLabel();
                rowScene.RemoveChild(cardScene);
                rowScene._richTextLabel.Text = $"{rowScene.GetPower()}";
                playerDiscard.AddChild(cardScene);
                playerDiscard.AddToDiscard(cardScene);

                cardScene.GlobalPosition = originalPosition;
                playerDiscard.AnimateCardToPosition(cardScene, playerDiscard.GlobalPosition);
            }
            rowScene.children.Clear();
        }

        foreach (RowScene rowScene in game.enemyRows)
        {
            var cards = new List<CardScene>(rowScene.children);
            foreach (CardScene cardScene in cards)
            {
                Godot.Vector2 originalPosition = cardScene.GlobalPosition;

                cardScene.ResetPower();
                cardScene.UpdatePowerLabel();
                rowScene.RemoveChild(cardScene);
                rowScene._richTextLabel.Text = $"{rowScene.GetPower()}";
                enemyDiscard.AddChild(cardScene);
                enemyDiscard.AddToDiscard(cardScene);

                cardScene.GlobalPosition = originalPosition;
                enemyDiscard.AnimateCardToPosition(cardScene, enemyDiscard.GlobalPosition);
            }
            rowScene.children.Clear();
        }


        foreach (CardScene cardScene in game.weatherRow.children)
        {
            Godot.Vector2 originalPosition = cardScene.GlobalPosition;

            game.weatherRow.RemoveChild(cardScene);
            enemyDiscard.AddChild(cardScene);
            enemyDiscard.AddToDiscard(cardScene);

            cardScene.GlobalPosition = originalPosition;
            enemyDiscard.AnimateCardToPosition(cardScene, enemyDiscard.GlobalPosition); //
        }
        game.weatherRow.children.Clear();

        game.playerTotalPower.Text = $"{GetNode<Game>("..").GetTotalPlayerPower()}";
        game.enemyTotalPower.Text = $"{GetNode<Game>("..").GetTotalEnemyPower()}";
    }
}