// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.CRMLog
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Xml;
using System;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class CRMLog
  {
    private string mappingID = "";
    private CRMLogType mappingType;
    private string contactGuid = "";
    private bool isDirty;
    private LogList log;
    public static string XmlType = "CRM";
    private CRMRoleType roleType = CRMRoleType.NotFound;

    internal CRMLog(
      string mappingID,
      CRMLogType mappingType,
      string contactGuid,
      LogList log,
      CRMRoleType roleType)
    {
      this.mappingID = mappingID;
      this.mappingType = mappingType;
      this.contactGuid = contactGuid;
      this.roleType = roleType;
      this.log = log;
    }

    public CRMLog(LogList log, XmlElement e)
    {
      AttributeReader attributeReader = new AttributeReader(e);
      this.mappingID = attributeReader.GetString(nameof (MappingID));
      this.mappingType = (CRMLogType) int.Parse(attributeReader.GetString(nameof (MappingType)));
      this.contactGuid = attributeReader.GetString(nameof (ContactGuid));
      this.log = log;
      try
      {
        this.roleType = (CRMRoleType) int.Parse(attributeReader.GetString(nameof (RoleType)));
      }
      catch (Exception ex)
      {
        this.roleType = this.getRoleType();
      }
    }

    internal void AttachToLog(LogList log) => this.log = log;

    public string MappingID
    {
      get => this.mappingID;
      set
      {
        this.mappingID = value;
        this.MarkAsDirty();
      }
    }

    public string ContactGuid
    {
      get => this.contactGuid;
      set
      {
        this.contactGuid = value;
        this.MarkAsDirty();
      }
    }

    public CRMLogType MappingType
    {
      get => this.mappingType;
      set
      {
        this.mappingType = value;
        this.MarkAsDirty();
      }
    }

    public CRMRoleType RoleType
    {
      get => this.roleType;
      set
      {
        this.roleType = value;
        this.MarkAsDirty();
      }
    }

    public CRMRoleType getRoleType()
    {
      if (this.mappingType != CRMLogType.BorrowerContact)
        return (CRMRoleType) int.Parse(this.mappingID);
      foreach (BorrowerPair borrowerPair in this.log.Loan.GetBorrowerPairs())
      {
        if (borrowerPair.Borrower.Id == this.mappingID)
          return CRMRoleType.Borrower;
        if (borrowerPair.CoBorrower.Id == this.mappingID)
          return CRMRoleType.Coborrower;
      }
      return CRMRoleType.NotFound;
    }

    public int GetBorrowerPairIndex()
    {
      if (this.mappingType != CRMLogType.BorrowerContact)
        return -1;
      BorrowerPair[] borrowerPairs = this.log.Loan.GetBorrowerPairs();
      for (int borrowerPairIndex = 0; borrowerPairIndex < borrowerPairs.Length; ++borrowerPairIndex)
      {
        if (borrowerPairs[borrowerPairIndex].Borrower.Id == this.mappingID || borrowerPairs[borrowerPairIndex].CoBorrower.Id == this.mappingID)
          return borrowerPairIndex;
      }
      return -1;
    }

    public void MarkAsClean() => this.isDirty = false;

    public void MarkAsDirty() => this.isDirty = true;

    public bool IsDirty() => this.isDirty;

    public void ToXml(XmlElement e)
    {
      AttributeWriter attributeWriter = new AttributeWriter(e);
      attributeWriter.Write("MappingID", (object) this.MappingID);
      attributeWriter.Write("MappingType", (object) (int) this.MappingType);
      attributeWriter.Write("ContactGuid", (object) this.ContactGuid);
      attributeWriter.Write("RoleType", (object) (int) this.RoleType);
    }
  }
}
