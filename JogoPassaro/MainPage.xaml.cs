namespace JogoPassaro;

public partial class MainPage : ContentPage
{

	public MainPage()
	{
		InitializeComponent();
	}

     private void BotaoIniciar(object sender, EventArgs args)
  {
      Application.Current.MainPage = new JgPage();
  }
}

