using Godot;

public partial class Game : Node2D
{
    
    //private Sprite2D _ballista;
    public override void _Ready(){
        
        base._Ready();
        
        UnitCreator creator = new UnitCreator();
        creator.LoadData();
        CardScene cardScene = creator.CreateCard("ballista");

        GD.Print(cardScene.texture.ToString());

        if (cardScene != null)
        {
            AddChild(cardScene);            
        }
    }
}
