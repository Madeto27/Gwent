using Godot;

public partial class RoundEndState : State
{
    public override void Enter()
    {
        base.Enter();

        int playerPower = game.GetTotalPlayerPower();
        int enemyPower = game.GetTotalEnemyPower();

        if (playerPower > enemyPower)
        {
            GetNode<Sprite2D>("../EnemyHP/1HP").Visible = false;
            game.score++;
        }
        else
        {
            GetNode<Sprite2D>("../PlayerHP/1HP").Visible = false;
            game.score--;
        }

        //var enemyDiscard = GetNode<EnemyDiscard>("EnemyDiscard");
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
        //var enemyDiscard = GetNode<EnemyDiscard>("EnemyDiscard");
        foreach (CardScene cardScene in game.row1.children)
        {
            cardScene.GetParent().RemoveChild(cardScene);
            playerDiscard.AddChild(cardScene);
            cardScene.Position = new Vector2(0, 0);
        }
        foreach (CardScene cardScene in game.row2.children)
        {
            cardScene.GetParent().RemoveChild(cardScene);
            playerDiscard.AddChild(cardScene);
            cardScene.Position = new Vector2(0, 0);
        }
        foreach (CardScene cardScene in game.row3.children)
        {
            cardScene.GetParent().RemoveChild(cardScene);
            playerDiscard.AddChild(cardScene);
            cardScene.Position = new Vector2(0, 0);
        }
    }
}