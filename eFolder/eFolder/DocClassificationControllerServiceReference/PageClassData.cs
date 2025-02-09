// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.DocClassificationControllerServiceReference.PageClassData
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

#nullable disable
namespace EllieMae.EMLite.eFolder.DocClassificationControllerServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
  [DataContract(Name = "PageClassData", Namespace = "http://schemas.datacontract.org/2004/07/DocumentClassificationService")]
  [Serializable]
  public class PageClassData : IExtensibleDataObject, INotifyPropertyChanged
  {
    [NonSerialized]
    private ExtensionDataObject extensionDataField;
    [OptionalField]
    private byte[] DataField;
    [OptionalField]
    private string PageTextField;

    [Browsable(false)]
    public ExtensionDataObject ExtensionData
    {
      get => this.extensionDataField;
      set => this.extensionDataField = value;
    }

    [DataMember]
    public byte[] Data
    {
      get => this.DataField;
      set
      {
        if (this.DataField == value)
          return;
        this.DataField = value;
        this.RaisePropertyChanged(nameof (Data));
      }
    }

    [DataMember]
    public string PageText
    {
      get => this.PageTextField;
      set
      {
        if ((object) this.PageTextField == (object) value)
          return;
        this.PageTextField = value;
        this.RaisePropertyChanged(nameof (PageText));
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
