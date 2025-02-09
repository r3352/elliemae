// Decompiled with JetBrains decompiler
// Type: Elli.CalculationEngine.Core.CalculationLibrary.CalculationTest
// Assembly: Elli.CalculationEngine.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E7988E98-462C-4B95-BC53-687EC5965B19
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.CalculationEngine.Core.dll

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

#nullable disable
namespace Elli.CalculationEngine.Core.CalculationLibrary
{
  [DataContract]
  public class CalculationTest : IExtensibleDataObject
  {
    private ExtensionDataObject extensionData;

    [DataMember]
    public Guid Id { get; set; }

    [DataMember]
    public string Name { get; set; }

    [DataMember]
    public string Description { get; set; }

    [DataMember]
    public List<InputField> InputList { get; set; }

    [DataMember]
    public FieldValue OutputField { get; set; }

    [DataMember]
    public bool IsAutomatedTest { get; set; }

    public CalculationTest()
    {
    }

    public CalculationTest(Guid id)
    {
      this.Id = id;
      this.IsAutomatedTest = true;
    }

    public virtual ExtensionDataObject ExtensionData
    {
      get => this.extensionData;
      set => this.extensionData = value;
    }
  }
}
