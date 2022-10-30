using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Shapes;
using TimeTrees.Core;

namespace TimeTrees.DesktopGui
{
    internal class FileWorker
    {
        
        //Запись для WPF
        public static void SaveInFileFromCanvas(Dictionary<Rectangle,RectangleData> allElems, string fileName)
        {
            List<RectangleData> elemsToSave= new List<RectangleData>();
            foreach(RectangleData elem in allElems.Values)
            {
                elemsToSave.Add(elem);
            }
            string text = JsonConvert.SerializeObject(elemsToSave, Formatting.Indented);
            File.WriteAllText(fileName, text);
        }


        public static void ReadFromFileAndDraw(string fileName)
        {
            string text = File.ReadAllText(fileName);
            text = text.Replace("T00:00:00", string.Empty);
            List<RectangleData> rectangleDatas = JsonConvert.DeserializeObject<List<RectangleData>>(text);
            foreach (RectangleData data in rectangleDatas)
            {
                
                Rectangle rect = new Rectangle();
                rect.Height = 100;
                rect.Width = 100;
                rect.Fill= new SolidColorBrush(Colors.Brown);
                rect.Stroke= new SolidColorBrush(Colors.Black);
                rect.MouseMove += MainWindow.AddRectTool.RectangleMouseMove;
                AddRectangleTool.RectangleDatas.Add(rect, data);

                Canvas.SetZIndex(rect, 2);
                MainWindow.Canvas1.Children.Add(rect);
                Canvas.SetLeft(rect,data.RectCenter.X-rect.Width/2);
                Canvas.SetTop(rect,data.RectCenter.Y-rect.Height/2);

                //Добавляем информацию на прямоугольники
                Person person = data.Person;
                if (person!=null)
                {
                    List<string> dataToAddOnRect = person.DeathDate == null 
                        ? new List<string>() { data.Person.Name, person.BirthDate.ToString() , ""}
                        : new List<string>() { data.Person.Name, person.BirthDate.ToString(), person.DeathDate.ToString() };
                    MainWindow.AddInformationTool.AddInformationToRect(rect,dataToAddOnRect);
                }              
            }
            //Добавляем линии
            foreach (RectangleData data in rectangleDatas)
            {
                if (data.TypeandConnectionIds!=null&&data.TypeandConnectionIds.Count > 0)
                {
                    for (int i = 0; i < Enum.GetNames(typeof(TypeOfConnection)).Length; i++)
                    {
                        if (data.TypeandConnectionIds.ContainsKey((TypeOfConnection)i))
                        {
                            foreach (int rectDataId in data.TypeandConnectionIds[(TypeOfConnection)i])
                            {
                                Connection connection = new Connection();
                                connection.RectData1 = data;
                                connection.RectData2 = RectangleData.SearchRectData(rectDataId);
                                connection.TypeOfFirstData = (TypeOfConnection)i;

                                if (connection.TypeOfFirstData == TypeOfConnection.Spouse)
                                    connection.TypeOfSecondData = TypeOfConnection.Spouse;
                                else if (connection.TypeOfFirstData == TypeOfConnection.Parent)
                                    connection.TypeOfSecondData = TypeOfConnection.Child;
                                else 
                                    connection.TypeOfSecondData = TypeOfConnection.Parent;

                                if (AddConnectionTool.IsConnectionNewTest(connection)) AddConnectionTool.connections.Add(connection);                               
                            }
                        }
                    }
                }
            }
            AddConnectionTool.DrawConnections(AddConnectionTool.connections);
            MainWindow.AddRectTool.GeneratorId.MaxId = AddRectangleTool.RectangleDatas.Count;
        }        
    }
}
