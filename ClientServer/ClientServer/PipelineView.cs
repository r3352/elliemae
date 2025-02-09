// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.PipelineView
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class PipelineView : 
    BinaryConvertible<PipelineView>,
    ITemplateSetting,
    IComparable<PipelineView>
  {
    private int viewID;
    private string name;
    private string loanFolder;
    private string[] loanFolders;
    private FieldFilterList filter;
    private TableLayout layout;
    private PipelineViewLoanOwnership ownership = PipelineViewLoanOwnership.All;
    private PipelineViewLoanOrgType orgType = PipelineViewLoanOrgType.Internal;
    private string externalOrgId;
    private bool includeArchiveFolders = true;
    private bool isUserView;
    private string personaName = string.Empty;

    public PipelineView()
    {
    }

    public PipelineView(string name) => this.name = name;

    public PipelineView(XmlSerializationInfo info)
    {
      this.name = info.GetString(nameof (name));
      this.loanFolder = info.GetString(nameof (loanFolder), (string) null);
      string[] strArray;
      if (this.loanFolder != null)
        strArray = this.loanFolder.Split(',');
      else
        strArray = (string[]) null;
      this.loanFolders = strArray;
      this.filter = (FieldFilterList) info.GetValue(nameof (filter), typeof (FieldFilterList));
      this.layout = (TableLayout) info.GetValue(nameof (layout), typeof (TableLayout));
      this.ownership = (PipelineViewLoanOwnership) info.GetValue(nameof (ownership), typeof (PipelineViewLoanOwnership), (object) PipelineViewLoanOwnership.All);
      this.orgType = (PipelineViewLoanOrgType) info.GetValue(nameof (orgType), typeof (PipelineViewLoanOrgType), (object) PipelineViewLoanOrgType.Internal);
      this.externalOrgId = info.GetString(nameof (externalOrgId), (string) null);
      this.includeArchiveFolders = info.GetBoolean(nameof (includeArchiveFolders), true);
      foreach (TableLayout.Column column in this.layout)
      {
        if (string.Compare(column.ColumnID, "Fields.CurrentTeamMember", true) == 0)
        {
          this.layout.AddColumn(column.Copy("CurrentLoanAssociate.FullName"));
          this.layout.Remove(column);
          break;
        }
      }
    }

    public string Name
    {
      get => this.name;
      set => this.name = value;
    }

    public string LoanFolder
    {
      get => this.loanFolder;
      set => this.loanFolder = value;
    }

    public string[] LoanFolders
    {
      get => this.loanFolders;
      set => this.loanFolders = value;
    }

    public FieldFilterList Filter
    {
      get => this.filter;
      set => this.filter = value;
    }

    public TableLayout Layout
    {
      get => this.layout;
      set => this.layout = value;
    }

    public PipelineViewLoanOwnership LoanOwnership
    {
      get => this.ownership;
      set => this.ownership = value;
    }

    public PipelineViewLoanOrgType LoanOrgType
    {
      get => this.orgType;
      set => this.orgType = value;
    }

    public string ExternalOrgId
    {
      get => this.externalOrgId;
      set => this.externalOrgId = value;
    }

    public string PersonaName
    {
      get => this.personaName;
      set => this.personaName = value;
    }

    public bool IsUserView
    {
      get => this.isUserView;
      set => this.isUserView = value;
    }

    public int ViewID
    {
      get => this.viewID;
      set => this.viewID = value;
    }

    public override string ToString() => this.Name;

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("name", (object) this.name);
      info.AddValue("loanFolder", (object) this.loanFolder);
      info.AddValue("filter", (object) this.filter);
      info.AddValue("layout", (object) this.layout);
      info.AddValue("ownership", (object) this.ownership);
      info.AddValue("orgType", (object) this.orgType);
      info.AddValue("externalOrgId", (object) this.externalOrgId);
    }

    string ITemplateSetting.TemplateName
    {
      get => !(this.personaName == string.Empty) ? this.personaName + " - " + this.name : this.name;
      set => this.name = value;
    }

    public string Description
    {
      get => "";
      set
      {
      }
    }

    public Hashtable GetProperties()
    {
      return new Hashtable((IEqualityComparer) StringComparer.CurrentCultureIgnoreCase);
    }

    public ITemplateSetting Duplicate()
    {
      ITemplateSetting templateSetting = (ITemplateSetting) this.Clone();
      templateSetting.TemplateName = "";
      return templateSetting;
    }

    public int CompareTo(PipelineView other)
    {
      return other != null ? string.Compare(this.Name, other.Name, true) : throw new Exception("Invalid value for comparison");
    }

    public static explicit operator PipelineView(BinaryObject binaryObject)
    {
      return BinaryConvertible<PipelineView>.Parse(binaryObject);
    }
  }
}
