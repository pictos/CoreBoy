using CoreBoy.gpu;
using CoreBoy.gui;

namespace CoreBoy.Maui;

public partial class App : Application
{
	public static Emulator Emulator { get; private set; } = default!;
	public static string RomPath => field ??=GetRomPath();

	public App()
	{
		InitializeComponent();
		Emulator = new (new GameboyOptions { Rom = RomPath });
	}

	static string GetRomPath()
	{
		// Create a temporary file from the embedded resource
		var assembly = typeof(App).Assembly;
		var resourceName = "CoreBoy.Maui.Mario.gbc";

		using var stream = assembly.GetManifestResourceStream(resourceName);
		if (stream == null)
		{
			throw new InvalidOperationException($"Could not find embedded resource: {resourceName}");
		}

		// Create a temporary file to store the ROM
		var tempFile = Path.Combine(FileSystem.CacheDirectory, "Mario.gbc");

		using (var fileStream = File.Create(tempFile))
		{
			stream.CopyTo(fileStream);
		}

		return tempFile;

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