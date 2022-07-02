using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortWayApp.ShortWayControl
{
    public class Vector
    {
        public float X;
        public float Y;
        public float Size;
        public string Name;
        public Vector(float x, float y, string name)
        {
            X = x;
            Y = y;
            Name = name;
            Size = 4;
        }
        static public Vector Create(int x, int y, string name)
        {
            return new Vector(x, y, name);
        }
        public double GetAngle(Vector b)
        {
            return Math.Atan2(b.Y, b.X) - Math.Atan2(this.Y, this.X);
        }
        static public double GetAngle(Vector a, Vector b)
        {
            return Math.Atan2(b.Y, b.X) - Math.Atan2(a.Y, a.X);
        }
    }
}
