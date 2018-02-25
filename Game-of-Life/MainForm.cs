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
        Graphics graphics;


        public MainForm()
        {
            InitializeComponent();
            grid = new Grid(100 , 100);
            grid.Randomize();
            backgroundWorker.WorkerSupportsCancellation = true;
            comboBox_size.SelectedItem = "100";
            DrawArea = new Bitmap(drawBox.Size.Width , drawBox.Size.Height);
        }

        private void button1_Click(object sender , EventArgs e)
        {
            if(!backgroundWorker.IsBusy)
            {
                backgroundWorker.RunWorkerAsync();
            }
        }


        private void drawBox_onMousedown(object sender , MouseEventArgs e)
        {
            if(!backgroundWorker.IsBusy)
            {
                this.Invoke((Action)(() =>
                {
                    var point = drawBox.PointToClient(Cursor.Position);
                    int x = point.X / (drawBox.Width / grid.Width + 1);
                    int y = point.Y / (drawBox.Height  / grid.Height + 1);
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
                DrawGrid();
                System.GC.Collect();
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
            this.Invoke((Action)(() =>
            {
                drawBox.Image = DrawArea;
                graphics = Graphics.FromImage(DrawArea);
            }));

            SolidBrush brush = new SolidBrush(Color.Black);
            Pen pen = new Pen(Color.Black);

            float w = (drawBox.Width / grid.Width+1);
            float h = (drawBox.Height / grid.Height+1);

            for(int i = 0; i < grid.Width; i++)
            {
                for(int j = 0; j < grid.Height; j++)
                {
                    if(grid.IsAlive(i , j))
                    {
                        brush.Color = Color.LimeGreen;
                    }
                    else
                    {
                        brush.Color = Color.Black;
                    }
                    Rectangle rect = new Rectangle((int)(i * w) , (int)(j * h) , (int)w , (int)h);
                    graphics.FillRectangle(brush , rect);
                    graphics.DrawRectangle(pen , rect);
                }
            }
        }

        private void button5_Click(object sender , EventArgs e)
        {
            if(!backgroundWorker.IsBusy)
            {
                grid.Advance();
                DrawGrid();
            }
        }

        private void MainForm_Load(object sender , EventArgs e) => DrawGrid();
    }
}

