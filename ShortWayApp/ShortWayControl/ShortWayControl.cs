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

        Dictionary<object, Vector> Vectors;
        List<Relation> Relations;
        AdjustableArrowCap adjustableArrowCap;
        public ShortWayControl()
        {
            InitializeComponent();
            InitializeVariables();
        }
        public object[] GetPoints()
        {
            return Vectors.Keys.ToArray();
        }
        private void InitializeVariables()
        {
            Vectors = new Dictionary<object, Vector>();
            Relations = new List<Relation>();
            adjustableArrowCap = new AdjustableArrowCap(4, 4);
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
            if (vector != null)
            {
                foreach (Relation relation in Relations)
                {
                    if (relation != null)
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
                        if (relation.Duplex)
                        {
                            if (relation.VectorA == vectorA && relation.VectorB == vectorB ||
                                relation.VectorA == vectorB && relation.VectorB == vectorA)
                                return relation;
                        }
                        else if (!relation.Reverse)
                        {
                            if (relation.VectorA == vectorA && relation.VectorB == vectorB)
                                return relation;
                        }
                        else
                        {
                            if (relation.VectorA == vectorB && relation.VectorB == vectorA)
                                return relation;
                        }
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
                char symbol = (char)item.Key;

                // Получить вектор
                Vector vectorA = item.Value;

                // Получение координат вектора
                float xA = render.X - vectorA.X * ZoomCam;
                float yA = render.Y - vectorA.Y * ZoomCam;

                if (xA > 0 && xA < Width && yA > 0 && yA < Height)
                {
                    Brush brush;
                    if (vectorA.Selected)
                    {
                        brush = Brushes.LightGreen;
                    }
                    else if (vectorA.Focused)
                    {
                        brush = Brushes.Aqua;
                    }
                    else if (HasRelation(vectorA))
                    {
                        brush = Brushes.Blue;
                    }
                    else
                    {
                        brush = Brushes.Black;
                    }
                    //Brush brush = vectorA.Selected ? Brushes.LightGreen : hasRelation ? Brushes.Blue : Brushes.Black;
                    g.FillEllipse(brush, xA - vectorA.Size, yA - vectorA.Size, vectorA.Size * 2, vectorA.Size * 2);
                    g.DrawString(symbol.ToString(), Font, brush, xA + vectorA.Size, yA + vectorA.Size);
                }

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
                    if (!relation.Selected)
                    {
                        if (relation.Duplex)
                            brush = vectorA.Focused || vectorB.Focused ? Brushes.Aqua : Brushes.Blue;
                        else if (!relation.Reverse)
                            brush = vectorA.Focused ? Brushes.Aqua : Brushes.Blue;
                        else
                            brush = vectorB.Focused ? Brushes.Aqua : Brushes.Blue;
                    }
                    else
                    {
                        brush = Brushes.LightGreen;
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
                        else if (!relation.Reverse)
                            pen.CustomEndCap = adjustableArrowCap;
                        else
                            pen.CustomStartCap = adjustableArrowCap;
                        g.DrawLine(pen, xA, yA, xB, yB);
                        g.DrawString(relation.Distance().ToString(), Font, brush, (xA + xB) / 2, (yA + yB) / 2);
                    }
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
                    if (vector.GetDistance(x, y) < vector.Size / ZoomCam)
                        vector.Focused = true;
                    else
                        vector.Focused = false;
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
                    Console.Write($"{ Vectors.ElementAt(index).Key,10} | ");
                }
                Console.WriteLine();
            }
        }
        public void ClearSelections()
        {

            foreach (Vector vector in Vectors.Values)
            {
                vector.Selected = false;
                vector.Focused = false;
            }
            foreach (Relation relation in Relations)
            {
                relation.Selected = false;
            }
        }
        public void ResetWayMatrix(int count)
        {
            WaysDistance = new double[count, count];
            WaysWay = new int[count, count];
            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < count; j++)
                {
                    WaysWay[i, j] = j;
                    if (i != j)
                        WaysDistance[i, j] = INF;
                    else
                        WaysDistance[i, i] = 0;
                }
            }
        }
        public void LoadWayMatrix(int count)
        {
            ResetWayMatrix(count);
            for (int i = 0; i < count; i++)
            {
                var itemA = Vectors.ElementAt(i);
                var valueA = itemA.Value;

                for (int j = 0; j < count; j++)
                {
                    var itemB = Vectors.ElementAt(j);
                    var valueB = itemB.Value;

                    if (i != j)
                    {
                        foreach (Relation relation in Relations)
                        {
                            if (relation.Duplex)
                            {
                                if (valueB == relation.VectorB && valueA == relation.VectorA ||
                                valueB == relation.VectorA && valueA == relation.VectorB)
                                {
                                    WaysDistance[i, j] = relation.Distance(1);
                                    WaysDistance[j, i] = relation.Distance(1);
                                }
                            }
                            else if (!relation.Reverse)
                            {
                                if (valueB == relation.VectorB && valueA == relation.VectorA)
                                {
                                    WaysDistance[i, j] = relation.Distance(1);
                                }
                            }
                            else
                            {
                                if (valueB == relation.VectorA && valueA == relation.VectorB)
                                {
                                    WaysDistance[j, i] = relation.Distance(1);
                                }
                            }
                        }
                    }
                }
            }

        }
        public void CalculateWayMatrix(int count)
        {
#if DEBUG
            Console.WriteLine("------------------------------");
            Console.WriteLine("Шаг: 0");
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
                        double column = WaysDistance[i, c];
                        double row = WaysDistance[r, i];
                        double cell = WaysDistance[r, c];
                        if (cell > column + row)
                        {
                            WaysDistance[r, c] = column + row;
                            WaysWay[r, c] = i;
                        }
                    }
                }
#if DEBUG
                Console.WriteLine("------------------------------");
                Console.WriteLine("Шаг: " + (i + 1));
                WriteMatrix();
#endif
            }
        }
        public void FillWayMatrix()
        {
            ClearSelections();
            int count = Vectors.Count;
            LoadWayMatrix(count);
            CalculateWayMatrix(count);

        }
        public string WayTracing(int startPoint, int endPoint)
        {
            double distance = 0;
            string textOutput = "";

            int middlePoint = startPoint;
            int oldPoint = middlePoint;
            Relation relatio = null;
            while (middlePoint != endPoint)
            {
                oldPoint = middlePoint;
                middlePoint = WaysWay[oldPoint, endPoint];
                relatio = GetRelation(Vectors.ElementAt(oldPoint).Value, Vectors.ElementAt(middlePoint).Value);
                if (relatio == null)
                {
                    for (int t = 0; t < Vectors.Count; t++)
                    {
                        middlePoint = WaysWay[oldPoint, middlePoint];
                        relatio = GetRelation(Vectors.ElementAt(oldPoint).Value, Vectors.ElementAt(middlePoint).Value);
                        if (relatio != null) break;
                    }
                }
                if (relatio != null)
                {
                    relatio.Select();

                    double d = relatio.Distance();
                    distance += d;
                    textOutput += "из пункта '" + Vectors.ElementAt(oldPoint).Key +
                        "' в пункт '" + Vectors.ElementAt(middlePoint).Key + "' ( " + d + "м )\n";
                }
                else
                {
                    textOutput = "Невозможно переместиться из пункта '" + 
                        Vectors.ElementAt(oldPoint).Key +
                        "' в пункт '" + Vectors.ElementAt(middlePoint).Key + "'!\n";
                    Console.WriteLine("Во время расчёта пути, была допущена ошибка!");
                    Console.WriteLine(String.Format(
                        "На координатах матрицы ('{0}','{1}') лежит следующая точка '{2}', но отношение между '{0}' и '{2}' отсутствует",
                        Vectors.ElementAt(oldPoint).Key,
                        Vectors.ElementAt(endPoint).Key,
                        Vectors.ElementAt(middlePoint).Key));
                    break;
                }

            }
            textOutput += "Общее расстояние: " + distance + "м";
#if DEBUG
            Console.WriteLine(textOutput);
#endif
            Refresh();
            return textOutput;
        }
    }
}
