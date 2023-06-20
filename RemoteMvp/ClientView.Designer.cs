namespace RemoteMvpClient
{
    partial class ClientView
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            button1 = new Button();
            button2 = new Button();
            tbUsername = new TextBox();
            tbPassword = new TextBox();
            label1 = new Label();
            label2 = new Label();
            btnRegisterAdmin = new Button();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Enabled = false;
            button1.Location = new Point(300, 181);
            button1.Margin = new Padding(3, 4, 3, 4);
            button1.Name = "button1";
            button1.Size = new Size(133, 37);
            button1.TabIndex = 0;
            button1.Text = "Login";
            button1.UseVisualStyleBackColor = true;
            button1.Click += login_Click;
            // 
            // button2
            // 
            button2.Enabled = false;
            button2.Location = new Point(14, 156);
            button2.Margin = new Padding(3, 4, 3, 4);
            button2.Name = "button2";
            button2.Size = new Size(130, 36);
            button2.TabIndex = 1;
            button2.Text = "Register";
            button2.UseVisualStyleBackColor = true;
            button2.Click += register_Click;
            // 
            // tbUsername
            // 
            tbUsername.Location = new Point(14, 40);
            tbUsername.Margin = new Padding(3, 4, 3, 4);
            tbUsername.Name = "tbUsername";
            tbUsername.Size = new Size(294, 27);
            tbUsername.TabIndex = 2;
            tbUsername.TextChanged += TbUsernameTextChanged;
            // 
            // tbPassword
            // 
            tbPassword.Location = new Point(14, 111);
            tbPassword.Margin = new Padding(3, 4, 3, 4);
            tbPassword.Name = "tbPassword";
            tbPassword.Size = new Size(294, 27);
            tbPassword.TabIndex = 3;
            tbPassword.TextChanged += TbPasswordTextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(14, 12);
            label1.Name = "label1";
            label1.Size = new Size(82, 20);
            label1.TabIndex = 4;
            label1.Text = "User Name";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(14, 83);
            label2.Name = "label2";
            label2.Size = new Size(70, 20);
            label2.TabIndex = 4;
            label2.Text = "Password";
            // 
            // btnRegisterAdmin
            // 
            btnRegisterAdmin.Location = new Point(14, 199);
            btnRegisterAdmin.Name = "btnRegisterAdmin";
            btnRegisterAdmin.Size = new Size(130, 29);
            btnRegisterAdmin.TabIndex = 5;
            btnRegisterAdmin.Text = "Register admin";
            btnRegisterAdmin.UseVisualStyleBackColor = true;
            btnRegisterAdmin.Click += btnRegisterAdmin_Click;
            // 
            // ClientView
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(465, 264);
            Controls.Add(btnRegisterAdmin);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(tbPassword);
            Controls.Add(tbUsername);
            Controls.Add(button2);
            Controls.Add(button1);
            Margin = new Padding(3, 4, 3, 4);
            Name = "ClientView";
            Text = "Login";
            Load += ClientView_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Button button2;
        private TextBox tbUsername;
        private TextBox tbPassword;
        private Label label1;
        private Label label2;
        private Button btnRegisterAdmin;
    }
}