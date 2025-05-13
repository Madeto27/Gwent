using Godot;
using System.Collections.Generic;

public partial class RowScene : Node2D
{
    public int row;
    //const int rowSize = 10;
    const int cardWidth = 100;
    private List<CardScene> children = new List<CardScene>();
    private float cardScale = 0.25f;
    public float centerScreenX;
    private Sprite2D _sprite;
    public Area2D _area;
    private CollisionShape2D _collisionShape;


    public override void _Ready(){
        base._Ready();
        centerScreenX = GetViewportRect().Size.X / 2;
        Position = new Vector2(centerScreenX, 100);
    }

    public void Add(CardScene cardScene){
        children.Add(cardScene);
        AddChild(cardScene);
        cardScene.Position = Vector2.Zero;
        UpdateCardPosition();
    }

    public void Remove(CardScene card){
        children.Remove(card);
        RemoveChild(card);
    }

    public int GetPower(){
        //overall row power
        return 0;
    }

    public void Initialize(int row){
        if (_sprite == null)
        {
            _sprite = GetNode<Sprite2D>("Sprite2D");
            if (_sprite == null)
            {
                GD.PrintErr("Sprite2D not found in scene!");
                return;
            }
        }
        
        Texture2D tex = GD.Load<Texture2D>("res://BoardTextures/raw.png");
        _sprite.Texture = tex;
        _sprite.Scale = new Vector2(cardScale, cardScale);

        Vector2 texSize = _sprite.Texture.GetSize() * cardScale;
        _collisionShape = GetNode<CollisionShape2D>("Area2D/CollisionShape2D");

        if (_collisionShape.Shape is RectangleShape2D rectangleShape)
        {
            rectangleShape.Size = texSize;
        }

        this.row = row;
    }
    
    public void UpdateCardPosition(){
        for (int i = 0; i<children.Count; i++){
            // Get new card position based on index passed in
            var newPosition = new Vector2(CalculateCardPosition(i), GlobalPosition.Y);
            var cardScene = children[i];
            AnimateCardToPosition(cardScene, newPosition);
        }  
    }

    public float CalculateCardPosition(int i){
        var totalWidth = (children.Count - 1)*cardWidth;
        var xOffset = centerScreenX + i*cardWidth - totalWidth/2;
        return xOffset;
    }

    public void AnimateCardToPosition(CardScene cardScene, Vector2 newPosition){
        var tween = GetTree().CreateTween();
        tween.TweenProperty(cardScene, "global_position", newPosition, 0.1);
    }
}
