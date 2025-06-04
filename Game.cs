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
    //public RowScene row1;
    //public RowScene row2;
    //public RowScene row3;
    //public RowScene row1Enemy;
    //public RowScene row2Enemy;
    //public RowScene row3Enemy;
    
    public RowScene[] playerRows = new RowScene[3];
    public RowScene[] enemyRows = new RowScene[3];

    public int GetTotalPlayerPower()
    {
        //return row1.GetPower() + row2.GetPower() + row3.GetPower();
        return playerRows[0].GetPower() + playerRows[1].GetPower()+ playerRows[2].GetPower();
    }

    public int GetTotalEnemyPower()
    {
        //return row1Enemy.GetPower()+row2Enemy.GetPower()+row3Enemy.GetPower();
        return enemyRows[0].GetPower() + enemyRows[1].GetPower()+ enemyRows[2].GetPower();
    }

    public void ChangeState(string key)
    {
        if (!_states.ContainsKey(key) || _currentState == _states[key]) return;

        if (_currentState != null) _currentState.Exit();

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

        int playerRowHeight = 545;
        for (int i = 0; i < 3; i++)
        {
            playerRows[i] = rCreator.CreateRow(i+1);
            if (playerRows[i] != null)
            {
                AddChild(playerRows[i]);
                playerRows[2] = playerRows[i];
                playerRows[i].GlobalPosition = new Vector2(1920 / 2, playerRowHeight);
            }
            playerRowHeight += 155;
        }

        int enemyRowHeight = 390;
        for (int i = 0; i < 3; i++)
        {
            enemyRows[i] = rCreator.CreateRow(i);
            if (enemyRows[i] != null)
            {
                AddChild(enemyRows[i]);
                enemyRows[2] = enemyRows[i];
                enemyRows[i].GlobalPosition = new Vector2(1920 / 2, enemyRowHeight);
            }
            enemyRowHeight -= 155;
        }
    }
}
