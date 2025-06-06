class ClearWeather : ICardAbility
{
    public void Execute(CardScene cardScene, RowScene rowScene)
    {
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
    }
} 