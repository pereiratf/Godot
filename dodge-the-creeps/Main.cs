using Godot;
using System;

public class Main : Node {
    //o pragma remove o alerta do editor sobre essa cena não ter sido atribuida
    #pragma warning disable 649
    [Export]
    public PackedScene MobScene;
    #pragma warning restore 649
    public int Score; //placar
    
    public override void _Ready() {
        GD.Randomize(); //semente do random
        //NewGame(); //descomente apenas para testar a cena principal
    }

    public void GameOver() {
        GetNode<Timer>("MobTimer").Stop();
        GetNode<Timer>("ScoreTimer").Stop();

        //Adicionado após a inserção do HUD
        GetNode<HUD>("HUD").ShowGameOver();
    }

    public void NewGame() {
        //Para limpar os mobs antes de um novo jogo
        GetTree().CallGroup("mobs", "queue_free");
        //Código adicionado após a conexão com o HUD
        var hud = GetNode<HUD>("HUD");
        hud.UpdateScore(Score);
        hud.ShowMessage("Get Ready!");

        Score = 0;

        var player = GetNode<Player>("Player");
        var startPosition = GetNode<Position2D>("StartPosition");
        player.Start(startPosition.Position);

        GetNode<Timer>("StartTimer").Start();
    }

    public void OnScoreTimerTimeout() {
        Score++;

        //adicionado após a integração do HUD no Main
        GetNode<HUD>("HUD").UpdateScore(Score);
    }

    public void OnStartTimerTimeout() {
        GetNode<Timer>("MobTimer").Start();
        GetNode<Timer>("ScoreTimer").Start();
    }

    public void OnMobTimerTimeout()  {
        //cria uma nova instância de inimigo (mob)
        var mob = (Mob)MobScene.Instance();

        //escolhe uma posição aleatória na linha que fizemos com o Path2D
        var mobSpawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
        mobSpawnLocation.Offset = GD.Randi();

        //define a direção do inimigo perperdicular a posição escolhida
        float direction = mobSpawnLocation.Rotation + Mathf.Pi / 2;

        //colocamos o inimigo na posição escolhida
        mob.Position = mobSpawnLocation.Position;

        //incluímos alguma aleatoriedade a direção
        direction += (float)GD.RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);
        mob.Rotation = direction;

        //escolhemos a velocidade
        var velocity = new Vector2((float)GD.RandRange(150.0, 250.0), 0);
        mob.LinearVelocity = velocity.Rotated(direction);

        //adicionamos o inimigo a cena Main
        AddChild(mob);
    }

}
