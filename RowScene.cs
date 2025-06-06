using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class RowScene : Node2D
{
    public int row;
    const int rowSize = 10;
    const int weatherSize = 3;
    public bool isFull;
    public int power;
    const int cardWidth = 80;
    public List<CardScene> children = new List<CardScene>();
    private float cardScale = 0.2f;
    public float centerScreenX;
    public Sprite2D weatherSpite = new Sprite2D();
    public Sprite2D _sprite;
    public Area2D _area;
    public CollisionShape2D _collisionShape;
    public RichTextLabel _richTextLabel;
    public Sprite2D _circle;


    public override void _Ready()
    {
        base._Ready();
        WeatherManager.Instance.Register(this);
        centerScreenX = GetViewportRect().Size.X / 2;
        Position = new Vector2(centerScreenX, 100);

        _circle = GetNode<Sprite2D>("Sprite2D3");
        _circle.Scale = new Vector2(_circle.Scale.X*cardScale, _circle.Scale.Y*cardScale);
        _circle.Position = new Vector2(_circle.Position.X*cardScale, _circle.Position.Y*cardScale);

        _richTextLabel = GetNode<RichTextLabel>("RichTextLabel");
        _richTextLabel.Scale = new Vector2(cardScale, cardScale);
        _richTextLabel.Position = new Vector2(_richTextLabel.Position.X*cardScale, _richTextLabel.Position.Y*cardScale);
    }

    public void Add(CardScene cardScene)
    {
        children.Add(cardScene);
        AddChild(cardScene);
        cardScene.Position = Vector2.Zero;
        UpdateCardPosition();
        cardScene.ZIndex = ZIndex+1;

        bool[] activeWeather = WeatherManager.Instance.activeWeather;
        if (row == 1) cardScene.isWeatherAffected = activeWeather[1];
        else if (row == 2) cardScene.isWeatherAffected = activeWeather[2];
        else if (row == 3) cardScene.isWeatherAffected = activeWeather[3];

        List<CardScene> childrenCopy = children;

        foreach (CardScene child in childrenCopy)
        {
            child.UpdatePower();
            if (child.ability is MoraleBoost){
                child.ResetPower();
            }
        }
        foreach (CardScene child in children)
        {
            if (children.Contains(child) && child != cardScene)
            {
                child.UseAbility(this);
                //child.UpdatePowerLabel();
            }
        }
        cardScene.UseAbility(this);

        foreach (CardScene child in children)
        {
            child.UpdatePowerLabel();
        }
        //cardScene.UpdatePowerLabel();

        _richTextLabel.Text = $"{GetPower()}";
        GetNode<Game>("..").playerTotalPower.Text = $"{GetNode<Game>("..").GetTotalPlayerPower()}";
        GetNode<Game>("..").enemyTotalPower.Text = $"{GetNode<Game>("..").GetTotalEnemyPower()}";

        isFull = CheckIfFull();
    }

    public bool CheckIfFull() {
        if (row == 4 && children.Count == weatherSize) return true;
        if (children.Count == rowSize) return true;
        return false;
    }

    public void OnWeatherChanged(bool[] activeWeather)
    {
        bool isAffected = false;
        if (row == 1 && activeWeather[1] == true) isAffected = true;
        else if (row == 2 && activeWeather[2] == true) isAffected = true;
        else if (row == 3 && activeWeather[3] == true) isAffected = true;
        ApplyWeatherEffect(isAffected);
    }

    private void ApplyWeatherEffect(bool isAffected)
    {
        foreach (CardScene child in children)
        {
            child.isWeatherAffected = isAffected;
            child.UpdatePower();
        }
        _richTextLabel.Text = $"{GetPower()}";
    }

    public void Remove(CardScene card)
    {
        var newChildren = new List<CardScene>(children);
        newChildren.Remove(card);
        children = newChildren;

        RemoveChild(card);
        UpdateCardPosition();
        _richTextLabel.Text = $"{GetPower()}";
    }

    public int GetPower()
    {
        power = 0;
        foreach (CardScene cardScene in children)
        {
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

        Texture2D tex;
        if (row == 4)
            tex = GD.Load<Texture2D>("res://BoardTextures/WeatherRowWider1.png");
        else
            tex = GD.Load<Texture2D>("res://BoardTextures/Row2.png");

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
        float xOffset;
        if (this.row != 4)
            xOffset = centerScreenX + i * cardWidth - totalWidth / 2;
        else
            xOffset = 1920 / 8 + i * cardWidth - totalWidth / 2;

        return xOffset;
    }

    public void AnimateCardToPosition(CardScene cardScene, Vector2 newPosition){
        var tween = GetTree().CreateTween();
        tween.TweenProperty(cardScene, "global_position", newPosition, 0.1);
    }
}
