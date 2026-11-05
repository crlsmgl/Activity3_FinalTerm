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
    public partial class Register : Form
    {
        public Register()
        {
            InitializeComponent();

            txtPassword.UseSystemPasswordChar = true;
            txtConfirmPassword.UseSystemPasswordChar = true; 

            btnRegister.MakeRounded(8); 
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Register_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void roundPictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void roundPictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void roundedTextBox1_Load(object sender, EventArgs e)
        {

        }

        private void roundedTextBox2_Load(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void roundedTextBox1_Enter(object sender, EventArgs e)
        {

        }

        private void roundedTextBox1_Leave(object sender, EventArgs e)
        {

        }

        private void roundedTextBox1_Load_1(object sender, EventArgs e)
        {

        }

        private void roundedTextBox3_Load(object sender, EventArgs e)
        {

        }

        private void roundedTextBox4_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void flatComboBox1_Load(object sender, EventArgs e)
        {

        }

        private void roundedTextBox5_Load(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkLabel1.LinkVisited = true;

            Login loginForm = new Login();
            loginForm.Show();
            this.FormClosed -= Register_FormClosed;

            this.Close();
        }

        private void Register_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void btnRegister_Click(object sender, EventArgs e)
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

            if(password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match. Please try again.", "Registration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if(password.Length < 8)
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
                        if(userCount > 0)
                        {
                            MessageBox.Show("Username already exists. Please choose another username.", "Registration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        string q = @"INSERT INTO Users (Username, PasswordHash, FullName, UserRole, isActive) VALUES (?, ?, ?, ?, ?)";

                        using (OleDbCommand cmd = new OleDbCommand(q, con))
                        {
                            string passwordHash = PasswordHelper.HashPassword(password);
                            cmd.Parameters.AddWithValue("@Username", username);
                            cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);
                            cmd.Parameters.AddWithValue("@FullName", fullName);
                            cmd.Parameters.AddWithValue("@UserRole", "User");
                            cmd.Parameters.AddWithValue("@isActive", true);
                            int rowsAffected = cmd.ExecuteNonQuery();
                            if(rowsAffected > 0)
                            {
                                MessageBox.Show("User registered successfully.", "Registration Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                txtFullName.Clear();
                                txtUsername.Clear();
                                txtPassword.Clear();
                                txtConfirmPassword.Clear();

                                Login login = new Login();
                                login.Show();
                                this.FormClosed -= Register_FormClosed;
                                this.Close();
                            } else
                            {
                                MessageBox.Show("Failed to register user. Please try again.", "Registration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void label1_Click_1(object sender, EventArgs e)
        {

        }
    }
}
