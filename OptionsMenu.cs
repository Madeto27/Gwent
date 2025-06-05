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

        volumeButton = GetNode<Button>("MarginContainer/VBoxContainer/Volume");
        volumeButton.Pressed += OnVolumeButtonPressed;

        difficultyButton = GetNode<Button>("MarginContainer/VBoxContainer/Difficulty");
        difficultyButton.Pressed += OnDiffficultyButtonPressed;

        backButton = GetNode<Button>("MarginContainer/VBoxContainer/Back");
        backButton.Pressed += OnBackButtonPressed;
    }

    void OnVolumeButtonPressed()
    {
        //GetTree().ChangeSceneToFile("res://menu.tscn");
    }

    void OnDiffficultyButtonPressed()
    {
    }

    void OnBackButtonPressed()
    {
        GetTree().ChangeSceneToFile("res://menu.tscn");
    }
}
