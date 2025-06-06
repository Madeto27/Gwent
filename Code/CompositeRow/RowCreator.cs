using Godot;

public class RowCreator
{
    public RowScene CreateRow(int row){
        var rowLoad = GD.Load<PackedScene>("res://Scenes/rowScene.tscn");
        RowScene rowScene =  rowLoad.Instantiate<RowScene>();
        rowScene.Initialize(row);
        return rowScene;
    }
}

