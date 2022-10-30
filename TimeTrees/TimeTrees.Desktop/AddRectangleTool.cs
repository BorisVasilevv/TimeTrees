using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Shapes;
using TimeTrees.Core;

namespace TimeTrees.DesktopGui
{
    public class AddRectangleTool
    {
        private Rectangle NewRectangle;
        public Rectangle SelectedRectangle;
        public static Dictionary<Rectangle, RectangleData> RectangleDatas = new Dictionary<Rectangle, RectangleData>();
        public static Dictionary<Rectangle, List<TextBlock>> InformationOnRect = new Dictionary<Rectangle, List<TextBlock>>();


        //AddRectangleTool AddRectTool;
        //AddTextBoxTool AddTextTool;
        //Canvas Canvas1;
        public IdentificatorGenerator GeneratorId;
        public AddRectangleTool()
        {
            GeneratorId = new IdentificatorGenerator();
        }

        public void RectMouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.Canvas1.MouseMove -= NewRectMouseMove;
            MainWindow.Canvas1.MouseDown -= RectMouseDown;
            NewRectangle = null;
            SelectedRectangle = null;
        }

        public void NewRectMouseMove(object sender, MouseEventArgs e)
        {
            Point point = new Point();
            double mouseX = e.GetPosition(MainWindow.Canvas1).X;
            double mouseY = e.GetPosition(MainWindow.Canvas1).Y;
            if (NewRectangle == null)
            {
                RectangleData rectangleData = new RectangleData();
                NewRectangle = new Rectangle();
                NewRectangle.Width = 100;
                NewRectangle.Height = 100;
                NewRectangle.Fill = new SolidColorBrush(Colors.Brown);
                NewRectangle.Stroke = new SolidColorBrush(Colors.Black);
                NewRectangle.MouseMove += RectangleMouseMove;
                Canvas.SetZIndex(NewRectangle, 2);
                MainWindow.Canvas1.Children.Add(NewRectangle);
                rectangleData.Id = GeneratorId.GenerateId();
                point.X = mouseX;
                point.Y = mouseY;
                rectangleData.RectCenter = point;
                RectangleDatas.Add(NewRectangle, rectangleData);
            }
            Canvas.SetLeft(NewRectangle, mouseX - NewRectangle.Width / 2);
            Canvas.SetTop(NewRectangle, mouseY - NewRectangle.Height / 2);
        }

        public void RectangleMouseMove(object sender, MouseEventArgs e)
        {
            double mouseX = e.GetPosition(MainWindow.Canvas1).X;
            double mouseY = e.GetPosition(MainWindow.Canvas1).Y;
            selectedShapes(MainWindow.Canvas1);
            DrawSelection();
            if (SelectedRectangle != null && e.LeftButton == MouseButtonState.Pressed)
            {
                Point point = new Point();
                point.X = mouseX;
                point.Y = mouseY + SelectedRectangle.Height / 4;
                Canvas.SetLeft(SelectedRectangle, mouseX - SelectedRectangle.Width / 2);
                Canvas.SetTop(SelectedRectangle, mouseY - SelectedRectangle.Height / 4);
                RectangleData rectangleData = AddRectangleTool.RectangleDatas[SelectedRectangle];

                if (InformationOnRect.ContainsKey(SelectedRectangle))
                {
                    List<TextBlock> textBlocks = InformationOnRect[SelectedRectangle];
                    for (int i = 0; i < textBlocks.Count; i++)
                    {
                        //20 так как это высота прямоугольника 
                        //(высота прямоуг)/2-20*textBlocks.Count=-10 чтобы все текстблоки влезли
                        Canvas.SetTop(textBlocks[i], rectangleData.RectCenter.Y + (i) * 20 - 10);
                        Canvas.SetLeft(textBlocks[i], rectangleData.RectCenter.X - SelectedRectangle.Height / 2);
                    }
                }
                rectangleData.RectCenter = point;
                AddConnectionTool.DrawConnections(AddConnectionTool.connections);
            }
        }

        public static void clearSelection(object sender, MouseEventArgs e)
        {
            foreach (Rectangle rect in RectangleDatas.Keys)
                if (!rect.IsMouseOver)
                    rect.Effect = null;
        }

        private void selectedShapes(Canvas canvas)
        {
            foreach (var elem in canvas.Children)
            {
                if (elem is Rectangle rect)
                {
                    if (rect.IsMouseOver)
                    {
                        SelectedRectangle = rect;
                    }
                }
            }
        }

        private void DrawSelection()
        {
            if (SelectedRectangle != null)
                SelectedRectangle.Effect = new DropShadowEffect() { Color = Colors.Green };
        }
    }
}
