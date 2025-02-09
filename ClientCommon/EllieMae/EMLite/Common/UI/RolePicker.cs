// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.RolePicker
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
  public class RolePicker : Form
  {
    private bool multiSelect = true;
    private IContainer components;
    private GridView gvRoles;
    private DialogButtons dlgButtons;
    private Label lblTitle;

    public RolePicker(RoleSummaryInfo[] roles, bool allowMultiSelect)
    {
      this.InitializeComponent();
      this.multiSelect = allowMultiSelect;
      this.loadRoles(roles);
      this.gvRoles.Columns[0].CheckBoxes = this.multiSelect;
      this.gvRoles.Selectable = !this.multiSelect;
      if (this.multiSelect)
        return;
      this.lblTitle.Text = "Select a role from the list provided:";
    }

    public RoleSummaryInfo[] GetSelectedRoles()
    {
      if (this.multiSelect)
        return this.getCheckedRoleObjects();
      return new RoleSummaryInfo[1]
      {
        this.gvRoles.SelectedItems[0].Tag as RoleSummaryInfo
      };
    }

    private void loadRoles(RoleSummaryInfo[] roles)
    {
      this.gvRoles.Items.Clear();
      foreach (RoleSummaryInfo role in roles)
        this.gvRoles.Items.Add(new GVItem()
        {
          SubItems = {
            [0] = {
              Text = role.RoleAbbr
            },
            [1] = {
              Text = role.Name
            }
          },
          Tag = (object) role
        });
      this.gvRoles.Sort(0, SortOrder.Ascending);
    }

    private void dlgButtons_OK(object sender, EventArgs e)
    {
      if (!(!this.multiSelect ? this.validateSingleRoleSelection() : this.validateMultiRoleSelection()))
        return;
      this.DialogResult = DialogResult.OK;
    }

    private bool validateMultiRoleSelection()
    {
      if (this.getCheckedRoleObjects().Length != 0)
        return true;
      int num = (int) Utils.Dialog((IWin32Window) this, "You must select one or more roles from the list.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      return false;
    }

    private bool validateSingleRoleSelection()
    {
      if (this.gvRoles.SelectedItems.Count != 0)
        return true;
      int num = (int) Utils.Dialog((IWin32Window) this, "You must first select a role from the list.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      return false;
    }

    private RoleSummaryInfo[] getCheckedRoleObjects()
    {
      List<RoleSummaryInfo> roleSummaryInfoList = new List<RoleSummaryInfo>();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvRoles.Items)
      {
        if (gvItem.SubItems[0].Checked)
          roleSummaryInfoList.Add(gvItem.Tag as RoleSummaryInfo);
      }
      return roleSummaryInfoList.ToArray();
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
      this.gvRoles = new GridView();
      this.dlgButtons = new DialogButtons();
      this.lblTitle = new Label();
      this.SuspendLayout();
      this.gvRoles.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      gvColumn1.CheckBoxes = true;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Abbrev";
      gvColumn1.Width = 90;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.SpringToFit = true;
      gvColumn2.Text = "Name";
      gvColumn2.Width = 207;
      this.gvRoles.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.gvRoles.Location = new Point(10, 24);
      this.gvRoles.Name = "gvRoles";
      this.gvRoles.Size = new Size(299, 274);
      this.gvRoles.TabIndex = 5;
      this.dlgButtons.Dock = DockStyle.Bottom;
      this.dlgButtons.Location = new Point(0, 299);
      this.dlgButtons.Name = "dlgButtons";
      this.dlgButtons.Size = new Size(320, 47);
      this.dlgButtons.TabIndex = 4;
      this.dlgButtons.OK += new EventHandler(this.dlgButtons_OK);
      this.lblTitle.AutoSize = true;
      this.lblTitle.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblTitle.Location = new Point(8, 8);
      this.lblTitle.Name = "lblTitle";
      this.lblTitle.Size = new Size(232, 14);
      this.lblTitle.TabIndex = 3;
      this.lblTitle.Text = "Select one or more roles from the list provided:";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(320, 346);
      this.Controls.Add((Control) this.gvRoles);
      this.Controls.Add((Control) this.dlgButtons);
      this.Controls.Add((Control) this.lblTitle);
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (RolePicker);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.Text = "Select Role(s)";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
