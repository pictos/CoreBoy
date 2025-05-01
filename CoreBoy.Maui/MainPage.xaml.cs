using CoreBoy.controller;
using Button = Microsoft.Maui.Controls.Button;

namespace CoreBoy.Maui;

public partial class MainPage : ContentPage, IController
{
	IButtonListener? listener;
	public MainPage()
	{
		InitializeComponent();
		Loaded += MainPage_Loaded;
		App.Emulator.Controller = this;
	}

	public void SetButtonListener(IButtonListener listener)
	{
		this.listener = listener;
	}

	void MainPage_Loaded(object? sender, EventArgs e)
	{
		App.Emulator.Run();
	}

	void UpClicked(object sender, EventArgs e)
	{
		HandlePressed(controller.Button.Up, (Button)sender);
	}

	void LeftClicked(object sender, EventArgs e)
	{
		HandlePressed(controller.Button.Left, (Button)sender);
	}

	void RightClicked(object sender, EventArgs e)
	{

		HandlePressed(controller.Button.Right, (Button)sender);
	}

	void DownClicked(object sender, EventArgs e)
	{
		HandlePressed(controller.Button.Down, (Button)sender);
	}

	void AClicked(object sender, EventArgs e)
	{
		HandlePressed(controller.Button.A, (Button)sender);
	}



	void BClicked(object sender, EventArgs e)
	{

		HandlePressed(controller.Button.B, (Button)sender);
	}

	void StartClicked(object sender, EventArgs e)
	{

		HandlePressed(controller.Button.Start, (Button)sender);
	}

	void SelectClicked(object sender, EventArgs e)
	{

		HandlePressed(controller.Button.Select, (Button)sender);
	}

	void HandlePressed(controller.Button gButton, Button mButton)
	{
		if (listener is null)
		{
			return;
		}

		if (mButton.IsPressed)
		{
			listener.OnButtonPress(gButton);
		}
		else
		{
			listener.OnButtonRelease(gButton);
		}
	}
}
