// Decompiled with JetBrains decompiler
// Type: Elli.CalculationEngine.Core.CalculationLibrary.ElementIdentity
// Assembly: Elli.CalculationEngine.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E7988E98-462C-4B95-BC53-687EC5965B19
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.CalculationEngine.Core.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace Elli.CalculationEngine.Core.CalculationLibrary
{
  [DataContract]
  public class ElementIdentity : IElementIdentity, IExtensibleDataObject
  {
    private Guid parentId;
    private string assemblyName;
    private string className;
    private ExtensionDataObject extensionData;

    [DataMember]
    public Guid Id { get; set; }

    [DataMember]
    public string Description { get; set; }

    [DataMember]
    public LibraryElementType Type { get; set; }

    public string AssemblyName
    {
      get
      {
        if (this.assemblyName == null)
          this.assemblyName = this.ParentId.ToString("N");
        return this.assemblyName;
      }
    }

    public string ClassName
    {
      get
      {
        if (this.className == null)
          this.className = string.Format("{0}_{1}", (object) this.Type.ToString(), (object) this.Id.ToString("N"));
        return this.className;
      }
    }

    public Guid ParentId
    {
      get => this.parentId;
      set => this.parentId = value;
    }

    public override string ToString() => this.Id.ToString("N");

    public bool Equals(IElementIdentity id) => true;

    public virtual ExtensionDataObject ExtensionData
    {
      get => this.extensionData;
      set => this.extensionData = value;
    }
  }
}
