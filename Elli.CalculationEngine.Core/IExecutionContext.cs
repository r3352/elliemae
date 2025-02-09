// Decompiled with JetBrains decompiler
// Type: Elli.CalculationEngine.Core.IExecutionContext
// Assembly: Elli.CalculationEngine.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E7988E98-462C-4B95-BC53-687EC5965B19
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.CalculationEngine.Core.dll

using Elli.CalculationEngine.Core.DataSource;
using System.Collections.Generic;

#nullable disable
namespace Elli.CalculationEngine.Core
{
  public interface IExecutionContext
  {
    DataEntityWrapper ContextRootEntity { get; set; }

    DataSourceWrapper RootDataSource { get; set; }

    bool LockAfterSetValue { get; set; }

    bool UnlockBeforeSetValue { get; set; }

    bool NoOperation { get; set; }

    IEnumerable<IDataField> ModifiedFields { get; set; }

    object GetFieldValue(string id);

    IDataField GetField(string id);

    T GetRelatedWrappedEntity<T>(string id) where T : IEntityWrapper;

    IEnumerable<DataEntityWrapper> GetRelatedEntities(
      string relationship,
      EntityDescriptor descriptor);

    IEnumerable<T> GetRelatedWrappedEntities<T>(string collectionName, string entityType);

    IEnumerable<T> GetRelatedWrappedEntities<T>(string collectionName, EntityDescriptor descriptor);

    object InvokeMethod(string methodName, params object[] parameters);

    void Lock();

    void Unlock();

    bool IsLocked(IDataField field);

    void SetNoOperation();

    bool IsModified(IDataField field);
  }
}
