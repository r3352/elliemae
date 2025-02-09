// Decompiled with JetBrains decompiler
// Type: Elli.CalculationEngine.Core.ExecutionContext
// Assembly: Elli.CalculationEngine.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E7988E98-462C-4B95-BC53-687EC5965B19
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.CalculationEngine.Core.dll

using Elli.CalculationEngine.Core.DataSource;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Elli.CalculationEngine.Core
{
  public class ExecutionContext : IExecutionContext, ICloneable
  {
    public DataEntityWrapper ContextRootEntity { get; set; }

    public DataSourceWrapper RootDataSource { get; set; }

    public bool LockAfterSetValue { get; set; }

    public bool UnlockBeforeSetValue { get; set; }

    public bool NoOperation { get; set; }

    public IEnumerable<IDataField> ModifiedFields { get; set; }

    public ExecutionContext(
      IDataEntity contextRootEntity,
      DataSourceWrapper rootDataSource,
      IEnumerable<IDataField> modifiedFields)
      : this(contextRootEntity, true)
    {
      this.RootDataSource = rootDataSource;
      this.ModifiedFields = modifiedFields;
    }

    private void fieldChange(object sender, EventArgs e)
    {
    }

    public ExecutionContext(IDataEntity dataSource, bool readOnly)
    {
      this.ContextRootEntity = !(dataSource is DataSourceWrapper) ? DataEntityWrapper.Create(dataSource, this.RootDataSource) : (DataEntityWrapper) dataSource;
      this.LockAfterSetValue = false;
      this.UnlockBeforeSetValue = false;
      this.NoOperation = false;
    }

    public T GetRelatedWrappedEntity<T>(string memberName) where T : IEntityWrapper
    {
      return this.ContextRootEntity.GetRelatedWrappedEntity<T>(memberName);
    }

    public IEnumerable<DataEntityWrapper> GetRelatedEntities(
      string relationship,
      EntityDescriptor descriptor)
    {
      return this.ContextRootEntity.GetRelatedWrappedEntities(relationship, descriptor);
    }

    public IEnumerable<T> GetRelatedWrappedEntities<T>(
      string collectionName,
      EntityDescriptor descriptor)
    {
      return this.ContextRootEntity.GetRelatedWrappedEntities<T>(collectionName, descriptor);
    }

    public IEnumerable<T> GetRelatedWrappedEntities<T>(string collectionName, string entityType)
    {
      return this.ContextRootEntity.GetRelatedWrappedEntities<T>(collectionName, entityType);
    }

    public object GetFieldValue(string fieldId) => this.ContextRootEntity.GetFieldValue(fieldId);

    public IDataField GetField(string fieldId) => this.ContextRootEntity.GetField(fieldId);

    public object InvokeMethod(string methodName, params object[] parameters)
    {
      return this.ContextRootEntity.InvokeMethod(methodName, parameters);
    }

    public void Lock() => this.LockAfterSetValue = true;

    public void Unlock() => this.UnlockBeforeSetValue = true;

    public bool IsLocked(IDataField field) => field != null && field.IsLocked();

    public void SetNoOperation() => this.NoOperation = true;

    public bool IsModified(IDataField field)
    {
      bool flag = false;
      if (field != null && this.ModifiedFields != null && this.ModifiedFields.Any<IDataField>())
        flag = this.ModifiedFields.Any<IDataField>((Func<IDataField, bool>) (p => p.GetElementId() == field.GetElementId()));
      return flag;
    }

    public virtual object Clone()
    {
      return (object) new ExecutionContext((IDataEntity) this.ContextRootEntity, this.RootDataSource, this.ModifiedFields);
    }

    public virtual void Dispose()
    {
      if (this.ContextRootEntity == null)
        return;
      this.ContextRootEntity = (DataEntityWrapper) null;
    }
  }
}
