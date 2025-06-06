class MoraleBoost : ICardAbility
{
    public void Execute(CardScene cardScene, RowScene rowScene){
        //Adds +1 strength to all units in the row, excluding itself
        foreach (CardScene card in rowScene.children)
        {
            if (card.GetInstanceId() != cardScene.GetInstanceId())
            {
                card.power += 1;
            }
        }
    }
}