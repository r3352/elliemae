// Decompiled with JetBrains decompiler
// Type: Elli.CalculationEngine.Core.DataSource.DataFieldWrapper
// Assembly: Elli.CalculationEngine.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E7988E98-462C-4B95-BC53-687EC5965B19
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.CalculationEngine.Core.dll

using Elli.CalculationEngine.Core.CalculationLibrary;
using Elli.CalculationEngine.Core.ExpressionParser;
using System;
using System.Runtime.Serialization;

#nullable disable
namespace Elli.CalculationEngine.Core.DataSource
{
  [DataContract]
  public class DataFieldWrapper : IDataField, IDataElement
  {
    [IgnoreDataMember]
    public static DataFieldWrapper NullDataField = DataFieldWrapper.Create((IDataField) null, (DataSourceWrapper) null);
    private IDataField field;
    private string id;
    private DataEntityWrapper parentEntity;
    private string _qualifiedName;

    public CalculationExpression Calculation
    {
      get => this.field != null ? this.field.Calculation : (CalculationExpression) null;
      set
      {
        if (this.field == null)
          return;
        this.field.Calculation = value;
      }
    }

    public IElementIdentity CalcId
    {
      get => this.field != null ? this.field.CalcId : (IElementIdentity) null;
      set
      {
        if (this.field == null)
          return;
        this.field.CalcId = value;
      }
    }

    public ICalculationImpl CalcImpl
    {
      get => this.field != null ? this.field.CalcImpl : (ICalculationImpl) null;
      set
      {
        if (this.field == null)
          return;
        this.field.CalcImpl = value;
      }
    }

    public string Id
    {
      get => this.id;
      set
      {
      }
    }

    public IDataEntity ParentEntity
    {
      get => (IDataEntity) this.parentEntity;
      set
      {
      }
    }

    public DataEntityWrapper WrappedParentEntity
    {
      get => this.parentEntity;
      set
      {
      }
    }

    private DataFieldWrapper(IDataField field, DataSourceWrapper rootDataSource)
    {
      this.field = field;
      this.id = field == null ? string.Empty : field.Id;
      this.parentEntity = field == null ? (DataEntityWrapper) null : DataEntityWrapper.Create(field.ParentEntity, rootDataSource);
    }

    public static DataFieldWrapper Create(IDataField field, DataSourceWrapper rootDataSource)
    {
      return field == null || !(field is DataFieldWrapper) ? new DataFieldWrapper(field, rootDataSource) : (DataFieldWrapper) field;
    }

    public Guid GetElementId() => this.field != null ? this.field.GetElementId() : Guid.Empty;

    public DataElementType GetElementType()
    {
      return this.field != null ? this.field.GetElementType() : DataElementType.DataFieldType;
    }

    public Elli.CalculationEngine.Common.ValueType GetFieldType() => this.field.GetFieldType();

    public object GetValue()
    {
      object obj = (object) null;
      if (EngineUtility.IsTransientField(this.id) || this.parentEntity != null && this.parentEntity.IsNull())
      {
        if (this.field != null)
          obj = this.field.GetValue();
      }
      else
        obj = this.parentEntity == null ? (object) null : this.parentEntity.GetFieldValue(this.id);
      return obj;
    }

    public void SetValue(object newValue)
    {
      if (EngineUtility.IsTransientField(this.id))
      {
        if (this.field == null)
          return;
        this.field.SetValue(newValue);
      }
      else
      {
        if (this.parentEntity == null)
          return;
        this.parentEntity.SetFieldValue(this.id, newValue);
      }
    }

    public bool IsLocked() => this.field != null && this.field.IsLocked();

    public void Lock()
    {
      if (this.field == null)
        return;
      this.field.Lock();
    }

    public void Unlock()
    {
      if (this.field == null)
        return;
      this.field.Unlock();
    }

    public bool IsNull() => this.field == null;

    public string GetQualifiedName()
    {
      return this._qualifiedName ?? (this._qualifiedName = string.Format("{{{0}:{1}}}.[{2}]", (object) this.field.ParentEntity.GetEntityName(), (object) this.field.ParentEntity.GetEntityType(), (object) this.field.Id));
    }
  }
}
