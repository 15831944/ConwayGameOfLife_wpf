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

        public MainWindow()
        {
            InitializeComponent();
            CreateGrids(16);
            Dictionary<Coordinates, CellState> InitialCells = new Dictionary<Coordinates, CellState>();
            InitialCells = GLife.GenerateLife(16);
            PopulateGrid(InitialCells);

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
                    block.Background = Brushes.Black;
                }
                else
                {
                    block.Background = Brushes.White;
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
            for(int i = 0; i <= gridSize; i++ )
            {
                ColumnDefinition col = new ColumnDefinition();
                _pDish.ColumnDefinitions.Add(col);
                RowDefinition row = new RowDefinition();
                _pDish.RowDefinitions.Add(row);
            }
        }

        private void _startButton_Click(object sender, RoutedEventArgs e)
        {
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
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
                ClearGrid();
                CreateGrids(sizeOfGrid);
                PopulateGrid(Cells);
                timer.Start();
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {

            Cells = GLife.NextGen(sizeOfGrid, Cells);

            ClearGrid();
            PopulateGrid(Cells);
        }

        private void _stopButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
        }
    }
}
