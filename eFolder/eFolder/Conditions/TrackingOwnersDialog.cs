// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Conditions.TrackingOwnersDialog
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.Conditions
{
  public class TrackingOwnersDialog : Form
  {
    private LoanDataMgr loanDataMgr;
    private EnhancedConditionLog cond;
    private IContainer components;
    internal GridView gvRoles;
    private Button btnSave;
    private GroupContainer groupContainer1;
    private EMHelpLink helpLink;
    private Label label1;
    private TextBox txtUnassigned;

    public TrackingOwnersDialog(LoanDataMgr loanDataMgr, EnhancedConditionLog cond)
    {
      this.InitializeComponent();
      this.loanDataMgr = loanDataMgr;
      this.cond = cond;
      this.loadRoleList();
    }

    private void loadRoleList()
    {
      List<string> defaultTrackingOptions = Utils.GetEnhanceConditionsDefaultTrackingOptions();
      foreach (RoleInfo allRole in this.loanDataMgr.SystemConfiguration.AllRoles)
      {
        string str = string.Empty;
        if (this.cond.Definitions.TrackingDefinitions != null)
        {
          foreach (StatusTrackingDefinition trackingDefinition in (IEnumerable<StatusTrackingDefinition>) this.cond.Definitions.TrackingDefinitions)
          {
            if (!defaultTrackingOptions.Contains(trackingDefinition.Name) && trackingDefinition.AllowedRoles != null)
            {
              foreach (int allowedRole in trackingDefinition.AllowedRoles)
              {
                if (allowedRole == allRole.ID)
                  str = this.addStringToList(str, trackingDefinition.Name);
              }
            }
          }
        }
        if (string.IsNullOrEmpty(str))
          this.txtUnassigned.Text = this.addStringToList(this.txtUnassigned.Text, allRole.Name);
        else
          this.addRole(allRole, str);
      }
    }

    private string addStringToList(string list, string value)
    {
      if (!list.Contains(value))
        list = !string.IsNullOrEmpty(list) ? list + ", " + value : value;
      return list;
    }

    private void addRole(RoleInfo role, string statuses)
    {
      this.gvRoles.Items.Add(new GVItem()
      {
        SubItems = {
          [0] = {
            Text = role.Name
          },
          [1] = {
            Text = statuses
          }
        },
        Tag = (object) role
      });
    }

    private void btnOk_Click(object sender, EventArgs e) => this.DialogResult = DialogResult.OK;

    private void TrackingOwnersDialog_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      Session.Application.DisplayHelp(this.helpLink.HelpTag);
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
      this.btnSave = new Button();
      this.groupContainer1 = new GroupContainer();
      this.helpLink = new EMHelpLink();
      this.label1 = new Label();
      this.txtUnassigned = new TextBox();
      this.groupContainer1.SuspendLayout();
      this.SuspendLayout();
      this.gvRoles.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "colRole";
      gvColumn1.Text = "Role";
      gvColumn1.Width = 150;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "colStatuses";
      gvColumn2.Text = "Statuses";
      gvColumn2.Width = 249;
      this.gvRoles.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.gvRoles.Dock = DockStyle.Fill;
      this.gvRoles.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvRoles.Location = new Point(1, 26);
      this.gvRoles.Name = "gvRoles";
      this.gvRoles.Size = new Size(400, 239);
      this.gvRoles.TabIndex = 2;
      this.gvRoles.TextTrimming = StringTrimming.EllipsisCharacter;
      this.btnSave.Location = new Point(339, 366);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(75, 22);
      this.btnSave.TabIndex = 4;
      this.btnSave.Text = "OK";
      this.btnSave.Click += new EventHandler(this.btnOk_Click);
      this.groupContainer1.Controls.Add((Control) this.gvRoles);
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(13, 12);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(402, 266);
      this.groupContainer1.TabIndex = 1;
      this.groupContainer1.Text = "Roles";
      this.helpLink.BackColor = Color.Transparent;
      this.helpLink.Cursor = Cursors.Hand;
      this.helpLink.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.helpLink.HelpTag = "Document Access Rights";
      this.helpLink.Location = new Point(15, 366);
      this.helpLink.Name = "helpLink";
      this.helpLink.Size = new Size(90, 18);
      this.helpLink.TabIndex = 6;
      this.helpLink.TabStop = false;
      this.helpLink.Visible = false;
      this.label1.Location = new Point(12, 281);
      this.label1.Name = "label1";
      this.label1.Size = new Size(350, 24);
      this.label1.TabIndex = 7;
      this.label1.Text = "Unassigned Roles";
      this.txtUnassigned.Location = new Point(12, 299);
      this.txtUnassigned.Multiline = true;
      this.txtUnassigned.Name = "txtUnassigned";
      this.txtUnassigned.ReadOnly = true;
      this.txtUnassigned.ScrollBars = ScrollBars.Vertical;
      this.txtUnassigned.Size = new Size(402, 61);
      this.txtUnassigned.TabIndex = 8;
      this.AcceptButton = (IButtonControl) this.btnSave;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(427, 396);
      this.Controls.Add((Control) this.txtUnassigned);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.helpLink);
      this.Controls.Add((Control) this.groupContainer1);
      this.Controls.Add((Control) this.btnSave);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (TrackingOwnersDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Tracking Owners";
      this.KeyDown += new KeyEventHandler(this.TrackingOwnersDialog_KeyDown);
      this.groupContainer1.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
