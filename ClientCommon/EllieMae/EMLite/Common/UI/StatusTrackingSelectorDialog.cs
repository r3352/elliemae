// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.StatusTrackingSelectorDialog
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class StatusTrackingSelectorDialog : Form
  {
    private string selectedStatus = "";
    private const string className = "StatusTrackingSelectorDialog";
    private IContainer components;
    private GridView gridView1;
    private Button selectBtn;
    private Button cancelBtn;
    private Label label1;

    public string SelectedStatus => this.selectedStatus;

    public StatusTrackingSelectorDialog(string tag, bool isTrackingOwner)
    {
      this.InitializeComponent();
      string str1 = "~%cbiz%~";
      string[] strArray1 = new string[2];
      this.gridView1.Items.Clear();
      this.gridView1.BeginUpdate();
      string[] separator = new string[1]{ str1 };
      string[] strArray2 = tag.Split(separator, StringSplitOptions.None);
      if (strArray2.Length > 1)
      {
        strArray1[0] = strArray2[0];
        strArray1[1] = strArray2[1];
      }
      if (isTrackingOwner)
      {
        this.label1.Text = "Select a Role:";
        this.Text = "Select Role";
        RoleInfo[] allRoleFunctions = ((WorkflowManager) Session.BPM.GetBpmManager(BpmCategory.Workflow)).GetAllRoleFunctions();
        if (allRoleFunctions != null && allRoleFunctions.Length != 0)
        {
          foreach (RoleInfo roleInfo in allRoleFunctions)
            this.gridView1.Items.Add(new GVItem()
            {
              SubItems = {
                [0] = {
                  Text = roleInfo.Name
                }
              }
            });
        }
        else
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "There are no Roles present to select", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          this.DialogResult = DialogResult.Cancel;
          return;
        }
      }
      else
      {
        List<string> definitionsOfConditionType = Session.ConfigurationManager.GetTrackingDefinitionsOfConditionType(strArray1[0]);
        if (definitionsOfConditionType != null)
        {
          foreach (string str2 in definitionsOfConditionType)
            this.gridView1.Items.Add(new GVItem()
            {
              SubItems = {
                [0] = {
                  Text = str2
                }
              }
            });
        }
        else
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "There are no Tracking status present to select", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          this.DialogResult = DialogResult.Cancel;
          return;
        }
      }
      if (this.gridView1.Items.Count > 0)
        this.gridView1.Items[0].Selected = true;
      this.gridView1.EndUpdate();
    }

    private void selectBtn_Click(object sender, EventArgs e)
    {
      if (this.gridView1.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You have to select one tracking status", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        this.selectedStatus = this.gridView1.SelectedItems[0].Text.Trim();
        this.DialogResult = DialogResult.OK;
      }
    }

    private void cancelBtn_Click(object sender, EventArgs e)
    {
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
      GVColumn gvColumn = new GVColumn();
      this.gridView1 = new GridView();
      this.selectBtn = new Button();
      this.cancelBtn = new Button();
      this.label1 = new Label();
      this.SuspendLayout();
      this.gridView1.AllowMultiselect = false;
      gvColumn.ImageIndex = -1;
      gvColumn.Name = "Column1";
      gvColumn.Text = "";
      gvColumn.Width = 550;
      this.gridView1.Columns.AddRange(new GVColumn[1]
      {
        gvColumn
      });
      this.gridView1.HeaderHeight = 0;
      this.gridView1.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gridView1.Location = new Point(12, 46);
      this.gridView1.Name = "gridView1";
      this.gridView1.Size = new Size(552, 386);
      this.gridView1.TabIndex = 0;
      this.selectBtn.Location = new Point(398, 452);
      this.selectBtn.Name = "selectBtn";
      this.selectBtn.Size = new Size(75, 23);
      this.selectBtn.TabIndex = 1;
      this.selectBtn.Text = "Select";
      this.selectBtn.UseVisualStyleBackColor = true;
      this.selectBtn.Click += new EventHandler(this.selectBtn_Click);
      this.cancelBtn.Location = new Point(489, 452);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 23);
      this.cancelBtn.TabIndex = 2;
      this.cancelBtn.Text = "Cancel";
      this.cancelBtn.UseVisualStyleBackColor = true;
      this.cancelBtn.Click += new EventHandler(this.cancelBtn_Click);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(12, 18);
      this.label1.Name = "label1";
      this.label1.Size = new Size((int) sbyte.MaxValue, 13);
      this.label1.TabIndex = 3;
      this.label1.Text = "Select a Tracking Status:";
      this.AutoScaleMode = AutoScaleMode.Inherit;
      this.ClientSize = new Size(586, 487);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.cancelBtn);
      this.Controls.Add((Control) this.selectBtn);
      this.Controls.Add((Control) this.gridView1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MinimizeBox = false;
      this.Name = nameof (StatusTrackingSelectorDialog);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Select Status Tracking";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
