using System.Collections.Generic;
using System.Numerics;
using Godot;

public partial class RoundEndState : State
{
    public override void Enter()
    {
        base.Enter();

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

        game.GetNode<DeckScene>("DeckScene").DrawCard();
        game.GetNode<EnemyDeck>("EnemyDeck").DrawCard();

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
                rowScene.RemoveChild(cardScene);
                playerDiscard.AddChild(cardScene);
                cardScene.Position = Godot.Vector2.Zero;
            }
            rowScene.children.Clear();
        }

        foreach (RowScene rowScene in game.enemyRows)
        {
            var cards = new List<CardScene>(rowScene.children);
            foreach (CardScene cardScene in cards)
            {
                rowScene.RemoveChild(cardScene);
                enemyDiscard.AddChild(cardScene);
                cardScene.Position = Godot.Vector2.Zero;
            }
            rowScene.children.Clear();
        }
    }
}