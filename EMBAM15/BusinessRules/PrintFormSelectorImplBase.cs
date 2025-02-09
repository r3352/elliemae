// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.BusinessRules.PrintFormSelectorImplBase
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Customization;
using System;

#nullable disable
namespace EllieMae.EMLite.BusinessRules
{
  public abstract class PrintFormSelectorImplBase : ContextBoundCodeImplBase, IFormSelectorImpl
  {
    private string currentFieldId;

    public bool Execute(IExecutionContext context, string fieldId, object newValue)
    {
      lock (this)
      {
        this.EstablishContext(context);
        this.currentFieldId = fieldId;
        try
        {
          return this.ExecutePrintFormSelector(fieldId, newValue);
        }
        catch (Exception ex)
        {
          throw new ExecutionException("Auto Print Form Selector execution error for field '" + fieldId + "'.", ex);
        }
      }
    }

    protected abstract bool ExecutePrintFormSelector(string fieldId, object newValue);

    protected IReadOnlyFieldSource Fields => (IReadOnlyFieldSource) this.Context.Fields;

    protected object XType(object value) => this.Fields.XType(value, this.currentFieldId);
  }
}
