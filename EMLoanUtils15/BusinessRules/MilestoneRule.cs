// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.BusinessRules.MilestoneRule
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Compiler;

#nullable disable
namespace EllieMae.EMLite.BusinessRules
{
  public class MilestoneRule : CodedBusinessRule
  {
    private string ruleName;
    private AdvancedCodeMilestonePair codePair;

    public MilestoneRule(
      string ruleName,
      RuleCondition condition,
      AdvancedCodeMilestonePair codePair)
      : base(ruleName, condition)
    {
      this.ruleName = ruleName;
      this.codePair = codePair;
    }

    public AdvancedCodeMilestonePair AdvancedCodeMilestonePair => this.codePair;

    protected override string GetRuleDefinition()
    {
      CodeWriter codeWriter = new CodeWriter(CodeLanguage.VB);
      codeWriter.WriteCommentLine("Milestone Rule: " + this.ruleName + ", Milestone = " + this.codePair.MilestoneID);
      codeWriter.WriteLine(this.codePair.SourceCode);
      return codeWriter.ToString();
    }
  }
}
