using Godot;

public partial class ProgressBar : Godot.ProgressBar
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void timeout()
	{
		Value -= .5;
		if (Value == 0)
			GetTree().ChangeSceneToFile("res://Scenes/Scene1.tscn");
	}
}