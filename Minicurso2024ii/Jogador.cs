using Godot;
using System;

public partial class Jogador : CharacterBody2D
{
	[Export] private float velocidade = 300.0f; //autoexplicativo
	[Export] private float altura_pulo = -400.0f; //salto é negativo, pois quanto meno o valor de y, mais para cima o personagem vai
	private Vector2 movimento; //combinação de velocidade e direção
	private Vector2 direcao; //direção no eixo x,y. Sendo 0,0 parado e 1,0 indo para direita

	/*	O _PhysicsProcess() vai sempre ser executado 60 vezes por segundo.
		Pode-se editar o número em Menu -> Projeto -> Project Settings -> Física -> Comum 
		-> ticks de física por segundo

		Já um _Process() vai executar o máximo de vezes que puder e um segundo */
	
	public override void _PhysicsProcess(double delta) {
		movimento = Velocity; // Copia o movimento atual, para podermos editar

		/*	IsOnFloor() é uma função do CharacterBody2D que retorna verdadeiro (true)
		    quando o jogador está em contato com o chão. 
			Sendo assim, se o jogador não estiver no chão
			A posição y dele irá aumentar, ou seja cair na velocidade presente em GetGravity()
			Para mudar o valor da gravidade basta ir em Menu -> Projeto -> Project Settings -> Física ->
			Física -> 2D -> gravidade
			A única justificativa para usarmos o valor de gravidade do sistema e não um valor direto
			é que não precisaremos entrar em cada código com a variável gravidade 
			para alterarmos isso no nosso jogo.
		*/
		if (IsOnFloor() == false) { 
			movimento += GetGravity() * (float)delta;
		}

		// Aplica ao eixo Y do movimento a altura_pulo se apertar enter e estiver no chão
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor()) {
			movimento.Y = altura_pulo;
		}

		//A função Input.GetVector() facilita o processo de fazer um if para cada direção
		direcao = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		
		if (direcao != Vector2.Zero) { //se a direção for diferente de zero
			movimento.X = direcao.X * velocidade;
		}
		else {
			/*Essa função faz o efeito de desaceletar.
			  Para isso são passados respectivamente:
			  velocidade inicial, velocidade final e a velocidade que irá frear.
			  Como a velocidade de inicial e de frear são iguais ele para instantaneamente,
			  sendo assim essa linha de código tem o mesmo efeito que escrever
			  movimento.X = 0; */
			movimento.X = Mathf.MoveToward(Velocity.X, 0, velocidade);
		}
        
		Velocity = movimento; //substitui o movimento atual pelo editado
		MoveAndSlide(); //função do CharacterBody2D que aplica os movimentos e a física
	}
}
