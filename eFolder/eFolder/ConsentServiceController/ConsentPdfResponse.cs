// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.ConsentServiceController.ConsentPdfResponse
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
  [DataContract(Name = "ConsentPdfResponse", Namespace = "http://www.elliemae.com/edm/platform")]
  [Serializable]
  public class ConsentPdfResponse : IExtensibleDataObject, INotifyPropertyChanged
  {
    [NonSerialized]
    private ExtensionDataObject extensionDataField;
    [OptionalField]
    private string ConsentPdfField;
    [OptionalField]
    private int? PackageIdField;

    [Browsable(false)]
    public ExtensionDataObject ExtensionData
    {
      get => this.extensionDataField;
      set => this.extensionDataField = value;
    }

    [DataMember]
    public string ConsentPdf
    {
      get => this.ConsentPdfField;
      set
      {
        if ((object) this.ConsentPdfField == (object) value)
          return;
        this.ConsentPdfField = value;
        this.RaisePropertyChanged(nameof (ConsentPdf));
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
