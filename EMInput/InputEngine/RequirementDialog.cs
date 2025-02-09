// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.RequirementDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class RequirementDialog : Form
  {
    private const string className = "RequirementDialog";
    private static readonly string sw = Tracing.SwDataEngine;
    private Button closeBtn;
    private Button refreshBtn;
    private System.ComponentModel.Container components;
    private LoanData loanData;
    private MilestoneLog msLog;
    private MilestoneLog currLog;
    private GroupContainer groupContainer1;
    private GridView listViewFields;
    private Button btnBackMS;
    private Button btnGoTo;
    private static RequirementDialog _instance = (RequirementDialog) null;
    private string selectedFieldID = "";
    private DocumentLog selectedDocument;

    public static RequirementDialog Instance
    {
      get
      {
        if (RequirementDialog._instance == null)
          RequirementDialog._instance = new RequirementDialog();
        return RequirementDialog._instance;
      }
    }

    public RequirementDialog() => this.InitializeComponent();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    public string SelectedFieldID => this.selectedFieldID;

    public DocumentLog SelectedDocument => this.selectedDocument;

    private void InitializeComponent()
    {
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      this.closeBtn = new Button();
      this.refreshBtn = new Button();
      this.groupContainer1 = new GroupContainer();
      this.listViewFields = new GridView();
      this.btnBackMS = new Button();
      this.btnGoTo = new Button();
      this.groupContainer1.SuspendLayout();
      this.SuspendLayout();
      this.closeBtn.DialogResult = DialogResult.Cancel;
      this.closeBtn.Location = new Point(371, 447);
      this.closeBtn.Name = "closeBtn";
      this.closeBtn.Size = new Size(75, 23);
      this.closeBtn.TabIndex = 16;
      this.closeBtn.Text = "&Close";
      this.closeBtn.Click += new EventHandler(this.closeBtn_Click);
      this.refreshBtn.DialogResult = DialogResult.Cancel;
      this.refreshBtn.Location = new Point(168, 447);
      this.refreshBtn.Name = "refreshBtn";
      this.refreshBtn.Size = new Size(75, 23);
      this.refreshBtn.TabIndex = 17;
      this.refreshBtn.Text = "&Refresh";
      this.refreshBtn.Click += new EventHandler(this.refreshBtn_Click);
      this.groupContainer1.Controls.Add((Control) this.listViewFields);
      this.groupContainer1.Location = new Point(8, 8);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(438, 433);
      this.groupContainer1.TabIndex = 18;
      this.groupContainer1.Text = "Missing Required Fields";
      this.listViewFields.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Field ID";
      gvColumn1.Width = 160;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.SpringToFit = true;
      gvColumn2.Text = "Field Description";
      gvColumn2.Width = 276;
      this.listViewFields.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.listViewFields.Dock = DockStyle.Fill;
      this.listViewFields.Location = new Point(1, 26);
      this.listViewFields.Name = "listViewFields";
      this.listViewFields.Size = new Size(436, 406);
      this.listViewFields.TabIndex = 0;
      this.listViewFields.SelectedIndexChanged += new EventHandler(this.listViewFields_SelectedIndexChanged);
      this.listViewFields.ItemDoubleClick += new GVItemEventHandler(this.listViewFields_ItemDoubleClick);
      this.btnBackMS.DialogResult = DialogResult.Cancel;
      this.btnBackMS.Location = new Point(249, 447);
      this.btnBackMS.Name = "btnBackMS";
      this.btnBackMS.Size = new Size(116, 23);
      this.btnBackMS.TabIndex = 19;
      this.btnBackMS.Text = "&Back to Milestone";
      this.btnBackMS.Click += new EventHandler(this.btnBackMS_Click);
      this.btnGoTo.DialogResult = DialogResult.Cancel;
      this.btnGoTo.Location = new Point(87, 447);
      this.btnGoTo.Name = "btnGoTo";
      this.btnGoTo.Size = new Size(75, 23);
      this.btnGoTo.TabIndex = 20;
      this.btnGoTo.Text = "&Go to Field";
      this.btnGoTo.Click += new EventHandler(this.btnGoTo_Click);
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(455, 482);
      this.Controls.Add((Control) this.btnGoTo);
      this.Controls.Add((Control) this.btnBackMS);
      this.Controls.Add((Control) this.groupContainer1);
      this.Controls.Add((Control) this.refreshBtn);
      this.Controls.Add((Control) this.closeBtn);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (RequirementDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Go To Fields";
      this.Load += new EventHandler(this.RequirementDialog_Load);
      this.Closing += new CancelEventHandler(this.RequirementDialog_Closing);
      this.groupContainer1.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    public void InitForm(
      LoanData loanData,
      MilestoneLog currLog,
      MilestoneLog msLog,
      FieldMilestonePair[] requiredFields)
    {
      if (msLog != null)
        this.msLog = msLog;
      if (loanData != null)
        this.loanData = loanData;
      if (currLog != null)
        this.currLog = currLog;
      string str = this.msLog.Stage;
      if (string.Compare(this.msLog.Stage, "Started", true) == 0)
        str = "File Started";
      LogList logList = (LogList) null;
      try
      {
        logList = this.loanData.GetLogList();
      }
      catch (Exception ex)
      {
      }
      this.listViewFields.Items.Clear();
      if (requiredFields.Length != 0)
      {
        string empty = string.Empty;
        for (int index = 0; index < requiredFields.Length; ++index)
        {
          string fieldDescription = this.getFieldDescription(requiredFields[index].FieldID);
          if (fieldDescription != null)
          {
            string simpleField = this.loanData.GetSimpleField(requiredFields[index].FieldID);
            if (!((simpleField ?? "") != string.Empty) || !(simpleField != "//"))
              this.listViewFields.Items.Add(new GVItem(requiredFields[index].FieldID)
              {
                SubItems = {
                  (object) fieldDescription
                },
                Tag = (object) requiredFields[index]
              });
          }
        }
      }
      this.listViewFields_SelectedIndexChanged((object) null, (EventArgs) null);
    }

    private void refreshBtn_Click(object sender, EventArgs e)
    {
      BusinessRuleCheck businessRuleCheck = new BusinessRuleCheck();
      if (businessRuleCheck.HasRequirement(this.loanData, this.msLog))
        this.InitForm(this.loanData, this.currLog, this.msLog, businessRuleCheck.RequiredFields);
      else
        this.listViewFields.Items.Clear();
    }

    private void closeBtn_Click(object sender, EventArgs e)
    {
      this.Close();
      this.Dispose();
    }

    private void RequirementDialog_Load(object sender, EventArgs e)
    {
      Tracing.Log(RequirementDialog.sw, TraceLevel.Verbose, nameof (RequirementDialog), "Load");
      Session.LoanDataMgr.LoanClosing += new EventHandler(this.closeBtn_Click);
    }

    private void RequirementDialog_Closing(object sender, CancelEventArgs e)
    {
      Tracing.Log(RequirementDialog.sw, TraceLevel.Verbose, nameof (RequirementDialog), "Closing");
      if (Session.LoanDataMgr != null)
        Session.LoanDataMgr.LoanClosing -= new EventHandler(this.closeBtn_Click);
      RequirementDialog._instance = (RequirementDialog) null;
    }

    private string getFieldDescription(string fieldID)
    {
      return fieldID.ToLower().StartsWith("cx.") && EncompassFields.GetField(fieldID, this.loanData.Settings.FieldSettings) == null ? (string) null : EncompassFields.GetDescription(fieldID, this.loanData.Settings.FieldSettings);
    }

    private void btnGoTo_Click(object sender, EventArgs e)
    {
      this.selectedFieldID = this.listViewFields.SelectedItems[0].Text;
      if (this.selectedFieldID == string.Empty)
        return;
      Session.Application.GetService<ILoanEditor>().GoToField(this.selectedFieldID);
    }

    private void btnBackMS_Click(object sender, EventArgs e)
    {
      Session.Application.GetService<ILoanEditor>().ShowMilestoneWorksheet(this.currLog);
    }

    private void listViewFields_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      if (e == null)
        return;
      this.selectedFieldID = e.Item.Text;
      if (this.selectedFieldID == string.Empty || VirtualFields.Contains(this.selectedFieldID))
        return;
      Session.Application.GetService<ILoanEditor>().GoToField(this.selectedFieldID);
    }

    private void listViewFields_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.listViewFields.SelectedItems.Count == 1)
      {
        this.selectedFieldID = this.listViewFields.SelectedItems[0].Text;
        if (this.selectedFieldID != string.Empty && VirtualFields.Contains(this.selectedFieldID))
        {
          this.btnGoTo.Enabled = false;
          return;
        }
      }
      this.btnGoTo.Enabled = this.listViewFields.SelectedItems.Count == 1;
    }
  }
}
