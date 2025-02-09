// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.ComplianceTestFields
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public static class ComplianceTestFields
  {
    public static readonly FieldDefinitionCollection All = new FieldDefinitionCollection();

    static ComplianceTestFields()
    {
      ComplianceTestFields.All.Add((FieldDefinition) new ComplianceReviewField("COMPLIANCEREVIEW.ALL", "Compliance Review All"));
      ComplianceTestFields.All.Add((FieldDefinition) new ComplianceReviewField("COMPLIANCEREVIEW.ALERTS", "Compliance Review Alerts"));
      ComplianceTestFields.All.Add((FieldDefinition) new ComplianceReviewField("COMPLIANCEREVIEW.PASSED", "Compliance Review Passed"));
      ComplianceTestFields.All.Add((FieldDefinition) new ComplianceReviewField("COMPLIANCEREVIEW.WARNINGS", "Compliance Review Warnings"));
      ComplianceTestFields.All.Add((FieldDefinition) new ComplianceReviewField("COMPLIANCEREVIEW.FAILURES", "Compliance Review Failures"));
      ComplianceTestFields.All.Add((FieldDefinition) new ComplianceReviewField("COMPLIANCEREVIEW.ERRORS", "Compliance Review Errors"));
    }
  }
}
