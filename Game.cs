using Godot;

public partial class Game : Node2D
{
    
    //private Sprite2D _ballista;
    public override void _Ready(){
        
        base._Ready();
        
        UnitCreator uCreator = new UnitCreator();
        uCreator.LoadData();
        CardScene cardScene = uCreator.CreateCard("ballista");

        GD.Print(cardScene.texture.ToString());

        if (cardScene != null){
            AddChild(cardScene);            
        }

        RowCreator rCreator= new RowCreator();
        RowScene row3 = rCreator.CreateRow();
        
        if (row3 != null){
            AddChild(row3);            
        }
        //GD.Print(row3._area.CollisionMask);
        //cardScene.Position = new Vector3( , 0,);
    }
}
