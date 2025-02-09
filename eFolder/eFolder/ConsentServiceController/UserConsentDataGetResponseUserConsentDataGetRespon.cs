// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.ConsentServiceController.UserConsentDataGetResponseUserConsentDataGetResponseBody
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
  [DataContract(Name = "UserConsentDataGetResponse.UserConsentDataGetResponseBody", Namespace = "http://www.elliemae.com/edm/platform")]
  [Serializable]
  public class UserConsentDataGetResponseUserConsentDataGetResponseBody : 
    IExtensibleDataObject,
    INotifyPropertyChanged
  {
    [NonSerialized]
    private ExtensionDataObject extensionDataField;
    [OptionalField]
    private string ApplicationIdField;
    [OptionalField]
    private DateTime? ConsentDateField;
    [OptionalField]
    private string ConsentIpAddressField;
    [OptionalField]
    private bool? ConsentStatusField;
    [OptionalField]
    private DateTime? DateConsentRequestField;
    [OptionalField]
    private int? PackageIdField;
    [OptionalField]
    private User UserField;

    [Browsable(false)]
    public ExtensionDataObject ExtensionData
    {
      get => this.extensionDataField;
      set => this.extensionDataField = value;
    }

    [DataMember]
    public string ApplicationId
    {
      get => this.ApplicationIdField;
      set
      {
        if ((object) this.ApplicationIdField == (object) value)
          return;
        this.ApplicationIdField = value;
        this.RaisePropertyChanged(nameof (ApplicationId));
      }
    }

    [DataMember]
    public DateTime? ConsentDate
    {
      get => this.ConsentDateField;
      set
      {
        if (this.ConsentDateField.Equals((object) value))
          return;
        this.ConsentDateField = value;
        this.RaisePropertyChanged(nameof (ConsentDate));
      }
    }

    [DataMember]
    public string ConsentIpAddress
    {
      get => this.ConsentIpAddressField;
      set
      {
        if ((object) this.ConsentIpAddressField == (object) value)
          return;
        this.ConsentIpAddressField = value;
        this.RaisePropertyChanged(nameof (ConsentIpAddress));
      }
    }

    [DataMember]
    public bool? ConsentStatus
    {
      get => this.ConsentStatusField;
      set
      {
        if (this.ConsentStatusField.Equals((object) value))
          return;
        this.ConsentStatusField = value;
        this.RaisePropertyChanged(nameof (ConsentStatus));
      }
    }

    [DataMember]
    public DateTime? DateConsentRequest
    {
      get => this.DateConsentRequestField;
      set
      {
        if (this.DateConsentRequestField.Equals((object) value))
          return;
        this.DateConsentRequestField = value;
        this.RaisePropertyChanged(nameof (DateConsentRequest));
      }
    }

    [DataMember]
    public int? PackageId
    {
      get => this.PackageIdField;
      set
      {
        if (this.PackageIdField.Equals((object) value))
          return;
        this.PackageIdField = value;
        this.RaisePropertyChanged(nameof (PackageId));
      }
    }

    [DataMember]
    public User User
    {
      get => this.UserField;
      set
      {
        if (this.UserField == value)
          return;
        this.UserField = value;
        this.RaisePropertyChanged(nameof (User));
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
