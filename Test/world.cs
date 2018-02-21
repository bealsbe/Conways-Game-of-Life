using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{


    class Game
    {

       // public Point[,] points;

        public Game(int x = 50 , int y = 50)
        {
            
        }

        public void Advance(int steps = 1)
        {
            for(int i = steps; i > 0; i--) {
                Step();
            }
        }

        public void Step()
        {

        }

        /*
        private void Step()
        {
            char[,] updatedWorld = new char[50,50];

            for(int i = 0; i < 50; i++) {
                for(int j = 0; j < 50; j++) {
                    int dead = 0;  //surronding alive tiles
                    int alive = 0; // surronding dead tiles
                    for(int x = -1; x <= 1; x++) { //loops through all of the surronding tiles
                        for(int y = -1; y <= 1; y++) {
                            int xi = x + i;
                            int jy = j + y;

                            if(xi == -1) { //warps around
                                xi = 49;
                            }
                            else if(xi == 50) {
                                xi = 0;
                            }

                            if(jy == -1) {
                                jy = 49;
                            }
                            else if(jy == 50) {
                                jy = 0;
                            }
                            if(world[xi , jy] == '*') {
                                if(!(x == 0 && y == 0)) { //skips the actual tile
                                    alive++;
                                }
                            }
                            else {
                                if(!(x == 0 && y == 0)) {
                                    dead++;
                                }
                            }
                        }
                    }
                    checkRules(updatedWorld , i , j , alive , dead , world[i , j]);
                }
            }
            world = updatedWorld;
            printWorld(world);

        }
        */
        //decices how the world changes based on the game rules
        void checkRules(char[,] updatedWorld , int i , int j , int alive , int dead , char thechar)
        {
            if(thechar == '*') {  //alive
                if(dead == 8) {  //if no alive blocks around  then kill it
                    updatedWorld[i , j] = ' ';
                }
                else if(alive < 2) {  // if there is less then 2 alive around then kill it
                    updatedWorld[i , j] = ' ';
                }
                else if(alive == 2 || alive == 3) { //if there is 2 or 3 alive around it then it will live on
                    updatedWorld[i , j] = thechar;
                }
                else if(alive > 3) { //if there are more then 3 then it dies from overpopulation
                    updatedWorld[i , j] = ' ';
                }
                else {
                    updatedWorld[i , j] = thechar; //just in case something is missed (no garbage values in arrary)
                }
            }
            else {  //dead
                if(alive == 3) {
                    updatedWorld[i , j] = '*'; //if it's surronded by 3 alive then it become alive
                }
                else {
                    updatedWorld[i , j] = thechar;  //otherwise stay dead
                }
            }
        }


        void printWorld(char[,] world)
        {
            Console.Clear();
            for(int i = 0; i < 50; i++) {
                for(int j = 0; j < 50; j++) {
                    Console.Write($"{world[i , j]} ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

    }
}
