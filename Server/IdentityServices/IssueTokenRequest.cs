// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.IdentityServices.IssueTokenRequest
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

#nullable disable
namespace EllieMae.EMLite.Server.IdentityServices
{
  [DebuggerStepThrough]
  [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
  [DataContract(Name = "IssueTokenRequest", Namespace = "http://www.elliemae.com/encompass/identity")]
  [Serializable]
  public class IssueTokenRequest : IExtensibleDataObject, INotifyPropertyChanged
  {
    [NonSerialized]
    private ExtensionDataObject extensionDataField;
    [OptionalField]
    private Dictionary<string, string> ClaimsField;
    [OptionalField]
    private string ClientIdField;
    [OptionalField]
    private int? ExpirationInMinutesField;
    [OptionalField]
    private string InstanceIdField;
    [OptionalField]
    private DateTime? NotAfterField;
    [OptionalField]
    private DateTime? NotBeforeField;
    [OptionalField]
    private string[] RequestedServicesField;
    [OptionalField]
    private TokenType TokenTypeField;
    [OptionalField]
    private string UserIdField;

    [Browsable(false)]
    public ExtensionDataObject ExtensionData
    {
      get => this.extensionDataField;
      set => this.extensionDataField = value;
    }

    [DataMember]
    public Dictionary<string, string> Claims
    {
      get => this.ClaimsField;
      set
      {
        if (this.ClaimsField == value)
          return;
        this.ClaimsField = value;
        this.RaisePropertyChanged(nameof (Claims));
      }
    }

    [DataMember]
    public string ClientId
    {
      get => this.ClientIdField;
      set
      {
        if ((object) this.ClientIdField == (object) value)
          return;
        this.ClientIdField = value;
        this.RaisePropertyChanged(nameof (ClientId));
      }
    }

    [DataMember]
    public int? ExpirationInMinutes
    {
      get => this.ExpirationInMinutesField;
      set
      {
        if (this.ExpirationInMinutesField.Equals((object) value))
          return;
        this.ExpirationInMinutesField = value;
        this.RaisePropertyChanged(nameof (ExpirationInMinutes));
      }
    }

    [DataMember]
    public string InstanceId
    {
      get => this.InstanceIdField;
      set
      {
        if ((object) this.InstanceIdField == (object) value)
          return;
        this.InstanceIdField = value;
        this.RaisePropertyChanged(nameof (InstanceId));
      }
    }

    [DataMember]
    public DateTime? NotAfter
    {
      get => this.NotAfterField;
      set
      {
        if (this.NotAfterField.Equals((object) value))
          return;
        this.NotAfterField = value;
        this.RaisePropertyChanged(nameof (NotAfter));
      }
    }

    [DataMember]
    public DateTime? NotBefore
    {
      get => this.NotBeforeField;
      set
      {
        if (this.NotBeforeField.Equals((object) value))
          return;
        this.NotBeforeField = value;
        this.RaisePropertyChanged(nameof (NotBefore));
      }
    }

    [DataMember]
    public string[] RequestedServices
    {
      get => this.RequestedServicesField;
      set
      {
        if (this.RequestedServicesField == value)
          return;
        this.RequestedServicesField = value;
        this.RaisePropertyChanged(nameof (RequestedServices));
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

    [DataMember]
    public string UserId
    {
      get => this.UserIdField;
      set
      {
        if ((object) this.UserIdField == (object) value)
          return;
        this.UserIdField = value;
        this.RaisePropertyChanged(nameof (UserId));
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
