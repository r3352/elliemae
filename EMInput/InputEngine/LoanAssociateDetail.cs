// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.LoanAssociateDetail
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.RemotingServices.Acl;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class LoanAssociateDetail : UserControl
  {
    private const string className = "FileContactForm";
    private static readonly string sw = Tracing.SwInputEngine;
    private Label label12;
    private Label label4;
    private Label label1;
    private IContainer components;
    private TextBox txtName;
    private TextBox txtPhone;
    private TextBox txtEmail;
    private CheckBox checkBoxAccess;
    private MilestoneLog ml;
    private MilestoneFreeRoleLog msfl;
    private LoanData loan;
    private Label label3;
    private TextBox txtFax;
    private Label label5;
    private TextBox txtCellPhone;
    private GroupContainer groupContainer1;
    private Label label2;
    private StandardIconButton btnEmail;
    private StandardIconButton btnCell;
    private StandardIconButton btnPhone;
    private ToolTip toolTip1;
    private StandardIconButton buttonSelect;
    private StandardIconButton btnFax;
    private TextBox txtTitle;
    private Label label6;
    private MilestoneLog precedingMl;

    public event EventHandler FieldOnKeyUp;

    public event EventHandler FieldsRefresh;

    public LoanAssociateDetail(
      MilestoneLog ml,
      MilestoneLog precedingMl,
      string loanAssociate,
      LoanData loan,
      bool readOnly)
    {
      this.ml = ml;
      this.loan = loan;
      this.precedingMl = precedingMl;
      this.Dock = DockStyle.Fill;
      this.InitializeComponent();
      this.groupContainer1.Text = loanAssociate;
      this.buttonSelect.Visible = false;
      this.txtName.Text = ml.LoanAssociateName;
      if (ml.LoanAssociateType == LoanAssociateType.Group)
        this.txtName.Text += " (Group)";
      this.txtPhone.Text = ml.LoanAssociatePhone;
      this.txtCellPhone.Text = ml.LoanAssociateCellPhone;
      this.txtFax.Text = ml.LoanAssociateFax;
      this.txtEmail.Text = ml.LoanAssociateEmail;
      this.txtTitle.Text = ml.LoanAssociateTitle;
      if ((ml.LoanAssociateName ?? "") == "" || !ml.Done)
      {
        this.checkBoxAccess.Enabled = false;
        this.checkBoxAccess.Checked = false;
      }
      else
      {
        if (this.haveGrantWriteAccessRight() && !readOnly && ml.LoanAssociateID != Session.UserInfo.Userid)
          this.checkBoxAccess.Enabled = true;
        else
          this.checkBoxAccess.Enabled = false;
        this.checkBoxAccess.Checked = this.ml.LoanAssociateAccess;
      }
      this.checkBoxAccess.CheckedChanged += new EventHandler(this.checkBoxAccess_CheckedChanged);
    }

    public LoanAssociateDetail(
      MilestoneFreeRoleLog msfl,
      string loanAssociate,
      LoanData loan,
      bool readOnly)
    {
      this.ml = (MilestoneLog) null;
      this.msfl = msfl;
      this.loan = loan;
      this.Dock = DockStyle.Fill;
      this.InitializeComponent();
      this.groupContainer1.Text = "Milestone Free " + loanAssociate;
      this.txtName.Text = msfl.LoanAssociateName;
      if (msfl.LoanAssociateType == LoanAssociateType.Group)
        this.txtName.Text += " (Group)";
      this.txtPhone.Text = msfl.LoanAssociatePhone;
      this.txtCellPhone.Text = msfl.LoanAssociateCellPhone;
      this.txtFax.Text = msfl.LoanAssociateFax;
      this.txtEmail.Text = msfl.LoanAssociateEmail;
      this.txtTitle.Text = msfl.LoanAssociateTitle;
      if (this.haveMilestoneFreeRoleAssignRight(msfl.RoleID))
        this.buttonSelect.Enabled = true;
      else
        this.buttonSelect.Enabled = false;
      if (readOnly)
        this.buttonSelect.Visible = false;
      if (this.haveGrantWriteAccessRight() && !readOnly && msfl.LoanAssociateID != "")
        this.checkBoxAccess.Enabled = true;
      else
        this.checkBoxAccess.Enabled = false;
      this.checkBoxAccess.Checked = this.msfl.LoanAssociateAccess;
      this.checkBoxAccess.CheckedChanged += new EventHandler(this.checkBoxAccess_CheckedChanged);
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
      this.txtName = new TextBox();
      this.label12 = new Label();
      this.label4 = new Label();
      this.txtPhone = new TextBox();
      this.txtEmail = new TextBox();
      this.label1 = new Label();
      this.checkBoxAccess = new CheckBox();
      this.txtFax = new TextBox();
      this.label3 = new Label();
      this.label5 = new Label();
      this.txtCellPhone = new TextBox();
      this.groupContainer1 = new GroupContainer();
      this.txtTitle = new TextBox();
      this.label6 = new Label();
      this.btnFax = new StandardIconButton();
      this.buttonSelect = new StandardIconButton();
      this.btnEmail = new StandardIconButton();
      this.btnCell = new StandardIconButton();
      this.btnPhone = new StandardIconButton();
      this.label2 = new Label();
      this.toolTip1 = new ToolTip(this.components);
      this.groupContainer1.SuspendLayout();
      ((ISupportInitialize) this.btnFax).BeginInit();
      ((ISupportInitialize) this.buttonSelect).BeginInit();
      ((ISupportInitialize) this.btnEmail).BeginInit();
      ((ISupportInitialize) this.btnCell).BeginInit();
      ((ISupportInitialize) this.btnPhone).BeginInit();
      this.SuspendLayout();
      this.txtName.Location = new Point(84, 33);
      this.txtName.Name = "txtName";
      this.txtName.ReadOnly = true;
      this.txtName.Size = new Size(232, 20);
      this.txtName.TabIndex = 22;
      this.txtName.TabStop = false;
      this.txtName.Tag = (object) "";
      this.label12.AutoSize = true;
      this.label12.Location = new Point(6, 36);
      this.label12.Name = "label12";
      this.label12.Size = new Size(35, 13);
      this.label12.TabIndex = 26;
      this.label12.Text = "Name";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(6, 82);
      this.label4.Name = "label4";
      this.label4.Size = new Size(67, 13);
      this.label4.TabIndex = 25;
      this.label4.Text = "Work Phone";
      this.txtPhone.Location = new Point(84, 79);
      this.txtPhone.Name = "txtPhone";
      this.txtPhone.Size = new Size(232, 20);
      this.txtPhone.TabIndex = 0;
      this.txtPhone.Tag = (object) "phone";
      this.txtPhone.TextChanged += new EventHandler(this.txtPhone_TextChanged);
      this.txtPhone.KeyDown += new KeyEventHandler(this.txtPhone_KeyDown);
      this.txtPhone.KeyUp += new KeyEventHandler(this.txtPhone_KeyDown);
      this.txtEmail.Location = new Point(84, 123);
      this.txtEmail.Name = "txtEmail";
      this.txtEmail.Size = new Size(232, 20);
      this.txtEmail.TabIndex = 2;
      this.txtEmail.Tag = (object) "email";
      this.txtEmail.TextChanged += new EventHandler(this.txtEmail_TextChanged);
      this.txtEmail.KeyDown += new KeyEventHandler(this.txtEmail_KeyDown);
      this.txtEmail.KeyUp += new KeyEventHandler(this.txtEmail_KeyDown);
      this.txtEmail.LostFocus += new EventHandler(this.txtEmail_LostFocus);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(6, 126);
      this.label1.Name = "label1";
      this.label1.Size = new Size(32, 13);
      this.label1.TabIndex = 29;
      this.label1.Text = "Email";
      this.checkBoxAccess.Location = new Point(108, 169);
      this.checkBoxAccess.Name = "checkBoxAccess";
      this.checkBoxAccess.Size = new Size(18, 20);
      this.checkBoxAccess.TabIndex = 4;
      this.txtFax.Location = new Point(84, 145);
      this.txtFax.Name = "txtFax";
      this.txtFax.Size = new Size(232, 20);
      this.txtFax.TabIndex = 3;
      this.txtFax.Tag = (object) "email";
      this.txtFax.TextChanged += new EventHandler(this.txtFax_TextChanged);
      this.txtFax.KeyDown += new KeyEventHandler(this.txtFax_KeyDown);
      this.txtFax.KeyUp += new KeyEventHandler(this.txtFax_KeyDown);
      this.label3.AutoSize = true;
      this.label3.Location = new Point(6, 148);
      this.label3.Name = "label3";
      this.label3.Size = new Size(24, 13);
      this.label3.TabIndex = 32;
      this.label3.Text = "Fax";
      this.label5.AutoSize = true;
      this.label5.Location = new Point(6, 104);
      this.label5.Name = "label5";
      this.label5.Size = new Size(58, 13);
      this.label5.TabIndex = 33;
      this.label5.Text = "Cell Phone";
      this.txtCellPhone.Location = new Point(84, 101);
      this.txtCellPhone.Name = "txtCellPhone";
      this.txtCellPhone.Size = new Size(232, 20);
      this.txtCellPhone.TabIndex = 1;
      this.txtCellPhone.TextChanged += new EventHandler(this.txtCellPhone_TextChanged);
      this.txtCellPhone.KeyDown += new KeyEventHandler(this.txtCellPhone_KeyDown);
      this.txtCellPhone.KeyUp += new KeyEventHandler(this.txtCellPhone_KeyDown);
      this.groupContainer1.Borders = AnchorStyles.Top;
      this.groupContainer1.Controls.Add((Control) this.txtTitle);
      this.groupContainer1.Controls.Add((Control) this.label6);
      this.groupContainer1.Controls.Add((Control) this.btnFax);
      this.groupContainer1.Controls.Add((Control) this.buttonSelect);
      this.groupContainer1.Controls.Add((Control) this.btnEmail);
      this.groupContainer1.Controls.Add((Control) this.btnCell);
      this.groupContainer1.Controls.Add((Control) this.btnPhone);
      this.groupContainer1.Controls.Add((Control) this.label2);
      this.groupContainer1.Controls.Add((Control) this.txtCellPhone);
      this.groupContainer1.Controls.Add((Control) this.label4);
      this.groupContainer1.Controls.Add((Control) this.label5);
      this.groupContainer1.Controls.Add((Control) this.label12);
      this.groupContainer1.Controls.Add((Control) this.txtFax);
      this.groupContainer1.Controls.Add((Control) this.txtName);
      this.groupContainer1.Controls.Add((Control) this.label3);
      this.groupContainer1.Controls.Add((Control) this.txtPhone);
      this.groupContainer1.Controls.Add((Control) this.checkBoxAccess);
      this.groupContainer1.Controls.Add((Control) this.label1);
      this.groupContainer1.Controls.Add((Control) this.txtEmail);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(541, 187);
      this.groupContainer1.TabIndex = 35;
      this.txtTitle.Location = new Point(84, 56);
      this.txtTitle.Name = "txtTitle";
      this.txtTitle.ReadOnly = true;
      this.txtTitle.Size = new Size(232, 20);
      this.txtTitle.TabIndex = 42;
      this.txtTitle.TabStop = false;
      this.txtTitle.Tag = (object) "";
      this.label6.AutoSize = true;
      this.label6.Location = new Point(6, 59);
      this.label6.Name = "label6";
      this.label6.Size = new Size(47, 13);
      this.label6.TabIndex = 41;
      this.label6.Text = "Job Title";
      this.btnFax.BackColor = Color.Transparent;
      this.btnFax.Location = new Point(320, 147);
      this.btnFax.MouseDownImage = (Image) null;
      this.btnFax.Name = "btnFax";
      this.btnFax.Size = new Size(16, 16);
      this.btnFax.StandardButtonType = StandardIconButton.ButtonType.FaxPhoneButton;
      this.btnFax.TabIndex = 40;
      this.btnFax.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnFax, "Open the Conversation Log");
      this.btnFax.Click += new EventHandler(this.btnFax_Click);
      this.buttonSelect.BackColor = Color.Transparent;
      this.buttonSelect.Location = new Point(320, 35);
      this.buttonSelect.MouseDownImage = (Image) null;
      this.buttonSelect.Name = "buttonSelect";
      this.buttonSelect.Size = new Size(16, 16);
      this.buttonSelect.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.buttonSelect.TabIndex = 39;
      this.buttonSelect.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.buttonSelect, "Open the Conversation Log");
      this.buttonSelect.Click += new EventHandler(this.buttonSelect_Click);
      this.btnEmail.BackColor = Color.Transparent;
      this.btnEmail.Location = new Point(320, 125);
      this.btnEmail.MouseDownImage = (Image) null;
      this.btnEmail.Name = "btnEmail";
      this.btnEmail.Size = new Size(16, 16);
      this.btnEmail.StandardButtonType = StandardIconButton.ButtonType.EmailButton;
      this.btnEmail.TabIndex = 38;
      this.btnEmail.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnEmail, "Open the Conversation Log");
      this.btnEmail.Click += new EventHandler(this.btnEmail_Click);
      this.btnCell.BackColor = Color.Transparent;
      this.btnCell.Location = new Point(320, 103);
      this.btnCell.MouseDownImage = (Image) null;
      this.btnCell.Name = "btnCell";
      this.btnCell.Size = new Size(16, 16);
      this.btnCell.StandardButtonType = StandardIconButton.ButtonType.CellPhoneButton;
      this.btnCell.TabIndex = 37;
      this.btnCell.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnCell, "Open the Conversation Log");
      this.btnCell.Click += new EventHandler(this.btnCell_Click);
      this.btnPhone.BackColor = Color.Transparent;
      this.btnPhone.Location = new Point(320, 81);
      this.btnPhone.MouseDownImage = (Image) null;
      this.btnPhone.Name = "btnPhone";
      this.btnPhone.Size = new Size(16, 16);
      this.btnPhone.StandardButtonType = StandardIconButton.ButtonType.PhoneButton;
      this.btnPhone.TabIndex = 36;
      this.btnPhone.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnPhone, "Open the Conversation Log");
      this.btnPhone.Click += new EventHandler(this.btnPhone_Click);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(6, 172);
      this.label2.Name = "label2";
      this.label2.Size = new Size(95, 13);
      this.label2.TabIndex = 35;
      this.label2.Text = "Give Write Access";
      this.Controls.Add((Control) this.groupContainer1);
      this.Name = nameof (LoanAssociateDetail);
      this.Size = new Size(541, 187);
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      ((ISupportInitialize) this.btnFax).EndInit();
      ((ISupportInitialize) this.buttonSelect).EndInit();
      ((ISupportInitialize) this.btnEmail).EndInit();
      ((ISupportInitialize) this.btnCell).EndInit();
      ((ISupportInitialize) this.btnPhone).EndInit();
      this.ResumeLayout(false);
    }

    private void txtPhone_TextChanged(object sender, EventArgs e)
    {
      if (this.ml != null)
      {
        bool flag = this.ml.LoanAssociatePhone == this.txtPhone.Text.Trim();
        this.ml.LoanAssociatePhone = this.txtPhone.Text.Trim();
        this.loan.Calculator.SetVerificationTitle(this.ml);
        if (flag)
          return;
        this.loan.Calculator.UpdateLenderRepresentative((LoanAssociateLog) this.ml, "associateWorkPhone");
      }
      else
      {
        bool flag = this.msfl.LoanAssociatePhone == this.txtPhone.Text.Trim();
        this.msfl.LoanAssociatePhone = this.txtPhone.Text.Trim();
        this.loan.Calculator.SetVerificationTitle(this.msfl);
        if (flag)
          return;
        this.loan.Calculator.UpdateLenderRepresentative((LoanAssociateLog) this.msfl, "associateWorkPhone");
      }
    }

    private void txtFax_TextChanged(object sender, EventArgs e)
    {
      if (this.ml != null)
      {
        this.ml.LoanAssociateFax = this.txtFax.Text.Trim();
        this.loan.Calculator.SetVerificationTitle(this.ml);
      }
      else
      {
        this.msfl.LoanAssociateFax = this.txtFax.Text.Trim();
        this.loan.Calculator.SetVerificationTitle(this.msfl);
      }
    }

    private void txtEmail_LostFocus(object sender, EventArgs e)
    {
      if (this.txtEmail.Text.Trim() != "" && Utils.ValidateEmail(this.txtEmail.Text.Trim()))
      {
        if (this.ml != null)
          this.ml.LoanAssociateEmail = this.txtEmail.Text.Trim();
        else
          this.msfl.LoanAssociateEmail = this.txtEmail.Text.Trim();
      }
      else
      {
        if (this.txtEmail.Text.Trim() != "")
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "The value '" + this.txtEmail.Text.Trim() + "' is not a valid email format", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          this.txtEmail.Text = "";
        }
        if (this.ml != null)
          this.ml.LoanAssociateEmail = "";
        else
          this.msfl.LoanAssociateEmail = "";
      }
    }

    private void txtEmail_TextChanged(object sender, EventArgs e)
    {
      if (this.ml != null)
      {
        bool flag = this.ml.LoanAssociateEmail == this.txtEmail.Text.Trim();
        this.ml.LoanAssociateEmail = this.txtEmail.Text.Trim();
        if (flag)
          return;
        this.loan.Calculator.UpdateLenderRepresentative((LoanAssociateLog) this.ml, "associateEmail");
      }
      else
      {
        bool flag = this.msfl.LoanAssociateEmail == this.txtEmail.Text.Trim();
        this.msfl.LoanAssociateEmail = this.txtEmail.Text.Trim();
        if (flag)
          return;
        this.loan.Calculator.UpdateLenderRepresentative((LoanAssociateLog) this.msfl, "associateEmail");
      }
    }

    private void checkBoxAccess_CheckedChanged(object sender, EventArgs e)
    {
      if (this.ml != null)
        this.ml.LoanAssociateAccess = this.checkBoxAccess.Checked;
      else
        this.msfl.LoanAssociateAccess = this.checkBoxAccess.Checked;
    }

    private bool haveGrantWriteAccessRight()
    {
      ToolsAclManager aclManager = (ToolsAclManager) Session.ACL.GetAclManager(AclCategory.ToolsGrantWriteAccess);
      if (this.msfl != null)
        return aclManager.GetAccessibleToolsAclInfo(Session.UserID, Session.UserInfo.UserPersonas, this.msfl.RoleID, "").Access == 1;
      if (this.precedingMl == null)
      {
        if (!(this.ml.Stage == "Started") || Session.UserInfo.IsSuperAdministrator())
          return true;
        EllieMae.EMLite.Workflow.Milestone milestoneInfo = this.getMilestoneInfo(this.ml.MilestoneID);
        return milestoneInfo == null || aclManager.GetAccessibleToolsAclInfo(Session.UserID, Session.UserInfo.UserPersonas, -1, milestoneInfo.MilestoneID).Access == 1;
      }
      EllieMae.EMLite.Workflow.Milestone milestoneInfo1 = this.getMilestoneInfo(this.ml.MilestoneID);
      return milestoneInfo1 == null || Session.UserInfo.IsSuperAdministrator() || aclManager.GetAccessibleToolsAclInfo(Session.UserID, Session.UserInfo.UserPersonas, -1, milestoneInfo1.MilestoneID).Access == 1;
    }

    private bool haveMilestoneFreeRoleAssignRight(int roleID)
    {
      return ((MilestoneFreeRoleAclManager) Session.ACL.GetAclManager(AclCategory.MilestonesFreeRole)).GetPermission(roleID, Session.UserInfo);
    }

    private EllieMae.EMLite.Workflow.Milestone getMilestoneInfo(string id)
    {
      return ((MilestoneTemplatesBpmManager) Session.BPM.GetBpmManager(BpmCategory.Milestones)).GetMilestoneByID(id);
    }

    private void buttonSelect_Click(object sender, EventArgs e)
    {
      using (ProcessorSelectionDialog processorSelectionDialog = new ProcessorSelectionDialog((LoanAssociateLog) this.msfl, false))
      {
        if (processorSelectionDialog.ShowDialog((IWin32Window) this) == DialogResult.OK)
          this.refreshAssociateDetails();
      }
      if (this.FieldsRefresh == null)
        return;
      this.FieldsRefresh((object) this.msfl, e);
    }

    private void refreshAssociateDetails()
    {
      this.txtName.Text = this.msfl.LoanAssociateName;
      if (this.msfl.LoanAssociateType == LoanAssociateType.Group)
        this.txtName.Text += " (Group)";
      this.txtPhone.TextChanged -= new EventHandler(this.txtPhone_TextChanged);
      this.txtFax.TextChanged -= new EventHandler(this.txtFax_TextChanged);
      this.txtPhone.Text = this.msfl.LoanAssociatePhone;
      this.txtFax.Text = this.msfl.LoanAssociateFax;
      this.txtCellPhone.Text = this.msfl.LoanAssociateCellPhone;
      this.txtEmail.Text = this.msfl.LoanAssociateEmail;
      this.txtTitle.Text = this.msfl.LoanAssociateTitle;
      this.loan.Calculator.SetVerificationTitle(this.msfl);
      this.checkBoxAccess.Enabled = this.haveGrantWriteAccessRight();
      this.txtPhone.TextChanged += new EventHandler(this.txtPhone_TextChanged);
      this.txtFax.TextChanged += new EventHandler(this.txtFax_TextChanged);
    }

    private void txtCellPhone_TextChanged(object sender, EventArgs e)
    {
      if (this.ml != null)
      {
        bool flag = this.ml.LoanAssociateCellPhone == this.txtCellPhone.Text.Trim();
        this.ml.LoanAssociateCellPhone = this.txtCellPhone.Text.Trim();
        if (flag)
          return;
        this.loan.Calculator.UpdateLenderRepresentative((LoanAssociateLog) this.ml, "associateCell");
      }
      else
      {
        bool flag = this.msfl.LoanAssociateCellPhone == this.txtCellPhone.Text.Trim();
        this.msfl.LoanAssociateCellPhone = this.txtCellPhone.Text.Trim();
        if (flag)
          return;
        this.loan.Calculator.UpdateLenderRepresentative((LoanAssociateLog) this.msfl, "associateCell");
      }
    }

    private void createConversationLog(bool isEmail, bool isCell)
    {
      ConversationLog con = new ConversationLog(DateTime.Now, Session.UserInfo.Userid);
      if (isCell && this.txtCellPhone.Text.Trim() != string.Empty)
        con.Phone = this.txtCellPhone.Text.Trim();
      else if (this.txtPhone.Text.Trim() != string.Empty)
        con.Phone = this.txtPhone.Text.Trim();
      if (this.txtEmail.Text.Trim() != string.Empty)
        con.Email = AppSecurity.EncodeCommand(this.txtEmail.Text.Trim());
      con.Name = this.txtName.Text.Trim();
      con.IsEmail = isEmail;
      Session.Application.GetService<ILoanEditor>().StartConversation(con);
    }

    private void btnPhone_Click(object sender, EventArgs e)
    {
      this.createConversationLog(false, false);
    }

    private void btnEmail_Click(object sender, EventArgs e)
    {
      this.createConversationLog(true, false);
    }

    private void btnCell_Click(object sender, EventArgs e)
    {
      this.createConversationLog(false, true);
    }

    private void btnFax_Click(object sender, EventArgs e)
    {
      this.createConversationLog(false, false);
    }

    private void txtFax_KeyDown(object sender, KeyEventArgs e)
    {
      FieldFormat dataFormat = FieldFormat.PHONE;
      TextBox textBox = (TextBox) sender;
      bool needsUpdate = false;
      string str = Utils.FormatInput(textBox.Text, dataFormat, ref needsUpdate);
      if (!needsUpdate)
        return;
      textBox.Text = str;
      textBox.SelectionStart = str.Length;
    }

    private void txtEmail_KeyDown(object sender, KeyEventArgs e)
    {
      if (this.FieldOnKeyUp == null)
        return;
      this.FieldOnKeyUp(sender, (EventArgs) e);
    }

    private void txtCellPhone_KeyDown(object sender, KeyEventArgs e)
    {
      FieldFormat dataFormat = FieldFormat.PHONE;
      TextBox textBox = (TextBox) sender;
      bool needsUpdate = false;
      string str = Utils.FormatInput(textBox.Text, dataFormat, ref needsUpdate);
      if (!needsUpdate)
        return;
      textBox.Text = str;
      textBox.SelectionStart = str.Length;
    }

    private void txtPhone_KeyDown(object sender, KeyEventArgs e)
    {
      FieldFormat dataFormat = FieldFormat.PHONE;
      TextBox textBox = (TextBox) sender;
      bool needsUpdate = false;
      string str = Utils.FormatInput(textBox.Text, dataFormat, ref needsUpdate);
      if (needsUpdate)
      {
        textBox.Text = str;
        textBox.SelectionStart = str.Length;
      }
      if (this.FieldOnKeyUp == null)
        return;
      this.FieldOnKeyUp(sender, (EventArgs) e);
    }
  }
}
