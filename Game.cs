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
            row3.GlobalPosition = new Vector2(1920/2,500);        
        }

        RowScene row2 = rCreator.CreateRow(2);
        
        if (row2 != null){
            AddChild(row2);
            row2.GlobalPosition = new Vector2(1920/2,300);          
        }

        RowScene row1 = rCreator.CreateRow(1);
        
        if (row1 != null){
            AddChild(row1);
            row1.GlobalPosition = new Vector2(1920/2,100);       
        }
    }
}
