using Godot;
using System;

public partial class SceneLight : DirectionalLight2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		if (Energy < 1)
			Energy += .001f;
	}
}
