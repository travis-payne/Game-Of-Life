﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Game_Of_Life
{
    class Grid
    {
        private static int ITERATION_NUMBER;
        private Cell[,] _cells;
        public int _grid_size { get; }


        public Grid(int grid_size, int initial_live_cells)
        { 

            ITERATION_NUMBER = 0;
            if (initial_live_cells > grid_size * grid_size)
                initial_live_cells = grid_size * grid_size;

            this._grid_size = grid_size;
            Create_Grid();
            Generate_Initial_Live_Cells(initial_live_cells);


        }

        private void Create_Grid()
        {
            this._cells = new Cell[this._grid_size, this._grid_size];

            for (int i = 0; i < this._grid_size; i++)
            {
                for (int j = 0; j < this._grid_size; j++)
                {
                    this._cells[i, j] = new Cell(false, i, j);
                }
            }



        }

        private void Generate_Initial_Live_Cells(int initial_live_cells)
        {
            for (int i = 0; i < initial_live_cells; i++)
            {
                Position_At_Random();
            }
        }
        private void Position_At_Random()
        {
            Random rnd = new Random();
            Cell cell = _cells[rnd.Next(0, _cells.GetLength(0)), rnd.Next(0, _cells.GetLength(1))];
            if (cell.Is_Cell_Alive())
            {
                Position_At_Random();
            }
            else
            {
                cell.Set_Cell_To_Alive();
            }



        }




        public void Iterate()
        {
            Console.Clear();
            if (ITERATION_NUMBER == 0)
            {
                ITERATION_NUMBER++;

                Draw_Grid();
            }
            else
            {
                Process_Grid_Iteration();
                ITERATION_NUMBER++;
                Draw_Grid();
            }
            Console.WriteLine("Iteration Number: " + ITERATION_NUMBER);
        }

        private void Process_Grid_Iteration()
        {
            Grid temp_grid = Copy_Cells();
            
            for (int i = 0; i < _grid_size; i++)
            {
                String s = "";
                for (int j = 0; j < _grid_size; j++)
                {
                    int y = Calculate_Cell_Neighbours(_cells[i, j]);

                    Determine_Cell_Action(temp_grid._cells[i, j], y);
                    s += " " + y;
                }
                //Console.WriteLine(s);
            }
            this._cells = temp_grid._cells;
        }

        private Grid Copy_Cells()
        {
            Grid temp = new Grid(this._grid_size,0);

            for (int i = 0; i < _grid_size; i++)
            {
                for (int j = 0; j < _grid_size; j++)
                {
                    temp._cells[i, j] = new Cell(_cells[i, j].Is_Cell_Alive(), _cells[i, j].Get_X(), _cells[i, j].Get_Y());
                }
            }

            return temp;
        }

        private bool InBounds(int x, int y)
        {
            return x >= 0 && y >= 0 && x < _grid_size && y < _grid_size;

        }

        private int Calculate_Cell_Neighbours(Cell cell)
        {
            int neighbours = 0;

            int cell_x = cell.Get_X();
            int cell_y = cell.Get_Y();



            for (int i = cell_x - 1; i < cell_x + 2; i++)
            {
                for (int j = cell_y - 1; j < cell_y + 2; j++)
                {
                    if (InBounds(i, j))
                    {
                        if (_cells[i, j].Is_Cell_Alive() && !_cells[i,j].Equals(cell))
                            neighbours++;
                    }
                }
            }

            return neighbours;
        }

        private void Draw_Grid()
        {
            for (int i = 0; i < this._grid_size; i++)
            {
                for (int j = 0; j < this._grid_size; j++)
                {
                    if (this._cells[i, j].Is_Cell_Alive())
                    {
                        Console.Write("[*]");
                    }
                    else
                    {
                        Console.Write("[ ]");

                    }
                }
                Console.WriteLine();
            }
        }

        private void Determine_Cell_Action(Cell cell, int neighbours)
        {
            switch(neighbours){

                case 0:
                case 1:
                    // Underpopulation
                    cell.Set_Cell_To_Dead();
                    break;
                case 2:
                    // Survival
                    break;
                case 3:
                    // Creation of Life
                    if (!cell.Is_Cell_Alive())
                        cell.Set_Cell_To_Alive();
                    // Survival
                    break;
                case var x when (neighbours > 3 ):
                    // Overcrowding
                    if (cell.Is_Cell_Alive())
                        cell.Set_Cell_To_Dead();
                    break;



            }
        }



    }
}
