// Decompiled with JetBrains decompiler
// Type: Elli.CalculationEngine.Core.DataSource.TransientField
// Assembly: Elli.CalculationEngine.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E7988E98-462C-4B95-BC53-687EC5965B19
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.CalculationEngine.Core.dll

using Elli.CalculationEngine.Core.CalculationLibrary;
using Elli.CalculationEngine.Core.ExpressionParser;
using System;

#nullable disable
namespace Elli.CalculationEngine.Core.DataSource
{
  public class TransientField : IDataField, IDataElement
  {
    public static readonly object NotSet = new object();
    private object value;
    private Elli.CalculationEngine.Common.ValueType type;
    private bool isLocked;
    private Guid elementId;

    public CalculationExpression Calculation { get; set; }

    public IElementIdentity CalcId { get; set; }

    public ICalculationImpl CalcImpl { get; set; }

    public string Id { get; set; }

    public IDataEntity ParentEntity { get; set; }

    public TransientField(string fieldId, Elli.CalculationEngine.Common.ValueType newType, IDataEntity parentEntity)
    {
      this.Id = fieldId;
      this.ParentEntity = parentEntity;
      this.type = newType;
      this.elementId = Guid.NewGuid();
      this.SetDefaultValue();
    }

    public Guid GetElementId() => this.elementId;

    public DataElementType GetElementType() => DataElementType.DataFieldType;

    public Elli.CalculationEngine.Common.ValueType GetFieldType() => this.type;

    public object GetValue() => this.value;

    public void SetValue(object newValue)
    {
      if (this.value == null)
        this.SetDefaultValue();
      try
      {
        this.value = CalculationUtility.ConvertValueType(newValue, this.type);
      }
      catch (Exception ex)
      {
        throw new Exception(string.Format("Error converting value \"{0}\" to type {1})", this.value, (object) this.type.ToString()), ex.InnerException);
      }
    }

    private void SetDefaultValue() => this.value = TransientField.NotSet;

    public bool IsLocked() => this.isLocked;

    public void Lock() => this.isLocked = true;

    public void Unlock() => this.isLocked = false;
  }
}
