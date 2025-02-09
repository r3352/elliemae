// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.IdentityServices.SecurityTokenRequest
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
  [DataContract(Name = "SecurityTokenRequest", Namespace = "http://schemas.datacontract.org/2004/07/Elli.Identity.ServiceModel")]
  [Serializable]
  public class SecurityTokenRequest : IExtensibleDataObject, INotifyPropertyChanged
  {
    [NonSerialized]
    private ExtensionDataObject extensionDataField;
    [OptionalField]
    private string IdentityTokenField;
    [OptionalField]
    private DateTime NotAfterField;
    [OptionalField]
    private DateTime NotBeforeField;
    [OptionalField]
    private string RequestedServiceField;
    [OptionalField]
    private TokenType TokenTypeField;

    [Browsable(false)]
    public ExtensionDataObject ExtensionData
    {
      get => this.extensionDataField;
      set => this.extensionDataField = value;
    }

    [DataMember]
    public string IdentityToken
    {
      get => this.IdentityTokenField;
      set
      {
        if ((object) this.IdentityTokenField == (object) value)
          return;
        this.IdentityTokenField = value;
        this.RaisePropertyChanged(nameof (IdentityToken));
      }
    }

    [DataMember]
    public DateTime NotAfter
    {
      get => this.NotAfterField;
      set
      {
        if (this.NotAfterField.Equals(value))
          return;
        this.NotAfterField = value;
        this.RaisePropertyChanged(nameof (NotAfter));
      }
    }

    [DataMember]
    public DateTime NotBefore
    {
      get => this.NotBeforeField;
      set
      {
        if (this.NotBeforeField.Equals(value))
          return;
        this.NotBeforeField = value;
        this.RaisePropertyChanged(nameof (NotBefore));
      }
    }

    [DataMember]
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
    public TokenType TokenType
    {
      get => this.TokenTypeField;
      set
      {
        if (this.TokenTypeField.Equals((object) value))
          return;
        this.TokenTypeField = value;
        this.RaisePropertyChanged(nameof (TokenType));
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
