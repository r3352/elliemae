// Decompiled with JetBrains decompiler
// Type: Elli.CalculationEngine.Core.DataSource.DataSourceWrapper
// Assembly: Elli.CalculationEngine.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E7988E98-462C-4B95-BC53-687EC5965B19
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.CalculationEngine.Core.dll

using Elli.CalculationEngine.Common;
using Elli.CalculationEngine.Core.CalculationLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

#nullable disable
namespace Elli.CalculationEngine.Core.DataSource
{
  public abstract class DataSourceWrapper : 
    DataEntityWrapper,
    IDataSource,
    IDisposable,
    IDataEntity,
    IDataElement
  {
    private Dictionary<string, DataEntityWrapper> _entityCache;
    private Dictionary<Tuple<string, string>, TransientField> transientFields;
    private CalculationSet calculationSet;
    private StringBuilder sbCalculationLog = new StringBuilder();

    public Dictionary<string, Type> TypeLookup { get; protected set; }

    protected DataSourceWrapper()
    {
    }

    protected DataSourceWrapper(
      IDataSource source,
      CalculationSet calculationSet,
      Dictionary<Tuple<string, string>, TransientField> initialTransients = null)
      : this()
    {
      this._entityCache = new Dictionary<string, DataEntityWrapper>();
      this.transientFields = initialTransients != null ? new Dictionary<Tuple<string, string>, TransientField>((IDictionary<Tuple<string, string>, TransientField>) initialTransients) : new Dictionary<Tuple<string, string>, TransientField>();
      this.entity = (IDataEntity) source;
      this.name = this.entity.GetEntityName();
      this.type = this.entity.GetEntityType();
      this.calculationSet = calculationSet;
      this._entityCache.Add(source.GetEntityName(), (DataEntityWrapper) this);
    }

    public Dictionary<string, DataEntityWrapper> EntityCache => this._entityCache;

    public string CalculationLog
    {
      get => this.sbCalculationLog.ToString();
      private set
      {
      }
    }

    public void Log(string message) => this.sbCalculationLog.AppendLine(message);

    public string RootRelationship
    {
      get => this.calculationSet.RootRelationship;
      set
      {
      }
    }

    public Dictionary<Tuple<string, string>, TransientField> TransientFields
    {
      get => this.transientFields;
      private set
      {
      }
    }

    public DataContractSerializer GetSerializer() => ((IDataSource) this.entity).GetSerializer();

    public void Dispose() => this.Dispose(true);

    protected void Dispose(bool disposing)
    {
    }

    public void ExportDataSource(string exportFile)
    {
      if (this.entity == null)
        return;
      DataContractSerializer serializer = ((IDataSource) this.entity).GetSerializer();
      XmlWriter xmlWriter = XmlWriter.Create(exportFile);
      XmlWriter writer = xmlWriter;
      IDataSource entity = (IDataSource) this.entity;
      serializer.WriteObject(writer, (object) entity);
      xmlWriter.Close();
    }

    public void ImportDataSource(string importFile)
    {
      if (this.entity == null)
        return;
      DataContractSerializer serializer = ((IDataSource) this.entity).GetSerializer();
      XmlReader reader = XmlReader.Create(importFile);
      this.entity = (IDataEntity) serializer.ReadObject(reader);
      reader.Close();
    }

    public new IEnumerable<string> GetFieldList()
    {
      return (this.entity == null ? (IEnumerable<string>) new List<string>() : this.entity.GetFieldList()).Concat<string>(this.transientFields.Keys.Select<Tuple<string, string>, string>((Func<Tuple<string, string>, string>) (keys => keys.ToString())));
    }

    public IEnumerable<IDataEntity> GetAllEntitiesOfType(
      EntityDescriptor entityType,
      StringBuilder sbTrace = null)
    {
      return (IEnumerable<IDataEntity>) this.GetAllWrappedEntitiesOfType(entityType, sbTrace);
    }

    public IEnumerable<DataEntityWrapper> GetAllWrappedEntitiesOfType(
      EntityDescriptor entityType,
      StringBuilder sbTrace = null)
    {
      IEnumerable<IDataEntity> source;
      try
      {
        source = ((IDataSource) this.entity).GetAllEntitiesOfType(entityType.ToInterned());
      }
      catch
      {
        entityType.EntityParameters.Clear();
        sbTrace?.AppendLine("DataSourceWrapper | -- GetAllWrappedEntitiesOfType() catch after entityType.EntityParameters.Clear() --");
        source = ((IDataSource) this.entity).GetAllEntitiesOfType(entityType.ToInterned());
      }
      if (source == null)
      {
        source = (IEnumerable<IDataEntity>) new List<IDataEntity>();
        sbTrace?.AppendLine(string.Format("DataSourceWrapper | -- GetAllWrappedEntitiesOfType() entities is null, entityType = {0}--", (object) entityType.ToString()));
      }
      else if (source.Any<IDataEntity>((Func<IDataEntity, bool>) (e => e == null)))
        throw new Exception(string.Format("GetAllEntitiesOfType returned a collection containing a null item for entity of type \"{0}\"; Count = {1}", (object) entityType, (object) source.Count<IDataEntity>()));
      return source.Select<IDataEntity, DataEntityWrapper>((Func<IDataEntity, DataEntityWrapper>) (element => DataEntityWrapper.Create(element, this.dataSource)));
    }

    public void InitializeDataField(IDataField dataSourceField)
    {
      EntityDescriptor entityDescriptor = dataSourceField.ParentEntity != null ? dataSourceField.ParentEntity.GetEntityType() : throw new Exception("DataSource Parent Entity cannot be null");
      FieldExpressionCalculation expressionCalculation = this.calculationSet.GetCalculation(dataSourceField.Id, entityDescriptor.ToString()) ?? this.calculationSet.GetCalculation(dataSourceField.Id, entityDescriptor.EntityType);
      if (expressionCalculation == null)
        return;
      expressionCalculation.Identity.ParentId = this.calculationSet.Identity.Id;
      dataSourceField.CalcImpl = this.calculationSet.GetCalcImpl(expressionCalculation.Identity);
      dataSourceField.CalcId = (IElementIdentity) expressionCalculation.Identity;
      dataSourceField.Calculation = expressionCalculation.Expression;
    }

    public void ClearEntityCache()
    {
      this._collectionCache.Clear();
      foreach (DataEntityWrapper dataEntityWrapper in this._entityCache.Values)
        dataEntityWrapper.ClearProperties();
      foreach (EntityDescriptor entityType in this.calculationSet.EntityTypes)
      {
        if (!entityType.IsBaseType())
          this.dataSource.GetAllEntitiesOfType(entityType);
      }
    }

    private TransientField AddTransientField(IDataEntity entity, string fieldId, Elli.CalculationEngine.Common.ValueType type)
    {
      EntityDescriptor entityType1 = entity.GetEntityType();
      string parentEntityType = entityType1.ToString();
      FieldExpressionCalculation calculation = this.calculationSet.GetCalculation(fieldId, parentEntityType);
      if (calculation == null)
      {
        string entityType2 = entityType1.EntityType;
        calculation = this.calculationSet.GetCalculation(fieldId, entityType2);
        if (calculation == null)
          throw new Exception(string.Format("Error initializing transient field {{{0}}}.[{1}], no calculation defined.", (object) entityType1, (object) fieldId));
      }
      if (type == Elli.CalculationEngine.Common.ValueType.None)
        type = calculation.Expression.ReturnType;
      DataEntityWrapper parentEntity = DataEntityWrapper.Create(entity, this);
      TransientField transientField = new TransientField(fieldId, type, (IDataEntity) parentEntity);
      calculation.Identity.ParentId = this.calculationSet.Identity.Id;
      Utility.AddCalculationToIdentityWhitelist(calculation.Identity.ClassName);
      transientField.CalcImpl = this.calculationSet.GetCalcImpl(calculation.Identity);
      transientField.CalcId = (IElementIdentity) calculation.Identity;
      transientField.Calculation = calculation.Expression;
      return transientField;
    }

    public TransientField GetTransientField(
      DataEntityWrapper entity,
      string uniqueEntityName,
      string fieldId)
    {
      TransientField transientField = (TransientField) null;
      Tuple<string, string> key = new Tuple<string, string>(uniqueEntityName, fieldId);
      if (this.transientFields.TryGetValue(key, out transientField))
      {
        DataEntityWrapper dataEntityWrapper = DataEntityWrapper.Create((IDataEntity) entity, this);
        transientField.ParentEntity = (IDataEntity) dataEntityWrapper;
      }
      else
      {
        transientField = this.AddTransientField((IDataEntity) entity, fieldId, Elli.CalculationEngine.Common.ValueType.None);
        this.transientFields.Add(key, transientField);
      }
      return transientField;
    }

    public IEnumerable<IDataEntity> GetAllEntitiesOfType(EntityDescriptor entityType)
    {
      return this.GetAllEntitiesOfType(entityType, (StringBuilder) null);
    }
  }
}
