// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.Specs.LoanContentAccessSpec
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataEngine;
using PostSharp.Aspects;
using PostSharp.ImplementationDetails_c7fe12ac;
using System;

#nullable disable
namespace EllieMae.EMLite.LoanUtils.Specs
{
  public class LoanContentAccessSpec : EncSpecBase<LoanContentAccessSpec_Data>
  {
    public override bool IsSatisfiedBy(LoanContentAccessSpec_Data entityData)
    {
      // ISSUE: reference to a compiler-generated field
      \u003C\u003Ez__a_2.a3.OnEntry((MethodExecutionArgs) null);
      try
      {
        bool flag = entityData.LoanData.ContentAccess == entityData.RequestedContentAccess;
        if (!flag)
          this.ViolationMessage = this.ViolationMessage + "User has no content access '" + Enum.GetName(typeof (LoanContentAccess), (object) entityData.RequestedContentAccess) + "' to the loan file.";
        return flag;
      }
      finally
      {
        // ISSUE: reference to a compiler-generated field
        \u003C\u003Ez__a_2.a3.OnExit((MethodExecutionArgs) null);
      }
    }

    public LoanContentAccessSpec()
    {
      // ISSUE: reference to a compiler-generated field
      \u003C\u003Ez__a_2.a4.OnEntry((MethodExecutionArgs) null);
      try
      {
      }
      finally
      {
        // ISSUE: reference to a compiler-generated field
        \u003C\u003Ez__a_2.a4.OnExit((MethodExecutionArgs) null);
      }
    }
  }
}
