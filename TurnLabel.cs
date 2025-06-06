using Godot;
using System;

public partial class TurnLabel : Node2D
{
    public RichTextLabel label;
    //public ColorRect background;
    public Sprite2D sprite;
    public AnimationPlayer animationPlayer;
    public override void _Ready()
    {
        base._Ready();
        label = GetNode<RichTextLabel>("Label");
        sprite = GetNode<Sprite2D>("Sprite2D");
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }

    public void ShowTurnLabel(string text)
    {
        label.Text = text;
        animationPlayer.Play("fadeIn");
    }
}
