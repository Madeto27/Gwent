public partial class CheckRoundEndState : State
{
    public override void Enter()
    {
        if (game.playerPassed && game.enemyPassed)
        {
            game.ChangeState("RoundEnd");
        }
        else if (game.playerPassed && !game.enemyPassed)
        {
            int playerPower = game.GetTotalPlayerPower();
            int enemyPower = game.GetTotalEnemyPower();

            if (enemyPower <= playerPower)
            {
                game.ChangeState("EnemyTurn");
            }
            else
            {
                game.enemyPassed = true;
                Enter();
            }
        }
        else
        {
            string nextState = game.lastStateWasPlayer ? "EnemyTurn" : "PlayerTurn";
            game.ChangeState(nextState);
        }
    }
}