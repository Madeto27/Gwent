using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class PlayerHand : Node2D
{
    const int handSize = 10;
    public int cardWidth = 80;
    public int handHeight = 1005;
    public List<CardScene> playerHand = new List<CardScene>();
    public float centerScreenX;
    private AudioStreamPlayer2D cardDrawSfx;
    public AudioStreamPlayer2D cardTakeSfx;
    private AudioStreamPlayer2D cardPlaceSfx;

    public override void _Ready()
    {
        base._Ready();
        //on ready draw n amount (10) of cards to hand
        centerScreenX = GetViewportRect().Size.X / 2;
        PlayerDeck deck = GetNode<PlayerDeck>("../PlayerDeck");
        cardDrawSfx = GetNode<AudioStreamPlayer2D>("CardDrawSfx");
        cardTakeSfx = GetNode<AudioStreamPlayer2D>("CardTakeSfx");
        cardPlaceSfx = GetNode<AudioStreamPlayer2D>("CardPlaceSfx");

        deck.DeckReady += () => DrawInitialHand(deck);

    }

    public void DrawInitialHand(PlayerDeck deck)
    {
        for (int i = 0; i < handSize; i++)
        {
            DrawCard(deck);
        }
        
        UpdateHandPosition();
    }

    public void DrawCard(PlayerDeck deck)
    {
        CardScene card = deck.GetTopCard();
        if (card != null)
        {
            card.GlobalPosition = deck.GlobalPosition;
            AddChild(card);
            AddCardToHand(card);
            card.GetNode<AnimationPlayer>("AnimationPlayer").Play("card_flip");
            card._circle.Visible = true;
            card._number.Visible = true;
        }
        cardDrawSfx.Play();
        UpdateHandPosition();
    }

    public void AddCardToHand(CardScene cardScene)
    {
        if (cardScene != null)
        {
            if (!playerHand.Contains(cardScene))
            {
                playerHand.Add(cardScene);
            }
            else
            {
                AnimateCardToPosition(cardScene, cardScene.playerHandPosition);
            }
            cardScene.ZIndex = 0;
        }
    }
    
    public void UpdateHandPosition(){
        for (int i = 0; i<playerHand.Count; i++){
            // Get new card position based on index passed in
            var newPosition = new Vector2(CalculateCardPosition(i), handHeight);
            var cardScene = playerHand[i];
            cardScene.playerHandPosition = newPosition;
            AnimateCardToPosition(cardScene, newPosition);
        }
    }

    public float CalculateCardPosition(int i){
        var totalWidth = (playerHand.Count - 1)*cardWidth;
        var xOffset = centerScreenX + i*cardWidth - totalWidth/2;
        return xOffset;
    }

    public void AnimateCardToPosition(CardScene cardScene, Vector2 newPosition){
        var tween = GetTree().CreateTween();
        tween.TweenProperty(cardScene, "position", newPosition, 0.1);
    }

    public void RemoveCardFromHand(CardScene cardScene){
        if (playerHand.Contains(cardScene))
        {
            playerHand.Remove(cardScene);
            UpdateHandPosition();
            cardPlaceSfx.Play();
        }
    }
}
