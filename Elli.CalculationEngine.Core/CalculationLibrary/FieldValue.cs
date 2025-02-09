// Decompiled with JetBrains decompiler
// Type: Elli.CalculationEngine.Core.CalculationLibrary.FieldValue
// Assembly: Elli.CalculationEngine.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E7988E98-462C-4B95-BC53-687EC5965B19
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.CalculationEngine.Core.dll

using Elli.CalculationEngine.Common;
using Elli.CalculationEngine.Core.DataSource;
using System.Collections.Generic;
using System.Runtime.Serialization;

#nullable disable
namespace Elli.CalculationEngine.Core.CalculationLibrary
{
  [KnownType(typeof (InputField))]
  [DataContract]
  public class FieldValue : IExtensibleDataObject
  {
    private ExtensionDataObject extensionData;

    [DataMember]
    public bool CalculationDefinitionDefined { get; set; }

    [DataMember]
    public string Name { get; set; }

    [DataMember]
    public ValueType ValueType { get; set; }

    [DataMember(Order = 1)]
    public DataElementType DataElementType { get; set; }

    [DataMember(Order = 1)]
    public FieldValuesAllowedType FieldValuesAllowedType { get; set; }

    [DataMember(Order = 1)]
    public bool IsEntityCreated { get; set; }

    [DataMember(Order = 1)]
    public List<string> Values { get; set; }

    [IgnoreDataMember]
    public string DisplayValue
    {
      get => this.Values != null && this.Values.Count == 1 ? this.Values[0] : "";
    }

    public virtual ExtensionDataObject ExtensionData
    {
      get => this.extensionData;
      set => this.extensionData = value;
    }
  }
}
