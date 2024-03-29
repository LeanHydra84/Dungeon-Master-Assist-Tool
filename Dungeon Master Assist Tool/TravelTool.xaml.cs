﻿using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Dungeon_Master_Assist_Tool
{
    /// <summary>
    /// Interaction logic for Window4.xaml
    /// </summary>
    public partial class WindowTravel : Window
    {
        public WindowTravel()
        {
            InitializeComponent();
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
            this.Close();
        }
    }
}
