// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.IdentityServices.ValidateTokenRequest
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

#nullable disable
namespace EllieMae.EMLite.Server.IdentityServices
{
  [DebuggerStepThrough]
  [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
  [DataContract(Name = "ValidateTokenRequest", Namespace = "http://www.elliemae.com/encompass/identity")]
  [Serializable]
  public class ValidateTokenRequest : IExtensibleDataObject, INotifyPropertyChanged
  {
    [NonSerialized]
    private ExtensionDataObject extensionDataField;
    private string RequestedServiceField;
    [OptionalField]
    private string SecurityTokenField;

    [Browsable(false)]
    public ExtensionDataObject ExtensionData
    {
      get => this.extensionDataField;
      set => this.extensionDataField = value;
    }

    [DataMember(IsRequired = true)]
    public string RequestedService
    {
      get => this.RequestedServiceField;
      set
      {
        if ((object) this.RequestedServiceField == (object) value)
          return;
        this.RequestedServiceField = value;
        this.RaisePropertyChanged(nameof (RequestedService));
      }
    }

    [DataMember]
    public string SecurityToken
    {
      get => this.SecurityTokenField;
      set
      {
        if ((object) this.SecurityTokenField == (object) value)
          return;
        this.SecurityTokenField = value;
        this.RaisePropertyChanged(nameof (SecurityToken));
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void RaisePropertyChanged(string propertyName)
    {
      PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
      if (propertyChanged == null)
        return;
      propertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
