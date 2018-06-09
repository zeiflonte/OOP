using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Runtime.Serialization;

namespace ZPaint
{
    [Serializable]
    public class Line : Shape
    {
        [DataMember]
        public double X1
        {
            get
            {
                return ((System.Windows.Shapes.Line)figure).X1;
            }
            set
            {
                ((System.Windows.Shapes.Line)figure).X1 = value;
            }
        }
        [DataMember]
        public double X2
        {
            get
            {
                return ((System.Windows.Shapes.Line)figure).X2;
            }
            set
            {
                ((System.Windows.Shapes.Line)figure).X2 = value;
            }
        }
        [DataMember]
        public double Y1
        {
            get
            {
                return ((System.Windows.Shapes.Line)figure).Y1;
            }
            set
            {
                ((System.Windows.Shapes.Line)figure).Y1 = value;
            }
        }
        [DataMember]
        public double Y2
        {
            get
            {
                return ((System.Windows.Shapes.Line)figure).Y2;
            }
            set
            {
                ((System.Windows.Shapes.Line)figure).Y2 = value;
            }
        }

        public Line(SolidColorBrush color, int thickness, Point point1, Point point2) : base(color, thickness, point1, point2)
        { }

        protected Line(SerializationInfo info, StreamingContext context) : base(info, context)
        { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("color", color);
            info.AddValue("thickness", thickness);
            info.AddValue("point1", point1);
            info.AddValue("point2", point2);
        }

        public override void SetParameters(SolidColorBrush color, int thickness, Point point1, Point point2)
        {
            this.point1 = point1;
            this.point2 = point2;

            X1 = this.point1.X;
            Y1 = this.point1.Y;
            X2 = this.point2.X;
            Y2 = this.point2.Y;

            this.thickness = thickness;
            this.color = color;
        }

        public override System.Windows.Shapes.Shape DrawFigure()
        {
            return new System.Windows.Shapes.Line();
        }
    }
}
