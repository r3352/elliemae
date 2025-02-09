// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ExternalOriginatorManagement.PipeLineExtOrgInfo
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
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
  public class PipeLineExtOrgInfo : Form
  {
    public ExternalOriginatorManagementData selectedOrg;
    private IContainer components;
    private GridView gridView1;
    private Button btnOk;
    private Button button2;

    public PipeLineExtOrgInfo(List<ExternalOriginatorManagementData> orgList)
    {
      this.InitializeComponent();
      this.PopulateAssignedToGridView(orgList);
      this.btnOk.Enabled = false;
    }

    private void PopulateAssignedToGridView(List<ExternalOriginatorManagementData> orgList)
    {
      this.gridView1.Items.Clear();
      ExternalOriginatorManagementData originatorManagementData = new ExternalOriginatorManagementData();
      originatorManagementData.OrganizationName = "All";
      originatorManagementData.ExternalID = "-1";
      this.gridView1.Items.Add(new GVItem(new string[2]
      {
        originatorManagementData.OrganizationName,
        originatorManagementData.ExternalID
      })
      {
        Tag = (object) originatorManagementData
      });
      foreach (ExternalOriginatorManagementData org in orgList)
        this.gridView1.Items.Add(new GVItem(new string[2]
        {
          org.OrganizationName,
          org.ExternalID
        })
        {
          Tag = (object) org
        });
      this.gridView1.Sort(0, SortOrder.Ascending);
      this.gridView1.Refresh();
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
      if (this.gridView1.SelectedItems == null || !this.gridView1.SelectedItems.Any<GVItem>())
        return;
      this.selectedOrg = (ExternalOriginatorManagementData) this.gridView1.SelectedItems[0].Tag;
      this.DialogResult = DialogResult.OK;
    }

    private void button2_Click(object sender, EventArgs e) => this.Close();

    private void gridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.gridView1.SelectedItems.Any<GVItem>())
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
      GVColumn gvColumn = new GVColumn();
      this.btnOk = new Button();
      this.button2 = new Button();
      this.gridView1 = new GridView();
      this.SuspendLayout();
      this.btnOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOk.Location = new Point(104, 252);
      this.btnOk.Name = "btnOk";
      this.btnOk.Size = new Size(75, 23);
      this.btnOk.TabIndex = 1;
      this.btnOk.Text = "OK";
      this.btnOk.UseVisualStyleBackColor = true;
      this.btnOk.Click += new EventHandler(this.btnOk_Click);
      this.button2.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.button2.Location = new Point(191, 252);
      this.button2.Name = "button2";
      this.button2.Size = new Size(75, 23);
      this.button2.TabIndex = 2;
      this.button2.Text = "Cancel";
      this.button2.UseVisualStyleBackColor = true;
      this.button2.Click += new EventHandler(this.button2_Click);
      this.gridView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      gvColumn.ImageIndex = -1;
      gvColumn.Name = "Column1";
      gvColumn.SpringToFit = true;
      gvColumn.Text = "External Org Name";
      gvColumn.Width = 276;
      this.gridView1.Columns.AddRange(new GVColumn[1]
      {
        gvColumn
      });
      this.gridView1.Location = new Point(-1, 0);
      this.gridView1.Name = "gridView1";
      this.gridView1.Size = new Size(278, 226);
      this.gridView1.TabIndex = 0;
      this.gridView1.SelectedIndexChanged += new EventHandler(this.gridView1_SelectedIndexChanged);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.AutoScroll = true;
      this.ClientSize = new Size(278, 298);
      this.Controls.Add((Control) this.button2);
      this.Controls.Add((Control) this.btnOk);
      this.Controls.Add((Control) this.gridView1);
      this.Name = nameof (PipeLineExtOrgInfo);
      this.Text = "External Org Info";
      this.ResumeLayout(false);
    }
  }
}
