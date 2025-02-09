// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.ConsentServiceController.Security
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

#nullable disable
namespace EllieMae.EMLite.eFolder.ConsentServiceController
{
  [DebuggerStepThrough]
  [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
  [DataContract(Name = "Security", Namespace = "http://www.elliemae.com/edm/platform")]
  [Serializable]
  public class Security : IExtensibleDataObject, INotifyPropertyChanged
  {
    [NonSerialized]
    private ExtensionDataObject extensionDataField;
    private string PasswordField;
    private string SecurityClientIdField;
    private string UserIdField;

    [Browsable(false)]
    public ExtensionDataObject ExtensionData
    {
      get => this.extensionDataField;
      set => this.extensionDataField = value;
    }

    [DataMember(IsRequired = true)]
    public string Password
    {
      get => this.PasswordField;
      set
      {
        if ((object) this.PasswordField == (object) value)
          return;
        this.PasswordField = value;
        this.RaisePropertyChanged(nameof (Password));
      }
    }

    [DataMember(IsRequired = true)]
    public string SecurityClientId
    {
      get => this.SecurityClientIdField;
      set
      {
        if ((object) this.SecurityClientIdField == (object) value)
          return;
        this.SecurityClientIdField = value;
        this.RaisePropertyChanged(nameof (SecurityClientId));
      }
    }

    [DataMember(IsRequired = true)]
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
