// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Reporting.ReportSettings
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.XPath;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Reporting
{
  [Serializable]
  public class ReportSettings
  {
    public static readonly int NoValue = -1;
    private double verNo = 9.0;
    private string reportTitle = "";
    private ReportsFor reportFor;
    private string reportType = "Table";
    private bool archive;
    private string fileStage = "";
    private string timeFrame = "";
    private DateTime timeFrom = DateTime.MinValue;
    private DateTime timeTo = DateTime.MinValue;
    private string paperSize = "Letter";
    private string paperOri = "Portrait";
    private double topMargin = 1.0;
    private double bottomMargin = 1.0;
    private double leftMargin = 0.75;
    private double rightMargin = 0.75;
    private ColumnInfo[] columns;
    private FieldFilter[] filters;
    private RelatedLoanMatchType relatedLoanFilterSource;
    private RelatedLoanMatchType relatedLoanFieldSource;
    private bool msAnyStage;
    private List<string> milestones;
    private ReportFolderOption folderOption;
    private string[] folders;
    private bool useFieldInDB;
    private bool useFilterFieldInDB;
    private ArrayList criteriaArray = new ArrayList();
    public ReportLoanFilterType LoanFilterType = ReportLoanFilterType.None;
    public int LoanFilterRoleId;
    public string LoanFilterUserInRole = string.Empty;
    public int LoanFilterOrganizationId;
    public bool LoanFilterIncludeChildren;
    public int LoanFilterUserGroupId;
    public QueryCriterion DynamicQueryCriterion;
    public bool forTPO;
    private bool tpoFilterIncludeChildFolder;

    public ReportSettings(string title)
    {
      this.reportTitle = title;
      this.milestones = new List<string>();
    }

    public ReportSettings(string title, string xmlData)
    {
      this.reportTitle = title;
      this.parseXml(xmlData);
    }

    public ReportSettings(ReportSettings source)
    {
      this.verNo = source.verNo;
      this.reportFor = source.reportFor;
      this.reportTitle = source.reportTitle;
      this.reportType = source.reportType;
      this.archive = source.archive;
      this.fileStage = source.fileStage;
      this.timeFrame = source.timeFrame;
      this.timeFrom = source.timeFrom;
      this.timeTo = source.timeTo;
      this.paperSize = source.paperSize;
      this.paperOri = source.paperOri;
      this.topMargin = source.topMargin;
      this.bottomMargin = source.bottomMargin;
      this.leftMargin = source.leftMargin;
      this.rightMargin = source.rightMargin;
      this.forTPO = source.ForTPO;
      this.tpoFilterIncludeChildFolder = source.tpoFilterIncludeChildFolder;
      this.FileStageMileStoneID = source.FileStageMileStoneID;
      this.msAnyStage = source.msAnyStage;
      this.milestones = source.milestones;
      this.useFieldInDB = source.useFieldInDB;
      this.useFilterFieldInDB = source.useFilterFieldInDB;
      this.relatedLoanFilterSource = source.relatedLoanFilterSource;
      this.relatedLoanFieldSource = source.relatedLoanFieldSource;
      this.folderOption = source.folderOption;
      this.columns = new ColumnInfo[source.Columns.Length];
      for (int index = 0; index < source.Columns.Length; ++index)
        this.columns[index] = new ColumnInfo(source.Columns[index]);
      for (int index = 0; index < source.Filters.Length; ++index)
        this.filters[index] = new FieldFilter(source.Filters[index]);
      if (source.Folders == null)
        return;
      this.folders = new string[source.Folders.Length];
      for (int index = 0; index < source.Folders.Length; ++index)
        this.folders[index] = source.Folders[index].Trim();
    }

    public int ReportID { get; set; }

    public string FilePath { get; set; }

    public double VerNo
    {
      get => this.verNo;
      set => this.verNo = value;
    }

    public ReportsFor ReportFor
    {
      get => this.reportFor;
      set => this.reportFor = value;
    }

    public string ReportTitle
    {
      get => this.reportTitle;
      set => this.reportTitle = value;
    }

    public string ReportType
    {
      get => this.reportType;
      set => this.reportType = value;
    }

    public bool Archive
    {
      get => this.archive;
      set => this.archive = value;
    }

    public string FileStage
    {
      get => this.fileStage;
      set => this.fileStage = value;
    }

    public string FileStageMileStoneID { get; set; }

    public string TimeFrame
    {
      get => this.timeFrame;
      set => this.timeFrame = value;
    }

    public DateTime TimeFrom
    {
      get => this.timeFrom;
      set => this.timeFrom = value;
    }

    public DateTime TimeTo
    {
      get => this.timeTo;
      set => this.timeTo = value;
    }

    public string PaperSize
    {
      get => this.paperSize;
      set => this.paperSize = value;
    }

    public string PaperOrientation
    {
      get => this.paperOri;
      set => this.paperOri = value;
    }

    public double TopMargin
    {
      get => this.topMargin;
      set => this.topMargin = value;
    }

    public double BottomMargin
    {
      get => this.bottomMargin;
      set => this.bottomMargin = value;
    }

    public double LeftMargin
    {
      get => this.leftMargin;
      set => this.leftMargin = value;
    }

    public double RightMargin
    {
      get => this.rightMargin;
      set => this.rightMargin = value;
    }

    public ReportFolderOption FolderOption
    {
      get => this.folderOption;
      set => this.folderOption = value;
    }

    public ColumnInfo[] Columns
    {
      get => this.columns;
      set => this.columns = value;
    }

    public FieldFilter[] Filters
    {
      get => this.filters;
      set => this.filters = value;
    }

    public RelatedLoanMatchType RelatedLoanFilterSource
    {
      get => this.relatedLoanFilterSource;
      set => this.relatedLoanFilterSource = value;
    }

    public RelatedLoanMatchType RelatedLoanFieldSource
    {
      get => this.relatedLoanFieldSource;
      set => this.relatedLoanFieldSource = value;
    }

    public bool MSAnyStage
    {
      get => this.msAnyStage;
      set => this.msAnyStage = value;
    }

    public List<string> Milestones
    {
      get => this.milestones;
      set => this.milestones = value;
    }

    [CLSCompliant(false)]
    public bool ForTPO
    {
      get => this.forTPO;
      set => this.forTPO = value;
    }

    public bool TpoFilterIncludeChildFolder
    {
      get => this.tpoFilterIncludeChildFolder;
      set => this.tpoFilterIncludeChildFolder = value;
    }

    public string[] Folders
    {
      get => this.folders;
      set => this.folders = value;
    }

    public bool UseFieldInDB
    {
      get => this.useFieldInDB;
      set => this.useFieldInDB = value;
    }

    public bool UseFilterFieldInDB
    {
      get => this.useFilterFieldInDB;
      set => this.useFilterFieldInDB = value;
    }

    public bool UsesReportingDatabase
    {
      get
      {
        return this.reportFor == ReportsFor.Loan && (this.useFieldInDB || this.useFilterFieldInDB && this.filters.Length != 0);
      }
    }

    public bool RequiresLoanFileAccess
    {
      get
      {
        return this.reportFor == ReportsFor.Loan && (!this.useFieldInDB || !this.useFilterFieldInDB && this.filters.Length == 0);
      }
    }

    public bool HasSummary()
    {
      for (int index = 0; index < this.columns.Length; ++index)
      {
        if (this.columns[index].SummaryType == ColumnSummaryType.Total || this.columns[index].SummaryType == ColumnSummaryType.Average || this.columns[index].SummaryType == ColumnSummaryType.Count)
          return true;
      }
      return false;
    }

    public bool ContainsFieldFilter(string criterionName)
    {
      if (this.filters == null)
        return false;
      bool flag = false;
      foreach (FieldFilter filter in this.filters)
      {
        if (filter.CriterionName == criterionName)
        {
          flag = true;
          break;
        }
      }
      return flag;
    }

    public override string ToString()
    {
      XmlDocument xmlDocument = new XmlDocument();
      XmlElement xmlElement1 = (XmlElement) xmlDocument.AppendChild((XmlNode) xmlDocument.CreateElement(nameof (ReportSettings)));
      XmlElement xmlElement2 = (XmlElement) xmlElement1.AppendChild((XmlNode) xmlDocument.CreateElement("Report"));
      ((XmlElement) xmlElement2.AppendChild((XmlNode) xmlDocument.CreateElement("Version"))).SetAttribute("value", this.verNo.ToString());
      ((XmlElement) xmlElement2.AppendChild((XmlNode) xmlDocument.CreateElement("ReportFor"))).SetAttribute("value", this.reportFor.ToString());
      ((XmlElement) xmlElement2.AppendChild((XmlNode) xmlDocument.CreateElement("ReportType"))).SetAttribute("value", this.reportType);
      ((XmlElement) xmlElement2.AppendChild((XmlNode) xmlDocument.CreateElement("FileStage"))).SetAttribute("value", this.fileStage);
      ((XmlElement) xmlElement2.AppendChild((XmlNode) xmlDocument.CreateElement("TimeFrame"))).SetAttribute("value", this.timeFrame);
      ((XmlElement) xmlElement2.AppendChild((XmlNode) xmlDocument.CreateElement("DateFrom"))).SetAttribute("value", this.timeFrom.ToString());
      ((XmlElement) xmlElement2.AppendChild((XmlNode) xmlDocument.CreateElement("DateTo"))).SetAttribute("value", this.timeTo.ToString());
      ((XmlElement) xmlElement2.AppendChild((XmlNode) xmlDocument.CreateElement("PaperSize"))).SetAttribute("value", this.paperSize.ToString());
      ((XmlElement) xmlElement2.AppendChild((XmlNode) xmlDocument.CreateElement("PaperOrientation"))).SetAttribute("value", this.paperOri.ToString());
      ((XmlElement) xmlElement2.AppendChild((XmlNode) xmlDocument.CreateElement("TopMargin"))).SetAttribute("value", this.topMargin.ToString());
      ((XmlElement) xmlElement2.AppendChild((XmlNode) xmlDocument.CreateElement("BottomMargin"))).SetAttribute("value", this.bottomMargin.ToString());
      ((XmlElement) xmlElement2.AppendChild((XmlNode) xmlDocument.CreateElement("LeftMargin"))).SetAttribute("value", this.leftMargin.ToString());
      ((XmlElement) xmlElement2.AppendChild((XmlNode) xmlDocument.CreateElement("RightMargin"))).SetAttribute("value", this.rightMargin.ToString());
      ((XmlElement) xmlElement2.AppendChild((XmlNode) xmlDocument.CreateElement("UseFieldInDB"))).SetAttribute("value", this.useFieldInDB ? "Y" : "N");
      ((XmlElement) xmlElement2.AppendChild((XmlNode) xmlDocument.CreateElement("UseFilterFieldInDB"))).SetAttribute("value", this.useFilterFieldInDB ? "Y" : "N");
      ((XmlElement) xmlElement2.AppendChild((XmlNode) xmlDocument.CreateElement("RelatedLoanFilterSource"))).SetAttribute("value", this.relatedLoanFilterSource.ToString());
      ((XmlElement) xmlElement2.AppendChild((XmlNode) xmlDocument.CreateElement("RelatedLoanFieldSource"))).SetAttribute("value", this.relatedLoanFieldSource.ToString());
      ((XmlElement) xmlElement2.AppendChild((XmlNode) xmlDocument.CreateElement("ForTPO"))).SetAttribute("value", this.forTPO ? "Y" : "N");
      ((XmlElement) xmlElement2.AppendChild((XmlNode) xmlDocument.CreateElement("TpoIncludeChildFolder"))).SetAttribute("value", this.tpoFilterIncludeChildFolder ? "Y" : "N");
      XmlElement xmlElement3 = (XmlElement) xmlElement1.AppendChild((XmlNode) xmlDocument.CreateElement("FieldList"));
      for (int index = 0; index < this.columns.Length; ++index)
      {
        ColumnInfo column = this.columns[index];
        XmlElement xmlElement4 = (XmlElement) xmlElement3.AppendChild((XmlNode) xmlDocument.CreateElement("Field"));
        xmlElement4.SetAttribute("desc", column.Description);
        xmlElement4.SetAttribute("id", column.ID);
        xmlElement4.SetAttribute("sorting", column.SortOrder.ToString());
        xmlElement4.SetAttribute("summary", column.SummaryType.ToString());
        xmlElement4.SetAttribute("decimal", column.DecimalPlaces.ToString());
        xmlElement4.SetAttribute("comortgagor", column.ComortPair.ToString());
        xmlElement4.SetAttribute("criterion", column.CriterionName);
        xmlElement4.SetAttribute("isexcelfield", column.IsExcelField ? "True" : "False");
        xmlElement4.SetAttribute("excelformula", column.ExcelFormula);
        if (column.Format != FieldFormat.NONE)
          xmlElement4.SetAttribute("format", column.Format.ToString());
      }
      XmlElement xmlElement5 = (XmlElement) xmlElement1.AppendChild((XmlNode) xmlDocument.CreateElement("FilterList"));
      for (int index = 0; index < this.filters.Length; ++index)
      {
        FieldFilter filter = this.filters[index];
        XmlElement xmlElement6 = (XmlElement) xmlElement5.AppendChild((XmlNode) xmlDocument.CreateElement("Filter"));
        xmlElement6.SetAttribute("FieldID", filter.FieldID);
        xmlElement6.SetAttribute("Description", filter.FieldDescription);
        xmlElement6.SetAttribute("FieldType", filter.FieldType.ToString());
        xmlElement6.SetAttribute("Operators", new OperatorTypesEnumNameProvider().GetName((object) filter.OperatorType));
        xmlElement6.SetAttribute("ValueFrom", filter.ValueFrom);
        xmlElement6.SetAttribute("ValueTo", filter.ValueTo);
        xmlElement6.SetAttribute("JointToken", filter.JointToken.ToString());
        xmlElement6.SetAttribute("LeftParentheses", filter.LeftParentheses.ToString());
        xmlElement6.SetAttribute("RightParentheses", filter.RightParentheses.ToString());
        xmlElement6.SetAttribute("CriterionName", filter.CriterionName);
      }
      XmlElement xmlElement7 = (XmlElement) xmlElement1.AppendChild((XmlNode) xmlDocument.CreateElement("Milestones"));
      ((XmlElement) xmlElement7.AppendChild((XmlNode) xmlDocument.CreateElement("Any"))).SetAttribute("value", this.msAnyStage ? "Y" : "N");
      foreach (string milestone in this.milestones)
        ((XmlElement) xmlElement7.AppendChild((XmlNode) xmlDocument.CreateElement("Milestone"))).SetAttribute("value", milestone);
      XmlElement xmlElement8 = (XmlElement) xmlElement1.AppendChild((XmlNode) xmlDocument.CreateElement("FolderList"));
      xmlElement8.SetAttribute("Option", this.folderOption.ToString());
      if (this.folders != null)
      {
        for (int index = 0; index < this.folders.Length; ++index)
          ((XmlElement) xmlElement8.AppendChild((XmlNode) xmlDocument.CreateElement("Folder"))).SetAttribute("name", this.folders[index]);
      }
      return xmlDocument.OuterXml;
    }

    private void parseXml(string xmlData)
    {
      XPathNavigator navigator = new XPathDocument((TextReader) new StringReader(xmlData)).CreateNavigator();
      this.milestones = new List<string>();
      XPathNodeIterator xpathNodeIterator1 = navigator.Select("/ReportSettings/Report/ReportFor");
      xpathNodeIterator1.MoveNext();
      this.reportFor = (ReportsFor) this.toEnum(xpathNodeIterator1.Current.GetAttribute("value", ""), typeof (ReportsFor));
      XPathNodeIterator xpathNodeIterator2 = navigator.Select("/ReportSettings/Report/ReportType");
      xpathNodeIterator2.MoveNext();
      this.reportType = xpathNodeIterator2.Current.GetAttribute("value", "");
      XPathNodeIterator xpathNodeIterator3 = navigator.Select("/ReportSettings/Report/UseFieldInDB");
      xpathNodeIterator3.MoveNext();
      this.useFieldInDB = this.toBoolean(xpathNodeIterator3.Current.GetAttribute("value", ""));
      XPathNodeIterator xpathNodeIterator4 = navigator.Select("/ReportSettings/Report/UseFilterFieldInDB");
      xpathNodeIterator4.MoveNext();
      this.useFilterFieldInDB = this.toBoolean(xpathNodeIterator4.Current.GetAttribute("value", ""));
      XPathNodeIterator xpathNodeIterator5 = navigator.Select("/ReportSettings/Report/RelatedLoanFilterSource");
      xpathNodeIterator5.MoveNext();
      try
      {
        this.relatedLoanFilterSource = (RelatedLoanMatchType) Enum.Parse(typeof (RelatedLoanMatchType), xpathNodeIterator5.Current.GetAttribute("value", ""));
      }
      catch
      {
        this.relatedLoanFilterSource = RelatedLoanMatchType.AnyOriginated;
      }
      XPathNodeIterator xpathNodeIterator6 = navigator.Select("/ReportSettings/Report/RelatedLoanFieldSource");
      xpathNodeIterator6.MoveNext();
      try
      {
        this.relatedLoanFieldSource = (RelatedLoanMatchType) Enum.Parse(typeof (RelatedLoanMatchType), xpathNodeIterator6.Current.GetAttribute("value", ""));
      }
      catch
      {
        this.relatedLoanFieldSource = RelatedLoanMatchType.LastClosed;
      }
      XPathNodeIterator xpathNodeIterator7 = navigator.Select("/ReportSettings/Report/Version");
      xpathNodeIterator7.MoveNext();
      this.verNo = this.toDouble(xpathNodeIterator7.Current.GetAttribute("value", ""));
      XPathNodeIterator xpathNodeIterator8 = navigator.Select("/ReportSettings/Report/FileStage");
      xpathNodeIterator8.MoveNext();
      this.fileStage = xpathNodeIterator8.Current.GetAttribute("value", "");
      if (this.verNo < 2.7)
      {
        if (this.fileStage == "Closed")
          this.fileStage = "Completed";
      }
      else if (this.verNo < 9.0 && this.fileStage == "submittal")
        this.fileStage = "Submittal";
      if (this.verNo < 1.5)
      {
        XPathNodeIterator xpathNodeIterator9 = navigator.Select("/ReportSettings/Report/Archive");
        xpathNodeIterator9.MoveNext();
        this.archive = this.toBoolean(xpathNodeIterator9.Current.GetAttribute("value", ""));
      }
      else if (this.verNo < 9.0)
      {
        XPathNodeIterator xpathNodeIterator10 = navigator.Select("/ReportSettings/Milestones/Any");
        xpathNodeIterator10.MoveNext();
        this.msAnyStage = this.toBoolean(xpathNodeIterator10.Current.GetAttribute("value", ""));
        XPathNodeIterator xpathNodeIterator11 = navigator.Select("/ReportSettings/Milestones/Started");
        xpathNodeIterator11.MoveNext();
        if (this.toBoolean(xpathNodeIterator11.Current.GetAttribute("value", "")))
          this.milestones.Add("Started");
        XPathNodeIterator xpathNodeIterator12 = navigator.Select("/ReportSettings/Milestones/Processing");
        xpathNodeIterator12.MoveNext();
        if (this.toBoolean(xpathNodeIterator12.Current.GetAttribute("value", "")))
          this.milestones.Add("Processing");
        XPathNodeIterator xpathNodeIterator13 = navigator.Select("/ReportSettings/Milestones/Submitted");
        xpathNodeIterator13.MoveNext();
        if (this.toBoolean(xpathNodeIterator13.Current.GetAttribute("value", "")))
          this.milestones.Add("Submittal");
        XPathNodeIterator xpathNodeIterator14 = navigator.Select("/ReportSettings/Milestones/Approved");
        xpathNodeIterator14.MoveNext();
        if (this.toBoolean(xpathNodeIterator14.Current.GetAttribute("value", "")))
          this.milestones.Add("Approval");
        XPathNodeIterator xpathNodeIterator15 = navigator.Select("/ReportSettings/Milestones/Signed");
        xpathNodeIterator15.MoveNext();
        if (this.toBoolean(xpathNodeIterator15.Current.GetAttribute("value", "")))
          this.milestones.Add("Docs Signing");
        XPathNodeIterator xpathNodeIterator16 = navigator.Select("/ReportSettings/Milestones/Funded");
        xpathNodeIterator16.MoveNext();
        if (this.toBoolean(xpathNodeIterator16.Current.GetAttribute("value", "")))
          this.milestones.Add("Funding");
        XPathNodeIterator xpathNodeIterator17 = this.verNo >= 2.7 ? navigator.Select("/ReportSettings/Milestones/Completed") : navigator.Select("/ReportSettings/Milestones/Closed");
        xpathNodeIterator17.MoveNext();
        if (this.toBoolean(xpathNodeIterator17.Current.GetAttribute("value", "")))
          this.milestones.Add("Completion");
      }
      else
      {
        XPathNodeIterator xpathNodeIterator18 = navigator.Select("/ReportSettings/Milestones/Any");
        xpathNodeIterator18.MoveNext();
        this.msAnyStage = this.toBoolean(xpathNodeIterator18.Current.GetAttribute("value", ""));
        XPathNodeIterator xpathNodeIterator19 = navigator.Select("/ReportSettings/Milestones/Milestone");
        while (xpathNodeIterator19.MoveNext())
          this.milestones.Add(xpathNodeIterator19.Current.GetAttribute("value", ""));
      }
      XPathNodeIterator xpathNodeIterator20 = navigator.Select("/ReportSettings/Report/TimeFrame");
      xpathNodeIterator20.MoveNext();
      this.timeFrame = xpathNodeIterator20.Current.GetAttribute("value", "");
      XPathNodeIterator xpathNodeIterator21 = navigator.Select("/ReportSettings/Report/DateFrom");
      xpathNodeIterator21.MoveNext();
      string attribute1 = xpathNodeIterator21.Current.GetAttribute("value", "");
      this.timeFrom = !(attribute1 != "") ? DateTime.MinValue : this.toDateTime(attribute1);
      XPathNodeIterator xpathNodeIterator22 = navigator.Select("/ReportSettings/Report/DateTo");
      xpathNodeIterator22.MoveNext();
      string attribute2 = xpathNodeIterator22.Current.GetAttribute("value", "");
      this.timeTo = !(attribute2 != "") ? DateTime.MinValue : this.toDateTime(attribute2);
      XPathNodeIterator xpathNodeIterator23 = navigator.Select("/ReportSettings/Report/PaperSize");
      xpathNodeIterator23.MoveNext();
      this.paperSize = xpathNodeIterator23.Current.GetAttribute("value", "");
      XPathNodeIterator xpathNodeIterator24 = navigator.Select("/ReportSettings/Report/PaperOrientation");
      xpathNodeIterator24.MoveNext();
      this.paperOri = xpathNodeIterator24.Current.GetAttribute("value", "");
      XPathNodeIterator xpathNodeIterator25 = navigator.Select("/ReportSettings/Report/TopMargin");
      xpathNodeIterator25.MoveNext();
      this.topMargin = this.toDouble(xpathNodeIterator25.Current.GetAttribute("value", ""));
      XPathNodeIterator xpathNodeIterator26 = navigator.Select("/ReportSettings/Report/BottomMargin");
      xpathNodeIterator26.MoveNext();
      this.bottomMargin = this.toDouble(xpathNodeIterator26.Current.GetAttribute("value", ""));
      XPathNodeIterator xpathNodeIterator27 = navigator.Select("/ReportSettings/Report/LeftMargin");
      xpathNodeIterator27.MoveNext();
      this.leftMargin = this.toDouble(xpathNodeIterator27.Current.GetAttribute("value", ""));
      XPathNodeIterator xpathNodeIterator28 = navigator.Select("/ReportSettings/Report/RightMargin");
      xpathNodeIterator28.MoveNext();
      this.rightMargin = this.toDouble(xpathNodeIterator28.Current.GetAttribute("value", ""));
      XPathNodeIterator xpathNodeIterator29 = navigator.Select("/ReportSettings/Report/ForTPO");
      xpathNodeIterator29.MoveNext();
      this.forTPO = xpathNodeIterator29.Current.GetAttribute("value", "") == "Y";
      XPathNodeIterator xpathNodeIterator30 = navigator.Select("/ReportSettings/Report/TpoIncludeChildFolder");
      xpathNodeIterator30.MoveNext();
      this.tpoFilterIncludeChildFolder = xpathNodeIterator30.Current.GetAttribute("value", "") == "Y";
      XPathNodeIterator xpathNodeIterator31 = navigator.Select("/ReportSettings/FieldList/Field");
      this.columns = new ColumnInfo[xpathNodeIterator31.Count];
      int index1 = 0;
      while (xpathNodeIterator31.MoveNext())
      {
        string attribute3 = xpathNodeIterator31.Current.GetAttribute("desc", "");
        string attribute4 = xpathNodeIterator31.Current.GetAttribute("id", "");
        ColumnSortOrder order = (ColumnSortOrder) this.toEnum(xpathNodeIterator31.Current.GetAttribute("sorting", ""), typeof (ColumnSortOrder));
        ColumnSummaryType summary = (ColumnSummaryType) this.toEnum(xpathNodeIterator31.Current.GetAttribute("summary", ""), typeof (ColumnSummaryType));
        int integer1 = this.toInteger(xpathNodeIterator31.Current.GetAttribute("decimal", ""));
        int integer2 = this.toInteger(xpathNodeIterator31.Current.GetAttribute("comortgagor", ""));
        string attribute5 = xpathNodeIterator31.Current.GetAttribute("criterion", "");
        string attribute6 = xpathNodeIterator31.Current.GetAttribute("isexcelfield", "");
        string attribute7 = xpathNodeIterator31.Current.GetAttribute("excelformula", "");
        if (attribute6.ToLower() != "true")
        {
          this.columns[index1] = new ColumnInfo(attribute4, attribute3, order, summary, integer1, integer2);
        }
        else
        {
          FieldFormat fieldFormat;
          try
          {
            fieldFormat = (FieldFormat) Enum.Parse(typeof (FieldFormat), xpathNodeIterator31.Current.GetAttribute("format", ""));
          }
          catch
          {
            fieldFormat = FieldFormat.NONE;
          }
          this.columns[index1] = new ColumnInfo(attribute3, order, summary, integer1, attribute7, fieldFormat);
        }
        this.columns[index1].CriterionName = attribute5;
        ++index1;
      }
      XPathNodeIterator xpathNodeIterator32 = navigator.Select("/ReportSettings/FilterList/Filter");
      this.filters = new FieldFilter[xpathNodeIterator32.Count];
      OperatorTypesEnumNameProvider enumNameProvider = new OperatorTypesEnumNameProvider();
      int index2 = 0;
      while (xpathNodeIterator32.MoveNext())
      {
        string attribute8 = xpathNodeIterator32.Current.GetAttribute("FieldID", "");
        this.filters[index2] = new FieldFilter(FieldTypes.IsString, attribute8, "", "", OperatorTypes.IsExact, "", "");
        this.filters[index2].CriterionName = attribute8;
        string attribute9 = xpathNodeIterator32.Current.GetAttribute("FieldType", "");
        this.filters[index2].FieldType = (FieldTypes) this.toEnum(attribute9, typeof (FieldTypes));
        this.filters[index2].OperatorType = (OperatorTypes) enumNameProvider.GetValue(xpathNodeIterator32.Current.GetAttribute("Operators", ""));
        this.filters[index2].ValueFrom = xpathNodeIterator32.Current.GetAttribute("ValueFrom", "");
        this.filters[index2].ValueTo = xpathNodeIterator32.Current.GetAttribute("ValueTo", "");
        this.filters[index2].JointToken = (JointTokens) this.toEnum(xpathNodeIterator32.Current.GetAttribute("JointToken", ""), typeof (JointTokens));
        this.filters[index2].LeftParentheses = this.toInteger(xpathNodeIterator32.Current.GetAttribute("LeftParentheses", ""));
        this.filters[index2].RightParentheses = this.toInteger(xpathNodeIterator32.Current.GetAttribute("RightParentheses", ""));
        this.filters[index2].FieldDescription = xpathNodeIterator32.Current.GetAttribute("Description", "");
        this.filters[index2].CriterionName = xpathNodeIterator32.Current.GetAttribute("CriterionName", "") ?? "";
        ++index2;
      }
      if (this.verNo > 1.1)
      {
        XPathNodeIterator xpathNodeIterator33 = navigator.Select("/ReportSettings/FolderList/Folder");
        this.folders = new string[xpathNodeIterator33.Count];
        int index3 = 0;
        while (xpathNodeIterator33.MoveNext())
        {
          this.folders[index3] = xpathNodeIterator33.Current.GetAttribute("name", "");
          ++index3;
        }
      }
      XPathNodeIterator xpathNodeIterator34 = navigator.Select("/ReportSettings/FolderList");
      xpathNodeIterator34.MoveNext();
      try
      {
        this.folderOption = (ReportFolderOption) Enum.Parse(typeof (ReportFolderOption), xpathNodeIterator34.Current.GetAttribute("Option", ""), true);
      }
      catch
      {
        this.folderOption = ReportFolderOption.None;
      }
      if (this.reportFor != ReportsFor.BorrowerContact && this.reportFor != ReportsFor.BusinessContact)
        return;
      this.migrateContactReportFields();
    }

    private void migrateContactReportFields()
    {
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      dictionary["CompletedLoan.AppraisedValue"] = "356";
      dictionary["CompletedLoan.LoanAmount"] = "1109";
      dictionary["CompletedLoan.InterestRate"] = "3";
      dictionary["CompletedLoan.Term"] = "4";
      dictionary["CompletedLoan.Purpose"] = "19";
      dictionary["CompletedLoan.DownPayment"] = "1335";
      dictionary["CompletedLoan.LTV"] = "353";
      dictionary["CompletedLoan.Amortization"] = "608";
      dictionary["CompletedLoan.DateCompleted"] = "MS.CLO";
      dictionary["CompletedLoan.LoanType"] = "1172";
      dictionary["CompletedLoan.LienPosition"] = "420";
      bool flag = false;
      foreach (ColumnInfo column in this.columns)
      {
        if (dictionary.ContainsKey(column.FieldID))
        {
          column.ReplaceFieldID(dictionary[column.FieldID]);
          this.relatedLoanFieldSource = RelatedLoanMatchType.LastClosed;
          flag = true;
        }
      }
      foreach (ColumnInfo column in this.columns)
      {
        if (column.FieldID == "Marketing.Loan Origination.TimeOfHistory" && !flag)
        {
          column.ReplaceFieldID("MS.START");
          this.relatedLoanFieldSource = RelatedLoanMatchType.LastOriginated;
        }
      }
      foreach (FieldFilter filter in this.filters)
      {
        if (dictionary.ContainsKey(filter.FieldID))
        {
          filter.FieldID = dictionary[filter.FieldID];
          this.relatedLoanFilterSource = RelatedLoanMatchType.LastClosed;
        }
      }
    }

    public QueryCriterion ToQueryCriterion()
    {
      QueryCriterion queryCriterion1 = (QueryCriterion) null;
      if (this.reportFor == ReportsFor.Loan || this.reportFor == ReportsFor.LoanTrades || this.reportFor == ReportsFor.CorrespondentTrades || this.reportFor == ReportsFor.MBSPools)
      {
        DateTime now = DateTime.Now;
        DateTime dateTime1 = DateTime.Now;
        DateTime dateTime2 = DateTime.Now;
        switch (this.timeFrame)
        {
          case "Current Month":
            dateTime1 = DateTime.Parse(now.Month.ToString("00") + "/01/" + now.Year.ToString("0000"));
            dateTime2 = now;
            break;
          case "Current Week":
            dateTime2 = now;
            int dayOfWeek1 = this.getDayOfWeek(now.DayOfWeek);
            dateTime1 = now.AddDays((double) dayOfWeek1);
            break;
          case "Custom Dates":
            dateTime1 = this.TimeFrom;
            dateTime2 = this.TimeTo;
            break;
          case "Empty Date Field":
            dateTime1 = DateTime.MinValue;
            dateTime2 = DateTime.MinValue;
            break;
          case "Last 30 Days":
            dateTime2 = now;
            dateTime1 = now.AddDays(-30.0);
            break;
          case "Last 365 Days":
            dateTime2 = now;
            dateTime1 = now.AddDays(-365.0);
            break;
          case "Last 7 Days":
            dateTime2 = now;
            dateTime1 = now.AddDays(-7.0);
            break;
          case "Last 90 Days":
            dateTime2 = now;
            dateTime1 = now.AddDays(-90.0);
            break;
          case "Previous Month":
            DateTime dateTime3 = now.AddMonths(-1);
            dateTime1 = DateTime.Parse(dateTime3.Month.ToString("00") + "/01/" + dateTime3.Year.ToString("0000"));
            dateTime3 = dateTime1.AddMonths(1).AddDays(-1.0);
            dateTime2 = DateTime.Parse(dateTime3.Month.ToString("00") + "/" + dateTime3.Day.ToString("00") + "/" + dateTime3.Year.ToString("0000"));
            break;
          case "Previous Week":
            DateTime dateTime4 = now.AddDays(-7.0);
            int dayOfWeek2 = this.getDayOfWeek(dateTime4.DayOfWeek);
            DateTime dateTime5 = dateTime4.AddDays((double) dayOfWeek2);
            dateTime1 = dateTime5;
            dateTime2 = dateTime5.AddDays(7.0);
            break;
          case "Previous Year":
            DateTime dateTime6 = now.AddYears(-1);
            int year = dateTime6.Year;
            dateTime1 = DateTime.Parse("01/01/" + year.ToString("0000"));
            year = dateTime6.Year;
            dateTime2 = DateTime.Parse("12/31/" + year.ToString("0000"));
            break;
          case "Year-to-date":
            dateTime1 = DateTime.Parse("01/01/" + now.Year.ToString("0000"));
            dateTime2 = now;
            break;
        }
        if (this.folderOption == ReportFolderOption.Active)
          queryCriterion1 = (QueryCriterion) new StringValueCriterion("LoanFolder.Active", "Y");
        else if (this.folderOption == ReportFolderOption.AllExceptTrash)
          queryCriterion1 = (QueryCriterion) new StringValueCriterion("LoanFolder.Trash", "N");
        else if (this.folderOption != ReportFolderOption.All && this.folders != null)
          queryCriterion1 = (QueryCriterion) new ListValueCriterion("Loan.LoanFolder", (Array) this.folders);
        if (!this.msAnyStage)
        {
          QueryCriterion queryCriterion2 = (QueryCriterion) null;
          if (this.milestones.Count > 0)
            queryCriterion2 = (QueryCriterion) new ListValueCriterion("Loan.CurrentMilestoneName", (Array) this.milestones.ToArray());
          if (this.FileStage != "")
          {
            string fieldName;
            switch (this.FileStage)
            {
              case "Approval":
                fieldName = "Loan.DateApprovalReceived";
                break;
              case "Completion":
                fieldName = "Loan.DateCompleted";
                break;
              case "Docs Signing":
                fieldName = "Loan.DateDocsSigned";
                break;
              case "Funding":
                fieldName = "Loan.DateFunded";
                break;
              case "Processing":
                fieldName = "Loan.DateSentToProcessing";
                break;
              case "Started":
                fieldName = "Loan.DateFileOpened";
                break;
              case "submittal":
                fieldName = "Loan.DateSubmittedToLender";
                break;
              default:
                fieldName = "Fields.Log.MS.Date." + this.FileStage;
                break;
            }
            if (fieldName != "")
            {
              QueryCriterion rhs = (QueryCriterion) new BinaryOperation(BinaryOperator.And, (QueryCriterion) new DateValueCriterion(fieldName, dateTime1, OrdinalMatchType.GreaterThanOrEquals, DateMatchPrecision.Day), (QueryCriterion) new DateValueCriterion(fieldName, dateTime2, OrdinalMatchType.LessThanOrEquals, DateMatchPrecision.Day));
              queryCriterion2 = queryCriterion2 != null ? (QueryCriterion) new BinaryOperation(BinaryOperator.And, queryCriterion2, rhs) : rhs;
            }
          }
          if (queryCriterion2 != null)
            queryCriterion1 = queryCriterion1 == null ? queryCriterion2 : queryCriterion1.And(queryCriterion2);
        }
      }
      return queryCriterion1;
    }

    private int getDayOfWeek(DayOfWeek dow)
    {
      switch (dow)
      {
        case DayOfWeek.Sunday:
          return 0;
        case DayOfWeek.Monday:
          return -1;
        case DayOfWeek.Tuesday:
          return -2;
        case DayOfWeek.Wednesday:
          return -3;
        case DayOfWeek.Thursday:
          return -4;
        case DayOfWeek.Friday:
          return -5;
        case DayOfWeek.Saturday:
          return -6;
        default:
          return 0;
      }
    }

    private int toInteger(string value)
    {
      try
      {
        return int.Parse(value);
      }
      catch
      {
        return ReportSettings.NoValue;
      }
    }

    private DateTime toDateTime(string value)
    {
      try
      {
        return DateTime.Parse(value);
      }
      catch
      {
        return DateTime.MinValue;
      }
    }

    private double toDouble(string value)
    {
      try
      {
        return double.Parse(value);
      }
      catch
      {
        return 0.0;
      }
    }

    private bool toBoolean(string value) => value == "Y";

    private object toEnum(string value, Type enumType)
    {
      try
      {
        return Enum.Parse(enumType, value, true);
      }
      catch
      {
        return (object) 0;
      }
    }

    public bool IncludeSort
    {
      get
      {
        for (int index = 0; index < this.columns.Length; ++index)
        {
          if (this.columns[index].SortOrder != ColumnSortOrder.None)
            return true;
        }
        return false;
      }
    }
  }
}
