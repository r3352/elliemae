// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.VirtualFieldSelectForm
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class VirtualFieldSelectForm : Form
  {
    private string specialFieldID;
    private LoanReportFieldDef selectedField;
    private IContainer components;
    private GridView gvLockFields;
    private Button btnSelect;
    private Button btnCancel;

    public VirtualFieldSelectForm(string specialFieldID)
    {
      this.specialFieldID = specialFieldID;
      this.InitializeComponent();
      this.initForm();
    }

    public LoanReportFieldDef SelectedField => this.selectedField;

    private void initForm()
    {
      this.gvLockFields.BeginUpdate();
      this.gvLockFields.Items.Clear();
      if (this.specialFieldID == LoanReportFieldDef.RateLockFieldSelector().FieldID)
      {
        foreach (FieldDefinition lockRequestField in EncompassFields.GetAllLockRequestFields(Session.LoanManager.GetFieldSettings()))
          this.addSourceField(new LoanReportFieldDef(lockRequestField), false);
        this.Text = "Select Rate Lock Field";
      }
      else if (this.specialFieldID == LoanReportFieldDef.InterimFieldSelector().FieldID)
      {
        foreach (FieldDefinition fieldDef in InterimServicingFields.All)
          this.addSourceField(new LoanReportFieldDef(fieldDef), false);
        this.Text = "Select Interim Servicing Field";
      }
      else if (this.specialFieldID == LoanReportFieldDef.GFEDisclosedFieldSelector().FieldID)
      {
        foreach (FieldDefinition fieldDef in LastDisclosedGFESnapshotFields.All)
          this.addSourceField(new LoanReportFieldDef(fieldDef), false);
        this.Text = "Select GFE Disclosure Tracking Field";
      }
      else if (this.specialFieldID == LoanReportFieldDef.LEDisclosedFieldSelector().FieldID)
      {
        foreach (FieldDefinition fieldDef in LastDisclosedLESnapshotFields.All)
          this.addSourceField(new LoanReportFieldDef(fieldDef), false);
        this.Text = "Select LE Disclosure Tracking Field";
      }
      else if (this.specialFieldID == LoanReportFieldDef.AuditTrailFieldSelector().FieldID)
      {
        foreach (LoanReportFieldDef trailFieldDefinition in LoanReportFieldDefs.GetAuditTrailFieldDefinitions())
          this.addSourceField(trailFieldDefinition, false);
        this.Text = "Select Audit Trail Field";
      }
      this.gvLockFields.EndUpdate();
    }

    private void addSourceField(LoanReportFieldDef field, bool selected)
    {
      GVItem gvItem = new GVItem(field.FieldID);
      gvItem.SubItems.Add((object) field.Description);
      string str = "String";
      if (field.ReportingDatabaseColumnType == ReportingDatabaseColumnType.Date || field.ReportingDatabaseColumnType == ReportingDatabaseColumnType.DateTime)
        str = "Date";
      else if (field.ReportingDatabaseColumnType == ReportingDatabaseColumnType.Numeric)
        str = "Numeric";
      gvItem.SubItems.Add((object) str);
      gvItem.Tag = (object) field;
      this.gvLockFields.Items.Add(gvItem);
      if (!selected)
        return;
      gvItem.Selected = true;
      this.gvLockFields.EnsureVisible(gvItem.Index);
    }

    private void btnSelect_Click(object sender, EventArgs e)
    {
      if (this.gvLockFields.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You have to select a field first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        this.selectedField = (LoanReportFieldDef) this.gvLockFields.SelectedItems[0].Tag;
        this.DialogResult = DialogResult.OK;
      }
    }

    private void gvLockFields_DoubleClick(object sender, EventArgs e)
    {
      if (this.gvLockFields.SelectedItems.Count <= 0)
        return;
      this.btnSelect_Click(sender, e);
    }

    private void VirtualFieldSelectForm_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar != '\u001B')
        return;
      this.DialogResult = DialogResult.Cancel;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      this.gvLockFields = new GridView();
      this.btnSelect = new Button();
      this.btnCancel = new Button();
      this.SuspendLayout();
      this.gvLockFields.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Field ID";
      gvColumn1.Width = 118;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.SpringToFit = true;
      gvColumn2.Text = "Description";
      gvColumn2.Width = 270;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "Type";
      gvColumn3.Width = 100;
      this.gvLockFields.Columns.AddRange(new GVColumn[3]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3
      });
      this.gvLockFields.Location = new Point(10, 10);
      this.gvLockFields.Name = "gvLockFields";
      this.gvLockFields.Size = new Size(490, 492);
      this.gvLockFields.TabIndex = 3;
      this.gvLockFields.DoubleClick += new EventHandler(this.gvLockFields_DoubleClick);
      this.btnSelect.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnSelect.Location = new Point(517, 10);
      this.btnSelect.Name = "btnSelect";
      this.btnSelect.Size = new Size(75, 22);
      this.btnSelect.TabIndex = 4;
      this.btnSelect.Text = "&Select";
      this.btnSelect.UseVisualStyleBackColor = true;
      this.btnSelect.Click += new EventHandler(this.btnSelect_Click);
      this.btnCancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(517, 35);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 5;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(606, 512);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnSelect);
      this.Controls.Add((Control) this.gvLockFields);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MinimizeBox = false;
      this.Name = nameof (VirtualFieldSelectForm);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Select Rate Lock Field";
      this.KeyPress += new KeyPressEventHandler(this.VirtualFieldSelectForm_KeyPress);
      this.ResumeLayout(false);
    }
  }
}
