using my_stocks.model;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace my_stocks.view
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Chart: ContentPage
	{
        SKPaint fillPaint = new SKPaint
        {
            Style = SKPaintStyle.Fill,
            Color = SKColors.Black
        };

		public Chart ()
		{
			InitializeComponent ();
		}

        private float Clamp01(float value)
        {
            return value < 0f ? 0f : (value > 1f ? 1f : value);
        }

        private float To01(float min, float max, float value)
        {
            float dv = max - min;
            float v = value - min;
            float res = v / dv;
            return Clamp01(res);
        }

        private float To01(DateTime min, DateTime max, DateTime value)
        {
            TimeSpan dv = max - min;
            TimeSpan v = value - min;
            double res = v.TotalSeconds / dv.TotalSeconds;
            return Clamp01((float)res);
        }

        private List<SKPoint> GetPointsNormalized(StockData[] data, DateTime minX, DateTime maxX, float minY, float maxY, SKRect chart)
        {
            List<SKPoint> points = new List<SKPoint>();
            for(int i = 0; i < data.Length; i++)
            {
                StockData el = data[i];

                SKPoint point = new SKPoint
                {
                    X = To01(minX, maxX, el.Date) * chart.Width,
                    Y = To01(minY, maxY, el.Value) * chart.Height
                };
                points.Add(point);
            }
            return points;
        }

        private SKPath GetPolygon(List<SKPoint> points, SKRect rect)
        {
            SKPoint p1 = new SKPoint(rect.Left + rect.Width, rect.Top + rect.Height);
            SKPoint p2 = new SKPoint(rect.Left, rect.Top + rect.Height);
            points.Add(p1);
            points.Add(p2);
            SKPath path = new SKPath();
            path.FillType = SKPathFillType.EvenOdd;
            path.AddPoly(points.ToArray(), true);
            return path;
        }

        private SKPath GetPath(List<SKPoint> points)
        {
            SKPath path = new SKPath();
            path.FillType = SKPathFillType.Winding;
            path.AddPoly(points.ToArray(), false);
            return path;
        }
        
        private void DrawData(SKCanvas canvas, Company[] companies, SKRect chartRect)
        {
            if (companies == null) return;
            DateTime now = DateTime.Now;
            List<SKPoint> points = GetPointsNormalized(companies[0].History, now, now, -1, 1, chartRect);

                        
            SKPath path = GetPath(points);
            SKPath poly = GetPolygon(points, chartRect);
            
            SKColor color = new SKColor(255, 0, 0, 100);
            canvas.DrawPath(poly, new SKPaint { Style = SKPaintStyle.Fill, Color = color });
            canvas.DrawPath(path, new SKPaint { Style = SKPaintStyle.Stroke, Color = SKColors.Red, StrokeWidth=5, IsAntialias=true });
        } 

        private void canvasView_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;

            int width = e.Info.Width;
            int height = e.Info.Height;
            int min = Math.Min(width, height);
            int labelXHeight = (int)(min * 0.1f);
            int labelYWidth = (int)(min * 0.1f);
            
            SKRect chartRect = new SKRect(0, 0, width - labelYWidth, height - labelXHeight);
            SKRect chartYLabel = new SKRect(width - labelYWidth, 0, width, height - labelXHeight);
            SKRect chartXLabel = new SKRect(0, height - labelXHeight, width - labelYWidth, height);

            canvas.Clear(SKColors.White);

            canvas.DrawRect(chartRect, new SKPaint{ Style = SKPaintStyle.Fill, Color = SKColors.White });
            canvas.DrawRect(chartXLabel, new SKPaint{ Style = SKPaintStyle.Fill, Color = SKColors.Green });
            canvas.DrawRect(chartYLabel, new SKPaint{ Style = SKPaintStyle.Fill, Color = SKColors.Blue });

            DrawData(canvas, null, chartRect); 
            
        }
	}
}