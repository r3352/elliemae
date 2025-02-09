// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ConcurrentEditing.MergeResultWholeView
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.UI;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ConcurrentEditing
{
  public class MergeResultWholeView : Form
  {
    private IContainer components;
    private GroupContainer groupContainer2;
    private GridView gvChanges;
    private Panel pnlBase;
    private BorderPanel borderPanel1;
    private Button btnCancel;
    private Panel panel3;
    private FormattedLabel formattedLabel1;

    public MergeResultWholeView(List<GVItem> itemList)
    {
      this.InitializeComponent();
      if (itemList != null)
        this.gvChanges.Items.AddRange(itemList.ToArray());
      if (itemList == null || itemList.Count <= 0 || itemList[0].SubItems.Count >= this.gvChanges.Columns.Count)
        return;
      this.gvChanges.Columns.RemoveAt(this.gvChanges.Columns.Count - 1);
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
      this.groupContainer2 = new GroupContainer();
      this.gvChanges = new GridView();
      this.pnlBase = new Panel();
      this.borderPanel1 = new BorderPanel();
      this.btnCancel = new Button();
      this.panel3 = new Panel();
      this.formattedLabel1 = new FormattedLabel();
      this.groupContainer2.SuspendLayout();
      this.pnlBase.SuspendLayout();
      this.borderPanel1.SuspendLayout();
      this.panel3.SuspendLayout();
      this.SuspendLayout();
      this.groupContainer2.Controls.Add((Control) this.formattedLabel1);
      this.groupContainer2.Controls.Add((Control) this.gvChanges);
      this.groupContainer2.Dock = DockStyle.Fill;
      this.groupContainer2.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer2.Location = new Point(0, 0);
      this.groupContainer2.Name = "groupContainer2";
      this.groupContainer2.Size = new Size(769, 445);
      this.groupContainer2.TabIndex = 0;
      this.groupContainer2.Text = " ";
      this.gvChanges.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Field ID";
      gvColumn1.Width = 100;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Field Description";
      gvColumn2.Width = 250;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column6";
      gvColumn3.Text = "Original Value";
      gvColumn3.Width = 130;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column3";
      gvColumn4.Text = "Modified by Other Users";
      gvColumn4.Width = 130;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column4";
      gvColumn5.Text = "Modified by You";
      gvColumn5.Width = 130;
      this.gvChanges.Columns.AddRange(new GVColumn[5]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5
      });
      this.gvChanges.Dock = DockStyle.Fill;
      this.gvChanges.Location = new Point(1, 26);
      this.gvChanges.Name = "gvChanges";
      this.gvChanges.Size = new Size(767, 418);
      this.gvChanges.TabIndex = 0;
      this.pnlBase.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlBase.Controls.Add((Control) this.groupContainer2);
      this.pnlBase.Location = new Point(5, 5);
      this.pnlBase.Name = "pnlBase";
      this.pnlBase.Size = new Size(769, 445);
      this.pnlBase.TabIndex = 2;
      this.borderPanel1.Controls.Add((Control) this.pnlBase);
      this.borderPanel1.Dock = DockStyle.Fill;
      this.borderPanel1.Location = new Point(0, 0);
      this.borderPanel1.Name = "borderPanel1";
      this.borderPanel1.Padding = new Padding(3);
      this.borderPanel1.Size = new Size(777, 462);
      this.borderPanel1.TabIndex = 9;
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(690, 8);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 25);
      this.btnCancel.TabIndex = 5;
      this.btnCancel.Text = "Close";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.panel3.Controls.Add((Control) this.btnCancel);
      this.panel3.Dock = DockStyle.Bottom;
      this.panel3.Location = new Point(0, 462);
      this.panel3.Name = "panel3";
      this.panel3.Size = new Size(777, 40);
      this.panel3.TabIndex = 10;
      this.formattedLabel1.BackColor = Color.Transparent;
      this.formattedLabel1.Location = new Point(5, 6);
      this.formattedLabel1.Name = "formattedLabel1";
      this.formattedLabel1.Size = new Size(476, 16);
      this.formattedLabel1.TabIndex = 1;
      this.formattedLabel1.Text = "<b>All Changes Made by Other Users</b> - These changes will be added to your copy of this loan file.";
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(777, 502);
      this.Controls.Add((Control) this.borderPanel1);
      this.Controls.Add((Control) this.panel3);
      this.Font = new Font("Arial", 8f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MinimizeBox = false;
      this.Name = nameof (MergeResultWholeView);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "All Fields";
      this.groupContainer2.ResumeLayout(false);
      this.groupContainer2.PerformLayout();
      this.pnlBase.ResumeLayout(false);
      this.borderPanel1.ResumeLayout(false);
      this.panel3.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
