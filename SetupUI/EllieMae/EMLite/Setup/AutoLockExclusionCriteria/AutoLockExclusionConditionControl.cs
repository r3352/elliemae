// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.AutoLockExclusionCriteria.AutoLockExclusionConditionControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.BusinessRules;
using EllieMae.EMLite.ClientServer;
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
namespace EllieMae.EMLite.Setup.AutoLockExclusionCriteria
{
  public class AutoLockExclusionConditionControl : UserControl
  {
    public string Condition = "";
    public string ConditionXML = "";
    private Sessions.Session session;
    private IContainer components;
    private StandardIconButton btnSelect;
    private TextBox textConditionCode;
    private Label labelAssigned;

    public AutoLockExclusionConditionControl(Sessions.Session session)
    {
      this.session = session;
      this.InitializeComponent();
    }

    private void btnSelect_Click(object sender, EventArgs e)
    {
      using (AdvConditionEditor advConditionEditor = new AdvConditionEditor(this.session, this.ConditionXML, (ReportFieldDefs) this.PrepareFieldDefs()))
      {
        if (advConditionEditor.GetConditionScript() != this.textConditionCode.Text)
          advConditionEditor.ClearFilters();
        if (advConditionEditor.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.textConditionCode.Text = this.Condition = advConditionEditor.GetConditionScript();
        this.ConditionXML = advConditionEditor.GetConditionXml();
      }
    }

    private LoanReportFieldDefs PrepareFieldDefs()
    {
      LoanReportFieldDefs loanReportFieldDefs = new LoanReportFieldDefs(this.session);
      loanReportFieldDefs.Add((ReportFieldDef) new LoanReportFieldDef("Rate Lock", "2866", "Loan Program", "Loan Program", FieldFormat.STRING, ""));
      loanReportFieldDefs.Add((ReportFieldDef) new LoanReportFieldDef("Rate Lock", "3041", "Lock Plan Code", "Lock Plan Code", FieldFormat.STRING, ""));
      loanReportFieldDefs.Add((ReportFieldDef) new LoanReportFieldDef("Rate Lock", "2867", "Documentation Type", "Documentation Type", FieldFormat.STRING, ""));
      loanReportFieldDefs.Add((ReportFieldDef) new LoanReportFieldDef("Rate Lock", "2940", "Borrower Min. Req. FICO", "Borrower Min. Req. FICO", FieldFormat.STRING, ""));
      loanReportFieldDefs.Add((ReportFieldDef) new LoanReportFieldDef("Rate Lock", "2941", "Co-Borrower Min. Req. FICO", "Co-Borrower Min. Req. FICO", FieldFormat.STRING, ""));
      loanReportFieldDefs.Add((ReportFieldDef) new LoanReportFieldDef("Rate Lock", "2853", "Credit Score for Decision Making", "Credit Score for Decision Making", FieldFormat.STRING, ""));
      loanReportFieldDefs.Add((ReportFieldDef) new LoanReportFieldDef("Rate Lock", "3528", "First-Time Homebuyer", "First-Time Homebuyer", FieldFormat.YN, ""));
      loanReportFieldDefs.Add((ReportFieldDef) new LoanReportFieldDef("Rate Lock", "3530", "Borrower can demonstrate a 12-month mortgage/rental history", "Borrower can demonstrate a 12-month mortgage/rental history", FieldFormat.YN, ""));
      loanReportFieldDefs.Add((ReportFieldDef) new LoanReportFieldDef("Rate Lock", "2942", "Address", "Address", FieldFormat.STRING, ""));
      loanReportFieldDefs.Add((ReportFieldDef) new LoanReportFieldDef("Rate Lock", "2943", "City", "City", FieldFormat.STRING, ""));
      loanReportFieldDefs.Add((ReportFieldDef) new LoanReportFieldDef("Rate Lock", "2945", "State", "State", FieldFormat.STATE, ""));
      loanReportFieldDefs.Add((ReportFieldDef) new LoanReportFieldDef("Rate Lock", "2946", "Zip", "Zip", FieldFormat.ZIPCODE, ""));
      loanReportFieldDefs.Add((ReportFieldDef) new LoanReportFieldDef("Rate Lock", "2944", "County", "County", FieldFormat.STRING, ""));
      loanReportFieldDefs.Add((ReportFieldDef) new LoanReportFieldDef("Rate Lock", "2947", "Property Type", "Property Type", FieldFormat.STRING, ""));
      loanReportFieldDefs.Add((ReportFieldDef) new LoanReportFieldDef("Rate Lock", "2948", "Estimated Value", "Estimated Value", FieldFormat.INTEGER, ""));
      loanReportFieldDefs.Add((ReportFieldDef) new LoanReportFieldDef("Rate Lock", "2949", "Appraised Value", "Appraised Value", FieldFormat.INTEGER, ""));
      loanReportFieldDefs.Add((ReportFieldDef) new LoanReportFieldDef("Rate Lock", "3038", "Purchase Price", "Purchase Price", FieldFormat.DECIMAL_2, ""));
      loanReportFieldDefs.Add((ReportFieldDef) new LoanReportFieldDef("Rate Lock", "2950", "Property Will Be", "Property Will Be", FieldFormat.STRING, ""));
      loanReportFieldDefs.Add((ReportFieldDef) new LoanReportFieldDef("Rate Lock", "3529", "No Units", "No Units", FieldFormat.INTEGER, ""));
      loanReportFieldDefs.Add((ReportFieldDef) new LoanReportFieldDef("Rate Lock", "4115", "Subject Property is a Condotel", "Subject Property is a Condotel", FieldFormat.YN, ""));
      loanReportFieldDefs.Add((ReportFieldDef) new LoanReportFieldDef("Rate Lock", "4116", "Subject Property is a Non-Warrantable Project", "Subject Property is a Non-Warrantable Project", FieldFormat.YN, ""));
      loanReportFieldDefs.Add((ReportFieldDef) new LoanReportFieldDef("Rate Lock", "2951", "Purpose of Loan", "Purpose of Loan", FieldFormat.STRING, ""));
      loanReportFieldDefs.Add((ReportFieldDef) new LoanReportFieldDef("Rate Lock", "2958", "Lien Position", "Lien Position", FieldFormat.STRING, ""));
      loanReportFieldDefs.Add((ReportFieldDef) new LoanReportFieldDef("Rate Lock", "3844", "203(k)", "203(k)", FieldFormat.YN, ""));
      loanReportFieldDefs.Add((ReportFieldDef) new LoanReportFieldDef("Rate Lock", "2952", "Loan Type", "Loan Type", FieldFormat.STRING, ""));
      loanReportFieldDefs.Add((ReportFieldDef) new LoanReportFieldDef("Rate Lock", "2959", "Term", "Term", FieldFormat.INTEGER, ""));
      loanReportFieldDefs.Add((ReportFieldDef) new LoanReportFieldDef("Rate Lock", "2960", "Due (in Months)", "Due (in Months)", FieldFormat.INTEGER, ""));
      loanReportFieldDefs.Add((ReportFieldDef) new LoanReportFieldDef("Rate Lock", "2961", "Impounds Waived", "Impounds Waived", FieldFormat.STRING, ""));
      loanReportFieldDefs.Add((ReportFieldDef) new LoanReportFieldDef("Rate Lock", "2962", "Impound Types", "Impound Types", FieldFormat.STRING, ""));
      loanReportFieldDefs.Add((ReportFieldDef) new LoanReportFieldDef("Rate Lock", "2963", "Prepay Penalty", "Prepay Penalty", FieldFormat.STRING, ""));
      loanReportFieldDefs.Add((ReportFieldDef) new LoanReportFieldDef("Rate Lock", "2964", "Penalty Term", "Penalty Term", FieldFormat.STRING, ""));
      loanReportFieldDefs.Add((ReportFieldDef) new LoanReportFieldDef("Rate Lock", "3892", "No Closing Cost", "No Closing Cost", FieldFormat.YN, ""));
      loanReportFieldDefs.Add((ReportFieldDef) new LoanReportFieldDef("Rate Lock", "2953", "Amortization Type", "Amortization Type", FieldFormat.STRING, ""));
      loanReportFieldDefs.Add((ReportFieldDef) new LoanReportFieldDef("Rate Lock", "2954", "Amortization Type %", "Amortization Type %", FieldFormat.DECIMAL_3, ""));
      loanReportFieldDefs.Add((ReportFieldDef) new LoanReportFieldDef("Rate Lock", "2955", "Amortization Type Yrs", "Amortization Type Yrs", FieldFormat.INTEGER, ""));
      loanReportFieldDefs.Add((ReportFieldDef) new LoanReportFieldDef("Rate Lock", "2956", "Amortization Type ARM", "Amortization Type ARM", FieldFormat.STRING, ""));
      loanReportFieldDefs.Add((ReportFieldDef) new LoanReportFieldDef("Rate Lock", "2957", "Amortization Type Other", "Amortization Type Other", FieldFormat.STRING, ""));
      loanReportFieldDefs.Add((ReportFieldDef) new LoanReportFieldDef("Rate Lock", "3043", "Loan Amount", "Loan Amount", FieldFormat.DECIMAL_2, ""));
      loanReportFieldDefs.Add((ReportFieldDef) new LoanReportFieldDef("Rate Lock", "3044", "MIP/Funding%", "MIP/Funding%", FieldFormat.DECIMAL_6, ""));
      loanReportFieldDefs.Add((ReportFieldDef) new LoanReportFieldDef("Rate Lock", "3045", "MIP/Funding Amount", "MIP/Funding Amount", FieldFormat.DECIMAL_2, ""));
      loanReportFieldDefs.Add((ReportFieldDef) new LoanReportFieldDef("Rate Lock", "3046", "Amount Paid in Cash Lock", "Amount Paid in Cash Lock", FieldFormat.YN, ""));
      loanReportFieldDefs.Add((ReportFieldDef) new LoanReportFieldDef("Rate Lock", "3047", "Amount Paid in Cash Amount", "Amount Paid in Cash Amount", FieldFormat.DECIMAL_2, ""));
      loanReportFieldDefs.Add((ReportFieldDef) new LoanReportFieldDef("Rate Lock", "3049", "Seller Paid MI Premium", "Seller Paid MI Premium", FieldFormat.DECIMAL_2, ""));
      loanReportFieldDefs.Add((ReportFieldDef) new LoanReportFieldDef("Rate Lock", "2965", "Total Loan Amount", "Total Loan Amount", FieldFormat.DECIMAL_2, ""));
      loanReportFieldDefs.Add((ReportFieldDef) new LoanReportFieldDef("Rate Lock", "3056", "Round to nearest $50", "Round to nearest $50", FieldFormat.YN, ""));
      loanReportFieldDefs.Add((ReportFieldDef) new LoanReportFieldDef("Rate Lock", "2966", "Estimated Close Date", "Estimated Close Date", FieldFormat.DATE, ""));
      loanReportFieldDefs.Add((ReportFieldDef) new LoanReportFieldDef("Rate Lock", "3241", "LTV", "LTV", FieldFormat.DECIMAL_3, ""));
      loanReportFieldDefs.Add((ReportFieldDef) new LoanReportFieldDef("Rate Lock", "976", "CLTV", "CLTV", FieldFormat.DECIMAL_3, ""));
      loanReportFieldDefs.Add((ReportFieldDef) new LoanReportFieldDef("Rate Lock", "3846", "HELOC Actual Balance", "HELOC Actual Balance", FieldFormat.STRING, ""));
      loanReportFieldDefs.Add((ReportFieldDef) new LoanReportFieldDef("Rate Lock", "2398", "Subordinate Financing", "Subordinate Financing", FieldFormat.DECIMAL_2, ""));
      loanReportFieldDefs.Add((ReportFieldDef) new LoanReportFieldDef("Rate Lock", "142", "Cash from borrower", "Cash from borrower", FieldFormat.DECIMAL_2, ""));
      loanReportFieldDefs.Add((ReportFieldDef) new LoanReportFieldDef("Rate Lock", "353", "LTV", "LTV", FieldFormat.DECIMAL_3, ""));
      loanReportFieldDefs.Add((ReportFieldDef) new LoanReportFieldDef("Rate Lock", "976", "CLTV", "CLTV", FieldFormat.DECIMAL_3, ""));
      loanReportFieldDefs.Add((ReportFieldDef) new LoanReportFieldDef("Rate Lock", "740", "DTI", "DTI", FieldFormat.DECIMAL_3, ""));
      loanReportFieldDefs.Add((ReportFieldDef) new LoanReportFieldDef("Rate Lock", "742", "DTI", "DTI", FieldFormat.DECIMAL_3, ""));
      loanReportFieldDefs.Add((ReportFieldDef) new LoanReportFieldDef("Rate Lock", "3841", "Request Type", "Request Type", FieldFormat.STRING, ""));
      loanReportFieldDefs.Add((ReportFieldDef) new LoanReportFieldDef("Rate Lock", "2092", "Base Rate", "Base Rate", FieldFormat.DECIMAL_3, ""));
      loanReportFieldDefs.Add((ReportFieldDef) new LoanReportFieldDef("Rate Lock", "2099", "Total Rate Adjustment", "Total Rate Adjustment", FieldFormat.DECIMAL_3, ""));
      loanReportFieldDefs.Add((ReportFieldDef) new LoanReportFieldDef("Rate Lock", "2100", "Net Rate Expected", "Net Rate Expected", FieldFormat.DECIMAL_3, ""));
      loanReportFieldDefs.Add((ReportFieldDef) new LoanReportFieldDef("Rate Lock", "3847", "UnDiscounted Rate", "UnDiscounted Rate", FieldFormat.DECIMAL_3, ""));
      loanReportFieldDefs.Add((ReportFieldDef) new LoanReportFieldDef("Rate Lock", "3872", "Starting Adjust Rate", "Starting Adjust Rate", FieldFormat.DECIMAL_3, ""));
      loanReportFieldDefs.Add((ReportFieldDef) new LoanReportFieldDef("Rate Lock", "3874", "Starting Adjust Price", "Starting Adjust Price", FieldFormat.DECIMAL_3, ""));
      loanReportFieldDefs.Add((ReportFieldDef) new LoanReportFieldDef("Rate Lock", "2101", "Base Price", "Base Price", FieldFormat.DECIMAL_10, ""));
      loanReportFieldDefs.Add((ReportFieldDef) new LoanReportFieldDef("Rate Lock", "2142", "Total Price Adjustment", "Total Price Adjustment", FieldFormat.DECIMAL_10, ""));
      loanReportFieldDefs.Add((ReportFieldDef) new LoanReportFieldDef("Rate Lock", "2143", "Net Price Expected", "Net Price Expected", FieldFormat.DECIMAL_10, ""));
      loanReportFieldDefs.Add((ReportFieldDef) new LoanReportFieldDef("Rate Lock", "2647", "Base ARM Margin", "Base ARM Margin", FieldFormat.DECIMAL_3, ""));
      loanReportFieldDefs.Add((ReportFieldDef) new LoanReportFieldDef("Rate Lock", "2688", "Total ARM Margin Adjustment", "Total ARM Margin Adjustment", FieldFormat.DECIMAL_3, ""));
      loanReportFieldDefs.Add((ReportFieldDef) new LoanReportFieldDef("Rate Lock", "2689", "Net ARM Margin Expected", "Net ARM Margin Expected", FieldFormat.DECIMAL_3, ""));
      loanReportFieldDefs.Add((ReportFieldDef) new LoanReportFieldDef("Rate Lock", "2088", "Rate Sheet ID", "Rate Sheet ID", FieldFormat.STRING, ""));
      loanReportFieldDefs.Add((ReportFieldDef) new LoanReportFieldDef("Rate Lock", "2089", "Lock Date", "Lock Date", FieldFormat.DATE, ""));
      loanReportFieldDefs.Add((ReportFieldDef) new LoanReportFieldDef("Rate Lock", "2090", "Lock # Days", "Lock # Days", FieldFormat.INTEGER, ""));
      loanReportFieldDefs.Add((ReportFieldDef) new LoanReportFieldDef("Rate Lock", "2091", "Lock Expiration Date", "Lock Expiration Date", FieldFormat.DATE, ""));
      loanReportFieldDefs.Add((ReportFieldDef) new LoanReportFieldDef("Rate Lock", "3254", "Last Rate Set Date", "Last Rate Set Date", FieldFormat.DATE, ""));
      return loanReportFieldDefs;
    }

    public void SetCondition(BizRuleInfo rule)
    {
      this.textConditionCode.Text = this.Condition = rule.ConditionState;
      this.ConditionXML = rule.AdvancedCodeXML;
    }

    public void ApplyCondition(BizRuleInfo rule)
    {
      rule.ConditionState = this.textConditionCode.Text;
      rule.AdvancedCodeXML = this.ConditionXML;
    }

    public bool ValidateCondition() => this.validateAdvancedCode();

    private bool validateAdvancedCode()
    {
      if (this.textConditionCode.Text == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this.ParentForm, "You must provide code to determine the conditions under which this rule applies.");
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
      this.btnSelect = new StandardIconButton();
      this.textConditionCode = new TextBox();
      this.labelAssigned = new Label();
      ((ISupportInitialize) this.btnSelect).BeginInit();
      this.SuspendLayout();
      this.btnSelect.Anchor = AnchorStyles.Right;
      this.btnSelect.BackColor = Color.Transparent;
      this.btnSelect.Location = new Point(358, 16);
      this.btnSelect.MouseDownImage = (Image) null;
      this.btnSelect.Name = "btnSelect";
      this.btnSelect.Size = new Size(16, 16);
      this.btnSelect.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnSelect.TabIndex = 34;
      this.btnSelect.TabStop = false;
      this.btnSelect.Click += new EventHandler(this.btnSelect_Click);
      this.textConditionCode.Anchor = AnchorStyles.Left | AnchorStyles.Right;
      this.textConditionCode.Location = new Point(0, 16);
      this.textConditionCode.Multiline = true;
      this.textConditionCode.Name = "textConditionCode";
      this.textConditionCode.Size = new Size(352, 57);
      this.textConditionCode.TabIndex = 33;
      this.labelAssigned.AutoSize = true;
      this.labelAssigned.Location = new Point(-3, 0);
      this.labelAssigned.Name = "labelAssigned";
      this.labelAssigned.Size = new Size(108, 13);
      this.labelAssigned.TabIndex = 35;
      this.labelAssigned.Text = "Advanced Conditions";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.labelAssigned);
      this.Controls.Add((Control) this.btnSelect);
      this.Controls.Add((Control) this.textConditionCode);
      this.Name = nameof (AutoLockExclusionConditionControl);
      this.Size = new Size(377, 74);
      ((ISupportInitialize) this.btnSelect).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
