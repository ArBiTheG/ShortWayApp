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
        // Позиция нажатия
        private Point ClickPosition;
        // Позиция смещения
        private Point OffsetCameraPosition;
        // Старые координаты камеры
        private Point LastCameraPosition;

        private PointF CameraPosition()
        {
            Point position = LastCameraPosition;
            position.X -= OffsetCameraPosition.X;
            position.Y -= OffsetCameraPosition.Y;
            return position;
        }
        public Scene2DControl()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            ClickPosition = MousePosition;
            OffsetDrag = true;
            Refresh();
            base.OnMouseDown(e);
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            LastCameraPosition.X -= OffsetCameraPosition.X;
            LastCameraPosition.Y -= OffsetCameraPosition.Y;
            OffsetCameraPosition.X = 0;
            OffsetCameraPosition.Y = 0;
            OffsetDrag = false;
            Refresh();

            base.OnMouseUp(e);
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (OffsetDrag)
            {
                OffsetCameraPosition.X = (ClickPosition.X - MousePosition.X);
                OffsetCameraPosition.Y = (ClickPosition.Y - MousePosition.Y);
                Refresh();
            }

            base.OnMouseMove(e);
        }
        private void DrawScene(Graphics g)
        {
            float step = Width / 10;
            float x = (CameraPosition().X % step) - step;
            float y = (CameraPosition().Y % step) - step;
            while (y < Height)
            {
                g.DrawLine(Pens.Green, x, y, x + step, y);
                g.DrawLine(Pens.Green, x + step, y, x + step, y + step - 1);
                x += step + 1;
                if (x > Width)
                {
                    x = (CameraPosition().X % step) - step;
                    y += step + 1;
                }
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawString($"Camera Position: x:{CameraPosition().X} y:{CameraPosition().Y}",Font,Brushes.Black,0,0);
            DrawScene(g);
            g.DrawRectangle(Pens.Black ,0,0,Width-1, Height -1);
            
            base.OnPaint(e);
        }

        private void Scene2DControl_Load(object sender, EventArgs e)
        {

        }
    }
}
