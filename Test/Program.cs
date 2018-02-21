using System;
using System.Collections;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public enum Status { alive, dead }

    public class Grid
    {
        Hashtable points = new Hashtable();
        int Width;
        int Height;

        public class Point
        {

            public bool swap { get; private set; } = false;
            public Status status { get; private set; }

            public Point(Status status) => this.status = status;

            public void Inverse() => swap = true;

            public bool IsAlive => (status == Status.alive);

            public void refresh()
            {
                if(swap) {
                    if(IsAlive)
                        status = Status.dead;
                    else
                        status = Status.alive;
                }
                swap = false;
            }
        }

        public Grid(int x , int y)
        {
            Width = x;
            Height = y;
            Random rnd = new Random();

            for(int i = 0; i < Width; i++)
                for(int j = 0; j < Height; j++) {
                    if(rnd.Next(0 , 10) < 5) {
                        points.Add($"{i},{j}" , new Point(Status.dead));
                    }
                    else {
                        points.Add($"{i},{j}" , new Point(Status.alive));
                    }
                }
        }

        public void Advance(int steps = 1)
        {
            for(int i = 0; i < steps; i++) {
                step();
                PrintGrid();
                refresh();
            }
        }

        private void refresh()
        {
             for(int x = 0; x < Width; x++) 
                for(int y = 0; y < Height; y++) {
                    Point p = (Point)points[$"{x},{y}"];
                    p.refresh();
                    points[$"{x} {y}"] = p;
                }
        }

        private void step()
        {
            for(int x = 0; x < Width; x++) {
                for(int y = 0; y < Height; y++) {
                    Tuple<int , int> neighbors = getNeighbors(x , y);
                    Point p = (Point)points[$"{x},{y}"];

                    if((p.IsAlive && (neighbors.Item2 == 8 || neighbors.Item1 < 2 || neighbors.Item1 > 3))
                      || !p.IsAlive && neighbors.Item1 == 3)
                        p.Inverse();

                    points[$"{x} {y}"] = points;
                }
            }
        }

        public Tuple<int , int> getNeighbors(int x , int y)
        {
            int alive = 0;
            int dead = 0;

            for(int i = -1; i <= 1; i++) {
                for(int j = -1; j <= 1; j++) {

                    if(i == 0 && j == 0) {
                        break;
                    }

                    int offsetX = (x + i);
                    int offsetY = (y + j);

                    if(offsetX == -1)
                        offsetX = Width - 1;
                    else if(offsetX == Width)
                        offsetX = 0;

                    if(offsetY == -1)
                        offsetY = Height - 1;
                    else if(offsetY == Height)
                        offsetY = 0;

                   Point neighbor = (Point)points[$"{offsetX},{offsetY}"];

                    if(!(x == offsetX && y == offsetY) && neighbor.IsAlive)
                        alive++;
                    else if(!(x == offsetX && y == offsetY) && !neighbor.IsAlive)
                        dead++;

                }
            }
            return Tuple.Create(alive , dead);
        }

        public void PrintGrid()
        {
            Console.CursorVisible = false;
            for(int x = 0; x < Width; x++) {
                for(int y = 0; y < Height; y++) {
                    Point p = (Point)points[$"{x},{y}"];
                    Console.SetCursorPosition(x , y);
                    if(p.IsAlive && p.swap)
                        Console.Write("#");
                    else
                        Console.Write("~");
                }
                Console.Write("\n");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();
            Grid g = new Grid(90 ,27);
            while(true) {
                g.Advance();
            }

        }
    }
}

