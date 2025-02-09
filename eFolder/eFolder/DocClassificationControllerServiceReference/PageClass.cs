// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.DocClassificationControllerServiceReference.PageClass
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
  [DataContract(Name = "PageClass", Namespace = "http://schemas.datacontract.org/2004/07/DocumentClassificationService")]
  [Serializable]
  public class PageClass : IExtensibleDataObject, INotifyPropertyChanged
  {
    [NonSerialized]
    private ExtensionDataObject extensionDataField;
    [OptionalField]
    private string ClientIdField;
    [OptionalField]
    private string FileIDField;
    [OptionalField]
    private string PageGUIDField;
    [OptionalField]
    private string StatusField;
    [OptionalField]
    private string StatusByField;
    [OptionalField]
    private string StatusDateField;
    [OptionalField]
    private string SuggestedByField;
    [OptionalField]
    private string SuggestedDateField;

    [Browsable(false)]
    public ExtensionDataObject ExtensionData
    {
      get => this.extensionDataField;
      set => this.extensionDataField = value;
    }

    [DataMember]
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

    [DataMember]
    public string FileID
    {
      get => this.FileIDField;
      set
      {
        if ((object) this.FileIDField == (object) value)
          return;
        this.FileIDField = value;
        this.RaisePropertyChanged(nameof (FileID));
      }
    }

    [DataMember]
    public string PageGUID
    {
      get => this.PageGUIDField;
      set
      {
        if ((object) this.PageGUIDField == (object) value)
          return;
        this.PageGUIDField = value;
        this.RaisePropertyChanged(nameof (PageGUID));
      }
    }

    [DataMember]
    public string Status
    {
      get => this.StatusField;
      set
      {
        if ((object) this.StatusField == (object) value)
          return;
        this.StatusField = value;
        this.RaisePropertyChanged(nameof (Status));
      }
    }

    [DataMember]
    public string StatusBy
    {
      get => this.StatusByField;
      set
      {
        if ((object) this.StatusByField == (object) value)
          return;
        this.StatusByField = value;
        this.RaisePropertyChanged(nameof (StatusBy));
      }
    }

    [DataMember]
    public string StatusDate
    {
      get => this.StatusDateField;
      set
      {
        if ((object) this.StatusDateField == (object) value)
          return;
        this.StatusDateField = value;
        this.RaisePropertyChanged(nameof (StatusDate));
      }
    }

    [DataMember]
    public string SuggestedBy
    {
      get => this.SuggestedByField;
      set
      {
        if ((object) this.SuggestedByField == (object) value)
          return;
        this.SuggestedByField = value;
        this.RaisePropertyChanged(nameof (SuggestedBy));
      }
    }

    [DataMember]
    public string SuggestedDate
    {
      get => this.SuggestedDateField;
      set
      {
        if ((object) this.SuggestedDateField == (object) value)
          return;
        this.SuggestedDateField = value;
        this.RaisePropertyChanged(nameof (SuggestedDate));
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
