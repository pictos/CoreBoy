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
		var x = currentPixel % DisplayWidth;
		var y = currentPixel / DisplayWidth - 1;
    
		if (y >= 0 && y < DisplayHeight && x >= 0 && x < DisplayWidth)
		{
			bitmap.SetPixel(x, y, Colors[color]);
		}
    
		currentPixel = (currentPixel + 1) % DisplayLength;
	}
	// public void PutDmgPixel(int color)
	// {
	// 	var pixel = currentPixel++;
	// 	var x = pixel % DisplayWidth;
	// 	// var y = pixel / DisplayWidth - 1;
	// 	var y = pixel / DisplayWidth ;
	// 	bitmap.SetPixel(x, y, Colors[color]);
	//
	// 	currentPixel %= DisplayLength;
	// }
	public void PutColorPixel(int gbcRgb)
	{
		var r = ((gbcRgb >> 0) & 0x1f) << 3;
		var g = ((gbcRgb >> 5) & 0x1f) << 3;
		var b = ((gbcRgb >> 10) & 0x1f) << 3;
    
		var x = currentPixel % DisplayWidth;
		var y = currentPixel / DisplayWidth;
		y+=1;
		
		if (y >= 0 && y < DisplayHeight && x >= 0 && x < DisplayWidth)
		{
			bitmap.SetPixel(x, y, new SKColor((byte)r, (byte)g, (byte)b));
		}
    
		currentPixel = (currentPixel + 1) % DisplayLength;
	}
	int maxR;
	// public void PutColorPixel(int gbcRgb)
	// {
	// 	var r = ((gbcRgb >> 0) & 0x1f) << 3;
	// 	var g = ((gbcRgb >> 5) & 0x1f) << 3;
	// 	var b = ((gbcRgb >> 10) & 0x1f) << 3;
	// 	maxR = Math.Max(r, maxR);
	// 	var pixel = currentPixel++;
	// 	var x = pixel % DisplayWidth;
	// 	var y = pixel / DisplayWidth;
	// 	// y += 1;
	//
	// 	if (y >= 0 && y < DisplayHeight)
	// 	{
	// 		bitmap.SetPixel(x, y, new SKColor((byte)r, (byte)g, (byte)b));
	// 	}
	// 	else
	// 	{
	// 		_ = 1;
	// 	}
	// 	//bitmap.SetPixel(x, y, new SKColor((byte)r, (byte)g, (byte)b));
	//
	// 	currentPixel %= DisplayLength;
	// }

	protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
	{
		var dirtyRect = e.Info.Rect;
		var canvas = e.Surface.Canvas;

		var vScale = dirtyRect.Height / DisplayHeight;
		var hScale = dirtyRect.Width / DisplayWidth;
		var scale = Math.Min(vScale, hScale);
		
		// Calculate centering offsets
		float xOffset = (dirtyRect.Width - (DisplayWidth * scale)) / 2;
		float yOffset = (dirtyRect.Height - (DisplayHeight * scale)) / 2;
		
		canvas.Clear(SKColors.White);
		canvas.Save();
		canvas.Translate(xOffset, yOffset);
		canvas.Scale(scale);
		canvas.DrawBitmap(bitmap, new SKPoint());
		canvas.Restore();
	}

	public void RequestRefresh()
	{
		MainThread.BeginInvokeOnMainThread(InvalidateSurface);
	}

	public void WaitForRefresh()
	{
	}

	public void Run(CancellationToken token)
	{
	}
}
