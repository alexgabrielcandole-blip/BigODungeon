using System.Drawing;
using System.Windows.Forms;

namespace BigODungeon
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            tlpRoot = new TableLayoutPanel();
            pnlHeader = new Panel();
            lblFloor = new Label();
            lblTitle = new Label();
            rtbStory = new RichTextBox();
            tlpSide = new TableLayoutPanel();
            lblBigOCaption = new Label();
            lblBigO = new Label();
            lblOpsCaption = new Label();
            lblOps = new Label();
            lblOptimalCaption = new Label();
            lblOptimal = new Label();
            lblStaminaCaption = new Label();
            prgStamina = new ProgressBar();
            lblTask = new Label();
            tlpInput = new TableLayoutPanel();
            lblPrompt = new Label();
            txtInput = new TextBox();
            btnSubmit = new Button();
            btnNext = new Button();
            tlpRoot.SuspendLayout();
            pnlHeader.SuspendLayout();
            tlpSide.SuspendLayout();
            tlpInput.SuspendLayout();
            SuspendLayout();

            // tlpRoot: 2 columns x 4 rows (header / story+side / task bar / input row)
            tlpRoot.BackColor = Color.FromArgb(24, 24, 32);
            tlpRoot.ColumnCount = 2;
            tlpRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 300F));
            tlpRoot.Controls.Add(pnlHeader, 0, 0);
            tlpRoot.Controls.Add(rtbStory, 0, 1);
            tlpRoot.Controls.Add(tlpSide, 1, 1);
            tlpRoot.Controls.Add(lblTask, 0, 2);
            tlpRoot.Controls.Add(tlpInput, 0, 3);
            tlpRoot.Dock = DockStyle.Fill;
            tlpRoot.Name = "tlpRoot";
            tlpRoot.Padding = new Padding(12);
            tlpRoot.RowCount = 4;
            tlpRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            tlpRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tlpRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
            tlpRoot.SetColumnSpan(pnlHeader, 2);
            tlpRoot.SetColumnSpan(lblTask, 2);
            tlpRoot.SetColumnSpan(tlpInput, 2);
            tlpRoot.TabIndex = 0;

            // pnlHeader: title (left) + floor/room info (right)
            pnlHeader.Controls.Add(lblFloor);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Dock = DockStyle.Fill;
            pnlHeader.Name = "pnlHeader";

            lblTitle.AutoSize = true;
            lblTitle.Dock = DockStyle.Left;
            lblTitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTitle.ForeColor = Color.Gold;
            lblTitle.Name = "lblTitle";
            lblTitle.Text = "DUNGEON DESCENT";
            lblTitle.TextAlign = ContentAlignment.MiddleLeft;

            lblFloor.Dock = DockStyle.Fill;
            lblFloor.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblFloor.ForeColor = Color.Gainsboro;
            lblFloor.Name = "lblFloor";
            lblFloor.Text = "Floor 1 of 5";
            lblFloor.TextAlign = ContentAlignment.MiddleRight;

            // rtbStory: main text area
            rtbStory.BackColor = Color.FromArgb(14, 14, 20);
            rtbStory.BorderStyle = BorderStyle.None;
            rtbStory.Dock = DockStyle.Fill;
            rtbStory.Font = new Font("Consolas", 11.5F);
            rtbStory.ForeColor = Color.Gainsboro;
            rtbStory.Margin = new Padding(0, 0, 12, 0);
            rtbStory.Name = "rtbStory";
            rtbStory.ReadOnly = true;
            rtbStory.TabStop = false;

            // tlpSide: Big-O label, step counter, best possible steps, stamina
            tlpSide.BackColor = Color.FromArgb(34, 34, 48);
            tlpSide.ColumnCount = 1;
            tlpSide.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpSide.Controls.Add(lblBigOCaption, 0, 0);
            tlpSide.Controls.Add(lblBigO, 0, 1);
            tlpSide.Controls.Add(lblOpsCaption, 0, 2);
            tlpSide.Controls.Add(lblOps, 0, 3);
            tlpSide.Controls.Add(lblOptimalCaption, 0, 4);
            tlpSide.Controls.Add(lblOptimal, 0, 5);
            tlpSide.Controls.Add(lblStaminaCaption, 0, 6);
            tlpSide.Controls.Add(prgStamina, 0, 7);
            tlpSide.Dock = DockStyle.Fill;
            tlpSide.Name = "tlpSide";
            tlpSide.Padding = new Padding(14);
            tlpSide.RowCount = 9;
            for (int i = 0; i < 8; i++)
                tlpSide.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpSide.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            lblBigOCaption.AutoSize = true;
            lblBigOCaption.Dock = DockStyle.Fill;
            lblBigOCaption.Font = new Font("Segoe UI", 9F);
            lblBigOCaption.ForeColor = Color.Silver;
            lblBigOCaption.Name = "lblBigOCaption";
            lblBigOCaption.Text = "CATEGORY";

            lblBigO.AutoSize = true;
            lblBigO.Dock = DockStyle.Fill;
            lblBigO.Font = new Font("Segoe UI", 26F, FontStyle.Bold);
            lblBigO.ForeColor = Color.Gold;
            lblBigO.Name = "lblBigO";
            lblBigO.Text = "O(1)";

            lblOpsCaption.AutoSize = true;
            lblOpsCaption.Dock = DockStyle.Fill;
            lblOpsCaption.Font = new Font("Segoe UI", 9F);
            lblOpsCaption.ForeColor = Color.Silver;
            lblOpsCaption.Margin = new Padding(3, 12, 3, 0);
            lblOpsCaption.Name = "lblOpsCaption";
            lblOpsCaption.Text = "STEPS (WORK DONE)";

            lblOps.AutoSize = true;
            lblOps.Dock = DockStyle.Fill;
            lblOps.Font = new Font("Segoe UI", 32F, FontStyle.Bold);
            lblOps.ForeColor = Color.LimeGreen;
            lblOps.Name = "lblOps";
            lblOps.Text = "0";

            lblOptimalCaption.AutoSize = true;
            lblOptimalCaption.Dock = DockStyle.Fill;
            lblOptimalCaption.Font = new Font("Segoe UI", 9F);
            lblOptimalCaption.ForeColor = Color.Silver;
            lblOptimalCaption.Margin = new Padding(3, 12, 3, 0);
            lblOptimalCaption.Name = "lblOptimalCaption";
            lblOptimalCaption.Text = "BEST POSSIBLE STEPS";

            lblOptimal.AutoSize = true;
            lblOptimal.Dock = DockStyle.Fill;
            lblOptimal.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblOptimal.ForeColor = Color.DeepSkyBlue;
            lblOptimal.Name = "lblOptimal";
            lblOptimal.Text = "-";

            lblStaminaCaption.AutoSize = true;
            lblStaminaCaption.Dock = DockStyle.Fill;
            lblStaminaCaption.Font = new Font("Segoe UI", 9F);
            lblStaminaCaption.ForeColor = Color.Silver;
            lblStaminaCaption.Margin = new Padding(3, 12, 3, 0);
            lblStaminaCaption.Name = "lblStaminaCaption";
            lblStaminaCaption.Text = "STAMINA";

            prgStamina.Dock = DockStyle.Fill;
            prgStamina.Margin = new Padding(3, 4, 3, 3);
            prgStamina.Maximum = 100;
            prgStamina.MinimumSize = new Size(0, 22);
            prgStamina.Name = "prgStamina";
            prgStamina.Style = ProgressBarStyle.Continuous;
            prgStamina.Value = 100;

            // lblTask: the yellow "what to type now" bar (right above the input row)
            lblTask.BackColor = Color.FromArgb(40, 52, 80);
            lblTask.Dock = DockStyle.Fill;
            lblTask.Font = new Font("Segoe UI", 11.5F, FontStyle.Bold);
            lblTask.ForeColor = Color.Gold;
            lblTask.Margin = new Padding(0, 8, 0, 0);
            lblTask.Name = "lblTask";
            lblTask.Padding = new Padding(12, 0, 0, 0);
            lblTask.Text = "TASK:  Read the story above.";
            lblTask.TextAlign = ContentAlignment.MiddleLeft;

            // tlpInput: [Type here:] [textbox.........] [Submit] [Next Floor]
            tlpInput.ColumnCount = 4;
            tlpInput.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90F));
            tlpInput.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpInput.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
            tlpInput.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            tlpInput.Controls.Add(lblPrompt, 0, 0);
            tlpInput.Controls.Add(txtInput, 1, 0);
            tlpInput.Controls.Add(btnSubmit, 2, 0);
            tlpInput.Controls.Add(btnNext, 3, 0);
            tlpInput.Dock = DockStyle.Fill;
            tlpInput.Margin = new Padding(0, 6, 0, 0);
            tlpInput.Name = "tlpInput";
            tlpInput.RowCount = 1;
            tlpInput.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            lblPrompt.Dock = DockStyle.Fill;
            lblPrompt.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblPrompt.ForeColor = Color.Gainsboro;
            lblPrompt.Name = "lblPrompt";
            lblPrompt.Text = "Type here:";
            lblPrompt.TextAlign = ContentAlignment.MiddleLeft;

            txtInput.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtInput.Font = new Font("Segoe UI", 12F);
            txtInput.Margin = new Padding(0, 0, 8, 0);
            txtInput.Name = "txtInput";
            txtInput.TabIndex = 1;

            btnSubmit.BackColor = Color.FromArgb(52, 120, 200);
            btnSubmit.Dock = DockStyle.Fill;
            btnSubmit.FlatStyle = FlatStyle.Flat;
            btnSubmit.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSubmit.ForeColor = Color.White;
            btnSubmit.Margin = new Padding(0, 6, 8, 6);
            btnSubmit.Name = "btnSubmit";
            btnSubmit.TabIndex = 2;
            btnSubmit.Text = "Submit";
            btnSubmit.UseVisualStyleBackColor = false;
            btnSubmit.Click += btnSubmit_Click;

            btnNext.BackColor = Color.FromArgb(60, 140, 80);
            btnNext.Dock = DockStyle.Fill;
            btnNext.FlatStyle = FlatStyle.Flat;
            btnNext.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnNext.ForeColor = Color.White;
            btnNext.Margin = new Padding(0, 6, 0, 6);
            btnNext.Name = "btnNext";
            btnNext.TabIndex = 3;
            btnNext.Text = "Next Floor >";
            btnNext.UseVisualStyleBackColor = false;
            btnNext.Click += btnNext_Click;

            // Form1
            AcceptButton = btnSubmit;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1000, 660);
            Controls.Add(tlpRoot);
            MinimumSize = new Size(900, 600);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Dungeon Descent - Big-O Game";
            Load += Form1_Load;

            tlpInput.ResumeLayout(false);
            tlpInput.PerformLayout();
            tlpSide.ResumeLayout(false);
            tlpSide.PerformLayout();
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            tlpRoot.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tlpRoot;
        private Panel pnlHeader;
        private Label lblTitle;
        private Label lblFloor;
        private RichTextBox rtbStory;
        private TableLayoutPanel tlpSide;
        private Label lblBigOCaption;
        private Label lblBigO;
        private Label lblOpsCaption;
        private Label lblOps;
        private Label lblOptimalCaption;
        private Label lblOptimal;
        private Label lblStaminaCaption;
        private ProgressBar prgStamina;
        private Label lblTask;
        private TableLayoutPanel tlpInput;
        private Label lblPrompt;
        private TextBox txtInput;
        private Button btnSubmit;
        private Button btnNext;
    }
}