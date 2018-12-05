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
	public partial class Chart: ContentView 
	{
        Dictionary<string, SKColor> nameToColor = new Dictionary<string, SKColor>();

        SKPaint fillPaint = new SKPaint
        {
            Style = SKPaintStyle.Fill,
            Color = SKColors.Black
        };

        private float lastTouch = 0;
        private int lastSnap = 0;
        private float lastValue = 0;
        private bool canMove = false;
        private bool animating = false;

        private int STROKE_SIZE = 4;

        private List<Company> companies;

        public Chart ()
		{
			InitializeComponent ();
            nameToColor.Add("first", SKColors.Red);
            nameToColor.Add("second", SKColors.Blue);
            canvasView.EnableTouchEvents = true;
            canvasView.Touch += OnTouch;
            lastTouch = float.MaxValue;
            AnimateBar();
		}

        private void OnTouch(object sender, SKTouchEventArgs touch){

            switch (touch.ActionType)
            {
                case SKTouchAction.Moved:
                    if (!canMove)
                        break;
                    lastTouch = touch.Location.X; 
                    canvasView.InvalidateSurface();
                    break;
                case SKTouchAction.Pressed:
                    canMove = true;
                    lastTouch = touch.Location.X; 
                    canvasView.InvalidateSurface();
                    break;
                case SKTouchAction.Released:
                    canMove = false;
                    canvasView.InvalidateSurface();
                    break;
            }
        }

        private float Clamp01(float value)
        {
            return value < 0f ? 0f : (value > 1f ? 1f : value);
        }

        private float Clamp(float value, float min, float max)
        {
            return value < min ? min : (value > max ? max : value);
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

        private string FormatValue(float value) {
            
            if(value > 1000000)
                return (value / 1000000D).ToString("0M");
            if (value > 1000)
            {
                return (value / 1000D).ToString("0K");
            }
            else
                return value.ToString("0");

        }

        private void DrawVerticalLabel(SKCanvas canvas, SKRect valuesRect, float minValue, float maxValue, float n)
        {
            float textSize = Math.Min(valuesRect.Width, valuesRect.Height)*0.5f;

            float startY = valuesRect.Bottom;
            float endY = valuesRect.Top;

            SKPaint paint = new SKPaint { Color = SKColors.Gray, Style = SKPaintStyle.Fill, TextSize=textSize, TextAlign = SKTextAlign.Left};

            Random r = new Random();

            for(int i = 0; i <= n; i++)
            {
                float value = Lerp(minValue, maxValue, (float)i / n);
                float posY = Lerp(startY, endY, (float)i / n);
                paint.TextSize = textSize; 

                SKRect bounds = new SKRect();

                string text = FormatValue(value);
                paint.MeasureText(text, ref bounds);



                long chars = paint.BreakText(text, valuesRect.Width);

                float newTextSize = valuesRect.Width * textSize/bounds.Width;
                newTextSize = Math.Min(textSize, newTextSize);
                paint.TextSize = newTextSize;

                paint.MeasureText(text, ref bounds);
                posY += i==0? 0 : i == n ? bounds.Height : bounds.Height/2f;

                 
                SKPoint position = 
                    new SKPoint(valuesRect.Left + 10, posY);
                canvas.DrawText(text, position, paint);
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
            float min = Math.Min(valuesRect.Width, valuesRect.Height)*0.45f;
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
                //   i == 0 ? SKTextAlign.Left : 
                    i == n ? SKTextAlign.Center : 
                  SKTextAlign.Left;

                //paint.TextAlign = SKTextAlign.Left;

                canvas.Save();
                {
                    SKPoint point = new SKPoint(pos, valuesRect.Top + min * (i==n? 1.65f:1));
                    canvas.Translate(point);
                    canvas.RotateDegrees(20);
                    canvas.DrawText(format(new DateTime(ticks)), new SKPoint(0,0), paint);
                }
                canvas.Restore();


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

            SKPoint[] shaderPoints =
            {
                new SKPoint(chartRect.Left, chartRect.Top),
                new SKPoint(chartRect.Left, chartRect.Bottom)
            };

            for(int i = 0; i < companies.Length; i++)
            {

                Company company = companies[i];
                List<SKPoint> points = GetPointsNormalized(company.History, minX, maxX, minY, maxY, chartRect);

                DrawHorizontalLabel(canvas, labelRect, minX, maxX, 4, 
                    (time) => { return time.Date.Day.ToString() + "/" + time.Month.ToString() +"/"+ time.Year.ToString(); }
                );

                DrawVerticalLabel(canvas, valuesRect, minY, maxY, 6);
                            
                SKPath path = GetPath(points);
                SKPath poly = GetPolygon(points, chartRect);

                SKColor color = nameToColor[company.name];
                SKColor color2 = new SKColor(color.Red, color.Green, color.Blue, 100);
                SKColor color3 = new SKColor(color.Red, color.Green, color.Blue, 10);

                SKPaint gradient =
                    new SKPaint
                    {
                        Style = SKPaintStyle.Fill,
                        Shader = SKShader.CreateLinearGradient(
                        shaderPoints[0], shaderPoints[1],
                        new SKColor[] { color2, color3 },
                        new float[] { 0, 1 },
                        SKShaderTileMode.Repeat)
                    };

                canvas.DrawPath(poly, gradient);
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
                    name="first",
                    History = new StockData[]
                    {
                        new StockData(100f, "1999-12-12"),
                        new StockData(301200.5f, "2000-12-12"),
                        new StockData(441300f, "2001-12-12"),
                        new StockData(111400f, "2002-12-12"),
                        new StockData(10500.5f, "2003-12-12"),
                        new StockData(20100f, "2004-12-12"),
                    }
                },
                new Company
                {
                    name="second",
                    History = new StockData[]
                    {
                        new StockData(300.05f, "1999-12-12"),
                        new StockData(100.10f, "2000-12-12"),
                        new StockData(1029400f, "2001-12-12"),
                        new StockData(20245f, "2002-12-12"),
                        new StockData(11190f, "2003-12-12"),
                        new StockData(51550f, "2004-12-12"),
                    }
                }
            };
        }
        
        private float Normalize(float value, float min, float max)
        {
            float v = value - min;
            float v2 = max - min;
            return v / v2;
        }

        async Task AnimateBar()
        {
            while (true)
            {
                if (animating)
                {
                    canvasView.InvalidateSurface();
                }
                await Task.Delay(TimeSpan.FromSeconds(0.1f / 30));
            }
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
            SKRect chartYLabel = new SKRect(width - labelYWidth - xOffset, yOffset, width -xOffset, height - labelXHeight - yOffset);
            SKRect chartXLabel = new SKRect(xOffset, height - labelXHeight - yOffset, width - labelYWidth -xOffset, height -yOffset);
            lastTouch = Clamp(lastTouch, chartRect.Left, chartRect.Right);

            canvas.Clear(SKColors.White);



            SKPaint line = new SKPaint { Color = SKColors.Black, StrokeWidth = 2, Style = SKPaintStyle.StrokeAndFill};

            int numberOfElements = GetDummies()[0].History.Length - 1;
            float offset = (chartRect.Width / numberOfElements) / 2f;
            lastSnap = (int)(numberOfElements * Normalize(lastTouch + offset, chartRect.Left, chartRect.Right));

            float barPos = chartRect.Left + chartRect.Width*(lastSnap / (float)numberOfElements);
            float dist = Math.Abs(lastValue - barPos);
            lastValue = Lerp(lastValue, barPos, 0.2f);
            if (dist < 2)
            {
                lastValue = barPos;
                animating = false;

            }
            else
            {
                if (!animating)
                {
                    animating = true;
                }
                       
            }

            SKPoint[] points =
            {
                new SKPoint(lastValue, chartRect.Top),
                new SKPoint(lastValue, chartRect.Bottom)
            };
            canvas.DrawLine(points[0], points[1], line);
            canvas.DrawCircle(points[0].X, (chartRect.Top + chartRect.Bottom) / 2f, 4, line);
            canvas.DrawLine(points[0], points[1], new SKPaint { Color = SKColors.Black, StrokeWidth=4 });

            //canvas.DrawRect(chartRect, new SKPaint{ Style = SKPaintStyle.Fill, Color = SKColors.Red });
            //canvas.DrawRect(chartXLabel, new SKPaint{ Style = SKPaintStyle.Fill, Color = SKColors.Green });
            //canvas.DrawRect(chartYLabel, new SKPaint{ Style = SKPaintStyle.Fill, Color = SKColors.Blue });

            DrawData(canvas, GetDummies(), chartRect, chartXLabel, chartYLabel); 
            
        }
	}
}