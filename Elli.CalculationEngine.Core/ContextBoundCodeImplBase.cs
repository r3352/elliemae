// Decompiled with JetBrains decompiler
// Type: Elli.CalculationEngine.Core.ContextBoundCodeImplBase
// Assembly: Elli.CalculationEngine.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E7988E98-462C-4B95-BC53-687EC5965B19
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.CalculationEngine.Core.dll

using Elli.CalculationEngine.Core.CoreFunctions;
using Elli.CalculationEngine.Core.DataSource;
using System.Collections.Generic;

#nullable disable
namespace Elli.CalculationEngine.Core
{
  public class ContextBoundCodeImplBase : CoreFunctionsBase
  {
    private IExecutionContext context;

    protected virtual void EstablishContext(IExecutionContext context)
    {
      context.LockAfterSetValue = false;
      context.UnlockBeforeSetValue = false;
      context.NoOperation = false;
      this.context = context;
    }

    protected virtual void ReleaseContext() => this.context = (IExecutionContext) null;

    protected IExecutionContext Context => this.context;

    protected object GetFieldValue(string id) => this.Context.GetFieldValue(id);

    protected IDataField GetField(string id) => this.Context.GetField(id);

    protected T GetRelatedWrappedEntity<T>(string relationship) where T : IEntityWrapper
    {
      return this.Context.GetRelatedWrappedEntity<T>(relationship);
    }

    protected IEnumerable<T> GetRelatedWrappedEntities<T>(string collectionName, string entityType)
    {
      return this.Context.GetRelatedWrappedEntities<T>(collectionName, entityType);
    }

    protected object InvokeMethod(string methodName, params object[] parameters)
    {
      return this.Context.InvokeMethod(methodName, parameters);
    }

    protected void Lock() => this.Context.Lock();

    protected void Unlock() => this.Context.Unlock();

    protected bool IsLocked(IDataField field) => this.Context.IsLocked(field);

    protected void SetNoOperation() => this.Context.SetNoOperation();

    protected bool IsModified(IDataField field) => this.Context.IsModified(field);
  }
}
