using System;
using System.Data;
using System.Windows.Forms;

namespace Calculator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            SetTextbox("0");

            // Prevent resizing
            this.FormBorderStyle = FormBorderStyle.FixedDialog;

            // Disable maximize button
            this.MaximizeBox = false;

            // Disable minimize button (optional)
            this.MinimizeBox = false;
        }

        // Number buttons
        private void zero_button_Click(object sender, EventArgs e) => AppendOutput("0");
        private void one_button_Click(object sender, EventArgs e) => AppendOutput("1");
        private void two_button_Click(object sender, EventArgs e) => AppendOutput("2");
        private void three_button_Click(object sender, EventArgs e) => AppendOutput("3");
        private void four_button_Click(object sender, EventArgs e) => AppendOutput("4");
        private void five_button_Click(object sender, EventArgs e) => AppendOutput("5");
        private void six_button_Click(object sender, EventArgs e) => AppendOutput("6");
        private void seven_button_Click(object sender, EventArgs e) => AppendOutput("7");
        private void eight_button_Click(object sender, EventArgs e) => AppendOutput("8");
        private void nine_button_Click(object sender, EventArgs e) => AppendOutput("9");

        // Decimal button
        private void decimal_button_Click(object sender, EventArgs e)
        {
            string text = text_box.Text;
            // Prevent multiple decimals in the current number
            int lastOperatorIndex = Math.Max(text.LastIndexOf('+'), Math.Max(text.LastIndexOf('-'), Math.Max(text.LastIndexOf('*'), text.LastIndexOf('/'))));
            string lastNumber = lastOperatorIndex >= 0 ? text.Substring(lastOperatorIndex + 1) : text;

            if (!lastNumber.Contains("."))
            {
                AppendOutput(".");
            }
        }

        // Operator buttons
        private void add_button_Click(object sender, EventArgs e) => AppendOperator("+");
        private void subtract_button_Click(object sender, EventArgs e) => AppendOperator("-");
        private void multiply_button_Click(object sender, EventArgs e) => AppendOperator("*");
        private void divide_button_Click(object sender, EventArgs e) => AppendOperator("/");

        private void AppendOperator(string op)
        {
            if (!IsLastCharOperator(text_box.Text))
                AppendOutput(op);
        }

        // Special buttons
        private void clear_button_Click(object sender, EventArgs e) => SetTextbox("0");

        private void equals_button_Click(object sender, EventArgs e)
        {
            try
            {
                string expression = text_box.Text;

                // Prevent evaluation if last char is an operator
                if (IsLastCharOperator(expression))
                {
                    SetTextbox("ERROR");
                    return;
                }

                // Evaluate using DataTable
                var resultObj = new DataTable().Compute(expression, null);

                // Convert to float
                float result = Convert.ToSingle(resultObj);

                // Display result
                SetTextbox(result.ToString());
            }
            catch
            {
                SetTextbox("ERROR");
            }
        }

        // Helper functions
        private void AppendOutput(string newOutput)
        {
            if (text_box.Text != "0")
                text_box.Text += newOutput;
            else
                text_box.Text = newOutput;
        }

        private void SetTextbox(string text) => text_box.Text = text;

        private bool IsLastCharOperator(string text)
        {
            if (string.IsNullOrEmpty(text)) return false;
            char last = text[text.Length - 1];
            return last == '+' || last == '-' || last == '*' || last == '/';
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
