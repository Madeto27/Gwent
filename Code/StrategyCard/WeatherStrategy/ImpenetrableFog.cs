class ImpenetrableFog : ICardAbility
{
    public void Execute(CardScene cardScene, RowScene rowScene)
    {
        //Sets all Ranged combat units to strength 1 for both players
        if (rowScene.row == 4)
        {
            WeatherManager.Instance.ApplyWeather(2);
        }
    }
}