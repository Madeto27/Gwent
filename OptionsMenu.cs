using Godot;
using System;

public partial class OptionsMenu : Control
{
    public Button volumeButton;
    public Button difficultyButton;
    public Button backButton;
    public MarginContainer marginContainer;

    public override void _Ready()
    {
        marginContainer = GetNode<MarginContainer>("MarginContainer");
        marginContainer.Position = new Vector2(GetViewportRect().Size.X / 2 - marginContainer.Size.X / 2,
                                               GetViewportRect().Size.Y / 2 - marginContainer.Size.Y / 2);

        backButton = GetNode<Button>("MarginContainer/VBoxContainer/Back");
        backButton.Pressed += OnBackButtonPressed;
    }

    void OnBackButtonPressed()
    {
        GetTree().ChangeSceneToFile("res://menu.tscn");
    }
}
