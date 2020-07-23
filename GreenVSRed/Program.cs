using System;

namespace GreenVSRed
{
    class Program
    {
        class Cell //Class that has the functionalities for a cell.
        {
            public int WidthPosition { get; private set; } //This value is the x coordinate of the cell.
            public int HeightPosition { get; private set; } //This value is the y coordinate of the cell.

            public int Colour { get; set; } //This value is the colour of the cell.

            public Cell(int heightPosition, int widthPosition, int state)
            {
                WidthPosition = widthPosition;
                HeightPosition = heightPosition;
                Colour = state;
            }
        }

        class Row //Class that has the functionalities of a row.
        {
            public string row { get; private set; } //A row is initially a string.
            public int HeightPosition { get; private set; } //This is the y coordinate of the row.
            public Cell[] cells; //The row consists of cells.

            public Row(string row, int height)
            {
                this.row = row;
                cells = new Cell[row.Length];

                for (int i = 0; i < row.Length; i++) //We turn every character of the string row into a array of cells.
                {
                    cells[i] = new Cell(height, i, int.Parse(row[i].ToString()));
                }
                HeightPosition = height;
            }
        }

        class Grid //Class that has the functionalities of a grid.
        {
            public int Width { get; private set; } //X dimension of the grid.
            public int Height { get; private set; } //Y dimension of the grid.
            public Cell[,] cells; //The row consists of cells.

            public Grid(Row[] rows) //As input an array of rows is given.
            {
                Width = rows[0].row.Length;
                Height = rows.Length;

                cells = new Cell[Width, Height];
                //We populate the grid with cells.
                for (int j = 0; j < Height; j++) 
                {
                    for (int i = 0; i < Width; i++)
                    {
                        cells[i, j] = rows[j].cells[i];
                    }
                }
            }

            public int GetNeighbours(Cell cell)
            {
                int greenNeighbours = 0;
                for (int k = -1; k <= 1; k++)
                    for (int l = -1; l <= 1; l++)
                    {
                        if (!((cell.WidthPosition + k < 0) || (cell.WidthPosition + k > Width - 1) || (cell.HeightPosition + l < 0) || (cell.HeightPosition + l > Height - 1)))
                        {
                            greenNeighbours += cells[cell.WidthPosition + k, cell.HeightPosition + l].Colour;
                        }
                    }
                greenNeighbours -= cell.Colour;

                return greenNeighbours;
            }
            public void DisplayGrid()
            {
                for (int j = 0; j < Height; j++)
                {
                    for (int i = 0; i < Width; i++)
                    {
                        Console.Write(cells[i, j].Colour);
                    }
                    Console.WriteLine();
                }
            }
        }

        class TransformGrid //This class deals with grid generations.
        {
            public Grid newGrid;

            public TransformGrid()
            {                
            }

            public Grid Update(Grid grid)//Converts one grid to another. The transformation is based on the provided rules.
            {
                newGrid = grid;
                int[,] newCells = new int[grid.Width, grid.Height]; //All the cells from the provided original grid are saved as an 2-dimensional array.

                foreach (Cell cell in grid.cells) //The rules of the game are applied here.
                {//When a rule is applied, changes are made to the value of the new formed cells.
                    if ((cell.Colour == 0) & ((grid.GetNeighbours(cell) == 3) || (grid.GetNeighbours(cell) == 6)))
                    {
                        newCells[cell.WidthPosition, cell.HeightPosition] = 1;
                    }
                    else if ((cell.Colour == 1) & !((grid.GetNeighbours(cell) == 2) || (grid.GetNeighbours(cell) == 3) || (grid.GetNeighbours(cell) == 6)))
                    {
                        newCells[cell.WidthPosition, cell.HeightPosition] = 0;
                    }
                    else
                    {
                        newCells[cell.WidthPosition, cell.HeightPosition] = grid.cells[cell.WidthPosition, cell.HeightPosition].Colour;
                    }
                }
                //The newly generated cells are replacing the old ones from the grid.
                foreach (Cell cell in newGrid.cells)
                {
                    cell.Colour = newCells[cell.WidthPosition, cell.HeightPosition];
                }
                return newGrid;
            }
        }

        static void Main(string[] args)
        {
            #region Failsafes
            //This region makes sure the input values from the user are in the proper format.
            
            int InputNumber() //This checks for the input of number of columns or number of rows.
            {
                if (Int32.TryParse(Console.ReadLine(), out int a)) //This values should always be integer.
                {
                    if (a>999 || a<1) //The number of rows and columns should be bigger than 0 and smaller than 1 000.
                    {
                        Console.WriteLine("Should be between 1 and 999! Try again:");
                        return InputNumber();
                    }
                    else
                    {
                        return a;
                    }
                }
                else
                {
                    Console.WriteLine("Not a proper value! Try again:");
                    return InputNumber();
                }
            }

            string InputRow(int limit) //This checks for the input of the values for one row.
            {
                string result = Console.ReadLine();
                if (result.Length == limit) //Each row should be the same length as the other ones.
                {
                    foreach (char c in result)
                    {
                        if (c != '1' && c != '0') //Only ones and zeros are excepted as values for each cell.
                        {
                            Console.WriteLine("Row should contain only ones and zeros! Try again:");
                            return InputRow(limit);
                        }
                    }
                    return result;
                }
                else
                {
                    Console.WriteLine("Row does not have the right number of characters! Try again:");
                    return InputRow(limit);
                }
            }

            int InputCoordinate(int limit) //Checks if requested cell coordinates are in bounds.
            {
                if (Int32.TryParse(Console.ReadLine(), out int a))
                {
                    if (a > limit - 1 || a < 0)
                    {
                        Console.WriteLine("Should be between {0} and {1}! Try again:",0, limit-1);
                        return InputCoordinate(limit);
                    }
                    else
                    {
                        return a;
                    }
                }
                else
                {
                    Console.WriteLine("Not a proper numeric value! Try again:");
                    return InputCoordinate(limit);
                }
            }

            int InputGenerations() //Checks if input number of generations is a positive integer value.
            {
                if (Int32.TryParse(Console.ReadLine(), out int a))
                {
                    if (a < 1)
                    {
                        Console.WriteLine("Should be bigger than 0! Try again:");
                        return InputGenerations();
                    }
                    else
                    {
                        return a;
                    }
                }
                else
                {
                    Console.WriteLine("Not a proper value! Try again:");
                    return InputGenerations();
                }
            }
            #endregion

            int x = 0;
            int y = 0;
            Console.WriteLine("Please, input number of columns of the grid:");
            x = InputNumber(); //We store the exact number of columns the grid will have.
            Console.WriteLine("Please, input number of rows of the grid:");
            y = InputNumber(); //We store the exact number of rows the grid will have.
            Console.WriteLine(x + ", " + y);

            //Next, we create a collection of rows based on the user input.
            Row[] row0 = new Row[y]; 

            for (int i = 0; i < y; i++)
            {
                Console.WriteLine("Please, input row number " + i + " of the grid:");
                Row row = new Row(InputRow(x), i); //User enters the values for each row.
                row0[i] = row;
            }

            Grid grid = new Grid(row0); //The rows are assembled into a grid.

            Console.WriteLine("Grid Zero:");
            grid.DisplayGrid(); //Grid is displayed.

            int x1;
            int y1;
            int N;

            Console.WriteLine("Please, input cell coordinate x:");
            x1 = InputCoordinate(x); //Coordinate x for which sell to watch are taken.
            Console.WriteLine("Please, input cell coordinate y:");
            y1 = InputCoordinate(y); //Coordinate y for which sell to watch are taken.
            Console.WriteLine("Please, input number of generations:");
            N = InputGenerations(); //Number of generations is taken.

            int result = 0;

            if (grid.cells[x1, y1].Colour == 1)
                result++; //We check, if the cell is green in the original grid

            for (int n = 0; n < N; n++)
            {
                TransformGrid updater = new TransformGrid(); //We update the grid.
                grid = updater.Update(grid);

                if (grid.cells[x1, y1].Colour == 1)
                    result++; //We increment the number of times the cell in question has been green for every grid generation.
            }

            Console.WriteLine("Number of generations in which the cell was green:");
            Console.WriteLine(result); //Final result is shown.

        }               
    }
}
