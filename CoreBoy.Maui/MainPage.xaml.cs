namespace CoreBoy.Maui;

public partial class MainPage : ContentPage
{

	public MainPage()
	{
		InitializeComponent();
		Loaded += MainPage_Loaded;
	}

	void MainPage_Loaded(object? sender, EventArgs e)
	{
		App.Emulator.Run();
	}
}
