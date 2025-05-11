using Godot;

public class RowCreator
{
    public RowScene CreateRow(){
        var rowScene = GD.Load<PackedScene>("res://rowScene.tscn");
        RowScene row =  rowScene.Instantiate<RowScene>();
        row.Initialize();
        return row;
    }
}

