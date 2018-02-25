using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class Grid
    {
        Point[,] world;

        public int Width { get; private set; }
        public int Height { get; private set; }

        public class Point
        {
            private bool status = false;
            private bool swap = false;
            public Point(bool status) => this.status = status;
            public void flip() => this.status = !status;
            public void kill() => swap = false;
            public void revive() => swap = true;
            public bool IsAlive => status;
            public void refresh() => status = swap;
        }

        public Grid(int x , int y) => Initialize(x , y);
        public void clear() => Initialize(Width , Height);
        public bool IsAlive(int x , int y) => world[x , y].IsAlive;
        public void flipPoint(int x , int y) => world[x , y].flip();

        public void Initialize(int x , int y)
        {
            Width = x;
            Height = y;
            world = new Point[x , y];

            for(int i = 0; i < Width; i++)
            {
                for(int j = 0; j < Height; j++)
                {
                    world[i , j] = new Point(false);
                }
            }
        }

        public void Randomize()
        {
            Random rnd = new Random();

            for(int i = 0; i < Width; i++)
                for(int j = 0; j < Height; j++)
                {
                    if(rnd.Next(0 , 10) > 2)
                        world[i , j] = new Point(false);
                    else
                        world[i , j] = new Point(true);
                }
        }

        public void Advance(int steps = 1)
        {
            for(int x = 0; x < Width; x++)
            {
                for(int y = 0; y < Height; y++)
                {
                    int alive = getNeighbors(x , y);

                    if(world[x , y].IsAlive && (alive < 2 || alive > 3))
                    {
                        world[x , y].kill();
                    }
                    else if(alive == 3)
                    {
                        world[x , y].revive();
                    }

                }
            }

            for(int x = 0; x < Width; x++)
                for(int y = 0; y < Height; y++)
                {
                    world[x , y].refresh();
                }
        }

        //Counts live neighbors
        public int getNeighbors(int x , int y)
        {
            int alive = 0;

            for(int i = -1; i <= 1; i++)
            {
                for(int j = -1; j <= 1; j++)
                {
                    //do not count self as neighbor
                    if(!(i == 0 && j == 0))
                    {
                        //Wraps around the Edges
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

                        if(world[offsetX , offsetY].IsAlive)
                            alive++;
                    }
                }
            }
            return alive;
        }
    }
}
