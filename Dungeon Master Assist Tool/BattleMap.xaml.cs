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
using System.Windows.Shapes;

namespace Dungeon_Master_Assist_Tool
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class WindowBattle : Window
    {
        public WindowBattle()
        {
            InitializeComponent();
        }

        private void dragEventHandler(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void CloseButtonClicked(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MinimizeButtonClicked(object sender, EventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        // Image Scaling

        private double maxScaleDelta = 0.8d;
        private double minSize = 100;

        private void Image_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            var trans = ((TransformGroup)battleImage.RenderTransform).Children[0] as ScaleTransform;

            double zoom = (e.Delta < 1/maxScaleDelta) ? 1/maxScaleDelta : (e.Delta > maxScaleDelta) ? maxScaleDelta : e.Delta;

            double newWidth = ((Vector)trans.Transform(new Point(battleImage.ActualWidth, 0)) - (Vector)trans.Transform(new Point(0, 0))).Length; 

            if(newWidth / zoom > minSize)
            {
                trans.ScaleX /= zoom;
                trans.ScaleY /= zoom;
            }
        }

        // Image Panning

        Point start, origin;

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {

            var trans = ((TransformGroup)battleImage.RenderTransform).Children[1] as TranslateTransform;

            start = e.GetPosition(imageBorder);
            origin = new Point(trans.X, trans.Y);

        }

        private void Image_MouseMove(object sender, MouseEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                var trans = ((TransformGroup)battleImage.RenderTransform).Children[1] as TranslateTransform;

                Vector v = start - e.GetPosition(imageBorder);

                trans.X = origin.X - v.X;
                trans.Y = origin.Y - v.Y;

            }
        }


    }
}
