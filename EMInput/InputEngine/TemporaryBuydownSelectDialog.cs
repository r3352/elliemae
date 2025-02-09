// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.TemporaryBuydownSelectDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer.Bpm;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class TemporaryBuydownSelectDialog : Form
  {
    private IContainer components;
    private GroupContainer groupContainer1;
    private GridView listView;
    private Button onBtn;
    private Button cancleBtn;

    public TemporaryBuydownSelectDialog(List<TemporaryBuydown> buydownTemplates)
    {
      this.InitializeComponent();
      this.loadTemporaryBuydownTypes(buydownTemplates);
      this.listView_SelectedIndexChanged((object) null, (EventArgs) null);
    }

    private void loadTemporaryBuydownTypes(List<TemporaryBuydown> buydownTemplates)
    {
      foreach (TemporaryBuydown buydownTemplate in buydownTemplates)
        this.listView.Items.Add(new GVItem()
        {
          SubItems = {
            (object) buydownTemplate.BuydownType,
            (object) buydownTemplate.Description
          },
          Tag = (object) buydownTemplate
        });
    }

    private void onBtn_Click(object sender, EventArgs e) => this.DialogResult = DialogResult.OK;

    public TemporaryBuydown SelectedBuydownTemplate
    {
      get => (TemporaryBuydown) this.listView.SelectedItems[0].Tag;
    }

    private void listView_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.onBtn.Enabled = this.listView.SelectedItems.Count > 0;
    }

    private void listView_DoubleClick(object sender, EventArgs e)
    {
      this.onBtn_Click((object) null, (EventArgs) null);
    }

    private void TemporaryBuydownSelectDialog_KeyPress(object sender, KeyPressEventArgs e)
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
      this.groupContainer1 = new GroupContainer();
      this.listView = new GridView();
      this.onBtn = new Button();
      this.cancleBtn = new Button();
      this.groupContainer1.SuspendLayout();
      this.SuspendLayout();
      this.groupContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.groupContainer1.Controls.Add((Control) this.listView);
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(12, 12);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(668, 316);
      this.groupContainer1.TabIndex = 9;
      this.groupContainer1.Text = "Select a Buydown Type";
      this.listView.AllowMultiselect = false;
      this.listView.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Buydown Type";
      gvColumn1.Width = 300;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.SpringToFit = true;
      gvColumn2.Text = "Description";
      gvColumn2.Width = 366;
      this.listView.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.listView.Dock = DockStyle.Fill;
      this.listView.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.listView.Location = new Point(1, 26);
      this.listView.Name = "listView";
      this.listView.Size = new Size(666, 289);
      this.listView.TabIndex = 7;
      this.listView.SelectedIndexChanged += new EventHandler(this.listView_SelectedIndexChanged);
      this.listView.DoubleClick += new EventHandler(this.listView_DoubleClick);
      this.onBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.onBtn.Location = new Point(524, 337);
      this.onBtn.Name = "onBtn";
      this.onBtn.Size = new Size(75, 23);
      this.onBtn.TabIndex = 10;
      this.onBtn.Text = "OK";
      this.onBtn.UseVisualStyleBackColor = true;
      this.onBtn.Click += new EventHandler(this.onBtn_Click);
      this.cancleBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.cancleBtn.DialogResult = DialogResult.Cancel;
      this.cancleBtn.Location = new Point(605, 337);
      this.cancleBtn.Name = "cancleBtn";
      this.cancleBtn.Size = new Size(75, 23);
      this.cancleBtn.TabIndex = 11;
      this.cancleBtn.Text = "Cancel";
      this.cancleBtn.UseVisualStyleBackColor = true;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(692, 369);
      this.Controls.Add((Control) this.cancleBtn);
      this.Controls.Add((Control) this.onBtn);
      this.Controls.Add((Control) this.groupContainer1);
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (TemporaryBuydownSelectDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Temporary Buydown Types";
      this.KeyPress += new KeyPressEventHandler(this.TemporaryBuydownSelectDialog_KeyPress);
      this.groupContainer1.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
