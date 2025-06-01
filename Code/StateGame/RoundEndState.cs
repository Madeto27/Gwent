public partial class RoundEndState : State
{
    public override void Enter()
    {
        base.Enter();

        int playerPower = game.row1.GetPower() + game.row2.GetPower() + game.row3.GetPower();
        int enemyPower = game.row1Enemy.GetPower() + game.row2Enemy.GetPower() + game.row3Enemy.GetPower();

        if (playerPower > enemyPower){
            game.score++;
        }
        else{
            game.score--;
        }

        game.GetNode<DeckScene>("DeckScene").DrawCard();
        game.GetNode<EnemyDeck>("EnemyDeck").DrawCard();

        game.playerPassed = false;
        game.enemyPassed = false;

        game.ChangeState("PlayerTurn");
    }
}