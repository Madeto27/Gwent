using System.Collections.Generic;
using Godot;

public partial class Game : Node2D
{
    [Export] public NodePath initialState;
    private Dictionary<string, State> _states;
    public State _currentState { get; private set; }
    public int score = 0;
    public bool playerPassed = false;
    public bool enemyPassed = false;
    public bool lastStateWasPlayer;
    public RowScene row1;
    public RowScene row2;
    public RowScene row3;
    public RowScene row1Enemy;
    public RowScene row2Enemy;
    public RowScene row3Enemy;

    public int GetTotalPlayerPower()
    {
        return row1.GetPower()+row2.GetPower()+row3.GetPower();
    }

    public int GetTotalEnemyPower()
    {
        return row1Enemy.GetPower()+row2Enemy.GetPower()+row3Enemy.GetPower();
    }

    public void ChangeState(string key)
    {
        if (!_states.ContainsKey(key) || _currentState == _states[key])
        {
            return;
        }

        if (_currentState != null)
        {
            _currentState.Exit();
        }
        _currentState = _states[key];
        _currentState.Enter();
    }

    public void ResetPassStates()
    {
        playerPassed = false;
        enemyPassed = false;
    }

    public override void _Ready()
    {
        _states = new Dictionary<string, State>();

        var playerTurn = new PlayerTurnState();
        var enemyTurn = new EnemyTurnState();
        var roundEnd = new RoundEndState();
        var checkEnd = new CheckRoundEndState();

        AddChild(playerTurn);
        AddChild(enemyTurn);
        AddChild(roundEnd);
        AddChild(checkEnd);

        _states.Add("PlayerTurn", playerTurn);
        _states.Add("EnemyTurn", enemyTurn);
        _states.Add("RoundEnd", roundEnd);
        _states.Add("CheckEnd", checkEnd);

        foreach (var state in _states.Values)
        {
            state.game = this;
        }

        ChangeState("PlayerTurn");

        RowCreator rCreator = new RowCreator();
        row3 = rCreator.CreateRow(3);
        if (row3 != null)
        {
            AddChild(row3);
            row3.GlobalPosition = new Vector2(1920 / 2, 855);
        }

        row2 = rCreator.CreateRow(2);
        if (row2 != null)
        {
            AddChild(row2);
            row2.GlobalPosition = new Vector2(1920 / 2, 700);
        }

        row1 = rCreator.CreateRow(1);
        if (row1 != null)
        {
            AddChild(row1);
            row1.GlobalPosition = new Vector2(1920 / 2, 545);
        }

        row3Enemy = rCreator.CreateRow(3);
        if (row3Enemy != null)
        {
            row3Enemy._collisionShape.QueueFree();
            AddChild(row3Enemy);
            row3Enemy.GlobalPosition = new Vector2(1920 / 2, 80);
        }
        row2Enemy = rCreator.CreateRow(2);
        if (row2Enemy != null)
        {
            row2Enemy._collisionShape.QueueFree();
            AddChild(row2Enemy);
            row2Enemy.GlobalPosition = new Vector2(1920 / 2, 235);
        }
        row1Enemy = rCreator.CreateRow(1);
        if (row1Enemy != null)
        {
            row1Enemy._collisionShape.QueueFree();
            AddChild(row1Enemy);
            row1Enemy.GlobalPosition = new Vector2(1920 / 2, 390);
        }
    }
}
