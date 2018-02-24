using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
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
            grid = new Grid(400 , 400);
            grid.Randomize();
            DrawArea = new Bitmap(drawBox.Size.Width , drawBox.Size.Height);
            drawBox.Image = DrawArea;
            backgroundWorker.WorkerSupportsCancellation = true;
            drawWorker.WorkerSupportsCancellation = true;
            comboBox_size.SelectedItem = "100";
        }

        private void MainForm_Load(object sender , EventArgs e) => drawWorker.RunWorkerAsync();


        private void button1_Click(object sender , EventArgs e)
        {
            if(!backgroundWorker.IsBusy)
            {
                backgroundWorker.RunWorkerAsync();
            }
        }


        private void drawBox_Click(object sender , MouseEventArgs e)
        {
            if(!backgroundWorker.IsBusy)
            {
                var point = drawBox.PointToClient(Cursor.Position);
                int x = point.X / (drawBox.Width / grid.Width + 1);
                int y = point.Y / (drawBox.Height / grid.Height + 1);

                this.Invoke((Action)(() =>
                {
                    grid.flipPoint(x , y);
                    DrawGrid();
                }));
            }
        }

        private void backgroundWorker_DoWork(object sender , DoWorkEventArgs e)
        {
            while(!backgroundWorker.CancellationPending)
            {
                grid.Advance();
            }
        }

        private void button2_Click(object sender , EventArgs e) => backgroundWorker.CancelAsync();

        private void button3_Click(object sender , EventArgs e)
        {
            if(!backgroundWorker.IsBusy)
            {
                this.Invoke((Action)(() =>
                {
                    grid.clear();
                    DrawGrid();
                }));
            }
        }

        private void button4_Click(object sender , EventArgs e)
        {
            if(!backgroundWorker.IsBusy)
            {
                this.Invoke((Action)(() =>
                {
                    grid = new Grid(Int32.Parse(comboBox_size.SelectedItem.ToString()) , Int32.Parse(comboBox_size.SelectedItem.ToString()));
                    grid.Randomize();
                    DrawGrid();
                }));
            }
        }

        private void DrawGrid()
        {
            drawBox.Image = DrawArea;
            Graphics graphics;
            graphics = Graphics.FromImage(DrawArea);
            SolidBrush brush = new SolidBrush(Color.White);
            Pen pen = new Pen(Color.Black);

            int w = (drawBox.Width / grid.Width + 1);
            int h = (drawBox.Height / grid.Height + 1);

            for(int i = 0; i < grid.Width; i++)
            {
                for(int j = 0; j < grid.Height; j++)
                {
                    if(grid.IsAlive(i , j))
                    {
                        brush.Color = Color.Maroon;
                    }
                    else
                    {
                        brush.Color = Color.White;
                    }
                    Rectangle rect = new Rectangle((i * w) , (j * h) , w , h);
                    graphics.FillRectangle(brush , rect);
                    //    graphics.DrawRectangle(pen , rect);

                }
            }
        }

        private void drawWorker_DoWork(object sender , DoWorkEventArgs e)
        {
            while(!drawWorker.CancellationPending)
            {
                DrawGrid();
            }
        }
    }
}

