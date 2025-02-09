// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ExternalOriginatorManagement.ExternalbankList
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.ExternalOriginatorManagement
{
  public class ExternalbankList : Form
  {
    public ExternalBank selectedbank;
    private IContainer components;
    private GridView gridView1;
    private Button button2;
    private Button btnOk;

    public ExternalbankList(List<ExternalBank> externalBanks)
    {
      this.InitializeComponent();
      this.PopulateAssignedToGridView(externalBanks);
      this.btnOk.Enabled = false;
    }

    private void PopulateAssignedToGridView(List<ExternalBank> externalBanks)
    {
      this.gridView1.Items.Clear();
      foreach (ExternalBank externalBank in externalBanks)
        this.gridView1.Items.Add(new GVItem(new string[6]
        {
          externalBank.BankName,
          externalBank.City,
          externalBank.State,
          externalBank.ABANumber,
          externalBank.Address,
          externalBank.Address1
        })
        {
          Tag = (object) externalBank
        });
      this.gridView1.Sort(0, SortOrder.Ascending);
      this.gridView1.Refresh();
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
      if (this.gridView1.SelectedItems == null || !this.gridView1.SelectedItems.Any<GVItem>())
        return;
      this.selectedbank = (ExternalBank) this.gridView1.SelectedItems[0].Tag;
      this.DialogResult = DialogResult.OK;
    }

    private void gridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.gridView1.SelectedItems.Any<GVItem>() && this.gridView1.SelectedItems.Count == 1)
        this.btnOk.Enabled = true;
      else
        this.btnOk.Enabled = false;
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
      GVColumn gvColumn5 = new GVColumn();
      GVColumn gvColumn6 = new GVColumn();
      this.gridView1 = new GridView();
      this.button2 = new Button();
      this.btnOk = new Button();
      this.SuspendLayout();
      this.gridView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Bank Name";
      gvColumn1.Width = 171;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "City";
      gvColumn2.Width = 80;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "State";
      gvColumn3.Width = 40;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column4";
      gvColumn4.Text = "ABA #";
      gvColumn4.Width = 120;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column5";
      gvColumn5.Text = "Address1";
      gvColumn5.Width = 225;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Column6";
      gvColumn6.Text = "Address2";
      gvColumn6.Width = 225;
      this.gridView1.Columns.AddRange(new GVColumn[6]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6
      });
      this.gridView1.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gridView1.Location = new Point(12, 12);
      this.gridView1.Name = "gridView1";
      this.gridView1.Size = new Size(813, 289);
      this.gridView1.TabIndex = 1;
      this.gridView1.SelectedIndexChanged += new EventHandler(this.gridView1_SelectedIndexChanged);
      this.button2.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.button2.DialogResult = DialogResult.Cancel;
      this.button2.Location = new Point(747, 321);
      this.button2.Name = "button2";
      this.button2.Size = new Size(75, 23);
      this.button2.TabIndex = 4;
      this.button2.Text = "Cancel";
      this.button2.UseVisualStyleBackColor = true;
      this.btnOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOk.Location = new Point(660, 321);
      this.btnOk.Name = "btnOk";
      this.btnOk.Size = new Size(75, 23);
      this.btnOk.TabIndex = 3;
      this.btnOk.Text = "OK";
      this.btnOk.UseVisualStyleBackColor = true;
      this.btnOk.Click += new EventHandler(this.btnOk_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(837, 365);
      this.Controls.Add((Control) this.button2);
      this.Controls.Add((Control) this.btnOk);
      this.Controls.Add((Control) this.gridView1);
      this.Name = nameof (ExternalbankList);
      this.ShowIcon = false;
      this.Text = "External Bank List";
      this.ResumeLayout(false);
    }
  }
}
