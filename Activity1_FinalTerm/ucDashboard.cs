using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Activity1_FinalTerm
{
    public partial class ucDashboard : UserControl
    {
        public ucDashboard()
        {
            InitializeComponent();

        }

        private void loadStats()
        {
            
            try
            {

                using (OleDbConnection conn = Database.GetConnection())
                {

                    conn.Open();

                    string q = @"SELECT 
                                    COUNT(*) AS TotalProducts, 
                                    SUM(IIF(Quantity > ReorderLevel, 1, 0)) AS InStock, 
                                    SUM(IIF(Quantity > 0 AND Quantity <= ReorderLevel, 1, 0)) AS LowStock, 
                                    SUM(IIF(Quantity <= 0, 1, 0)) AS OutOfStock 
                                    FROM Products";

                    using (OleDbCommand cmd = new OleDbCommand(q, conn))
                    using (OleDbDataReader rdr = cmd.ExecuteReader())
                    {

                        if(rdr.Read())
                        {
                            lblTotalProducts.Text = rdr["TotalProducts"].ToString();
                            lblInStock.Text = rdr["InStock"].ToString();
                            lblLowStock.Text = rdr["LowStock"].ToString();
                            lblOutOfStock.Text = rdr["OutOfStock"].ToString();
                        }

                    }

                }

            }
            catch (Exception ex)
            {

                MessageBox.Show("Statistic Error: " + ex.Message, "Statistics Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }

        private void ucDashboard_Load(object sender, EventArgs e)
        {
            if (!this.DesignMode)
            {
                loadStats();
            }
            
        }
    }
}
