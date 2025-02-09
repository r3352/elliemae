// Decompiled with JetBrains decompiler
// Type: Elli.CalculationEngine.Core.CalculationImplBase
// Assembly: Elli.CalculationEngine.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E7988E98-462C-4B95-BC53-687EC5965B19
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.CalculationEngine.Core.dll

using Elli.CalculationEngine.Core.DataSource;
using System;

#nullable disable
namespace Elli.CalculationEngine.Core
{
  public abstract class CalculationImplBase : ContextBoundCodeImplBase, ICalculationImpl
  {
    public object Calculate<T>(T model, ref ICalculationContext context)
    {
      try
      {
        return this.ExecuteCalculation<T>(model, ref context);
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    protected abstract object ExecuteCalculation<T>(T model, ref ICalculationContext context);

    protected new object GetFieldValue(string id) => this.Context.GetFieldValue(id);

    protected new IDataField GetField(string id) => this.Context.GetField(id);

    protected new T GetRelatedWrappedEntity<T>(string id) where T : IEntityWrapper
    {
      try
      {
        return this.Context.GetRelatedWrappedEntity<T>(id);
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    protected new object InvokeMethod(string methodName, params object[] parameters)
    {
      return this.Context.InvokeMethod(methodName, parameters);
    }

    protected new void Lock() => this.Context.Lock();

    protected new void Unlock() => this.Context.Unlock();

    protected new bool IsLocked(IDataField field) => this.Context.IsLocked(field);

    protected new void SetNoOperation() => this.Context.SetNoOperation();

    protected new bool IsModified(IDataField field) => this.Context.IsModified(field);
  }
}
