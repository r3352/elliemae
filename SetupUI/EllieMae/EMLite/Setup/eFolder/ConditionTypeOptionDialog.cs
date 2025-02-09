// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.eFolder.ConditionTypeOptionDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.eFolder
{
  public class ConditionTypeOptionDialog : Form
  {
    private Dictionary<string, int> existingOptions;
    private IContainer components;
    private Button btnCancel;
    private Button btnOk;
    private ToolTip tooltip;
    private Panel pnltop;
    private CheckBox chkisopen;
    private TextBox txtoption;
    private Label Option;

    public string option { get; set; }

    public bool IsTrackingOption { get; set; }

    public ConditionTypeOptionDialog(
      string Title,
      bool isTrackingOption,
      Dictionary<string, int> keyValues)
    {
      this.InitializeComponent();
      this.Text = Title;
      this.existingOptions = keyValues;
      this.IsTrackingOption = isTrackingOption;
      if (!isTrackingOption)
        return;
      this.chkisopen.Visible = true;
      this.Option.Text = "Status";
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      if (this.existingOptions.ContainsKey(this.txtoption.Text.Trim().ToLower()))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, this.txtoption.Text + " already exists. Please provide a unique Name", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        this.option = this.txtoption.Text;
        if (this.IsTrackingOption)
          this.IsTrackingOption = this.chkisopen.Checked;
        this.DialogResult = DialogResult.OK;
      }
    }

    private void txtoption_TextChanged(object sender, EventArgs e)
    {
      this.btnOk.Enabled = !string.IsNullOrEmpty(this.txtoption.Text) && !string.IsNullOrWhiteSpace(this.txtoption.Text);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      this.btnCancel = new Button();
      this.btnOk = new Button();
      this.tooltip = new ToolTip(this.components);
      this.pnltop = new Panel();
      this.Option = new Label();
      this.txtoption = new TextBox();
      this.chkisopen = new CheckBox();
      this.pnltop.SuspendLayout();
      this.SuspendLayout();
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(252, 59);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 24);
      this.btnCancel.TabIndex = 20;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOk.Enabled = false;
      this.btnOk.Location = new Point(171, 59);
      this.btnOk.Name = "btnOk";
      this.btnOk.Size = new Size(75, 24);
      this.btnOk.TabIndex = 10;
      this.btnOk.Text = "OK";
      this.btnOk.UseVisualStyleBackColor = true;
      this.btnOk.Click += new EventHandler(this.btnSave_Click);
      this.pnltop.Controls.Add((Control) this.chkisopen);
      this.pnltop.Controls.Add((Control) this.txtoption);
      this.pnltop.Controls.Add((Control) this.Option);
      this.pnltop.Dock = DockStyle.Top;
      this.pnltop.Location = new Point(0, 0);
      this.pnltop.Name = "pnltop";
      this.pnltop.Size = new Size(339, 58);
      this.pnltop.TabIndex = 8;
      this.pnltop.TabStop = true;
      this.Option.AutoSize = true;
      this.Option.Location = new Point(3, 11);
      this.Option.Name = "Option";
      this.Option.Size = new Size(44, 14);
      this.Option.TabIndex = 0;
      this.Option.Text = "Option :";
      this.txtoption.Location = new Point(53, 11);
      this.txtoption.Name = "txtoption";
      this.txtoption.Size = new Size(274, 20);
      this.txtoption.TabIndex = 2;
      this.txtoption.TextChanged += new EventHandler(this.txtoption_TextChanged);
      this.chkisopen.AutoSize = true;
      this.chkisopen.Location = new Point(53, 37);
      this.chkisopen.Name = "chkisopen";
      this.chkisopen.Size = new Size(63, 18);
      this.chkisopen.TabIndex = 5;
      this.chkisopen.Text = "Is Open";
      this.chkisopen.UseVisualStyleBackColor = true;
      this.chkisopen.Visible = false;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(339, 90);
      this.Controls.Add((Control) this.pnltop);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOk);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ConditionTypeOptionDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Add  Option";
      this.pnltop.ResumeLayout(false);
      this.pnltop.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
