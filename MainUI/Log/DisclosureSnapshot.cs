// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.DisclosureSnapshot
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.UI;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class DisclosureSnapshot : Form
  {
    private IContainer components;
    private GridView gvHistory;

    public DisclosureSnapshot(Dictionary<string, string> history)
    {
      this.InitializeComponent();
      foreach (string key in history.Keys)
        this.gvHistory.Items.Add(new GVItem(key)
        {
          SubItems = {
            (object) history[key]
          }
        });
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
      this.gvHistory = new GridView();
      this.SuspendLayout();
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Field ID";
      gvColumn1.Width = 100;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Data";
      gvColumn2.Width = 200;
      this.gvHistory.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.gvHistory.Location = new Point(12, 12);
      this.gvHistory.Name = "gvHistory";
      this.gvHistory.Size = new Size(336, 365);
      this.gvHistory.TabIndex = 0;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(364, 394);
      this.Controls.Add((Control) this.gvHistory);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (DisclosureSnapshot);
      this.Text = "Disclosured Data";
      this.ResumeLayout(false);
    }
  }
}
