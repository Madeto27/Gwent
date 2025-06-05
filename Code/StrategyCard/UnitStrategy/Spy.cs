using System.Linq;
using Godot;

class Spy : ICardAbility
{
    public void Execute(CardScene cardScene, RowScene rowScene)
    {
        //Place on your opponents battlefield (counts towards their total strength) then draw two new cards from your deck
        var game = rowScene.GetNode<Game>("..");
        if (game == null) return;
        cardScene.ability = null;

        RowScene targetRow = null;
        if (game.playerRows.Contains(rowScene))
        {
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
                var deck = game.GetNode<PlayerDeck>("PlayerDeck");
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
    }
}