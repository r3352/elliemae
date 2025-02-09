// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Dashboard.DashboardTemplate
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Dashboard
{
  [Serializable]
  public class DashboardTemplate : BinaryConvertibleObject, ITemplateSetting
  {
    public const string IsNew = "IsNewTemplate�";
    public const string IsValid = "IsValidTemplate�";
    private string guid = string.Empty;
    private string templateName = string.Empty;
    private string description = string.Empty;
    private string versionNumber = "1.0";
    private bool isNewTemplate = true;
    private bool isValidTemplate;
    private bool isPredefinedTemplate;
    private DashboardDataSourceType dataSourceType = DashboardDataSourceType.None;
    private DashboardChartType chartType = DashboardChartType.None;
    private string drillDownTemplate = string.Empty;
    private int maxBars = 50;
    private DashboardSubsetType subsetType;
    private string timeFrameField = "Loan.DateFileOpened";
    private string xAxisField = string.Empty;
    private string yAxisField = "Dashboard.LoanCount";
    private ColumnSummaryType yAxisSummaryType = ColumnSummaryType.Count;
    private XmlList<string> folders = new XmlList<string>();
    private FieldFilterList filters = new FieldFilterList();
    private int maxLines = 10;
    private string groupByField = "Dashboard.NoGroupBy";
    private int maxRows = 1000;
    private XmlList<ColumnInfo> fields = new XmlList<ColumnInfo>();
    private int roleId = RoleInfo.All.ID;
    private int organizationId = -1;
    private bool includeChildren;
    private int userGroupId = AclGroup.NoUserGroupId;
    private string summaryField1 = "Dashboard.LoanCount";
    private string summaryField2 = "Dashboard.NoSummary";
    private string summaryField3 = "Dashboard.NoSummary";
    private ColumnSummaryType summaryType1 = ColumnSummaryType.Count;
    private ColumnSummaryType summaryType2;
    private ColumnSummaryType summaryType3;
    private bool includeMin = true;
    private bool includeMax = true;
    private bool includeAverage = true;
    private bool includeTotal = true;
    public static string DashboardAllFolder = nameof (DashboardAllFolder);
    public static string DashboardNoArchiveFolder = nameof (DashboardNoArchiveFolder);

    public string Guid
    {
      get => this.guid;
      set => this.guid = value;
    }

    public string TemplateName
    {
      get => this.templateName;
      set => this.templateName = value == null ? string.Empty : value.Trim();
    }

    public string Description
    {
      get => this.description;
      set => this.description = value == null ? string.Empty : value.Trim();
    }

    public string VersionNumber
    {
      get => this.versionNumber;
      set => this.versionNumber = value == null ? string.Empty : value.Trim();
    }

    public bool IsNewTemplate
    {
      get => this.isNewTemplate;
      set => this.isNewTemplate = value;
    }

    public bool IsValidTemplate
    {
      get => this.isValidTemplate;
      set => this.isValidTemplate = value;
    }

    public bool IsPredefinedTemplate
    {
      get => this.isPredefinedTemplate;
      set => this.isPredefinedTemplate = value;
    }

    public DashboardDataSourceType DataSourceType
    {
      get => this.dataSourceType;
      set => this.dataSourceType = value;
    }

    public DashboardChartType ChartType
    {
      get => this.chartType;
      set => this.chartType = value;
    }

    public string DrillDownTemplate
    {
      get => this.drillDownTemplate;
      set => this.drillDownTemplate = value == null ? string.Empty : value.Trim();
    }

    public int MaxBars
    {
      get => this.maxBars;
      set => this.maxBars = value;
    }

    public DashboardSubsetType SubsetType
    {
      get => this.subsetType;
      set => this.subsetType = value;
    }

    public string TimeFrameField
    {
      get => this.timeFrameField;
      set => this.timeFrameField = value;
    }

    public string XAxisField
    {
      get => this.xAxisField;
      set => this.xAxisField = value;
    }

    public string YAxisField
    {
      get => this.yAxisField;
      set => this.yAxisField = value;
    }

    public ColumnSummaryType YAxisSummaryType
    {
      get => this.yAxisSummaryType;
      set => this.yAxisSummaryType = value;
    }

    public bool SelectAllFolders
    {
      get => this.folders.Count == 1 && this.folders.Contains(DashboardTemplate.DashboardAllFolder);
    }

    public bool SelectAllWOArchiveFolders
    {
      get
      {
        return this.folders.Count == 1 && this.folders.Contains(DashboardTemplate.DashboardNoArchiveFolder);
      }
    }

    public List<string> Folders
    {
      get => (List<string>) this.folders;
      set
      {
        this.folders = value == null ? new XmlList<string>() : new XmlList<string>((IEnumerable<string>) value);
      }
    }

    public FieldFilterList Filters
    {
      get => this.filters;
      set => this.filters = value == null ? new FieldFilterList() : new FieldFilterList(value);
    }

    public int MaxLines
    {
      get => this.maxLines;
      set => this.maxLines = value;
    }

    public string GroupByField
    {
      get => this.groupByField;
      set => this.groupByField = value;
    }

    public int MaxRows
    {
      get => this.maxRows;
      set => this.maxRows = value;
    }

    public List<ColumnInfo> Fields
    {
      get => (List<ColumnInfo>) this.fields;
      set
      {
        this.fields = value == null ? new XmlList<ColumnInfo>() : new XmlList<ColumnInfo>((IEnumerable<ColumnInfo>) value);
      }
    }

    public int RoleId
    {
      get => this.roleId;
      set => this.roleId = value;
    }

    public int OrganizationId
    {
      get => this.organizationId;
      set => this.organizationId = value;
    }

    public bool IncludeChildren
    {
      get => this.includeChildren;
      set => this.includeChildren = value;
    }

    public int UserGroupId
    {
      get => this.userGroupId;
      set => this.userGroupId = value;
    }

    public string SummaryField1
    {
      get => this.summaryField1;
      set => this.summaryField1 = value;
    }

    public string SummaryField2
    {
      get => this.summaryField2;
      set => this.summaryField2 = value;
    }

    public string SummaryField3
    {
      get => this.summaryField3;
      set => this.summaryField3 = value;
    }

    public ColumnSummaryType SummaryType1
    {
      get => this.summaryType1;
      set => this.summaryType1 = value;
    }

    public ColumnSummaryType SummaryType2
    {
      get => this.summaryType2;
      set => this.summaryType2 = value;
    }

    public ColumnSummaryType SummaryType3
    {
      get => this.summaryType3;
      set => this.summaryType3 = value;
    }

    public bool IncludeMin
    {
      get => this.includeMin;
      set => this.includeMin = value;
    }

    public bool IncludeMax
    {
      get => this.includeMax;
      set => this.includeMax = value;
    }

    public bool IncludeAverage
    {
      get => this.includeAverage;
      set => this.includeAverage = value;
    }

    public bool IncludeTotal
    {
      get => this.includeTotal;
      set => this.includeTotal = value;
    }

    public static explicit operator DashboardTemplate(BinaryObject binaryObject)
    {
      return (DashboardTemplate) BinaryConvertibleObject.Parse(binaryObject, typeof (DashboardTemplate));
    }

    public DashboardTemplate(string templateName, string description)
    {
      this.guid = System.Guid.NewGuid().ToString();
      this.templateName = templateName;
      this.description = description;
    }

    public void ResetDefaults()
    {
      this.maxBars = 50;
      this.subsetType = DashboardSubsetType.Top;
      this.timeFrameField = "Loan.DateFileOpened";
      this.xAxisField = string.Empty;
      this.yAxisField = "Dashboard.LoanCount";
      this.yAxisSummaryType = ColumnSummaryType.Count;
      this.maxLines = 10;
      this.groupByField = "Dashboard.NoGroupBy";
      this.maxRows = 1000;
      this.fields = new XmlList<ColumnInfo>();
      this.roleId = RoleInfo.All.ID;
      this.organizationId = -1;
      this.includeChildren = false;
      this.userGroupId = AclGroup.NoUserGroupId;
      this.summaryField1 = "Dashboard.LoanCount";
      this.summaryField2 = "Dashboard.NoSummary";
      this.summaryField3 = "Dashboard.NoSummary";
      this.summaryType1 = ColumnSummaryType.Count;
      this.summaryType2 = ColumnSummaryType.None;
      this.summaryType3 = ColumnSummaryType.None;
      this.includeMin = true;
      this.includeMax = true;
      this.includeAverage = true;
      this.includeTotal = true;
    }

    public DashboardTemplate(XmlSerializationInfo info)
    {
      this.guid = info.GetString(nameof (guid));
      this.templateName = info.GetString(nameof (templateName));
      this.description = info.GetString(nameof (description));
      this.versionNumber = info.GetString(nameof (versionNumber));
      this.isNewTemplate = info.GetBoolean(nameof (isNewTemplate));
      this.isValidTemplate = info.GetBoolean(nameof (isValidTemplate));
      this.isPredefinedTemplate = info.GetBoolean(nameof (isPredefinedTemplate));
      this.dataSourceType = DashboardDataSourceType.None;
      try
      {
        this.dataSourceType = info.GetEnum<DashboardDataSourceType>(nameof (dataSourceType));
      }
      catch
      {
      }
      this.chartType = DashboardChartType.None;
      try
      {
        this.chartType = info.GetEnum<DashboardChartType>(nameof (chartType));
      }
      catch
      {
      }
      this.drillDownTemplate = info.GetString(nameof (drillDownTemplate));
      this.maxBars = info.GetInteger(nameof (maxBars));
      this.subsetType = DashboardSubsetType.None;
      try
      {
        this.subsetType = info.GetEnum<DashboardSubsetType>(nameof (subsetType));
      }
      catch
      {
      }
      this.timeFrameField = info.GetString(nameof (timeFrameField));
      this.xAxisField = info.GetString(nameof (xAxisField));
      this.yAxisField = info.GetString(nameof (yAxisField));
      this.yAxisSummaryType = ColumnSummaryType.None;
      try
      {
        this.yAxisSummaryType = info.GetEnum<ColumnSummaryType>(nameof (yAxisSummaryType));
      }
      catch
      {
      }
      this.folders = (XmlList<string>) info.GetValue(nameof (folders), typeof (XmlList<string>));
      this.filters = (FieldFilterList) info.GetValue(nameof (filters), typeof (FieldFilterList));
      this.maxLines = info.GetInteger(nameof (maxLines));
      this.groupByField = info.GetString(nameof (groupByField));
      this.maxRows = info.GetInteger(nameof (maxRows));
      this.fields = (XmlList<ColumnInfo>) info.GetValue(nameof (fields), typeof (XmlList<ColumnInfo>));
      this.roleId = info.GetInteger(nameof (roleId));
      this.organizationId = info.GetInteger(nameof (organizationId));
      this.includeChildren = info.GetBoolean(nameof (includeChildren));
      this.userGroupId = info.GetInteger(nameof (userGroupId));
      this.summaryField1 = info.GetString(nameof (summaryField1));
      this.summaryField2 = info.GetString(nameof (summaryField2));
      this.summaryField3 = info.GetString(nameof (summaryField3));
      this.summaryType1 = ColumnSummaryType.None;
      this.summaryType2 = ColumnSummaryType.None;
      this.summaryType3 = ColumnSummaryType.None;
      try
      {
        this.summaryType1 = info.GetEnum<ColumnSummaryType>(nameof (summaryType1));
        this.summaryType2 = info.GetEnum<ColumnSummaryType>(nameof (summaryType2));
        this.summaryType3 = info.GetEnum<ColumnSummaryType>(nameof (summaryType3));
      }
      catch
      {
      }
      try
      {
        this.includeMin = info.GetString(nameof (IncludeMin)) == "1";
        this.includeMax = info.GetString(nameof (IncludeMax)) == "1";
        this.includeAverage = info.GetString(nameof (IncludeAverage)) == "1";
        if (info.GetString(nameof (IncludeTotal)) == "1")
          this.includeTotal = true;
        else
          this.includeTotal = false;
      }
      catch (Exception ex)
      {
      }
    }

    public Hashtable GetProperties()
    {
      Hashtable insensitiveHashtable = CollectionsUtil.CreateCaseInsensitiveHashtable();
      insensitiveHashtable.Add((object) "Guid", (object) this.guid);
      insensitiveHashtable.Add((object) "Name", (object) this.templateName);
      insensitiveHashtable.Add((object) "Description", (object) this.description);
      insensitiveHashtable.Add((object) "IsNewTemplate", this.isNewTemplate ? (object) "true" : (object) "false");
      insensitiveHashtable.Add((object) "IsValidTemplate", this.IsValidTemplate ? (object) "true" : (object) "false");
      return insensitiveHashtable;
    }

    public ITemplateSetting Duplicate()
    {
      DashboardTemplate dashboardTemplate = (DashboardTemplate) this.Clone();
      dashboardTemplate.guid = System.Guid.NewGuid().ToString();
      dashboardTemplate.TemplateName = "";
      return (ITemplateSetting) dashboardTemplate;
    }

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("guid", (object) this.guid);
      info.AddValue("templateName", (object) this.templateName);
      info.AddValue("description", (object) this.description);
      info.AddValue("versionNumber", (object) this.versionNumber);
      info.AddValue("isNewTemplate", (object) this.isNewTemplate);
      info.AddValue("isValidTemplate", (object) this.isValidTemplate);
      info.AddValue("isPredefinedTemplate", (object) this.isPredefinedTemplate);
      info.AddValue("dataSourceType", (object) this.dataSourceType);
      info.AddValue("chartType", (object) this.chartType);
      info.AddValue("drillDownTemplate", (object) this.drillDownTemplate);
      info.AddValue("maxBars", (object) this.maxBars);
      info.AddValue("subsetType", (object) this.subsetType);
      info.AddValue("timeFrameField", (object) this.timeFrameField);
      info.AddValue("xAxisField", (object) this.xAxisField);
      info.AddValue("yAxisField", (object) this.yAxisField);
      info.AddValue("yAxisSummaryType", (object) this.yAxisSummaryType);
      info.AddValue("folders", (object) this.folders);
      info.AddValue("filters", (object) this.filters);
      info.AddValue("maxLines", (object) this.maxLines);
      info.AddValue("groupByField", (object) this.groupByField);
      info.AddValue("maxRows", (object) this.maxRows);
      info.AddValue("fields", (object) this.fields);
      info.AddValue("roleId", (object) this.roleId);
      info.AddValue("organizationId", (object) this.organizationId);
      info.AddValue("includeChildren", (object) this.includeChildren);
      info.AddValue("userGroupId", (object) this.userGroupId);
      info.AddValue("summaryField1", (object) this.summaryField1);
      info.AddValue("summaryField2", (object) this.summaryField2);
      info.AddValue("summaryField3", (object) this.summaryField3);
      info.AddValue("summaryType1", (object) this.summaryType1);
      info.AddValue("summaryType2", (object) this.summaryType2);
      info.AddValue("summaryType3", (object) this.summaryType3);
      if (this.includeMin)
        info.AddValue("IncludeMin", (object) "1");
      else
        info.AddValue("IncludeMin", (object) "0");
      if (this.includeMax)
        info.AddValue("IncludeMax", (object) "1");
      else
        info.AddValue("IncludeMax", (object) "0");
      if (this.includeAverage)
        info.AddValue("IncludeAverage", (object) "1");
      else
        info.AddValue("IncludeAverage", (object) "0");
      if (this.includeTotal)
        info.AddValue("IncludeTotal", (object) "1");
      else
        info.AddValue("IncludeTotal", (object) "0");
    }
  }
}
