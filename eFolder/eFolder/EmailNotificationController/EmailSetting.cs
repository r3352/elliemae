// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.EmailNotificationController.EmailSetting
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

#nullable disable
namespace EllieMae.EMLite.eFolder.EmailNotificationController
{
  [DebuggerStepThrough]
  [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
  [DataContract(Name = "EmailSetting", Namespace = "http://www.elliemae.com/edm/platform")]
  [KnownType(typeof (FailedEmailSetting))]
  [Serializable]
  public class EmailSetting : IExtensibleDataObject, INotifyPropertyChanged
  {
    [NonSerialized]
    private ExtensionDataObject extensionDataField;
    private string EmailAddressField;
    private Guid LoanIdField;
    [OptionalField]
    private DateTime ValidTillField;

    [Browsable(false)]
    public ExtensionDataObject ExtensionData
    {
      get => this.extensionDataField;
      set => this.extensionDataField = value;
    }

    [DataMember(IsRequired = true)]
    public string EmailAddress
    {
      get => this.EmailAddressField;
      set
      {
        if ((object) this.EmailAddressField == (object) value)
          return;
        this.EmailAddressField = value;
        this.RaisePropertyChanged(nameof (EmailAddress));
      }
    }

    [DataMember(IsRequired = true)]
    public Guid LoanId
    {
      get => this.LoanIdField;
      set
      {
        if (this.LoanIdField.Equals(value))
          return;
        this.LoanIdField = value;
        this.RaisePropertyChanged(nameof (LoanId));
      }
    }

    [DataMember]
    public DateTime ValidTill
    {
      get => this.ValidTillField;
      set
      {
        if (this.ValidTillField.Equals(value))
          return;
        this.ValidTillField = value;
        this.RaisePropertyChanged(nameof (ValidTill));
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
