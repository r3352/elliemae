// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.DynamicDataManagement.DataTableReference
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Bpm;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.DynamicDataManagement
{
  public class DataTableReference : Form
  {
    private DDMDataTable _dataTable;
    private string _dataTableName;
    private IContainer components;
    private GroupContainer gcBaseRate;
    private GridView listDataTableRefs;
    private GradientPanel gradientPanel2;
    private Label lblSubTitle;
    private Button btnOk;
    private Button btnCancel;

    private new DialogResult DialogResult { get; set; }

    public DataTableReference()
    {
      this.InitializeComponent();
      this.DialogResult = DialogResult.Cancel;
    }

    public DataTableReference(DDMDataTable dataTable)
      : this()
    {
      this._dataTable = dataTable;
      this.InitializeReferenceList();
    }

    public DataTableReference(string dataTableName)
      : this()
    {
      this._dataTableName = dataTableName;
      this.InitializeReferenceList();
    }

    private void InitializeReferenceList()
    {
      List<DDMDataTableReference> dataTableReferences = ((DDMDataTableBpmManager) Session.BPM.GetBpmManager(BpmCategory.DDMDataTables)).GetDDMDataTableReferences(this._dataTableName);
      this.listDataTableRefs.Items.Clear();
      if (dataTableReferences == null)
        return;
      foreach (DDMDataTableReference dataTableReference in dataTableReferences)
        this.listDataTableRefs.Items.Add(new GVItem(((FieldSearchRuleType) dataTableReference.RuleType).ToString())
        {
          SubItems = {
            (object) dataTableReference.RuleName,
            (object) dataTableReference.ScenarioName,
            (object) dataTableReference.ReferenceCount
          },
          Tag = (object) dataTableReference
        });
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
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
      GVColumn gvColumn4 = new GVColumn();
      this.btnOk = new Button();
      this.btnCancel = new Button();
      this.gradientPanel2 = new GradientPanel();
      this.lblSubTitle = new Label();
      this.gcBaseRate = new GroupContainer();
      this.listDataTableRefs = new GridView();
      this.gradientPanel2.SuspendLayout();
      this.gcBaseRate.SuspendLayout();
      this.SuspendLayout();
      this.btnOk.DialogResult = DialogResult.OK;
      this.btnOk.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnOk.Location = new Point(616, 487);
      this.btnOk.Name = "btnOk";
      this.btnOk.Size = new Size(75, 22);
      this.btnOk.TabIndex = 8;
      this.btnOk.Text = "OK";
      this.btnOk.UseVisualStyleBackColor = false;
      this.btnOk.Click += new EventHandler(this.btnOk_Click);
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnCancel.Location = new Point(694, 487);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 9;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = false;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.gradientPanel2.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gradientPanel2.Controls.Add((Control) this.lblSubTitle);
      this.gradientPanel2.Dock = DockStyle.Top;
      this.gradientPanel2.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel2.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel2.GradientPaddingColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      this.gradientPanel2.Location = new Point(0, 0);
      this.gradientPanel2.Name = "gradientPanel2";
      this.gradientPanel2.Size = new Size(774, 31);
      this.gradientPanel2.Style = GradientPanel.PanelStyle.PageSubHeader;
      this.gradientPanel2.TabIndex = 7;
      this.lblSubTitle.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.lblSubTitle.AutoEllipsis = true;
      this.lblSubTitle.BackColor = Color.Transparent;
      this.lblSubTitle.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblSubTitle.Location = new Point(12, 9);
      this.lblSubTitle.Name = "lblSubTitle";
      this.lblSubTitle.Size = new Size(751, 14);
      this.lblSubTitle.TabIndex = 0;
      this.lblSubTitle.Text = "The Rules listed below refers to this Data table. Please note that updating the Data table will also update these existing references.";
      this.lblSubTitle.TextAlign = ContentAlignment.MiddleLeft;
      this.gcBaseRate.Borders = AnchorStyles.Top;
      this.gcBaseRate.Controls.Add((Control) this.listDataTableRefs);
      this.gcBaseRate.HeaderForeColor = SystemColors.ControlText;
      this.gcBaseRate.Location = new Point(0, 31);
      this.gcBaseRate.Name = "gcBaseRate";
      this.gcBaseRate.Size = new Size(773, 441);
      this.gcBaseRate.TabIndex = 6;
      this.gcBaseRate.Text = "Data Tables List";
      this.listDataTableRefs.AutoHeight = true;
      this.listDataTableRefs.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Rule Type";
      gvColumn1.Width = 150;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Rule Name";
      gvColumn2.Width = 220;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "Scenario Name";
      gvColumn3.Width = 220;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column4";
      gvColumn4.Text = "# of References";
      gvColumn4.Width = 150;
      this.listDataTableRefs.Columns.AddRange(new GVColumn[4]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4
      });
      this.listDataTableRefs.Dock = DockStyle.Fill;
      this.listDataTableRefs.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.listDataTableRefs.HeaderHeight = 22;
      this.listDataTableRefs.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.listDataTableRefs.Location = new Point(0, 26);
      this.listDataTableRefs.Name = "listDataTableRefs";
      this.listDataTableRefs.Size = new Size(773, 415);
      this.listDataTableRefs.SortingType = SortingType.AlphaNumeric;
      this.listDataTableRefs.TabIndex = 0;
      this.AutoScaleDimensions = new SizeF(7f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(774, 522);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOk);
      this.Controls.Add((Control) this.gradientPanel2);
      this.Controls.Add((Control) this.gcBaseRate);
      this.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.Margin = new Padding(2);
      this.MaximizeBox = false;
      this.MaximumSize = new Size(790, 561);
      this.MinimizeBox = false;
      this.MinimumSize = new Size(790, 561);
      this.Name = nameof (DataTableReference);
      this.ShowIcon = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Encompass";
      this.gradientPanel2.ResumeLayout(false);
      this.gcBaseRate.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
