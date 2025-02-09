// Decompiled with JetBrains decompiler
// Type: Elli.CalculationEngine.Core.DataSource.DataEntityWrapper
// Assembly: Elli.CalculationEngine.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E7988E98-462C-4B95-BC53-687EC5965B19
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.CalculationEngine.Core.dll

using Elli.CalculationEngine.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

#nullable disable
namespace Elli.CalculationEngine.Core.DataSource
{
  public class DataEntityWrapper : IDataEntity, IDataElement, IEntityWrapper
  {
    public static readonly object NotSet = new object();
    protected static readonly DataEntityWrapper NullDataEntity = new DataEntityWrapper((IDataEntity) null, (DataSourceWrapper) null);
    protected IDataEntity entity;
    protected string name;
    protected EntityDescriptor type;
    protected DataSourceWrapper dataSource;
    protected Dictionary<string, IEnumerable<DataEntityWrapper>> _collectionCache;

    public IDataEntity Entity
    {
      get => this.entity;
      private set
      {
      }
    }

    protected DataEntityWrapper()
    {
      this._collectionCache = new Dictionary<string, IEnumerable<DataEntityWrapper>>();
    }

    protected DataEntityWrapper(IDataEntity entity, DataSourceWrapper rootDataSource)
      : this()
    {
      if (entity == null)
        return;
      this.entity = entity;
      this.name = entity.GetEntityName();
      this.type = entity.GetEntityType();
      this.dataSource = rootDataSource;
    }

    public virtual void ClearProperties()
    {
    }

    private static DataEntityWrapper GetDataEntityWrapper(
      Type type,
      IDataEntity entity,
      DataSourceWrapper rootDataSource)
    {
      return DataEntityWrapper.GetDataEntityWrapper(type, (string) null, entity, rootDataSource);
    }

    private static DataEntityWrapper GetDataEntityWrapper(
      Type type,
      string typeName,
      IDataEntity entity,
      DataSourceWrapper rootDataSource)
    {
      DataEntityWrapper dataEntityWrapper = DataEntityWrapper.NullDataEntity;
      if (entity != null)
      {
        if (entity is DataEntityWrapper)
        {
          dataEntityWrapper = (DataEntityWrapper) entity;
        }
        else
        {
          string key = entity.GetEntityName();
          if (string.IsNullOrEmpty(key))
            key = !string.IsNullOrEmpty(typeName) ? typeName : entity.GetEntityType().EntityType;
          if (!rootDataSource.EntityCache.TryGetValue(key, out dataEntityWrapper))
          {
            dataEntityWrapper = (DataEntityWrapper) Activator.CreateInstance(type, (object) entity, (object) rootDataSource);
            rootDataSource.EntityCache.Add(key, dataEntityWrapper);
          }
        }
      }
      return dataEntityWrapper;
    }

    public static T Create<T>(IDataEntity entity, DataSourceWrapper rootDataSource)
    {
      DataEntityWrapper dataEntityWrapper = DataEntityWrapper.GetDataEntityWrapper(typeof (T), entity, rootDataSource);
      return dataEntityWrapper.IsNull() ? (T) Activator.CreateInstance(typeof (T), new object[2]) : (T) dataEntityWrapper;
    }

    public static DataEntityWrapper Create(IDataEntity entity, DataSourceWrapper rootDataSource)
    {
      string entityType = entity.GetEntityType().EntityType;
      Type type;
      return !rootDataSource.TypeLookup.TryGetValue(entityType, out type) ? DataEntityWrapper.GetDataEntityWrapper(typeof (GenericEntityWrapper), entityType, entity, rootDataSource) : DataEntityWrapper.GetDataEntityWrapper(type, entityType, entity, rootDataSource);
    }

    public Guid GetElementId() => this.entity.GetElementId();

    public DataElementType GetElementType() => this.entity.GetElementType();

    public EntityDescriptor GetEntityType() => this.type;

    public string GetEntityName() => this.name;

    public IDataSource GetDataSource() => (IDataSource) this.dataSource;

    public IEnumerable<string> GetFieldList()
    {
      return this.entity != null ? this.entity.GetFieldList() : (IEnumerable<string>) new List<string>();
    }

    public IDataField GetField(string fieldId) => (IDataField) this.GetWrappedField(fieldId);

    public DataFieldWrapper GetWrappedField(string fieldId, StringBuilder sbTrace = null)
    {
      if (this.entity != null)
      {
        if (EngineUtility.IsTransientField(fieldId))
          return this.GetTransientField(fieldId);
        try
        {
          IDataField field = this.entity.GetField(fieldId);
          if (field != null)
            return DataFieldWrapper.Create(field, this.dataSource);
        }
        catch (Exception ex)
        {
          sbTrace?.AppendLine(string.Format("Failed to get field {{{0}}}.[{1}] from DataSource.", (object) this.GetQualifiedName(), (object) fieldId));
          sbTrace?.AppendLine(ex.Message);
          sbTrace?.AppendLine(ex.StackTrace);
          throw new Exception(string.Format("Failed to get field {{{0}}}.[{1}] from DataSource.", (object) this.GetQualifiedName(), (object) fieldId), ex);
        }
      }
      return DataFieldWrapper.NullDataField;
    }

    private DataFieldWrapper GetTransientField(string fieldId)
    {
      return DataFieldWrapper.Create((IDataField) ((DataSourceWrapper) this.GetDataSource()).GetTransientField(this, this.name, fieldId), this.dataSource);
    }

    public virtual object GetFieldValue(string fieldId)
    {
      object fieldValue = (object) null;
      if (this.entity != null)
      {
        if (EngineUtility.IsTransientField(fieldId))
          fieldValue = (this.GetField(fieldId) ?? throw new Exception(string.Format("Error: Transient Field [{0}] not defined for entity {{{1}}}", (object) fieldId, (object) this.GetQualifiedName()))).GetValue();
        else
          fieldValue = this.entity.GetFieldValue(fieldId);
      }
      return fieldValue;
    }

    public virtual void SetFieldValue(string fieldId, object value)
    {
      if (this.entity == null)
        return;
      if (EngineUtility.IsTransientField(fieldId))
        (this.GetField(fieldId) ?? throw new Exception(string.Format("Error: Transient Field [{0}] not defined for entity {{{1}}}", (object) fieldId, (object) this.GetQualifiedName()))).SetValue(value);
      else
        this.entity.SetFieldValue(fieldId, value);
    }

    public IDataEntity GetEntityByUniqueName(string entityName)
    {
      return this.entity != null ? (IDataEntity) DataEntityWrapper.Create(this.entity.GetEntityByUniqueName(entityName), this.dataSource) : (IDataEntity) DataEntityWrapper.NullDataEntity;
    }

    public IDataEntity GetRelatedEntity(string relationship)
    {
      return (IDataEntity) this.GetRelatedWrappedEntity(relationship);
    }

    public T GetRelatedWrappedEntity<T>(string relationship) where T : IEntityWrapper
    {
      if (relationship == this.dataSource.RootRelationship)
        return (T) this.GetDataSource();
      IDataEntity relatedEntity = this.entity.GetRelatedEntity(relationship);
      return this.entity != null ? DataEntityWrapper.Create<T>(relatedEntity, this.dataSource) : default (T);
    }

    public DataEntityWrapper GetRelatedWrappedEntity(string relationship)
    {
      if (string.CompareOrdinal(relationship, this.dataSource.RootRelationship) == 0)
        return (DataEntityWrapper) this.dataSource;
      IDataEntity relatedEntity = this.entity.GetRelatedEntity(relationship);
      IDataEntity relatedWrappedEntity;
      if (relatedEntity != null)
      {
        string entityType = relatedEntity.GetEntityType().EntityType;
        Type type;
        if (!this.dataSource.TypeLookup.TryGetValue(entityType, out type))
          type = typeof (DataEntityWrapper);
        relatedWrappedEntity = (IDataEntity) DataEntityWrapper.GetDataEntityWrapper(type, entityType, relatedEntity, this.dataSource);
      }
      else
        relatedWrappedEntity = (IDataEntity) DataEntityWrapper.NullDataEntity;
      return (DataEntityWrapper) relatedWrappedEntity;
    }

    public IEnumerable<IDataEntity> GetRelatedEntities(
      string relationship,
      EntityDescriptor descriptor)
    {
      return (IEnumerable<IDataEntity>) this.GetRelatedWrappedEntities(relationship, descriptor);
    }

    public IEnumerable<DataEntityWrapper> GetRelatedWrappedEntities(
      string relationship,
      EntityDescriptor descriptor)
    {
      Type classType;
      if (!this.dataSource.TypeLookup.TryGetValue(descriptor.EntityType, out classType))
        classType = typeof (DataEntityWrapper);
      return this.GetDataEntityWrappers(classType, relationship, descriptor, (string) null);
    }

    public IEnumerable<T> GetRelatedWrappedEntities<T>(string relationship, string entityType)
    {
      return this.GetDataEntityWrappers(typeof (T), relationship, (EntityDescriptor) null, entityType).Cast<T>();
    }

    private IEnumerable<DataEntityWrapper> GetDataEntityWrappers(
      Type classType,
      string relationship,
      EntityDescriptor descriptor,
      string entityType)
    {
      IEnumerable<DataEntityWrapper> dataEntityWrappers = (IEnumerable<DataEntityWrapper>) null;
      try
      {
        if (this.entity != null)
        {
          if (string.IsNullOrEmpty(entityType))
            entityType = descriptor.ToString();
          string key = string.Format("{0}:{1}", (object) relationship, (object) entityType);
          if (!this._collectionCache.TryGetValue(key, out dataEntityWrappers))
          {
            if ((EntityDescriptor) null == descriptor)
              descriptor = EntityDescriptor.Create(entityType);
            IEnumerable<IDataEntity> relatedEntities = this.entity.GetRelatedEntities(relationship, descriptor.ToInterned());
            if (relatedEntities != null)
            {
              if (relatedEntities.Any<IDataEntity>())
              {
                dataEntityWrappers = (IEnumerable<DataEntityWrapper>) relatedEntities.Select<IDataEntity, DataEntityWrapper>((Func<IDataEntity, DataEntityWrapper>) (returnedEntity => DataEntityWrapper.GetDataEntityWrapper(classType, descriptor.EntityType, returnedEntity, this.dataSource))).ToList<DataEntityWrapper>();
                this._collectionCache.Add(key, dataEntityWrappers);
              }
            }
          }
        }
      }
      catch
      {
        Tracing.Log(TraceLevel.Error, this.GetType().Name, string.Format("Calculation Error: Entity {0} does not have any entities that fulfill relationship {{{1}}}.", (object) this.GetQualifiedName(), (object) relationship));
      }
      if (dataEntityWrappers == null)
        dataEntityWrappers = (IEnumerable<DataEntityWrapper>) new List<DataEntityWrapper>();
      return dataEntityWrappers;
    }

    public IEnumerable<T> GetRelatedWrappedEntities<T>(
      string relationship,
      EntityDescriptor descriptor)
    {
      return this.GetDataEntityWrappers(typeof (T), relationship, descriptor, (string) null).Cast<T>();
    }

    public bool IsEntityInRelationship(IDataEntity relatedEntity, string relationship)
    {
      try
      {
        return relatedEntity.IsEntityInRelationship(relatedEntity, relationship);
      }
      catch
      {
        return false;
      }
    }

    public object InvokeMethod(string functionName, params object[] parameters)
    {
      Stopwatch stopwatch = new Stopwatch();
      stopwatch.Start();
      object obj;
      try
      {
        obj = this.entity.InvokeMethod(functionName, parameters);
      }
      catch (Exception ex)
      {
        throw new Exception(string.Format("Exception thrown by InvokeMethod(\"{0}\").", (object) functionName), ex);
      }
      stopwatch.Stop();
      Tracing.Log(TraceLevel.Info, nameof (DataEntityWrapper), string.Format("InvokeMethod(\"{0}\") Duration: {1}", (object) functionName, (object) stopwatch.Elapsed));
      return obj;
    }

    public bool IsNull() => this.entity == null;

    public string GetQualifiedName() => "{{" + this.name + ":" + this.type.ToString() + "}}";

    protected virtual object getValue(string fieldName, ref object value)
    {
      if (value == DataEntityWrapper.NotSet)
        value = this.entity != null ? this.entity.GetFieldValue(fieldName) : (object) null;
      return value;
    }

    protected void setValue(string fieldName, ref object value)
    {
      this.entity.SetFieldValue(fieldName, value);
      value = DataEntityWrapper.NotSet;
    }
  }
}
