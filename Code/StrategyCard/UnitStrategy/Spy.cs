using System.Linq;
using Godot;

class Spy : ICardAbility
{
    public void Execute(CardScene cardScene, RowScene rowScene)
    {
        //Place on your opponents battlefield (counts towards their total strength) then draw two new cards from your deck
        //rowScene.Remove(cardScene);
        //var game = cardScene.GetNode<Game>("..");
        var game = rowScene.GetNode<Game>("..");
        if (game == null) return;

        var ability = cardScene.ability;
        cardScene.ability = null;

        //RowScene targetRow = game.playerRows.Contains(rowScene)
        //? game.enemyRows.FirstOrDefault(x => x.row == rowScene.row)
        //: game.playerRows.FirstOrDefault(x => x.row == rowScene.row);

        RowScene targetRow = null;
        if (game.playerRows.Contains(rowScene))
        {
            // Search in enemy rows for matching row type
            foreach (RowScene enemyRow in game.enemyRows)
            {
                if (enemyRow.row == rowScene.row)
                {
                    targetRow = enemyRow;
                    break;
                }
            }
        }
        else
        {
            // Search in player rows for matching row type
            foreach (RowScene playerRow in game.playerRows)
            {
                if (playerRow.row == rowScene.row)
                {
                    targetRow = playerRow;
                    break;
                }
            }
        }

        if (targetRow != null)
        {
            rowScene.Remove(cardScene);
            targetRow.Add(cardScene);
            if (game.playerRows.Contains(rowScene))
            {
                var deck = game.GetNode<DeckScene>("DeckScene");
                var hand = game.GetNode<PlayerHand>("PlayerHand");
                hand.DrawCard(deck);
                hand.DrawCard(deck);
            }
            else
            {
                var deck = game.GetNode<EnemyDeck>("EnemyDeck");
                var hand = game.GetNode<EnemyHand>("EnemyHand");
                hand.DrawCard(deck);
                hand.DrawCard(deck);
            }
        }

        //cardScene.ability = ability;
        //remove parent
        //get reference
        //put in opponent row
        //draw 2 cards
    }
}