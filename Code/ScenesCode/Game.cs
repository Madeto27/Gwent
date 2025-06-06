using System.Collections.Generic;
using Godot;

public partial class Game : Node2D
{
    [Export] public NodePath initialState;
    private Dictionary<string, State> _states;
    public State _currentState { get; private set; }
    public int[] score = new int[2];
    public bool playerPassed = false;
    public bool enemyPassed = false;
    public bool lastStateWasPlayer;
    public RowScene[] playerRows = new RowScene[3];
    public RowScene[] enemyRows = new RowScene[3];
    public RichTextLabel playerTotalPower;
    public RichTextLabel enemyTotalPower;
    public RowScene weatherRow;
    public TurnLabel turnLabel;
    public ColorRect background;


    public int GetTotalPlayerPower()
    {
        return playerRows[0].GetPower() + playerRows[1].GetPower() + playerRows[2].GetPower();
    }

    public int GetTotalEnemyPower()
    {
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

        var redraw = new RedrawState();
        var playerTurn = new PlayerTurnState();
        var enemyTurn = new EnemyTurnState();
        var roundEnd = new RoundEndState();
        var checkEnd = new CheckRoundEndState();

        AddChild(redraw);
        AddChild(playerTurn);
        AddChild(enemyTurn);
        AddChild(roundEnd);
        AddChild(checkEnd);

        _states.Add("Redraw", redraw);
        _states.Add("PlayerTurn", playerTurn);
        _states.Add("EnemyTurn", enemyTurn);
        _states.Add("RoundEnd", roundEnd);
        _states.Add("CheckEnd", checkEnd);

        foreach (var state in _states.Values)
        {
            state.game = this;
        }

        ChangeState("Redraw");//game by default


        RowCreator rCreator = new RowCreator();

        int playerRowHeight = 545;
        for (int i = 0; i < 3; i++)
        {
            playerRows[i] = rCreator.CreateRow(i + 1);
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
            enemyRows[i] = rCreator.CreateRow(i + 1);
            if (enemyRows[i] != null)
            {
                enemyRows[i]._collisionShape.QueueFree();
                AddChild(enemyRows[i]);
                enemyRows[2] = enemyRows[i];
                enemyRows[i].GlobalPosition = new Vector2(1920 / 2, enemyRowHeight);
                enemyRows[i]._circle.Modulate = new Color(0.75f, 0.15f, 0.15f);//making a bit red
            }
            enemyRowHeight -= 155;
        }

        weatherRow = rCreator.CreateRow(4);
        if (weatherRow != null)
        {
            AddChild(weatherRow);
            weatherRow.GlobalPosition = new Vector2(1920 / 8, 1080 / 2);
        }

        turnLabel = GetNode<TurnLabel>("TurnLabel");
        playerTotalPower = GetNode<RichTextLabel>("PlayerTotalPower/RichTextLabel");
        enemyTotalPower = GetNode<RichTextLabel>("EnemyTotalPower/RichTextLabel");
    }
}
