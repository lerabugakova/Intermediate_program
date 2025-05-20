using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GameOfLife
{
    class Grid
    {
        private readonly int SizeX;
        private readonly int SizeY;
        private readonly Cell[,] cells;
        private readonly Cell[,] nextGenerationCells;
        private readonly Ellipse[,] cellsVisuals;
        private readonly Canvas drawCanvas;
        private static readonly Random rnd = new Random();

        public Grid(Canvas canvas)
        {
            drawCanvas = canvas;
            SizeX = (int)(canvas.Width / 5);
            SizeY = (int)(canvas.Height / 5);

            cells = new Cell[SizeX, SizeY];
            nextGenerationCells = new Cell[SizeX, SizeY];
            cellsVisuals = new Ellipse[SizeX, SizeY];

            InitializeCells();
            SetRandomPattern();
            InitCellsVisuals();
            UpdateGraphics();
        }

        private void InitializeCells()
        {
            for (int i = 0; i < SizeX; i++)
            {
                for (int j = 0; j < SizeY; j++)
                {
                    cells[i, j] = new Cell(i, j, 0, false);
                    nextGenerationCells[i, j] = new Cell(i, j, 0, false);
                }
            }
        }

        public void Clear()
        {
            for (int i = 0; i < SizeX; i++)
            {
                for (int j = 0; j < SizeY; j++)
                {
                    cells[i, j].IsAlive = false;
                    cells[i, j].Age = 0;

                    nextGenerationCells[i, j].IsAlive = false;
                    nextGenerationCells[i, j].Age = 0;

                    cellsVisuals[i, j].Fill = Brushes.Gray;
                }
            }
        }

        private void MouseMove(object sender, MouseEventArgs e)
        {
            if (sender is Ellipse cellVisual)
            {
                int i = (int)(cellVisual.Margin.Left / 5);
                int j = (int)(cellVisual.Margin.Top / 5);

                if (e.LeftButton == MouseButtonState.Pressed && !cells[i, j].IsAlive)
                {
                    cells[i, j].IsAlive = true;
                    cells[i, j].Age = 0;
                    cellVisual.Fill = Brushes.White;
                }
            }
        }

        public void UpdateGraphics()
        {
            for (int i = 0; i < SizeX; i++)
            {
                for (int j = 0; j < SizeY; j++)
                {
                    Brush targetBrush = cells[i, j].IsAlive
                        ? (cells[i, j].Age < 2 ? Brushes.White : Brushes.DarkGray)
                        : Brushes.Gray;

                    if (!cellsVisuals[i, j].Fill.Equals(targetBrush))
                        cellsVisuals[i, j].Fill = targetBrush;
                }
            }
        }

        public void InitCellsVisuals()
        {
            for (int i = 0; i < SizeX; i++)
            {
                for (int j = 0; j < SizeY; j++)
                {
                    Ellipse ellipse = new Ellipse
                    {
                        Width = 5,
                        Height = 5,
                        Fill = Brushes.Gray,
                        Margin = new Thickness(cells[i, j].PositionX, cells[i, j].PositionY, 0, 0)
                    };

                    ellipse.MouseMove += MouseMove;
                    ellipse.MouseLeftButtonDown += MouseMove;

                    cellsVisuals[i, j] = ellipse;
                    drawCanvas.Children.Add(ellipse);
                }
            }
        }

        public void Dispose()
        {
            for (int i = 0; i < SizeX; i++)
            {
                for (int j = 0; j < SizeY; j++)
                {
                    var ellipse = cellsVisuals[i, j];
                    ellipse.MouseMove -= MouseMove;
                    ellipse.MouseLeftButtonDown -= MouseMove;
                }
            }

            drawCanvas.Children.Clear();
        }

        public static bool GetRandomBoolean() => rnd.NextDouble() > 0.8;

        public void SetRandomPattern()
        {
            for (int i = 0; i < SizeX; i++)
            {
                for (int j = 0; j < SizeY; j++)
                {
                    cells[i, j].IsAlive = GetRandomBoolean();
                    cells[i, j].Age = 0;
                }
            }
        }

        public void UpdateToNextGeneration()
        {
            for (int i = 0; i < SizeX; i++)
            {
                for (int j = 0; j < SizeY; j++)
                {
                    cells[i, j].IsAlive = nextGenerationCells[i, j].IsAlive;
                    cells[i, j].Age = nextGenerationCells[i, j].Age;
                }
            }

            UpdateGraphics();
        }

        public void Update()
        {
            bool alive = false;
            int age = 0;

            for (int i = 0; i < SizeX; i++)
            {
                for (int j = 0; j < SizeY; j++)
                {
                    CalculateNextGeneration(i, j, ref alive, ref age);
                    nextGenerationCells[i, j].IsAlive = alive;
                    nextGenerationCells[i, j].Age = age;
                }
            }

            UpdateToNextGeneration();
        }

        public void CalculateNextGeneration(int row, int column, ref bool isAlive, ref int age)
        {
            isAlive = cells[row, column].IsAlive;
            age = cells[row, column].Age;

            int count = CountNeighbors(row, column);

            if (isAlive && count < 2 || isAlive && count > 3)
            {
                isAlive = false;
                age = 0;
            }
            else if (isAlive && (count == 2 || count == 3))
            {
                age = ++cells[row, column].Age;
                isAlive = true;
            }
            else if (!isAlive && count == 3)
            {
                isAlive = true;
                age = 0;
            }
        }

        public int CountNeighbors(int i, int j)
        {
            int count = 0;

            for (int dx = -1; dx <= 1; dx++)
            {
                int nx = i + dx;
                if (nx < 0 || nx >= SizeX) continue;

                for (int dy = -1; dy <= 1; dy++)
                {
                    int ny = j + dy;
                    if (ny < 0 || ny >= SizeY) continue;
                    if (dx == 0 && dy == 0) continue;

                    if (cells[nx, ny].IsAlive) count++;
                }
            }

            return count;
        }
    }
}
