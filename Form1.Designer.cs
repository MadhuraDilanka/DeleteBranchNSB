using System.Drawing;
using System.Windows.Forms;

namespace DeleteBranchNSB;

partial class Form1
{
    private System.ComponentModel.IContainer components = null;

    // ── Connection ────────────────────────────────────────────────────────────
    private GroupBox       grpConnection       = null;
    private Label          lblConnStr          = null;
    private TextBox        txtConnectionString  = null;
    private Button         btnConnect          = null;

    // ── Selection ─────────────────────────────────────────────────────────────
    private GroupBox       grpSelection        = null;
    private Label          lblBranch           = null;
    private ComboBox       cboBranch           = null;
    private Label          lblLibraries        = null;
    private CheckedListBox clbLibraries        = null;
    private Button         btnSelectAll        = null;
    private Button         btnClearAll         = null;

    // ── Action ────────────────────────────────────────────────────────────────
    private Button         btnDelete           = null;

    // ── Progress ──────────────────────────────────────────────────────────────
    private ProgressBar    pbProgress          = null;

    // ── Log ───────────────────────────────────────────────────────────────────
    private GroupBox       grpLog              = null;
    private RichTextBox    rtbLog              = null;
    private Button         btnClearLog         = null;

    // ── Status ────────────────────────────────────────────────────────────────
    private StatusStrip          statusStrip   = null;
    private ToolStripStatusLabel tsslStatus    = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
            components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        grpConnection = new GroupBox();
        lblConnStr = new Label();
        txtConnectionString = new TextBox();
        btnConnect = new Button();
        grpSelection = new GroupBox();
        lblBranch = new Label();
        cboBranch = new ComboBox();
        lblLibraries = new Label();
        clbLibraries = new CheckedListBox();
        btnSelectAll = new Button();
        btnClearAll = new Button();
        btnDelete = new Button();
        pbProgress = new ProgressBar();
        grpLog = new GroupBox();
        rtbLog = new RichTextBox();
        btnClearLog = new Button();
        tsslStatus = new ToolStripStatusLabel();
        statusStrip = new StatusStrip();
        grpConnection.SuspendLayout();
        grpSelection.SuspendLayout();
        grpLog.SuspendLayout();
        statusStrip.SuspendLayout();
        SuspendLayout();
        // 
        // grpConnection
        // 
        grpConnection.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        grpConnection.Controls.Add(lblConnStr);
        grpConnection.Controls.Add(txtConnectionString);
        grpConnection.Controls.Add(btnConnect);
        grpConnection.Location = new Point(11, 13);
        grpConnection.Margin = new Padding(3, 4, 3, 4);
        grpConnection.Name = "grpConnection";
        grpConnection.Padding = new Padding(3, 4, 3, 4);
        grpConnection.Size = new Size(1079, 96);
        grpConnection.TabIndex = 0;
        grpConnection.TabStop = false;
        grpConnection.Text = "SQL Server Connection";
        // 
        // lblConnStr
        // 
        lblConnStr.Location = new Point(9, 40);
        lblConnStr.Name = "lblConnStr";
        lblConnStr.Size = new Size(135, 31);
        lblConnStr.TabIndex = 0;
        lblConnStr.Text = "Connection String:";
        lblConnStr.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtConnectionString
        // 
        txtConnectionString.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtConnectionString.Location = new Point(149, 37);
        txtConnectionString.Margin = new Padding(3, 4, 3, 4);
        txtConnectionString.Name = "txtConnectionString";
        txtConnectionString.PlaceholderText = "Server=.;Database=YourDB;Trusted_Connection=True;TrustServerCertificate=True;";
        txtConnectionString.Size = new Size(779, 27);
        txtConnectionString.TabIndex = 1;
        txtConnectionString.Text = "Server=4.194.128.51,1433;Database=Enadoc12688B9B38A4000;User Id=sa;Password=Alctraz56#$;TrustServerCertificate=True;\r\n";
        // 
        // btnConnect
        // 
        btnConnect.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnConnect.BackColor = Color.SteelBlue;
        btnConnect.Cursor = Cursors.Hand;
        btnConnect.FlatStyle = FlatStyle.Flat;
        btnConnect.ForeColor = Color.White;
        btnConnect.Location = new Point(939, 36);
        btnConnect.Margin = new Padding(3, 4, 3, 4);
        btnConnect.Name = "btnConnect";
        btnConnect.Size = new Size(126, 35);
        btnConnect.TabIndex = 2;
        btnConnect.Text = "Connect";
        btnConnect.UseVisualStyleBackColor = false;
        btnConnect.Click += btnConnect_Click;
        // 
        // grpSelection
        // 
        grpSelection.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        grpSelection.Controls.Add(lblBranch);
        grpSelection.Controls.Add(cboBranch);
        grpSelection.Controls.Add(lblLibraries);
        grpSelection.Controls.Add(clbLibraries);
        grpSelection.Controls.Add(btnSelectAll);
        grpSelection.Controls.Add(btnClearAll);
        grpSelection.Location = new Point(11, 123);
        grpSelection.Margin = new Padding(3, 4, 3, 4);
        grpSelection.Name = "grpSelection";
        grpSelection.Padding = new Padding(3, 4, 3, 4);
        grpSelection.Size = new Size(1079, 407);
        grpSelection.TabIndex = 1;
        grpSelection.TabStop = false;
        grpSelection.Text = "Select Branch && Libraries";
        // 
        // lblBranch
        // 
        lblBranch.Location = new Point(9, 40);
        lblBranch.Name = "lblBranch";
        lblBranch.Size = new Size(80, 31);
        lblBranch.TabIndex = 0;
        lblBranch.Text = "Branch:";
        lblBranch.TextAlign = ContentAlignment.MiddleRight;
        // 
        // cboBranch
        // 
        cboBranch.DropDownStyle = ComboBoxStyle.DropDownList;
        cboBranch.Location = new Point(94, 37);
        cboBranch.Margin = new Padding(3, 4, 3, 4);
        cboBranch.Name = "cboBranch";
        cboBranch.Size = new Size(457, 28);
        cboBranch.TabIndex = 1;
        // 
        // lblLibraries
        // 
        lblLibraries.Location = new Point(9, 88);
        lblLibraries.Name = "lblLibraries";
        lblLibraries.Size = new Size(80, 31);
        lblLibraries.TabIndex = 2;
        lblLibraries.Text = "Libraries:";
        lblLibraries.TextAlign = ContentAlignment.MiddleRight;
        // 
        // clbLibraries
        // 
        clbLibraries.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        clbLibraries.CheckOnClick = true;
        clbLibraries.Location = new Point(94, 85);
        clbLibraries.Margin = new Padding(3, 4, 3, 4);
        clbLibraries.Name = "clbLibraries";
        clbLibraries.Size = new Size(973, 246);
        clbLibraries.TabIndex = 3;
        // 
        // btnSelectAll
        // 
        btnSelectAll.Cursor = Cursors.Hand;
        btnSelectAll.Location = new Point(94, 363);
        btnSelectAll.Margin = new Padding(3, 4, 3, 4);
        btnSelectAll.Name = "btnSelectAll";
        btnSelectAll.Size = new Size(103, 35);
        btnSelectAll.TabIndex = 4;
        btnSelectAll.Text = "Select All";
        btnSelectAll.Click += btnSelectAll_Click;
        // 
        // btnClearAll
        // 
        btnClearAll.Cursor = Cursors.Hand;
        btnClearAll.Location = new Point(208, 363);
        btnClearAll.Margin = new Padding(3, 4, 3, 4);
        btnClearAll.Name = "btnClearAll";
        btnClearAll.Size = new Size(103, 35);
        btnClearAll.TabIndex = 5;
        btnClearAll.Text = "Clear All";
        btnClearAll.Click += btnClearAll_Click;
        // 
        // btnDelete
        // 
        btnDelete.BackColor = Color.Firebrick;
        btnDelete.Cursor = Cursors.Hand;
        btnDelete.FlatStyle = FlatStyle.Flat;
        btnDelete.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        btnDelete.ForeColor = Color.White;
        btnDelete.Location = new Point(431, 547);
        btnDelete.Margin = new Padding(3, 4, 3, 4);
        btnDelete.Name = "btnDelete";
        btnDelete.Size = new Size(251, 53);
        btnDelete.TabIndex = 2;
        btnDelete.Text = "Delete Documents";
        btnDelete.UseVisualStyleBackColor = false;
        btnDelete.Click += btnDelete_Click;
        // 
        // pbProgress
        // 
        pbProgress.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        pbProgress.Location = new Point(11, 616);
        pbProgress.Margin = new Padding(3, 4, 3, 4);
        pbProgress.Name = "pbProgress";
        pbProgress.Size = new Size(1079, 27);
        pbProgress.TabIndex = 3;
        // 
        // grpLog
        // 
        grpLog.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        grpLog.Controls.Add(rtbLog);
        grpLog.Controls.Add(btnClearLog);
        grpLog.Location = new Point(11, 656);
        grpLog.Margin = new Padding(3, 4, 3, 4);
        grpLog.Name = "grpLog";
        grpLog.Padding = new Padding(3, 4, 3, 4);
        grpLog.Size = new Size(1079, 357);
        grpLog.TabIndex = 4;
        grpLog.TabStop = false;
        grpLog.Text = "Operation Log";
        // 
        // rtbLog
        // 
        rtbLog.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        rtbLog.BackColor = Color.FromArgb(20, 20, 20);
        rtbLog.BorderStyle = BorderStyle.None;
        rtbLog.Font = new Font("Consolas", 9F);
        rtbLog.ForeColor = Color.LightGreen;
        rtbLog.Location = new Point(7, 29);
        rtbLog.Margin = new Padding(3, 4, 3, 4);
        rtbLog.Name = "rtbLog";
        rtbLog.ReadOnly = true;
        rtbLog.ScrollBars = RichTextBoxScrollBars.Vertical;
        rtbLog.Size = new Size(1061, 280);
        rtbLog.TabIndex = 0;
        rtbLog.Text = "";
        // 
        // btnClearLog
        // 
        btnClearLog.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnClearLog.Cursor = Cursors.Hand;
        btnClearLog.Location = new Point(7, 317);
        btnClearLog.Margin = new Padding(3, 4, 3, 4);
        btnClearLog.Name = "btnClearLog";
        btnClearLog.Size = new Size(103, 32);
        btnClearLog.TabIndex = 1;
        btnClearLog.Text = "Clear Log";
        btnClearLog.Click += btnClearLog_Click;
        // 
        // tsslStatus
        // 
        tsslStatus.Name = "tsslStatus";
        tsslStatus.Size = new Size(1085, 20);
        tsslStatus.Spring = true;
        tsslStatus.Text = "Not connected";
        tsslStatus.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // statusStrip
        // 
        statusStrip.ImageScalingSize = new Size(20, 20);
        statusStrip.Items.AddRange(new ToolStripItem[] { tsslStatus });
        statusStrip.Location = new Point(0, 1027);
        statusStrip.Name = "statusStrip";
        statusStrip.Padding = new Padding(1, 0, 16, 0);
        statusStrip.Size = new Size(1102, 26);
        statusStrip.TabIndex = 5;
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(245, 245, 245);
        ClientSize = new Size(1102, 1053);
        Controls.Add(grpConnection);
        Controls.Add(grpSelection);
        Controls.Add(btnDelete);
        Controls.Add(pbProgress);
        Controls.Add(grpLog);
        Controls.Add(statusStrip);
        Font = new Font("Segoe UI", 9F);
        Margin = new Padding(3, 4, 3, 4);
        MinimumSize = new Size(889, 918);
        Name = "Form1";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Delete Branch NSB - Document Cleanup Tool";
        grpConnection.ResumeLayout(false);
        grpConnection.PerformLayout();
        grpSelection.ResumeLayout(false);
        grpLog.ResumeLayout(false);
        statusStrip.ResumeLayout(false);
        statusStrip.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }
}
