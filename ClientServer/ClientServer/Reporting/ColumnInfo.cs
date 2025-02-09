// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Reporting.ColumnInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Reporting
{
  [Serializable]
  public class ColumnInfo : BinaryConvertibleObject
  {
    public const int NoValue = -1;
    private string versionNumber = "1.0";
    private string desc;
    private string id;
    private ColumnSortOrder sortOrder;
    private ColumnSummaryType summaryType;
    private int decimalPlaces;
    private string criterionName = string.Empty;
    private int comortPair;
    private bool isExcelField;
    private string excelFormula = "";
    private FieldFormat fieldFormat;
    private bool isParent;

    public ColumnInfo(
      string id,
      string desc,
      ColumnSortOrder order,
      ColumnSummaryType summary,
      int decimalPlaces)
      : this(id, desc, order, summary, decimalPlaces, 0)
    {
    }

    public ColumnInfo(
      string desc,
      ColumnSortOrder order,
      ColumnSummaryType summary,
      int decimalPlaces,
      string excelFormula,
      FieldFormat fieldFormat)
      : this("ExcelField." + Guid.NewGuid().ToString(), desc, order, summary, decimalPlaces, 0)
    {
      this.excelFormula = excelFormula;
      this.fieldFormat = fieldFormat;
      this.isExcelField = true;
    }

    public ColumnInfo(
      string id,
      string desc,
      ColumnSortOrder order,
      ColumnSummaryType summary,
      int decimalPlaces,
      int comortPair)
    {
      this.id = id;
      this.desc = desc;
      this.sortOrder = order;
      this.summaryType = summary;
      this.decimalPlaces = decimalPlaces;
      this.comortPair = comortPair;
      if (this.comortPair >= 1)
        return;
      this.comortPair = 1;
    }

    public ColumnInfo()
    {
    }

    public ColumnInfo(ColumnInfo source)
    {
      this.versionNumber = source.versionNumber;
      this.id = source.id;
      this.desc = source.desc;
      this.sortOrder = source.sortOrder;
      this.summaryType = source.summaryType;
      this.decimalPlaces = source.decimalPlaces;
      this.criterionName = source.criterionName;
      this.comortPair = source.comortPair;
      this.isExcelField = source.isExcelField;
      this.excelFormula = source.excelFormula;
      this.fieldFormat = source.fieldFormat;
      if (this.comortPair >= 1)
        return;
      this.comortPair = 1;
    }

    public static explicit operator ColumnInfo(BinaryObject binaryObject)
    {
      return (ColumnInfo) BinaryConvertibleObject.Parse(binaryObject, typeof (ColumnInfo));
    }

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("versionNumber", (object) this.versionNumber);
      info.AddValue("desc", (object) this.desc);
      info.AddValue("id", (object) this.id);
      info.AddValue("sortOrder", (object) this.sortOrder);
      info.AddValue("summaryType", (object) this.summaryType);
      info.AddValue("decimalPlaces", (object) this.decimalPlaces);
      info.AddValue("criterionName", (object) this.criterionName);
      info.AddValue("comortgagor", (object) this.comortPair);
      info.AddValue("isexcelfield", this.isExcelField ? (object) "True" : (object) "False");
      info.AddValue("excelformula", (object) this.excelFormula);
      info.AddValue("format", (object) this.fieldFormat.ToString());
    }

    public ColumnInfo(XmlSerializationInfo info)
    {
      this.versionNumber = info.GetString(nameof (versionNumber));
      this.desc = info.GetString(nameof (desc));
      this.id = info.GetString(nameof (id));
      this.sortOrder = (ColumnSortOrder) info.GetValue(nameof (sortOrder), typeof (ColumnSortOrder));
      this.summaryType = (ColumnSummaryType) info.GetValue(nameof (summaryType), typeof (ColumnSummaryType));
      this.decimalPlaces = info.GetInteger(nameof (decimalPlaces));
      this.criterionName = info.GetString(nameof (criterionName));
      this.comortPair = info.GetInteger("comortgagor");
      try
      {
        this.isExcelField = info.GetString("isexcelfield") == "True";
      }
      catch
      {
      }
      try
      {
        this.excelFormula = info.GetString("excelformula");
      }
      catch
      {
      }
      try
      {
        this.fieldFormat = (FieldFormat) Enum.Parse(typeof (FieldFormat), info.GetString("format"), true);
      }
      catch
      {
        this.fieldFormat = FieldFormat.NONE;
      }
    }

    public string VersionNumber
    {
      get => this.versionNumber;
      set => this.versionNumber = value;
    }

    public string FieldID
    {
      get
      {
        return this.id.IndexOf('#') == -1 && this.comortPair > 1 ? FieldPairParser.GetFieldIDForBorrowerPair(this.id, this.comortPair) : this.id;
      }
    }

    public string ID
    {
      get => this.id;
      set => this.id = value;
    }

    public string Description
    {
      get => this.desc;
      set => this.desc = value;
    }

    public FieldFormat Format
    {
      get => this.fieldFormat;
      set => this.fieldFormat = value;
    }

    public ColumnSortOrder SortOrder
    {
      get => this.sortOrder;
      set => this.sortOrder = value;
    }

    public ColumnSummaryType SummaryType
    {
      get => this.summaryType;
      set => this.summaryType = value;
    }

    public int DecimalPlaces
    {
      get => this.decimalPlaces;
      set => this.decimalPlaces = value;
    }

    public string CriterionName
    {
      get => this.criterionName;
      set => this.criterionName = value;
    }

    public int ComortPair
    {
      get
      {
        if (this.comortPair > 1 || this.id.IndexOf('#') <= -1)
          return this.comortPair;
        FieldPairInfo fieldPairInfo = FieldPairParser.ParseFieldPairInfo(this.id);
        return fieldPairInfo != null ? fieldPairInfo.PairIndex : this.comortPair;
      }
      set => this.comortPair = value;
    }

    public bool IsExcelField
    {
      get => this.isExcelField;
      set => this.isExcelField = value;
    }

    public string[] FieldsIncludedInExcelFormula
    {
      get
      {
        if (!this.isExcelField || this.excelFormula.Trim() == "")
          return new string[0];
        List<string> stringList = new List<string>();
        int startIndex1;
        for (int startIndex2 = this.excelFormula.IndexOf('['); startIndex2 > 0 && startIndex2 < this.excelFormula.Length; startIndex2 = this.excelFormula.IndexOf('[', startIndex1))
        {
          startIndex1 = this.excelFormula.IndexOf(']', startIndex2);
          if (startIndex1 >= 0)
          {
            string str = this.excelFormula.Substring(startIndex2 + 1, startIndex1 - startIndex2 - 1);
            stringList.Add(str);
          }
          else
            break;
        }
        return stringList.ToArray();
      }
    }

    public string ExcelFormula
    {
      get => this.excelFormula;
      set => this.excelFormula = value;
    }

    public string FieldIDWOBorrowerPair
    {
      get
      {
        if (this.id.IndexOf('#') == -1 || this.id.StartsWith("AuditTrail"))
          return this.id;
        FieldPairInfo fieldPairInfo = FieldPairParser.ParseFieldPairInfo(this.id);
        return fieldPairInfo == null ? this.id : fieldPairInfo.FieldID;
      }
    }

    public int FilterListOrder { get; set; }

    public void ReplaceFieldID(string fieldId) => this.id = fieldId;

    public bool IsParent
    {
      get => this.isParent;
      set => this.isParent = value;
    }
  }
}
