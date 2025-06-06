using Godot;
using System;

public partial class Menu : Control
{
    public Button playButton;
    public Button optionsButton;
    public Button quitButton;
    public MarginContainer marginContainer;

    public override void _Ready()
    {
        marginContainer = GetNode<MarginContainer>("MarginContainer");
        marginContainer.Position = new Vector2(GetViewportRect().Size.X / 2 - marginContainer.Size.X / 2,
                                               GetViewportRect().Size.Y / 2 - marginContainer.Size.Y / 2);

        playButton = GetNode<Button>("MarginContainer/VBoxContainer/Play");
        playButton.Pressed += OnPlayButtonPressed;

        optionsButton = GetNode<Button>("MarginContainer/VBoxContainer/Options");
        optionsButton.Pressed += OnOptionsButtonPressed;

        quitButton = GetNode<Button>("MarginContainer/VBoxContainer/Quit");
        quitButton.Pressed += OnQuitButtonPressed;
    }

    void OnPlayButtonPressed()
    {
        GetTree().ChangeSceneToFile("res://Scenes/Game.tscn");
    }

    void OnOptionsButtonPressed()
    {
        GetTree().ChangeSceneToFile("res://Scenes/optionsMenu.tscn");
    }

    void OnQuitButtonPressed()
    {
        GetTree().Quit();
    }
}
