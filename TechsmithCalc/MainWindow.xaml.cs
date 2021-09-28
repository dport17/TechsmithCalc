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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TechsmithCalc
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Button1.Click += delegate(object sender, RoutedEventArgs e) { Button_Click(sender, e, '1'); };
            Button2.Click += delegate(object sender, RoutedEventArgs e) { Button_Click(sender, e, '2'); };
            Button3.Click += delegate(object sender, RoutedEventArgs e) { Button_Click(sender, e, '3'); };
            Button4.Click += delegate(object sender, RoutedEventArgs e) { Button_Click(sender, e, '4'); };
            Button5.Click += delegate(object sender, RoutedEventArgs e) { Button_Click(sender, e, '5'); };
            Button6.Click += delegate(object sender, RoutedEventArgs e) { Button_Click(sender, e, '6'); };
            Button7.Click += delegate(object sender, RoutedEventArgs e) { Button_Click(sender, e, '7'); };
            Button8.Click += delegate(object sender, RoutedEventArgs e) { Button_Click(sender, e, '8'); };
            Button9.Click += delegate(object sender, RoutedEventArgs e) { Button_Click(sender, e, '9'); };
            Button0.Click += delegate(object sender, RoutedEventArgs e) { Button_Click(sender, e, '0'); };


            ButtonDot.Click += delegate (object sender, RoutedEventArgs e) { Button_Click(sender, e, '.'); };
            ButtonPlus.Click += delegate (object sender, RoutedEventArgs e) { Button_Click(sender, e, '+'); };
            ButtonMinus.Click += delegate (object sender, RoutedEventArgs e) { Button_Click(sender, e, '-'); };
            ButtonExpo.Click += delegate (object sender, RoutedEventArgs e) { Button_Click(sender, e, '^'); };
            ButtonDiv.Click += delegate (object sender, RoutedEventArgs e) { Button_Click(sender, e, '/'); };
            ButtonX.Click += delegate (object sender, RoutedEventArgs e) { Button_Click(sender, e, '*'); };

            ButtonDEL.Click += delegate (object sender, RoutedEventArgs e) { Delete(sender, e); };
            ButtonEq.Click += delegate (object sender, RoutedEventArgs e) { Calculate(sender, e); };
        }

        private void EquationBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //char typedChange = EquationBox.Text[EquationBox.Text.Length - 1];

            if (EquationBox.Text.Length == 1 && (EquationBox.Text.Equals("0") || !double.TryParse(EquationBox.Text, out double result)))
            {
                EquationBox.Text = "";
            }
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            if(EquationBox.Text.Length > 0)
            {
                EquationBox.Text = EquationBox.Text.Substring(0, EquationBox.Text.Length - 1);
            }
            
        }



        //make sure you check whether there's too much in the box.
        private void Calculate(object sender, RoutedEventArgs e)
        {
            string eq = EquationBox.Text;
            try
            {
                string postfix = InfixToPostfix(eq);
                EquationBox.Text = evalPostfix(postfix).ToString();
            }
            catch(Exception exc)
            {
                MessageBox.Show("Invalid Input.");
                EquationBox.Text = "";
                return;
            }
            

            
        }

        private double evalPostfix(string eq)
        {
            Stack<double> opStack = new Stack<double>();
            List<string> opList = eq.Split(" ").ToList();
            foreach (string op in opList)
            {
                if (double.TryParse(op, out double res))
                {
                    opStack.Push(res);
                }
                else
                {
                    double op2 = opStack.Pop();
                    double op1 = opStack.Pop();
                    double result = Operate(op1, op2, op);
                    opStack.Push(result);
                }
            }
            return opStack.Pop();
        }

        private double Operate(double first, double second, string op)
        {
            if (op == "+")
            {
                return Add(first, second);
            }
            else if(op == "-")
            {
                return Subtract(first, second);
            }
            else if (op == "*")
            {
                return Multiply(first, second);
            }
            else if(op == "/")
            {
                return Divide(first, second);
            }
            else
            {
                return Expo(first, second);
            }
        }

        private double Expo(double first, double second)
        {
            return Math.Pow(first, second);
        }

        private double Divide(double first, double second)
        {
            return first / second;
        }

        private double Multiply(double first, double second)
        {
            return first * second;
        }

        private double Subtract(double first, double second)
        {
            return first - second;
        }

        private double Add(double first, double second)
        {
            return first + second;
        }

        private void Button_Click(object sender, RoutedEventArgs e, char value)
        {
            if (EquationBox.Text.Equals("0"))
            {
                EquationBox.Text = "";
            }
            EquationBox.Text += value;
        }

        private string InfixToPostfix(string eq)
        {
            string result = "";
            Stack<char> operatorStack = new Stack<char>();

            foreach(char c in eq)
            {  
                if (char.IsDigit(c) || c == '.')
                {
                    result += c;
                }
                else
                {
                    result += " ";
                    if (operatorStack.Count == 0)
                    {
                        operatorStack.Push(c);
                    }
                    else if(PrecVal(c) > PrecVal(operatorStack.Peek()))
                    {
                        operatorStack.Push(c);
                        
                    }
                    else
                    {
                        while(operatorStack.Count>0 && !(PrecVal(c) > PrecVal(operatorStack.Peek())))
                        {
                            result += operatorStack.Pop()+" ";
                        }
                        operatorStack.Push(c);
                    }
                }
            }
            while(operatorStack.Count>0)
            {
                result += " " + operatorStack.Pop();
                
            }
            return result;
        }

        private int PrecVal(char c)
        {
            Dictionary<char, int> precDict = new Dictionary<char, int>();
            precDict.Add('^', 4);
            precDict.Add('*', 3);
            precDict.Add('/', 2);
            precDict.Add('+', 1);
            precDict.Add('-', 1);
            return precDict[c];
        }
    }

}
