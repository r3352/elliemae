// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.IdentityServices.SecurityTokenResponse
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
  [DataContract(Name = "SecurityTokenResponse", Namespace = "http://schemas.datacontract.org/2004/07/Elli.Identity.ServiceModel")]
  [Serializable]
  public class SecurityTokenResponse : IExtensibleDataObject, INotifyPropertyChanged
  {
    [NonSerialized]
    private ExtensionDataObject extensionDataField;
    [OptionalField]
    private string TokenField;
    [OptionalField]
    private TokenType TokenTypeField;

    [Browsable(false)]
    public ExtensionDataObject ExtensionData
    {
      get => this.extensionDataField;
      set => this.extensionDataField = value;
    }

    [DataMember]
    public string Token
    {
      get => this.TokenField;
      set
      {
        if ((object) this.TokenField == (object) value)
          return;
        this.TokenField = value;
        this.RaisePropertyChanged(nameof (Token));
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
