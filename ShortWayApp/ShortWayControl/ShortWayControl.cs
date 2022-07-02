using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShortWayApp.ShortWayControl
{
    public partial class ShortWayControl : Scene2DControl
    {
        
        Dictionary<char, Vector> Vectors;
        List<Relation> Relations;
        public ShortWayControl()
        {
            InitializeComponent();
            InitializeVariables();
        }
        private void InitializeVariables()
        {
            Vectors = new Dictionary<char, Vector>();
            Relations = new List<Relation>();
            AddPoint('A', 5, 6);
            AddPoint('B', 100, 80);
            AddPoint('V', -100, 80);
            AddPoint('C', -20, 60);
            AddPoint('D', 90, -5);
            AddRelation('A', 'B');
            AddRelation('A', 'V');
            AddRelation('B', 'C');
            AddRelation('A', 'C');
            AddRelation('D', 'C');
        }
        public void AddPoint(char point, int x, int y, string name = "")
        {
            Vectors.Add(point, Vector.Create(x, y, name));
        }
        public void AddRelation(char pointA, char pointB)
        {
            Vector a;
            Vector b;
            Vectors.TryGetValue(pointA, out a);
            Vectors.TryGetValue(pointB, out b);
            Relations.Add(Relation.Create(a, b));
        }
        private Relation[] GetRelations(Vector vector)
        {
            List<Relation> relations = new List<Relation>();
            if (vector!= null)
            {
                foreach (Relation relation in Relations)
                {
                    if (relation!=null)
                        if (relation.VectorA == vector)
                            relations.Add(relation);
                }
            }
            return relations.ToArray();
        }
        private bool HasRelation(Vector vector)
        {
            if (vector != null)
            {
                foreach (Relation relation in Relations)
                {
                    if (relation != null)
                        if (relation.VectorA == vector || relation.VectorB == vector)
                            return true;
                }
            }
            return false;
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            base.OnPaint(e);
            PointF render = RenderPosition();
            foreach (var item in Vectors)
            {
                char symbol = item.Key;
                Vector vector = (item.Value);

                float vector_x = render.X - vector.X * ZoomCam;
                float vector_y = render.Y - vector.Y * ZoomCam;

                Relation[] relations = GetRelations(vector);
                foreach (Relation relation in relations)
                {
                    float vector2_x = render.X - relation.VectorB.X * ZoomCam;
                    float vector2_y = render.Y - relation.VectorB.Y * ZoomCam;
                    if ((vector_x > 0 || vector2_x > 0) &&
                        (vector_y > 0 || vector2_y > 0) &&
                        (vector_x < Width || vector2_x < Width) &&
                        (vector_y < Height || vector2_y < Height)
                        )
                    {
                        g.DrawLine(Pens.Blue, vector_x, vector_y, vector2_x, vector2_y);
                    }
                }


                if (vector_x > 0 && vector_x < Width && vector_y > 0 && vector_y < Height)
                {
                    bool hasRelation = HasRelation(vector);
                    Brush brush = hasRelation ? Brushes.Blue : Brushes.Black;
                    g.FillEllipse(brush, vector_x - vector.Size, vector_y - vector.Size, vector.Size * 2, vector.Size * 2);
                    g.DrawString(symbol.ToString(), Font, brush, vector_x + vector.Size, vector_y + vector.Size);
                }
            }
        }
    }
}
