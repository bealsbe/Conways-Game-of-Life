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

            public void refresh()
            {
                status = swap;
            }
        }

        public Grid(int x , int y) => Initialize(x , y);

        public void Initialize(int x , int y)
        {
            Width = x;
            Height = y;

            world = new Point[x , y];

            for(int i = 0; i < Width; i++)
                for(int j = 0; j < Height; j++) {
                    world[i , j] = new Point(false);
                }
        }

        public void Randomize()
        {
            Random rnd = new Random();
            for(int i = 0; i < Width; i++)
                for(int j = 0; j < Height; j++) {
                    if(rnd.Next(0 , 10) > 2) {
                        world[i , j] = new Point(false);
                    }
                    else {
                        world[i , j] = new Point(true);
                    }

                }

        }

        public void clear() => Initialize(Width , Height);

        public bool IsAlive(int x , int y) => world[x , y].IsAlive;

        public void Advance(int steps = 1)
        {
            for(int i = 0; i < steps; i++) {
                step();
                refresh();
            }
        }

        private void refresh()
        {
            for(int x = 0; x < Width; x++)
                for(int y = 0; y < Height; y++) {
                    world[x , y].refresh();
                }
        }

        private void step()
        {
            for(int x = 0; x < Width; x++) {
                for(int y = 0; y < Height; y++) {
                    int alive = getNeighbors(x , y);

                    if(world[x , y].IsAlive) {  //alive
                        if(alive == 0) {  //if no alive blocks around  then kill it
                            world[x , y].kill();
                        }
                        else if(alive < 2) {  // if there is less then 2 alive around then kill it
                            world[x , y].kill();
                        }
                        else if(alive == 2 || alive == 3) { //if there is 2 or 3 alive around it then it will live on
                            world[x , y].revive();
                        }
                        else if(alive > 3) { //if there are more then 3 then it dies from overpopulation
                            world[x , y].kill();
                        }
                    }
                    else {  //dead
                        if(alive == 3) {
                            world[x , y].revive();
                        }
                        else {
                            world[x , y].kill();
                        }
                    }

                }
            }
        }

        //Counts live neighbors
        public int getNeighbors(int x , int y)
        {
            int alive = 0;

            for(int i = -1; i <= 1; i++) {
                for(int j = -1; j <= 1; j++) {

                    //do not count self as neighbor
                    if(!(i == 0 && j == 0)) {
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


        public void flipPoint(int x , int y) => world[x , y].flip();

        public void PrintGrid()
        {
            for(int x = 0; x < Width; x++) {
                for(int y = 0; y < Height; y++) {
                    if(world[x , y].IsAlive)
                        Console.Write("#");
                    else
                        Console.Write("~");
                }
                Console.WriteLine();
            }
        }
    }
}
