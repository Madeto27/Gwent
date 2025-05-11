using Godot;
using System;

public partial class CardManager : Node2D
{

    const int collisionMask = 1;
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
                //GD.Print("Click");
                var card = rayCastCheckForCard();
                if (card != null){
                    cardBeingDragged = card;
                }
            }
            else if (@event.IsReleased()){
                //GD.Print("Click released");
                cardBeingDragged = null;
            }
        }
    }

    public CardScene rayCastCheckForCard(){
        var spaceState = GetWorld2D().DirectSpaceState;
        var parameters = new PhysicsPointQueryParameters2D();

        parameters.Position = GetGlobalMousePosition();
        parameters.CollideWithAreas = true;
        parameters.CollisionMask = collisionMask;

        var result = spaceState.IntersectPoint(parameters);
        //var collider = result[0]["collider"];
        if(result.Count > 0){
            //return result[0]["collider"].ToString();
            var collider = result[0]["collider"].As<Area2D>();
            return collider.GetParent() as CardScene;
        }
        return null;
    }
}
