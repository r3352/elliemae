// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalDocumentsSettings
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
using EllieMae.Encompass.Collections;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.ExternalOrganization
{
  /// <summary>Represent ExternalDocumentsSettings Class</summary>
  public class ExternalDocumentsSettings : IExternalDocumentsSettings
  {
    /// <summary>Private Properties</summary>
    private Guid guid;
    private int externalOrgId;
    private string addedBy;
    private bool active;
    private bool defaultActive;
    private string fileName;
    private string displayName;
    private int category;
    private ExternalOrganizationEntityType channel;
    private DateTime dateAdded;
    private DateTime startDate;
    private DateTime endDate;
    private bool availbleAllTPO;
    private bool isArchive;
    private bool isDefault;
    private int sortId;
    private ExternalOrgOriginatorStatus status;
    private ExternalOrgOriginatorStatus defaultStatus;
    private int assignCount;
    private string fileSize;

    /// <summary>Public Constructor</summary>
    public ExternalDocumentsSettings() => this.guid = Guid.NewGuid();

    /// <summary>Guid of the document</summary>
    public Guid Guid => this.guid;

    /// <summary>External Org ID</summary>
    public int ExternalOrgId
    {
      get => this.externalOrgId;
      set => this.externalOrgId = value;
    }

    /// <summary>AddedBy UserId for the document</summary>
    public string AddedBy
    {
      get => this.addedBy;
      set => this.addedBy = value;
    }

    /// <summary>Document Active Flag</summary>
    public bool Active
    {
      get => this.active;
      set => this.active = value;
    }

    /// <summary>Default Active Flag</summary>
    public bool DefaultActive
    {
      get => this.defaultActive;
      set => this.defaultActive = value;
    }

    /// <summary>File Name</summary>
    public string FileName
    {
      get => this.fileName;
      set => this.fileName = value;
    }

    /// <summary>Display Name</summary>
    public string DisplayName
    {
      get => this.displayName;
      set => this.displayName = value;
    }

    /// <summary>Category</summary>
    public int Category
    {
      get => this.category;
      set => this.category = value;
    }

    /// <summary>Channel - Enum Type</summary>
    public ExternalOrganizationEntityType Channel
    {
      get => this.channel;
      set => this.channel = value;
    }

    /// <summary>Date Added</summary>
    public DateTime DateAdded
    {
      get => this.dateAdded;
      set => this.dateAdded = value;
    }

    /// <summary>Start Date</summary>
    public DateTime StartDate
    {
      get => this.startDate;
      set => this.startDate = value;
    }

    /// <summary>End Date</summary>
    public DateTime EndDate
    {
      get => this.endDate;
      set => this.endDate = value;
    }

    /// <summary>Available to All TPO</summary>
    public bool AvailbleAllTPO
    {
      get => this.availbleAllTPO;
      set => this.availbleAllTPO = value;
    }

    /// <summary>IsArchive Flag</summary>
    public bool IsArchive
    {
      get => this.isArchive;
      set => this.isArchive = value;
    }

    /// <summary>IsDefault Flag</summary>
    public bool IsDefault
    {
      get => this.isDefault;
      set => this.isDefault = value;
    }

    /// <summary>Status Enum Type</summary>
    public ExternalOrgOriginatorStatus Status
    {
      get => this.status;
      set => this.status = value;
    }

    /// <summary>Default Status Enum Type</summary>
    public ExternalOrgOriginatorStatus DefaultStatus
    {
      get => this.defaultStatus;
      set => this.defaultStatus = value;
    }

    /// <summary>Assign Count</summary>
    public int AssignCount
    {
      get => this.assignCount;
      set => this.assignCount = value;
    }

    /// <summary>File Size</summary>
    public string FileSize
    {
      get => this.fileSize;
      set => this.fileSize = value;
    }

    /// <summary>Sort ID</summary>
    public int SortId
    {
      get => this.sortId;
      set => this.sortId = value;
    }

    /// <summary>Channel String</summary>
    public string ChannelStr
    {
      get
      {
        string channelStr = "All";
        if (this.Channel == ExternalOrganizationEntityType.Broker)
          channelStr = "Broker";
        else if (this.Channel == ExternalOrganizationEntityType.Correspondent)
          channelStr = "Correspondent";
        if (this.Channel == ExternalOrganizationEntityType.Both)
          channelStr = "All";
        return channelStr;
      }
    }

    /// <summary>Status String</summary>
    public string StatusStr
    {
      get
      {
        string statusStr = "";
        if (this.status == ExternalOrgOriginatorStatus.Active)
          statusStr = "Active";
        else if (this.status == ExternalOrgOriginatorStatus.Expired)
          statusStr = "Expired";
        else if (this.status == ExternalOrgOriginatorStatus.NotActive)
          statusStr = "Not Active";
        else if (this.status == ExternalOrgOriginatorStatus.Pending)
          statusStr = "Pending";
        return statusStr;
      }
    }

    /// <summary>Create Document List</summary>
    /// <param name="documents"></param>
    /// <returns></returns>
    internal static ExternalDocumentList ToList(List<DocumentSettingInfo> documents)
    {
      ExternalDocumentList list = new ExternalDocumentList();
      for (int index = 0; index < documents.Count; ++index)
        list.Add(new ExternalDocumentsSettings(documents[index]));
      return list;
    }

    /// <summary>Create Document Dictionary</summary>
    /// <param name="documents"></param>
    /// <returns></returns>
    internal static Dictionary<int, ExternalDocumentList> ToDictionary(
      Dictionary<int, List<DocumentSettingInfo>> documents)
    {
      Dictionary<int, ExternalDocumentList> dictionary = new Dictionary<int, ExternalDocumentList>();
      foreach (KeyValuePair<int, List<DocumentSettingInfo>> document in documents)
        dictionary.Add(document.Key, ExternalDocumentsSettings.ToList(document.Value));
      return dictionary;
    }

    /// <summary>Constructor</summary>
    /// <param name="docSetting"></param>
    internal ExternalDocumentsSettings(DocumentSettingInfo docSetting)
    {
      this.active = docSetting.Active;
      this.defaultActive = docSetting.DefaultActive;
      this.isDefault = docSetting.IsDefault;
      this.externalOrgId = docSetting.ExternalOrgId;
      this.addedBy = docSetting.AddedBy;
      this.availbleAllTPO = docSetting.AvailbleAllTPO;
      this.category = docSetting.Category;
      this.channel = (ExternalOrganizationEntityType) Convert.ToInt32((object) docSetting.Channel);
      this.dateAdded = docSetting.DateAdded;
      this.displayName = docSetting.DisplayName;
      this.endDate = docSetting.EndDate;
      this.fileName = docSetting.FileName;
      this.guid = docSetting.Guid;
      this.isArchive = docSetting.IsArchive;
      this.startDate = docSetting.StartDate;
      this.status = (ExternalOrgOriginatorStatus) Convert.ToInt32((object) docSetting.Status);
      this.defaultStatus = (ExternalOrgOriginatorStatus) Convert.ToInt32((object) docSetting.DefaultStatus);
      this.fileSize = docSetting.FileSize;
    }

    /// <summary>
    /// Transform ExternalDocumentSettings Object to DocumentSettingInfo Object
    /// </summary>
    /// <param name="docSetting"></param>
    /// <returns></returns>
    internal static DocumentSettingInfo ToDocumentSettingInfoObj(
      ExternalDocumentsSettings docSetting)
    {
      return new DocumentSettingInfo()
      {
        Active = docSetting.Active,
        ExternalOrgId = docSetting.ExternalOrgId,
        AddedBy = docSetting.AddedBy,
        AvailbleAllTPO = docSetting.AvailbleAllTPO,
        Category = docSetting.Category,
        Channel = (ExternalOriginatorEntityType) Convert.ToInt32((object) docSetting.Channel),
        DateAdded = docSetting.DateAdded,
        DisplayName = docSetting.DisplayName,
        EndDate = docSetting.EndDate,
        FileName = docSetting.FileName,
        Guid = docSetting.Guid,
        IsArchive = docSetting.IsArchive,
        StartDate = docSetting.StartDate,
        SortId = docSetting.sortId,
        FileSize = docSetting.FileSize
      };
    }
  }
}
