// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.BusinessRules.ValidatorImplBase
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Customization;
using System;
using System.Collections.Concurrent;
using System.Threading;

#nullable disable
namespace EllieMae.EMLite.BusinessRules
{
  public abstract class ValidatorImplBase : ContextBoundCodeImplBase, IValidatorImpl
  {
    private ConcurrentDictionary<int, string> threadFieldIds = new ConcurrentDictionary<int, string>();

    public void Validate(IExecutionContext context, string fieldId, object value)
    {
      this.EstablishContext(context);
      this.FieldID = fieldId;
      try
      {
        this.ExecuteRule(value);
      }
      catch (ValidationException ex)
      {
        throw;
      }
      catch (Exception ex)
      {
        throw new ValidationExecutionException("Business Rule Execution Error: " + ex.Message, ex);
      }
      finally
      {
        this.FieldID = "";
        this.ReleaseContext();
      }
    }

    protected abstract void ExecuteRule(object value);

    protected IReadOnlyFieldSource Fields => (IReadOnlyFieldSource) this.Context.Fields;

    private string FieldID
    {
      get => this.threadFieldIds[Thread.CurrentThread.ManagedThreadId];
      set
      {
        string str = (string) null;
        if (string.IsNullOrEmpty(value))
          this.threadFieldIds.TryRemove(Thread.CurrentThread.ManagedThreadId, out str);
        else
          this.threadFieldIds[Thread.CurrentThread.ManagedThreadId] = value;
      }
    }

    protected void Fail(string description) => throw new ValidationException(description);

    protected object XType(object value) => this.Fields.XType(value, this.FieldID);
  }
}
