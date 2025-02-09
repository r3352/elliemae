// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ExternalOriginatorManagement.AddEditTPOFee
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.BusinessRules;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Compiler;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.ExternalOriginatorManagement
{
  public class AddEditTPOFee : Form
  {
    private string advancedCodeXml;
    private Sessions.Session session;
    private ExternalFeeManagement fee;
    private string userID = "";
    private bool edit;
    private IContainer components;
    private Panel pnlAddFee;
    private Label lblTrigger;
    private Label lblEndDate;
    private Label lblStartDate;
    private Label lblCode;
    private Label lblFeeAmt;
    private Label lblDesc;
    private TextBox txtName;
    private Label lblName;
    private TextBox txtCode;
    private TextBox txtFeeAmt;
    private DatePicker dpEndDate;
    private RichTextBox rtDesc;
    private ComboBox cmbTrigger;
    private DatePicker dpStartDate;
    private Button btnOK;
    private Button btnCancel;
    private Label label2;
    private Label lblReq;
    private StandardIconButton btnSelect;
    private TextBox textConditionCode;
    private Label lblOf;
    private TextBox txtFeePlus;
    private Label lblPlus;
    private Label lblChannel;
    private TextBox textBox1;

    public AddEditTPOFee(SessionObjects obj, ExternalFeeManagement fee)
    {
      this.InitializeComponent();
      this.session = new Sessions.Session(obj.SessionID);
      this.userID = obj.UserID;
      this.fee = fee;
      this.edit = fee != null;
      if (!this.edit)
        this.fee = new ExternalFeeManagement();
      this.textConditionCode.Visible = this.btnSelect.Visible = false;
      this.ClientSize = new Size(409, 420);
      this.populateData();
    }

    public AddEditTPOFee(Sessions.Session session, ExternalFeeManagement fee)
    {
      this.InitializeComponent();
      this.session = session;
      this.fee = fee;
      this.userID = session.UserID;
      this.edit = fee != null;
      if (!this.edit)
        this.fee = new ExternalFeeManagement();
      else
        this.Text = "Edit TPO Fee";
      this.textConditionCode.Visible = this.btnSelect.Visible = false;
      this.ClientSize = new Size(409, 420);
      this.populateData();
    }

    private void populateData()
    {
      this.txtName.Text = this.fee.FeeName;
      this.rtDesc.Text = this.fee.Description;
      this.txtFeeAmt.Text = this.fee.FeePercent == 0.0 ? "" : this.fee.FeePercent.ToString();
      this.txtFeePlus.Text = this.fee.FeeAmount == 0.0 ? "" : this.fee.FeeAmount.ToString();
      this.txtCode.Text = this.fee.Code;
      if (this.fee.StartDate != DateTime.MinValue)
        this.dpStartDate.Value = this.fee.StartDate;
      if (this.fee.EndDate != DateTime.MinValue)
        this.dpEndDate.Value = this.fee.EndDate;
      this.cmbTrigger.SelectedIndex = this.fee.Condition;
      if (this.fee.Condition == 1)
      {
        this.textConditionCode.Text = this.fee.AdvancedCode;
        this.advancedCodeXml = this.fee.AdvancedCodeXml;
      }
      this.txtFeePlus_Leave((object) null, (EventArgs) null);
      this.txtFeeAmt_Leave((object) null, (EventArgs) null);
    }

    private void cmbTrigger_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.cmbTrigger.SelectedIndex == 1)
      {
        this.textConditionCode.Visible = this.btnSelect.Visible = true;
        this.ClientSize = new Size(409, 478);
      }
      else
      {
        this.textConditionCode.Visible = this.btnSelect.Visible = false;
        this.ClientSize = new Size(409, 420);
      }
    }

    private void btnSelect_Click(object sender, EventArgs e)
    {
      using (AdvConditionEditor advConditionEditor = new AdvConditionEditor(this.session, this.advancedCodeXml))
      {
        if (advConditionEditor.GetConditionScript() != this.textConditionCode.Text)
          advConditionEditor.ClearFilters();
        if (advConditionEditor.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.textConditionCode.Text = advConditionEditor.GetConditionScript();
        this.advancedCodeXml = advConditionEditor.GetConditionXml();
      }
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (this.txtName.Text.Trim() == "")
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Name cannot be blank.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else if ((this.txtFeeAmt.Text.Trim() == "0" || this.txtFeeAmt.Text.Trim() == "") && (this.txtFeePlus.Text.Trim() == "0" || this.txtFeePlus.Text.Trim() == ""))
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "Fee Amount cannot be blank.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else if (this.cmbTrigger.SelectedIndex == -1)
      {
        int num3 = (int) Utils.Dialog((IWin32Window) this, "Please select a condition for when the fee is triggered.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        if (this.cmbTrigger.SelectedIndex == 1 && !this.validateAdvancedCode())
          return;
        if (this.dpEndDate.Value < this.dpStartDate.Value && this.dpStartDate.Value != DateTime.MinValue && this.dpEndDate.Value != DateTime.MinValue)
        {
          int num4 = (int) Utils.Dialog((IWin32Window) this, "Start Date cannot be greater than End Date.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          this.dpStartDate.Focus();
        }
        else
        {
          if (!this.edit)
          {
            this.fee.DateCreated = DateTime.Now;
            this.fee.CreatedBy = this.userID;
          }
          this.fee.DateUpdated = DateTime.Now;
          this.fee.UpdatedBy = this.userID;
          this.fee.FeeName = this.txtName.Text;
          this.fee.Description = this.rtDesc.Text;
          this.fee.Channel = ExternalOriginatorEntityType.Correspondent;
          this.fee.FeePercent = this.txtFeeAmt.Text == "" ? 0.0 : Convert.ToDouble(this.txtFeeAmt.Text);
          this.fee.FeeAmount = this.txtFeePlus.Text == "" ? 0.0 : Convert.ToDouble(this.txtFeePlus.Text);
          this.fee.Code = this.txtCode.Text;
          this.fee.StartDate = this.dpStartDate.Value;
          this.fee.EndDate = this.dpEndDate.Value;
          this.fee.Condition = this.cmbTrigger.SelectedIndex;
          if (this.fee.Condition == 1)
          {
            this.fee.AdvancedCode = this.textConditionCode.Text;
            this.fee.AdvancedCodeXml = this.advancedCodeXml;
          }
          this.DialogResult = DialogResult.OK;
        }
      }
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
    }

    private void numericOnly_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar.Equals((object) Keys.Delete))
        return;
      char keyChar = e.KeyChar;
      if (keyChar.Equals('\b'))
        return;
      keyChar = e.KeyChar;
      if (keyChar.Equals('.'))
        return;
      if (!char.IsNumber(e.KeyChar))
        e.Handled = true;
      else
        e.Handled = false;
    }

    public ExternalFeeManagement Fees => this.fee;

    private void txtFeePlus_Enter(object sender, EventArgs e)
    {
      if (this.txtFeePlus.Text == "" || Convert.ToDouble(this.txtFeePlus.Text) == 0.0)
        return;
      this.txtFeePlus.Text = Convert.ToDouble(this.txtFeePlus.Text).ToString("0.00");
    }

    private void txtFeePlus_Leave(object sender, EventArgs e)
    {
      if (this.txtFeePlus.Text == "")
        return;
      try
      {
        if (Convert.ToDouble(this.txtFeePlus.Text) == 0.0)
          return;
        this.txtFeePlus.Text = Convert.ToDouble(this.txtFeePlus.Text).ToString("#,0.00");
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtFeePlus.Text = "";
        this.txtFeePlus.Focus();
      }
    }

    private void txtFeeAmt_Enter(object sender, EventArgs e)
    {
      if (this.txtFeeAmt.Text == "" || Convert.ToDouble(this.txtFeeAmt.Text) == 0.0)
        return;
      this.txtFeeAmt.Text = Convert.ToDouble(this.txtFeeAmt.Text).ToString("0.0000");
    }

    private void txtFeeAmt_Leave(object sender, EventArgs e)
    {
      if (this.txtFeeAmt.Text == "")
        return;
      try
      {
        if (Convert.ToDouble(this.txtFeeAmt.Text) == 0.0)
          return;
        this.txtFeeAmt.Text = Convert.ToDouble(this.txtFeeAmt.Text).ToString("#,0.0000");
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtFeeAmt.Text = "";
        this.txtFeeAmt.Focus();
      }
    }

    private bool validateAdvancedCode()
    {
      if (this.textConditionCode.Text == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this.ParentForm, "You must provide code to determine the conditions under which the fee is conditionally triggered.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      try
      {
        using (RuntimeContext context = RuntimeContext.Create())
          new AdvancedCodeCondition(this.textConditionCode.Text).CreateImplementation(context);
        return true;
      }
      catch (CompileException ex)
      {
        if (EnConfigurationSettings.GlobalSettings.Debug)
        {
          ErrorDialog.Display((Exception) ex);
          return false;
        }
        int num = (int) Utils.Dialog((IWin32Window) this.ParentForm, "Validation failed: the condition contains errors or is not a valid expression.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      catch (Exception ex)
      {
        if (EnConfigurationSettings.GlobalSettings.Debug)
        {
          ErrorDialog.Display(ex);
          return false;
        }
        int num = (int) Utils.Dialog((IWin32Window) this.ParentForm, "Error validating expression: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (AddEditTPOFee));
      this.pnlAddFee = new Panel();
      this.textBox1 = new TextBox();
      this.lblOf = new Label();
      this.txtFeePlus = new TextBox();
      this.lblPlus = new Label();
      this.btnSelect = new StandardIconButton();
      this.textConditionCode = new TextBox();
      this.label2 = new Label();
      this.lblReq = new Label();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.rtDesc = new RichTextBox();
      this.cmbTrigger = new ComboBox();
      this.dpEndDate = new DatePicker();
      this.dpStartDate = new DatePicker();
      this.txtCode = new TextBox();
      this.txtFeeAmt = new TextBox();
      this.lblTrigger = new Label();
      this.lblEndDate = new Label();
      this.lblStartDate = new Label();
      this.lblCode = new Label();
      this.lblFeeAmt = new Label();
      this.lblChannel = new Label();
      this.lblDesc = new Label();
      this.txtName = new TextBox();
      this.lblName = new Label();
      this.pnlAddFee.SuspendLayout();
      ((ISupportInitialize) this.btnSelect).BeginInit();
      this.SuspendLayout();
      this.pnlAddFee.Controls.Add((Control) this.textBox1);
      this.pnlAddFee.Controls.Add((Control) this.lblOf);
      this.pnlAddFee.Controls.Add((Control) this.txtFeePlus);
      this.pnlAddFee.Controls.Add((Control) this.lblPlus);
      this.pnlAddFee.Controls.Add((Control) this.btnSelect);
      this.pnlAddFee.Controls.Add((Control) this.textConditionCode);
      this.pnlAddFee.Controls.Add((Control) this.label2);
      this.pnlAddFee.Controls.Add((Control) this.lblReq);
      this.pnlAddFee.Controls.Add((Control) this.btnOK);
      this.pnlAddFee.Controls.Add((Control) this.btnCancel);
      this.pnlAddFee.Controls.Add((Control) this.rtDesc);
      this.pnlAddFee.Controls.Add((Control) this.cmbTrigger);
      this.pnlAddFee.Controls.Add((Control) this.dpEndDate);
      this.pnlAddFee.Controls.Add((Control) this.dpStartDate);
      this.pnlAddFee.Controls.Add((Control) this.txtCode);
      this.pnlAddFee.Controls.Add((Control) this.txtFeeAmt);
      this.pnlAddFee.Controls.Add((Control) this.lblTrigger);
      this.pnlAddFee.Controls.Add((Control) this.lblEndDate);
      this.pnlAddFee.Controls.Add((Control) this.lblStartDate);
      this.pnlAddFee.Controls.Add((Control) this.lblCode);
      this.pnlAddFee.Controls.Add((Control) this.lblFeeAmt);
      this.pnlAddFee.Controls.Add((Control) this.lblChannel);
      this.pnlAddFee.Controls.Add((Control) this.lblDesc);
      this.pnlAddFee.Controls.Add((Control) this.txtName);
      this.pnlAddFee.Controls.Add((Control) this.lblName);
      this.pnlAddFee.Dock = DockStyle.Fill;
      this.pnlAddFee.Location = new Point(0, 0);
      this.pnlAddFee.Name = "pnlAddFee";
      this.pnlAddFee.Size = new Size(393, 450);
      this.pnlAddFee.TabIndex = 0;
      this.textBox1.Location = new Point(113, 134);
      this.textBox1.Name = "textBox1";
      this.textBox1.ReadOnly = true;
      this.textBox1.Size = new Size(164, 20);
      this.textBox1.TabIndex = 39;
      this.textBox1.Text = "Correspondent";
      this.lblOf.AutoSize = true;
      this.lblOf.Location = new Point(284, 163);
      this.lblOf.Name = "lblOf";
      this.lblOf.Size = new Size(15, 13);
      this.lblOf.TabIndex = 38;
      this.lblOf.Text = "%";
      this.txtFeePlus.Location = new Point(114, 192);
      this.txtFeePlus.MaxLength = 10;
      this.txtFeePlus.Name = "txtFeePlus";
      this.txtFeePlus.ShortcutsEnabled = false;
      this.txtFeePlus.Size = new Size(164, 20);
      this.txtFeePlus.TabIndex = 6;
      this.txtFeePlus.Enter += new EventHandler(this.txtFeePlus_Enter);
      this.txtFeePlus.KeyPress += new KeyPressEventHandler(this.numericOnly_KeyPress);
      this.txtFeePlus.Leave += new EventHandler(this.txtFeePlus_Leave);
      this.lblPlus.AutoSize = true;
      this.lblPlus.Location = new Point(94, 196);
      this.lblPlus.Name = "lblPlus";
      this.lblPlus.Size = new Size(22, 13);
      this.lblPlus.TabIndex = 37;
      this.lblPlus.Text = "+ $";
      this.btnSelect.BackColor = Color.Transparent;
      this.btnSelect.Location = new Point(364, 340);
      this.btnSelect.MouseDownImage = (Image) null;
      this.btnSelect.Name = "btnSelect";
      this.btnSelect.Size = new Size(16, 16);
      this.btnSelect.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnSelect.TabIndex = 34;
      this.btnSelect.TabStop = false;
      this.btnSelect.Click += new EventHandler(this.btnSelect_Click);
      this.textConditionCode.Location = new Point(113, 340);
      this.textConditionCode.Multiline = true;
      this.textConditionCode.Name = "textConditionCode";
      this.textConditionCode.Size = new Size(245, 56);
      this.textConditionCode.TabIndex = 11;
      this.textConditionCode.Visible = false;
      this.label2.AutoSize = true;
      this.label2.BackColor = Color.Transparent;
      this.label2.ForeColor = Color.Red;
      this.label2.Location = new Point(76, 161);
      this.label2.Name = "label2";
      this.label2.Size = new Size(11, 13);
      this.label2.TabIndex = 21;
      this.label2.Text = "*";
      this.lblReq.AutoSize = true;
      this.lblReq.BackColor = Color.Transparent;
      this.lblReq.ForeColor = Color.Red;
      this.lblReq.Location = new Point(46, 26);
      this.lblReq.Name = "lblReq";
      this.lblReq.Size = new Size(11, 13);
      this.lblReq.TabIndex = 19;
      this.lblReq.Text = "*";
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Location = new Point(203, 415);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 12;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(293, 415);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 13;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.rtDesc.Location = new Point(113, 48);
      this.rtDesc.Name = "rtDesc";
      this.rtDesc.ScrollBars = RichTextBoxScrollBars.Vertical;
      this.rtDesc.Size = new Size(245, 81);
      this.rtDesc.TabIndex = 2;
      this.rtDesc.Text = "";
      this.cmbTrigger.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbTrigger.FormattingEnabled = true;
      this.cmbTrigger.Items.AddRange(new object[2]
      {
        (object) "Always",
        (object) "Conditional"
      });
      this.cmbTrigger.Location = new Point(113, 301);
      this.cmbTrigger.Name = "cmbTrigger";
      this.cmbTrigger.Size = new Size(165, 21);
      this.cmbTrigger.TabIndex = 10;
      this.cmbTrigger.SelectedIndexChanged += new EventHandler(this.cmbTrigger_SelectedIndexChanged);
      this.dpEndDate.BackColor = SystemColors.Window;
      this.dpEndDate.Location = new Point(113, 274);
      this.dpEndDate.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dpEndDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpEndDate.Name = "dpEndDate";
      this.dpEndDate.Size = new Size(165, 21);
      this.dpEndDate.TabIndex = 9;
      this.dpEndDate.ToolTip = "";
      this.dpEndDate.Value = new DateTime(0L);
      this.dpStartDate.BackColor = SystemColors.Window;
      this.dpStartDate.Location = new Point(113, 246);
      this.dpStartDate.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dpStartDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpStartDate.Name = "dpStartDate";
      this.dpStartDate.Size = new Size(165, 21);
      this.dpStartDate.TabIndex = 8;
      this.dpStartDate.ToolTip = "";
      this.dpStartDate.Value = new DateTime(0L);
      this.txtCode.Location = new Point(114, 219);
      this.txtCode.MaxLength = 64;
      this.txtCode.Name = "txtCode";
      this.txtCode.Size = new Size(165, 20);
      this.txtCode.TabIndex = 7;
      this.txtFeeAmt.Location = new Point(114, 160);
      this.txtFeeAmt.MaxLength = 10;
      this.txtFeeAmt.Name = "txtFeeAmt";
      this.txtFeeAmt.ShortcutsEnabled = false;
      this.txtFeeAmt.Size = new Size(165, 20);
      this.txtFeeAmt.TabIndex = 4;
      this.txtFeeAmt.Enter += new EventHandler(this.txtFeeAmt_Enter);
      this.txtFeeAmt.KeyPress += new KeyPressEventHandler(this.numericOnly_KeyPress);
      this.txtFeeAmt.Leave += new EventHandler(this.txtFeeAmt_Leave);
      this.lblTrigger.AutoSize = true;
      this.lblTrigger.Location = new Point(16, 304);
      this.lblTrigger.Name = "lblTrigger";
      this.lblTrigger.Size = new Size(83, 13);
      this.lblTrigger.TabIndex = 8;
      this.lblTrigger.Text = "Fee is Triggered";
      this.lblEndDate.AutoSize = true;
      this.lblEndDate.Location = new Point(16, 279);
      this.lblEndDate.Name = "lblEndDate";
      this.lblEndDate.Size = new Size(52, 13);
      this.lblEndDate.TabIndex = 7;
      this.lblEndDate.Text = "End Date";
      this.lblStartDate.AutoSize = true;
      this.lblStartDate.Location = new Point(16, 251);
      this.lblStartDate.Name = "lblStartDate";
      this.lblStartDate.Size = new Size(55, 13);
      this.lblStartDate.TabIndex = 6;
      this.lblStartDate.Text = "Start Date";
      this.lblCode.AutoSize = true;
      this.lblCode.Location = new Point(16, 220);
      this.lblCode.Name = "lblCode";
      this.lblCode.Size = new Size(32, 13);
      this.lblCode.TabIndex = 5;
      this.lblCode.Text = "Code";
      this.lblFeeAmt.AutoSize = true;
      this.lblFeeAmt.Location = new Point(16, 162);
      this.lblFeeAmt.Name = "lblFeeAmt";
      this.lblFeeAmt.Size = new Size(64, 13);
      this.lblFeeAmt.TabIndex = 4;
      this.lblFeeAmt.Text = "Fee Amount";
      this.lblChannel.AutoSize = true;
      this.lblChannel.Location = new Point(16, 137);
      this.lblChannel.Name = "lblChannel";
      this.lblChannel.Size = new Size(46, 13);
      this.lblChannel.TabIndex = 3;
      this.lblChannel.Text = "Channel";
      this.lblDesc.AutoSize = true;
      this.lblDesc.Location = new Point(16, 51);
      this.lblDesc.Name = "lblDesc";
      this.lblDesc.Size = new Size(60, 13);
      this.lblDesc.TabIndex = 2;
      this.lblDesc.Text = "Description";
      this.txtName.Location = new Point(113, 23);
      this.txtName.MaxLength = 64;
      this.txtName.Name = "txtName";
      this.txtName.Size = new Size(245, 20);
      this.txtName.TabIndex = 1;
      this.lblName.AutoSize = true;
      this.lblName.Location = new Point(16, 26);
      this.lblName.Name = "lblName";
      this.lblName.Size = new Size(35, 13);
      this.lblName.TabIndex = 0;
      this.lblName.Text = "Name";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(393, 450);
      this.Controls.Add((Control) this.pnlAddFee);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AddEditTPOFee);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Add New TPO Fee";
      this.pnlAddFee.ResumeLayout(false);
      this.pnlAddFee.PerformLayout();
      ((ISupportInitialize) this.btnSelect).EndInit();
      this.ResumeLayout(false);
    }
  }
}
