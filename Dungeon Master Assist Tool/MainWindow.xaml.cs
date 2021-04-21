using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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

namespace Dungeon_Master_Assist_Tool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        DataManager manager;

        public MainWindow()
        {
            InitializeComponent();
            manager = (DataManager)DataContext;

            MonsterListBox.SelectionChanged += manager.MonsterData.UpdateIndex;
            MonsterSearchBar.TextChanged += manager.MonsterData.UpdateSearchQuery;

            SpellListBox.SelectionChanged += manager.SpellsList.UpdateIndex;
            SpellSearchBar.TextChanged += manager.SpellsList.UpdateSearchQuery;

        }

        public void dragEventHandler(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        public void CloseButtonClicked(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MinimizeButtonClicked(object sender, EventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Calcwindow_Click(object sender, RoutedEventArgs e)
        {
            Window Calculator = new WindowCalc();
            Calculator.Show();
        }

        private void BattleMapwindow_Click(object sender, RoutedEventArgs e)
        {
            Window BattleMap = new WindowBattle();
            BattleMap.Show();
        }

        private void DMGwindow_Click(object sender, RoutedEventArgs e)
        {
            Window DMGwindow = new WindowDMG();
            DMGwindow.Show();
        }

        private void Travelwindow_Click(object sender, RoutedEventArgs e)
        {
            Window Travelwindow = new WindowTravel();
            Travelwindow.Show();
        }
        private void Noteswindow_Click(object sender, RoutedEventArgs e)
        {
            Window Noteswindow = new WindowNotes();
            Noteswindow.Show();
        }

    }
}
