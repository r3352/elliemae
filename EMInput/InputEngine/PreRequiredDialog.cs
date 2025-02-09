// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.PreRequiredDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.BusinessRules;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
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
  public class PreRequiredDialog : Form
  {
    private const string className = "PreRequiredDialog";
    private static readonly string sw = Tracing.SwDataEngine;
    private Label labelDesc;
    private Button closeBtn;
    private Button refreshBtn;
    private System.ComponentModel.Container components;
    private MissingPrerequisiteException missingException;
    private Label labelCurrentField;
    private string currentFieldID = string.Empty;
    private LoanData loanData;
    private FieldSettings fieldSettings;
    private Button yesBtn;
    private Label labelForFields;
    private Label labelForFields2;
    private GridView listViewFields;
    private static PreRequiredDialog _instance = (PreRequiredDialog) null;
    private string selectedFieldID = "";

    public event EventHandler FieldSelectedDoubleClick;

    public static PreRequiredDialog Instance
    {
      get
      {
        if (PreRequiredDialog._instance == null)
        {
          PreRequiredDialog._instance = new PreRequiredDialog();
          PreRequiredDialog._instance.RefreshButtonVisible = true;
        }
        return PreRequiredDialog._instance;
      }
    }

    public PreRequiredDialog()
    {
      this.InitializeComponent();
      try
      {
        this.fieldSettings = Session.LoanManager.GetFieldSettings();
      }
      catch (Exception ex)
      {
        Tracing.Log(PreRequiredDialog.sw, TraceLevel.Error, nameof (PreRequiredDialog), "PreRequiredDialog: can't load custom fields. Error: " + ex.Message);
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    public string SelectedFieldID => this.selectedFieldID;

    private void InitializeComponent()
    {
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      this.labelDesc = new Label();
      this.closeBtn = new Button();
      this.refreshBtn = new Button();
      this.labelCurrentField = new Label();
      this.yesBtn = new Button();
      this.labelForFields = new Label();
      this.labelForFields2 = new Label();
      this.listViewFields = new GridView();
      this.SuspendLayout();
      this.labelDesc.AutoSize = true;
      this.labelDesc.Location = new Point(13, 12);
      this.labelDesc.Name = "labelDesc";
      this.labelDesc.Size = new Size((int) byte.MaxValue, 13);
      this.labelDesc.TabIndex = 0;
      this.labelDesc.Text = "The fields below must be completed prior to this field:";
      this.closeBtn.DialogResult = DialogResult.Cancel;
      this.closeBtn.Location = new Point(435, 267);
      this.closeBtn.Name = "closeBtn";
      this.closeBtn.Size = new Size(75, 23);
      this.closeBtn.TabIndex = 16;
      this.closeBtn.Text = "&Close";
      this.closeBtn.Click += new EventHandler(this.closeBtn_Click);
      this.refreshBtn.DialogResult = DialogResult.Cancel;
      this.refreshBtn.Location = new Point(351, 267);
      this.refreshBtn.Name = "refreshBtn";
      this.refreshBtn.Size = new Size(75, 23);
      this.refreshBtn.TabIndex = 17;
      this.refreshBtn.Text = "&Refresh";
      this.refreshBtn.Click += new EventHandler(this.refreshBtn_Click);
      this.labelCurrentField.Location = new Point(269, 12);
      this.labelCurrentField.Name = "labelCurrentField";
      this.labelCurrentField.Size = new Size(241, 16);
      this.labelCurrentField.TabIndex = 18;
      this.yesBtn.DialogResult = DialogResult.Cancel;
      this.yesBtn.Location = new Point(267, 267);
      this.yesBtn.Name = "yesBtn";
      this.yesBtn.Size = new Size(75, 23);
      this.yesBtn.TabIndex = 20;
      this.yesBtn.Text = "&Yes";
      this.yesBtn.Visible = false;
      this.yesBtn.Click += new EventHandler(this.yesBtn_Click);
      this.labelForFields.Location = new Point(12, 12);
      this.labelForFields.Name = "labelForFields";
      this.labelForFields.Size = new Size(498, 36);
      this.labelForFields.TabIndex = 21;
      this.labelForFields.Text = "The following fields have been locked or hidden by your system administrator. If you apply the loan template, these fields will not be changed in your loan.";
      this.labelForFields.Visible = false;
      this.labelForFields2.AutoSize = true;
      this.labelForFields2.Location = new Point(12, 48);
      this.labelForFields2.Name = "labelForFields2";
      this.labelForFields2.Size = new Size(129, 13);
      this.labelForFields2.TabIndex = 22;
      this.labelForFields2.Text = "Do you want to continue?";
      this.labelForFields2.Visible = false;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Field ID";
      gvColumn1.Width = 109;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.SpringToFit = true;
      gvColumn2.Text = "Description";
      gvColumn2.Width = 387;
      this.listViewFields.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.listViewFields.Location = new Point(12, 32);
      this.listViewFields.Name = "listViewFields";
      this.listViewFields.Size = new Size(498, 220);
      this.listViewFields.TabIndex = 23;
      this.listViewFields.DoubleClick += new EventHandler(this.listViewFields_DoubleClick);
      this.AcceptButton = (IButtonControl) this.closeBtn;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(522, 302);
      this.Controls.Add((Control) this.listViewFields);
      this.Controls.Add((Control) this.yesBtn);
      this.Controls.Add((Control) this.labelCurrentField);
      this.Controls.Add((Control) this.refreshBtn);
      this.Controls.Add((Control) this.closeBtn);
      this.Controls.Add((Control) this.labelDesc);
      this.Controls.Add((Control) this.labelForFields2);
      this.Controls.Add((Control) this.labelForFields);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.Name = nameof (PreRequiredDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Encompass";
      this.Load += new EventHandler(this.PreRequiredDialog_Load);
      this.Closing += new CancelEventHandler(this.PreRequiredDialog_Closing);
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    public void ChangeToRegularFieldList()
    {
      this.labelForFields.Visible = true;
      this.labelForFields2.Visible = true;
      this.yesBtn.Visible = true;
      this.labelDesc.Visible = false;
      this.labelCurrentField.Visible = false;
      this.refreshBtn.Visible = false;
      this.closeBtn.Text = "&Cancel";
      this.yesBtn.Left = this.refreshBtn.Left;
      this.listViewFields.Top += 36;
      this.listViewFields.Height -= 36;
    }

    public bool RefreshButtonVisible
    {
      get => this.refreshBtn.Visible;
      set => this.refreshBtn.Visible = value;
    }

    public void InitForm(
      LoanData loanData,
      MissingPrerequisiteException missingException,
      string currentFieldID,
      string[] preRequiredFields)
    {
      if (currentFieldID != string.Empty)
      {
        if (currentFieldID != null && currentFieldID.StartsWith("BUTTON_", StringComparison.CurrentCultureIgnoreCase))
          this.labelDesc.Text = "The fields below must be completed prior to this function:";
        else
          this.labelDesc.Text = "The fields below must be completed prior to this field:";
        this.currentFieldID = currentFieldID;
        this.labelCurrentField.Text = this.currentFieldID;
      }
      this.labelCurrentField.Left = this.labelDesc.Left + this.labelDesc.Width;
      if (loanData != null)
        this.loanData = loanData;
      if (missingException != null)
        this.missingException = missingException;
      this.listViewFields.Items.Clear();
      string empty = string.Empty;
      for (int index = 0; index < preRequiredFields.Length; ++index)
        this.listViewFields.Items.Add(new GVItem(preRequiredFields[index])
        {
          SubItems = {
            (object) this.getFieldDescription(preRequiredFields[index])
          },
          Tag = (object) preRequiredFields[index]
        });
    }

    private void refreshBtn_Click(object sender, EventArgs e)
    {
      BusinessRuleCheck businessRuleCheck = new BusinessRuleCheck();
      if (businessRuleCheck.HasPrerequiredFields(this.loanData, this.currentFieldID))
        this.InitForm((LoanData) null, (MissingPrerequisiteException) null, string.Empty, businessRuleCheck.PrerequiredFields);
      else
        this.listViewFields.Items.Clear();
    }

    private void closeBtn_Click(object sender, EventArgs e)
    {
      this.Close();
      this.Dispose();
    }

    private void listViewFields_DoubleClick(object sender, EventArgs e)
    {
      if (this.listViewFields.SelectedItems.Count == 0)
      {
        this.selectedFieldID = "";
      }
      else
      {
        this.selectedFieldID = this.listViewFields.SelectedItems[0].Text;
        if (this.FieldSelectedDoubleClick == null)
          return;
        this.FieldSelectedDoubleClick(sender, e);
      }
    }

    private void PreRequiredDialog_Load(object sender, EventArgs e)
    {
      Tracing.Log(PreRequiredDialog.sw, TraceLevel.Verbose, nameof (PreRequiredDialog), "Load");
      Session.LoanDataMgr.LoanClosing += new EventHandler(this.closeBtn_Click);
    }

    private void PreRequiredDialog_Closing(object sender, CancelEventArgs e)
    {
      Tracing.Log(PreRequiredDialog.sw, TraceLevel.Verbose, nameof (PreRequiredDialog), "Closing");
      if (Session.LoanDataMgr != null)
        Session.LoanDataMgr.LoanClosing -= new EventHandler(this.closeBtn_Click);
      PreRequiredDialog._instance = (PreRequiredDialog) null;
    }

    private string getFieldDescription(string fieldID)
    {
      return EncompassFields.GetDescription(fieldID, this.fieldSettings);
    }

    private void yesBtn_Click(object sender, EventArgs e) => this.DialogResult = DialogResult.OK;
  }
}
