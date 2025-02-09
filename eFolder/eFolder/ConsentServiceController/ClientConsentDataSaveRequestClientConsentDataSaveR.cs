// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.ConsentServiceController.ClientConsentDataSaveRequestClientConsentDataSaveRequestBody
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
  [DataContract(Name = "ClientConsentDataSaveRequest.ClientConsentDataSaveRequestBody", Namespace = "http://www.elliemae.com/edm/platform")]
  [Serializable]
  public class ClientConsentDataSaveRequestClientConsentDataSaveRequestBody : 
    IExtensibleDataObject,
    INotifyPropertyChanged
  {
    [NonSerialized]
    private ExtensionDataObject extensionDataField;
    [OptionalField]
    private string CityField;
    private string ClientIdField;
    [OptionalField]
    private int? ConsentModelField;
    [OptionalField]
    private string FaxField;
    [OptionalField]
    private string FulfillmentFeeField;
    [OptionalField]
    private string PhoneField;
    [OptionalField]
    private string StateField;
    [OptionalField]
    private string StreetAddressField;
    [OptionalField]
    private string UserNameField;
    [OptionalField]
    private string ZipField;

    [Browsable(false)]
    public ExtensionDataObject ExtensionData
    {
      get => this.extensionDataField;
      set => this.extensionDataField = value;
    }

    [DataMember(EmitDefaultValue = false)]
    public string City
    {
      get => this.CityField;
      set
      {
        if ((object) this.CityField == (object) value)
          return;
        this.CityField = value;
        this.RaisePropertyChanged(nameof (City));
      }
    }

    [DataMember(IsRequired = true)]
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

    [DataMember(EmitDefaultValue = false)]
    public int? ConsentModel
    {
      get => this.ConsentModelField;
      set
      {
        if (this.ConsentModelField.Equals((object) value))
          return;
        this.ConsentModelField = value;
        this.RaisePropertyChanged(nameof (ConsentModel));
      }
    }

    [DataMember(EmitDefaultValue = false)]
    public string Fax
    {
      get => this.FaxField;
      set
      {
        if ((object) this.FaxField == (object) value)
          return;
        this.FaxField = value;
        this.RaisePropertyChanged(nameof (Fax));
      }
    }

    [DataMember(EmitDefaultValue = false)]
    public string FulfillmentFee
    {
      get => this.FulfillmentFeeField;
      set
      {
        if ((object) this.FulfillmentFeeField == (object) value)
          return;
        this.FulfillmentFeeField = value;
        this.RaisePropertyChanged(nameof (FulfillmentFee));
      }
    }

    [DataMember(EmitDefaultValue = false)]
    public string Phone
    {
      get => this.PhoneField;
      set
      {
        if ((object) this.PhoneField == (object) value)
          return;
        this.PhoneField = value;
        this.RaisePropertyChanged(nameof (Phone));
      }
    }

    [DataMember(EmitDefaultValue = false)]
    public string State
    {
      get => this.StateField;
      set
      {
        if ((object) this.StateField == (object) value)
          return;
        this.StateField = value;
        this.RaisePropertyChanged(nameof (State));
      }
    }

    [DataMember(EmitDefaultValue = false)]
    public string StreetAddress
    {
      get => this.StreetAddressField;
      set
      {
        if ((object) this.StreetAddressField == (object) value)
          return;
        this.StreetAddressField = value;
        this.RaisePropertyChanged(nameof (StreetAddress));
      }
    }

    [DataMember(EmitDefaultValue = false)]
    public string UserName
    {
      get => this.UserNameField;
      set
      {
        if ((object) this.UserNameField == (object) value)
          return;
        this.UserNameField = value;
        this.RaisePropertyChanged(nameof (UserName));
      }
    }

    [DataMember(EmitDefaultValue = false)]
    public string Zip
    {
      get => this.ZipField;
      set
      {
        if ((object) this.ZipField == (object) value)
          return;
        this.ZipField = value;
        this.RaisePropertyChanged(nameof (Zip));
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
