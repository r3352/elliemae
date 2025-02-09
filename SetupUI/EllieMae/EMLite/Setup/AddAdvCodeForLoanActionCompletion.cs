// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.AddAdvCodeForLoanActionCompletion
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.BusinessRules;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Compiler;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class AddAdvCodeForLoanActionCompletion : Form
  {
    private string originalCode;
    private IContainer components;
    private Label label1;
    private TextBox txtSourceCode;
    private DialogButtons dialogButtons1;

    public AddAdvCodeForLoanActionCompletion()
      : this("")
    {
    }

    public AddAdvCodeForLoanActionCompletion(string sourceCode)
    {
      this.InitializeComponent();
      this.txtSourceCode.Text = sourceCode ?? "";
      this.originalCode = sourceCode ?? "";
    }

    public string SourceCode => this.txtSourceCode.Text;

    private void dialogButtons1_OK(object sender, EventArgs e)
    {
      if (this.txtSourceCode.Text.Trim() == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Enter your advanced code in the space provided.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        if (!this.validateCode(this.txtSourceCode.Text))
          return;
        this.DialogResult = DialogResult.OK;
      }
    }

    private bool validateCode(string sourceCode)
    {
      try
      {
        using (RuntimeContext context = RuntimeContext.Create())
          new RuleBuilder().CreateValidatorImpl((CodedBusinessRule) new LoanActionCompletionRule("Validator", (RuleCondition) PredefinedCondition.Empty, new AdvancedCodeLoanActionPair("1", sourceCode)), context);
        return true;
      }
      catch (CompileException ex)
      {
        using (CompileExceptionDialog compileExceptionDialog = new CompileExceptionDialog(ex))
        {
          int num = (int) compileExceptionDialog.ShowDialog((IWin32Window) this);
        }
        return false;
      }
      catch (Exception ex)
      {
        ErrorDialog.Display(ex);
        return false;
      }
    }

    private void AddAdvCodeForLoanActionCompletion_FormClosing(
      object sender,
      FormClosingEventArgs e)
    {
      if (!(this.originalCode != this.SourceCode) || this.DialogResult == DialogResult.OK || Utils.Dialog((IWin32Window) this, "Are you sure you want to discard your changes?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
        return;
      e.Cancel = true;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.label1 = new Label();
      this.txtSourceCode = new TextBox();
      this.dialogButtons1 = new DialogButtons();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Location = new Point(8, 10);
      this.label1.Name = "label1";
      this.label1.Size = new Size(302, 14);
      this.label1.TabIndex = 0;
      this.label1.Text = "Enter your advanced validation code into the space provided:";
      this.txtSourceCode.AcceptsReturn = true;
      this.txtSourceCode.AcceptsTab = true;
      this.txtSourceCode.Location = new Point(10, 26);
      this.txtSourceCode.Multiline = true;
      this.txtSourceCode.Name = "txtSourceCode";
      this.txtSourceCode.Size = new Size(424, 172);
      this.txtSourceCode.TabIndex = 1;
      this.dialogButtons1.Dock = DockStyle.Bottom;
      this.dialogButtons1.Location = new Point(0, 199);
      this.dialogButtons1.Name = "dialogButtons1";
      this.dialogButtons1.Size = new Size(444, 47);
      this.dialogButtons1.TabIndex = 2;
      this.dialogButtons1.OK += new EventHandler(this.dialogButtons1_OK);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(444, 246);
      this.Controls.Add((Control) this.dialogButtons1);
      this.Controls.Add((Control) this.txtSourceCode);
      this.Controls.Add((Control) this.label1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.MinimizeBox = false;
      this.Name = nameof (AddAdvCodeForLoanActionCompletion);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Advanced Coding";
      this.FormClosing += new FormClosingEventHandler(this.AddAdvCodeForLoanActionCompletion_FormClosing);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
