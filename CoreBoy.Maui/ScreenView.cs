using CoreBoy.gpu;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;

namespace CoreBoy.Maui;
sealed partial class ScreenView : SKCanvasView, IDisplay
{
	public static readonly int DisplayWidth = 160;
	public static readonly int DisplayHeight = 144;
	public static int DisplayLength = DisplayWidth * DisplayHeight;

	public static readonly SKColor[] Colors = [0xe6f8da, 0x99c886, 0x437969, 0x051f2a];

	private readonly SKBitmap bitmap = new(DisplayWidth, DisplayHeight);
	private int currentPixel;

	public bool Enabled { get; set; }

	public event FrameProducedEventHandler? OnFrameProduced;

	public ScreenView()
	{
		App.SetDisplay(this);
	}

	protected override Size MeasureOverride(double widthConstraint, double heightConstraint)
	{
		var min = Math.Min(widthConstraint, heightConstraint);
		return new(min, min);
	}

	public void PutDmgPixel(int color)
	{
		var pixel = currentPixel++;
		var x = pixel % DisplayWidth;
		var y = pixel / DisplayWidth - 1;

		bitmap.SetPixel(x, y, Colors[color]);

		currentPixel %= DisplayLength;
	}
	int maxR;
	public void PutColorPixel(int gbcRgb)
	{
		var r = ((gbcRgb >> 0) & 0x1f) << 3;
		var g = ((gbcRgb >> 5) & 0x1f) << 3;
		var b = ((gbcRgb >> 10) & 0x1f) << 3;
		maxR = Math.Max(r, maxR);
		var pixel = currentPixel++;
		var x = pixel % DisplayWidth;
		var y = pixel / DisplayWidth;
		y += 1;
		bitmap.SetPixel(x, y,
			new SKColor((byte)r, (byte)g, (byte)b));

		currentPixel %= DisplayLength;
	}

	protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
	{
		var dirtyRect = e.Info.Rect;
		var canvas = e.Surface.Canvas;

		var vScale = dirtyRect.Height / DisplayHeight;
		var hScale = dirtyRect.Width / DisplayWidth;
		var scale = Math.Min(vScale, hScale);
		canvas.Scale(scale);
		canvas.Clear(SKColors.White);
		canvas.DrawBitmap(bitmap, new SKPoint());
	}

	public void RequestRefresh()
	{
		InvalidateSurface();
	}

	public void WaitForRefresh()
	{
	}

	public void Run(CancellationToken token)
	{
	}
}
