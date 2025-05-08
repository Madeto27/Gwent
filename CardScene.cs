using Godot;

public partial class CardScene : Node2D
{
    public string name; 
    public int power;
    public string desc;
    public string texture;

    private Sprite2D _sprite;

    public override void _Ready()
    {   
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
    }
}
