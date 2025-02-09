// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.AdminTools.LoanUpdateToolMainForm
// Assembly: AdminToolsUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: BCE9F231-878C-4206-826C-76CFCB8C9167
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\AdminToolsUI.dll

using EllieMae.EMLite.AdminTools.Properties;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.Version;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.AdminTools
{
  public class LoanUpdateToolMainForm : 
    Form,
    IProgressFeedback,
    ISynchronizeInvoke,
    IWin32Window,
    IServerProgressFeedback
  {
    public static IWin32Window MainScreen;
    private bool cancel;
    private LoanBatchUpdates loanBatchTool;
    private List<HMDAProfile> hmdaProfiles;
    private IContainer components;
    private Button btnClose;
    private Button btnExecute;
    private Panel panelProgressBar;
    private Label labelProcessStatus;
    private ProgressBar processPBar;
    private TextBox txtStatus;
    private Label label1;
    private ComboBox cboHMDAProfiles;
    private Label label2;
    private RadioButton rdoUpdateHMDAX100;
    private Label label7;
    private Label label6;
    private RadioButton rdoUpdateULIOnly;
    private PictureBox picAlert;
    private Button btnContinuePendingList;
    private Label label12;
    private Panel panelAlert;
    private Button btnBrowseCSV;
    private TextBox txtCSVFile;
    private Label label13;
    private Label label3;

    public LoanUpdateToolMainForm()
    {
      this.InitializeComponent();
      this.initForm();
      LoanUpdateToolMainForm.MainScreen = (IWin32Window) this;
      this.Text = "HMDA Batch Update - " + VersionInformation.CurrentVersion.DisplayVersionString;
      this.refreshAlert();
    }

    private void initForm()
    {
      this.panelProgressBar.Visible = false;
      this.txtStatus.Height = this.panelProgressBar.Top + this.panelProgressBar.Height - this.txtStatus.Top;
      this.hmdaProfiles = Session.ConfigurationManager.GetHMDAProfile();
      for (int index = 0; index < this.hmdaProfiles.Count; ++index)
        this.cboHMDAProfiles.Items.Add((object) this.hmdaProfiles[index].HMDAProfileName);
      this.cboHMDAProfiles_SelectedIndexChanged((object) null, (EventArgs) null);
      this.cboHMDAProfiles.Tag = (object) this.hmdaProfiles;
      this.option_CheckedChanged((object) null, (EventArgs) null);
    }

    private void btnExecute_Click(object sender, EventArgs e)
    {
      Button button = (Button) sender;
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
        if (button.Name == "btnContinuePendingList")
        {
          try
          {
            using (FileStream fileStream = new FileStream(LoanBatchUpdates.PendingListFile_Options, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
              using (StreamReader streamReader = new StreamReader((Stream) fileStream))
              {
                string[] strArray = streamReader.ReadToEnd().Split('|');
                streamReader.Close();
                if (strArray[0] == "1")
                {
                  this.rdoUpdateHMDAX100.Checked = true;
                  this.cboHMDAProfiles.SelectedIndex = this.findHMDAProfileIndex(Utils.ParseInt((object) strArray[1]), this.hmdaProfiles);
                }
                else
                  this.rdoUpdateULIOnly.Checked = true;
              }
            }
          }
          catch (Exception ex)
          {
            int num = (int) Utils.Dialog(LoanUpdateToolMainForm.MainScreen, "Cannot read pending list file \"" + LoanBatchUpdates.PendingListFile_Options + "\" from temp folder. " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return;
          }
        }
        LoanUpdateCondition loanUpdateCondition = new LoanUpdateCondition(this.rdoUpdateHMDAX100.Checked ? this.hmdaProfiles[this.cboHMDAProfiles.SelectedIndex] : (HMDAProfile) null);
        loanUpdateCondition.CSVFile = this.txtCSVFile.Text;
        loanUpdateCondition.HmdaProfiles = this.hmdaProfiles;
        loanUpdateCondition.UpdateOption = this.rdoUpdateULIOnly.Checked ? LoanUpdateCondition.UpdateOptions.UpdateOnly : LoanUpdateCondition.UpdateOptions.AssignNewHMDA;
        loanUpdateCondition.InTestMode = false;
        loanUpdateCondition.ProcessPendingList = button.Name == "btnContinuePendingList";
        this.btnExecute.Text = "&Stop";
        this.btnClose.Enabled = false;
        TextBox txtStatus = this.txtStatus;
        txtStatus.Text = txtStatus.Text + (this.txtStatus.Text != "" ? "\r\n\r\n" : "") + "** Batch processing started at " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt");
        this.panelProgressBar.Visible = true;
        this.txtStatus.Height = this.panelProgressBar.Top - this.txtStatus.Top - 10;
        this.loanBatchTool = new LoanBatchUpdates(loanUpdateCondition, (IProgressFeedback) this);
        this.loanBatchTool.AsynchronousProcessCompleted += new EventHandler(this.asynchronousProcess_Completed);
        this.loanBatchTool.LoanProcessCompleted += new EventHandler(this.loanProcess_Completed);
        this.loanBatchTool.Run();
      }
    }

    private int findHMDAProfileIndex(int HMDAProfileID, List<HMDAProfile> hmdaProfiles)
    {
      for (int index = 0; index < hmdaProfiles.Count; ++index)
      {
        if (hmdaProfiles[index].HMDAProfileID == HMDAProfileID)
          return index;
      }
      return -1;
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
      this.panelAlert.Invoke((Delegate) (() => this.panelAlert.Visible = File.Exists(LoanBatchUpdates.PendingListFile_AssignNew) && File.Exists(LoanBatchUpdates.PendingListFile_Options)));
    }

    private void refreshAlert()
    {
      this.panelAlert.Visible = File.Exists(LoanBatchUpdates.PendingListFile_AssignNew) && File.Exists(LoanBatchUpdates.PendingListFile_Options);
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
          this.Invoke((Delegate) new LoanUpdateToolMainForm.PropertySetter(this.setDetails), (object) value);
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
        this.Invoke((Delegate) new LoanUpdateToolMainForm.PropertySetter(this.incrementProgress), (object) count);
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
        this.Invoke((Delegate) new LoanUpdateToolMainForm.PropertySetter(this.resetProgress), (object) maxValue);
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

    private void cboHMDAProfiles_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.setExecuteButtonStatus();
    }

    private void option_CheckedChanged(object sender, EventArgs e)
    {
      if (sender != null)
      {
        RadioButton radioButton = (RadioButton) sender;
        if (radioButton.Name == "rdoUpdateHMDAX100" && radioButton.Checked)
        {
          this.rdoUpdateULIOnly.Checked = false;
          this.cboHMDAProfiles.Enabled = true;
        }
        else if (radioButton.Name == "rdoUpdateULIOnly" && radioButton.Checked)
          this.rdoUpdateHMDAX100.Checked = false;
      }
      if (!this.rdoUpdateHMDAX100.Checked)
      {
        this.cboHMDAProfiles.SelectedIndex = -1;
        this.cboHMDAProfiles.Enabled = false;
      }
      this.setExecuteButtonStatus();
    }

    private void setExecuteButtonStatus()
    {
      if (this.rdoUpdateHMDAX100.Checked)
        this.btnExecute.Enabled = this.cboHMDAProfiles.Text != "";
      else if (this.rdoUpdateULIOnly.Checked)
        this.btnExecute.Enabled = true;
      else
        this.btnExecute.Enabled = false;
      if (!this.btnExecute.Enabled || !(this.txtCSVFile.Text == "") && this.txtCSVFile.Text.Trim().ToLower().EndsWith(".csv") && File.Exists(this.txtCSVFile.Text))
        return;
      this.btnExecute.Enabled = false;
    }

    private void optionUpdate_CheckedChanged(object sender, EventArgs e)
    {
      this.setExecuteButtonStatus();
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

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (LoanUpdateToolMainForm));
      this.btnClose = new Button();
      this.btnExecute = new Button();
      this.panelProgressBar = new Panel();
      this.labelProcessStatus = new Label();
      this.processPBar = new ProgressBar();
      this.txtStatus = new TextBox();
      this.label1 = new Label();
      this.cboHMDAProfiles = new ComboBox();
      this.label2 = new Label();
      this.rdoUpdateHMDAX100 = new RadioButton();
      this.label7 = new Label();
      this.label6 = new Label();
      this.rdoUpdateULIOnly = new RadioButton();
      this.picAlert = new PictureBox();
      this.btnContinuePendingList = new Button();
      this.label12 = new Label();
      this.panelAlert = new Panel();
      this.btnBrowseCSV = new Button();
      this.txtCSVFile = new TextBox();
      this.label13 = new Label();
      this.label3 = new Label();
      this.panelProgressBar.SuspendLayout();
      ((ISupportInitialize) this.picAlert).BeginInit();
      this.panelAlert.SuspendLayout();
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
      this.txtStatus.Location = new Point(12, 239);
      this.txtStatus.Multiline = true;
      this.txtStatus.Name = "txtStatus";
      this.txtStatus.ScrollBars = ScrollBars.Both;
      this.txtStatus.Size = new Size(941, 399);
      this.txtStatus.TabIndex = 6;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(12, 203);
      this.label1.Name = "label1";
      this.label1.Size = new Size(95, 13);
      this.label1.TabIndex = 7;
      this.label1.Text = "Processing Status:";
      this.cboHMDAProfiles.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboHMDAProfiles.FormattingEnabled = true;
      this.cboHMDAProfiles.Location = new Point(166, 162);
      this.cboHMDAProfiles.Name = "cboHMDAProfiles";
      this.cboHMDAProfiles.Size = new Size(303, 21);
      this.cboHMDAProfiles.TabIndex = 8;
      this.cboHMDAProfiles.SelectedIndexChanged += new EventHandler(this.cboHMDAProfiles_SelectedIndexChanged);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(34, 165);
      this.label2.Name = "label2";
      this.label2.Size = new Size(117, 13);
      this.label2.TabIndex = 9;
      this.label2.Text = "HMDA Profile to Assign";
      this.rdoUpdateHMDAX100.AutoSize = true;
      this.rdoUpdateHMDAX100.Location = new Point(19, 125);
      this.rdoUpdateHMDAX100.Name = "rdoUpdateHMDAX100";
      this.rdoUpdateHMDAX100.Size = new Size(136, 17);
      this.rdoUpdateHMDAX100.TabIndex = 12;
      this.rdoUpdateHMDAX100.TabStop = true;
      this.rdoUpdateHMDAX100.Text = "Override Existing Profile";
      this.rdoUpdateHMDAX100.UseVisualStyleBackColor = true;
      this.rdoUpdateHMDAX100.CheckedChanged += new EventHandler(this.option_CheckedChanged);
      this.label7.AutoSize = true;
      this.label7.ForeColor = SystemColors.ControlText;
      this.label7.Location = new Point(34, 144);
      this.label7.Name = "label7";
      this.label7.Size = new Size(409, 13);
      this.label7.TabIndex = 14;
      this.label7.Text = "Use this option to apply HMDA calculations using selected profile to all selected loans";
      this.label6.AutoSize = true;
      this.label6.ForeColor = SystemColors.ControlText;
      this.label6.Location = new Point(34, 90);
      this.label6.Name = "label6";
      this.label6.Size = new Size(447, 13);
      this.label6.TabIndex = 13;
      this.label6.Text = "Use this option to apply HMDA calculations using current assigned profile to all selected loans";
      this.rdoUpdateULIOnly.AutoSize = true;
      this.rdoUpdateULIOnly.Location = new Point(19, 71);
      this.rdoUpdateULIOnly.Name = "rdoUpdateULIOnly";
      this.rdoUpdateULIOnly.Size = new Size(115, 17);
      this.rdoUpdateULIOnly.TabIndex = 12;
      this.rdoUpdateULIOnly.TabStop = true;
      this.rdoUpdateULIOnly.Text = "Use Existing Profile";
      this.rdoUpdateULIOnly.UseVisualStyleBackColor = true;
      this.rdoUpdateULIOnly.CheckedChanged += new EventHandler(this.option_CheckedChanged);
      this.picAlert.Image = (Image) Resources.alert_16x16;
      this.picAlert.Location = new Point(3, 3);
      this.picAlert.Name = "picAlert";
      this.picAlert.Size = new Size(16, 16);
      this.picAlert.SizeMode = PictureBoxSizeMode.AutoSize;
      this.picAlert.TabIndex = 22;
      this.picAlert.TabStop = false;
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
      this.panelAlert.Location = new Point(113, 198);
      this.panelAlert.Name = "panelAlert";
      this.panelAlert.Size = new Size(292, 24);
      this.panelAlert.TabIndex = 25;
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
      this.label3.AutoSize = true;
      this.label3.Location = new Point(12, 50);
      this.label3.Name = "label3";
      this.label3.Size = new Size(99, 13);
      this.label3.TabIndex = 32;
      this.label3.Text = "Recalculate HMDA";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(965, 739);
      this.Controls.Add((Control) this.label7);
      this.Controls.Add((Control) this.label6);
      this.Controls.Add((Control) this.rdoUpdateHMDAX100);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.cboHMDAProfiles);
      this.Controls.Add((Control) this.rdoUpdateULIOnly);
      this.Controls.Add((Control) this.label13);
      this.Controls.Add((Control) this.txtCSVFile);
      this.Controls.Add((Control) this.btnBrowseCSV);
      this.Controls.Add((Control) this.panelAlert);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.txtStatus);
      this.Controls.Add((Control) this.panelProgressBar);
      this.Controls.Add((Control) this.btnExecute);
      this.Controls.Add((Control) this.btnClose);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MinimizeBox = false;
      this.Name = nameof (LoanUpdateToolMainForm);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "HMDA Batch Update";
      this.panelProgressBar.ResumeLayout(false);
      ((ISupportInitialize) this.picAlert).EndInit();
      this.panelAlert.ResumeLayout(false);
      this.panelAlert.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    [SpecialName]
    Form IProgressFeedback.get_ParentForm() => this.ParentForm;

    private delegate void PropertySetter(object value);
  }
}
