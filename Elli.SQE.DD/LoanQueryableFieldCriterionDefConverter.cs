// Decompiled with JetBrains decompiler
// Type: Elli.SQE.DD.LoanQueryableFieldCriterionDefConverter
// Assembly: Elli.SQE.DD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: AD1B11DD-560E-4083-B59A-D629AF47C790
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.SQE.DD.dll

using Elli.Common.ModelFields;

#nullable disable
namespace Elli.SQE.DD
{
  public class LoanQueryableFieldCriterionDefConverter(
    ICanonicalFragmentFormatter canonicalFragmentFormatter = null) : 
    QueryableFieldCriterionDefConverter(canonicalFragmentFormatter)
  {
    protected override bool ShouldAddIndexFragment(
      ModelFieldPathExpression expression,
      QueryableField obj)
    {
      if (expression.Path == "Applications[0]")
        return true;
      if (!expression.IsIndexed)
      {
        int num = expression.HasQualifier ? 1 : 0;
      }
      return false;
    }
  }
}
