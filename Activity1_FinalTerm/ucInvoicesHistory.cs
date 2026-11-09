using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Activity1_FinalTerm
{
    public partial class ucInvoicesHistory : UserControl
    {
        public ucInvoicesHistory()
        {
            InitializeComponent();
        }

        private void ucInvoicesHistory_Load(object sender, EventArgs e)
        {

        }

        private void roundedTextBox1_Load(object sender, EventArgs e)
        {
            roundedTextBox1.PlaceholderText = "Search by Invoice ID, Customer Name, Phone...";
            roundedTextBox1.PlaceholderColor = Color.Gray;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void flatComboBox1_Load(object sender, EventArgs e)
        {
        }

        private void flatComboBox2_Load(object sender, EventArgs e)
        {
            flatComboBox2.SelectedIndex = 0;
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
