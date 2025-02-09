// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.IncomeAnalyzerControl
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.Properties;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class IncomeAnalyzerControl : UserControl
  {
    private Sessions.Session session;
    private LoanData loan;
    private AIQIncomeData aiqData;
    public List<Control> isSelectedControls = new List<Control>();
    private string messageID = string.Empty;
    private bool? _importAiqAnalyzerIncomeData;
    private IContainer components;
    private GroupContainer groupContainer2;
    private Button btnImport;
    private Panel panelError;
    private Label labelError;
    private Panel panelAIQandVOEData;
    private Panel panelHeader;
    private CheckBox toImportAll;
    private Label label2;
    private Label label1;
    private Label label3;
    private PictureBox expand;
    private PictureBox collapse;
    private CheckBox chk_automate;

    public string MessageID
    {
      get => this.messageID;
      set => this.messageID = value;
    }

    public IncomeAnalyzerControl(Sessions.Session session, LoanData loan)
    {
      this.session = session;
      this.loan = loan;
      this.InitializeComponent();
      if (!this.ImportAIQAnalyzerIncomeDataAuthorized())
        this.btnImport.Visible = false;
      this.populateAutomateCheckbox();
      this.Dock = DockStyle.Fill;
    }

    private void populateAutomateCheckbox()
    {
      if (this.loan.GetField("ANALYZER.X18") == "Y")
        this.chk_automate.Checked = true;
      else
        this.chk_automate.Checked = false;
    }

    private bool ImportAIQAnalyzerIncomeDataAuthorized()
    {
      if (!this._importAiqAnalyzerIncomeData.HasValue)
      {
        int num;
        if (this.session.StartupInfo.UserAclFeaturConfigRights.ContainsKey(AclFeature.SettingsTab_EncompassAIQAccess_AIQAnalyzers_ImportIncomeData))
          num = ((IEnumerable<int>) new int[2]
          {
            1,
            int.MaxValue
          }).Contains<int>(this.session.StartupInfo.UserAclFeaturConfigRights[AclFeature.SettingsTab_EncompassAIQAccess_AIQAnalyzers_ImportIncomeData]) ? 1 : 0;
        else
          num = 0;
        this._importAiqAnalyzerIncomeData = new bool?(num != 0);
      }
      return this._importAiqAnalyzerIncomeData.Value;
    }

    public string PopulateData(AIQIncomeData aiqData)
    {
      this.aiqData = aiqData;
      try
      {
        this.refreshDataForAllBorrowers();
      }
      catch (Exception ex)
      {
        return ex.Message;
      }
      return "";
    }

    private void chkAutomate_Changed(object sender, EventArgs e)
    {
      if (this.chk_automate.Checked)
        this.loan.SetCurrentField("ANALYZER.X18", "Y");
      else
        this.loan.SetCurrentField("ANALYZER.X18", "N");
    }

    private void btnImport_Click(object sender, EventArgs e)
    {
      string text = this.checkUniqueImportRecords();
      if (text != string.Empty)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, text);
      }
      else if (!this.checkIfAnyRecordSelected())
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "Please select at least one record to import.", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
      }
      else
      {
        this.aiqData.import();
        this.loan.ClearAIQIncomeAnalyzerAlert = true;
        this.loan.AiqIncomeEpassMessageID = this.MessageID;
        this.generateSuccessMessage();
        this.refreshDataForAllBorrowers();
      }
    }

    private string checkUniqueImportRecords()
    {
      string str = "";
      List<string> stringList = new List<string>();
      for (int index1 = 0; index1 < this.loan.GetBorrowerPairs().Length; ++index1)
      {
        for (int index2 = 0; index2 <= 1; ++index2)
        {
          IList<AIQEmploymentData> employmentList = this.aiqData.getEmploymentList((index1 + 1).ToString() + (index2 == 1 ? (object) "C" : (object) "B"));
          int count = employmentList != null ? employmentList.Count : 0;
          HashSet<int> intSet = new HashSet<int>();
          for (int index3 = 0; index3 < count; ++index3)
          {
            AIQEmploymentData aiqEmploymentData = employmentList[index3];
            if (aiqEmploymentData.linkedEncompassVOEBlockNumber > 0)
            {
              if (!intSet.Contains(aiqEmploymentData.linkedEncompassVOEBlockNumber))
                intSet.Add(aiqEmploymentData.linkedEncompassVOEBlockNumber);
              else
                stringList.Add(aiqEmploymentData.employerFullname);
            }
          }
        }
      }
      if (stringList.Count > 0)
        str = "More than one Income Analyzer Employment is being imported into the same Encompass VOE record. Please correct the selection and try again.";
      return str;
    }

    private void refreshDataForAllBorrowers()
    {
      this.panelAIQandVOEData.Controls.Clear();
      this.toImportAll.Checked = false;
      this.isSelectedControls.Clear();
      int num = 0;
      BorrowerPair currentBorrowerPair = this.loan.CurrentBorrowerPair;
      int y = 0;
      BorrowerPair[] borrowerPairs = this.loan.GetBorrowerPairs();
      for (int borrowerPairIndex = borrowerPairs.Length - 1; borrowerPairIndex >= 0; --borrowerPairIndex)
      {
        IncomeBorrowerUIControl borrowerUiControl1 = new IncomeBorrowerUIControl(true, this.loan, this.session, this.aiqData, borrowerPairs[borrowerPairIndex], borrowerPairIndex);
        if (borrowerUiControl1.TotalControls > 0)
        {
          borrowerUiControl1.Location = new Point(0, y);
          this.panelAIQandVOEData.Controls.Add((Control) borrowerUiControl1);
          y += borrowerUiControl1.Controls["panelBorrowerIncome"].Height;
          ++num;
          this.isSelectedControls.AddRange((IEnumerable<Control>) borrowerUiControl1.isSelectedControls);
        }
        IncomeBorrowerUIControl borrowerUiControl2 = new IncomeBorrowerUIControl(false, this.loan, this.session, this.aiqData, borrowerPairs[borrowerPairIndex], borrowerPairIndex);
        if (borrowerUiControl2.TotalControls > 0)
        {
          borrowerUiControl2.Location = new Point(0, y);
          this.panelAIQandVOEData.Controls.Add((Control) borrowerUiControl2);
          y += borrowerUiControl2.Controls["panelBorrowerIncome"].Height;
          ++num;
          this.isSelectedControls.AddRange((IEnumerable<Control>) borrowerUiControl2.isSelectedControls);
        }
      }
      if (num == 0)
        this.labelError.Text = "No information returned.";
      this.panelError.Visible = num == 0;
      this.panelAIQandVOEData.Visible = num > 0;
      this.loan.SetBorrowerPair(currentBorrowerPair);
    }

    private void expandall_Click(object sender, EventArgs e)
    {
      foreach (Control control1 in (ArrangedElementCollection) this.panelAIQandVOEData.Controls)
      {
        if (control1 is IncomeBorrowerUIControl)
        {
          foreach (Control control2 in (ArrangedElementCollection) control1.Controls)
          {
            if (control2 is Panel && control2.Name == "panelBorrowerIncome")
              control2.Visible = true;
          }
        }
      }
    }

    private void collapseall_Click(object sender, EventArgs e)
    {
      foreach (Control control1 in (ArrangedElementCollection) this.panelAIQandVOEData.Controls)
      {
        if (control1 is IncomeBorrowerUIControl)
        {
          foreach (Control control2 in (ArrangedElementCollection) control1.Controls)
          {
            if (control2 is Panel && control2.Name == "panelBorrowerIncome")
              control2.Visible = false;
          }
        }
      }
    }

    private void toImportAll_CheckedChanged(object sender, EventArgs e)
    {
      foreach (CheckBox isSelectedControl in this.isSelectedControls)
      {
        isSelectedControl.Checked = this.toImportAll.Checked;
        isSelectedControl.Refresh();
      }
    }

    private bool checkIfAnyRecordSelected()
    {
      foreach (CheckBox isSelectedControl in this.isSelectedControls)
      {
        if (isSelectedControl.Checked)
          return true;
      }
      return false;
    }

    private void generateSuccessMessage()
    {
      int num1 = 0;
      int num2 = 0;
      int num3 = 0;
      foreach (CheckBox isSelectedControl in this.isSelectedControls)
      {
        if (isSelectedControl.Checked)
        {
          if (isSelectedControl.Tag is AIQEmploymentData)
            ++num1;
          else if (isSelectedControl.Tag is AIQPropertyRentalData)
            ++num2;
          else if (isSelectedControl.Tag is AIQOtherIncomeData)
            ++num3;
        }
      }
      int num4 = (int) Utils.Dialog((IWin32Window) this, "Analyzer Income data has been successfully imported: \n" + (num1 > 0 ? num1.ToString() + " record(s) in the VOE\n" : "") + (num2 > 0 ? num2.ToString() + " record(s) in the VOM\n" : "") + (num3 > 0 ? num3.ToString() + " record(s) in the VOOI\n" : ""));
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.groupContainer2 = new GroupContainer();
      this.chk_automate = new CheckBox();
      this.panelAIQandVOEData = new Panel();
      this.panelHeader = new Panel();
      this.expand = new PictureBox();
      this.collapse = new PictureBox();
      this.label3 = new Label();
      this.toImportAll = new CheckBox();
      this.label2 = new Label();
      this.label1 = new Label();
      this.panelError = new Panel();
      this.labelError = new Label();
      this.btnImport = new Button();
      this.groupContainer2.SuspendLayout();
      this.panelHeader.SuspendLayout();
      ((ISupportInitialize) this.expand).BeginInit();
      ((ISupportInitialize) this.collapse).BeginInit();
      this.panelError.SuspendLayout();
      this.SuspendLayout();
      this.groupContainer2.Controls.Add((Control) this.chk_automate);
      this.groupContainer2.Controls.Add((Control) this.panelAIQandVOEData);
      this.groupContainer2.Controls.Add((Control) this.panelHeader);
      this.groupContainer2.Controls.Add((Control) this.panelError);
      this.groupContainer2.Controls.Add((Control) this.btnImport);
      this.groupContainer2.Dock = DockStyle.Fill;
      this.groupContainer2.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer2.Location = new Point(0, 0);
      this.groupContainer2.Name = "groupContainer2";
      this.groupContainer2.Size = new Size(992, 852);
      this.groupContainer2.TabIndex = 5;
      this.groupContainer2.Text = "Income Comparison";
      this.chk_automate.AutoSize = true;
      this.chk_automate.BackColor = Color.Transparent;
      this.chk_automate.Location = new Point(284, 7);
      this.chk_automate.Name = "chk_automate";
      this.chk_automate.Size = new Size(159, 21);
      this.chk_automate.TabIndex = 45;
      this.chk_automate.Text = "Automate Analyzer Income";
      this.chk_automate.UseVisualStyleBackColor = false;
      this.chk_automate.CheckedChanged += new EventHandler(this.chkAutomate_Changed);
      this.panelAIQandVOEData.AutoScroll = true;
      this.panelAIQandVOEData.AutoSize = true;
      this.panelAIQandVOEData.Dock = DockStyle.Fill;
      this.panelAIQandVOEData.Location = new Point(1, 223);
      this.panelAIQandVOEData.Name = "panelAIQandVOEData";
      this.panelAIQandVOEData.Size = new Size(990, 628);
      this.panelAIQandVOEData.TabIndex = 44;
      this.panelHeader.BackColor = Color.WhiteSmoke;
      this.panelHeader.Controls.Add((Control) this.expand);
      this.panelHeader.Controls.Add((Control) this.collapse);
      this.panelHeader.Controls.Add((Control) this.label3);
      this.panelHeader.Controls.Add((Control) this.toImportAll);
      this.panelHeader.Controls.Add((Control) this.label2);
      this.panelHeader.Controls.Add((Control) this.label1);
      this.panelHeader.Dock = DockStyle.Top;
      this.panelHeader.Location = new Point(1, 143);
      this.panelHeader.Margin = new Padding(4, 5, 4, 5);
      this.panelHeader.Name = "panelHeader";
      this.panelHeader.Size = new Size(990, 80);
      this.panelHeader.TabIndex = 43;
      this.expand.Image = (Image) Resources.expand;
      this.expand.Location = new Point(56, 23);
      this.expand.Margin = new Padding(4, 5, 4, 5);
      this.expand.Name = "expand";
      this.expand.Padding = new Padding(4, 0, 0, 0);
      this.expand.Size = new Size(42, 35);
      this.expand.TabIndex = 10;
      this.expand.TabStop = false;
      this.expand.Click += new EventHandler(this.expandall_Click);
      this.collapse.Image = (Image) Resources.collapse;
      this.collapse.Location = new Point(14, 23);
      this.collapse.Margin = new Padding(4, 5, 4, 5);
      this.collapse.Name = "collapse";
      this.collapse.Padding = new Padding(4, 0, 0, 0);
      this.collapse.Size = new Size(42, 35);
      this.collapse.TabIndex = 9;
      this.collapse.TabStop = false;
      this.collapse.Click += new EventHandler(this.collapseall_Click);
      this.label3.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label3.Location = new Point(756, 3);
      this.label3.Margin = new Padding(4, 0, 4, 0);
      this.label3.Name = "label3";
      this.label3.Size = new Size(128, 72);
      this.label3.TabIndex = 8;
      this.label3.Text = "Select All From Income Analyzer";
      this.label3.TextAlign = ContentAlignment.MiddleLeft;
      this.toImportAll.AutoSize = true;
      this.toImportAll.Font = new Font("Microsoft Sans Serif", 9f);
      this.toImportAll.Location = new Point(892, 34);
      this.toImportAll.Margin = new Padding(4, 5, 4, 5);
      this.toImportAll.MaximumSize = new Size(225, 62);
      this.toImportAll.MinimumSize = new Size(15, 0);
      this.toImportAll.Name = "toImportAll";
      this.toImportAll.Size = new Size(22, 21);
      this.toImportAll.TabIndex = 2;
      this.toImportAll.UseVisualStyleBackColor = true;
      this.toImportAll.CheckedChanged += new EventHandler(this.toImportAll_CheckedChanged);
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Microsoft Sans Serif", 9f);
      this.label2.Location = new Point(538, 18);
      this.label2.Margin = new Padding(4, 0, 4, 0);
      this.label2.Name = "label2";
      this.label2.Size = new Size(125, 22);
      this.label2.TabIndex = 1;
      this.label2.Text = "From Analyzer";
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Microsoft Sans Serif", 9f);
      this.label1.Location = new Point(279, 18);
      this.label1.Margin = new Padding(4, 0, 4, 0);
      this.label1.Name = "label1";
      this.label1.Size = new Size(162, 22);
      this.label1.TabIndex = 0;
      this.label1.Text = "In Loan Application";
      this.panelError.Controls.Add((Control) this.labelError);
      this.panelError.Dock = DockStyle.Top;
      this.panelError.Location = new Point(1, 26);
      this.panelError.Name = "panelError";
      this.panelError.Size = new Size(990, 117);
      this.panelError.TabIndex = 39;
      this.panelError.Visible = false;
      this.labelError.ForeColor = Color.Red;
      this.labelError.Location = new Point(6, 6);
      this.labelError.Name = "labelError";
      this.labelError.Size = new Size(968, 106);
      this.labelError.TabIndex = 0;
      this.labelError.Text = "(Display any error here)";
      this.btnImport.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnImport.Location = new Point(838, 3);
      this.btnImport.Name = "btnImport";
      this.btnImport.Size = new Size(129, 34);
      this.btnImport.TabIndex = 35;
      this.btnImport.Text = "Import Data";
      this.btnImport.UseVisualStyleBackColor = true;
      this.btnImport.Click += new EventHandler(this.btnImport_Click);
      this.AutoScaleDimensions = new SizeF(9f, 20f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.groupContainer2);
      this.Name = nameof (IncomeAnalyzerControl);
      this.Size = new Size(992, 852);
      this.groupContainer2.ResumeLayout(false);
      this.groupContainer2.PerformLayout();
      this.panelHeader.ResumeLayout(false);
      this.panelHeader.PerformLayout();
      ((ISupportInitialize) this.expand).EndInit();
      ((ISupportInitialize) this.collapse).EndInit();
      this.panelError.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
