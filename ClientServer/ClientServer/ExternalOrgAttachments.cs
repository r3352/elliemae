// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ExternalOrgAttachments
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class ExternalOrgAttachments
  {
    private Guid guid;
    private int externalOrgID;
    private string fileName;
    private DateTime dateAdded;
    private string description;
    private int category;
    private DateTime fileDate;
    private string userWhoAdded;
    private DateTime expirationDate;
    private int daysToExpire;
    private string realFileName;

    public ExternalOrgAttachments(
      Guid guid,
      int externalOrgID,
      string fileName,
      string description,
      DateTime dateAdded,
      int category,
      DateTime fileDate,
      string userWhoAdded,
      DateTime expirationDate,
      int daysToExpire,
      string realFileName)
    {
      this.guid = guid;
      this.externalOrgID = externalOrgID;
      this.fileName = fileName;
      this.description = description;
      this.dateAdded = dateAdded;
      this.category = category;
      this.fileDate = fileDate;
      this.userWhoAdded = userWhoAdded;
      this.expirationDate = expirationDate;
      this.daysToExpire = daysToExpire;
      this.realFileName = realFileName;
    }

    public Guid Guid
    {
      get => this.guid;
      set => this.guid = value;
    }

    public int ExternalOrgID
    {
      get => this.externalOrgID;
      set => this.externalOrgID = value;
    }

    public string FileName
    {
      get => this.fileName;
      set => this.fileName = value;
    }

    public string Description
    {
      get => this.description;
      set => this.description = value;
    }

    public int Category
    {
      get => this.category;
      set => this.category = value;
    }

    public DateTime FileDate
    {
      get => this.fileDate;
      set => this.fileDate = value;
    }

    public string UserWhoAdded
    {
      get => this.userWhoAdded;
      set => this.userWhoAdded = value;
    }

    public DateTime ExpirationDate
    {
      get => this.expirationDate;
      set => this.expirationDate = value;
    }

    public int DaysToExpire
    {
      get => this.daysToExpire;
      set => this.daysToExpire = value;
    }

    public string RealFileName
    {
      get => this.realFileName;
      set => this.realFileName = value;
    }

    public DateTime DateAdded
    {
      get => this.dateAdded;
      set => this.dateAdded = value;
    }
  }
}
