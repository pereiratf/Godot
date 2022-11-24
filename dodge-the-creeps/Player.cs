using Godot;
using System;

public class Player : Area2D {
    [Signal]
    public delegate void Hit();
    
    [Export]
    public int Speed = 400; //velocidade de movimento (pixel/segundo)

    public Vector2 ScreenSize; //tamanho da janela de jogo

    public override void _Ready()     {
        ScreenSize = GetViewportRect().Size;
        Hide();
    }

    public void Start(Vector2 pos)
    {
        Position = pos;
        Show();
        GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
    }

    public override void _Process(float delta) {
        var velocity = Vector2.Zero; // O Player inicia com movimento zero

        // Ao se movimentar ajusta a posição no eixo(x,y)
        if (Input.IsActionPressed("move_right")) {
            velocity.x += 1;
        }
        if (Input.IsActionPressed("move_left")) {
            velocity.x -= 1;
        }
        if (Input.IsActionPressed("move_down")) {
            velocity.y += 1;
        }
        if (Input.IsActionPressed("move_up")) {
            velocity.y -= 1;
        }

        //Normaliza velocity, evitando que o Player se mova mais rápido na diagonal
        var animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
        if (velocity.Length() > 0) {
            velocity = velocity.Normalized() * Speed;
            animatedSprite.Play();
        } else {
            animatedSprite.Stop();
        }

        //Atualiza a posição do Player
        Position += velocity * delta;
        Position = new Vector2(
            x: Mathf.Clamp(Position.x, 0, ScreenSize.x), 
            y: Mathf.Clamp(Position.y, 0, ScreenSize.y) );
            //Mathf.Clamp() impede o movimento do jogador para fora da tela

        if (velocity.x != 0) {
            animatedSprite.Animation = "walk";
            //animatedSprite.FlipV = false;
            animatedSprite.FlipH = velocity.x < 0;
        } else if (velocity.y != 0) {
            animatedSprite.Animation = "up";
            animatedSprite.FlipV = velocity.y > 0;
        }

    } //_Process()

    public void OnPlayerBodyEntered(PhysicsBody2D body)
    {
        Hide(); //O Jogador desaparece ao ser acertado
        EmitSignal(nameof(Hit));
        //SetDeferred irá fazer o comando esperar um momento seguro para executar a ação
        //Após a colisão iremos desabilitar o teste, pois não precisamos de novas chamadas
        GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled", true);
    }



} //Player
