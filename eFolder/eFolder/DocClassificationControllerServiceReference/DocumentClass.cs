// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.DocClassificationControllerServiceReference.DocumentClass
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
  [DataContract(Name = "DocumentClass", Namespace = "http://schemas.datacontract.org/2004/07/DocumentClassificationService")]
  [Serializable]
  public class DocumentClass : IExtensibleDataObject, INotifyPropertyChanged
  {
    [NonSerialized]
    private ExtensionDataObject extensionDataField;
    [OptionalField]
    private string AdminStatusField;
    [OptionalField]
    private string AdminStatusByField;
    [OptionalField]
    private string AdminStatusDateField;
    [OptionalField]
    private string DocumentGUIDField;
    [OptionalField]
    private string DocumentTitleField;
    [OptionalField]
    private bool PendingSuggestionsField;

    [Browsable(false)]
    public ExtensionDataObject ExtensionData
    {
      get => this.extensionDataField;
      set => this.extensionDataField = value;
    }

    [DataMember]
    public string AdminStatus
    {
      get => this.AdminStatusField;
      set
      {
        if ((object) this.AdminStatusField == (object) value)
          return;
        this.AdminStatusField = value;
        this.RaisePropertyChanged(nameof (AdminStatus));
      }
    }

    [DataMember]
    public string AdminStatusBy
    {
      get => this.AdminStatusByField;
      set
      {
        if ((object) this.AdminStatusByField == (object) value)
          return;
        this.AdminStatusByField = value;
        this.RaisePropertyChanged(nameof (AdminStatusBy));
      }
    }

    [DataMember]
    public string AdminStatusDate
    {
      get => this.AdminStatusDateField;
      set
      {
        if ((object) this.AdminStatusDateField == (object) value)
          return;
        this.AdminStatusDateField = value;
        this.RaisePropertyChanged(nameof (AdminStatusDate));
      }
    }

    [DataMember]
    public string DocumentGUID
    {
      get => this.DocumentGUIDField;
      set
      {
        if ((object) this.DocumentGUIDField == (object) value)
          return;
        this.DocumentGUIDField = value;
        this.RaisePropertyChanged(nameof (DocumentGUID));
      }
    }

    [DataMember]
    public string DocumentTitle
    {
      get => this.DocumentTitleField;
      set
      {
        if ((object) this.DocumentTitleField == (object) value)
          return;
        this.DocumentTitleField = value;
        this.RaisePropertyChanged(nameof (DocumentTitle));
      }
    }

    [DataMember]
    public bool PendingSuggestions
    {
      get => this.PendingSuggestionsField;
      set
      {
        if (this.PendingSuggestionsField.Equals(value))
          return;
        this.PendingSuggestionsField = value;
        this.RaisePropertyChanged(nameof (PendingSuggestions));
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
