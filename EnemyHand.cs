using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class EnemyHand : Node2D
{
    const int handSize = 10;
    const int cardWidth = 80;
    const int handHeight = -65;
    public List<CardScene> enemyHand = new List<CardScene>();
    public float centerScreenX;

    public override void _Ready()
    {
        base._Ready();
        //on ready draw n amount (10) of cards to hand
        centerScreenX = GetViewportRect().Size.X / 2;
        EnemyDeck deck = GetNode<EnemyDeck>("../EnemyDeck");
        if (deck.GetChildCount() > 0)
            DrawInitialHand(deck);
        else
            deck.DeckReady += () => DrawInitialHand(deck);
    }

    public void DrawInitialHand(EnemyDeck deck)
    {
        for (int i = 0; i < handSize; i++)
        {
            DrawCard(deck);
        }
        UpdateHandPosition();
    }

    public void DrawCard(EnemyDeck deck)
    {
        CardScene card = deck.GetTopCard();
        if (card != null)
        {
            card.GlobalPosition = deck.GlobalPosition;
            AddChild(card);
            AddCardToHand(card);
        }
        UpdateHandPosition();
    }

    public void AddCardToHand(CardScene cardScene)
    {
        if (cardScene != null)
        {
            if (!enemyHand.Contains(cardScene))
                enemyHand.Add(cardScene);
            else
                AnimateCardToPosition(cardScene, cardScene.playerHandPosition);
            cardScene.ZIndex = 1;
        }
    }

    public void UpdateHandPosition(){
        for (int i = 0; i<enemyHand.Count; i++){
            // Get new card position based on index passed in
            var newPosition = new Vector2(CalculateCardPosition(i), handHeight);
            var cardScene = enemyHand[i];
            cardScene.playerHandPosition = newPosition;
            AnimateCardToPosition(cardScene, newPosition);
        }
    }

    public float CalculateCardPosition(int i){
        var totalWidth = (enemyHand.Count - 1)*cardWidth;
        var xOffset = centerScreenX + i*cardWidth - totalWidth/2;
        return xOffset;
    }

    public void AnimateCardToPosition(CardScene cardScene, Vector2 newPosition){
        var tween = GetTree().CreateTween();
        tween.TweenProperty(cardScene, "position", newPosition, 0.1);
    }

    public void RemoveCardFromHand(CardScene cardScene){
        if(enemyHand.Contains(cardScene)){
            enemyHand.Remove(cardScene);
            UpdateHandPosition();
        }
    }
}
