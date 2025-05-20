using Godot;

public partial class State : Node2D {

    public Game game;
    public virtual void Enter() {}
    public virtual void Exit() {}
    public virtual void Ready() {}
    public virtual void Update(float delta) { }
}