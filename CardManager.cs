using Godot;
using System;

public partial class CardManager : Node2D
{
    const int collisionMaskCard = 1;
    const int collisionMaskRow = 2;
    CardScene cardBeingDragged;
    Vector2 screenSize;
    PlayerHand playerHandReference;
    EnemyHand enemyHandReference;
    DeckScene playerDeckReference;
    EnemyDeck enemyDeckReference;
    private Game _game;
    public bool cardPlayedThisTurn = false;

    public override void _Ready()
    {
        _game = GetNode<Game>("..");
        screenSize = GetViewportRect().Size;
        playerHandReference = GetNode<PlayerHand>("../PlayerHand");
        enemyHandReference = GetNode<EnemyHand>("../EnemyHand");
        playerDeckReference = GetNode<DeckScene>("../DeckScene");
        enemyDeckReference = GetNode<EnemyDeck>("../EnemyDeck");
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        if (cardBeingDragged != null)
        {
            var mousePos = GetGlobalMousePosition();
            cardBeingDragged.Position = new Vector2(Mathf.Clamp(mousePos.X, 0, screenSize.X), Mathf.Clamp(mousePos.Y, 0, screenSize.Y));//mousePos;
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (_game._currentState is EnemyTurnState) return;
        if (@event.IsAction("mouse_button_left"))
        {
            if (@event.IsPressed())
            {
                var card = rayCastCheckForCard();
                if (CanDragCard(card)){
                    cardBeingDragged = card;
                }
            }
            else if (@event.IsReleased())
            {
                var rowFound = rayCastCheckForRow();
                if (rowFound != null && cardBeingDragged != null && rowFound.row == cardBeingDragged.row)
                {
                    playerHandReference.RemoveCardFromHand(cardBeingDragged);
                    cardBeingDragged.GetParent().RemoveChild(cardBeingDragged);
                    rowFound.Add(cardBeingDragged);
                    cardBeingDragged.GetNode<CollisionShape2D>("Area2D/CollisionShape2D").Disabled = true;
                    cardPlayedThisTurn = true;
                }
                else
                {
                    playerHandReference.AddCardToHand(cardBeingDragged);
                }
                cardBeingDragged = null;
            }
        }
    }

    public CardScene rayCastCheckForCard()
    {
        var spaceState = GetWorld2D().DirectSpaceState;
        var parameters = new PhysicsPointQueryParameters2D();

        parameters.Position = GetGlobalMousePosition();
        parameters.CollideWithAreas = true;
        parameters.CollisionMask = collisionMaskCard;

        var result = spaceState.IntersectPoint(parameters);

        if (result.Count > 0)
        {
            var collider = result[0]["collider"].As<Area2D>();
            return collider.GetParent() as CardScene;
        }
        return null;
    }

    public RowScene rayCastCheckForRow()
    {
        var spaceState = GetWorld2D().DirectSpaceState;
        var parameters = new PhysicsPointQueryParameters2D();

        parameters.Position = GetGlobalMousePosition();
        parameters.CollideWithAreas = true;
        parameters.CollisionMask = collisionMaskRow;

        var result = spaceState.IntersectPoint(parameters);

        if (result.Count > 0)
        {
            var collider = result[0]["collider"].As<Area2D>();
            return collider.GetParent() as RowScene;
        }
        return null;
    }

    public bool CanDragCard(CardScene cardScene)
    {
        if (cardScene == null || cardPlayedThisTurn) return false;

        var parent = cardScene.GetParent();
        return parent != null &&
               parent != enemyHandReference &&
               parent != enemyDeckReference &&
               parent != playerDeckReference;
    }
}
