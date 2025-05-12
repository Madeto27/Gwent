using Godot;

public partial class Game : Node2D
{
    public override void _Ready(){
        
        base._Ready();
        
        //create ballista in Game
        UnitCreator uCreator = new UnitCreator();
        uCreator.LoadData();
        CardScene ballista = uCreator.CreateCard("ballista");

        GD.Print(ballista.texture.ToString());

        if (ballista != null){
            AddChild(ballista);            
        }

        //create infantry in game
        CardScene infantry = uCreator.CreateCard("poorInfantry");
        GD.Print(infantry.texture.ToString());
        if (infantry != null){
            AddChild(infantry);
        }

        //create row in Game
        RowCreator rCreator= new RowCreator();
        RowScene row3 = rCreator.CreateRow();
        
        if (row3 != null){
            AddChild(row3);            
        }
    }
}
