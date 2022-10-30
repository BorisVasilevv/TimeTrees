using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TimeTrees.Core;


namespace TimeTrees.DesktopGui
{

    
    internal class AddConnectionTool
    {

        public static List<Connection> connections = new List<Connection>();
        public static Connection NewConnection = new Connection();
        public static Rectangle Rect1 = null;
        public static Rectangle Rect2 = null;
        public static TypeOfConnection TypeOfNewConnection { get; set; }   

        public static void AddConnection(object sender, MouseEventArgs e)
        {
            RectangleData data1,data2;
            data1 = data2 = null;
            if (MainWindow.AddRectTool.SelectedRectangle != null)
            {               
                if (Rect1 == null)
                {
                    Rect1 = MainWindow.AddRectTool.SelectedRectangle;
                }
                else if (Rect1 != MainWindow.AddRectTool.SelectedRectangle)
                {
                    Rect2 = MainWindow.AddRectTool.SelectedRectangle;
                    NewConnection.RectData1 = AddRectangleTool.RectangleDatas[Rect1];
                    NewConnection.RectData2= AddRectangleTool.RectangleDatas[Rect2];                  
                    if (IsConnectionNewTest(NewConnection))
                    {
                        
                        data1 = AddRectangleTool.RectangleDatas[Rect1];
                        data2 = AddRectangleTool.RectangleDatas[Rect2];
                        if (TypeOfNewConnection==TypeOfConnection.Spouse)
                        {
                            NewConnection.TypeOfFirstData = NewConnection.TypeOfSecondData = TypeOfConnection.Spouse;
                            if (data1.TypeandConnectionIds.ContainsKey(TypeOfConnection.Spouse))
                                data1.TypeandConnectionIds[TypeOfConnection.Spouse].Add((data2.Id));
                            else
                                data1.TypeandConnectionIds.Add(TypeOfConnection.Spouse, new List<int>() { data2.Id });
                        }
                        else
                        {
                            NewConnection.TypeOfFirstData = TypeOfConnection.Parent;
                            NewConnection.TypeOfSecondData = TypeOfConnection.Child;
                            if (data1.TypeandConnectionIds.ContainsKey(TypeOfConnection.Parent))
                                data1.TypeandConnectionIds[TypeOfConnection.Parent].Add((data2.Id));
                            else
                                data1.TypeandConnectionIds.Add(TypeOfConnection.Parent, new List<int>() { data2.Id });

                            if(data2.TypeandConnectionIds.ContainsKey(TypeOfConnection.Child))
                                data2.TypeandConnectionIds[TypeOfConnection.Child].Add((data2.Id));
                            else
                                data2.TypeandConnectionIds.Add(TypeOfConnection.Child, new List<int>() { data1.Id });
                        }
                        connections.Add(NewConnection);
                        Rect1 = Rect2 = null;
                    }                    
                    DrawConnections(connections);
                    NewConnection = new Connection();
                    MainWindow.Canvas1.MouseDown -= AddConnection;
                }
            }
            MainWindow.Canvas1.MouseDown += MainWindow.AddRectTool.RectMouseDown;
        }

        public static bool IsConnectionNewTest(Connection connectionToTest)
        {
            foreach (Connection connection in connections)
            {
                if ((connection.RectData1 == connectionToTest.RectData1 && connection.RectData2 == connectionToTest.RectData2)
                    || (connection.RectData1 == connectionToTest.RectData2 && connection.RectData2 == connectionToTest.RectData1))
                {
                    return false;
                }
            }
            return true;
        }

        public static void DrawConnections(List<Connection> connections)
        {
            foreach (Connection connection in connections)
            {
                if (connection.Polyline != null) MainWindow.Canvas1.Children.Remove(connection.Polyline); //удаление линии при перетаскивании прямоугольника
                Polyline line = new Polyline();
                PointCollection points = new PointCollection();
                Point point1 = connection.RectData1.RectCenter;
                Point point2 = connection.RectData2.RectCenter;

                if(connection.TypeOfFirstData == TypeOfConnection.Spouse)
                {
                    if(point1.X>point2.X)
                    {
                        point1.X -= 50;
                        point2.X += 50;
                    }
                    else
                    {
                        point2.X -= 50;
                        point1.X += 50;
                    }
                    points.Add(point1);
                    points.Add(point2);
                    line.Fill = Brushes.Blue;
                    line.Stroke = Brushes.Blue;
                }
                else if (connection.TypeOfFirstData == TypeOfConnection.Child&&connection.TypeOfSecondData == TypeOfConnection.Parent)
                {
                    point1.Y -= 50;
                    points.Add(point1);                    
                    point2.Y += 50;
                    points.Add(point2);

                    line.Fill = Brushes.Red;
                    line.Stroke = Brushes.Red;
                }
                else if (connection.TypeOfSecondData == TypeOfConnection.Child && connection.TypeOfFirstData == TypeOfConnection.Parent)
                {
                    point1.Y += 50;
                    points.Add(point1);
                    point2.Y -= 50;
                    points.Add(point2);
                    line.Fill = Brushes.Red;
                    line.Stroke = Brushes.Red;
                }
                line.Points = points;              
                Canvas.SetZIndex(line, 3);
                connection.Polyline = line;
                MainWindow.Canvas1.Children.Add(line);
            }
        }
    }
}
