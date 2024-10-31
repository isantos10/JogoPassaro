namespace JogoPassaro;

public partial class JgPage : ContentPage
{
	const int gravidade = 30;
	const int aberturaMinima = 10;
	const int tempoEntreFrames = 25;
	bool estaMorto = false;
	double larguraJanela = 0;
	double alturaJanela = 0;
	int velocidade = 20;
	const int maxTempoPulando = 3;
	int tempoPulando = 0;
	bool estaPulando = false;
	const int forcaPulo = 60;
	int score = 0;
    const int tamanhoMinimoPassagem = 200;


	public JgPage()
	{
		InitializeComponent();
	}
	protected override void OnAppearing()
	{
		base.OnAppearing();
		SoundHelper.Play("song.wav", true);
	}
	void AplicaPulo()
	{
		imgpassaro.TranslationY -= forcaPulo;
		tempoPulando++;
		if (tempoPulando >= maxTempoPulando)
		{
			estaPulando = false;
			tempoPulando = 0;
		}
	}
	void AplicaGravidade()
	{
		imgpassaro.TranslationY += gravidade;
	}

	async Task Desenhar()
	{
		while (!estaMorto)
		{
			if (estaPulando)
				AplicaPulo();
			else
				AplicaGravidade();

			
			GerenciaCanos();
			if (VerificaColisao())
			{
				estaMorto = true;
				FrameGameOver.IsVisible = true;
				break;
			}
			await Task.Delay(tempoEntreFrames);
		}
	}



	protected override void OnSizeAllocated(double w, double h)
	{
		base.OnSizeAllocated(w, h);
		larguraJanela = w;
		alturaJanela = h;
		if (h > 0)
		{
			CanoDeCima.HeightRequest = h;
			CanoDeBaixo.HeightRequest = h ;
		}
	}

	void GerenciaCanos()
	{
		CanoDeCima.TranslationX -= velocidade;
		CanoDeBaixo.TranslationX -= velocidade;
		if (CanoDeBaixo.TranslationX <= -larguraJanela)
		{
			CanoDeBaixo.TranslationX = 4;
			CanoDeCima.TranslationX = 4;

			var alturaMax = -100;
			var alturaMin = -CanoDeBaixo.HeightRequest;
			CanoDeCima.TranslationY = Random.Shared.Next((int)alturaMin, (int)alturaMax);
			CanoDeBaixo.TranslationY = CanoDeCima.TranslationY + aberturaMinima + CanoDeBaixo.HeightRequest;
			score++;
			LabelScore.Text = "Canos: " + score.ToString("D3");
            if (score % 4 == 0)
				velocidade++;


		}

	}
	void OnGameOverClicked(object s, TappedEventArgs a)
	{
		FrameGameOver.IsVisible = false;
		estaMorto = false;
		Inicializar();
		Desenhar();
		SoundHelper.Play("song.wav");
		LabelCanos.Text = "Você passou por " + score.ToString("D3") + " Canos!!";
	}

void Inicializar()
{
	CanoDeBaixo.TranslationX = -larguraJanela;
		CanoDeCima.TranslationX = -larguraJanela;
		imgpassaro.TranslationX = 0;
		imgpassaro.TranslationY = 0;
		score = 0;

		GerenciaCanos();
}

bool VerificaColisao()
{
	if (!estaMorto)
	{
		if (VerificaColisaoTeto() ||
		VerificaColisaoChao()||
		VerificaColisaoCanoCima())
		{
			return true;
		}

	}
	return false;
}
bool VerificaColisaoTeto()
{
	var minY = -alturaJanela / 2;
	if (imgpassaro.TranslationY <= minY)
		return true;
	else
		return false;
}
bool VerificaColisaoChao()
{
	var maxY = alturaJanela / 2 - FundoImg.HeightRequest;
	if (imgpassaro.TranslationY >= maxY)
		return true;
	else
		return false;

}

void OnGridClicked(object s, TappedEventArgs a)
{
	estaPulando = true;

}
bool VerificaColisaoCanoCima()
	{
		var posHPassaro = (larguraJanela / 2) - (imgpassaro.WidthRequest / 2);
		var posVPassaro = (alturaJanela / 2) - (imgpassaro.HeightRequest / 2) + imgpassaro.TranslationY;
		if (posHPassaro >= Math.Abs(CanoDeCima.TranslationX) - CanoDeCima.WidthRequest &&
		 posHPassaro <= Math.Abs(CanoDeCima.TranslationX) + CanoDeCima.WidthRequest &&
		 posVPassaro <= CanoDeCima.HeightRequest + CanoDeCima.TranslationY)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	 bool VerificaColisaoCanoBaixo()
  {
    var posicaoHorizontalPardal = larguraJanela - 50 - imgpassaro.WidthRequest / 2;
    var posicaoVerticalPardal   = (alturaJanela / 2) + (imgpassaro.HeightRequest / 2) + imgpassaro.TranslationY;

    var yMaxCano = CanoDeCima.HeightRequest + CanoDeCima.TranslationY + aberturaMinima;

    if (
         posicaoHorizontalPardal >= Math.Abs(CanoDeCima.TranslationX) - CanoDeCima.WidthRequest &&
         posicaoHorizontalPardal <= Math.Abs(CanoDeCima.TranslationX) + CanoDeCima.WidthRequest &&
         posicaoVerticalPardal   >= yMaxCano
       )
      return true;
    else
      return false;
  }


}