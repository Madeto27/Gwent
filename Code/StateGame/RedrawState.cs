using System.Collections.Generic;
using System.Numerics;
using Godot;
public partial class RedrawState : State
{
    public override void Enter()
    {
        var game = GetNode<Game>("..");

        game.background = game.GetNode<ColorRect>("Background");
        game.background.Color = new Color(0, 0, 0, 0.5f);
        game.background.Size = GetViewportRect().Size;
        game.background.ZIndex = 1;

        var playerHand = game.GetNode<PlayerHand>("PlayerHand");

        playerHand.cardWidth *= 2;
        playerHand.UpdateHandPosition();

        foreach (CardScene cardScene in playerHand.playerHand)
        {
            cardScene.ZIndex = 2;
            cardScene.Scale = new Godot.Vector2(2, 2);
        }
        playerHand.GlobalPosition = new Godot.Vector2(0, -playerHand.handHeight + GetViewportRect().Size.Y / 2);
    }

    public override void Exit()
    {
        var battleManager = GetNode<BattleManager>("../BattleManager");
        var playerHand = game.GetNode<PlayerHand>("PlayerHand");

        playerHand.cardWidth /= 2;

        foreach (CardScene cardScene in playerHand.playerHand)
        {
            cardScene.ZIndex = 1;
            cardScene.Scale = new Godot.Vector2(1, 1);
        }

        playerHand.GlobalPosition = new Godot.Vector2(0, 0);
        playerHand.UpdateHandPosition();

        game.RemoveChild(game.background);
        game.background.QueueFree();
    }
}
