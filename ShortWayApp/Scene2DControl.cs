﻿using System;
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
        private int old_mouse_x;
        private int old_mouse_y;


        /// <summary>
        /// Координаты смещения от координат нажатия по сцене
        /// </summary>
        private PointF camera_offset_position;
        /// <summary>
        /// Координаты смещения от координат нажатия по сцене
        /// </summary>
        public PointF CameraOffsetPosition
        {
            get
            {
                return camera_offset_position;
            }
        }
        /// <summary>
        /// Координаты камеры
        /// </summary>
        private PointF camera_position;
        /// <summary>
        /// Координаты камеры
        /// </summary>
        public PointF CameraPosition
        {
            get
            {
                PointF position = camera_position;
                position.X = position.X + camera_offset_position.X;
                position.Y = position.Y + camera_offset_position.Y;
                return position;
            }
        }

        protected PointF RenderPosition
        {
            get
            {
                PointF position = camera_position;
                position.X = (position.X + camera_offset_position.X) * zoom_cam + Width / 2;
                position.Y = (position.Y + camera_offset_position.Y) * zoom_cam + Height / 2;
                return position;
            }
        }

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

        public Scene2DControl()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                old_mouse_x = e.X;
                old_mouse_y = e.Y;
                OffsetDrag = true;
                Refresh();
            }
            base.OnMouseDown(e);
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                camera_position.X += camera_offset_position.X;
                camera_position.Y += camera_offset_position.Y;
                camera_offset_position.X = 0;
                camera_offset_position.Y = 0;
                OffsetDrag = false;
                Refresh();
            }
            base.OnMouseUp(e);
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (OffsetDrag && e.Button == MouseButtons.Right)
            {
                camera_offset_position.X = (e.X - old_mouse_x) / zoom_cam;
                camera_offset_position.Y = (e.Y - old_mouse_y) / zoom_cam;
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
        protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    camera_position.Y += 2;
                    break;
                case Keys.Down:
                    camera_position.Y -= 2;
                    break;
                case Keys.Left:
                    camera_position.X += 2;
                    break;
                case Keys.Right:
                    camera_position.X -= 2;
                    break;
            }
            Refresh();
            base.OnPreviewKeyDown(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            //DrawScene(g);
            DrawInfo(g);

            g.DrawLine(Pens.Black, Width / 2 - DotSize / 2, Height / 2, Width / 2 + DotSize / 2, Height / 2);
            g.DrawLine(Pens.Black, Width / 2, Height / 2 - DotSize / 2, Width / 2, Height / 2 + DotSize / 2);
            g.DrawRectangle(Pens.Black ,0,0,Width-1, Height -1);
            
            base.OnPaint(e);
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Refresh();
        }

        /// <summary>
        /// Написать на экран информацию
        /// </summary>
        private void DrawInfo(Graphics g)
        {
            PointF camera = CameraPosition;
            g.DrawString($"Camera Position: x:{camera.X} y:{camera.Y}", Font, Brushes.Black, 0, 0);
            g.DrawString($"Camera Zoom: {Math.Round(ZoomCam, 2)}", Font, Brushes.Black, 0, 30);

            if (OffsetDrag)
                g.DrawString($"Offset: x:{camera_offset_position.X} y:{camera_offset_position.Y}", Font, Brushes.Black, 0, 45);

        }
        /// <summary>
        /// Отрисовать сцену
        /// </summary>
        private void DrawScene(Graphics g)
        {
            PointF render = RenderPosition;
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
