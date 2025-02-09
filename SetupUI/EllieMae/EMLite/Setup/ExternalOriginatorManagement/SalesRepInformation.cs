// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ExternalOriginatorManagement.SalesRepInformation
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
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
  public class SalesRepInformation : Form
  {
    public GVItem selectedUser;
    private IContainer components;
    private GridView gridView1;
    private Button btnOk;
    private Button btnCancel;

    public SalesRepInformation(UserInfo[] userList)
    {
      this.InitializeComponent();
      this.btnOk.Enabled = false;
      this.PopulateAssignedToGridView(userList);
    }

    private void PopulateAssignedToGridView(UserInfo[] userList)
    {
      this.gridView1.Items.Clear();
      foreach (UserInfo user in userList)
      {
        string str = string.Join(",", ((IEnumerable<Persona>) user.UserPersonas).Select<Persona, string>((Func<Persona, string>) (p => p.Name)).ToArray<string>());
        this.gridView1.Items.Add(new GVItem(new string[3]
        {
          user.FullName,
          user.Userid,
          str
        }));
      }
      this.gridView1.Refresh();
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
      if (this.gridView1.SelectedItems == null || !this.gridView1.SelectedItems.Any<GVItem>())
        return;
      this.selectedUser = this.gridView1.SelectedItems[0];
    }

    private void gridView1_DoubleClick(object sender, EventArgs e)
    {
      if (this.gridView1.SelectedItems == null || !this.gridView1.SelectedItems.Any<GVItem>())
        return;
      this.selectedUser = this.gridView1.SelectedItems[0];
      this.DialogResult = DialogResult.OK;
    }

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
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (SalesRepInformation));
      this.btnOk = new Button();
      this.btnCancel = new Button();
      this.gridView1 = new GridView();
      this.SuspendLayout();
      this.btnOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOk.DialogResult = DialogResult.OK;
      this.btnOk.Location = new Point(217, 393);
      this.btnOk.Name = "btnOk";
      this.btnOk.Size = new Size(75, 23);
      this.btnOk.TabIndex = 1;
      this.btnOk.Text = "OK";
      this.btnOk.UseVisualStyleBackColor = true;
      this.btnOk.Click += new EventHandler(this.btnOk_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(309, 393);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 2;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.gridView1.AllowMultiselect = false;
      this.gridView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Full Name";
      gvColumn1.Width = 100;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "User ID";
      gvColumn2.Width = 100;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "Persona";
      gvColumn3.Width = 100;
      this.gridView1.Columns.AddRange(new GVColumn[3]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3
      });
      this.gridView1.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gridView1.Location = new Point(0, 0);
      this.gridView1.Name = "gridView1";
      this.gridView1.Size = new Size(396, 385);
      this.gridView1.TabIndex = 0;
      this.gridView1.SelectedIndexChanged += new EventHandler(this.gridView1_SelectedIndexChanged);
      this.gridView1.DoubleClick += new EventHandler(this.gridView1_DoubleClick);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(396, 428);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOk);
      this.Controls.Add((Control) this.gridView1);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (SalesRepInformation);
      this.Text = "Sales Reps";
      this.ResumeLayout(false);
    }
  }
}
