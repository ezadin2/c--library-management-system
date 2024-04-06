using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;

namespace LibraryManagementSystem
{
    public partial class RegisterForm : Form
    {
        SqlConnection connect = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Users\ezadi\OneDrive\Desktop\New folder (9)\Library-Management-System-using-CSharp\LibraryManagementSystem\LibraryManagementSystem\library.mdf"";Integrated Security=True;Connect Timeout=30");
        public RegisterForm()
        {
            InitializeComponent();
        }

        private void signIn_btn_Click(object sender, EventArgs e)
        {
            LoginForm lForm = new LoginForm();
            lForm.Show();
            this.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void register_btn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(register_email.Text) || string.IsNullOrEmpty(register_username.Text) || string.IsNullOrEmpty(register_password.Text))
            {
                MessageBox.Show("Please fill all blank fields", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (SqlConnection connect = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Users\ezadi\OneDrive\Desktop\New folder (9)\Library-Management-System-using-CSharp\LibraryManagementSystem\LibraryManagementSystem\library.mdf"";Integrated Security=True;Connect Timeout=30"))
                {
                    connect.Open();

                    string checkUsernameQuery = "SELECT COUNT(*) FROM users WHERE username = @username";
                    using (SqlCommand checkUsernameCommand = new SqlCommand(checkUsernameQuery, connect))
                    {
                        checkUsernameCommand.Parameters.AddWithValue("@username", register_username.Text.Trim());
                        int count = (int)checkUsernameCommand.ExecuteScalar();

                        if (count >= 1)
                        {
                            MessageBox.Show(register_username.Text.Trim() + " is already taken", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    // Check for duplicate email here if needed

                    // TO GET THE DATE TODAY
                    DateTime today = DateTime.Today;

                    string insertDataQuery = "INSERT INTO users (email, username, password, date_register) " +
                        "VALUES (@email, @username, @password, @date_register)";
                    using (SqlCommand insertCommand = new SqlCommand(insertDataQuery, connect))
                    {
                        insertCommand.Parameters.AddWithValue("@email", register_email.Text.Trim());
                        insertCommand.Parameters.AddWithValue("@username", register_username.Text.Trim());
                        insertCommand.Parameters.AddWithValue("@password", register_password.Text.Trim());
                        insertCommand.Parameters.AddWithValue("@date_register", today);

                        insertCommand.ExecuteNonQuery();

                        MessageBox.Show("Register successfully!", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        LoginForm lForm = new LoginForm();
                        lForm.Show();
                        this.Hide();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error registering user: " + ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void register_showPass_CheckedChanged(object sender, EventArgs e)
        {
            register_password.PasswordChar = register_showPass.Checked ? '\0' : '*';
        }

        private void RegisterForm_Load(object sender, EventArgs e)
        {

        }
    }
}
