using System.Collections.Generic;
class TightBond : ICardAbility
{
    public void Execute(CardScene cardScene, RowScene rowScene){
        //Place next to a card with the same name to double the strength of both cards
        int count = 0;
        foreach (CardScene card in rowScene.children){
            if (card.name == cardScene.name){
                count++;
            }
        }

        for (int i = 0; i < count-1; i++){
            cardScene.power *= 2;
        }
    }
}