using System;
using System.Threading.Tasks.Dataflow;
using Godot;

public partial class CharacterBody2D : Godot.CharacterBody2D
{
	public const float Speed = 300.0f;
	public float JumpVelocity = -600.0f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = 1000;

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y += gravity * (float)delta;

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_up") && IsOnFloor())
			velocity.Y = JumpVelocity;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		float xInput = Input.GetAxis("ui_left", "ui_right");
		if (xInput != 0)
			velocity.X = xInput * Speed;
		else
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);

		if(Input.IsActionJustPressed("ui_down"))
		{
			gravity *= -1;
			JumpVelocity *= -1;
			UpDirection *= Vector2.Up;
		}
		Velocity = velocity;
		MoveAndSlide();
	}
	
	private void enter(Node2D body)
	{
		GD.Print("hi");
	}

	public void exit(Node2D body)
	{
		GD.Print("bye");
	}
}
