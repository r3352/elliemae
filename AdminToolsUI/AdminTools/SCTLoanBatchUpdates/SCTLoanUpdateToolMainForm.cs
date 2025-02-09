// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.AdminTools.SCTLoanBatchUpdates.SCTLoanUpdateToolMainForm
// Assembly: AdminToolsUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: BCE9F231-878C-4206-826C-76CFCB8C9167
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\AdminToolsUI.dll

using EllieMae.EMLite.AdminTools.Properties;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.Version;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.AdminTools.SCTLoanBatchUpdates
{
  public class SCTLoanUpdateToolMainForm : 
    Form,
    IProgressFeedback,
    ISynchronizeInvoke,
    IWin32Window,
    IServerProgressFeedback
  {
    public static IWin32Window MainScreen;
    private bool cancel;
    private EllieMae.EMLite.AdminTools.SCTLoanBatchUpdates.SCTLoanBatchUpdates loanBatchTool;
    private string OldLog;
    private IContainer components;
    private Button btnClose;
    private Button btnExecute;
    private Panel panelProgressBar;
    private Label labelProcessStatus;
    private ProgressBar processPBar;
    private TextBox txtStatus;
    private Label label1;
    private PictureBox picAlert;
    private Button btnContinuePendingList;
    private Label label12;
    private Panel panelAlert;
    private Button btnBrowseCSV;
    private TextBox txtCSVFile;
    private Label label13;
    private CheckBox TestModeCB;
    private Panel panelError;
    private PictureBox pictureBox1;
    private Button btnDisplayErrorLog;
    private Button btnExportErrorLog;
    private Label label2;
    private Button btnExportLog;
    private Button btnDisplayLog;
    private CheckBox cbSkipDupCheck;

    public SCTLoanUpdateToolMainForm()
    {
      this.InitializeComponent();
      this.initForm();
      SCTLoanUpdateToolMainForm.MainScreen = (IWin32Window) this;
      this.Text = "Commitment Terms Data Migration Tool - " + VersionInformation.CurrentVersion.DisplayVersionString;
      this.refreshAlert();
      this.refreshError();
    }

    private void initForm()
    {
      this.panelProgressBar.Visible = false;
      this.txtStatus.Height = this.panelProgressBar.Top + this.panelProgressBar.Height - this.txtStatus.Top;
      this.TestModeCB.Checked = false;
      this.TestModeCB.Visible = false;
      this.cbSkipDupCheck.Visible = false;
      this.cbSkipDupCheck.Checked = false;
      EllieMae.EMLite.AdminTools.SCTLoanBatchUpdates.SCTLoanBatchUpdates.ErrorFilePath = (string) null;
    }

    private void btnExecute_Click(object sender, EventArgs e)
    {
      if (this.btnDisplayErrorLog.Text == "Done")
      {
        this.btnDisplayErrorLog.Text = "Display Errors";
        this.txtStatus.Text = this.OldLog;
        this.OldLog = (string) null;
      }
      if (this.btnDisplayLog.Text == "Done")
      {
        this.btnDisplayLog.Text = "Display Log";
        this.txtStatus.Text = this.OldLog;
        this.OldLog = (string) null;
      }
      if (!(sender is Button button))
        return;
      if (string.Compare(this.btnExecute.Text, "&Stop", true) == 0)
      {
        if (Utils.Dialog(LoanUpdateToolMainForm.MainScreen, "Are you sure you want to stop process?", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) != DialogResult.OK)
          return;
        this.loanBatchTool.IsCancelled = true;
      }
      else
      {
        if (!(button.Name == "btnContinuePendingList") && string.Compare(this.btnExecute.Text, "&Execute", true) != 0)
          return;
        if (button.Name == "btnExecute" && this.panelAlert.Visible)
        {
          if (Utils.Dialog(LoanUpdateToolMainForm.MainScreen, "You have pending list to complete. Are you sure you want to start a new batch process? Select \"Ok\" will remove the old pending list.", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK)
            return;
          this.panelAlert.Visible = false;
        }
        SCTLoanUpdateCondition loanUpdateCondition = new SCTLoanUpdateCondition();
        loanUpdateCondition.CSVFile = this.txtCSVFile.Text;
        loanUpdateCondition.ProcessPendingList = button.Name == "btnContinuePendingList";
        this.btnExecute.Text = "&Stop";
        this.btnClose.Enabled = false;
        TextBox txtStatus = this.txtStatus;
        txtStatus.Text = txtStatus.Text + (this.txtStatus.Text != "" ? "\r\n\r\n" : "") + "** Batch processing started at " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt");
        this.panelProgressBar.Visible = true;
        this.txtStatus.Height = this.panelProgressBar.Top - this.txtStatus.Top - 10;
        this.loanBatchTool = new EllieMae.EMLite.AdminTools.SCTLoanBatchUpdates.SCTLoanBatchUpdates(loanUpdateCondition, (IProgressFeedback) this);
        this.loanBatchTool.AsynchronousProcessCompleted += new EventHandler(this.asynchronousProcess_Completed);
        this.loanBatchTool.LoanProcessCompleted += new EventHandler(this.loanProcess_Completed);
        this.loanBatchTool.Run();
      }
    }

    private void asynchronousProcess_Completed(object sender, EventArgs e)
    {
      if (string.Compare(this.btnExecute.Text, "&Stop", true) == 0)
        this.btnExecute.Invoke((Delegate) (() => this.btnExecute.Text = "&Execute"));
      this.btnClose.Invoke((Delegate) (() => this.btnClose.Enabled = true));
      this.panelProgressBar.Invoke((Delegate) (() => this.panelProgressBar.Visible = false));
      this.txtStatus.Invoke((Delegate) (() => this.txtStatus.Height = this.panelProgressBar.Top + this.panelProgressBar.Height - this.txtStatus.Top));
      this.txtStatus.Invoke((Delegate) (() =>
      {
        TextBox txtStatus = this.txtStatus;
        txtStatus.Text = txtStatus.Text + "\r\n-- Batch processing " + (this.loanBatchTool.ProcessingResult != DialogResult.OK ? "aborted" : "completed") + " at " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt");
      }));
      this.txtStatus.Invoke((Delegate) (() => this.txtStatus.SelectionStart = this.txtStatus.TextLength));
      this.txtStatus.Invoke((Delegate) (() => this.txtStatus.ScrollToCaret()));
      this.panelAlert.Invoke((Delegate) (() => this.refreshAlert()));
      this.panelAlert.Invoke((Delegate) (() => this.refreshError()));
    }

    private void refreshAlert()
    {
      this.panelAlert.Visible = !string.IsNullOrWhiteSpace(EllieMae.EMLite.AdminTools.SCTLoanBatchUpdates.SCTLoanBatchUpdates.PendingFile) && File.Exists(EllieMae.EMLite.AdminTools.SCTLoanBatchUpdates.SCTLoanBatchUpdates.PendingFile);
    }

    private void refreshError()
    {
      this.panelError.Visible = !string.IsNullOrWhiteSpace(EllieMae.EMLite.AdminTools.SCTLoanBatchUpdates.SCTLoanBatchUpdates.ErrorFilePath) && File.Exists(EllieMae.EMLite.AdminTools.SCTLoanBatchUpdates.SCTLoanBatchUpdates.ErrorFilePath);
      Point location = this.panelAlert.Location;
      if (this.panelAlert.Visible)
        location.Offset(this.panelAlert.Width + 5, 0);
      this.panelError.Location = location;
    }

    private void loanProcess_Completed(object sender, EventArgs e)
    {
      object[] loanBasicInfo = (object[]) null;
      string status = "";
      if (sender is string)
        status = (string) sender;
      else
        loanBasicInfo = (object[]) sender;
      if (loanBasicInfo == null)
        this.txtStatus.Invoke((Delegate) (() =>
        {
          TextBox txtStatus = this.txtStatus;
          txtStatus.Text = txtStatus.Text + "\r\n" + status;
        }));
      else
        this.txtStatus.Invoke((Delegate) (() =>
        {
          TextBox txtStatus = this.txtStatus;
          txtStatus.Text = txtStatus.Text + "\r\n" + loanBasicInfo[0].ToString() + " - " + (loanBasicInfo[1] != null ? loanBasicInfo[1].ToString() : "") + " : " + (loanBasicInfo[2] != null ? loanBasicInfo[2].ToString().Replace(":", "") : "") + " : " + loanBasicInfo[3].ToString();
        }));
      this.txtStatus.Invoke((Delegate) (() => this.txtStatus.SelectionStart = this.txtStatus.TextLength));
      this.txtStatus.Invoke((Delegate) (() => this.txtStatus.ScrollToCaret()));
    }

    public DialogResult ShowDialog(System.Type formType, params object[] args) => DialogResult.OK;

    public DialogResult MsgBox(string text, MessageBoxButtons buttons, MessageBoxIcon icon)
    {
      return DialogResult.OK;
    }

    public int MaxValue { get; set; }

    public bool Cancel { get; set; }

    public string Status { get; set; }

    public string Details
    {
      get
      {
        lock (this)
          return this.labelProcessStatus.Text;
      }
      set
      {
        if (this.InvokeRequired)
          this.Invoke((Delegate) new SCTLoanUpdateToolMainForm.PropertySetter(this.setDetails), (object) value);
        else
          this.setDetails((object) value);
      }
    }

    private void setDetails(object value)
    {
      lock (this)
      {
        if (this.cancel)
          return;
        this.labelProcessStatus.Text = this.fitStringToLabel(value.ToString(), this.labelProcessStatus);
      }
    }

    private string fitStringToLabel(string text, Label label)
    {
      using (Graphics graphics = label.CreateGraphics())
      {
        if ((double) graphics.MeasureString(text, label.Font).Width < (double) label.Width)
          return text;
        for (; text.Length > 0; text = text.Substring(1))
        {
          if ((double) graphics.MeasureString(text + "...", label.Font).Width <= (double) label.Width)
            break;
        }
      }
      return text + "...";
    }

    public bool Increment(int count)
    {
      if (this.InvokeRequired)
        this.Invoke((Delegate) new SCTLoanUpdateToolMainForm.PropertySetter(this.incrementProgress), (object) count);
      else
        this.incrementProgress((object) count);
      lock (this)
        return !this.cancel;
    }

    private void incrementProgress(object value)
    {
      lock (this)
        this.processPBar.Value = Math.Min(this.processPBar.Value + (int) value, this.processPBar.Maximum);
    }

    public int Value { get; set; }

    public bool ResetCounter(int maxValue)
    {
      if (this.InvokeRequired)
        this.Invoke((Delegate) new SCTLoanUpdateToolMainForm.PropertySetter(this.resetProgress), (object) maxValue);
      else
        this.resetProgress((object) maxValue);
      lock (this)
        return !this.cancel;
    }

    private void resetProgress(object maxValue)
    {
      lock (this)
      {
        this.processPBar.Value = 0;
        this.processPBar.Maximum = (int) maxValue;
      }
    }

    public bool SetFeedback(string status, string details, int value) => true;

    private void btnClose_Click(object sender, EventArgs e)
    {
      this.Close();
      this.Dispose();
    }

    private void setExecuteButtonStatus()
    {
      this.btnExecute.Enabled = !string.IsNullOrWhiteSpace(this.txtCSVFile.Text);
      this.btnExecute.Enabled &= this.txtCSVFile.Text.Trim().ToLower().EndsWith(".csv");
      this.btnExecute.Enabled &= File.Exists(this.txtCSVFile.Text);
    }

    private void btnContinuePendingList_Click(object sender, EventArgs e)
    {
      this.btnExecute_Click(sender, e);
    }

    private void btnBrowseCSV_Click(object sender, EventArgs e)
    {
      using (OpenFileDialog openFileDialog = new OpenFileDialog())
      {
        openFileDialog.Filter = "CSV File (*.csv)|*.csv";
        openFileDialog.Multiselect = false;
        openFileDialog.ReadOnlyChecked = true;
        openFileDialog.Title = "Please select a CSV file generated by Report or Pipeline View";
        if (openFileDialog.ShowDialog((IWin32Window) this) == DialogResult.OK)
          this.txtCSVFile.Text = openFileDialog.FileName;
      }
      this.setExecuteButtonStatus();
    }

    private void txtCSVFile_TextChanged(object sender, EventArgs e)
    {
      this.setExecuteButtonStatus();
    }

    private void btnDisplayErrorLog_Click(object sender, EventArgs e)
    {
      if (this.btnDisplayLog.Text == "Done")
      {
        this.btnDisplayLog.Text = "Display Log";
        this.txtStatus.Text = this.OldLog;
        this.OldLog = (string) null;
      }
      if (this.OldLog == null)
      {
        this.btnDisplayErrorLog.Text = "Done";
        if (string.IsNullOrWhiteSpace(EllieMae.EMLite.AdminTools.SCTLoanBatchUpdates.SCTLoanBatchUpdates.ErrorFilePath) || !File.Exists(EllieMae.EMLite.AdminTools.SCTLoanBatchUpdates.SCTLoanBatchUpdates.ErrorFilePath))
          return;
        this.OldLog = this.txtStatus.Text;
        using (StreamReader streamReader = new StreamReader(EllieMae.EMLite.AdminTools.SCTLoanBatchUpdates.SCTLoanBatchUpdates.ErrorFilePath, Encoding.ASCII))
          this.txtStatus.Text = streamReader.ReadToEnd();
      }
      else
      {
        this.btnDisplayErrorLog.Text = "Display Errors";
        this.txtStatus.Text = this.OldLog;
        this.OldLog = (string) null;
      }
    }

    private void btnExportErrorLog_Click(object sender, EventArgs e)
    {
      if (!string.IsNullOrWhiteSpace(EllieMae.EMLite.AdminTools.SCTLoanBatchUpdates.SCTLoanBatchUpdates.ErrorFilePath) && File.Exists(EllieMae.EMLite.AdminTools.SCTLoanBatchUpdates.SCTLoanBatchUpdates.ErrorFilePath))
      {
        using (SaveFileDialog saveFileDialog = new SaveFileDialog())
        {
          saveFileDialog.Filter = "CSV File (*.csv)|*.csv";
          saveFileDialog.Title = "Please enter a file name for the error log.";
          saveFileDialog.FileName = Path.GetFileNameWithoutExtension(this.txtCSVFile.Text) + "_errors.csv";
          if (saveFileDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
            return;
          string fileName = saveFileDialog.FileName;
          File.Move(EllieMae.EMLite.AdminTools.SCTLoanBatchUpdates.SCTLoanBatchUpdates.ErrorFilePath, fileName);
        }
      }
      else
      {
        int num = (int) MessageBox.Show("The error log file was not found", "Export Log File", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }

    private void btnDisplayLog_Click(object sender, EventArgs e)
    {
      if (this.btnDisplayErrorLog.Text == "Done")
      {
        this.btnDisplayErrorLog.Text = "Display Errors";
        this.txtStatus.Text = this.OldLog;
        this.OldLog = (string) null;
      }
      if (this.OldLog == null)
      {
        this.btnDisplayLog.Text = "Done";
        if (string.IsNullOrWhiteSpace(EllieMae.EMLite.AdminTools.SCTLoanBatchUpdates.SCTLoanBatchUpdates.getLogFile()) || !File.Exists(EllieMae.EMLite.AdminTools.SCTLoanBatchUpdates.SCTLoanBatchUpdates.getLogFile()))
          return;
        this.OldLog = this.txtStatus.Text;
        using (StreamReader streamReader = new StreamReader(EllieMae.EMLite.AdminTools.SCTLoanBatchUpdates.SCTLoanBatchUpdates.getLogFile(), Encoding.ASCII))
          this.txtStatus.Text = streamReader.ReadToEnd();
      }
      else
      {
        this.btnDisplayLog.Text = "Display Log";
        this.txtStatus.Text = this.OldLog;
        this.OldLog = (string) null;
      }
    }

    private void btnExportLog_Click(object sender, EventArgs e)
    {
      if (!string.IsNullOrWhiteSpace(EllieMae.EMLite.AdminTools.SCTLoanBatchUpdates.SCTLoanBatchUpdates.getLogFile()) && File.Exists(EllieMae.EMLite.AdminTools.SCTLoanBatchUpdates.SCTLoanBatchUpdates.getLogFile()))
      {
        using (SaveFileDialog saveFileDialog = new SaveFileDialog())
        {
          saveFileDialog.Filter = "Log File (*.log)|*.log";
          saveFileDialog.Title = "Please enter a file name for the log file.";
          saveFileDialog.FileName = Path.GetFileNameWithoutExtension(this.txtCSVFile.Text) + "_log.log";
          if (saveFileDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
            return;
          string fileName = saveFileDialog.FileName;
          File.Move(EllieMae.EMLite.AdminTools.SCTLoanBatchUpdates.SCTLoanBatchUpdates.getLogFile(), fileName);
        }
      }
      else
      {
        int num = (int) MessageBox.Show("The log file was not found", "Export Log File", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (SCTLoanUpdateToolMainForm));
      this.btnClose = new Button();
      this.btnExecute = new Button();
      this.panelProgressBar = new Panel();
      this.labelProcessStatus = new Label();
      this.processPBar = new ProgressBar();
      this.txtStatus = new TextBox();
      this.label1 = new Label();
      this.btnContinuePendingList = new Button();
      this.label12 = new Label();
      this.panelAlert = new Panel();
      this.picAlert = new PictureBox();
      this.btnBrowseCSV = new Button();
      this.txtCSVFile = new TextBox();
      this.label13 = new Label();
      this.TestModeCB = new CheckBox();
      this.panelError = new Panel();
      this.pictureBox1 = new PictureBox();
      this.btnDisplayErrorLog = new Button();
      this.btnExportErrorLog = new Button();
      this.label2 = new Label();
      this.btnExportLog = new Button();
      this.btnDisplayLog = new Button();
      this.cbSkipDupCheck = new CheckBox();
      this.panelProgressBar.SuspendLayout();
      this.panelAlert.SuspendLayout();
      ((ISupportInitialize) this.picAlert).BeginInit();
      this.panelError.SuspendLayout();
      ((ISupportInitialize) this.pictureBox1).BeginInit();
      this.SuspendLayout();
      this.btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnClose.Location = new Point(878, 704);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new Size(75, 23);
      this.btnClose.TabIndex = 0;
      this.btnClose.Text = "&Close";
      this.btnClose.UseVisualStyleBackColor = true;
      this.btnClose.Click += new EventHandler(this.btnClose_Click);
      this.btnExecute.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnExecute.Location = new Point(797, 704);
      this.btnExecute.Name = "btnExecute";
      this.btnExecute.Size = new Size(75, 23);
      this.btnExecute.TabIndex = 1;
      this.btnExecute.Text = "&Execute";
      this.btnExecute.UseVisualStyleBackColor = true;
      this.btnExecute.Click += new EventHandler(this.btnExecute_Click);
      this.panelProgressBar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.panelProgressBar.BorderStyle = BorderStyle.FixedSingle;
      this.panelProgressBar.Controls.Add((Control) this.labelProcessStatus);
      this.panelProgressBar.Controls.Add((Control) this.processPBar);
      this.panelProgressBar.Location = new Point(12, 644);
      this.panelProgressBar.Name = "panelProgressBar";
      this.panelProgressBar.Size = new Size(941, 51);
      this.panelProgressBar.TabIndex = 5;
      this.labelProcessStatus.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.labelProcessStatus.Location = new Point(3, 6);
      this.labelProcessStatus.Name = "labelProcessStatus";
      this.labelProcessStatus.Size = new Size(933, 13);
      this.labelProcessStatus.TabIndex = 6;
      this.labelProcessStatus.Text = "(Status)";
      this.labelProcessStatus.TextAlign = ContentAlignment.BottomLeft;
      this.processPBar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.processPBar.Location = new Point(6, 26);
      this.processPBar.Name = "processPBar";
      this.processPBar.Size = new Size(930, 17);
      this.processPBar.TabIndex = 5;
      this.txtStatus.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.txtStatus.Location = new Point(8, 75);
      this.txtStatus.Multiline = true;
      this.txtStatus.Name = "txtStatus";
      this.txtStatus.ScrollBars = ScrollBars.Both;
      this.txtStatus.Size = new Size(941, 554);
      this.txtStatus.TabIndex = 6;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(12, 54);
      this.label1.Name = "label1";
      this.label1.Size = new Size(95, 13);
      this.label1.TabIndex = 7;
      this.label1.Text = "Processing Status:";
      this.btnContinuePendingList.Location = new Point(196, -1);
      this.btnContinuePendingList.Name = "btnContinuePendingList";
      this.btnContinuePendingList.Size = new Size(75, 23);
      this.btnContinuePendingList.TabIndex = 23;
      this.btnContinuePendingList.Text = "Continue";
      this.btnContinuePendingList.UseVisualStyleBackColor = true;
      this.btnContinuePendingList.Click += new EventHandler(this.btnContinuePendingList_Click);
      this.label12.AutoSize = true;
      this.label12.ForeColor = Color.Brown;
      this.label12.Location = new Point(25, 5);
      this.label12.Name = "label12";
      this.label12.Size = new Size(170, 13);
      this.label12.TabIndex = 24;
      this.label12.Text = "You have pending list to complete.";
      this.panelAlert.BackColor = SystemColors.ActiveCaption;
      this.panelAlert.BorderStyle = BorderStyle.FixedSingle;
      this.panelAlert.Controls.Add((Control) this.picAlert);
      this.panelAlert.Controls.Add((Control) this.btnContinuePendingList);
      this.panelAlert.Controls.Add((Control) this.label12);
      this.panelAlert.Location = new Point(113, 49);
      this.panelAlert.Name = "panelAlert";
      this.panelAlert.Size = new Size(292, 24);
      this.panelAlert.TabIndex = 25;
      this.picAlert.Image = (Image) Resources.alert_16x16;
      this.picAlert.Location = new Point(3, 3);
      this.picAlert.Name = "picAlert";
      this.picAlert.Size = new Size(16, 16);
      this.picAlert.SizeMode = PictureBoxSizeMode.AutoSize;
      this.picAlert.TabIndex = 22;
      this.picAlert.TabStop = false;
      this.btnBrowseCSV.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnBrowseCSV.Location = new Point(878, 16);
      this.btnBrowseCSV.Name = "btnBrowseCSV";
      this.btnBrowseCSV.Size = new Size(75, 23);
      this.btnBrowseCSV.TabIndex = 27;
      this.btnBrowseCSV.Text = "Browse...";
      this.btnBrowseCSV.UseVisualStyleBackColor = true;
      this.btnBrowseCSV.Click += new EventHandler(this.btnBrowseCSV_Click);
      this.txtCSVFile.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtCSVFile.Location = new Point(68, 18);
      this.txtCSVFile.Name = "txtCSVFile";
      this.txtCSVFile.ReadOnly = true;
      this.txtCSVFile.Size = new Size(804, 20);
      this.txtCSVFile.TabIndex = 28;
      this.txtCSVFile.TextChanged += new EventHandler(this.txtCSVFile_TextChanged);
      this.label13.AutoSize = true;
      this.label13.Location = new Point(12, 21);
      this.label13.Name = "label13";
      this.label13.Size = new Size(50, 13);
      this.label13.TabIndex = 29;
      this.label13.Text = "Input File";
      this.TestModeCB.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.TestModeCB.AutoSize = true;
      this.TestModeCB.Location = new Point(12, 708);
      this.TestModeCB.Name = "TestModeCB";
      this.TestModeCB.Size = new Size(77, 17);
      this.TestModeCB.TabIndex = 30;
      this.TestModeCB.Text = "Test Mode";
      this.TestModeCB.UseVisualStyleBackColor = true;
      this.panelError.BackColor = SystemColors.ActiveCaption;
      this.panelError.BorderStyle = BorderStyle.FixedSingle;
      this.panelError.Controls.Add((Control) this.pictureBox1);
      this.panelError.Controls.Add((Control) this.btnDisplayErrorLog);
      this.panelError.Controls.Add((Control) this.btnExportErrorLog);
      this.panelError.Controls.Add((Control) this.label2);
      this.panelError.Location = new Point(411, 49);
      this.panelError.Name = "panelError";
      this.panelError.Size = new Size(442, 24);
      this.panelError.TabIndex = 25;
      this.pictureBox1.Image = (Image) Resources.alert_16x16;
      this.pictureBox1.Location = new Point(3, 3);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new Size(16, 16);
      this.pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureBox1.TabIndex = 22;
      this.pictureBox1.TabStop = false;
      this.btnDisplayErrorLog.Location = new Point(241, 0);
      this.btnDisplayErrorLog.Name = "btnDisplayErrorLog";
      this.btnDisplayErrorLog.Size = new Size(90, 23);
      this.btnDisplayErrorLog.TabIndex = 23;
      this.btnDisplayErrorLog.Text = "Display Errors";
      this.btnDisplayErrorLog.UseVisualStyleBackColor = true;
      this.btnDisplayErrorLog.Click += new EventHandler(this.btnDisplayErrorLog_Click);
      this.btnExportErrorLog.Location = new Point(337, 0);
      this.btnExportErrorLog.Name = "btnExportErrorLog";
      this.btnExportErrorLog.Size = new Size(90, 23);
      this.btnExportErrorLog.TabIndex = 23;
      this.btnExportErrorLog.Text = "Export Errors...";
      this.btnExportErrorLog.UseVisualStyleBackColor = true;
      this.btnExportErrorLog.Click += new EventHandler(this.btnExportErrorLog_Click);
      this.label2.AutoSize = true;
      this.label2.ForeColor = Color.Brown;
      this.label2.Location = new Point(25, 5);
      this.label2.Name = "label2";
      this.label2.Size = new Size(199, 13);
      this.label2.TabIndex = 24;
      this.label2.Text = "There were errors proccessing the loans.";
      this.btnExportLog.Location = new Point(683, 704);
      this.btnExportLog.Name = "btnExportLog";
      this.btnExportLog.Size = new Size(75, 23);
      this.btnExportLog.TabIndex = 23;
      this.btnExportLog.Text = "Export Log...";
      this.btnExportLog.UseVisualStyleBackColor = true;
      this.btnExportLog.Click += new EventHandler(this.btnExportLog_Click);
      this.btnDisplayLog.Location = new Point(602, 704);
      this.btnDisplayLog.Name = "btnDisplayLog";
      this.btnDisplayLog.Size = new Size(75, 23);
      this.btnDisplayLog.TabIndex = 23;
      this.btnDisplayLog.Text = "Display Log";
      this.btnDisplayLog.UseVisualStyleBackColor = true;
      this.btnDisplayLog.Click += new EventHandler(this.btnDisplayLog_Click);
      this.cbSkipDupCheck.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.cbSkipDupCheck.AutoSize = true;
      this.cbSkipDupCheck.Location = new Point(95, 708);
      this.cbSkipDupCheck.Name = "cbSkipDupCheck";
      this.cbSkipDupCheck.Size = new Size(129, 17);
      this.cbSkipDupCheck.TabIndex = 30;
      this.cbSkipDupCheck.Text = "Skip Duplicate Check";
      this.cbSkipDupCheck.UseVisualStyleBackColor = true;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(965, 739);
      this.Controls.Add((Control) this.cbSkipDupCheck);
      this.Controls.Add((Control) this.TestModeCB);
      this.Controls.Add((Control) this.btnDisplayLog);
      this.Controls.Add((Control) this.btnExportLog);
      this.Controls.Add((Control) this.label13);
      this.Controls.Add((Control) this.txtCSVFile);
      this.Controls.Add((Control) this.btnBrowseCSV);
      this.Controls.Add((Control) this.panelError);
      this.Controls.Add((Control) this.panelAlert);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.txtStatus);
      this.Controls.Add((Control) this.panelProgressBar);
      this.Controls.Add((Control) this.btnExecute);
      this.Controls.Add((Control) this.btnClose);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MinimizeBox = false;
      this.Name = nameof (SCTLoanUpdateToolMainForm);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Commitment Terms Data Migration Tool";
      this.panelProgressBar.ResumeLayout(false);
      this.panelAlert.ResumeLayout(false);
      this.panelAlert.PerformLayout();
      ((ISupportInitialize) this.picAlert).EndInit();
      this.panelError.ResumeLayout(false);
      this.panelError.PerformLayout();
      ((ISupportInitialize) this.pictureBox1).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    [SpecialName]
    Form IProgressFeedback.get_ParentForm() => this.ParentForm;

    private delegate void PropertySetter(object value);
  }
}
