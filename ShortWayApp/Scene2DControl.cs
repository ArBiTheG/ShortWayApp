using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShortWayApp
{
    public partial class Scene2DControl : UserControl
    {
        private bool OffsetDrag = false;
        /// <summary>
        /// Координаты нажатия по сцене
        /// </summary>
        private PointF ClickScenePos;

        /// <summary>
        /// Координаты смещения от координат нажатия по сцене
        /// </summary>
        private PointF OffsetCamPos;

        /// <summary>
        /// Координаты камеры
        /// </summary>
        private PointF CamPos;

        /// <summary>
        /// Увеличение камеры
        /// </summary>
        private float zoom_cam = 1.0f;
        public float ZoomCam
        {
            get
            {
                if (zoom_cam > 0.1f)
                    return zoom_cam;
                return 0.1f;
            }
            set
            {
                if (value > 0.1f)
                    zoom_cam = value;
                else
                    zoom_cam = 0.1f;
            }
        }

        /// <summary>
        /// Размер прицельной точки
        /// </summary>
        private int DotSize = 20;

        /// <summary>
        /// Координаты камеры со смещением, для получения плавности перемещения
        /// </summary>
        /// <returns>Возращает сумму координат камеры и координат смещения</returns>
        protected PointF CameraPosition()
        {
            PointF position = CamPos;
            position.X += OffsetCamPos.X;
            position.Y += OffsetCamPos.Y;
            return position;
        }
        /// <summary>
        /// Координаты камеры для корректной обработки изображения на экране
        /// </summary>
        /// <returns></returns>
        protected PointF RenderPosition()
        {
            PointF position = CamPos;
            position.X += OffsetCamPos.X;
            position.Y += OffsetCamPos.Y;
            position.X *= zoom_cam;
            position.Y *= zoom_cam;
            position.X += Width / 2;
            position.Y += Height / 2;
            return position;
        }
        public Scene2DControl()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            ClickScenePos = MousePosition;
            OffsetDrag = true;
            Refresh();
            base.OnMouseDown(e);
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            CamPos.X += OffsetCamPos.X;
            CamPos.Y += OffsetCamPos.Y;
            OffsetCamPos.X = 0;
            OffsetCamPos.Y = 0;
            OffsetDrag = false;
            Refresh();

            base.OnMouseUp(e);
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (OffsetDrag)
            {
                OffsetCamPos.X = (MousePosition.X - ClickScenePos.X) / zoom_cam;
                OffsetCamPos.Y = (MousePosition.Y - ClickScenePos.Y) / zoom_cam;
                Refresh();
            }

            base.OnMouseMove(e);
        }
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (e.Delta != 0)
            {
                if (e.Delta > 0)
                    ZoomCam -= 0.1f;
                else
                    ZoomCam += 0.1f;
            }
            Refresh();
            base.OnMouseWheel(e);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            DrawScene(g);
            DrawInfo(g);

            g.DrawLine(Pens.Black, Width / 2 - DotSize / 2, Height / 2, Width / 2 + DotSize / 2, Height / 2);
            g.DrawLine(Pens.Black, Width / 2, Height / 2 - DotSize / 2, Width / 2, Height / 2 + DotSize / 2);
            g.DrawRectangle(Pens.Black ,0,0,Width-1, Height -1);
            
            base.OnPaint(e);
        }

        /// <summary>
        /// Написать на экран информацию
        /// </summary>
        private void DrawInfo(Graphics g)
        {
            PointF camera = CameraPosition();
            PointF area = RenderPosition();
            g.DrawString($"Camera Position: x:{camera.X} y:{camera.Y}", Font, Brushes.Black, 0, 0);
            g.DrawString($"Camera Zoom: {Math.Round(ZoomCam, 2)}", Font, Brushes.Black, 0, 30);

            if (OffsetDrag)
                g.DrawString($"Offset: x:{OffsetCamPos.X} y:{OffsetCamPos.Y}", Font, Brushes.Black, 0, 45);

        }
        /// <summary>
        /// Отрисовать сцену
        /// </summary>
        private void DrawScene(Graphics g)
        {
            PointF render = RenderPosition();
            float step = 50 * zoom_cam;
            float x_start = render.X % step;
            float x = x_start;
            float y = render.Y % step;

            while (y < Height)
            {
                g.DrawLine(Pens.Green, x, 0, x, Height);
                x += step;

                if (x >= Width)
                    x = x_start;

                g.DrawLine(Pens.Green, 0, y, Width, y);
                y += step;
            }
        }

        private void Scene2DControl_Load(object sender, EventArgs e)
        {

        }
    }
}
