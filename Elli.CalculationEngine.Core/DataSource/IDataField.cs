// Decompiled with JetBrains decompiler
// Type: Elli.CalculationEngine.Core.DataSource.IDataField
// Assembly: Elli.CalculationEngine.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E7988E98-462C-4B95-BC53-687EC5965B19
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.CalculationEngine.Core.dll

using Elli.CalculationEngine.Common;
using Elli.CalculationEngine.Core.CalculationLibrary;
using Elli.CalculationEngine.Core.ExpressionParser;
using System.Runtime.Serialization;

#nullable disable
namespace Elli.CalculationEngine.Core.DataSource
{
  public interface IDataField : IDataElement
  {
    string Id { get; set; }

    IDataEntity ParentEntity { get; set; }

    CalculationExpression Calculation { get; set; }

    [IgnoreDataMember]
    IElementIdentity CalcId { get; set; }

    [IgnoreDataMember]
    ICalculationImpl CalcImpl { get; set; }

    ValueType GetFieldType();

    object GetValue();

    void SetValue(object value);

    void Lock();

    void Unlock();

    bool IsLocked();
  }
}
