using Godot;
using System;

public partial class PlayerHp : Node2D
{
    Sprite2D hp1;
    Sprite2D hp2;

    public override void _Ready()
    {
        hp1 = GetNode<Sprite2D>("1HP");
        hp2 = GetNode<Sprite2D>("2HP");

        hp1.Position = new Vector2(-25, 0);
        hp2.Position = new Vector2(25, 0);
    }   
}
