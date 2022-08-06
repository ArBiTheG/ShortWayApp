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
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            shortWayControl1.AddPoint('A', 5, 6);
            shortWayControl1.AddPoint('B', 100, 80);
            shortWayControl1.AddPoint('V', -100, 80);
            shortWayControl1.AddPoint('C', -20, 60);
            shortWayControl1.AddPoint('D', 90, -5);
            shortWayControl1.AddRelation('A', 'B', false, true);
            shortWayControl1.AddRelation('A', 'V', false, true);
            shortWayControl1.AddRelation('C', 'B');
            shortWayControl1.AddRelation('A', 'C');
            shortWayControl1.AddRelation('D', 'C', false, true);
        }

        private void shortWayControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Выполнение трасировки путей!");
            shortWayControl1.WayTracing();
        }
    }
}
