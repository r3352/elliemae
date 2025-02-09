// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ExternalOriginatorManagement.DocumentSettingInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.ExternalOriginatorManagement
{
  [Serializable]
  public class DocumentSettingInfo
  {
    public Guid Guid { get; set; }

    public int ExternalOrgId { get; set; }

    public string AddedBy { get; set; }

    public bool Active { get; set; }

    public bool DefaultActive { get; set; }

    public string FileName { get; set; }

    public string DisplayName { get; set; }

    public int Category { get; set; }

    public ExternalOriginatorEntityType Channel { get; set; }

    public DateTime DateAdded { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public bool AvailbleAllTPO { get; set; }

    public bool IsArchive { get; set; }

    public bool IsDefault { get; set; }

    public int SortId { get; set; }

    public ExternalOriginatorStatus Status { get; set; }

    public ExternalOriginatorStatus DefaultStatus { get; set; }

    public int AssignCount { get; set; }

    public string FileSize { get; set; }

    public string VaultFileId { get; set; }

    public string ChannelStr
    {
      get
      {
        string channelStr = "All";
        if (this.Channel == ExternalOriginatorEntityType.Broker)
          channelStr = "Broker";
        else if (this.Channel == ExternalOriginatorEntityType.Correspondent)
          channelStr = "Correspondent";
        return channelStr;
      }
    }

    public string StatusStr
    {
      get
      {
        string statusStr = "";
        if (this.Status == ExternalOriginatorStatus.Active)
          statusStr = "Active";
        else if (this.Status == ExternalOriginatorStatus.Expired)
          statusStr = "Expired";
        else if (this.Status == ExternalOriginatorStatus.NotActive)
          statusStr = "Not Active";
        else if (this.Status == ExternalOriginatorStatus.Pending)
          statusStr = "Pending";
        return statusStr;
      }
    }

    public static int CompareByAssignCount(DocumentSettingInfo d1, DocumentSettingInfo d2)
    {
      return d1.AssignCount.CompareTo(d2.AssignCount);
    }
  }
}
