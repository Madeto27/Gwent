using Godot;

public partial class EnemyTurnState : State
{
    //private Timer _enemyTimer;

    public override void Enter()
    {
        game.GetNode<CardManager>("CardManager").ProcessMode = ProcessModeEnum.Disabled;
        OnEnemyActionFinished();
    }

    private void OnEnemyActionFinished()
    {
        game.ChangeState("PlayerTurn");
    }
}
