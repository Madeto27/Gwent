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
        NotifyObservers();
    }

    public void ClearWeather(int row)
    {
        activeWeather[row] = false;
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
}
