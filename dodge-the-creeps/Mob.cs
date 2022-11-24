using Godot;
using System;

public class Mob : RigidBody2D
{

    // Criamos um vetor com nossas animações e escolhemos uma aleatoriamente
    public override void _Ready()
    {
        var animSprite = GetNode<AnimatedSprite>("AnimatedSprite");
        animSprite.Playing = true;
        string[] mobTypes = animSprite.Frames.GetAnimationNames();
        animSprite.Animation = mobTypes[GD.Randi() % mobTypes.Length];
    }

    public void OnVisibilityNotifier2DScreenExited()
    {
        QueueFree(); //deleta o inimigo quando sai da tela
    }


//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
