using Godot;
using System;

public partial class Door : Area2D
{
    public void BodyEntered(Node2D body)
    {
        if (!body.IsInGroup("Player"))
            return;
        
        foreach (Enemy e in GetTree().GetNodesInGroup("Enemy"))
        {
            if (!e.isDead())
                return;
        }

        GetTree().ReloadCurrentScene();
    }
}
