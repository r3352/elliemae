// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.UserPipelineView
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using System;
using System.Text;
using System.Windows.Forms;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class UserPipelineView : PipelineViewBase, IXmlSerializable
  {
    public const string DefaultViewName = "Default View�";
    private int sortIndex = -1;
    private PipelineViewLoanOrgType orgType;
    private string externalOrgId;
    private UserPipelineViewColumnCollection columns = new UserPipelineViewColumnCollection();

    public UserPipelineView(XmlSerializationInfo info)
    {
      this.name = info.GetString("name");
      this.loanFolders = info.GetString("loanFolder", string.Empty);
      this.filter = (FieldFilterList) info.GetValue("filter", typeof (FieldFilterList), (object) null);
      this.SetUserPipelineViewColumn((XmlElement) info.GetValue("layout", typeof (XmlElement), (object) null, true));
      this.ownership = info.GetEnum<PipelineViewLoanOwnership>("ownership", PipelineViewLoanOwnership.None);
      this.orgType = info.GetEnum<PipelineViewLoanOrgType>(nameof (orgType), PipelineViewLoanOrgType.Internal);
      this.externalOrgId = info.GetString(nameof (externalOrgId), string.Empty);
    }

    private string returnColumnValue(XmlNode xn, string attName)
    {
      if (xn.SelectSingleNode("element[@name='" + attName + "']") == null)
        return string.Empty;
      return xn.SelectSingleNode("element[@name='" + attName + "']")?.InnerText;
    }

    private void SetUserPipelineViewColumn(XmlElement xElement)
    {
      if (xElement == null)
        return;
      UserPipelineViewColumnCollection columnCollection = new UserPipelineViewColumnCollection();
      foreach (XmlNode selectNode in xElement.SelectNodes("element[@name='columns']/element"))
      {
        UserPipelineViewColumn newColumn = new UserPipelineViewColumn();
        newColumn.ColumnDBName = this.returnColumnValue(selectNode, "id");
        string s1 = this.returnColumnValue(selectNode, "width");
        if (!string.IsNullOrEmpty(s1))
          newColumn.width = int.Parse(s1);
        string s2 = this.returnColumnValue(selectNode, "displayOrder");
        if (!string.IsNullOrEmpty(s2))
          newColumn.orderIndex = int.Parse(s2);
        newColumn.sortOrder = (SortOrder) Enum.Parse(typeof (SortOrder), this.returnColumnValue(selectNode, "sortOrder"), true);
        string s3 = this.returnColumnValue(selectNode, "sortPriority");
        if (!string.IsNullOrEmpty(s3))
          newColumn.SortPriority = int.Parse(s3);
        newColumn.Alignment = this.returnColumnValue(selectNode, "alignment");
        newColumn.IsRequired = this.returnColumnValue(selectNode, "required") == "1";
        columnCollection.Add(newColumn);
      }
      this.columns = columnCollection;
    }

    public void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("Name", (object) this.name);
    }

    public UserPipelineView(string userID, string name = "New View�", int viewID = -1)
    {
      this.viewID = viewID;
      this.ID = userID;
      this.name = name;
    }

    public UserPipelineView(
      int viewID,
      string userID,
      string name,
      string loanFolders,
      PipelineViewLoanOwnership ownership,
      PipelineViewLoanOrgType orgType,
      string externalOrgId,
      UserPipelineViewColumn[] columns,
      string filterXml)
    {
      this.viewID = viewID;
      this.ID = userID;
      this.name = name;
      this.orgType = orgType;
      this.externalOrgId = externalOrgId;
      this.loanFolders = loanFolders;
      this.ownership = ownership;
      this.columns.AddRange(columns);
      this.filter = FieldFilterList.Parse(filterXml);
    }

    public int ViewID
    {
      get => this.viewID;
      set => this.viewID = value;
    }

    public string UserID => this.ID;

    public PipelineViewLoanOrgType OrgType
    {
      get => this.orgType;
      set => this.orgType = value;
    }

    public string ExternalOrgId
    {
      get => this.externalOrgId;
      set => this.externalOrgId = value;
    }

    public string Name
    {
      get => this.name;
      set => this.name = value;
    }

    public string LoanFolders
    {
      get => this.loanFolders;
      set => this.loanFolders = value;
    }

    public PipelineViewLoanOwnership Ownership
    {
      get => this.ownership;
      set => this.ownership = value;
    }

    public FieldFilterList Filter
    {
      get => this.filter;
      set => this.filter = value;
    }

    public bool HasFilter => this.filter != null && !this.filter.IsEmpty;

    public UserPipelineViewColumnCollection Columns => this.columns;

    public UserPipelineView Duplicate(string copyToUserID)
    {
      UserPipelineView userPipelineView = new UserPipelineView(copyToUserID, "New View");
      userPipelineView.loanFolders = this.loanFolders;
      userPipelineView.filter = this.filter;
      userPipelineView.name = this.name;
      userPipelineView.ownership = this.ownership;
      userPipelineView.orgType = this.orgType;
      userPipelineView.externalOrgId = this.externalOrgId;
      foreach (UserPipelineViewColumn column in this.columns)
        userPipelineView.columns.Add(column.Clone());
      return userPipelineView;
    }

    public override string ToString() => this.name;

    public override bool Equals(object obj)
    {
      return obj is UserPipelineView userPipelineView && userPipelineView.ID == this.ID && string.Compare(userPipelineView.name, this.name, true) == 0;
    }

    public override int GetHashCode() => this.ID.GetHashCode() ^ this.name.ToLower().GetHashCode();

    public int CompareTo(UserPipelineView other)
    {
      if (other == null)
        return 1;
      if (this.ID != other.ID)
        return string.Compare(this.ID, other.ID, true);
      return this.sortIndex != other.sortIndex ? this.sortIndex - other.sortIndex : string.Compare(this.name, other.name, true);
    }

    public static explicit operator UserPipelineView(BinaryObject obj)
    {
      return (UserPipelineView) UserPipelineView.Parse(obj, typeof (UserPipelineView));
    }

    private static object Parse(BinaryObject o, System.Type objectType)
    {
      return o == null ? (object) null : (object) new XmlSerializer().Deserialize<UserPipelineView>(o.ToString(Encoding.Default));
    }
  }
}
