using Godot;
using System;

public partial class Pacman : Godot.CharacterBody2D
{
	private const float Speed = 128.0f;
	private AnimationTree _tree;
	

	public override void _Ready()
	{
		_tree = GetNode<AnimationTree>("AnimationTree");
		_tree.Active = true;
	}
	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
		
		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
			if (velocity.X == 0)
				velocity.Y = direction.Y * Speed;
			else
				velocity.Y = 0;
			_tree.Set("parameters/blend_position", velocity);
			
		}
		
		Velocity = velocity;
		MoveAndSlide();
	}
}
