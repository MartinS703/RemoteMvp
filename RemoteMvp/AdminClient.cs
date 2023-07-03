using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RemoteMvpClient
{
    public partial class AdminClient : Form
    {
        public event EventHandler ShowUserListRequested;
        public event EventHandler<string> DeleteUserRequested;
        public event EventHandler LogoutRequested;
        private List<string> _users;
        public AdminClient()
        {
            InitializeComponent();
            _users = new List<string>();
        }

        public void ShowUsers(List<string> users)
        {
            _users = users;
            listViewRegisteredPersons.Items.Clear();
            foreach (var user in _users)
            {
                ListViewItem livItem = new ListViewItem(user);
                listViewRegisteredPersons.Items.Add(livItem);
            }
        }
        private void btnShow_Click(object sender, EventArgs e)
        {
            ShowUserListRequested?.Invoke(this, EventArgs.Empty);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

            
            string userToDelete = Interaction.InputBox("Please input username", Title: "User deletion");
            if (_users.Contains(userToDelete))
            {
                if (string.IsNullOrEmpty(userToDelete) == false)
                {
                    DeleteUserRequested?.Invoke(this, userToDelete);
                }
            }
            else
            {
                MessageBox.Show("User not found: "+userToDelete, "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            LogoutRequested?.Invoke(this, EventArgs.Empty);
        }

        private void AdminClient_Load(object sender, EventArgs e)
        {

        }

        private void AdminClient_Load_1(object sender, EventArgs e)
        {

        }

    }
}
