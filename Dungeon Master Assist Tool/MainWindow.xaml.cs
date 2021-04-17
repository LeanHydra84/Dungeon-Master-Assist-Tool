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

    }
}
