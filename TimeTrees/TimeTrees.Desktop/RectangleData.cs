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
    public class RectangleData
    {       
        public int Id { get; set; }

        public Person Person { get; set; }

        public Point RectCenter { get; set; }

        public Dictionary<TypeOfConnection,List<int>> TypeandConnectionIds= new Dictionary<TypeOfConnection, List<int>>();

        public static RectangleData SearchRectData(int rectDataId)
        {
            foreach (RectangleData data in AddRectangleTool.RectangleDatas.Values)
            {
                if (data.Id == rectDataId) return data;
            }
            throw new ArgumentNullException();
        }
    }
}
