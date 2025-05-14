using Godot;

public partial class Game : Node2D
{
    private State state;

    public void ChangeState(State state){
        this.state = state;
    }


    public override void _Ready(){
        
        base._Ready();
        
        //ChangeState(new PlayerTurnState(this));
        //create row in Game
        RowCreator rCreator= new RowCreator();
        RowScene row3 = rCreator.CreateRow(3);
        
        if (row3 != null){
            AddChild(row3);            
        }
    }
}
