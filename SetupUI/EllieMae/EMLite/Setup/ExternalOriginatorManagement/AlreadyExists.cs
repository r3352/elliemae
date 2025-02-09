// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ExternalOriginatorManagement.AlreadyExists
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.ExternalOriginatorManagement
{
  [Serializable]
  public class AlreadyExists : Form
  {
    private List<string> updateLeads = new List<string>();
    private Dictionary<string, string> contacts;
    private IContainer components;
    private Button btnOK;
    private Button btnCancel;
    private Label label1;
    private GridView gridViewCompany;
    private Panel panel1;
    private CheckBox chkAll;

    public AlreadyExists(Dictionary<string, string> contacts)
    {
      this.InitializeComponent();
      this.contacts = contacts;
      this.populateGrid();
    }

    private void populateGrid()
    {
      foreach (KeyValuePair<string, string> contact in this.contacts)
        this.gridViewCompany.Items.Add(new GVItem(contact.Value)
        {
          Tag = (object) contact.Key
        });
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      for (int nItemIndex = 0; nItemIndex < this.gridViewCompany.Items.Count; ++nItemIndex)
      {
        if (this.gridViewCompany.Items[nItemIndex].SubItems[0].Checked)
          this.updateLeads.Add(this.gridViewCompany.Items[nItemIndex].Tag.ToString());
      }
      this.DialogResult = DialogResult.OK;
    }

    public List<string> Value => this.updateLeads;

    private void chkAll_CheckedChanged(object sender, EventArgs e)
    {
      this.gridViewCompany.SubItemCheck -= new GVSubItemEventHandler(this.gridViewCompany_SubItemCheck);
      for (int nItemIndex = 0; nItemIndex < this.gridViewCompany.Items.Count; ++nItemIndex)
        this.gridViewCompany.Items[nItemIndex].SubItems[0].Checked = this.chkAll.Checked;
      this.gridViewCompany.SubItemCheck += new GVSubItemEventHandler(this.gridViewCompany_SubItemCheck);
    }

    private void gridViewCompany_SubItemCheck(object source, GVSubItemEventArgs e)
    {
      this.chkAll.CheckedChanged -= new EventHandler(this.chkAll_CheckedChanged);
      this.chkAll.Checked = true;
      for (int nItemIndex = 0; nItemIndex < this.gridViewCompany.Items.Count; ++nItemIndex)
      {
        if (!this.gridViewCompany.Items[nItemIndex].SubItems[0].Checked)
        {
          this.chkAll.Checked = false;
          break;
        }
      }
      this.chkAll.CheckedChanged += new EventHandler(this.chkAll_CheckedChanged);
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
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.label1 = new Label();
      this.gridViewCompany = new GridView();
      this.panel1 = new Panel();
      this.chkAll = new CheckBox();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Location = new Point(206, 246);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 1;
      this.btnOK.Text = "&OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(287, 246);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 2;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(5, 14);
      this.label1.Name = "label1";
      this.label1.Size = new Size(315, 13);
      this.label1.TabIndex = 3;
      this.label1.Text = "The following companies already exist. Please select to overwrite:";
      this.gridViewCompany.BorderStyle = BorderStyle.None;
      gvColumn.CheckBoxes = true;
      gvColumn.ImageIndex = -1;
      gvColumn.Name = "Column1";
      gvColumn.SpringToFit = true;
      gvColumn.Text = "     Company Name";
      gvColumn.Width = 352;
      this.gridViewCompany.Columns.AddRange(new GVColumn[1]
      {
        gvColumn
      });
      this.gridViewCompany.Dock = DockStyle.Fill;
      this.gridViewCompany.Location = new Point(0, 0);
      this.gridViewCompany.Name = "gridViewCompany";
      this.gridViewCompany.Size = new Size(352, 208);
      this.gridViewCompany.TabIndex = 4;
      this.gridViewCompany.SubItemCheck += new GVSubItemEventHandler(this.gridViewCompany_SubItemCheck);
      this.panel1.BorderStyle = BorderStyle.FixedSingle;
      this.panel1.Controls.Add((Control) this.chkAll);
      this.panel1.Controls.Add((Control) this.gridViewCompany);
      this.panel1.Location = new Point(8, 30);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(354, 210);
      this.panel1.TabIndex = 5;
      this.chkAll.AutoSize = true;
      this.chkAll.Location = new Point(4, 2);
      this.chkAll.Name = "chkAll";
      this.chkAll.Size = new Size(15, 14);
      this.chkAll.TabIndex = 5;
      this.chkAll.UseVisualStyleBackColor = true;
      this.chkAll.CheckedChanged += new EventHandler(this.chkAll_CheckedChanged);
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(374, 278);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOK);
      this.MinimizeBox = false;
      this.Name = nameof (AlreadyExists);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Company Already Exists";
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
