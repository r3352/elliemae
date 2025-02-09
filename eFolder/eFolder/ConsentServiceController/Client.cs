// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.ConsentServiceController.Client
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
  [DataContract(Name = "Client", Namespace = "http://www.elliemae.com/edm/platform")]
  [Serializable]
  public class Client : IExtensibleDataObject, INotifyPropertyChanged
  {
    [NonSerialized]
    private ExtensionDataObject extensionDataField;
    [OptionalField]
    private string ClientCityField;
    [OptionalField]
    private string ClientFaxField;
    [OptionalField]
    private string ClientFulfillmentFeeField;
    private string ClientIdField;
    [OptionalField]
    private string ClientPhoneField;
    [OptionalField]
    private string ClientStateField;
    [OptionalField]
    private string ClientStreetAddressField;
    [OptionalField]
    private string ClientUserNameField;
    [OptionalField]
    private string ClientZipField;

    [Browsable(false)]
    public ExtensionDataObject ExtensionData
    {
      get => this.extensionDataField;
      set => this.extensionDataField = value;
    }

    [DataMember(EmitDefaultValue = false)]
    public string ClientCity
    {
      get => this.ClientCityField;
      set
      {
        if ((object) this.ClientCityField == (object) value)
          return;
        this.ClientCityField = value;
        this.RaisePropertyChanged(nameof (ClientCity));
      }
    }

    [DataMember(EmitDefaultValue = false)]
    public string ClientFax
    {
      get => this.ClientFaxField;
      set
      {
        if ((object) this.ClientFaxField == (object) value)
          return;
        this.ClientFaxField = value;
        this.RaisePropertyChanged(nameof (ClientFax));
      }
    }

    [DataMember(EmitDefaultValue = false)]
    public string ClientFulfillmentFee
    {
      get => this.ClientFulfillmentFeeField;
      set
      {
        if ((object) this.ClientFulfillmentFeeField == (object) value)
          return;
        this.ClientFulfillmentFeeField = value;
        this.RaisePropertyChanged(nameof (ClientFulfillmentFee));
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
    public string ClientPhone
    {
      get => this.ClientPhoneField;
      set
      {
        if ((object) this.ClientPhoneField == (object) value)
          return;
        this.ClientPhoneField = value;
        this.RaisePropertyChanged(nameof (ClientPhone));
      }
    }

    [DataMember(EmitDefaultValue = false)]
    public string ClientState
    {
      get => this.ClientStateField;
      set
      {
        if ((object) this.ClientStateField == (object) value)
          return;
        this.ClientStateField = value;
        this.RaisePropertyChanged(nameof (ClientState));
      }
    }

    [DataMember(EmitDefaultValue = false)]
    public string ClientStreetAddress
    {
      get => this.ClientStreetAddressField;
      set
      {
        if ((object) this.ClientStreetAddressField == (object) value)
          return;
        this.ClientStreetAddressField = value;
        this.RaisePropertyChanged(nameof (ClientStreetAddress));
      }
    }

    [DataMember(EmitDefaultValue = false)]
    public string ClientUserName
    {
      get => this.ClientUserNameField;
      set
      {
        if ((object) this.ClientUserNameField == (object) value)
          return;
        this.ClientUserNameField = value;
        this.RaisePropertyChanged(nameof (ClientUserName));
      }
    }

    [DataMember(EmitDefaultValue = false)]
    public string ClientZip
    {
      get => this.ClientZipField;
      set
      {
        if ((object) this.ClientZipField == (object) value)
          return;
        this.ClientZipField = value;
        this.RaisePropertyChanged(nameof (ClientZip));
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
