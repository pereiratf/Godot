using Godot;
using System;

public class HUD : CanvasLayer
{
    //usaremos esse sinal para avisar o Main que o botão foi apertado
    [Signal]
    public delegate void StartGame();

    //essa função nos permitirá mostrar a mensagem por um tempo
    public void ShowMessage(string text) {
        var message = GetNode<Label>("Message");
        message.Text = text;
        message.Show();

        GetNode<Timer>("MessageTimer").Start();
    }

    //essa função é para indicar fim de jogo e início de um novo jogo
    async public void ShowGameOver() {
        ShowMessage("Game Over");

        var messageTimer = GetNode<Timer>("MessageTimer");
        await ToSignal(messageTimer, "timeout");

        var message = GetNode<Label>("Message");
        message.Text = "Dodge the\nCreeps!";
        message.Show();

        await ToSignal(GetTree().CreateTimer(1), "timeout");
        GetNode<Button>("StartButton").Show();
    }

    //função chamada no main sempre que o placar for atualizado
    public void UpdateScore(int Score) {
        GetNode<Label>("ScoreLabel").Text = Score.ToString();
    }

    //função chamada quando o botão for pressionado
    public void OnStartButtonPressed() {
        GetNode<Button>("StartButton").Hide();
        EmitSignal("StartGame");
    }

    //chamada com o sinal pressed()
    public void OnMessageTimerTimeout() {
        GetNode<Label>("Message").Hide();
    }


}
