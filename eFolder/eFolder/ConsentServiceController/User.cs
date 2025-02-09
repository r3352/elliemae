// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.ConsentServiceController.User
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
  [DataContract(Name = "User", Namespace = "http://www.elliemae.com/edm/platform")]
  [Serializable]
  public class User : IExtensibleDataObject, INotifyPropertyChanged
  {
    [NonSerialized]
    private ExtensionDataObject extensionDataField;
    private string UserEmailField;
    private string UserFirstNameField;
    private string UserLastNameField;
    [OptionalField]
    private string UserTypeField;

    [Browsable(false)]
    public ExtensionDataObject ExtensionData
    {
      get => this.extensionDataField;
      set => this.extensionDataField = value;
    }

    [DataMember(IsRequired = true)]
    public string UserEmail
    {
      get => this.UserEmailField;
      set
      {
        if ((object) this.UserEmailField == (object) value)
          return;
        this.UserEmailField = value;
        this.RaisePropertyChanged(nameof (UserEmail));
      }
    }

    [DataMember(IsRequired = true)]
    public string UserFirstName
    {
      get => this.UserFirstNameField;
      set
      {
        if ((object) this.UserFirstNameField == (object) value)
          return;
        this.UserFirstNameField = value;
        this.RaisePropertyChanged(nameof (UserFirstName));
      }
    }

    [DataMember(IsRequired = true)]
    public string UserLastName
    {
      get => this.UserLastNameField;
      set
      {
        if ((object) this.UserLastNameField == (object) value)
          return;
        this.UserLastNameField = value;
        this.RaisePropertyChanged(nameof (UserLastName));
      }
    }

    [DataMember(EmitDefaultValue = false)]
    public string UserType
    {
      get => this.UserTypeField;
      set
      {
        if ((object) this.UserTypeField == (object) value)
          return;
        this.UserTypeField = value;
        this.RaisePropertyChanged(nameof (UserType));
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
