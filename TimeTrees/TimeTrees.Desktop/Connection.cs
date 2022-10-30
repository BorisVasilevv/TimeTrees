using System;
using System.Windows.Shapes;

namespace TimeTrees.DesktopGui
{

    public enum TypeOfConnection
    {
        Spouse,
        Child,
        Parent
    } 

    public class Connection
    {
        public RectangleData RectData1 { get; set; }
        public RectangleData RectData2 { get; set; }
        public Polyline Polyline { get; set; }
        public TypeOfConnection TypeOfFirstData { get; set; }
        public TypeOfConnection TypeOfSecondData { get; set; }
    }
}
