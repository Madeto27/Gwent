using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class PlayerHand : Node2D
{
    const int handSize = 10;
    const int cardWidth = 100;
    public List<CardScene> playerHand = new List<CardScene>();
    public float centerScreenX;

    public override void _Ready(){
        base._Ready();
        centerScreenX = GetViewportRect().Size.X/2;

        UnitCreator uCreator = new UnitCreator();
        uCreator.LoadData();
        
        List<string> baseCardIdsList = uCreator.GetCardIds();
        List<string> fullCardIdsList = new List<string>();

        foreach (string cardId in baseCardIdsList){
            int amount = uCreator.GetCardAmount(cardId);
            for (int i = 0; i < amount; i++){
                fullCardIdsList.Add(cardId);
            }
        }

        fullCardIdsList.Shuffle();

        for (int i = 0; i < handSize; i++){
            CardScene card = uCreator.CreateCard(fullCardIdsList[i]);
            if (card != null){
                AddChild(card);
                AddCardToHand(card);          
            }
        }

        UpdateHandPosition();
    }

    public void AddCardToHand(CardScene cardScene){
        if (cardScene != null){
            if (!playerHand.Contains(cardScene)){
                playerHand.Add(cardScene);
            }
            else{
                AnimateCardToPosition(cardScene, cardScene.playerHandPosition);
            }
        }
    }
    
    public void UpdateHandPosition(){
        for (int i = 0; i<playerHand.Count; i++){
            // Get new card position based on index passed in
            var newPosition = new Vector2(CalculateCardPosition(i), 500);
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
        if(playerHand.Contains(cardScene)){
            playerHand.Remove(cardScene);
            UpdateHandPosition();
        }
    }
}
