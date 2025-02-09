// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.LogAlert
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Xml;
using System;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class LogAlert
  {
    internal static readonly string XmlType = "Alert";
    private string userId = string.Empty;
    private int roleId;
    private string systemId = string.Empty;
    private DateTime dueDate = DateTime.MinValue;
    private DateTime followedUpDate = DateTime.MinValue;
    private bool isNew;
    private bool isDirty;
    private bool isFollowedUp;

    public string UserId => this.userId;

    public int RoleId
    {
      get => this.roleId;
      set
      {
        if (this.roleId == value)
          return;
        this.roleId = value;
        this.isDirty = true;
        if (this.roleId != 0)
          return;
        this.dueDate = DateTime.MinValue;
        this.followedUpDate = DateTime.MinValue;
      }
    }

    public string EntityId { get; set; }

    public string SystemID
    {
      get => this.systemId;
      set => this.systemId = value;
    }

    public DateTime DueDate
    {
      get => this.dueDate;
      set
      {
        this.dueDate = value;
        this.isDirty = true;
      }
    }

    public DateTime FollowedUpDate
    {
      get => this.followedUpDate;
      set
      {
        this.followedUpDate = value;
        this.isDirty = true;
      }
    }

    public bool IsNew
    {
      get => this.isNew;
      set => this.isNew = value;
    }

    public bool IsDirty
    {
      get => this.isDirty;
      set => this.isDirty = false;
    }

    public bool IsFollowedUp
    {
      get => this.isFollowedUp;
      set => this.isFollowedUp = value;
    }

    public object Clone()
    {
      return (object) new LogAlert(this.userId, this.roleId, this.dueDate, this.followedUpDate)
      {
        systemId = this.systemId,
        IsNew = this.isNew,
        IsDirty = this.isDirty
      };
    }

    public DateTime GetSortDate()
    {
      return DateTime.MinValue == this.FollowedUpDate ? this.DueDate : this.FollowedUpDate;
    }

    internal void SetSystemID(string systemId) => this.systemId = systemId;

    public LogAlert(string userId)
    {
      this.userId = userId;
      this.EntityId = Guid.NewGuid().ToString();
      this.isNew = true;
      this.isDirty = true;
    }

    public LogAlert(XmlElement e)
    {
      AttributeReader attributeReader = new AttributeReader(e);
      this.EntityId = attributeReader.GetString("_eid", Guid.NewGuid().ToString());
      this.userId = attributeReader.GetString(nameof (UserId));
      this.roleId = attributeReader.GetInteger(nameof (RoleId));
      this.systemId = attributeReader.GetString("SysID");
      this.dueDate = attributeReader.GetDate(nameof (DueDate));
      this.followedUpDate = attributeReader.GetDate(nameof (FollowedUpDate));
      this.isDirty = false;
      this.isNew = false;
      this.isFollowedUp = !(DateTime.MinValue == this.followedUpDate);
    }

    public LogAlert(string userId, int roleId, DateTime dueDate, DateTime followedUpDate)
    {
      this.userId = userId;
      this.RoleId = roleId;
      this.dueDate = dueDate;
      this.followedUpDate = followedUpDate;
      this.isNew = false;
      this.isDirty = false;
      this.isFollowedUp = !(DateTime.MinValue == followedUpDate);
    }

    public void ToXml(XmlElement parentElement)
    {
      AttributeWriter attributeWriter = new AttributeWriter((XmlElement) parentElement.AppendChild((XmlNode) parentElement.OwnerDocument.CreateElement(LogAlert.XmlType)));
      attributeWriter.Write("_eid", (object) this.EntityId);
      attributeWriter.Write("UserId", (object) this.userId);
      attributeWriter.Write("RoleId", (object) this.roleId);
      attributeWriter.Write("SysID", (object) this.systemId);
      attributeWriter.Write("DueDate", (object) this.dueDate);
      attributeWriter.Write("FollowedUpDate", (object) this.followedUpDate);
    }
  }
}
