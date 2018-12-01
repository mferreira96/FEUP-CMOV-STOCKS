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
        Dictionary<string, SKColor> nameToColor = new Dictionary<string, SKColor>();

        SKPaint fillPaint = new SKPaint
        {
            Style = SKPaintStyle.Fill,
            Color = SKColors.Black
        };

        private int STROKE_SIZE = 4;

		public Chart ()
		{
			InitializeComponent ();
            nameToColor.Add("first", SKColors.Red);
            nameToColor.Add("second", SKColors.Blue);
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
                    X = To01(minX, maxX, el.Date) * chart.Width + chart.Left,
                    Y = chart.Height - To01(minY, maxY, el.Value) * chart.Height + chart.Top
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

        private void CalculateMinMax(Company[] companies, 
            ref DateTime minX, ref DateTime maxX,
            ref float minY, ref float maxY)
        {
            for(int i = 0; i < companies.Length; i++)
            {
                for(int j = 0; j < companies[i].History.Length; j++)
                {
                    StockData data = companies[i].History[j];
                    minY = Math.Min(minY, data.Value);
                    maxY = Math.Max(maxY, data.Value);
                    
                    if(minX > data.Date)
                    {
                        minX = data.Date;
                    }else if(maxX < data.Date)
                    {
                        maxX = data.Date;
                    }
                }
            }
        }

        private float Lerp(float a, float b, float t)
        {
            return (1 - t) * a + b * t;
        }

        private long Lerp(long a, long b, double t)
        {
            return (long)((1 - t) * a + b * t);
        }

        private void DrawVerticalLabel(SKCanvas canvas, SKRect valuesRect, float minValue, float maxValue, float n)
        {
            float min = Math.Min(valuesRect.Width, valuesRect.Height)*0.5f;

            float startY = valuesRect.Bottom;
            float endY = valuesRect.Top;

            SKPaint paint = new SKPaint { Color = SKColors.Gray, Style = SKPaintStyle.Fill, TextSize=min, TextAlign = SKTextAlign.Left};

            for(int i = 0; i <= n; i++)
            {
                float value = Lerp(minValue, maxValue, (float)i / n);
                float posY = Lerp(startY, endY, (float)i / n);
                posY += i == n ? min : min/2f;
                  
                SKPoint position = 
                    new SKPoint(valuesRect.Left + 5, posY);
                canvas.DrawText(String.Format("{0:0}",value), position, paint);
            }
        }

        private void DrawHorizontalStripes(SKCanvas canvas, SKRect rect, int n)
        {
            SKPaint paint = new SKPaint { Color = SKColors.Gray, Style = SKPaintStyle.Fill, TextAlign = SKTextAlign.Left};
            float start = rect.Bottom;
            float end = rect.Top;
            for(int i = 0; i <= n; i++)
            {
                paint.StrokeWidth = i==0? 3: 1;
                float pos = Lerp(start, end, (float)i / n);
                SKPoint p1 = new SKPoint(rect.Left, pos);
                SKPoint p2 = new SKPoint(rect.Right, pos);
                canvas.DrawLine(p1, p2, paint);
            }
        }

        private void DrawHorizontalLabel(SKCanvas canvas, SKRect valuesRect, DateTime minValue, DateTime maxValue, int n, Func<DateTime, String> format)
        {
            float min = Math.Min(valuesRect.Width, valuesRect.Height)*0.5f;
            SKPaint paint = new SKPaint { Color = SKColors.Gray, Style = SKPaintStyle.Fill, TextSize=min, TextAlign = SKTextAlign.Left};

            long startTicks = minValue.Ticks;
            long endTicks = maxValue.Ticks;
            float startPos = valuesRect.Left;
            float endPos = valuesRect.Right;

            for(int i = 0; i <= n; i++)
            {
                long ticks = Lerp(startTicks, endTicks, (double)i / n);
                float pos = Lerp(startPos, endPos, (float)i / n);

                paint.TextAlign = 
                    i == 0 ? SKTextAlign.Left : 
                    i == n ? SKTextAlign.Right : 
                    SKTextAlign.Center;

                SKPoint point = new SKPoint(pos, valuesRect.Top + min);
                canvas.DrawText(format(new DateTime(ticks)), point, paint);

            } 
        }
        
        private void DrawData(SKCanvas canvas, Company[] companies, SKRect chartRect, SKRect labelRect, SKRect valuesRect)
        {
            DateTime now = DateTime.Now;
            DateTime minX, maxX;
            float minY, maxY;
            minX = maxX = companies[0].History[0].Date;
            minY = maxY = companies[0].History[0].Value;

            CalculateMinMax(companies, ref minX, ref maxX, ref minY, ref maxY);
            minY = 0f;

            for(int i = 0; i < companies.Length; i++)
            {

                Company company = companies[i];
                List<SKPoint> points = GetPointsNormalized(company.History, minX, maxX, minY, maxY, chartRect);
                DrawHorizontalLabel(canvas, labelRect, minX, maxX, 4, 
                    (time) => { return time.Year.ToString(); }
                );

                DrawVerticalLabel(canvas, valuesRect, minY, maxY, 6);
                            
                SKPath path = GetPath(points);
                SKPath poly = GetPolygon(points, chartRect);

                SKColor color = nameToColor[company.Name];
                SKColor color2 = new SKColor(color.Red, color.Green, color.Blue, 100);

                canvas.DrawPath(poly, new SKPaint { Style = SKPaintStyle.Fill, Color = color2 });
                canvas.DrawPath(path, new SKPaint { Style = SKPaintStyle.Stroke, Color = color, StrokeWidth=STROKE_SIZE, IsAntialias=true });

                SKPaint white = new SKPaint { Color = SKColors.White, Style = SKPaintStyle.Fill };
                SKPaint border = new SKPaint { Color = color, Style = SKPaintStyle.Stroke, StrokeWidth=STROKE_SIZE, IsAntialias=true };
                for(int j = 0; j < path.PointCount; j++)
                {
                    SKPoint p = path.Points[j];
                    canvas.DrawCircle(p, STROKE_SIZE, white);
                    canvas.DrawCircle(p, STROKE_SIZE, border);
                }

            }


            
            DrawHorizontalStripes(canvas, chartRect, 6);
        } 

        private Company[] GetDummies()
        {
            return new Company[]
            {
                new Company
                {
                    Name="first",
                    History = new StockData[]
                    {
                        new StockData(100, "1999-12-12"),
                        new StockData(200, "2000-12-12"),
                        new StockData(300, "2001-12-12"),
                        new StockData(400, "2002-12-12"),
                        new StockData(500, "2003-12-12"),
                        new StockData(100, "2004-12-12"),
                    }
                },
                new Company
                {
                    Name="second",
                    History = new StockData[]
                    {
                        new StockData(300, "1999-12-12"),
                        new StockData(100, "2000-12-12"),
                        new StockData(400, "2001-12-12"),
                        new StockData(245, "2002-12-12"),
                        new StockData(190, "2003-12-12"),
                        new StockData(550, "2004-12-12"),
                    }
                }
            };
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

            int yOffset = 20;
            int xOffset = 20;

            SKRect chartRect = new SKRect(xOffset, yOffset, width - labelYWidth -xOffset, height - labelXHeight -yOffset);
            SKRect chartYLabel = new SKRect(width - labelYWidth - xOffset, 0, width -xOffset, height - labelXHeight - yOffset);
            SKRect chartXLabel = new SKRect(xOffset, height - labelXHeight - yOffset, width - labelYWidth -xOffset, height -yOffset);

            canvas.Clear(SKColors.White);

//            canvas.DrawRect(chartRect, new SKPaint{ Style = SKPaintStyle.Fill, Color = SKColors.White });
 //           canvas.DrawRect(chartXLabel, new SKPaint{ Style = SKPaintStyle.Fill, Color = SKColors.Green });
  //          canvas.DrawRect(chartYLabel, new SKPaint{ Style = SKPaintStyle.Fill, Color = SKColors.Blue });

            DrawData(canvas, GetDummies(), chartRect, chartXLabel, chartYLabel); 
            
        }
	}
}