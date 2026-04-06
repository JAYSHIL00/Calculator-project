using System;
using System.Windows.Forms;

namespace Calculator
{
    public partial class Form1 : Form
    {
        // Stores the first part of the calculation
        private double result = 0;
        
        // Stores the current operation (+, -, *, /)
        private string currentOperator = "";
        
        // Helps track when we need to clear the screen for typing the next number
        private bool isOperatorClicked = false;

        public Form1()
        {
            InitializeComponent();
        }

        // This single event handler handles all number buttons (0-9)
        private void btnNumber_Click(object sender, EventArgs e)
        {
            // If the screen shows "0" (initial state) or an operator was just pressed, clear the screen
            if (txtDisplay.Text == "0" || isOperatorClicked)
            {
                txtDisplay.Clear();
            }

            isOperatorClicked = false;
            
            // Identify which button was clicked and append its text (the number) to the display
            Button button = (Button)sender;
            txtDisplay.Text = txtDisplay.Text + button.Text;
        }

        // This single event handler handles all basic operations (+, -, *, /)
        private void btnOperator_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            // If we are chaining operations (e.g. 5 + 5 + ...), we optionally calculate the previous first
            if (result != 0 && !isOperatorClicked && currentOperator != "")
            {
                btnEqual_Click(this, e);
            }

            // Read the current display value into the result variable for processing
            if (double.TryParse(txtDisplay.Text, out double number))
            {
                result = number;
            }

            // Store the operator to know what to perform when "=" is pressed
            currentOperator = button.Text;
            
            // Set flag so next number click will start a fresh number on display
            isOperatorClicked = true;
        }

        // Calculates the final value
        private void btnEqual_Click(object sender, EventArgs e)
        {
            // Get the second number currently present on the display
            if (double.TryParse(txtDisplay.Text, out double num))
            {
                switch (currentOperator)
                {
                    case "+":
                        txtDisplay.Text = (result + num).ToString();
                        break;
                    case "-":
                        txtDisplay.Text = (result - num).ToString();
                        break;
                    case "*":
                        txtDisplay.Text = (result * num).ToString();
                        break;
                    case "/":
                        // Prevent application crash on divide-by-zero
                        if (num == 0)
                        {
                            MessageBox.Show("Cannot divide by zero!", "Math Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        txtDisplay.Text = (result / num).ToString();
                        break;
                    default:
                        // Do nothing if no operator is set
                        break;
                }

                // Make the result the new stored value so they can keep calculating consecutively
                if (double.TryParse(txtDisplay.Text, out double newResult))
                {
                    result = newResult;
                }

                // Clear the current operator so we are ready for a new operation string
                currentOperator = "";
                
                // Keep true so if a number is typed next, we clear the answer first
                isOperatorClicked = true;
            }
        }

        // Completely clears the calculator
        private void btnClear_Click(object sender, EventArgs e)
        {
            txtDisplay.Text = "0";
            result = 0;
            currentOperator = "";
            isOperatorClicked = false;
        }
    }
}
