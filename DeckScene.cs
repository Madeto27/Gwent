using Godot;
using System.Collections.Generic;

public partial class DeckScene : Node2D
{
    [Signal]
    public delegate void DeckReadyEventHandler();
    public float centerScreenX;
    public List<CardScene> deck = new List<CardScene>();
    //class should be responsible for creating and storing all cards
    public override void _Ready()
    {
        base._Ready();
        centerScreenX = GetViewportRect().Size.X / 2;

        UnitCreator uCreator = new UnitCreator();
        uCreator.LoadData();

        List<string> baseCardIdsList = uCreator.GetCardIds();
        List<string> fullCardIdsList = new List<string>();

        foreach (string cardId in baseCardIdsList)
        {
            int amount = uCreator.GetCardAmount(cardId);
            for (int i = 0; i < amount; i++)
            {
                fullCardIdsList.Add(cardId);
            }
        }

        //list full of just IDs, not CardScene's
        fullCardIdsList.Shuffle();

        for (int i = 0; i < fullCardIdsList.Count; i++)
        {
            CardScene card = uCreator.CreateCard(fullCardIdsList[i]);
            if (card != null)
            {
                AddChild(card);
                deck.Add(card);
            }
        }
        //update position

        EmitSignal(SignalName.DeckReady);
    }

    public CardScene DrawCard()
    {
        if (deck.Count == 0){
            return null;
        }

        CardScene card = deck[0];
        deck.RemoveAt(0);
        RemoveChild(card);
        return card;
    }
}
