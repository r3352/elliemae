// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.ZipcodeSelectorDialog
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class ZipcodeSelectorDialog : Form
  {
    private ZipCodeInfo selectedZipCodeInfo;
    private IContainer components;
    private GridView gvCities;
    private DialogButtons dlgButtons;
    private Label lblTitle;

    public ZipcodeSelectorDialog(string zipcode, ZipCodeInfo[] zips)
    {
      this.InitializeComponent();
      string empty = string.Empty;
      List<string> stringList = new List<string>();
      this.gvCities.BeginUpdate();
      for (int index = 0; index < zips.Length; ++index)
      {
        string str = zips[index].City.ToLower() + "_" + zips[index].County.ToLower();
        if (!stringList.Contains(str))
        {
          this.gvCities.Items.Add(new GVItem(zips[index].City)
          {
            SubItems = {
              [1] = {
                Text = zips[index].County
              },
              [2] = {
                Text = zips[index].State
              }
            },
            Tag = (object) zips[index]
          });
          stringList.Add(str);
        }
      }
      this.gvCities.EndUpdate();
      this.gvCities.Items[0].Selected = true;
    }

    public ZipCodeInfo SelectedZipCodeInfo => this.selectedZipCodeInfo;

    private void dlgButtons_OK(object sender, EventArgs e)
    {
      this.selectedZipCodeInfo = (ZipCodeInfo) this.gvCities.SelectedItems[0].Tag;
      this.DialogResult = DialogResult.OK;
    }

    private void gvCities_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.dlgButtons.OKButton.Enabled = this.gvCities.SelectedItems.Count == 1;
    }

    private void gvCities_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      if (!e.Item.Selected || !(e.Item.Tag is ZipCodeInfo))
        return;
      this.selectedZipCodeInfo = (ZipCodeInfo) e.Item.Tag;
      this.DialogResult = DialogResult.OK;
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
      this.gvCities = new GridView();
      this.dlgButtons = new DialogButtons();
      this.lblTitle = new Label();
      this.SuspendLayout();
      this.gvCities.AllowMultiselect = false;
      this.gvCities.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.SpringToFit = true;
      gvColumn1.Text = "City Name";
      gvColumn1.Width = 142;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "County";
      gvColumn2.Width = 150;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "State";
      gvColumn3.Width = 40;
      this.gvCities.Columns.AddRange(new GVColumn[3]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3
      });
      this.gvCities.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvCities.Location = new Point(12, 28);
      this.gvCities.Name = "gvCities";
      this.gvCities.Size = new Size(334, 203);
      this.gvCities.TabIndex = 8;
      this.gvCities.SelectedIndexChanged += new EventHandler(this.gvCities_SelectedIndexChanged);
      this.gvCities.ItemDoubleClick += new GVItemEventHandler(this.gvCities_ItemDoubleClick);
      this.dlgButtons.Dock = DockStyle.Bottom;
      this.dlgButtons.Location = new Point(0, 222);
      this.dlgButtons.Name = "dlgButtons";
      this.dlgButtons.Size = new Size(358, 47);
      this.dlgButtons.TabIndex = 7;
      this.dlgButtons.OK += new EventHandler(this.dlgButtons_OK);
      this.lblTitle.AutoSize = true;
      this.lblTitle.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblTitle.Location = new Point(10, 12);
      this.lblTitle.Name = "lblTitle";
      this.lblTitle.Size = new Size(333, 14);
      this.lblTitle.TabIndex = 6;
      this.lblTitle.Text = "Several cities are located in this Zip Code. Select a city from the list:";
      this.AcceptButton = (IButtonControl) this.dlgButtons;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(358, 269);
      this.Controls.Add((Control) this.gvCities);
      this.Controls.Add((Control) this.dlgButtons);
      this.Controls.Add((Control) this.lblTitle);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ZipcodeSelectorDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Select a City";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
