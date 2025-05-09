using Godot;
using System;

public partial class CardManager : Node2D
{
    public override void _Input(InputEvent @event){
        if (@event.IsAction("mouse_button_left")){
            if (@event.IsPressed()){
                GD.Print("Click");
            }
            else if (@event.IsReleased()){
                GD.Print("Click release");
            }
        }
    }
}
