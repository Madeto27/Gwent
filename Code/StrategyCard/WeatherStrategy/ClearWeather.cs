class ClearWeather : ICardAbility
{
    public void Execute(CardScene cardScene, RowScene rowScene)
    {
        //Removes all weather effects
        /*
        Godot.Timer delayTimer = new Godot.Timer();
        delayTimer.WaitTime = 2.0f;
        delayTimer.OneShot = true;
        delayTimer.Autostart = true;
        cardScene.AddChild(delayTimer);

        delayTimer.Timeout += () =>
        {*/
            if (rowScene.row == 4)
            {
                WeatherManager.Instance.ClearWeather(1);
                WeatherManager.Instance.ClearWeather(2);
                WeatherManager.Instance.ClearWeather(3);

                foreach (CardScene card in rowScene.children)
                {
                    card.QueueFree();
                }
                rowScene.children.Clear();
            }
            //delayTimer.QueueFree();
        //};
    }
} 