using Godot;
using System;
using System.ComponentModel;
using System.Net;

public partial class Player : CharacterBody2D
{
	public const float Speed = 300.0f;
	public const float JumpVelocity = -500.0f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	public AnimationTree tree;
	public AnimatedSprite2D sprite;
	public Timer timer;

	int jumpCount = 0;
	bool isRunning;
	int attackType = 0;

    public override void _Ready()
    {
        base._Ready();
		tree = GetNode<AnimationTree>("AnimationTree");
		sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		timer = GetNode<Timer>("Timer");
		tree.Active = true;
		isRunning = false;
		attackType = 0;
    }
    public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
		AnimationNodeStateMachinePlayback state = (AnimationNodeStateMachinePlayback)tree.Get("parameters/playback");
		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y += gravity * (float)delta;
		else
			jumpCount = 0;

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_up"))
		{
			if (IsOnFloor())
			{
				velocity.Y = JumpVelocity;
				state.Travel("Jump");
				jumpCount++;
			}
			else if (jumpCount == 1)
			{
				velocity.Y = JumpVelocity * 2;
				state.Travel("High Jump");
				jumpCount++;
			}
		}
		
		if (Input.IsActionJustPressed("ui_accept"))
			timer.Start();
		if (timer.IsStopped())
			attackType = 0;
		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		float direction = Input.GetAxis("ui_left", "ui_right");
		if (direction != 0)
		{
			if (Input.IsActionPressed("run"))
			{
				direction *= 2;
				if (Input.IsActionJustPressed("ui_accept") || attackType == 1 || attackType == 2)
				{
					state.Travel("Run Attack");
					attackType = 3;
				}
				else if (IsOnFloor() && !isRunning && attackType == 0)
				{
					state.Travel("Run");
					isRunning = true;
				}
			}
			else
			{
				isRunning = false;
				if (Input.IsActionJustPressed("ui_accept") || attackType == 1 || attackType == 3)
				{
					state.Travel("Walk Attack");
					attackType = 2;
				}
				else if (IsOnFloor() && velocity.X == 0 && attackType == 0)
					state.Travel("Walk");
			}

			velocity.X = direction * Speed;
			if (direction < 0)
				sprite.Scale = sprite.Scale with {X = -2};
			else
				sprite.Scale = sprite.Scale with {X = 2};
		}
		else
		{
			isRunning = false;
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			if (Input.IsActionJustPressed("ui_accept") || attackType > 1)
			{
				state.Travel("Attack");
				attackType = 1;
			}
			if (IsOnFloor() && Velocity.X != 0 && velocity.X == 0 && attackType == 0)
				state.Travel("Idle");
		}
		Velocity = velocity;
		MoveAndSlide();
	}
}
