// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.Validators.LoanIsEditableValidator
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.LoanUtils.Specs;
using PostSharp.Aspects;
using PostSharp.ImplementationDetails_c7fe12ac;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.LoanUtils.Validators
{
  public class LoanIsEditableValidator : IEncValidator<LoanIsEditableValidator_Data>
  {
    private readonly LoanContentAccessSpec _loanContentAccessSpec;
    private readonly LoanFieldIsEditableSpec _loanFieldIsEditableSpec;

    public LoanIsEditableValidator(
      LoanContentAccessSpec loanContentAccessSpec,
      LoanFieldIsEditableSpec loanFieldIsEditableSpec)
    {
      // ISSUE: reference to a compiler-generated field
      \u003C\u003Ez__a_1.a0.OnEntry((MethodExecutionArgs) null);
      try
      {
        this._loanContentAccessSpec = loanContentAccessSpec;
        this._loanFieldIsEditableSpec = loanFieldIsEditableSpec;
      }
      finally
      {
        // ISSUE: reference to a compiler-generated field
        \u003C\u003Ez__a_1.a0.OnExit((MethodExecutionArgs) null);
      }
    }

    public bool IsValid(LoanIsEditableValidator_Data entity, out IEnumerable<string> brokenSpecs)
    {
      // ISSUE: reference to a compiler-generated field
      \u003C\u003Ez__a_1.a1.OnEntry((MethodExecutionArgs) null);
      try
      {
        brokenSpecs = this.CheckBrokenSpecs(entity);
        return !brokenSpecs.Any<string>();
      }
      finally
      {
        // ISSUE: reference to a compiler-generated field
        \u003C\u003Ez__a_1.a1.OnExit((MethodExecutionArgs) null);
      }
    }

    private IEnumerable<string> CheckBrokenSpecs(LoanIsEditableValidator_Data entity)
    {
      // ISSUE: reference to a compiler-generated field
      \u003C\u003Ez__a_1.a2.OnEntry((MethodExecutionArgs) null);
      try
      {
        List<string> source = new List<string>();
        if (!source.Any<string>())
        {
          if (!this._loanFieldIsEditableSpec.IsSatisfiedBy(new LoanFieldIsEditableSpec_Data()
          {
            LoanData = entity.LoanData,
            FieldId = entity.FieldId
          }))
            source.Add(this._loanFieldIsEditableSpec.GetViolationMessage());
          if (this._loanContentAccessSpec.IsSatisfiedBy(new LoanContentAccessSpec_Data()
          {
            LoanData = entity.LoanData,
            RequestedContentAccess = LoanContentAccess.None
          }))
            source.Add(this._loanContentAccessSpec.GetViolationMessage());
        }
        return (IEnumerable<string>) source;
      }
      finally
      {
        // ISSUE: reference to a compiler-generated field
        \u003C\u003Ez__a_1.a2.OnExit((MethodExecutionArgs) null);
      }
    }
  }
}
