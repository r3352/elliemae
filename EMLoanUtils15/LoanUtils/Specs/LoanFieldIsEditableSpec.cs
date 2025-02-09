// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.Specs.LoanFieldIsEditableSpec
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using PostSharp.Aspects;
using PostSharp.ImplementationDetails_c7fe12ac;

#nullable disable
namespace EllieMae.EMLite.LoanUtils.Specs
{
  public class LoanFieldIsEditableSpec : EncSpecBase<LoanFieldIsEditableSpec_Data>
  {
    public override bool IsSatisfiedBy(LoanFieldIsEditableSpec_Data entityData)
    {
      // ISSUE: reference to a compiler-generated field
      \u003C\u003Ez__a_3.a5.OnEntry((MethodExecutionArgs) null);
      try
      {
        bool flag = !entityData.LoanData.IsFieldReadOnly(entityData.FieldId);
        if (!flag)
          this.ViolationMessage = "Field ID '" + entityData.FieldId + "' is read only.";
        return flag;
      }
      finally
      {
        // ISSUE: reference to a compiler-generated field
        \u003C\u003Ez__a_3.a5.OnExit((MethodExecutionArgs) null);
      }
    }

    public LoanFieldIsEditableSpec()
    {
      // ISSUE: reference to a compiler-generated field
      \u003C\u003Ez__a_3.a6.OnEntry((MethodExecutionArgs) null);
      try
      {
      }
      finally
      {
        // ISSUE: reference to a compiler-generated field
        \u003C\u003Ez__a_3.a6.OnExit((MethodExecutionArgs) null);
      }
    }
  }
}
