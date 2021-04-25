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

        private void dragEventHandler(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void CreateMonster_Click(object sender, EventArgs e)
        {
            var dialog = new CreateMonster();

            if(dialog.ShowDialog() == true)
            {
                Debug.WriteLine("we got him");
            }

        }

        private void CloseButtonClicked(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MinimizeButtonClicked(object sender, EventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void BattleMapwindow_Click(object sender, RoutedEventArgs e)
        {
            Window battleMap = new WindowBattle();
            battleMap.Show();
        }

        private void DMGwindow_Click(object sender, RoutedEventArgs e)
        {
            Window damageWindow = new WindowDMG();
            damageWindow.Show();
        }

        private void Travelwindow_Click(object sender, RoutedEventArgs e)
        {
            Window travelWindow = new WindowTravel();
            travelWindow.DataContext = manager;
            travelWindow.Show();
        }
        private void Noteswindow_Click(object sender, RoutedEventArgs e)
        {
            Window notesWindow = new WindowNotes();
            notesWindow.Show();
        }

    }
}
