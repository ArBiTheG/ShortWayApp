using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
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
        AdjustableArrowCap adjustableArrowCap;
        public ShortWayControl()
        {
            InitializeComponent();
            InitializeVariables();
        }
        private void InitializeVariables()
        {
            Vectors = new Dictionary<char, Vector>();
            Relations = new List<Relation>();
            adjustableArrowCap = new AdjustableArrowCap(4,4);
        }
        public void AddPoint(char point, int x, int y, string name = "")
        {
            Vectors.Add(point, Vector.Create(x, y, name));
        }
        public void AddRelation(char pointA, char pointB, bool reverse = false, bool duplex = false)
        {
            Vector a;
            Vector b;
            Vectors.TryGetValue(pointA, out a);
            Vectors.TryGetValue(pointB, out b);
            Relations.Add(Relation.Create(a, b, reverse, duplex));
        }
        private List<Relation> GetRelations(Vector vector)
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
            return relations;
        }
        private Relation GetRelation(Vector vectorA, Vector vectorB)
        {
            if (vectorA != null && vectorB != null)
            {
                foreach (Relation relation in Relations)
                {
                    if (relation != null)
                        if (relation.VectorA == vectorA && relation.VectorB == vectorB ||
                            relation.VectorA == vectorB && relation.VectorB == vectorA)
                            return relation;
                }
            }
            return null;
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
            // Получить видимую сцену
            PointF render = RenderPosition;

            foreach (var item in Vectors)
            {
                // Получить букву вектора
                char symbol = item.Key;

                // Получить вектор
                Vector vectorA = item.Value;

                // Получение координат вектора
                float xA = render.X - vectorA.X * ZoomCam;
                float yA = render.Y - vectorA.Y * ZoomCam;

                List<Relation> relations = GetRelations(vectorA);
                foreach (Relation relation in relations)
                {
                    // Получить второй вектор
                    Vector vectorB = relation.VectorB;

                    // Получить координаты второй точки
                    float xB = render.X - vectorB.X * ZoomCam;
                    float yB = render.Y - vectorB.Y * ZoomCam;

                    // Запрет отрисовки соединений за пределами видимой сцены
                    if ((xA < 0 && xB < 0) || 
                        (yA < 0 && yB < 0) ||
                        (xA > Width && xB > Width) || 
                        (yA > Height && yB > Height)) continue;

                    // Выборка цветов
                    Brush brush;
                    if (relation.Selected)
                    {
                        brush = Brushes.LightGreen;
                    }
                    else
                    {
                        if (relation.Duplex)
                            brush = vectorA.Selected || vectorB.Selected ? Brushes.Red : Brushes.Blue;
                        else
                            if (relation.Reverse)
                            brush = vectorB.Selected ? Brushes.Red : Brushes.Blue;
                        else
                            brush = vectorA.Selected ? Brushes.Red : Brushes.Blue;
                    }

                    // Отрисовка линий и текста
                    if (brush != null)
                    {
                        Pen pen = new Pen(brush, 2);
                        if (relation.Duplex)
                        {
                            pen.CustomStartCap = adjustableArrowCap;
                            pen.CustomEndCap = adjustableArrowCap;
                        }
                        else
                        {
                            if (relation.Reverse)
                                pen.CustomStartCap = adjustableArrowCap;
                            else
                                pen.CustomEndCap = adjustableArrowCap;
                        }
                        g.DrawLine(pen, xA, yA, xB, yB);
                        g.DrawString(relation.Distance().ToString(), Font, brush, (xA + xB) / 2, (yA + yB) / 2);
                    }
                }

                if (xA > 0 && xA < Width && yA > 0 && yA < Height)
                {
                    bool hasRelation = HasRelation(vectorA);
                    Brush brush = vectorA.Selected ? Brushes.Red : hasRelation ? Brushes.Blue : Brushes.Black;
                    g.FillEllipse(brush, xA - vectorA.Size, yA - vectorA.Size, vectorA.Size * 2, vectorA.Size * 2);
                    g.DrawString(symbol.ToString(), Font, brush, xA + vectorA.Size, yA + vectorA.Size);
                }
            }
        }
        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                float x = CameraPosition.X - ((e.X - Width / 2) / ZoomCam);
                float y = CameraPosition.Y - ((e.Y - Height / 2) / ZoomCam);

                foreach (var item in Vectors)
                {
                    Vector vector = (item.Value);
                    if (vector.GetDistance(x,y) < vector.Size / ZoomCam)
                        vector.Selected = true;
                    else
                        vector.Selected = false;
                }
            }
            Refresh();
            base.OnMouseClick(e);
        }

        private double[,] WaysDistance;
        private int[,] WaysWay;
        const double INF = double.MaxValue;
        const string INFS = "Бесконеч.";

        public void WriteMatrix()
        {
            int count = Vectors.Count;
            Console.WriteLine("\nМатрица путей");
            Console.Write($"{'\\',2}");
            for (int i = 0; i < count; i++)
            {
                var item = Vectors.ElementAt(i);
                var key = item.Key;
                Console.Write($"{key,10}   ");
            }
            Console.WriteLine();
            for (int i = 0; i < count; i++)
            {
                var item = Vectors.ElementAt(i);
                var key = item.Key;
                Console.Write($"{key,2}");
                for (int j = 0; j < count; j++)
                {
                    double value = WaysDistance[i, j];
                    Console.Write($"{(value == INF ? INFS : value.ToString()),10} | ");
                }
                Console.WriteLine();
            }
            Console.WriteLine("\nМатрица маршрутов");
            Console.Write($"{'\\',2}");
            for (int i = 0; i < count; i++)
            {
                var item = Vectors.ElementAt(i);
                var key = item.Key;
                Console.Write($"{key,10}   ");
            }
            Console.WriteLine();
            for (int i = 0; i < count; i++)
            {
                var item = Vectors.ElementAt(i);
                var key = item.Key;
                Console.Write($"{key,2}");
                for (int j = 0; j < count; j++)
                {
                    int index = WaysWay[i, j];
                    Console.Write($"{ Vectors.ElementAt(index).Key ,10} | ");
                }
                Console.WriteLine();
            }
        }
        public void WayTracing()
        {
            int count = Vectors.Count;
            char[] symbols = Vectors.Keys.ToArray();
            Vector[] vectors = Vectors.Values.ToArray();
            WaysDistance = new double[count,count];
            WaysWay = new int[count, count];

            #region Заполнение всех путей до предела
            Console.WriteLine("Этап 1: Заполнение всех путей до предела");
            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < count; j++)
                {
                    if (i != j)
                        WaysDistance[i, j] = INF;
                    else
                        WaysDistance[i, i] = 0;
                }
            }
            #endregion

            #region Внесение в матрицу начальных значений
            Console.WriteLine("Этап 2: Внесение в матрицу начальных значений");
            for (int i = 0; i < count; i++)
            {
                var itemA = Vectors.ElementAt(i);
                var valueA = itemA.Value;

                for (int j = 0; j < count; j++)
                {
                    var itemB = Vectors.ElementAt(j);
                    var valueB = itemB.Value;

                    WaysWay[i, j] = j;
                    if (i != j)
                    {
                        foreach (Relation relation in Relations)
                        {
                            if (relation.Duplex)
                            {
                                if (valueB == relation.VectorB && valueA == relation.VectorA)
                                {
                                    WaysDistance[i, j] = relation.Distance(1);
                                    WaysDistance[j, i] = relation.Distance(1);
                                }
                            }
                            else
                            {
                                if (relation.Reverse)
                                {
                                    if (valueB == relation.VectorB)
                                    {
                                        if (valueA == relation.VectorA)
                                        {
                                            WaysDistance[i, j] = relation.Distance(1);
                                        }
                                    }
                                }
                                else
                                {
                                    if (valueA == relation.VectorA)
                                    {
                                        if (valueB == relation.VectorB)
                                        {
                                            WaysDistance[i, j] = relation.Distance(1);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        WaysDistance[i, i] = 0;
                    }
                }
            }
            #endregion
#if DEBUG
            WriteMatrix();
#endif
            for (int i = 0; i < count; i++)
            {

                for (int r = 0; r < count; r++)
                {
                    if (r == i) continue;
                    for (int c = 0; c < count; c++)
                    {
                        if (c == i) continue;
                        double column = WaysDistance[i,c];
                        double row = WaysDistance[r, i];
                        double cell = WaysDistance[r, c];
                        if (cell > column + row)
                        {
                            WaysDistance[r, c] = column + row;
                            WaysWay[r, c] = i;
                        }
                    }
                }
                Console.WriteLine("Шаг: " + i + 1);
                WriteMatrix();
            }
            int startPoint = 4;
            int endPoint = 2;
            int middlePoint = WaysWay[startPoint, endPoint];
            Console.WriteLine(Vectors.ElementAt(startPoint).Key + " -> " + Vectors.ElementAt(middlePoint).Key);
            Relation relatio = GetRelation(Vectors.ElementAt(startPoint).Value, Vectors.ElementAt(middlePoint).Value);
            if (relatio != null)
            {
                relatio.Select();
            }
            while (middlePoint != endPoint)
            {
                int old = middlePoint;
                middlePoint = WaysWay[old, endPoint];
                relatio = GetRelation(Vectors.ElementAt(old).Value, Vectors.ElementAt(middlePoint).Value);
                if (relatio != null)
                {
                    relatio.Select();
                }
                Console.WriteLine(Vectors.ElementAt(old).Key + " -> " + Vectors.ElementAt(middlePoint).Key);
            }
        }
    }
}
