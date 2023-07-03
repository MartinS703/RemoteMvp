namespace RemoteMvpClient
{
    partial class AdminClient
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            listViewRegisteredPersons = new ListView();
            lblInfo = new Label();
            btnShow = new Button();
            btnDelete = new Button();
            btnLogout = new Button();
            SuspendLayout();
            // 
            // listViewRegisteredPersons
            // 
            listViewRegisteredPersons.Location = new Point(12, 76);
            listViewRegisteredPersons.Name = "listViewRegisteredPersons";
            listViewRegisteredPersons.Size = new Size(449, 280);
            listViewRegisteredPersons.TabIndex = 0;
            listViewRegisteredPersons.UseCompatibleStateImageBehavior = false;
            // 
            // lblInfo
            // 
            lblInfo.AutoSize = true;
            lblInfo.Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point);
            lblInfo.Location = new Point(12, 28);
            lblInfo.Name = "lblInfo";
            lblInfo.Size = new Size(235, 31);
            lblInfo.TabIndex = 1;
            lblInfo.Text = "All registered Persons";
            // 
            // btnShow
            // 
            btnShow.Location = new Point(579, 76);
            btnShow.Name = "btnShow";
            btnShow.Size = new Size(94, 29);
            btnShow.TabIndex = 2;
            btnShow.Text = "Show";
            btnShow.UseVisualStyleBackColor = true;
            btnShow.Click += btnShow_Click;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(579, 131);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(94, 29);
            btnDelete.TabIndex = 3;
            btnDelete.Text = "Delete";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // btnLogout
            // 
            btnLogout.Location = new Point(695, 410);
            btnLogout.Name = "btnLogout";
            btnLogout.Size = new Size(94, 29);
            btnLogout.TabIndex = 4;
            btnLogout.Text = "Logout";
            btnLogout.UseVisualStyleBackColor = true;
            btnLogout.Click += btnLogout_Click;
            // 
            // AdminClient
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnLogout);
            Controls.Add(btnDelete);
            Controls.Add(btnShow);
            Controls.Add(lblInfo);
            Controls.Add(listViewRegisteredPersons);
            Name = "AdminClient";
            Text = "AdminClient";
            Load += AdminClient_Load_1;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListView listViewRegisteredPersons;
        private Label lblInfo;
        private Button btnShow;
        private Button btnDelete;
        private Button btnLogout;
    }
}