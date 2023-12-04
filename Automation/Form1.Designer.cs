namespace Automation
{
    partial class Form1
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
            navigationBar = new TextBox();
            btnNavigateTo = new Button();
            tabControl1 = new TabControl();
            newTabBtn = new Button();
            btnCloseTab = new Button();
            btnExecuteScript = new Button();
            txtBoxPortNumber = new TextBox();
            btnCreateUserTab = new Button();
            SuspendLayout();
            // 
            // navigationBar
            // 
            navigationBar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            navigationBar.Location = new Point(12, 70);
            navigationBar.Name = "navigationBar";
            navigationBar.Size = new Size(939, 23);
            navigationBar.TabIndex = 1;
            navigationBar.KeyDown += navigationBar_KeyDown;
            navigationBar.KeyUp += textBox1_KeyUp;
            // 
            // btnNavigateTo
            // 
            btnNavigateTo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnNavigateTo.Location = new Point(964, 70);
            btnNavigateTo.Name = "btnNavigateTo";
            btnNavigateTo.Size = new Size(288, 29);
            btnNavigateTo.TabIndex = 2;
            btnNavigateTo.Text = "Go";
            btnNavigateTo.UseVisualStyleBackColor = true;
            btnNavigateTo.Click += Button1_Click;
            // 
            // tabControl1
            // 
            tabControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabControl1.Location = new Point(12, 105);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1240, 555);
            tabControl1.TabIndex = 4;
            tabControl1.Selecting += tabControl1_Selecting;
            tabControl1.Selected += tabControl1_Selected;
            // 
            // newTabBtn
            // 
            newTabBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            newTabBtn.Location = new Point(1146, 6);
            newTabBtn.Name = "newTabBtn";
            newTabBtn.Size = new Size(106, 23);
            newTabBtn.TabIndex = 5;
            newTabBtn.Text = "New Tab";
            newTabBtn.UseVisualStyleBackColor = true;
            newTabBtn.Click += newTabBtn_Click;
            // 
            // btnCloseTab
            // 
            btnCloseTab.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCloseTab.Location = new Point(845, 12);
            btnCloseTab.Name = "btnCloseTab";
            btnCloseTab.Size = new Size(106, 52);
            btnCloseTab.TabIndex = 6;
            btnCloseTab.Text = "Close Tab";
            btnCloseTab.UseVisualStyleBackColor = true;
            btnCloseTab.Click += btnCloseTab_Click;
            // 
            // btnExecuteScript
            // 
            btnExecuteScript.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnExecuteScript.Location = new Point(964, 35);
            btnExecuteScript.Name = "btnExecuteScript";
            btnExecuteScript.Size = new Size(288, 29);
            btnExecuteScript.TabIndex = 7;
            btnExecuteScript.Text = "Script";
            btnExecuteScript.UseVisualStyleBackColor = true;
            btnExecuteScript.Click += btnExecuteScript_Click;
            // 
            // txtBoxPortNumber
            // 
            txtBoxPortNumber.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            txtBoxPortNumber.Location = new Point(964, 6);
            txtBoxPortNumber.Name = "txtBoxPortNumber";
            txtBoxPortNumber.Size = new Size(169, 23);
            txtBoxPortNumber.TabIndex = 8;
            // 
            // btnCreateUserTab
            // 
            btnCreateUserTab.Location = new Point(12, 12);
            btnCreateUserTab.Name = "btnCreateUserTab";
            btnCreateUserTab.Size = new Size(96, 52);
            btnCreateUserTab.TabIndex = 9;
            btnCreateUserTab.Text = "New User Tab";
            btnCreateUserTab.UseVisualStyleBackColor = true;
            btnCreateUserTab.Click += btnCreateUserTab_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlDark;
            ClientSize = new Size(1264, 681);
            Controls.Add(btnCreateUserTab);
            Controls.Add(txtBoxPortNumber);
            Controls.Add(btnExecuteScript);
            Controls.Add(btnCloseTab);
            Controls.Add(newTabBtn);
            Controls.Add(tabControl1);
            Controls.Add(btnNavigateTo);
            Controls.Add(navigationBar);
            MinimumSize = new Size(1280, 720);
            Name = "Form1";
            Text = "Juan's Browser";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private TextBox navigationBar;
        private Button btnNavigateTo;
        private TabControl tabControl1;
        private Button newTabBtn;
        private Button btnCloseTab;
        private Button btnExecuteScript;
        private TextBox txtBoxPortNumber;
        private Button btnCreateUserTab;
    }
}