using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Drawing;
using System.IO;

namespace ConwayGameOfLife_wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        DispatcherTimer timer;
        Dictionary<Coordinates, CellState> Cells;
        int sizeOfGrid;
        int generation;
        bool isGenerated;
        Bitmap gridBitmap;
            
        public MainWindow()
        {
            InitializeComponent();
            CreateGrids(16);
            Dictionary<Coordinates, CellState> InitialCells = new Dictionary<Coordinates, CellState>();
            InitialCells = GLife.GenerateLife(16);
            PopulateGrid(InitialCells);
            isGenerated = false;
            _pDishImg.SnapsToDevicePixels = true;
        }

        private void PopulateGrid(Dictionary<Coordinates, CellState> dic)
        {
            foreach(KeyValuePair<Coordinates, CellState> cell in dic)
            {
                Coordinates coord = cell.Key;
                CellState state = cell.Value;
                Grid block = new Grid();
                if(state == CellState.ALIVE)
                {
                    block.Background = System.Windows.Media.Brushes.Black;
                }
                else
                {
                    block.Background = System.Windows.Media.Brushes.White;
                }
                Grid.SetRow(block, coord.X);
                Grid.SetColumn(block, coord.Y);
                _pDish.Children.Add(block);
            }

        }

        private void ClearGrid()
        {
            _pDish.Children.Clear();
        }

        private void CreateGrids(int gridSize)
        {
            _pDish.RowDefinitions.Clear();
            _pDish.ColumnDefinitions.Clear();
            for(int i = 1; i <= gridSize; i++ )
            {
                ColumnDefinition col = new ColumnDefinition();
                _pDish.ColumnDefinitions.Add(col);
                RowDefinition row = new RowDefinition();
                _pDish.RowDefinitions.Add(row);
            }
        }

        private void initBitmap()
        {
            gridBitmap = new Bitmap(sizeOfGrid, sizeOfGrid, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            for (int x = 0; x < gridBitmap.Width; x++)
            {
                for (int y = 0; y < gridBitmap.Height; y++)
                {
                    foreach (KeyValuePair<Coordinates, CellState> cell in Cells)
                    {
                        Coordinates cellCoord = cell.Key;
                        CellState neighbourState = cell.Value;
                        if (cellCoord.X == x && cellCoord.Y == y)
                        {
                            if (neighbourState == CellState.ALIVE)
                            {
                                gridBitmap.SetPixel(x, y, System.Drawing.Color.Black);
                                break; //Save time. Next items are redundant
                            }
                            else
                            {
                                gridBitmap.SetPixel(x, y, System.Drawing.Color.White);
                                break; //Save time. Next items are redundant
                            }
                        }
                    }
                }
            }

        }

        public BitmapImage ConvertBitmap(Bitmap src)
        {
            System.IO.MemoryStream ms = new MemoryStream();
            ((System.Drawing.Bitmap)src).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();
            return image;
        }

        private void _startButton_Click(object sender, RoutedEventArgs e)
        {
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0 ,0, 300);
            timer.Tick += Timer_Tick; 
            int size = 0;
            int.TryParse(_gridSize.Text, out size);
            double probablity = 0;
            double.TryParse(_lifeGeneration.Text, out probablity);
            if(size != 0 && probablity != 0)
            {
                sizeOfGrid = size;
                Cells = new Dictionary<Coordinates, CellState>();
                Cells = GLife.GenerateLife(sizeOfGrid, probablity);
                initBitmap();
                _pDishImg.Source = ConvertBitmap(gridBitmap);
                /*
                 * oldway
                ClearGrid();
                CreateGrids(sizeOfGrid);
                PopulateGrid(Cells);
                */
                timer.Start();
                isGenerated = true;
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (isGenerated)
            {
                isGenerated = false;
                Cells = GLife.NextGen(sizeOfGrid, Cells);
                generation++;
                _generationLabel.Content = string.Format("GENERATION: {0}", generation.ToString());
                /*
                ClearGrid();
                PopulateGrid(Cells);
                */
                initBitmap();
                _pDishImg.Source = ConvertBitmap(gridBitmap);
                //gridBitmap.Save(@"C:\Users\Jericho Masigan\Documents\Test Environment\gameoflife\" + generation.ToString() + ".bmp", System.Drawing.Imaging.ImageFormat.Bmp);
                isGenerated = true;
            }
        }

        private void _stopButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
        }
    }
}
