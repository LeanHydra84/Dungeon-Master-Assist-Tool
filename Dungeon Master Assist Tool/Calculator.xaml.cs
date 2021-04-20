using System;
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
    public partial class WindowCalc : Window
    {
        Double Value = 0; // Store Decimal and Int Values if needed
        String Operand; // Store the Operand - Will Determine the Maths
        Boolean OperandPressed;

        public WindowCalc()
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

    private void Clear_Click(object sender, RoutedEventArgs e)
        {
            Value = 0;
            Display.Content = "0";
            Equation.Content = "";
        }

        private void CE_Click(object sender, RoutedEventArgs e)
        {
            Display.Content = "0";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            Button B = (Button)sender; // Cast Generic Object To Buttom

            //To Get Rid of the Initial 0 in the Display
            if ((Display.Content.ToString() == "0") || (OperandPressed))
            {
                Display.Content = " ";
            }

            // If Decimal Point already exsists within the Display then do not add another Dedcimal Point
            if (B.Content.ToString() == ".")
            {
                if (!Display.Content.ToString().Contains("."))
                {
                    Display.Content = Display.Content.ToString() + B.Content.ToString(); // Append Number onto Display 
                }
            }
            else
            {
                Display.Content = Display.Content.ToString() + B.Content.ToString(); // Append Number onto Display 
            }

            OperandPressed = false;
        }

        private void Button_Operand(object sender, RoutedEventArgs e)
        {
            Button B = (Button)sender; // Cast Obj to Button

            // Continously operate on the current results without having to press equals.
            if (Value != 0)
            {
                Button_Equals(this, null); // emulate the equals button press
                Operand = B.Content.ToString(); // store operand
                OperandPressed = true;
                Equation.Content = Value + " " + Operand; // Update the Equation Label
            }
            else
            {
                Operand = B.Content.ToString();
                Value = Double.Parse(Display.Content.ToString()); // Store the First Value
                OperandPressed = true;
                Equation.Content = Value + " " + Operand;
            }

        }

        private void Button_Equals(object sender, RoutedEventArgs e)
        {
            Equation.Content = " ";

            /*Maths is dependent on which operand has been pressed:
             Do the correct maths with first value stored earlier and second value which is currently on the display. */
            switch (Operand)
            {
                case "+":
                    Display.Content = Value + Double.Parse(Display.Content.ToString());
                    break;
                case "-":
                    Display.Content = Value - Double.Parse(Display.Content.ToString());
                    break;
                case "×":
                    Display.Content = Value * Double.Parse(Display.Content.ToString());
                    break;
                case "÷":
                    Display.Content = Value / Double.Parse(Display.Content.ToString());
                    break;
                default:
                    break;
            }

            Value = Double.Parse(Display.Content.ToString()); //Whatever Last one display is stored in Value
            Operand = ""; // Clear Operand once pressed

        }

        /*Functionality Added to make the calculator work with Keyboard NumberPad
         When the correct Key is detected it will fire off the corresponding button press*/
        private void Window_KeyDownPreview(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.NumPad0:
                    Zero.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
                case Key.NumPad1:
                    One.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
                case Key.NumPad2:
                    Two.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
                case Key.NumPad3:
                    Three.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
                case Key.NumPad4:
                    Four.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
                case Key.NumPad5:
                    Five.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
                case Key.NumPad6:
                    Six.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
                case Key.NumPad7:
                    Seven.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
                case Key.NumPad8:
                    Eight.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
                case Key.NumPad9:
                    Nine.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
                case Key.Add:
                    Plus.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
                case Key.Subtract:
                    Minus.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
                case Key.Multiply:
                    Multiply.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
                case Key.Divide:
                    Divide.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
                case Key.Enter:
                    Equals.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
            }
        }
    }
}