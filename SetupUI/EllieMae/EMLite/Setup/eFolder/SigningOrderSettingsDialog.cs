// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.eFolder.SigningOrderSettingsDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.eFolder
{
  public class SigningOrderSettingsDialog : Form
  {
    private EDisclosureSignOrderSetup signOrderSetup;
    private IContainer components;
    private GroupContainer gcStates;
    private GridView gvStates;
    private Button btnOK;
    private Button btnCancel;

    public SigningOrderSettingsDialog(EDisclosureSignOrderSetup signOrderSetup)
    {
      this.InitializeComponent();
      this.signOrderSetup = signOrderSetup;
      this.initStateList();
    }

    private void initStateList()
    {
      foreach (KeyValuePair<string, bool> state in this.signOrderSetup.States)
        this.gvStates.Items.Add(new GVItem()
        {
          SubItems = {
            [0] = {
              Checked = state.Value
            },
            [1] = {
              Value = (object) state.Key
            }
          }
        });
      this.gvStates.Sort(1, SortOrder.Ascending);
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvStates.Items)
        this.signOrderSetup.States[gvItem.SubItems[1].Value.ToString()] = gvItem.Checked;
      this.DialogResult = DialogResult.OK;
    }

    private void btnCancel_Click(object sender, EventArgs e)
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
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      this.gcStates = new GroupContainer();
      this.gvStates = new GridView();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.gcStates.SuspendLayout();
      this.SuspendLayout();
      this.gcStates.Controls.Add((Control) this.gvStates);
      this.gcStates.HeaderForeColor = SystemColors.ControlText;
      this.gcStates.Location = new Point(12, 12);
      this.gcStates.Name = "gcStates";
      this.gcStates.Size = new Size(534, 460);
      this.gcStates.TabIndex = 0;
      this.gcStates.Text = "Select States Where Loan Officer will eSign eDisclosures before all other parties";
      this.gvStates.AllowColumnResize = false;
      this.gvStates.BorderStyle = BorderStyle.None;
      gvColumn1.CheckBoxes = true;
      gvColumn1.ColumnHeaderCheckbox = true;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "colCheckbox";
      gvColumn1.Text = "";
      gvColumn1.Width = 26;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "colState";
      gvColumn2.SpringToFit = true;
      gvColumn2.Text = "State";
      gvColumn2.Width = 506;
      this.gvStates.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.gvStates.Dock = DockStyle.Fill;
      this.gvStates.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvStates.Location = new Point(1, 26);
      this.gvStates.Name = "gvStates";
      this.gvStates.Size = new Size(532, 433);
      this.gvStates.TabIndex = 0;
      this.btnOK.Location = new Point(389, 487);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(70, 23);
      this.btnOK.TabIndex = 1;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(475, 487);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(70, 23);
      this.btnCancel.TabIndex = 2;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(554, 524);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.gcStates);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (SigningOrderSettingsDialog);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Signing order setting";
      this.gcStates.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
