using System.Collections.Generic;
using Godot;

public partial class Game : Node2D
{

    [Export] public NodePath initialState;
        private Dictionary<string, State> _states;
    private State _currentState;

    public void ChangeState(string key)
    {
        if (!_states.ContainsKey(key) || _currentState == _states[key])
        {
            return;
        }
        _currentState.Exit();
        _currentState = _states[key];
        _currentState.Enter();
    }

    public override void _Ready()
    {
        base._Ready();

        /*
        _states = new Dictionary<string, State>();
        foreach (Node2D node2d in GetChildren())
        {
            if (node2d is State s)
            {
                _states[node2d.Name] = s;
                s.game = this;
                s.Ready();
                s.Exit(); //reset all states
            }    
        }

        _currentState = GetNode<State>(initialState);
        _currentState.Enter();
        */

        RowCreator rCreator = new RowCreator();
        RowScene row3 = rCreator.CreateRow(3);
        if (row3 != null)
        {
            AddChild(row3);
            row3.GlobalPosition = new Vector2(1920 / 2, 855);
        }

        RowScene row2 = rCreator.CreateRow(2);
        if (row2 != null)
        {
            AddChild(row2);
            row2.GlobalPosition = new Vector2(1920 / 2, 700);
        }

        RowScene row1 = rCreator.CreateRow(1);
        if (row1 != null)
        {
            AddChild(row1);
            row1.GlobalPosition = new Vector2(1920 / 2, 545);
        }

        RowScene row3Enemy = rCreator.CreateRow(3);
        if (row3Enemy != null)
        {
            row3Enemy._collisionShape.QueueFree();
            AddChild(row3Enemy);
            row3Enemy.GlobalPosition = new Vector2(1920 / 2, 80);
        }
        RowScene row2Enemy = rCreator.CreateRow(2);
        if (row2Enemy != null)
        {
            row2Enemy._collisionShape.QueueFree();
            AddChild(row2Enemy);
            row2Enemy.GlobalPosition = new Vector2(1920 / 2, 235);
        }
        RowScene row1Enemy = rCreator.CreateRow(1);
        if (row1Enemy != null)
        {
            row1Enemy._collisionShape.QueueFree();
            AddChild(row1Enemy);
            row1Enemy.GlobalPosition = new Vector2(1920 / 2, 390);
        }
    }
}
