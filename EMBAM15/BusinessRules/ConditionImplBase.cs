// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.BusinessRules.ConditionImplBase
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Customization;
using System;

#nullable disable
namespace EllieMae.EMLite.BusinessRules
{
  public abstract class ConditionImplBase : ContextBoundCodeImplBase, IConditionImpl
  {
    public bool AppliesTo(IExecutionContext context)
    {
      this.EstablishContext(context);
      try
      {
        return this.Applies();
      }
      catch (Exception ex)
      {
        throw new ValidationException("Business Rule Execution Error: " + ex.Message);
      }
    }

    protected abstract bool Applies();

    protected IReadOnlyFieldSource Fields => (IReadOnlyFieldSource) this.Context.Fields;
  }
}
