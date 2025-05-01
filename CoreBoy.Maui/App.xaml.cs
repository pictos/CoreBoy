using CoreBoy.gpu;
using CoreBoy.gui;

namespace CoreBoy.Maui;

public partial class App : Application
{
	public static Emulator Emulator { get; private set; } = default!;
	public static string RomPath { get; set; } = string.Empty;

	public App()
	{
		InitializeComponent();
		Emulator = new (new GameboyOptions { Rom = RomPath });
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new AppShell());
	}

	public static void SetDisplay(IDisplay display)
	{
		Emulator.Stop();
		Emulator.Display = display;
		Emulator.Run();
	}
}