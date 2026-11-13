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
    public partial class ucUsers : UserControl
    {

        private int selectedUserID = 0;

        public ucUsers()
        {
            InitializeComponent();

            txtSearch.SetPlaceholder("Search User...", showWhenFocused: true);
            txtPassword.UseSystemPasswordChar = true;
            txtConfirmPassword.UseSystemPasswordChar = true;

            loadUsers();
        }

        private void loadUsers(string searchKeyword = "")
        {

            try
            {

                using (OleDbConnection conn = Database.GetConnection())
                {

                    conn.Open();

                    string q = @"SELECT UserID, Username, FullName, UserRole,
                                        IIF(IsActive = True, 'Active', 'Inactive') AS Status 
                                        FROM Users";

                    if(!string.IsNullOrWhiteSpace(searchKeyword))
                    {

                        q += " WHERE FullName LIKE ? OR Username LIKE ? OR UserRole LIKE ?";

                    }

                    q += " ORDER BY UserID ASC";

                    using (OleDbCommand cmd = new OleDbCommand(q, conn))
                    {

                        if(!string.IsNullOrWhiteSpace(searchKeyword))
                        {

                            string pattern = "%" + searchKeyword.Trim() + "%";
                            cmd.Parameters.Add("?", OleDbType.VarWChar).Value = pattern;
                            cmd.Parameters.Add("?", OleDbType.VarWChar).Value = pattern;
                            cmd.Parameters.Add("?", OleDbType.VarWChar).Value = pattern;

                        }

                        using (OleDbDataAdapter adapter = new OleDbDataAdapter(cmd))
                        {

                            DataTable dt = new DataTable();

                            adapter.Fill(dt);

                            dvgUsers.DataSource = dt;

                            int userCount = dvgUsers.Rows.Count;

                            label1.Text = $"Showing {userCount} {(userCount <= 1 ? "User" : "Users")}";

                        }

                    }

                }

            }
            catch (Exception ex)
            {

                MessageBox.Show("Error loading users: " + ex.Message, "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }

        private void ClearField()
        {
            txtFullName.Clear();
            txtUsername.Clear();
            txtPassword.Clear();
            txtConfirmPassword.Clear();

            selectedUserID = 0;

            dvgUsers.ClearSelection();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void ucUsers_Load(object sender, EventArgs e)
        {
            if(!this.DesignMode)
            {
                loadUsers();
            }
        }

        private void dvgUsers_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dvgUsers.Columns[e.ColumnIndex].Name == "Status" && e.Value != null)
            {

                string status = e.Value.ToString();
                e.CellStyle.Font = new Font(dvgUsers.Font, FontStyle.Bold);
                e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            
                if(status == "Active")
                {
                    e.CellStyle.ForeColor = Color.MediumSeaGreen;
                } else
                {
                    e.CellStyle.ForeColor = Color.Crimson;
                }

            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {

            if (txtSearch.Text == "Search User...")
            {
                loadUsers("");
                return;
            }

            loadUsers(txtSearch.Text);

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            
            if(selectedUserID == 0)
            {
                MessageBox.Show("Please select a product row to update");
                return;
            }

            popupAddUser.Visible = false;
            popupEditUser.Visible = true;

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            
            if(selectedUserID == 0)
            {

                MessageBox.Show("Please select a user row to delete.");
                return;

            }

            DialogResult result = MessageBox.Show("Are you sure you want to delete this user?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

            if(result == DialogResult.Yes)
            {

                try
                {

                    using (OleDbConnection conn = Database.GetConnection())
                    {

                        conn.Open();

                        string q = @"DELETE FROM Users WHERE UserID=?";

                        using (OleDbCommand cmd = new OleDbCommand(q, conn))
                        {

                            cmd.Parameters.Add("?", OleDbType.Integer).Value = selectedUserID;

                            cmd.ExecuteNonQuery();

                        }

                    }

                    MessageBox.Show("User deleted successfully!", "User Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    popupEditUser.Visible = false;
                    btnEdit.BackColor = Color.Silver;
                    btnDelete.BackColor = Color.Silver;

                    loadUsers();
                    ClearField();

                }
                catch (Exception ex)
                {

                    MessageBox.Show("Error deleting user: " + ex.Message, "Error deleting user", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }

            }

        }

        private void dvgUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            btnEdit.BackColor = Color.MediumSeaGreen;
            btnDelete.BackColor = Color.Maroon;

            if(e.RowIndex >= 0)
            {

                DataGridViewRow row = dvgUsers.Rows[e.RowIndex];

                selectedUserID = Convert.ToInt32(row.Cells["UserID"].Value);

                txtEditUsername.Text = row.Cells["Username"].Value.ToString();
                txtEditFullName.Text = row.Cells["FullName"].Value.ToString();
                txtEditStatus.Text = row.Cells["Status"].Value.ToString();

            }

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            popupAddUser.Visible = false;
            btnEdit.BackColor = Color.Silver;
            btnDelete.BackColor = Color.Silver;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            popupAddUser.Visible = true;
            popupEditUser.Visible = false;

        }

        private void btnConfirmAdd_Click(object sender, EventArgs e)
        {
            string fullName = txtFullName.Text.Trim();
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            if (fullName == "" || username == "" || password == "" || confirmPassword == "")
            {
                MessageBox.Show("Please fill all the fields.", "Registration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match. Please try again.", "Registration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (password.Length < 8)
            {
                MessageBox.Show("Password must be at least 8 characters long.", "Registration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (OleDbConnection con = Database.GetConnection())
                {
                    con.Open();

                    string checkQuery = "SELECT COUNT(*) FROM Users WHERE Username = ?";

                    using (OleDbCommand checkCmd = new OleDbCommand(checkQuery, con))
                    {
                        checkCmd.Parameters.AddWithValue("@Username", username);
                        int userCount = (int)checkCmd.ExecuteScalar();
                        if (userCount > 0)
                        {
                            MessageBox.Show("Username already exists. Please choose another username.", "Registration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        string q = @"INSERT INTO Users (Username, PasswordHash, FullName, UserRole, IsActive) VALUES (?, ?, ?, ?, ?)";

                        using (OleDbCommand cmd = new OleDbCommand(q, con))
                        {
                            string passwordHash = PasswordHelper.HashPassword(password);
                            cmd.Parameters.AddWithValue("@Username", username);
                            cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);
                            cmd.Parameters.AddWithValue("@FullName", fullName);
                            cmd.Parameters.AddWithValue("@UserRole", "User");
                            cmd.Parameters.AddWithValue("@isActive", true);
                            int rowsAffected = cmd.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("User added successfully.", "Adding User Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                popupAddUser.Visible = false;
                                btnEdit.BackColor = Color.Silver;
                                btnDelete.BackColor = Color.Silver;
                                ClearField();
                                loadUsers();
                            }
                            else
                            {
                                MessageBox.Show("Failed to add user. Please try again.", "Adding User Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while registering the user: " + ex.Message, "Registration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            popupEditUser.Visible = false;
            btnEdit.BackColor = Color.Silver;
            btnDelete.BackColor = Color.Silver;

            ClearField();
        }

        private bool ValidateEditUser()
        {
            if (string.IsNullOrWhiteSpace(txtEditUsername.Text))
            {
                MessageBox.Show("Please enter a username.");
                txtEditUsername.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtEditFullName.Text))
            {
                MessageBox.Show("Please enter fullname.");
                txtEditFullName.Focus();
                return false;
            }

            if (txtEditStatus.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a status.");
                txtEditStatus.Focus();
                return false;
            }

            return true;

        }

        private void btnConfirmEdit_Click(object sender, EventArgs e)
        {

            if(!ValidateEditUser())
            {
                return;
            }

            try
            {

                using (OleDbConnection conn = Database.GetConnection())
                {

                    conn.Open();

                    string q = @"UPDATE Users SET Username=?, FullName=?, IsActive=? WHERE UserID=?";

                    using (OleDbCommand cmd = new OleDbCommand(q, conn))
                    {

                        bool isActive = txtEditStatus.Text.Trim().Equals("Active", StringComparison.OrdinalIgnoreCase);

                        cmd.Parameters.Add("?", OleDbType.VarWChar).Value = txtEditUsername.Text.Trim();
                        cmd.Parameters.Add("?", OleDbType.VarWChar).Value = txtEditFullName.Text.Trim();
                        cmd.Parameters.Add("?", OleDbType.Boolean).Value = isActive;
                        cmd.Parameters.Add("?", OleDbType.Integer).Value = selectedUserID;

                        cmd.ExecuteNonQuery();
                    }

                }

                MessageBox.Show("User updated successfully!", "User Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);

                popupEditUser.Visible = false;
                btnEdit.BackColor = Color.Silver;
                btnDelete.BackColor = Color.Silver;

                ClearField();
                loadUsers();

            }
            catch (Exception ex)
            {

                MessageBox.Show("Error updating user: " + ex.Message, "Error updating user", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }
    }
}
