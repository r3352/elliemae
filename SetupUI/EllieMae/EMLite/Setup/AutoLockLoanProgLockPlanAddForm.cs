// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.AutoLockLoanProgLockPlanAddForm
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class AutoLockLoanProgLockPlanAddForm : Form
  {
    private GridView currentView;
    private List<GVItem> result;
    public bool isAddMore;
    private IContainer components;
    private Panel pnlFannieMaeProductName;
    private TextBox txtNewItem3;
    private TextBox txtNewItem2;
    private TextBox txtNewItem1;
    private TextBox txtNewItem7;
    private TextBox txtNewItem6;
    private TextBox txtNewItem5;
    private TextBox txtNewItem4;
    private Button button1;
    private Button button2;
    private Button button3;
    private GroupContainer groupContainer1;

    public AutoLockLoanProgLockPlanAddForm(GridView currentView)
    {
      this.InitializeComponent();
      this.currentView = currentView;
      this.result = new List<GVItem>();
      this.setForm();
    }

    public List<GVItem> Result => this.result;

    private void Add(object sender, EventArgs e)
    {
      this.addToListView();
      this.DialogResult = DialogResult.OK;
    }

    private void AddMore(object sender, EventArgs e)
    {
      this.addToListView();
      this.isAddMore = true;
      this.DialogResult = DialogResult.OK;
    }

    private void Cancel(object sender, EventArgs e) => this.DialogResult = DialogResult.Cancel;

    private void addToListView()
    {
      if (!string.IsNullOrWhiteSpace(this.txtNewItem1.Text))
        this.result.Add(new GVItem(this.txtNewItem1.Text));
      if (!string.IsNullOrWhiteSpace(this.txtNewItem2.Text))
        this.result.Add(new GVItem(this.txtNewItem2.Text));
      if (!string.IsNullOrWhiteSpace(this.txtNewItem3.Text))
        this.result.Add(new GVItem(this.txtNewItem3.Text));
      if (!string.IsNullOrWhiteSpace(this.txtNewItem4.Text))
        this.result.Add(new GVItem(this.txtNewItem4.Text));
      if (!string.IsNullOrWhiteSpace(this.txtNewItem5.Text))
        this.result.Add(new GVItem(this.txtNewItem5.Text));
      if (!string.IsNullOrWhiteSpace(this.txtNewItem6.Text))
        this.result.Add(new GVItem(this.txtNewItem6.Text));
      if (string.IsNullOrWhiteSpace(this.txtNewItem7.Text))
        return;
      this.result.Add(new GVItem(this.txtNewItem7.Text));
    }

    private void setForm()
    {
      if (this.currentView.Name == "gvLoanProg")
      {
        this.Text = "Add Loan Program";
        this.groupContainer1.Text = "Loan Program";
      }
      else
      {
        if (!(this.currentView.Name == "gvLockPlanCode"))
          return;
        this.Text = "Add Lock Plan Code";
        this.groupContainer1.Text = "Lock Plan Code";
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.pnlFannieMaeProductName = new Panel();
      this.txtNewItem7 = new TextBox();
      this.txtNewItem6 = new TextBox();
      this.txtNewItem5 = new TextBox();
      this.txtNewItem4 = new TextBox();
      this.txtNewItem3 = new TextBox();
      this.txtNewItem2 = new TextBox();
      this.txtNewItem1 = new TextBox();
      this.button1 = new Button();
      this.button2 = new Button();
      this.button3 = new Button();
      this.groupContainer1 = new GroupContainer();
      this.pnlFannieMaeProductName.SuspendLayout();
      this.groupContainer1.SuspendLayout();
      this.SuspendLayout();
      this.pnlFannieMaeProductName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlFannieMaeProductName.Controls.Add((Control) this.txtNewItem7);
      this.pnlFannieMaeProductName.Controls.Add((Control) this.txtNewItem6);
      this.pnlFannieMaeProductName.Controls.Add((Control) this.txtNewItem5);
      this.pnlFannieMaeProductName.Controls.Add((Control) this.txtNewItem4);
      this.pnlFannieMaeProductName.Controls.Add((Control) this.txtNewItem3);
      this.pnlFannieMaeProductName.Controls.Add((Control) this.txtNewItem2);
      this.pnlFannieMaeProductName.Controls.Add((Control) this.txtNewItem1);
      this.pnlFannieMaeProductName.Location = new Point(4, 27);
      this.pnlFannieMaeProductName.Margin = new Padding(3, 10, 3, 3);
      this.pnlFannieMaeProductName.Name = "pnlFannieMaeProductName";
      this.pnlFannieMaeProductName.Size = new Size(457, 184);
      this.pnlFannieMaeProductName.TabIndex = 1;
      this.txtNewItem7.Location = new Point(5, 160);
      this.txtNewItem7.MaxLength = (int) byte.MaxValue;
      this.txtNewItem7.Name = "txtNewItem7";
      this.txtNewItem7.Size = new Size(440, 20);
      this.txtNewItem7.TabIndex = 9;
      this.txtNewItem6.Location = new Point(4, 134);
      this.txtNewItem6.MaxLength = (int) byte.MaxValue;
      this.txtNewItem6.Name = "txtNewItem6";
      this.txtNewItem6.Size = new Size(440, 20);
      this.txtNewItem6.TabIndex = 8;
      this.txtNewItem5.Location = new Point(4, 109);
      this.txtNewItem5.MaxLength = (int) byte.MaxValue;
      this.txtNewItem5.Name = "txtNewItem5";
      this.txtNewItem5.Size = new Size(440, 20);
      this.txtNewItem5.TabIndex = 7;
      this.txtNewItem4.Location = new Point(4, 84);
      this.txtNewItem4.MaxLength = (int) byte.MaxValue;
      this.txtNewItem4.Name = "txtNewItem4";
      this.txtNewItem4.Size = new Size(440, 20);
      this.txtNewItem4.TabIndex = 6;
      this.txtNewItem3.Location = new Point(5, 61);
      this.txtNewItem3.MaxLength = (int) byte.MaxValue;
      this.txtNewItem3.Name = "txtNewItem3";
      this.txtNewItem3.Size = new Size(440, 20);
      this.txtNewItem3.TabIndex = 5;
      this.txtNewItem2.Location = new Point(5, 36);
      this.txtNewItem2.MaxLength = (int) byte.MaxValue;
      this.txtNewItem2.Name = "txtNewItem2";
      this.txtNewItem2.Size = new Size(440, 20);
      this.txtNewItem2.TabIndex = 3;
      this.txtNewItem1.Location = new Point(5, 11);
      this.txtNewItem1.MaxLength = (int) byte.MaxValue;
      this.txtNewItem1.Name = "txtNewItem1";
      this.txtNewItem1.Size = new Size(440, 20);
      this.txtNewItem1.TabIndex = 1;
      this.button1.Location = new Point(135, 232);
      this.button1.Name = "button1";
      this.button1.Size = new Size(75, 23);
      this.button1.TabIndex = 2;
      this.button1.Text = "Add";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new EventHandler(this.Add);
      this.button2.Location = new Point(228, 232);
      this.button2.Name = "button2";
      this.button2.Size = new Size(75, 23);
      this.button2.TabIndex = 3;
      this.button2.Text = "Add More";
      this.button2.UseVisualStyleBackColor = true;
      this.button2.Click += new EventHandler(this.AddMore);
      this.button3.Location = new Point(319, 232);
      this.button3.Name = "button3";
      this.button3.Size = new Size(75, 23);
      this.button3.TabIndex = 4;
      this.button3.Text = "Cancel";
      this.button3.UseVisualStyleBackColor = true;
      this.button3.Click += new EventHandler(this.Cancel);
      this.groupContainer1.Controls.Add((Control) this.pnlFannieMaeProductName);
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(30, 12);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(465, 214);
      this.groupContainer1.TabIndex = 5;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(543, 262);
      this.Controls.Add((Control) this.groupContainer1);
      this.Controls.Add((Control) this.button3);
      this.Controls.Add((Control) this.button2);
      this.Controls.Add((Control) this.button1);
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AutoLockLoanProgLockPlanAddForm);
      this.ShowIcon = false;
      this.pnlFannieMaeProductName.ResumeLayout(false);
      this.pnlFannieMaeProductName.PerformLayout();
      this.groupContainer1.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
