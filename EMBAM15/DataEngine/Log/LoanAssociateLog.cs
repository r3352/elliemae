// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.LoanAssociateLog
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Xml;
using System;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public abstract class LoanAssociateLog : LogRecordBase
  {
    private string encompassSystemID = string.Empty;
    private LoanAssociateType loanAssociateType;
    private string loanAssociateID = string.Empty;
    private string loanAssociateName = string.Empty;
    private string loanAssociateTitle = string.Empty;
    private string loanAssociatePhone = string.Empty;
    private string loanAssociateEmail = string.Empty;
    private string loanAssociateFax = string.Empty;
    private string loanAssociateCellPhone = string.Empty;
    private bool loanAssociateAccess;
    private int roleId = -1;
    private string roleName = string.Empty;
    private string aPIClientID = string.Empty;

    protected LoanAssociateLog()
    {
    }

    protected LoanAssociateLog(LogList log)
      : base(log)
    {
    }

    protected LoanAssociateLog(LogList log, XmlElement e)
      : base(log, e)
    {
      AttributeReader attributeReader = new AttributeReader(e);
      this.encompassSystemID = attributeReader.GetString("SysID");
      this.loanAssociateID = attributeReader.GetString(nameof (LoanAssociateID));
      this.loanAssociateName = attributeReader.GetString(nameof (LoanAssociateName));
      this.loanAssociateTitle = attributeReader.GetString(nameof (LoanAssociateTitle));
      this.loanAssociatePhone = attributeReader.GetString(nameof (LoanAssociatePhone));
      this.loanAssociateCellPhone = attributeReader.GetString(nameof (LoanAssociateCellPhone));
      this.loanAssociateFax = attributeReader.GetString(nameof (LoanAssociateFax));
      this.loanAssociateEmail = attributeReader.GetString(nameof (LoanAssociateEmail));
      this.roleId = attributeReader.GetInteger(nameof (RoleID), -1);
      this.roleName = attributeReader.GetString(nameof (RoleName));
      this.loanAssociateAccess = attributeReader.GetBoolean("WriteAccess");
      this.aPIClientID = attributeReader.GetString(nameof (APIClientID));
      string str = attributeReader.GetString(nameof (LoanAssociateType), "");
      if (str != "")
        this.loanAssociateType = (LoanAssociateType) Enum.Parse(typeof (LoanAssociateType), str, true);
      else if (this.loanAssociateID != "")
        this.loanAssociateType = LoanAssociateType.User;
      this.MarkAsClean();
    }

    public LoanAssociateType LoanAssociateType => this.loanAssociateType;

    public string LoanAssociateID => this.loanAssociateID;

    public string LoanAssociateName => this.loanAssociateName;

    public string LoanAssociateTitle => this.loanAssociateTitle;

    public string LoanAssociatePhone
    {
      get => this.loanAssociatePhone;
      set
      {
        if (!(this.loanAssociatePhone != value))
          return;
        this.loanAssociatePhone = value;
        this.MarkAsDirty();
      }
    }

    public string LoanAssociateCellPhone
    {
      get => this.loanAssociateCellPhone;
      set
      {
        if (!(this.loanAssociateCellPhone != value))
          return;
        this.loanAssociateCellPhone = value;
        this.MarkAsDirty();
      }
    }

    public string LoanAssociateFax
    {
      get => this.loanAssociateFax;
      set
      {
        if (!(this.loanAssociateFax != value))
          return;
        this.loanAssociateFax = value;
        this.MarkAsDirty();
      }
    }

    public string LoanAssociateEmail
    {
      get => this.loanAssociateEmail;
      set
      {
        if (!(this.loanAssociateEmail != value))
          return;
        this.loanAssociateEmail = value;
        this.MarkAsDirty();
      }
    }

    public int RoleID
    {
      get => this.roleId;
      set
      {
        if (value == RoleInfo.FileStarter.ID && this.roleId != RoleInfo.FileStarter.ID)
        {
          if (this.roleId == -1)
            return;
          this.roleId = -1;
          this.MarkAsDirty();
        }
        else
        {
          if (this.roleId == value)
            return;
          this.roleId = value;
          this.MarkAsDirty();
        }
      }
    }

    public string RoleName
    {
      get => this.roleName;
      set
      {
        if (!(this.roleName != value))
          return;
        this.roleName = value;
        this.MarkAsDirty();
      }
    }

    public bool LoanAssociateAccess
    {
      get => this.loanAssociateType != LoanAssociateType.None && this.loanAssociateAccess;
      set
      {
        if (this.loanAssociateAccess == value)
          return;
        this.loanAssociateAccess = value;
        this.MarkAsDirty();
      }
    }

    public string APIClientID
    {
      get => this.aPIClientID;
      set
      {
        if (!(this.aPIClientID != value))
          return;
        this.aPIClientID = value;
        this.MarkAsDirty();
      }
    }

    public void SetLoanAssociate(
      string userId,
      string fullName,
      string email,
      string phone,
      string cellPhone,
      string fax,
      string title)
    {
      if (this.loanAssociateType != LoanAssociateType.User)
      {
        this.loanAssociateType = LoanAssociateType.User;
        this.MarkAsDirty();
      }
      if (this.loanAssociateID != userId)
      {
        this.loanAssociateID = userId;
        this.MarkAsDirty();
      }
      if (this.loanAssociateName != fullName)
      {
        this.loanAssociateName = fullName;
        this.MarkAsDirty();
      }
      if (this.loanAssociateTitle != title)
      {
        this.loanAssociateTitle = title;
        this.MarkAsDirty();
      }
      if (this.loanAssociateEmail != email)
      {
        this.loanAssociateEmail = email;
        this.MarkAsDirty();
      }
      if (this.loanAssociatePhone != phone)
      {
        this.loanAssociatePhone = phone;
        this.MarkAsDirty();
      }
      if (this.loanAssociateCellPhone != cellPhone)
      {
        this.loanAssociateCellPhone = cellPhone;
        this.MarkAsDirty();
      }
      if (!(this.loanAssociateFax != fax))
        return;
      this.loanAssociateFax = fax;
      this.MarkAsDirty();
    }

    public void SetLoanAssociate(UserInfo user)
    {
      if (this.loanAssociateType != LoanAssociateType.User)
      {
        this.loanAssociateType = LoanAssociateType.User;
        this.MarkAsDirty();
      }
      if (this.loanAssociateID != user.Userid)
      {
        this.loanAssociateID = user.Userid;
        this.MarkAsDirty();
      }
      if (this.loanAssociateName != user.FullName)
      {
        this.loanAssociateName = user.FullName;
        this.MarkAsDirty();
      }
      if (this.loanAssociateTitle != user.JobTitle)
      {
        this.loanAssociateTitle = user.JobTitle;
        this.MarkAsDirty();
      }
      if (this.loanAssociateEmail != user.Email)
      {
        this.loanAssociateEmail = user.Email;
        this.MarkAsDirty();
      }
      if (this.loanAssociatePhone != user.Phone)
      {
        this.loanAssociatePhone = user.Phone;
        this.MarkAsDirty();
      }
      if (this.loanAssociateCellPhone != user.CellPhone)
      {
        this.loanAssociateCellPhone = user.CellPhone;
        this.MarkAsDirty();
      }
      if (this.loanAssociateFax != user.Fax)
      {
        this.loanAssociateFax = user.Fax;
        this.MarkAsDirty();
      }
      if (!(this.aPIClientID != user.OAuthClientId))
        return;
      this.aPIClientID = user.OAuthClientId;
      this.MarkAsDirty();
    }

    public void SetLoanAssociate(AclGroup group)
    {
      if (this.loanAssociateType != LoanAssociateType.Group)
      {
        this.loanAssociateType = LoanAssociateType.Group;
        this.MarkAsDirty();
      }
      if (this.loanAssociateID != string.Concat((object) group.ID))
      {
        this.loanAssociateID = string.Concat((object) group.ID);
        this.MarkAsDirty();
      }
      if (this.loanAssociateName != group.Name)
      {
        this.loanAssociateName = group.Name;
        this.MarkAsDirty();
      }
      if (this.loanAssociateEmail != "")
      {
        this.loanAssociateEmail = "";
        this.MarkAsDirty();
      }
      if (this.loanAssociatePhone != "")
      {
        this.loanAssociatePhone = "";
        this.MarkAsDirty();
      }
      if (this.loanAssociateCellPhone != "")
      {
        this.loanAssociateCellPhone = "";
        this.MarkAsDirty();
      }
      if (!(this.LoanAssociateFax != ""))
        return;
      this.LoanAssociateFax = "";
      this.MarkAsDirty();
    }

    public void ClearLoanAssociate()
    {
      this.loanAssociateType = LoanAssociateType.None;
      this.loanAssociateID = "";
      this.loanAssociateName = "";
      this.loanAssociateTitle = "";
      this.loanAssociateEmail = "";
      this.loanAssociatePhone = "";
      this.loanAssociateCellPhone = "";
      this.loanAssociateFax = "";
    }

    internal override bool IsSystemSpecific() => true;

    protected void SetRoleInformation(int roleId, string roleName)
    {
      this.roleId = roleId;
      this.roleName = roleName;
    }

    public override void ToXml(XmlElement e)
    {
      base.ToXml(e);
      AttributeWriter attributeWriter = new AttributeWriter(e);
      if ((this.loanAssociateID ?? "") != "")
      {
        attributeWriter.Write("LoanAssociateType", (object) this.loanAssociateType.ToString());
        attributeWriter.Write("LoanAssociateID", (object) this.loanAssociateID);
        attributeWriter.Write("LoanAssociateName", (object) this.loanAssociateName);
        attributeWriter.Write("LoanAssociateTitle", (object) this.loanAssociateTitle);
        attributeWriter.Write("LoanAssociatePhone", (object) this.loanAssociatePhone);
        attributeWriter.Write("LoanAssociateCellPhone", (object) this.loanAssociateCellPhone);
        attributeWriter.Write("LoanAssociateFax", (object) this.loanAssociateFax);
        attributeWriter.Write("LoanAssociateEmail", (object) this.loanAssociateEmail);
        attributeWriter.Write("APIClientID", (object) this.aPIClientID);
      }
      if (this.RoleID < RoleInfo.FileStarter.ID)
        return;
      attributeWriter.Write("RoleID", (object) this.roleId.ToString());
      attributeWriter.Write("RoleName", (object) this.roleName);
      attributeWriter.Write("WriteAccess", (object) this.loanAssociateAccess);
    }

    public string GetLoanAssociateDisplayName()
    {
      if (this.loanAssociateType == LoanAssociateType.User)
        return this.loanAssociateName + " (" + this.loanAssociateID + ")";
      return this.loanAssociateType == LoanAssociateType.Group ? this.loanAssociateName + " (Group)" : "";
    }

    public PipelineInfo.LoanAssociateInfo ToLoanAssociateInfo()
    {
      return this.ToLoanAssociateInfo((string) null, 0);
    }

    public PipelineInfo.LoanAssociateInfo ToLoanAssociateInfo(string milestoneId, int order)
    {
      if (this.loanAssociateType == LoanAssociateType.User)
        return new PipelineInfo.LoanAssociateInfo(this.Guid, this.loanAssociateID, this.loanAssociateName, this.loanAssociateEmail, this.loanAssociatePhone, this.loanAssociateFax, this.loanAssociateTitle, this.roleId, milestoneId, this.loanAssociateAccess, order, this.aPIClientID);
      return this.loanAssociateType == LoanAssociateType.Group ? new PipelineInfo.LoanAssociateInfo(this.Guid, Utils.ParseInt((object) this.loanAssociateID), this.loanAssociateName, this.roleId, milestoneId, this.loanAssociateAccess, order) : (PipelineInfo.LoanAssociateInfo) null;
    }
  }
}
