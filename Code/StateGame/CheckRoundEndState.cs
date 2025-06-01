public partial class CheckRoundEndState : State
{
    public override void Enter()
    {
        if (game.playerPassed && game.enemyPassed)
        {
            game.ChangeState("RoundEnd");
        }
        else
        {
            string nextState = game.lastStateWasPlayer ? "EnemyTurn" : "PlayerTurn";
            game.ChangeState(nextState);
        }
    }
}