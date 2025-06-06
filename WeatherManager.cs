using Godot;
using System;
using System.Collections.Generic;

public partial class WeatherManager : Node2D
{
    private static WeatherManager instance;
    public static WeatherManager Instance => instance;
    public bool[] activeWeather = new bool[4];
    private List<RowScene> observers = new List<RowScene>();

    public override void _Ready()
    {
        instance = this;
    }

    public void ApplyWeather(int weatherType)
    {
        activeWeather[weatherType] = true;
        ApplyWeatherTexture(weatherType);
        NotifyObservers();
    }

    public void ClearWeather(int row)
    {
        activeWeather[row] = false;
        RemoveWeatherTexture(row);
        NotifyObservers();
    }

    public void Register(RowScene rowScene) => observers.Add(rowScene);
    public void Unregister(RowScene rowScene) => observers.Remove(rowScene);

    private void NotifyObservers()
    {

        foreach (var rowScene in observers)
        {
            rowScene.OnWeatherChanged(activeWeather);
        }
    }

    private void RemoveWeatherTexture(int row)
    {
        Game game = GetNode<Game>("..");
        game.playerRows[row - 1].weatherSpite.Texture = null;
        game.enemyRows[row-1].weatherSpite.Texture = null;
    }

    private void ApplyWeatherTexture(int row)
    {
        Game game = GetNode<Game>("..");
        var playerRow = game.playerRows[row - 1];
        var enemyRow = game.enemyRows[row - 1];

        Texture2D texture = GD.Load<Texture2D>($"res://BoardTextures/Weather{row}.png");

        playerRow.weatherSpite.Texture = texture;
        enemyRow.weatherSpite.Texture = texture;

        SetRightScale(playerRow.weatherSpite, playerRow._sprite);
        SetRightScale(enemyRow.weatherSpite, enemyRow._sprite);

        playerRow.weatherSpite.Modulate = new Color(1, 1, 1, 0.75f);
        enemyRow.weatherSpite.Modulate = new Color(1, 1, 1, 0.75f);

        if (playerRow.weatherSpite.GetParent() == null)
            playerRow.AddChild(playerRow.weatherSpite);
        if (enemyRow.weatherSpite.GetParent() == null)
            enemyRow.AddChild(enemyRow.weatherSpite);

        playerRow.weatherSpite.ZIndex = -1;
        enemyRow.weatherSpite.ZIndex = -1;
    }

    private void SetRightScale(Sprite2D sprite, Sprite2D scaledTo)
    {
        float targetWidth = scaledTo.Texture.GetSize().X * scaledTo.Scale.X;
        float targetHeight = scaledTo.Texture.GetSize().Y * scaledTo.Scale.Y;

        float spriteWidth = sprite.Texture.GetSize().X;
        float spriteHeight = sprite.Texture.GetSize().Y;

        if (spriteWidth <= 0 || spriteHeight <= 0)
            return;

        float scaleX = targetWidth / spriteWidth;
        float scaleY = targetHeight / spriteHeight;

        sprite.Scale = new Vector2(scaleX, scaleY);
    }
}
