using System.Collections.Generic;
using System.Numerics;
using Godot;

public partial class RoundEndState : State
{
    public override void Enter()
    {
        int playerPower = game.GetTotalPlayerPower();
        int enemyPower = game.GetTotalEnemyPower();

        if (playerPower > enemyPower){
            game.score++;
        }
        else{
            game.score--;
        }

        switch (game.score)
        {
            case 0:
                GetNode<Sprite2D>("../PlayerHP/1HP").Visible = false;
                GetNode<Sprite2D>("../EnemyHP/1HP").Visible = false;
                break;
            case 1:
                GetNode<Sprite2D>("../EnemyHP/1HP").Visible = false;
                break;
            case 2:
                GetNode<Sprite2D>("../EnemyHP/2HP").Visible = false; //player Won
                break;
            case -1:
                GetNode<Sprite2D>("../PlayerHP/1HP").Visible = false;
                break;
            case -2:
                GetNode<Sprite2D>("../PlayerHP/2HP").Visible = false; //player Lost
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

        foreach (RowScene rowScene in game.playerRows)
        {
            var cards = new List<CardScene>(rowScene.children);
            foreach (CardScene cardScene in cards)
            {
                Godot.Vector2 originalPosition = cardScene.GlobalPosition;

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
    }
}