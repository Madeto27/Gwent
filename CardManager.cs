using Godot;
using System;

public partial class CardManager : Node2D
{

    const int collisionMaskCard = 1;
    const int collisionMaskRow = 2;
    CardScene cardBeingDragged;
    Vector2 screenSize;

    public override void _Ready(){
        screenSize = GetViewportRect().Size;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        if (cardBeingDragged != null){
            var mousePos = GetGlobalMousePosition();
            cardBeingDragged.Position = new Vector2(Mathf.Clamp(mousePos.X,0,screenSize.X), Mathf.Clamp(mousePos.Y,0,screenSize.Y));//mousePos;
        }
    }

    public override void _Input(InputEvent @event){
        if (@event.IsAction("mouse_button_left")){
            if (@event.IsPressed()){
                var card = rayCastCheckForCard();
                if (card != null){
                    cardBeingDragged = card;
                }
            }
            else if (@event.IsReleased()){
                var rowFound = rayCastCheckForRow();
                if (rowFound != null && cardBeingDragged != null){
                    if (rowFound.row == cardBeingDragged.row){
                        //there is empty slot
                        cardBeingDragged.GetParent().RemoveChild(cardBeingDragged);
                        //Vector2 globalPos = cardBeingDragged.GlobalPosition;
                        rowFound.Add(cardBeingDragged);
                        cardBeingDragged.Position = Vector2.Zero;
                        cardBeingDragged.GetNode<CollisionShape2D>("Area2D/CollisionShape2D").Disabled = true;
                    }
                }
                cardBeingDragged = null;
            }
        }
    }

    public CardScene rayCastCheckForCard(){
        var spaceState = GetWorld2D().DirectSpaceState;
        var parameters = new PhysicsPointQueryParameters2D();

        parameters.Position = GetGlobalMousePosition();
        parameters.CollideWithAreas = true;
        parameters.CollisionMask = collisionMaskCard;

        var result = spaceState.IntersectPoint(parameters);

        if(result.Count > 0){
            var collider = result[0]["collider"].As<Area2D>();
            return collider.GetParent() as CardScene;
        }
        return null;
    }

    public RowScene rayCastCheckForRow(){
        var spaceState = GetWorld2D().DirectSpaceState;
        var parameters = new PhysicsPointQueryParameters2D();

        parameters.Position = GetGlobalMousePosition();
        parameters.CollideWithAreas = true;
        parameters.CollisionMask = collisionMaskRow;

        var result = spaceState.IntersectPoint(parameters);

        if(result.Count > 0){
            var collider = result[0]["collider"].As<Area2D>();
            return collider.GetParent() as RowScene;
        }
        return null;
    }
}
