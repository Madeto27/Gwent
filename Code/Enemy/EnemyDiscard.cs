using Godot;
using System;
using System.Collections.Generic;

public partial class EnemyDiscard : Node2D
{
    public List<CardScene> discardedCards = new List<CardScene>();

    public void AddToDiscard(CardScene card)
    {
        discardedCards.Add(card);
    }

    public void RemoveFromDiscard(CardScene card)
    {
        RemoveChild(card);
        discardedCards.Remove(card);
    }


    public void AnimateCardToPosition(CardScene cardScene, Vector2 newPosition)
    {
        var tween = GetTree().CreateTween();
        tween.TweenProperty(cardScene, "global_position", newPosition, 0.5);
    }
}
