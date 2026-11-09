using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Activity1_FinalTerm
{
    public partial class Dashboard : Form
    {
        public Dashboard(string DisplayName)
        {
            InitializeComponent();

            txtUsername.Text = DisplayName;

            txtUsername.AutoSize = true;

            SetActiveButton(btnDashboard);
        }

        private void ShowPage(UserControl page)
        {
            mainPanel.Controls.Clear();

            page.Dock = DockStyle.Fill;

            mainPanel.Controls.Add(page);
            page.BringToFront();
        }

        private void SetActiveButton(Button activeButton)
        {
            Color activeColor = Color.ForestGreen;
            Color defaultColor = Color.White;
            Color activeText = Color.White;
            Color defaultText = Color.Black;
            Color mouseDownColor = Color.DarkSeaGreen;
            Color mouseOverColor = Color.DarkSeaGreen;
            Color defaultMouseDownOverColor = Color.LightGray;

            foreach (Control control in panel1.Controls)
            {
                if (control is Button btn)
                {
                    if (btn == btnLogout) continue;

                    if (btn == activeButton)
                    {
                        btn.BackColor = activeColor;
                        btn.ForeColor = activeText;
                        btn.FlatAppearance.MouseDownBackColor = mouseDownColor;
                        btn.FlatAppearance.MouseOverBackColor = mouseOverColor;
                    }
                    else
                    {
                        btn.BackColor = defaultColor;
                        btn.ForeColor = defaultText;
                        btn.FlatAppearance.MouseOverBackColor = defaultMouseDownOverColor;
                        btn.FlatAppearance.MouseDownBackColor = defaultMouseDownOverColor;
                    }
                }
            }
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            ShowPage(new ucDashboard());
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to logout?", "Confirm Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Restart();
            }
        }

        private void btnCustomers_Click(object sender, EventArgs e)
        {
            mainPanel.Controls.Clear();
            ucCustomers customers = new ucCustomers();
            customers.Dock = DockStyle.Fill;
            mainPanel.Controls.Add(customers);
            SetActiveButton(btnCustomers);
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void txtUsername_Click(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void toolStripContainer1_ContentPanel_Load(object sender, EventArgs e)
        {

        }

        private void toolStripContainer2_LeftToolStripPanel_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void panel3_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void btnDashboard_Click_1(object sender, EventArgs e)
        {
            mainPanel.Controls.Clear();
            ucDashboard dashboard = new ucDashboard();
            dashboard.Dock = DockStyle.Fill;
            mainPanel.Controls.Add(dashboard);
            SetActiveButton(btnDashboard);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel7_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void btnSalesReport_Click(object sender, EventArgs e)
        {
            mainPanel.Controls.Clear();
            ucSalesReport salesReport = new ucSalesReport();
            salesReport.Dock = DockStyle.Fill;
            mainPanel.Controls.Add(salesReport);
            SetActiveButton(btnSalesReport);
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void chart1_Click_1(object sender, EventArgs e)
        {

        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnInvoices_Click(object sender, EventArgs e)
        {
            mainPanel.Controls.Clear();
            ucInvoicesHistory invoices = new ucInvoicesHistory();
            invoices.Dock = DockStyle.Fill;
            mainPanel.Controls.Add(invoices);
            SetActiveButton(btnInvoices);
        }

        private void btnPOS_Click(object sender, EventArgs e)
        {
            mainPanel.Controls.Clear();
            ucPOS POS = new ucPOS();
            POS.Dock = DockStyle.Fill;
            mainPanel.Controls.Add(POS);
            SetActiveButton(btnPOS);
        }

        private void btnProducts_Click(object sender, EventArgs e)
        {
            mainPanel.Controls.Clear();
            ucProducts products = new ucProducts();
            products.Dock = DockStyle.Fill;
            mainPanel.Controls.Add(products);
            SetActiveButton(btnProducts);
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            mainPanel.Controls.Clear();
            ucSettings settings = new ucSettings();
            settings.Dock = DockStyle.Fill;
            mainPanel.Controls.Add(settings);
            SetActiveButton(btnSettings);
        }
    }
}
