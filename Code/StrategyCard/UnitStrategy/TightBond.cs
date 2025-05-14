using System.Collections.Generic;
class TightBond : ICardAbility
{
    public void Execute(CardScene cardScene, RowScene rowScene){
        //Place next to a card with the same name to double the strength of both cards
        foreach(CardScene cardSame in rowScene.children){
            if (cardSame.name == cardScene.name && cardSame.GetInstanceId() != cardScene.GetInstanceId()){
                cardSame.power = cardSame.power * 2;
                cardScene.power = cardScene.power * 2;
            }
        }
    }
}