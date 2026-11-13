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

namespace Activity1_FinalTerm
{
    public partial class ucProducts : UserControl
    {

        private int selectedProductID = 0;

        public ucProducts()
        {
            InitializeComponent();

            txtCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            txtSearch.SetPlaceholder("Search Product...", showWhenFocused: true);

            loadProducts();
        }

        private void loadProducts(string searchKeyword = "")
        {

            if(searchKeyword == "Search Product...")
            {
                searchKeyword = "";
            }

            try
            {

                using (OleDbConnection conn = Database.GetConnection())
                {
                    
                    conn.Open();

                    string q = @"SELECT * FROM Products";

                    if(!string.IsNullOrWhiteSpace(searchKeyword))
                    {

                        q += " WHERE ProductName LIKE ? OR Category LIKE ? OR Description LIKE ?";

                    }

                    q += " ORDER BY ProductID DESC";

                    using (OleDbCommand cmd = new OleDbCommand(q, conn))
                    {

                        if(!string.IsNullOrWhiteSpace(searchKeyword))
                        {

                            string pattern = "%" + searchKeyword + "%";
                            cmd.Parameters.Add("?", OleDbType.VarWChar).Value = pattern;
                            cmd.Parameters.Add("?", OleDbType.VarWChar).Value = pattern;
                            cmd.Parameters.Add("?", OleDbType.VarWChar).Value = pattern;

                        }

                        using (OleDbDataAdapter adapter = new OleDbDataAdapter(cmd))
                        {

                            DataTable dt = new DataTable();

                            adapter.Fill(dt);

                            dgvProducts.DataSource = null;
                            dgvProducts.Columns.Clear();
                            dgvProducts.AutoGenerateColumns = true;

                            dgvProducts.DataSource = dt;

                            int productCount = dt.Rows.Count;

                            label1.Text = $"Showing {productCount} {(productCount <= 1 ? "Product" : "Products")}";

                        }


                    }
                
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show("Unable to load products: " + ex.Message, "Unable to load products", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            popupAddProduct.Visible = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            popupAddProduct.Visible = false;

            btnEditProduct.BackColor = Color.Silver;
            btnDelete.BackColor = Color.Silver;

            ClearField();
        }

        private void popupAddProduct_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ucProducts_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if(txtSearch.Text == "Search Product...")
            {
                loadProducts("");
                return;
            }

            loadProducts(txtSearch.Text);
        }

        private void txtSearch_MouseDown(object sender, MouseEventArgs e)
        {
        }

        private bool ValidateProduct()
        {
            
            if(string.IsNullOrEmpty(txtProductName.Text))
            {
                MessageBox.Show("Please enter a product name.");
                txtProductName.Focus();
                return false;
            }

            if (txtCategory.SelectedIndex == -1)
            {
                MessageBox.Show("Please enter a category.");
                txtCategory.Focus();
                return false;
            }

            decimal price;

            if (!decimal.TryParse(txtUnitPrice.Text, out price) || price < 0)
            {
                MessageBox.Show("Please enter a product price.");
                txtUnitPrice.Focus();
                return false;
            }

            int quantity;

            if (!int.TryParse(txtQuantity.Text, out quantity) || quantity < 0)
            {
                MessageBox.Show("Please enter quantity.");
                txtQuantity.Focus();
                return false;
            }

            int reorder;

            if (!int.TryParse(txtReorderLevel.Text, out reorder) || reorder < 0)
            {
                MessageBox.Show("Please enter reorder level.");
                txtReorderLevel.Focus();
                return false;
            }

            return true;

        }

        private void btnConfirmAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateProduct())
            {
                return;
            }

            try
            {

                using (OleDbConnection conn = Database.GetConnection())
                {

                    conn.Open();

                    string q = @"INSERT INTO Products (ProductName, Category, Description, UnitPrice, Quantity, ReorderLevel, CreatedAt) VALUES (?, ?, ?, ?, ?, ?, Now())";

                    using (OleDbCommand cmd = new OleDbCommand(q, conn))
                    {

                        cmd.Parameters.Add("?", OleDbType.VarWChar).Value = txtProductName.Text.Trim();
                        cmd.Parameters.Add("?", OleDbType.VarWChar).Value = txtCategory.Text;
                        cmd.Parameters.Add("?", OleDbType.LongVarWChar).Value = txtDescription.Text.Trim();
                        cmd.Parameters.Add("?", OleDbType.Currency).Value = decimal.Parse(txtUnitPrice.Text);
                        cmd.Parameters.Add("?", OleDbType.Integer).Value = int.Parse(txtQuantity.Text);
                        cmd.Parameters.Add("?", OleDbType.Integer).Value = int.Parse(txtReorderLevel.Text);

                        cmd.ExecuteNonQuery();

                    }

                }

                MessageBox.Show("Product added successfully!", "Product Added", MessageBoxButtons.OK, MessageBoxIcon.Information);

                popupAddProduct.Visible = false;
                btnEditProduct.BackColor = Color.Silver;
                btnDelete.BackColor = Color.Silver;

                ClearField();
                loadProducts();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding products: " + ex.Message, "Error adding products", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            btnEditProduct.BackColor = Color.MediumSeaGreen;
            btnDelete.BackColor = Color.Maroon;

            if (e.RowIndex >= 0)
            {

                DataGridViewRow row = dgvProducts.Rows[e.RowIndex];

                selectedProductID = Convert.ToInt32(row.Cells["ProductID"].Value);

                txtEditProductName.Text = row.Cells["ProductName"].Value.ToString();
                txtEditCategory.Text = row.Cells["Category"].Value.ToString();
                txtEditDescription.Text = row.Cells["Description"].Value.ToString();
                txtEditUnitPrice.Text = row.Cells["UnitPrice"].Value.ToString();
                txtEditQuantity.Text = row.Cells["Quantity"].Value.ToString();
                txtEditReorderLevel.Text = row.Cells["ReorderLevel"].Value.ToString();

            }

        }

        private void ClearField()
        {
            txtProductName.Clear();
            txtCategory.SelectedIndex = -1;
            txtUnitPrice.Clear();
            txtQuantity.Clear();
            txtReorderLevel.Clear();
            txtDescription.Clear();

            selectedProductID = 0;

            dgvProducts.ClearSelection();
        }

        private void btnEditProduct_Click(object sender, EventArgs e)
        {

            if (selectedProductID == 0)
            {
                
                MessageBox.Show("Please select a product row to update");
                return;

            }

            popupAddProduct.Visible = false;
            popupEditProduct.Visible = true;

        }

        private bool ValidateEditProduct()
        {
            if (string.IsNullOrWhiteSpace(txtEditProductName.Text))
            {
                MessageBox.Show("Please enter a product name.");
                txtEditProductName.Focus();
                return false;
            }

            if (txtEditCategory.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a category.");
                txtEditCategory.Focus();
                return false;
            }

            if (!decimal.TryParse(txtEditUnitPrice.Text, out decimal price) || price < 0)
            {
                MessageBox.Show("Please enter a valid product price.");
                txtEditUnitPrice.Focus();
                return false;
            }

            if (!int.TryParse(txtEditQuantity.Text, out int quantity) || quantity < 0)
            {
                MessageBox.Show("Please enter a valid quantity.");
                txtEditQuantity.Focus();
                return false;
            }

            if (!int.TryParse(txtEditReorderLevel.Text, out int reorder) || reorder < 0)
            {
                MessageBox.Show("Please enter a valid reorder level.");
                txtEditReorderLevel.Focus();
                return false;
            }

            return true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            popupEditProduct.Visible = false;
            btnEditProduct.BackColor = Color.Silver;
            btnDelete.BackColor = Color.Silver;

            ClearField();
        }

        private void btnConfirmEdit_Click(object sender, EventArgs e)
        {

            if (!ValidateEditProduct())
            {
                return;
            }

            try
            {

                using (OleDbConnection conn = Database.GetConnection())
                {

                    conn.Open();

                    string q = @"UPDATE Products SET ProductName=?, Category=?, Description=?, UnitPrice=?, Quantity=?, ReorderLevel=? WHERE ProductID=?";

                    using (OleDbCommand cmd = new OleDbCommand(q, conn))
                    {

                        cmd.Parameters.Add("?", OleDbType.VarWChar).Value = txtEditProductName.Text.Trim();
                        cmd.Parameters.Add("?", OleDbType.VarWChar).Value = txtEditCategory.Text;
                        cmd.Parameters.Add("?", OleDbType.LongVarWChar).Value = txtEditDescription.Text.Trim();
                        cmd.Parameters.Add("?", OleDbType.Currency).Value = decimal.Parse(txtEditUnitPrice.Text);
                        cmd.Parameters.Add("?", OleDbType.Integer).Value = int.Parse(txtEditQuantity.Text);
                        cmd.Parameters.Add("?", OleDbType.Integer).Value = int.Parse(txtEditReorderLevel.Text);
                        cmd.Parameters.Add("?", OleDbType.Integer).Value = selectedProductID;

                        cmd.ExecuteNonQuery();

                    }

                }

                MessageBox.Show("Product updated successfully!", "Product Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);

                popupEditProduct.Visible = false;
                btnEditProduct.BackColor = Color.Silver;
                btnDelete.BackColor = Color.Silver;

                ClearField();
                loadProducts();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating products: " + ex.Message, "Error updating products", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

            if(selectedProductID == 0)
            {

                MessageBox.Show("Please select a product row to delete.");
                return;

            }

            DialogResult result = MessageBox.Show("Are you sure you want to delete this product?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

            if(result == DialogResult.Yes)
            {

                try
                {

                    using (OleDbConnection conn = Database.GetConnection())
                    {

                        conn.Open();

                        string q = @"DELETE FROM Products WHERE ProductID=?";

                        using (OleDbCommand cmd = new OleDbCommand(q, conn))
                        {

                            cmd.Parameters.Add("?", OleDbType.Integer).Value = selectedProductID;

                            cmd.ExecuteNonQuery();

                        }

                    }

                    MessageBox.Show("Product deleted successfully!", "Product Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    popupEditProduct.Visible = false;
                    btnEditProduct.BackColor = Color.Silver;
                    btnDelete.BackColor = Color.Silver;

                    ClearField();
                    loadProducts();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting product: " + ex.Message, "Error deleting product", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }

        }

        private void ucProducts_Load_1(object sender, EventArgs e)
        {

        }
    }
}
