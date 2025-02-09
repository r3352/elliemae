// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.FeeLineSelectForm
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.CalculationEngine;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class FeeLineSelectForm : Form
  {
    private IContainer components;
    private Button btnSelect;
    private Button btnCancel;
    private GroupContainer grpSelected;
    private GridView listFees;

    public FeeLineSelectForm()
    {
      this.InitializeComponent();
      this.initForm();
      this.listFees_SelectedIndexChanged((object) null, (EventArgs) null);
    }

    private void initForm()
    {
      List<string[]> lineDescriptions = DDM_FieldAccess_Utils.GetFeeLineDescriptions();
      this.listFees.BeginUpdate();
      for (int index = 0; index < lineDescriptions.Count; ++index)
      {
        if (!(lineDescriptions[index][0] == "802") && !(lineDescriptions[index][0] == "1011"))
        {
          GVItem gvItem = !(lineDescriptions[index][0] == "803x") ? new GVItem(lineDescriptions[index][0]) : new GVItem("803");
          gvItem.SubItems.Add((object) lineDescriptions[index][1]);
          this.listFees.Items.Add(gvItem);
        }
      }
      this.listFees.EndUpdate();
    }

    public string SelectedLine => this.listFees.SelectedItems[0].Text;

    private void btnSelect_Click(object sender, EventArgs e) => this.DialogResult = DialogResult.OK;

    private void listFees_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnSelect.Enabled = this.listFees.SelectedItems.Count > 0;
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
      this.btnSelect = new Button();
      this.btnCancel = new Button();
      this.grpSelected = new GroupContainer();
      this.listFees = new GridView();
      this.grpSelected.SuspendLayout();
      this.SuspendLayout();
      this.btnSelect.Location = new Point(326, 431);
      this.btnSelect.Name = "btnSelect";
      this.btnSelect.Size = new Size(75, 23);
      this.btnSelect.TabIndex = 0;
      this.btnSelect.Text = "&Select";
      this.btnSelect.UseVisualStyleBackColor = true;
      this.btnSelect.Click += new EventHandler(this.btnSelect_Click);
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(410, 431);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 1;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.grpSelected.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.grpSelected.Controls.Add((Control) this.listFees);
      this.grpSelected.HeaderForeColor = SystemColors.ControlText;
      this.grpSelected.Location = new Point(12, 12);
      this.grpSelected.Name = "grpSelected";
      this.grpSelected.Size = new Size(473, 411);
      this.grpSelected.TabIndex = 31;
      this.grpSelected.Text = "Selected Fee Line";
      this.listFees.AllowMultiselect = false;
      this.listFees.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column4";
      gvColumn1.Text = "Fee Line";
      gvColumn1.Width = 80;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column1";
      gvColumn2.SpringToFit = true;
      gvColumn2.Text = "Fee Description";
      gvColumn2.Width = 391;
      this.listFees.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.listFees.Dock = DockStyle.Fill;
      this.listFees.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.listFees.Location = new Point(1, 26);
      this.listFees.Name = "listFees";
      this.listFees.Size = new Size(471, 384);
      this.listFees.TabIndex = 3;
      this.listFees.SelectedIndexChanged += new EventHandler(this.listFees_SelectedIndexChanged);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(497, 464);
      this.Controls.Add((Control) this.grpSelected);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnSelect);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (FeeLineSelectForm);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Select Fee Line";
      this.grpSelected.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
