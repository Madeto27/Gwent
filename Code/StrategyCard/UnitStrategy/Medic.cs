using System;
using System.Linq;

class Medic : ICardAbility
{
    public void Execute(CardScene cardScene, RowScene rowScene)
    {
        //Choose one car from your discard pile and play it instantly (no Heroes or Special Cards)
        var game = rowScene.GetNode<Game>("..");
        if (game == null) return;
        cardScene.ability = null;

        Random rnd = new Random();//has to change! CHOOSE

        CardScene randomCard;

        if (game.playerRows.Contains(rowScene))
        {
            //Player
            var discard = game.GetNode<PlayerDiscard>("PlayerDiscard");
            if (discard.discardedCards.Count == 0) return;
            randomCard = discard.discardedCards[rnd.Next(0, discard.discardedCards.Count)];
            discard.RemoveFromDiscard(randomCard);
        }
        else
        {
            //Enemy
            var discard = game.GetNode<EnemyDiscard>("EnemyDiscard");
            if (discard.discardedCards.Count == 0) return;
            randomCard = discard.discardedCards[rnd.Next(0, discard.discardedCards.Count)];
            discard.RemoveFromDiscard(randomCard);
        }

        if (randomCard.row == 4)
        {
            game.weatherRow.Add(randomCard);
        }
        else
        {
            var targetRow = game.playerRows.Contains(rowScene)
            ? game.playerRows[randomCard.row - 1]
            : game.enemyRows[randomCard.row - 1];
            targetRow.Add(randomCard);
        }

        randomCard.UpdatePower();
        randomCard.UseAbility(rowScene);        
    }
}