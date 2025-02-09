// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.InterestInfoForm
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class InterestInfoForm : Form
  {
    private LoanData loan;
    private Sessions.Session session = Session.DefaultInstance;
    private IContainer components;
    private Button btnCancel;
    private Button btnOK;
    private Label label1;
    private ComboBox cboDays;
    private ComboBox cboRounding;
    private Label label2;
    private PictureBox pboxAsterisk;
    private ToolTip fieldToolTip;
    private PictureBox pboxDownArrow;

    public InterestInfoForm(LoanData loan)
    {
      this.loan = loan;
      this.InitializeComponent();
      this.initForm();
      this.fieldToolTip.SetToolTip((Control) this.cboDays, "3549");
      this.fieldToolTip.SetToolTip((Control) this.cboRounding, "3550");
      ResourceManager resources = new ResourceManager(typeof (MIPDialog));
      PopupBusinessRules popupBusinessRules = new PopupBusinessRules(this.loan, resources, (Image) resources.GetObject("pboxAsterisk.Image"), (Image) resources.GetObject("pboxDownArrow.Image"), this.session);
      popupBusinessRules.SetBusinessRules((object) this.cboDays, "3549");
      popupBusinessRules.SetBusinessRules((object) this.cboRounding, "3550");
    }

    private void initForm()
    {
      this.cboDays.Text = this.loan.GetField("3549");
      this.cboRounding.Text = this.loan.GetField("3550");
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      try
      {
        this.loan.SetField("3549", this.cboDays.Text);
        this.loan.SetField("3550", this.cboRounding.Text);
        this.loan.TriggerCalculation("3549", this.cboDays.Text);
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, ex.Message);
      }
      this.DialogResult = DialogResult.OK;
    }

    private void InterestInfoForm_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar != '\u001B')
        return;
      this.Close();
    }

    private void cboDays_MouseClick(object sender, MouseEventArgs e)
    {
      Session.Application.GetService<IStatusDisplay>().DisplayFieldID("3549");
    }

    private void cboRounding_MouseClick(object sender, MouseEventArgs e)
    {
      Session.Application.GetService<IStatusDisplay>().DisplayFieldID("3550");
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (InterestInfoForm));
      this.btnCancel = new Button();
      this.btnOK = new Button();
      this.label1 = new Label();
      this.cboDays = new ComboBox();
      this.cboRounding = new ComboBox();
      this.label2 = new Label();
      this.pboxAsterisk = new PictureBox();
      this.fieldToolTip = new ToolTip(this.components);
      this.pboxDownArrow = new PictureBox();
      ((ISupportInitialize) this.pboxAsterisk).BeginInit();
      ((ISupportInitialize) this.pboxDownArrow).BeginInit();
      this.SuspendLayout();
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(223, 75);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 0;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnOK.Location = new Point(142, 75);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 1;
      this.btnOK.Text = "&OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(12, 12);
      this.label1.Name = "label1";
      this.label1.Size = new Size(83, 13);
      this.label1.TabIndex = 2;
      this.label1.Text = "Number of Days";
      this.cboDays.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboDays.FormattingEnabled = true;
      this.cboDays.Items.AddRange(new object[4]
      {
        (object) "",
        (object) "360",
        (object) "364",
        (object) "365"
      });
      this.cboDays.Location = new Point(155, 9);
      this.cboDays.Name = "cboDays";
      this.cboDays.Size = new Size(143, 21);
      this.cboDays.TabIndex = 0;
      this.cboDays.MouseClick += new MouseEventHandler(this.cboDays_MouseClick);
      this.cboRounding.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboRounding.FormattingEnabled = true;
      this.cboRounding.Items.AddRange(new object[4]
      {
        (object) "",
        (object) "2 Decimals",
        (object) "4 Decimals",
        (object) "No Rounding"
      });
      this.cboRounding.Location = new Point(155, 33);
      this.cboRounding.Name = "cboRounding";
      this.cboRounding.Size = new Size(143, 21);
      this.cboRounding.TabIndex = 1;
      this.cboRounding.MouseClick += new MouseEventHandler(this.cboRounding_MouseClick);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(12, 36);
      this.label2.Name = "label2";
      this.label2.Size = new Size(137, 13);
      this.label2.TabIndex = 4;
      this.label2.Text = "Per Diem Interest Rounding";
      this.pboxAsterisk.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.pboxAsterisk.Image = (Image) componentResourceManager.GetObject("pboxAsterisk.Image");
      this.pboxAsterisk.Location = new Point(15, 82);
      this.pboxAsterisk.Name = "pboxAsterisk";
      this.pboxAsterisk.Size = new Size(24, 12);
      this.pboxAsterisk.TabIndex = 18;
      this.pboxAsterisk.TabStop = false;
      this.pboxAsterisk.Visible = false;
      this.pboxDownArrow.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.pboxDownArrow.Image = (Image) componentResourceManager.GetObject("pboxDownArrow.Image");
      this.pboxDownArrow.Location = new Point(54, 77);
      this.pboxDownArrow.Name = "pboxDownArrow";
      this.pboxDownArrow.Size = new Size(17, 17);
      this.pboxDownArrow.TabIndex = 69;
      this.pboxDownArrow.TabStop = false;
      this.pboxDownArrow.Visible = false;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(310, 106);
      this.Controls.Add((Control) this.pboxDownArrow);
      this.Controls.Add((Control) this.pboxAsterisk);
      this.Controls.Add((Control) this.cboRounding);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.cboDays);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.btnCancel);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (InterestInfoForm);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Interest Calculation";
      this.KeyPress += new KeyPressEventHandler(this.InterestInfoForm_KeyPress);
      ((ISupportInitialize) this.pboxAsterisk).EndInit();
      ((ISupportInitialize) this.pboxDownArrow).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
