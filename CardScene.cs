using Godot;

public partial class CardScene : Node2D
{
    public string name;
    public int power;
    public int basePower;
    public string desc;
    public int row;
    public string texture;
    public int amount;
    public bool isWeatherAffected;

    public Vector2 playerHandPosition;
    public ICardAbility ability;
    public bool abilityExecuted = false;
    private float cardScale = 0.2f;

    private Sprite2D _sprite;
    public Area2D _area;
    private CollisionShape2D _collisionShape;
    public RichTextLabel _number;
    public Sprite2D _circle;

    public void Initialize(string name, int power, string desc, int row, string texture, int amount, ICardAbility ability)
    {
        this.name = name;
        this.power = power;
        this.basePower = power;
        this.desc = desc;
        this.row = row;
        this.texture = texture;
        this.amount = amount;
        this.ability = ability;


        if (_sprite == null)
        {
            _sprite = GetNode<Sprite2D>("Sprite2D");
        }

        Texture2D tex = GD.Load<Texture2D>(texture);
        _sprite.Texture = tex;
        _sprite.Scale = new Vector2(cardScale, cardScale);

        Vector2 texSize = _sprite.Texture.GetSize() * cardScale;
        _collisionShape = GetNode<CollisionShape2D>("Area2D/CollisionShape2D");

        _number = GetNode<RichTextLabel>("Power");
        _number.AddThemeColorOverride("default_color", Colors.Black);
        _number.Text = $"{power}";
        _number.Visible = false;

        _circle = GetNode<Sprite2D>("Sprite2D3");
        _circle.Visible = false;

        if (_collisionShape.Shape is RectangleShape2D rectangleShape)
        {
            rectangleShape.Size = texSize;
        }
    }

    public void UseAbility(RowScene rowScene)
    {
        ability?.Execute(this, rowScene);
    }

    public void UpdatePowerLabel()
    {
        _number.Text = $"{power}";
        if (!isWeatherAffected)
            _number.AddThemeColorOverride("default_color", Colors.Black);
        if (power > basePower)
            _number.AddThemeColorOverride("default_color", Colors.Green);
        if (isWeatherAffected)
            _number.AddThemeColorOverride("default_color", Colors.Red);
    }

    public void ResetPower()
    {
        power = basePower;
    }

    public void UpdatePower()
    {
        if (isWeatherAffected)
        {
            power = 1;
        }
        else if (power < basePower)
        {
            power = basePower;
        }
        UpdatePowerLabel();
    }
}
