using System;
using System.Collections.Generic;
using System.IO;
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



namespace TimeTrees.DesktopGui
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        public static Canvas Canvas1;
        public static AddRectangleTool AddRectTool = new AddRectangleTool();
        public static AddInformationTool AddInformationTool = new AddInformationTool();
        public static string FileToWork = "..\\..\\..\\..\\";
        public bool IsProgramReady = false;


        public MainWindow()
        {
            InitializeComponent();
            Canvas1 = canvas1;
            canvas1.MouseMove += AddRectangleTool.clearSelection;
            TextBlock textBlock = new TextBlock();
            textBlock.VerticalAlignment = VerticalAlignment.Top;
            textBlock.HorizontalAlignment = HorizontalAlignment.Left;
            textBlock.Width = 735;
            textBlock.Height = 30;
            textBlock.Text = "Выберите Файл для работы";
            canvas1.Children.Add(textBlock);
            Canvas.SetLeft(textBlock, 65);
            string[] allFiles = Directory.GetFiles("..\\..\\..\\..\\");
            Button btn = new Button();
            btn.Width = 100;
            btn.Height = 30;
            btn.VerticalAlignment = VerticalAlignment.Top;
            btn.HorizontalAlignment = HorizontalAlignment.Left;
            canvas1.Children.Add(btn);
            Canvas.SetLeft(btn, 100);
            Canvas.SetTop(btn, 40);
            btn.Content = "Новый файл";
            btn.Click += btnCreateNewFile_Click;
            int buttonCounter = 1;
            foreach (string file in allFiles)
            {
                string extension = System.IO.Path.GetExtension(file);
                string mainNameOfFile = file.Substring(12);
                if (extension == ".json" && mainNameOfFile != "timeline.json" && mainNameOfFile != "people.json")
                {
                    Button button = new Button();
                    button.Width = 100;
                    button.Height = 30;
                    button.VerticalAlignment = VerticalAlignment.Top;
                    button.HorizontalAlignment = HorizontalAlignment.Left;
                    canvas1.Children.Add(button);
                    Canvas.SetLeft(button, 100 * (buttonCounter / 10 + 1));
                    Canvas.SetTop(button, 40 * (buttonCounter % 10 + 1));
                    buttonCounter++;
                    button.Content = mainNameOfFile;
                    button.Click += btnFileName_Click;
                }
            }
        }

        private void btnFileName_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            FileToWork += btn.Content.ToString();
            IsProgramReady = true;
            canvas1.Children.Clear();
            FileWorker.ReadFromFileAndDraw(FileToWork);
        }

        private void btnCreateNewFile_Click(object sender, RoutedEventArgs e)
        {
            canvas1.Children.Clear();
            TextBlock textBlock = new TextBlock();
            textBlock.VerticalAlignment = VerticalAlignment.Top;
            textBlock.HorizontalAlignment = HorizontalAlignment.Left;
            textBlock.Width = 735;
            textBlock.Height = 30;
            textBlock.Text = "Введите имя файла ниже и нажмите Enter (нельзя ввести имя timeline или people)";
            canvas1.Children.Add(textBlock);
            Canvas.SetLeft(textBlock, 65);
            TextBox textBox = new TextBox();
            textBox.VerticalAlignment = VerticalAlignment.Top;
            textBox.HorizontalAlignment = HorizontalAlignment.Left;
            textBox.Width = 735;
            textBox.Height = 30;
            textBox.KeyDown += TextBox_KeyDown;
            Canvas.SetTop(textBox, 40);
            Canvas.SetLeft(textBox, 65);
            canvas1.Children.Add(textBox);
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (e.Key == Key.Enter)
            {
                string nameToNewFile = textBox.Text;
                if (nameToNewFile != "timeline") FileToWork = "..\\..\\..\\..\\" + nameToNewFile + ".json";
                IsProgramReady = !File.Exists(FileToWork);
                if (IsProgramReady) canvas1.Children.Clear();
            }
        }

        private void btnCreateSquare_Click(object sender, RoutedEventArgs e)
        {
            if (IsProgramReady)
            {
                canvas1.MouseMove += AddRectTool.NewRectMouseMove;
                canvas1.MouseDown += AddRectTool.RectMouseDown;
            }
        }

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            if (IsProgramReady)
            {
                Button button = (Button)sender;
                canvas1.MouseDown -= AddRectTool.RectMouseDown;
                if (button.Name == "btnConnectSpouse") AddConnectionTool.TypeOfNewConnection = TypeOfConnection.Spouse;
                if (button.Name == "btnConnectChildren") AddConnectionTool.TypeOfNewConnection = TypeOfConnection.Child;
                canvas1.MouseDown += AddConnectionTool.AddConnection;
            }
        }


        private void btnAddInformation_Click(object sender, RoutedEventArgs e)
        {
            if (IsProgramReady && !AddRectangleTool.InformationOnRect.ContainsKey(AddRectTool.SelectedRectangle))
            {
                canvas1.MouseDown -= AddRectTool.RectMouseDown;
                canvas1.MouseDown += AddInformationTool.AddInformationMouseDown;

            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            FileWorker.SaveInFileFromCanvas(AddRectangleTool.RectangleDatas, FileToWork);
        }
    }
}