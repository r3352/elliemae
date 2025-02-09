// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.FieldValuePicker
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class FieldValuePicker : Form
  {
    private IContainer components;
    private DialogButtons dlgButtons;
    private GroupContainer grpOptions;
    private GridView gvOptions;

    public FieldValuePicker(FieldDefinition fieldDef)
    {
      this.InitializeComponent();
      foreach (FieldOption option in fieldDef.Options)
        this.gvOptions.Items.Add((object) option);
      this.grpOptions.Text = "Field Options (" + (object) this.gvOptions.Items.Count + ")";
    }

    public FieldOption SelectedOption => this.gvOptions.SelectedItems[0].Value as FieldOption;

    private void dlgButtons_OK(object sender, EventArgs e)
    {
      if (this.gvOptions.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must first select an option from the list provided.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
        this.DialogResult = DialogResult.OK;
    }

    private void gvOptions_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      if (!e.Item.Selected)
        return;
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
      GVColumn gvColumn = new GVColumn();
      this.dlgButtons = new DialogButtons();
      this.grpOptions = new GroupContainer();
      this.gvOptions = new GridView();
      this.grpOptions.SuspendLayout();
      this.SuspendLayout();
      this.dlgButtons.Dock = DockStyle.Bottom;
      this.dlgButtons.Location = new Point(0, 281);
      this.dlgButtons.Name = "dlgButtons";
      this.dlgButtons.Size = new Size(382, 40);
      this.dlgButtons.TabIndex = 0;
      this.dlgButtons.OK += new EventHandler(this.dlgButtons_OK);
      this.grpOptions.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.grpOptions.Controls.Add((Control) this.gvOptions);
      this.grpOptions.HeaderForeColor = SystemColors.ControlText;
      this.grpOptions.Location = new Point(10, 11);
      this.grpOptions.Name = "grpOptions";
      this.grpOptions.Size = new Size(361, 268);
      this.grpOptions.TabIndex = 1;
      this.grpOptions.Text = "Field Options";
      this.gvOptions.AllowMultiselect = false;
      this.gvOptions.BorderStyle = BorderStyle.None;
      gvColumn.ImageIndex = -1;
      gvColumn.Name = "Column1";
      gvColumn.SpringToFit = true;
      gvColumn.Text = "Column";
      gvColumn.Width = 359;
      this.gvOptions.Columns.AddRange(new GVColumn[1]
      {
        gvColumn
      });
      this.gvOptions.Dock = DockStyle.Fill;
      this.gvOptions.HeaderHeight = 0;
      this.gvOptions.HeaderVisible = false;
      this.gvOptions.Location = new Point(1, 26);
      this.gvOptions.Name = "gvOptions";
      this.gvOptions.Size = new Size(359, 241);
      this.gvOptions.TabIndex = 0;
      this.gvOptions.ItemDoubleClick += new GVItemEventHandler(this.gvOptions_ItemDoubleClick);
      this.AcceptButton = (IButtonControl) this.dlgButtons;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(382, 321);
      this.Controls.Add((Control) this.grpOptions);
      this.Controls.Add((Control) this.dlgButtons);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.MinimizeBox = false;
      this.Name = nameof (FieldValuePicker);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Select Field Value";
      this.grpOptions.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
