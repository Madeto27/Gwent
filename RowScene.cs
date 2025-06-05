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
    private Sprite2D _sprite;
    public Area2D _area;
    public CollisionShape2D _collisionShape;
    public RichTextLabel _richTextLabel;


    public override void _Ready()
    {
        base._Ready();
        WeatherManager.Instance.Register(this);
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

        bool[] activeWeather = WeatherManager.Instance.activeWeather;
        if (row == 1) cardScene.isWeatherAffected = activeWeather[1];
        else if (row == 2) cardScene.isWeatherAffected = activeWeather[2];
        else if (row == 3) cardScene.isWeatherAffected = activeWeather[3];

        List<CardScene> childrenCopy = children;
        //var weather = GetNode<WeatherManager>("../WeatherManager");
        //OnWeatherChanged(weather.activeWeather); //АБІЛКИ НЕ РАХУЮТЬСЯ ЧОГОСЬ

        foreach (CardScene child in childrenCopy)
        {
            child.UpdatePower();
            //child.ResetPower();
        }
        foreach (CardScene child in children)
        {
            if (children.Contains(child) && child != cardScene)
            {
                child.UseAbility(this);
                child.UpdatePowerLabel(); //nothing happens if card placed after
                //child.UpdatePower(); //nothing happens if card placed after, BUT if another weather card placed it fixes
            }
        }

        //коли додаю нову погоду, то стара реаплаїться
        //так як вони в окермому ряду, то здібності карті в ряді на який погода не реаплаяться

        cardScene.UseAbility(this);
        cardScene.UpdatePowerLabel();

        _richTextLabel.Text = $"{GetPower()}";

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
            tex = GD.Load<Texture2D>("res://BoardTextures/WeatherRow.png");
        else
            tex = GD.Load<Texture2D>("res://BoardTextures/Row.png");

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
