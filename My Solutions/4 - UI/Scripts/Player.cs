

using System;
using Godot;

public partial class Player : CharacterBody2D
{
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	PackedScene bullet;
	Node2D bulletNode;
	public override void _Ready()
	{
		Tween tween = GetTree().CreateTween().BindNode(this).SetTrans(Tween.TransitionType.Linear);
		tween.TweenProperty(this, "rotation_degrees", -10, 3).AsRelative();
		tween.TweenProperty(this, "rotation_degrees", 10, 3).AsRelative();
		tween.SetLoops();
		tween.Play();
		bullet = GD.Load<PackedScene>("res://Scenes/Bullet.tscn");
	}
	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y += gravity * (float)delta;

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
			velocity.Y = JumpVelocity;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		}
		
		if (Input.IsActionJustPressed("click") && bulletNode == null)
		{
			bulletNode = bullet.Instantiate<Node2D>();
			AddChild(bulletNode);
			bulletNode.GlobalPosition = this.GlobalPosition;
			Tween tween = bulletNode.CreateTween();
			tween.TweenProperty(bulletNode, "position", GetLocalMousePosition(), 1);
			tween.TweenCallback(Callable.From(bulletNode.QueueFree));
			tween.TweenCallback(Callable.From(() => bulletNode = null));
			tween.Play();
		}
		Velocity = velocity;
		MoveAndSlide();
	}

}
