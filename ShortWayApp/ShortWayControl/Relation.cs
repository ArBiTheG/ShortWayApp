using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortWayApp.ShortWayControl
{
    public class Relation
    {
        private Vector vectorA;
        private Vector vectorB;
        public Vector VectorA => vectorA;
        public Vector VectorB => vectorB;
        public bool Reverse = false;
        public bool Duplex = false;
        public bool Selected = false;
        public Relation(Vector vectorA, Vector vectorB, bool reverse, bool duplex)
        {
            this.vectorA = vectorA;
            this.vectorB = vectorB;
            this.Reverse = reverse;
            this.Duplex = duplex;
        }
        static public Relation Create(Vector a, Vector b, bool reverse, bool duplex)
        {
            if (a != b)
                if (a!=null && b!=null)
                    return new Relation(a, b, reverse, duplex);
            return null;
        }
        public double GetAngle()
        {
            if (VectorA != VectorB)
            {
                if ((VectorA != null) && (VectorB != null))
                {
                    double angle = Math.Atan2(VectorB.Y, VectorB.X) - Math.Atan2(VectorA.Y, VectorA.X);
                    return angle;
                }
            }
            return 0;
        }
        public double Distance(int dec = 1)
        {
            double x = Math.Abs(VectorA.X - VectorB.X);
            double y = Math.Abs(VectorA.Y - VectorB.Y);
            double d = Math.Sqrt(x * x + y * y);
            return Math.Round(d, dec);
        }
        public void Select(bool value = true)
        {
            Selected = value;
        }
    }
}
