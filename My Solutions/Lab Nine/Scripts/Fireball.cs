using Godot;
using System;

public partial class Fireball : Area2D
{
	public Player _player;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void BodyEntered(Node2D body)
	{
		if (body.IsInGroup("Enemy"))
		{
			Enemy enemy = (Enemy)body;
			enemy.Attack();
			_player._isAttacking = false;
			QueueFree();
		}
	}
}
