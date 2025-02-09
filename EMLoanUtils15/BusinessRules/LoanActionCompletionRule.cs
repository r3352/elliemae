// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.BusinessRules.LoanActionCompletionRule
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Compiler;

#nullable disable
namespace EllieMae.EMLite.BusinessRules
{
  public class LoanActionCompletionRule : CodedBusinessRule
  {
    private string ruleName;
    private AdvancedCodeLoanActionPair codePair;

    public LoanActionCompletionRule(
      string ruleName,
      RuleCondition condition,
      AdvancedCodeLoanActionPair codePair)
      : base(ruleName, condition)
    {
      this.ruleName = ruleName;
      this.codePair = codePair;
    }

    public AdvancedCodeLoanActionPair AdvancedCodeLoanActionPair => this.codePair;

    protected override string GetRuleDefinition()
    {
      CodeWriter codeWriter = new CodeWriter(CodeLanguage.VB);
      codeWriter.WriteCommentLine("Loan Action Completion Rule: " + this.ruleName + ", Loan Action = " + this.codePair.LoanActionID);
      codeWriter.WriteLine(this.codePair.SourceCode);
      return codeWriter.ToString();
    }
  }
}
