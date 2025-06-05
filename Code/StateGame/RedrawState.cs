using System.Collections.Generic;
using System.Numerics;
using Godot;
public partial class RedrawState : State
{
    public override void Enter()
    {
        var game = GetNode<Game>("..");

        ColorRect background = new ColorRect();
        background.Color = new Color(0, 0, 0, 0.5f);
        background.Size = GetViewportRect().Size;
        game.AddChild(background);
        background.ZIndex = 1;

        var playerHand = game.GetNode<PlayerHand>("PlayerHand");

        playerHand.cardWidth *= 2;
        playerHand.UpdateHandPosition();

        foreach (CardScene cardScene in playerHand.playerHand)
        {
            cardScene.ZIndex = 2;
            cardScene.Scale = new Godot.Vector2(2, 2);
        }
        playerHand.GlobalPosition = new Godot.Vector2(0, -playerHand.handHeight + GetViewportRect().Size.Y / 2);

        //[COMPLETED]click on card and insta redraw it
        //[COMPLETED]add black half visible sprite behind hand
        //[COMPLETED]Only can click, not move card
        //[COMPLETED]calculate hand position back and go to player state
    }

    public override void Exit()
    {
        var playerHand = game.GetNode<PlayerHand>("PlayerHand");
        playerHand.cardWidth /= 2;

        foreach (CardScene cardScene in playerHand.playerHand)
        {
            cardScene.ZIndex = 1;
            cardScene.Scale = new Godot.Vector2(1, 1);
        }

        //playerHand.handHeight += (int)GetViewportRect().Size.Y / 2;
        playerHand.GlobalPosition = new Godot.Vector2(0, 0); //can better
        playerHand.UpdateHandPosition();

        var background = game.GetNode<ColorRect>("@ColorRect@109");
        game.RemoveChild(background);
        background.QueueFree();
    }
}
