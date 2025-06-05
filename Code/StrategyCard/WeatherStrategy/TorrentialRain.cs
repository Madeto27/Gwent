class TorrentialRain : ICardAbility
{
    public void Execute(CardScene cardScene, RowScene rowScene)
    {
        //Sets all Siege units to strength 1 for both players
        if (rowScene.row == 4)
        {
            WeatherManager.Instance.ApplyWeather(3);
        }
    }
}