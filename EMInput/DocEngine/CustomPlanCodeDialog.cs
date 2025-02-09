// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DocEngine.CustomPlanCodeDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.DocEngine
{
  public class CustomPlanCodeDialog : Form
  {
    private CustomPlanCode planCode;
    private DocumentOrderType orderType = DocumentOrderType.Closing;
    private Sessions.Session session;
    private IContainer components;
    private Panel pnlAlias;
    private Panel panel1;
    private Label label1;
    private TextBox txtEMPlanID;
    private Label label2;
    private TextBox txtDescription;
    private TextBox txtPlanCode;
    private Label label5;
    private Label label4;
    private Label label3;
    private CheckBox chkImport;
    private TextBox txtInvestor;
    private Button btnCancel;
    private Button btnSave;
    private Label label6;
    private Panel pnlImportInvestor;
    private Panel pnlButtons;
    private Label label7;
    private CheckBox chkActive;
    private Panel pnlPlanID;
    private Panel pnlStatus;
    private Label label8;

    public CustomPlanCodeDialog(Plan plan, DocumentOrderType orderType)
    {
      this.session = Session.DefaultInstance;
    }

    public CustomPlanCodeDialog(Plan plan, DocumentOrderType orderType, Sessions.Session session)
    {
      this.InitializeComponent();
      this.orderType = orderType;
      this.session = session;
      if (plan != null)
      {
        this.txtEMPlanID.Text = plan.PlanID;
        this.txtInvestor.Text = plan.InvestorName;
      }
      else
      {
        this.pnlAlias.Visible = this.pnlImportInvestor.Visible = this.pnlPlanID.Visible = false;
        this.Text = "Create Custom Plan Code";
      }
    }

    public CustomPlanCodeDialog(CustomPlanCode customPlanCode, Sessions.Session session)
    {
      this.planCode = customPlanCode;
      this.session = session;
      this.orderType = this.planCode.OrderType;
      this.InitializeComponent();
      this.loadControls();
      this.txtPlanCode.ReadOnly = true;
      this.btnSave.Text = "Save";
      if (this.planCode.IsEMAlias)
        this.Text = "Edit EM Plan Code as Alias";
      else
        this.Text = "Edit Custom Plan Code";
      this.pnlAlias.Visible = this.pnlImportInvestor.Visible = this.pnlPlanID.Visible = this.planCode.IsEMAlias;
    }

    private void adjustDialogSize()
    {
      this.ClientSize = new Size(this.ClientSize.Width, this.pnlButtons.Bottom);
    }

    private void loadControls()
    {
      if (this.planCode.IsEMAlias)
        this.txtEMPlanID.Text = this.planCode.PlanCodeID;
      this.txtPlanCode.Text = this.planCode.PlanCode.Substring(2);
      this.txtDescription.Text = this.planCode.Description;
      this.txtInvestor.Text = this.planCode.Investor;
      this.chkImport.Checked = !this.planCode.ImportInvestorToLoan;
      this.chkActive.Checked = this.planCode.IsActive;
    }

    public CustomPlanCode CustomPlanCode => this.planCode;

    private void btnSave_Click(object sender, EventArgs e)
    {
      bool flag1 = false;
      DocumentTrackingSetup setup = (DocumentTrackingSetup) null;
      if (this.txtDescription.Text.Trim() == string.Empty)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please provide a description.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (this.planCode == null)
        {
          foreach (CustomPlanCode customPlanCode in this.session.ConfigurationManager.GetCompanyCustomPlanCodes(this.orderType).ToArray())
          {
            if (customPlanCode.PlanCode == "C." + this.txtPlanCode.Text)
            {
              int num2 = (int) Utils.Dialog((IWin32Window) this, "This plan code is already in use.  Please select another one.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              return;
            }
          }
          this.planCode = new CustomPlanCode();
        }
        else if (this.planCode.IsActive && !this.chkActive.Checked)
        {
          setup = this.session.ConfigurationManager.GetDocumentTrackingSetup();
          bool flag2 = false;
          foreach (DocumentTemplate documentTemplate in setup)
          {
            if (!flag2)
            {
              if (documentTemplate.ClosingCriteria != null && documentTemplate.ClosingCriteria.PlanCodeValues != null)
              {
                foreach (string planCodeValue in documentTemplate.ClosingCriteria.PlanCodeValues)
                {
                  if (planCodeValue == this.planCode.PlanCode)
                  {
                    flag2 = true;
                    break;
                  }
                }
              }
              if (!flag2)
              {
                if (documentTemplate.OpeningCriteria != null && documentTemplate.OpeningCriteria.PlanCodeValues != null)
                {
                  foreach (string planCodeValue in documentTemplate.OpeningCriteria.PlanCodeValues)
                  {
                    if (planCodeValue == this.planCode.PlanCode)
                    {
                      flag2 = true;
                      break;
                    }
                  }
                }
              }
              else
                break;
            }
            else
              break;
          }
          if (flag2 && Utils.Dialog((IWin32Window) this, "Do you want to remove this custom plan code from the document trigger criteria?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            flag1 = true;
        }
        if (this.pnlAlias.Visible)
        {
          this.planCode.PlanCodeID = this.txtEMPlanID.Text;
          this.planCode.IsEMAlias = true;
          this.planCode.OrderType = this.orderType;
        }
        else
        {
          this.planCode.PlanCodeID = "";
          this.planCode.IsEMAlias = false;
          this.planCode.OrderType = this.orderType;
        }
        this.planCode.PlanCode = "C." + this.txtPlanCode.Text;
        this.planCode.Description = this.txtDescription.Text;
        this.planCode.Investor = this.txtInvestor.Text;
        this.planCode.ImportInvestorToLoan = !this.chkImport.Checked;
        this.planCode.IsActive = this.chkActive.Checked;
        this.session.ConfigurationManager.AddCompanyCustomPlanCode(this.planCode);
        if (flag1)
        {
          foreach (DocumentTemplate documentTemplate in setup)
          {
            if (this.planCode.OrderType == DocumentOrderType.Closing && documentTemplate.ClosingCriteria != null && documentTemplate.ClosingCriteria.PlanCodeValues != null && documentTemplate.ClosingCriteria.PlanCodeValues.Length != 0)
            {
              List<string> stringList = new List<string>((IEnumerable<string>) documentTemplate.ClosingCriteria.PlanCodeValues);
              if (stringList.Contains(this.planCode.PlanCode))
                stringList.Remove(this.planCode.PlanCode);
              documentTemplate.ClosingCriteria.PlanCodeValues = stringList.ToArray();
            }
            if (this.planCode.OrderType == DocumentOrderType.Opening && documentTemplate.OpeningCriteria != null && documentTemplate.OpeningCriteria.PlanCodeValues != null && documentTemplate.OpeningCriteria.PlanCodeValues.Length != 0)
            {
              List<string> stringList = new List<string>((IEnumerable<string>) documentTemplate.OpeningCriteria.PlanCodeValues);
              if (stringList.Contains(this.planCode.PlanCode))
                stringList.Remove(this.planCode.PlanCode);
              documentTemplate.OpeningCriteria.PlanCodeValues = stringList.ToArray();
            }
          }
          this.session.ConfigurationManager.SaveDocumentTrackingSetup(setup);
        }
        this.DialogResult = DialogResult.OK;
      }
    }

    private void CustomPlanCodeDialog_Shown(object sender, EventArgs e) => this.adjustDialogSize();

    private void txtPlanCode_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (char.IsControl(e.KeyChar) || char.IsLetterOrDigit(e.KeyChar) || e.KeyChar == '.' || e.KeyChar == '_')
        return;
      e.Handled = true;
    }

    private void txtPlanCode_TextChanged(object sender, EventArgs e)
    {
      int num = this.txtPlanCode.SelectionStart;
      string str = "";
      for (int index = 0; index < this.txtPlanCode.Text.Length; ++index)
      {
        if (char.IsLetterOrDigit(this.txtPlanCode.Text[index]) || this.txtPlanCode.Text[index] == '.' || this.txtPlanCode.Text[index] == '_')
          str += this.txtPlanCode.Text[index].ToString();
      }
      if (!(str != this.txtPlanCode.Text))
        return;
      this.txtPlanCode.Text = str;
      try
      {
        num = Math.Max(0, this.txtPlanCode.SelectionStart - 1);
      }
      catch
      {
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
      this.pnlAlias = new Panel();
      this.label1 = new Label();
      this.txtEMPlanID = new TextBox();
      this.label2 = new Label();
      this.panel1 = new Panel();
      this.txtPlanCode = new TextBox();
      this.label6 = new Label();
      this.txtInvestor = new TextBox();
      this.txtDescription = new TextBox();
      this.label5 = new Label();
      this.label4 = new Label();
      this.label3 = new Label();
      this.label7 = new Label();
      this.chkActive = new CheckBox();
      this.btnCancel = new Button();
      this.btnSave = new Button();
      this.chkImport = new CheckBox();
      this.pnlImportInvestor = new Panel();
      this.pnlButtons = new Panel();
      this.label8 = new Label();
      this.pnlPlanID = new Panel();
      this.pnlStatus = new Panel();
      this.pnlAlias.SuspendLayout();
      this.panel1.SuspendLayout();
      this.pnlImportInvestor.SuspendLayout();
      this.pnlButtons.SuspendLayout();
      this.pnlPlanID.SuspendLayout();
      this.pnlStatus.SuspendLayout();
      this.SuspendLayout();
      this.pnlAlias.Controls.Add((Control) this.label1);
      this.pnlAlias.Dock = DockStyle.Top;
      this.pnlAlias.Location = new Point(0, 0);
      this.pnlAlias.Name = "pnlAlias";
      this.pnlAlias.Size = new Size(432, 39);
      this.pnlAlias.TabIndex = 0;
      this.label1.Location = new Point(9, 9);
      this.label1.Name = "label1";
      this.label1.Size = new Size(401, 30);
      this.label1.TabIndex = 0;
      this.label1.Text = "A Plan Code Alias will inherit all of the attributes of an ICE Mortgage Technology Plan Code but allow you to override or hide the data values shown below.";
      this.txtEMPlanID.Location = new Point(176, 0);
      this.txtEMPlanID.Name = "txtEMPlanID";
      this.txtEMPlanID.ReadOnly = true;
      this.txtEMPlanID.Size = new Size(245, 20);
      this.txtEMPlanID.TabIndex = 2;
      this.txtEMPlanID.TabStop = false;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(9, 4);
      this.label2.Name = "label2";
      this.label2.Size = new Size(163, 14);
      this.label2.TabIndex = 1;
      this.label2.Text = "ICE Mortgage Technology Plan ID";
      this.panel1.Controls.Add((Control) this.txtPlanCode);
      this.panel1.Controls.Add((Control) this.label6);
      this.panel1.Controls.Add((Control) this.txtInvestor);
      this.panel1.Controls.Add((Control) this.txtDescription);
      this.panel1.Controls.Add((Control) this.label5);
      this.panel1.Controls.Add((Control) this.label4);
      this.panel1.Controls.Add((Control) this.label3);
      this.panel1.Dock = DockStyle.Top;
      this.panel1.Location = new Point(0, 96);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(432, 65);
      this.panel1.TabIndex = 1;
      this.txtPlanCode.Location = new Point(191, 2);
      this.txtPlanCode.MaxLength = 45;
      this.txtPlanCode.Name = "txtPlanCode";
      this.txtPlanCode.Size = new Size(230, 20);
      this.txtPlanCode.TabIndex = 3;
      this.txtPlanCode.TextChanged += new EventHandler(this.txtPlanCode_TextChanged);
      this.txtPlanCode.KeyPress += new KeyPressEventHandler(this.txtPlanCode_KeyPress);
      this.label6.AutoSize = true;
      this.label6.Location = new Point(176, 5);
      this.label6.Name = "label6";
      this.label6.Size = new Size(17, 14);
      this.label6.TabIndex = 9;
      this.label6.Text = "C.";
      this.txtInvestor.Location = new Point(176, 44);
      this.txtInvestor.Name = "txtInvestor";
      this.txtInvestor.Size = new Size(245, 20);
      this.txtInvestor.TabIndex = 5;
      this.txtDescription.Location = new Point(176, 23);
      this.txtDescription.Name = "txtDescription";
      this.txtDescription.Size = new Size(245, 20);
      this.txtDescription.TabIndex = 4;
      this.label5.AutoSize = true;
      this.label5.Location = new Point(9, 48);
      this.label5.Name = "label5";
      this.label5.Size = new Size(46, 14);
      this.label5.TabIndex = 2;
      this.label5.Text = "Investor";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(9, 26);
      this.label4.Name = "label4";
      this.label4.Size = new Size(68, 14);
      this.label4.TabIndex = 1;
      this.label4.Text = "* Description";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(9, 5);
      this.label3.Name = "label3";
      this.label3.Size = new Size(62, 14);
      this.label3.TabIndex = 0;
      this.label3.Text = "* Plan Code";
      this.label7.AutoSize = true;
      this.label7.Location = new Point(9, 10);
      this.label7.Name = "label7";
      this.label7.Size = new Size(38, 14);
      this.label7.TabIndex = 11;
      this.label7.Text = "Status";
      this.chkActive.AutoSize = true;
      this.chkActive.Checked = true;
      this.chkActive.CheckState = CheckState.Checked;
      this.chkActive.Location = new Point(176, 9);
      this.chkActive.Name = "chkActive";
      this.chkActive.Size = new Size(57, 18);
      this.chkActive.TabIndex = 10;
      this.chkActive.Text = "Active";
      this.chkActive.UseVisualStyleBackColor = true;
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(347, 15);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 8;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnSave.Location = new Point(264, 15);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(75, 22);
      this.btnSave.TabIndex = 7;
      this.btnSave.Text = "Add";
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.chkImport.AutoSize = true;
      this.chkImport.Location = new Point(176, 2);
      this.chkImport.Name = "chkImport";
      this.chkImport.Size = new Size(195, 18);
      this.chkImport.TabIndex = 6;
      this.chkImport.Text = "Do not import investor name to loan";
      this.chkImport.UseVisualStyleBackColor = true;
      this.pnlImportInvestor.Controls.Add((Control) this.chkImport);
      this.pnlImportInvestor.Dock = DockStyle.Top;
      this.pnlImportInvestor.Location = new Point(0, 161);
      this.pnlImportInvestor.Name = "pnlImportInvestor";
      this.pnlImportInvestor.Size = new Size(432, 22);
      this.pnlImportInvestor.TabIndex = 9;
      this.pnlButtons.Controls.Add((Control) this.label8);
      this.pnlButtons.Controls.Add((Control) this.btnCancel);
      this.pnlButtons.Controls.Add((Control) this.btnSave);
      this.pnlButtons.Dock = DockStyle.Top;
      this.pnlButtons.Location = new Point(0, 183);
      this.pnlButtons.Name = "pnlButtons";
      this.pnlButtons.Size = new Size(432, 43);
      this.pnlButtons.TabIndex = 10;
      this.label8.AutoSize = true;
      this.label8.Location = new Point(9, 4);
      this.label8.Name = "label8";
      this.label8.Size = new Size(57, 14);
      this.label8.TabIndex = 9;
      this.label8.Text = "* Required";
      this.pnlPlanID.Controls.Add((Control) this.txtEMPlanID);
      this.pnlPlanID.Controls.Add((Control) this.label2);
      this.pnlPlanID.Dock = DockStyle.Top;
      this.pnlPlanID.Location = new Point(0, 67);
      this.pnlPlanID.Name = "pnlPlanID";
      this.pnlPlanID.Size = new Size(432, 21);
      this.pnlPlanID.TabIndex = 11;
      this.pnlStatus.Controls.Add((Control) this.label7);
      this.pnlStatus.Controls.Add((Control) this.chkActive);
      this.pnlStatus.Dock = DockStyle.Top;
      this.pnlStatus.Location = new Point(0, 39);
      this.pnlStatus.Name = "pnlStatus";
      this.pnlStatus.Size = new Size(432, 28);
      this.pnlStatus.TabIndex = 12;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(432, 232);
      this.Controls.Add((Control) this.pnlButtons);
      this.Controls.Add((Control) this.pnlImportInvestor);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.pnlPlanID);
      this.Controls.Add((Control) this.pnlStatus);
      this.Controls.Add((Control) this.pnlAlias);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (CustomPlanCodeDialog);
      this.ShowIcon = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Add ICE Mortgage Technology Plan Code as Alias";
      this.Shown += new EventHandler(this.CustomPlanCodeDialog_Shown);
      this.pnlAlias.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.pnlImportInvestor.ResumeLayout(false);
      this.pnlImportInvestor.PerformLayout();
      this.pnlButtons.ResumeLayout(false);
      this.pnlButtons.PerformLayout();
      this.pnlPlanID.ResumeLayout(false);
      this.pnlPlanID.PerformLayout();
      this.pnlStatus.ResumeLayout(false);
      this.pnlStatus.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
