using Godot;

public partial class CardScene : Node2D
{
    public string name; 
    public int power;
    public string desc;
    public string texture;

    private float cardScale = 0.25f;

    private Sprite2D _sprite;
    private Area2D _area;
    private CollisionShape2D _collisionShape;

    public override void _Ready()
    {   
        Position = new Vector2(100, 500);
        GD.Print("CardScene_Ready()");
        //_sprite = GetNode<Sprite2D>("Sprite2D");

        if (_sprite == null){
            GD.Print("Sprite not found");
        }
        else{
            GD.Print("Sprite is found");
        }

        if (string.IsNullOrEmpty(texture))
        {
            GD.Print("Texture is null or empty");
        }
        else
        {
            GD.Print("Texture is set: " + texture);
        }
    }
    

    public void Initialize(string name, int power, string desc, string texture)
    {
        this.name = name;
        this.power = power;
        this.desc = desc;
        this.texture = texture;

        if (_sprite == null)
        {
            _sprite = GetNode<Sprite2D>("Sprite2D");
            if (_sprite == null)
            {
                GD.PrintErr("Sprite2D not found in scene!");
                return;
            }
        }

        Texture2D tex = GD.Load<Texture2D>(texture);
        _sprite.Texture = tex;
        _sprite.Scale = new Vector2(cardScale, cardScale);

        Vector2 texSize = _sprite.Texture.GetSize() * cardScale;
        _collisionShape = GetNode<CollisionShape2D>("Area2D/CollisionShape2D");

        if (_collisionShape.Shape is RectangleShape2D rectangleShape)
        {
            rectangleShape.Size = texSize;
        }
    }
}
