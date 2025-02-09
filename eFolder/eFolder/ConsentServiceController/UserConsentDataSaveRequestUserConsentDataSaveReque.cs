// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.ConsentServiceController.UserConsentDataSaveRequestUserConsentDataSaveRequestBody
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
  [DataContract(Name = "UserConsentDataSaveRequest.UserConsentDataSaveRequestBody", Namespace = "http://www.elliemae.com/edm/platform")]
  [Serializable]
  public class UserConsentDataSaveRequestUserConsentDataSaveRequestBody : 
    IExtensibleDataObject,
    INotifyPropertyChanged
  {
    [NonSerialized]
    private ExtensionDataObject extensionDataField;
    private string ApplicationIdField;
    [OptionalField]
    private Client ClientField;
    [OptionalField]
    private string ConsentIpAddressField;
    [OptionalField]
    private bool? ConsentRequestField;
    [OptionalField]
    private bool? ConsentStatusField;
    [OptionalField]
    private bool? IsFromDeclineField;
    private string LoanGuidField;
    [OptionalField]
    private int? PackageIdField;
    [OptionalField]
    private bool? UseBranchAddressField;
    [OptionalField]
    private User UserField;

    [Browsable(false)]
    public ExtensionDataObject ExtensionData
    {
      get => this.extensionDataField;
      set => this.extensionDataField = value;
    }

    [DataMember(IsRequired = true)]
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
    public Client Client
    {
      get => this.ClientField;
      set
      {
        if (this.ClientField == value)
          return;
        this.ClientField = value;
        this.RaisePropertyChanged(nameof (Client));
      }
    }

    [DataMember(EmitDefaultValue = false)]
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

    [DataMember(EmitDefaultValue = false)]
    public bool? ConsentRequest
    {
      get => this.ConsentRequestField;
      set
      {
        if (this.ConsentRequestField.Equals((object) value))
          return;
        this.ConsentRequestField = value;
        this.RaisePropertyChanged(nameof (ConsentRequest));
      }
    }

    [DataMember(EmitDefaultValue = false)]
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

    [DataMember(EmitDefaultValue = false)]
    public bool? IsFromDecline
    {
      get => this.IsFromDeclineField;
      set
      {
        if (this.IsFromDeclineField.Equals((object) value))
          return;
        this.IsFromDeclineField = value;
        this.RaisePropertyChanged(nameof (IsFromDecline));
      }
    }

    [DataMember(IsRequired = true)]
    public string LoanGuid
    {
      get => this.LoanGuidField;
      set
      {
        if ((object) this.LoanGuidField == (object) value)
          return;
        this.LoanGuidField = value;
        this.RaisePropertyChanged(nameof (LoanGuid));
      }
    }

    [DataMember(EmitDefaultValue = false)]
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

    [DataMember(EmitDefaultValue = false)]
    public bool? UseBranchAddress
    {
      get => this.UseBranchAddressField;
      set
      {
        if (this.UseBranchAddressField.Equals((object) value))
          return;
        this.UseBranchAddressField = value;
        this.RaisePropertyChanged(nameof (UseBranchAddress));
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
