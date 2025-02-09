// Decompiled with JetBrains decompiler
// Type: Elli.CalculationEngine.Core.CalculationLibrary.LibraryElement
// Assembly: Elli.CalculationEngine.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E7988E98-462C-4B95-BC53-687EC5965B19
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.CalculationEngine.Core.dll

using System.Runtime.Serialization;

#nullable disable
namespace Elli.CalculationEngine.Core.CalculationLibrary
{
  [KnownType(typeof (FieldExpressionCalculation))]
  [KnownType(typeof (Function))]
  [KnownType(typeof (TransientDataObject))]
  [KnownType(typeof (CalculationTemplate))]
  [DataContract]
  public class LibraryElement : IExtensibleDataObject
  {
    private ExtensionDataObject extensionData;

    [DataMember]
    public ElementIdentity Identity { get; set; }

    public string IdentityString
    {
      get => this.Identity != null ? this.Identity.ToString() : string.Empty;
      set
      {
      }
    }

    public virtual ExtensionDataObject ExtensionData
    {
      get => this.extensionData;
      set => this.extensionData = value;
    }
  }
}
