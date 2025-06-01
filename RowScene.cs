using Godot;
using System.Collections.Generic;

public partial class RowScene : Node2D
{
    public int row;
    //const int rowSize = 10;
    public int power;
    const int cardWidth = 100;
    public List<CardScene> children = new List<CardScene>();
    private float cardScale = 0.2f;
    public float centerScreenX;
    private Sprite2D _sprite;
    public Area2D _area;
    public CollisionShape2D _collisionShape;
    private RichTextLabel _richTextLabel;


    public override void _Ready()
    {
        base._Ready();
        centerScreenX = GetViewportRect().Size.X / 2;
        Position = new Vector2(centerScreenX, 100);
        _richTextLabel = GetNode<RichTextLabel>("RichTextLabel");
        _richTextLabel.Scale = new Vector2(cardScale*10, cardScale*10);
        _richTextLabel.Position = new Vector2(-500,-25);
    }

    public void Add(CardScene cardScene)
    {
        children.Add(cardScene);
        AddChild(cardScene);
        cardScene.Position = Vector2.Zero;
        UpdateCardPosition();
        cardScene.ZIndex = ZIndex;

        foreach (CardScene child in children)
        {
            child.ResetPower();
        }
        foreach (CardScene child in children)
        {
            child.UseAbility(this);
        }
        _richTextLabel.Text = $"{GetPower()}";
    }

    public void Remove(CardScene card){
        children.Remove(card);
        RemoveChild(card);
    }

    public int GetPower(){
        //можна просто додавати cardScene.power до power в Add()
        power = 0;
        foreach(CardScene cardScene in children){
            power += cardScene.power;
        }
        return power;
    }

    public void Initialize(int row){
        if (_sprite == null){
            _sprite = GetNode<Sprite2D>("Sprite2D");
            if (_sprite == null){
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
        ZIndex = -1;
    }
    
    public void UpdateCardPosition(){
        for (int i = 0; i<children.Count; i++){
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
