// <copyright file="CalculatorControl.xaml.cs" company="Ivan Yankov">
//     Copyright (c) Ivan Yankov 2018. All rights reserved.
// </copyright>
// <summary>.</summary>
// <author>Ivan Yankov</author>
namespace Calculator
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for CalculatorControl.
    /// </summary>
    public partial class CalculatorControl : UserControl
    {
        /// <summary>
        /// Represents the current math expression, which will be computed.
        /// </summary>
        public static readonly DependencyProperty ExpressionProperty =
            DependencyProperty.Register(
            "Expression",
            typeof(string),
            typeof(CalculatorControl),
            new PropertyMetadata(string.Empty));

        /// <summary>
        /// Represents the math result of the performed math operations.
        /// </summary>
        public static readonly DependencyProperty ResultProperty =
            DependencyProperty.Register(
            "Result",
            typeof(string),
            typeof(CalculatorControl),
            new PropertyMetadata("0"));

        /// <summary>
        /// ICommand used to delegate the input bindings.
        /// </summary>
        public static readonly DependencyProperty InputCommandProperty =
            DependencyProperty.Register(
            "InputCommand",
            typeof(ICommand),
            typeof(CalculatorControl),
            new PropertyMetadata(null));

        /// <summary>
        /// The copy result command.
        /// </summary>
        public static readonly DependencyProperty CopyCommandProperty =
            DependencyProperty.Register(
            "CopyCommand",
            typeof(ICommand),
            typeof(CalculatorControl),
            new PropertyMetadata(null));

        /// <summary>
        /// Saves the last pushed button.
        /// </summary>
        private CalculatorButton lastButton;

        /// <summary>
        /// Initializes a new instance of the <see cref="CalculatorControl"/> class.
        /// </summary>
        public CalculatorControl()
        {
            this.InitializeComponent();
            this.InputCommand = new RelayCommand(this.OnInputCommandRequested);
            this.CopyCommand = new RelayCommand(this.OnCopyResultRequested);
            this.lastButton = new CalculatorButton() { FunctionType = FunctionType.Clear };
        }

        /// <summary>
        /// Gets or sets the current math expression.
        /// </summary>
        public string Expression
        {
            get { return (string)this.GetValue(ExpressionProperty); }
            set { this.SetValue(ExpressionProperty, value); }
        }

        /// <summary>
        /// Gets or sets the current math result.
        /// </summary>
        public string Result
        {
            get { return (string)this.GetValue(ResultProperty); }
            set { this.SetValue(ResultProperty, value); }
        }

        /// <summary>
        /// Gets or sets input command, which will be executed when a certain key is pressed.
        /// </summary>
        public ICommand InputCommand
        {
            get { return (ICommand)this.GetValue(InputCommandProperty); }
            set { this.SetValue(InputCommandProperty, value); }
        }

        /// <summary>
        /// Gets or sets input command, which will be executed when the user wants to copy the calculator result.
        /// </summary>
        public ICommand CopyCommand
        {
            get { return (ICommand)this.GetValue(CopyCommandProperty); }
            set { this.SetValue(CopyCommandProperty, value); }
        }

        /// <summary>
        /// Event handler, used for all the calculator buttons.
        /// </summary>
        /// <param name="sender">The object, which sends the event.</param>
        /// <param name="e">The routed event arguments.</param>
        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            var button = e.Source as CalculatorButton;
            if (button != null)
            {
                this.ProcessUserInput(button);
            }
        }

        /// <summary>
        /// Executed, when the user hits some of the defined input bindings.
        /// </summary>
        /// <param name="parameter">The command parameter. In this case it must be the button content.</param>
        private void OnInputCommandRequested(object parameter)
        {
            string buttonContent = parameter.ToString();
            if (buttonContent == ".")
            {
                // Changing the hardcoded (at the xaml) dot
                buttonContent = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            }

            var button = VisualHelper.FindButton(this, buttonContent);
            if (button != null)
            {
                this.ProcessUserInput(button);
            }
        }

        /// <summary>
        /// Executed, when the user wants to copy the calculator result.
        /// </summary>
        /// <param name="parameter">Parameter of object type. Null at this case.</param>
        private void OnCopyResultRequested(object parameter)
        {
            Clipboard.SetText(this.Result);
        }

        /// <summary>
        /// Performs the needed operations after the user input.
        /// </summary>
        /// <param name="button">The button, which has been pressed.</param>
        private void ProcessUserInput(CalculatorButton button)
        {
            if (button == null)
            {
                throw new ArgumentNullException("button");
            }

            if (button.FunctionType == FunctionType.Clear)
            {
                this.Result = "0";
                this.Expression = string.Empty;
                this.lastButton = button;
                return;
            }
            else if (button.FunctionType == FunctionType.ClearEntry)
            {
                this.Result = "0";
            }

            if (button.FunctionType == FunctionType.LeftBracket)
            {
                if (this.lastButton.FunctionType == FunctionType.LeftBracket || this.lastButton.IsOperatorButton || this.lastButton.FunctionType == FunctionType.Clear)
                {
                    this.Expression += button.Text;
                    this.Result = "0";
                    this.lastButton = button;
                }

                return;
            }
            else if (button.FunctionType == FunctionType.RightBracket)
            {
                int leftBracketsCount = 0;
                int rightBracketsCount = 0;

                foreach (var ch in this.Expression)
                {
                    if (ch == '(')
                    {
                        leftBracketsCount++;
                    }
                    else if (ch == ')')
                    {
                        rightBracketsCount++;
                    }
                }

                if (leftBracketsCount - rightBracketsCount < 1)
                {
                    this.lastButton = button;
                    return;
                }

                this.Expression += this.Result + button.Text;
                this.Result = string.Empty;
                this.lastButton = button;
                return;
            }

            // Checking if the result is not a text (i.e we have an error).
            double resultNumber;
            bool isParseOK = double.TryParse(
                this.Result,
                NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign,
                CultureInfo.CurrentCulture,
                out resultNumber);

            if (this.Result.Length != 0 && (!isParseOK || double.IsInfinity(resultNumber) || double.IsNaN(resultNumber)))
            {
                // The user can only use the 'C' (clear all) button.
                return;
            }

            if (button.IsDigitButton)
            {
                if (this.Result == "0")
                {
                    this.Result = string.Empty;
                }

                if (this.lastButton.IsOperatorButton)
                {
                    this.Result = button.Text;
                }
                else
                {
                    this.Result += button.Text;
                }

                this.lastButton = button;
                return;
            }

            if (button.IsOperatorButton)
            {
                if (this.lastButton.IsOperatorButton)
                {
                    // Changing the last operator
                    var oldExpression = this.Expression;
                    var index = oldExpression.LastIndexOf(" " + this.lastButton.Text[0] + " ");
                    var newExpression = oldExpression.Remove(index, 3);
                    newExpression += " " + button.Text[0] + " ";
                    this.Expression = newExpression;
                }
                else if (this.lastButton.IsBracket)
                {
                    this.Expression += " " + button.Text + " ";
                }
                else
                {
                    this.Expression += this.Result + " " + button.Text + " ";
                }

                this.lastButton = button;
                return;
            }

            if (button.FunctionType == FunctionType.Equals)
            {
                this.Expression += this.Result;
                try
                {
                    this.Result = CalculatorCore.Operations.Compute(this.Expression).ToString();
                }
                catch (Exception ex)
                {
                    this.Result = ex.Message;
                }

                this.Expression = string.Empty;
            }
            else if (button.FunctionType == FunctionType.Back)
            {
                if (this.Result.Length > 0 && this.lastButton.IsErasableButton)
                {
                    this.Result = this.Result.Substring(0, this.Result.Length - 1);
                    if (this.Result.Length == 0)
                    {
                        this.Result = "0";
                    }
                }
            }
            else if (button.FunctionType == FunctionType.Comma)
            {
                if (!this.lastButton.IsErasableButton)
                {
                    this.Result = "0" + button.Text;
                    this.lastButton = button;
                    return;
                }

                if (!this.Result.Contains(button.Text))
                {
                    this.Result += button.Text;
                }
            }
            else if (button.IsCustomFunction)
            {
                string fname = button.FunctionType.ToString().ToLower();
                if (this.Result == string.Empty)
                {
                    this.Expression = string.Format("{0}({1})", fname, this.Expression);
                }
                else
                {
                    this.Expression += string.Format("{0}({1})", fname, this.Result);
                }

                this.Result = string.Empty;
            }
            else if (button.FunctionType == FunctionType.Pi)
            {
                this.Result = Math.PI.ToString();
            }
            else if (button.FunctionType == FunctionType.E)
            {
                this.Result = Math.E.ToString();
            }

            this.lastButton = button;
        }
    }
}