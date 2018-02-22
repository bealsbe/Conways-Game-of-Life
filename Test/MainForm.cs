using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Test
{
    public partial class MainForm :Form
    {
        Grid grid;
        Bitmap DrawArea;

        public MainForm()
        {
            InitializeComponent();
            grid = new Grid(100 , 100);
            grid.Randomize();

            DrawArea = new Bitmap(drawBox.Size.Width , drawBox.Size.Height);
            drawBox.Image = DrawArea;
            backgroundWorker.WorkerSupportsCancellation = true;
            comboBox_size.SelectedItem = "10";
        }

        private void drawGrid()
        {
            drawBox.Image = DrawArea;
            // drawBox.BackColor = Color.Black;
            Graphics graphics;
            graphics = Graphics.FromImage(DrawArea);
            Pen pen = new Pen(Color.Black , 1);
            SolidBrush brush = new SolidBrush(Color.White);

            int w = (drawBox.Width / grid.Width);
            int h = (drawBox.Height / grid.Height);


            for(int i = 0; i < grid.Width; i++) {
                for(int j = 0; j < grid.Height; j++) {
                    if(grid.IsAlive(i , j)) {
                        brush.Color = Color.Red;
                    }
                    else {
                        brush.Color = Color.White;
                    }
                    Rectangle rect = new Rectangle((i * w) , (j * h) , w , h);
                    graphics.FillRectangle(brush , rect);
                    graphics.DrawRectangle(pen , rect);

                }
            }
        }

        private void MainForm_Load(object sender , EventArgs e) => drawGrid();

        private void button1_Click(object sender , EventArgs e)
        {
            if(!backgroundWorker.IsBusy) {
                backgroundWorker.RunWorkerAsync();
            }
        }


        private void drawBox_Click(object sender , MouseEventArgs e)
        {
            if(!backgroundWorker.IsBusy) {
                var point = drawBox.PointToClient(Cursor.Position);
                int x = point.X / (drawBox.Width / grid.Width);
                int y = point.Y / (drawBox.Height / grid.Height);
                grid.flipPoint(x , y);
                drawGrid();
            }

        }

        private void backgroundWorker_DoWork(object sender , DoWorkEventArgs e)
        {
            while(!backgroundWorker.CancellationPending) {
                grid.Advance();
                drawGrid();
                Thread.Sleep(25);
            }
        }

        private void button2_Click(object sender , EventArgs e) => backgroundWorker.CancelAsync();

        private void button3_Click(object sender , EventArgs e)
        {
            if(!backgroundWorker.IsBusy) {
                grid.clear();
                drawGrid();
            }
        }

        private void button4_Click(object sender , EventArgs e)
        {
            if(!backgroundWorker.IsBusy) {
                grid = new Grid(Int32.Parse(comboBox_size.SelectedItem.ToString()), Int32.Parse(comboBox_size.SelectedItem.ToString()));
                grid.Randomize();
                drawGrid();
            }
        }
    }
}
