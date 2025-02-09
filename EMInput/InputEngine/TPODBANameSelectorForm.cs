// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.TPODBANameSelectorForm
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class TPODBANameSelectorForm : Form
  {
    private List<ExternalOrgDBAName> dbas;
    private IContainer components;
    private GroupContainer grpTPODBA;
    private GridView gvNames;
    private Button btnSelect;
    private Button btnCancel;

    public TPODBANameSelectorForm(List<ExternalOrgDBAName> dbas)
    {
      this.InitializeComponent();
      this.btnSelect.Enabled = this.gvNames.SelectedItems.Count == 1;
      this.dbas = dbas;
      this.populate();
    }

    private void populate()
    {
      this.gvNames.Items.Clear();
      foreach (ExternalOrgDBAName dba in this.dbas)
      {
        GVItem gvItem = new GVItem();
        gvItem.SubItems.Add((object) dba.Name);
        if (dba.SetAsDefault)
          gvItem.SubItems.Add((object) "Default");
        gvItem.Tag = (object) dba;
        this.gvNames.Items.Add(gvItem);
      }
    }

    private void btnSelect_Click(object sender, EventArgs e) => this.DialogResult = DialogResult.OK;

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
    }

    private void gvNames_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnSelect.Enabled = this.gvNames.SelectedItems.Count == 1;
    }

    public ExternalOrgDBAName SelectedDBAName
    {
      get => (ExternalOrgDBAName) this.gvNames.SelectedItems[0].Tag;
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (TPODBANameSelectorForm));
      this.grpTPODBA = new GroupContainer();
      this.gvNames = new GridView();
      this.btnSelect = new Button();
      this.btnCancel = new Button();
      this.grpTPODBA.SuspendLayout();
      this.SuspendLayout();
      this.grpTPODBA.Controls.Add((Control) this.gvNames);
      this.grpTPODBA.Dock = DockStyle.Top;
      this.grpTPODBA.HeaderForeColor = SystemColors.ControlText;
      this.grpTPODBA.Location = new Point(0, 0);
      this.grpTPODBA.Name = "grpTPODBA";
      this.grpTPODBA.Size = new Size(629, 428);
      this.grpTPODBA.TabIndex = 10;
      this.grpTPODBA.Text = "DBA Details";
      this.gvNames.AllowMultiselect = false;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Name";
      gvColumn1.Text = "DBA Name";
      gvColumn1.Width = 500;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Default";
      gvColumn2.Text = "Default";
      gvColumn2.Width = 100;
      this.gvNames.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.gvNames.Dock = DockStyle.Fill;
      this.gvNames.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvNames.Location = new Point(1, 26);
      this.gvNames.Name = "gvNames";
      this.gvNames.Size = new Size(627, 401);
      this.gvNames.SortOption = GVSortOption.None;
      this.gvNames.TabIndex = 8;
      this.gvNames.SelectedIndexChanged += new EventHandler(this.gvNames_SelectedIndexChanged);
      this.btnSelect.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnSelect.Location = new Point(462, 447);
      this.btnSelect.Name = "btnSelect";
      this.btnSelect.Size = new Size(75, 23);
      this.btnSelect.TabIndex = 12;
      this.btnSelect.Text = "&Select";
      this.btnSelect.UseVisualStyleBackColor = true;
      this.btnSelect.Click += new EventHandler(this.btnSelect_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(543, 447);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 11;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.AcceptButton = (IButtonControl) this.btnSelect;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(629, 482);
      this.Controls.Add((Control) this.btnSelect);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.grpTPODBA);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (TPODBANameSelectorForm);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Third Party Originator DBA Name Selector";
      this.grpTPODBA.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
