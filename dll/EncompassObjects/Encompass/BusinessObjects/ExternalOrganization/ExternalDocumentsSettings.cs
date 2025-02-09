// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalDocumentsSettings
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
using EllieMae.Encompass.Collections;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.ExternalOrganization
{
  public class ExternalDocumentsSettings : IExternalDocumentsSettings
  {
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

    public ExternalDocumentsSettings() => this.guid = Guid.NewGuid();

    public Guid Guid => this.guid;

    public int ExternalOrgId
    {
      get => this.externalOrgId;
      set => this.externalOrgId = value;
    }

    public string AddedBy
    {
      get => this.addedBy;
      set => this.addedBy = value;
    }

    public bool Active
    {
      get => this.active;
      set => this.active = value;
    }

    public bool DefaultActive
    {
      get => this.defaultActive;
      set => this.defaultActive = value;
    }

    public string FileName
    {
      get => this.fileName;
      set => this.fileName = value;
    }

    public string DisplayName
    {
      get => this.displayName;
      set => this.displayName = value;
    }

    public int Category
    {
      get => this.category;
      set => this.category = value;
    }

    public ExternalOrganizationEntityType Channel
    {
      get => this.channel;
      set => this.channel = value;
    }

    public DateTime DateAdded
    {
      get => this.dateAdded;
      set => this.dateAdded = value;
    }

    public DateTime StartDate
    {
      get => this.startDate;
      set => this.startDate = value;
    }

    public DateTime EndDate
    {
      get => this.endDate;
      set => this.endDate = value;
    }

    public bool AvailbleAllTPO
    {
      get => this.availbleAllTPO;
      set => this.availbleAllTPO = value;
    }

    public bool IsArchive
    {
      get => this.isArchive;
      set => this.isArchive = value;
    }

    public bool IsDefault
    {
      get => this.isDefault;
      set => this.isDefault = value;
    }

    public ExternalOrgOriginatorStatus Status
    {
      get => this.status;
      set => this.status = value;
    }

    public ExternalOrgOriginatorStatus DefaultStatus
    {
      get => this.defaultStatus;
      set => this.defaultStatus = value;
    }

    public int AssignCount
    {
      get => this.assignCount;
      set => this.assignCount = value;
    }

    public string FileSize
    {
      get => this.fileSize;
      set => this.fileSize = value;
    }

    public int SortId
    {
      get => this.sortId;
      set => this.sortId = value;
    }

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

    internal static ExternalDocumentList ToList(List<DocumentSettingInfo> documents)
    {
      ExternalDocumentList list = new ExternalDocumentList();
      for (int index = 0; index < documents.Count; ++index)
        list.Add(new ExternalDocumentsSettings(documents[index]));
      return list;
    }

    internal static Dictionary<int, ExternalDocumentList> ToDictionary(
      Dictionary<int, List<DocumentSettingInfo>> documents)
    {
      Dictionary<int, ExternalDocumentList> dictionary = new Dictionary<int, ExternalDocumentList>();
      foreach (KeyValuePair<int, List<DocumentSettingInfo>> document in documents)
        dictionary.Add(document.Key, ExternalDocumentsSettings.ToList(document.Value));
      return dictionary;
    }

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
        Channel = (ExternalOriginatorEntityType) (int) (byte) Convert.ToInt32((object) docSetting.Channel),
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
