// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.AutoLockLoanProgLockPlanEditForm
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class AutoLockLoanProgLockPlanEditForm : Form
  {
    private GridView currentView;
    private GVItem item;
    private IContainer components;
    private GroupContainer groupContainer1;
    private Panel pnlFannieMaeProductName;
    private TextBox txtEditItem;
    private Button btnCancel;
    private Button btnSave;

    public AutoLockLoanProgLockPlanEditForm(GridView currentView, GVItem item)
    {
      this.InitializeComponent();
      this.txtEditItem.Text = item.Text;
      this.item = item;
      this.currentView = currentView;
      this.setForm();
    }

    public GVItem Item => this.item;

    private void setForm()
    {
      if (this.currentView.Name == "gvLoanProg")
      {
        this.Text = "Edit Loan Program";
        this.groupContainer1.Text = "Loan Program";
      }
      else
      {
        if (!(this.currentView.Name == "gvLockPlanCode"))
          return;
        this.Text = "Edit Lock Plan Code";
        this.groupContainer1.Text = "Lock Plan Code";
      }
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      if (string.IsNullOrWhiteSpace(this.txtEditItem.Text))
      {
        switch (Utils.Dialog((IWin32Window) this, "Field entry is required during edit.  You must either enter a value or cancel the edit.  If you wish to remove the entry you will need to cancel this edit, highlight the field in the associated criteria table and then click the Delete icon.", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation))
        {
          case DialogResult.OK:
            this.txtEditItem.Text = this.item.Text;
            return;
          case DialogResult.Cancel:
            this.Close();
            return;
        }
      }
      if (this.currentView.Items.GetItemByTag((object) this.txtEditItem.Text) != null && this.currentView.Items.GetItemByTag((object) this.txtEditItem.Text).Tag != this.currentView.SelectedItems[0].Tag)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The Edit Name already exists. You must enter a unique name for " + this.groupContainer1.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        this.txtEditItem.Text = this.item.Text;
      }
      else
      {
        this.item.Text = this.txtEditItem.Text;
        this.DialogResult = DialogResult.OK;
      }
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
      this.groupContainer1 = new GroupContainer();
      this.pnlFannieMaeProductName = new Panel();
      this.txtEditItem = new TextBox();
      this.btnCancel = new Button();
      this.btnSave = new Button();
      this.groupContainer1.SuspendLayout();
      this.pnlFannieMaeProductName.SuspendLayout();
      this.SuspendLayout();
      this.groupContainer1.Controls.Add((Control) this.pnlFannieMaeProductName);
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(40, 12);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(465, 65);
      this.groupContainer1.TabIndex = 6;
      this.pnlFannieMaeProductName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlFannieMaeProductName.Controls.Add((Control) this.txtEditItem);
      this.pnlFannieMaeProductName.Location = new Point(4, 27);
      this.pnlFannieMaeProductName.Margin = new Padding(3, 10, 3, 3);
      this.pnlFannieMaeProductName.Name = "pnlFannieMaeProductName";
      this.pnlFannieMaeProductName.Size = new Size(457, 35);
      this.pnlFannieMaeProductName.TabIndex = 1;
      this.txtEditItem.Location = new Point(5, 7);
      this.txtEditItem.MaxLength = (int) byte.MaxValue;
      this.txtEditItem.Name = "txtEditItem";
      this.txtEditItem.Size = new Size(440, 20);
      this.txtEditItem.TabIndex = 1;
      this.btnCancel.Location = new Point(323, 83);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 8;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.btnSave.Location = new Point(139, 83);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(75, 23);
      this.btnSave.TabIndex = 7;
      this.btnSave.Text = "Save";
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(543, 110);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnSave);
      this.Controls.Add((Control) this.groupContainer1);
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AutoLockLoanProgLockPlanEditForm);
      this.ShowIcon = false;
      this.groupContainer1.ResumeLayout(false);
      this.pnlFannieMaeProductName.ResumeLayout(false);
      this.pnlFannieMaeProductName.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
