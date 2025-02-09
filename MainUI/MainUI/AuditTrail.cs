// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.MainUI.AuditTrail
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Reporting;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.MainUI
{
  public class AuditTrail : UserControl, IOnlineHelpTarget
  {
    private const string className = "AuditTrail";
    private ResourceManager UIresources;
    private Dictionary<string, LoanXDBField> fieldList = new Dictionary<string, LoanXDBField>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
    private IContainer components;
    private Label label1;
    private TextBox txtFieldID;
    private Button btnShow;
    private Label lblFieldDesc;
    private GroupContainer groupContainer1;
    private StandardIconButton siBtnSearch;
    private StandardIconButton btnPrint;
    private ToolTip toolTip1;
    private GridView gvList;
    private BorderPanel borderPanel1;
    private GroupContainer groupContainer2;
    private BorderPanel borderPanel2;

    public AuditTrail()
    {
      this.InitializeComponent();
      this.setFieldList();
      this.UIresources = new ResourceManager(typeof (AuditTrail));
      this.Dock = DockStyle.Fill;
    }

    private void btnShow_Click(object sender, EventArgs e)
    {
      this.gvList.Items.Clear();
      this.lblFieldDesc.Text = "";
      string str = this.txtFieldID.Text.Trim();
      if (str == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select a field ID", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        if (!this.checkSelectedFieldID(str))
          return;
        foreach (AuditRecord auditRecord in Session.LoanManager.GetAuditRecords(Session.LoanData.GUID, str))
          this.gvList.Items.Add(new GVItem(auditRecord.ModifiedDateTime.ToString("G"))
          {
            SubItems = {
              (object) auditRecord.UserID,
              (object) auditRecord.FirstName,
              (object) auditRecord.LastName,
              (object) Session.LoanData.FormatValue(string.Concat(auditRecord.DataValue), Session.LoanData.GetFormat(str))
            },
            Tag = (object) auditRecord
          });
        this.lblFieldDesc.Text = this.fieldList[this.txtFieldID.Text.Trim()].Description;
      }
    }

    private void btnPrint_Click(object sender, EventArgs e)
    {
      AuditRecord[] auditRecords = this.getAuditRecords();
      ExcelHandler excelHandler = new ExcelHandler();
      excelHandler.AddHeaderColumn("Date", "m/d/yyyy hh:mm");
      excelHandler.AddHeaderColumn("UserID");
      excelHandler.AddHeaderColumn("First Name");
      excelHandler.AddHeaderColumn("Last Name");
      excelHandler.AddHeaderColumn("New Value");
      string id = this.txtFieldID.Text.Trim();
      foreach (AuditRecord auditRecord in auditRecords)
      {
        string[] data = new string[5]
        {
          auditRecord.ModifiedDateTime.ToString("g"),
          auditRecord.UserID,
          auditRecord.FirstName,
          auditRecord.LastName,
          Session.LoanData.FormatValue(string.Concat(auditRecord.DataValue), Session.LoanData.GetFormat(id))
        };
        excelHandler.AddDataRow(data);
      }
      excelHandler.CreateExcel();
    }

    private AuditRecord[] getAuditRecords()
    {
      List<AuditRecord> auditRecordList = new List<AuditRecord>();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvList.Items)
        auditRecordList.Add((AuditRecord) gvItem.Tag);
      return auditRecordList.ToArray();
    }

    private void btnExport_Click(object sender, EventArgs e)
    {
      AuditRecord[] auditRecords = this.getAuditRecords();
      ExcelHandler excelHandler = new ExcelHandler();
      excelHandler.AddHeaderColumn("Date", "m/d/yyyy hh:mm");
      excelHandler.AddHeaderColumn("UserID");
      excelHandler.AddHeaderColumn("First Name");
      excelHandler.AddHeaderColumn("Last Name");
      excelHandler.AddHeaderColumn("New Value");
      string id = this.txtFieldID.Text.Trim();
      foreach (AuditRecord auditRecord in auditRecords)
      {
        string[] data = new string[5]
        {
          auditRecord.ModifiedDateTime.ToString("g"),
          auditRecord.UserID,
          auditRecord.FirstName,
          auditRecord.LastName,
          Session.LoanData.FormatValue(string.Concat(auditRecord.DataValue), Session.LoanData.GetFormat(id))
        };
        excelHandler.AddDataRow(data);
      }
      excelHandler.Export();
    }

    public string GetHelpTargetName() => nameof (AuditTrail);

    private void setFieldList()
    {
      this.fieldList.Clear();
      LoanXDBField[] trailLoanXdbField = Session.LoanManager.GetAuditTrailLoanXDBField();
      for (int index = 0; index < trailLoanXdbField.Length; ++index)
        this.fieldList.Add(trailLoanXdbField[index].FieldIDWithCoMortgagor.Replace("*", ""), trailLoanXdbField[index]);
    }

    private bool checkSelectedFieldID(string fieldID)
    {
      if (!this.fieldList.ContainsKey(fieldID))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "This field is not an audit field.  Contact your administrator for more details.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      LoanXDBField field = this.fieldList[fieldID];
      if (Session.UserInfo.IsSuperAdministrator() || Session.LoanDataMgr.GetFieldAccessRights(field.FieldID) != BizRule.FieldAccessRight.Hide)
        return true;
      int num1 = (int) Utils.Dialog((IWin32Window) this, "You do not have access to this data field based on the field access rule.  Contact your administrator for more details.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      return false;
    }

    private void pictureBoxSearch_Click(object sender, EventArgs e)
    {
      List<LoanXDBField> loanXdbFieldList = new List<LoanXDBField>();
      loanXdbFieldList.AddRange((IEnumerable<LoanXDBField>) this.fieldList.Values);
      FindAuditTrailField findAuditTrailField = new FindAuditTrailField(loanXdbFieldList.ToArray(), this.txtFieldID.Text.Trim());
      if (DialogResult.OK != findAuditTrailField.ShowDialog((IWin32Window) this))
        return;
      this.txtFieldID.Text = findAuditTrailField.SelectedField.FieldIDWithCoMortgagor.Replace("*", "");
      this.btnShow_Click((object) null, (EventArgs) null);
    }

    private void txtFieldID_KeyUp(object sender, KeyEventArgs e)
    {
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      GVColumn gvColumn5 = new GVColumn();
      this.label1 = new Label();
      this.txtFieldID = new TextBox();
      this.btnShow = new Button();
      this.lblFieldDesc = new Label();
      this.groupContainer1 = new GroupContainer();
      this.borderPanel1 = new BorderPanel();
      this.groupContainer2 = new GroupContainer();
      this.gvList = new GridView();
      this.btnPrint = new StandardIconButton();
      this.borderPanel2 = new BorderPanel();
      this.siBtnSearch = new StandardIconButton();
      this.toolTip1 = new ToolTip(this.components);
      this.groupContainer1.SuspendLayout();
      this.borderPanel1.SuspendLayout();
      this.groupContainer2.SuspendLayout();
      ((ISupportInitialize) this.btnPrint).BeginInit();
      this.borderPanel2.SuspendLayout();
      ((ISupportInitialize) this.siBtnSearch).BeginInit();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(5, 10);
      this.label1.Name = "label1";
      this.label1.Size = new Size(41, 14);
      this.label1.TabIndex = 0;
      this.label1.Text = "Field ID";
      this.txtFieldID.Location = new Point(53, 7);
      this.txtFieldID.Name = "txtFieldID";
      this.txtFieldID.Size = new Size(282, 20);
      this.txtFieldID.TabIndex = 1;
      this.btnShow.BackColor = SystemColors.Control;
      this.btnShow.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnShow.Location = new Point(361, 5);
      this.btnShow.Name = "btnShow";
      this.btnShow.Size = new Size(46, 22);
      this.btnShow.TabIndex = 2;
      this.btnShow.Text = "Show";
      this.btnShow.UseVisualStyleBackColor = true;
      this.btnShow.Click += new EventHandler(this.btnShow_Click);
      this.lblFieldDesc.AutoSize = true;
      this.lblFieldDesc.Location = new Point(78, 360);
      this.lblFieldDesc.Name = "lblFieldDesc";
      this.lblFieldDesc.Size = new Size(0, 13);
      this.lblFieldDesc.TabIndex = 4;
      this.groupContainer1.Controls.Add((Control) this.borderPanel1);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.Font = new Font("Arial", 8.25f);
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(729, 548);
      this.groupContainer1.TabIndex = 49;
      this.groupContainer1.Text = "Audit Trail";
      this.borderPanel1.Borders = AnchorStyles.None;
      this.borderPanel1.Controls.Add((Control) this.groupContainer2);
      this.borderPanel1.Controls.Add((Control) this.borderPanel2);
      this.borderPanel1.Dock = DockStyle.Fill;
      this.borderPanel1.Location = new Point(1, 26);
      this.borderPanel1.Name = "borderPanel1";
      this.borderPanel1.Size = new Size(727, 521);
      this.borderPanel1.TabIndex = 52;
      this.groupContainer2.Borders = AnchorStyles.Top;
      this.groupContainer2.Controls.Add((Control) this.gvList);
      this.groupContainer2.Controls.Add((Control) this.btnPrint);
      this.groupContainer2.Dock = DockStyle.Fill;
      this.groupContainer2.Location = new Point(0, 34);
      this.groupContainer2.Name = "groupContainer2";
      this.groupContainer2.Size = new Size(727, 487);
      this.groupContainer2.TabIndex = 1;
      this.groupContainer2.Text = "Description";
      this.gvList.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Date";
      gvColumn1.Width = 120;
      gvColumn1.SortMethod = GVSortMethod.DateTime;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "User ID";
      gvColumn2.Width = 100;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "First Name";
      gvColumn3.Width = 100;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column4";
      gvColumn4.Text = "Last Name";
      gvColumn4.Width = 100;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column5";
      gvColumn5.Text = "New Value";
      gvColumn5.Width = 120;
      this.gvList.Columns.AddRange(new GVColumn[5]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5
      });
      this.gvList.Dock = DockStyle.Fill;
      this.gvList.Location = new Point(0, 26);
      this.gvList.Name = "gvList";
      this.gvList.Size = new Size(727, 461);
      this.gvList.TabIndex = 51;
      this.btnPrint.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnPrint.BackColor = Color.Transparent;
      this.btnPrint.Location = new Point(707, 6);
      this.btnPrint.Name = "btnPrint";
      this.btnPrint.Size = new Size(16, 16);
      this.btnPrint.StandardButtonType = StandardIconButton.ButtonType.ExcelButton;
      this.btnPrint.TabIndex = 49;
      this.btnPrint.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnPrint, "Export to Excel");
      this.btnPrint.Click += new EventHandler(this.btnPrint_Click);
      this.borderPanel2.Borders = AnchorStyles.None;
      this.borderPanel2.Controls.Add((Control) this.label1);
      this.borderPanel2.Controls.Add((Control) this.txtFieldID);
      this.borderPanel2.Controls.Add((Control) this.siBtnSearch);
      this.borderPanel2.Controls.Add((Control) this.btnShow);
      this.borderPanel2.Dock = DockStyle.Top;
      this.borderPanel2.Location = new Point(0, 0);
      this.borderPanel2.Name = "borderPanel2";
      this.borderPanel2.Size = new Size(727, 34);
      this.borderPanel2.TabIndex = 0;
      this.siBtnSearch.BackColor = Color.Transparent;
      this.siBtnSearch.Location = new Point(340, 8);
      this.siBtnSearch.Name = "siBtnSearch";
      this.siBtnSearch.Size = new Size(16, 16);
      this.siBtnSearch.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.siBtnSearch.TabIndex = 50;
      this.siBtnSearch.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.siBtnSearch, "Find Fields");
      this.siBtnSearch.Click += new EventHandler(this.pictureBoxSearch_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.groupContainer1);
      this.Controls.Add((Control) this.lblFieldDesc);
      this.Name = nameof (AuditTrail);
      this.Size = new Size(729, 548);
      this.groupContainer1.ResumeLayout(false);
      this.borderPanel1.ResumeLayout(false);
      this.groupContainer2.ResumeLayout(false);
      ((ISupportInitialize) this.btnPrint).EndInit();
      this.borderPanel2.ResumeLayout(false);
      this.borderPanel2.PerformLayout();
      ((ISupportInitialize) this.siBtnSearch).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
