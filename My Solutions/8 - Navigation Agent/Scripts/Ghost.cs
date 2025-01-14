using Godot;
using System;
using System.Runtime.Versioning;

public partial class Ghost : CharacterBody2D
{
	public const float Speed = 96.0f;
	private Pacman _player;
	private AnimationTree _tree;
	private NavigationAgent2D _nav;
	private bool _initialize;

	public override void _Ready()
	{
		_tree = GetNode<AnimationTree>("AnimationTree");
		_tree.Active = true;
		_player = (Pacman)GetTree().GetFirstNodeInGroup("Player");
		_nav = GetNode<Godot.NavigationAgent2D>("NavigationAgent2D");
		_initialize = false;
	}

	public void timeout() {
		_initialize = true;
	}
	
	public override void _PhysicsProcess(double delta)
	{
		if (!_initialize)
			return;
		Vector2 velocity = Velocity;
		_nav.TargetPosition = _player.GlobalPosition;
		velocity = ToLocal(_nav.GetNextPathPosition()).Normalized() * Speed;
		_tree.Set("parameters/blend_position", velocity);
		Velocity = velocity;
		MoveAndSlide();
	}
}
