// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.IdentityServices.ValidateTokenResponse
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
  [DataContract(Name = "ValidateTokenResponse", Namespace = "http://www.elliemae.com/encompass/identity")]
  [Serializable]
  public class ValidateTokenResponse : IExtensibleDataObject, INotifyPropertyChanged
  {
    [NonSerialized]
    private ExtensionDataObject extensionDataField;
    [OptionalField]
    private Dictionary<string, string> ClaimsField;
    [OptionalField]
    private DateTime? ExpiresUtcField;

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
    public DateTime? ExpiresUtc
    {
      get => this.ExpiresUtcField;
      set
      {
        if (this.ExpiresUtcField.Equals((object) value))
          return;
        this.ExpiresUtcField = value;
        this.RaisePropertyChanged(nameof (ExpiresUtc));
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
