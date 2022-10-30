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
    public class AddInformationTool
    {
        
        const int TextBoxWithName = 0;
        const int BirthDateTextBox = 1;
        const int DeathDateTextBox = 2;
        const int AmountOfParams = 3;

        Rectangle RectToAddData = null;
        List<TextBox> TextBoxs = new List<TextBox>();
        List<TextBlock> TextBlocks = new List<TextBlock>();
        TextBlock TextBlockError = null;
        Button BtnReadyToAddData = new Button();
        RectangleData RectangleDataToAddData = null;

        Rectangle SelectedRect;

        public AddInformationTool()
        {
            if (RectToAddData == null)
            {
                RectToAddData = new Rectangle();
                RectToAddData.Height = 450;
                RectToAddData.Width = 180;
                RectToAddData.HorizontalAlignment = HorizontalAlignment.Left;
                RectToAddData.VerticalAlignment = VerticalAlignment.Top;
                RectToAddData.Fill = new SolidColorBrush(Colors.Brown);
            }
        }

        public void AddInformationMouseDown(object sender, MouseButtonEventArgs e)
        {
            SelectedRect = MainWindow.AddRectTool.SelectedRectangle;
            if (SelectedRect != null && SelectedRect.IsMouseOver && e.LeftButton == MouseButtonState.Pressed)
            {
                RectangleDataToAddData = AddRectangleTool.RectangleDatas[SelectedRect];
                MainWindow.Canvas1.Children.Add(RectToAddData);
                Canvas.SetZIndex(RectToAddData, 1);
                Canvas.SetLeft(RectToAddData, MainWindow.Canvas1.Width - RectToAddData.Width - 15);
              
                for (int i = 0; i < AmountOfParams; i++)
                {
                    TextBox textBox = new TextBox();
                    textBox.Height = 20;
                    textBox.Width = 150;
                    textBox.TextAlignment = TextAlignment.Left;
                    textBox.VerticalAlignment = VerticalAlignment.Top;
                    textBox.HorizontalAlignment = HorizontalAlignment.Center;
                    Canvas.SetZIndex(textBox, 2);
                    Canvas.SetTop(textBox, 40 * (i + 1));
                    Canvas.SetLeft(textBox, MainWindow.Canvas1.Width - RectToAddData.Width);
                    MainWindow.Canvas1.Children.Add(textBox);
                    TextBoxs.Add(textBox);

                    TextBlock textBlock = new TextBlock();
                    textBlock.Height = 20;
                    textBlock.Width = 150;
                    textBlock.Text = i == 2 ? "Введите дату смерти" : i == 1 ? "Введите дату рождения" : "Введите ФИО";
                    Canvas.SetTop(textBlock, 20 + 40 * i);
                    Canvas.SetLeft(textBlock, MainWindow.Canvas1.Width - RectToAddData.Width);
                    Canvas.SetZIndex(textBlock, 2);
                    MainWindow.Canvas1.Children.Add(textBlock);
                    TextBlocks.Add(textBlock);
                }
                
                BtnReadyToAddData.Height = 20;
                BtnReadyToAddData.Width = 150;
                Canvas.SetTop(BtnReadyToAddData, 160);
                Canvas.SetLeft(BtnReadyToAddData, MainWindow.Canvas1.Width - RectToAddData.Width);
                BtnReadyToAddData.Content = "Готово";
                BtnReadyToAddData.HorizontalContentAlignment = HorizontalAlignment.Center;
                BtnReadyToAddData.VerticalContentAlignment = VerticalAlignment.Center;
                Canvas.SetZIndex(BtnReadyToAddData, 2);
                MainWindow.Canvas1.Children.Add(BtnReadyToAddData);
                BtnReadyToAddData.Click += btnReadyToAddData_Click;                
            }
            MainWindow.Canvas1.MouseDown += MainWindow.AddRectTool.RectMouseDown;
            MainWindow.Canvas1.MouseDown -= AddInformationMouseDown;
        }

        private void btnReadyToAddData_Click(object sender, RoutedEventArgs e)
        {
            if (TextBoxs.Count == 3 && DateParser.IsDateCorrect(TextBoxs[BirthDateTextBox].Text)
                        && (TextBoxs[DeathDateTextBox].Text == "" || DateParser.IsDateCorrect(TextBoxs[DeathDateTextBox].Text)))
            {
                RectangleDataToAddData.Person = new Person()
                {
                    Id = RectangleDataToAddData.Id,
                    Name = TextBoxs[TextBoxWithName].Text,
                    BirthDate=DateParser.ParseToDateTime(TextBoxs[BirthDateTextBox].Text),
                    DeathDate= TextBoxs[DeathDateTextBox].Text == ""? null: DateParser.ParseToDateTime(TextBoxs[DeathDateTextBox].Text)
                };

                AddInformationToRect(SelectedRect, new List<string>()
                {
                    TextBoxs[TextBoxWithName].Text ,
                    TextBoxs[BirthDateTextBox].Text,
                    TextBoxs[DeathDateTextBox].Text
                });

                foreach (TextBox textBox in TextBoxs) 
                    MainWindow.Canvas1.Children.Remove(textBox);
                foreach(TextBlock textBlock in TextBlocks)
                    MainWindow.Canvas1.Children.Remove(textBlock);
                MainWindow.Canvas1.Children.Remove(RectToAddData);
                MainWindow.Canvas1.Children.Remove(TextBlockError);
                MainWindow.Canvas1.Children.Remove(BtnReadyToAddData);

                TextBlocks.Clear();
                RectangleDataToAddData = null;
                TextBoxs.Clear();
                TextBlockError = null;
            }
            else if (TextBoxs.Count == 3 && TextBlockError == null)
            {
                TextBlockError = new TextBlock();
                TextBlockError.Height = 20;
                TextBlockError.Width = 150;
                Canvas.SetTop(TextBlockError, 190);
                Canvas.SetLeft(TextBlockError, MainWindow.Canvas1.Width - RectToAddData.Width);
                Canvas.SetZIndex(TextBlockError, 2);
                TextBlockError.Text = "Ошибка в введённых данных";
                MainWindow.Canvas1.Children.Add(TextBlockError);
            }
        }

        public void AddInformationToRect(Rectangle rect, List<string> information)
        {
            RectangleData rectData = AddRectangleTool.RectangleDatas[rect];
            TextBlock textBlockName = new TextBlock() { Text = information[TextBoxWithName] };
            TextBlock textBlockBirthDate = new TextBlock() { Text = information[BirthDateTextBox].Replace("0:00:00", String.Empty) };
            TextBlock textBlockDeathDate = new TextBlock() { Text = information[DeathDateTextBox].Replace("0:00:00", String.Empty) };
            List<TextBlock> list = new List<TextBlock>() { textBlockName, textBlockBirthDate, textBlockDeathDate };
            for (int i = 0; i < list.Count; i++)
            {
                list[i].Height = 20;
                list[i].Width = 100;
                list[i].VerticalAlignment = VerticalAlignment.Top;
                list[i].HorizontalAlignment = HorizontalAlignment.Center;
                list[i].TextAlignment = TextAlignment.Center;
                Canvas.SetZIndex(list[i], 2);
                MainWindow.Canvas1.Children.Add(list[i]);
                Canvas.SetTop(list[i], rectData.RectCenter.Y + (i) * 20 - 10);
                Canvas.SetLeft(list[i], rectData.RectCenter.X - rect.Height / 2);
            }
            AddRectangleTool.InformationOnRect.Add(rect, list);
        }
    }
}
