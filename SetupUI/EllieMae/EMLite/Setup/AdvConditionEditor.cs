// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.AdvConditionEditor
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class AdvConditionEditor : Form
  {
    private IContainer components;
    private Panel panel1;
    private AdvancedSearchControl ctlAdvSearch;
    private DialogButtons dlgButtons;

    public AdvConditionEditor(Sessions.Session session) => this.InitializeComponent();

    public AdvConditionEditor(Sessions.Session session, string xmlCondition)
    {
      this.InitializeComponent();
      this.ctlAdvSearch.FieldDefs = (ReportFieldDefs) LoanReportFieldDefs.GetFieldDefs(session, LoanReportFieldFlags.BasicLoanDataFields);
      this.ctlAdvSearch.SetIncludeChildFolderVisible = false;
      if (!((xmlCondition ?? "") != ""))
        return;
      this.loadCondition(xmlCondition);
    }

    public AdvConditionEditor(Sessions.Session session, string xmlCondition, bool isDDM)
      : this(session, xmlCondition)
    {
      this.ctlAdvSearch.DDMSetting = isDDM;
    }

    public AdvConditionEditor(
      Sessions.Session session,
      string xmlCondition,
      ReportFieldDefs fielDefs)
    {
      this.InitializeComponent();
      this.ctlAdvSearch.FieldDefs = fielDefs;
      this.ctlAdvSearch.SetIncludeChildFolderVisible = false;
      if (!((xmlCondition ?? "") != ""))
        return;
      this.loadCondition(xmlCondition);
    }

    private void loadCondition(string xml)
    {
      this.ctlAdvSearch.SetCurrentFilter((FieldFilterList) new XmlSerializer().Deserialize(xml, typeof (FieldFilterList)));
    }

    public void ClearFilters() => this.ctlAdvSearch.ClearFilters();

    public string GetConditionXml()
    {
      return new XmlSerializer().Serialize((object) this.ctlAdvSearch.GetCurrentFilter());
    }

    public string GetConditionScript()
    {
      return AdvConditionEditor.GetConditionScriptForFilter(this.ctlAdvSearch.GetCurrentFilter());
    }

    public static string GetConditionScriptForFilter(FieldFilterList filters)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (FieldFilter filter in (List<FieldFilter>) filters)
      {
        stringBuilder.Append(filter.LeftParenthesesToString);
        stringBuilder.Append(AdvConditionEditor.createFilterExpression(filter));
        stringBuilder.Append(filter.RightParenthesesToString);
        if (filter.JointToken != JointTokens.Nothing)
          stringBuilder.Append(" " + filter.JointTokenToString + " ");
      }
      return stringBuilder.ToString();
    }

    private static string createFilterExpression(FieldFilter filter)
    {
      switch (filter.FieldType)
      {
        case FieldTypes.IsNumeric:
          return AdvConditionEditor.createNumericExpression(filter);
        case FieldTypes.IsDate:
          return AdvConditionEditor.createDateExpression(filter, false);
        case FieldTypes.IsMonthDay:
          return AdvConditionEditor.createMonthDayExpression(filter);
        case FieldTypes.IsOptionList:
          return AdvConditionEditor.createOptionListExpression(filter);
        case FieldTypes.IsYesNo:
          return AdvConditionEditor.createYesNoExpression(filter);
        case FieldTypes.IsCheckbox:
          return AdvConditionEditor.createCheckboxExpression(filter);
        case FieldTypes.IsDateTime:
          return AdvConditionEditor.createDateExpression(filter, true);
        default:
          return AdvConditionEditor.createStringExpression(filter);
      }
    }

    private static string createCheckboxExpression(FieldFilter filter)
    {
      return filter.OperatorType == OperatorTypes.IsChecked ? "[" + filter.FieldID + "] = \"X\"" : "[" + filter.FieldID + "] <> \"X\"";
    }

    private static string createYesNoExpression(FieldFilter filter)
    {
      return filter.OperatorType == OperatorTypes.IsYes ? "[" + filter.FieldID + "] = \"Y\"" : "[" + filter.FieldID + "] <> \"Y\"";
    }

    private static string createOptionListExpression(FieldFilter filter)
    {
      string[] optionsList = filter.GetOptionsList();
      if (optionsList.Length == 0)
        return "";
      if (optionsList.Length == 1)
        return "[" + filter.FieldID + "] " + (filter.OperatorType == OperatorTypes.IsAnyOf ? "=" : "<>") + " \"" + optionsList[0] + "\"";
      string str = "(";
      if (filter.OperatorType == OperatorTypes.IsNotAnyOf)
        str = "NOT " + str;
      for (int index = 0; index < optionsList.Length; ++index)
      {
        if (index > 0)
          str += " OR ";
        str = str + "[" + filter.FieldID + "] = \"" + optionsList[index] + "\"";
      }
      return str + ")";
    }

    private static string createDateExpression(FieldFilter filter, bool includeTime)
    {
      string str = "[@" + filter.FieldID + "]";
      string format = "M/d/yyyy";
      if (includeTime)
        format += " hh:mm";
      switch (filter.OperatorType)
      {
        case OperatorTypes.IsExact:
        case OperatorTypes.Equals:
          return str + " = #" + filter.DateFrom.ToString(format) + "#";
        case OperatorTypes.IsNot:
          return str + " <> #" + Utils.ParseDate((object) filter.ValueFrom).ToString(format) + "#";
        case OperatorTypes.NotEqual:
          return str + " <> #" + filter.DateFrom.ToString(format) + "#";
        case OperatorTypes.GreaterThan:
        case OperatorTypes.DateAfter:
          return str + " > #" + filter.DateFrom.ToString(format) + "#";
        case OperatorTypes.NotGreaterThan:
        case OperatorTypes.DateOnOrBefore:
          return str + " <= #" + filter.DateFrom.ToString(format) + "#";
        case OperatorTypes.LessThan:
        case OperatorTypes.DateBefore:
          return str + " < #" + filter.DateFrom.ToString(format) + "#";
        case OperatorTypes.NotLessThan:
        case OperatorTypes.DateOnOrAfter:
          return str + " >= #" + filter.DateFrom.ToString(format) + "#";
        case OperatorTypes.DateBetween:
          string[] strArray1 = new string[9];
          strArray1[0] = "(";
          strArray1[1] = str;
          strArray1[2] = " >= #";
          DateTime dateTime1 = filter.DateFrom;
          strArray1[3] = dateTime1.ToString(format);
          strArray1[4] = "# AND ";
          strArray1[5] = str;
          strArray1[6] = " <= #";
          dateTime1 = filter.DateTo;
          strArray1[7] = dateTime1.ToString(format);
          strArray1[8] = "#)";
          return string.Concat(strArray1);
        case OperatorTypes.DateNotBetween:
          string[] strArray2 = new string[9];
          strArray2[0] = "(";
          strArray2[1] = str;
          strArray2[2] = " < #";
          DateTime dateTime2 = filter.DateFrom;
          strArray2[3] = dateTime2.ToString(format);
          strArray2[4] = "# OR ";
          strArray2[5] = str;
          strArray2[6] = " > #";
          dateTime2 = filter.DateTo;
          strArray2[7] = dateTime2.ToString(format);
          strArray2[8] = "#)";
          return string.Concat(strArray2);
        case OperatorTypes.EmptyDate:
          return "IsEmpty([" + filter.FieldID + "])";
        case OperatorTypes.NotEmptyDate:
          return "Not IsEmpty([" + filter.FieldID + "])";
        default:
          throw new Exception("The specified operator (" + (object) filter.OperatorType + ") is invalid for a date field");
      }
    }

    private static string createMonthDayExpression(FieldFilter filter)
    {
      string str = "[@" + filter.FieldID + "]";
      string format = "M/d";
      switch (filter.OperatorType)
      {
        case OperatorTypes.IsExact:
        case OperatorTypes.Equals:
          return str + " = XMonthDay(\"" + filter.DateFrom.ToString(format) + "\")";
        case OperatorTypes.IsNot:
          return str + " <> XMonthDay(\"" + Utils.ParseDate((object) filter.ValueFrom).ToString(format) + "\")";
        case OperatorTypes.NotEqual:
          return str + " <> XMonthDay(\"" + filter.DateFrom.ToString(format) + "\")";
        case OperatorTypes.GreaterThan:
        case OperatorTypes.DateAfter:
          return str + " > XMonthDay(\"" + filter.DateFrom.ToString(format) + "\")";
        case OperatorTypes.NotGreaterThan:
        case OperatorTypes.DateOnOrBefore:
          return str + " <= XMonthDay(\"" + filter.DateFrom.ToString(format) + "\")";
        case OperatorTypes.LessThan:
        case OperatorTypes.DateBefore:
          return str + " < XMonthDay(\"" + filter.DateFrom.ToString(format) + "\")";
        case OperatorTypes.NotLessThan:
        case OperatorTypes.DateOnOrAfter:
          return str + " >= XMonthDay(\"" + filter.DateFrom.ToString(format) + "\")";
        case OperatorTypes.CurrentMonth:
        case OperatorTypes.DateBetween:
          return "(" + str + " >= XMonthDay(\"" + filter.DateFrom.ToString(format) + "\") AND " + str + " <= XMonthDay(\"" + filter.DateTo.ToString(format) + "\"))";
        case OperatorTypes.DateNotBetween:
          return "(" + str + " < XMonthDay(\"" + filter.DateFrom.ToString(format) + "\") OR " + str + " > XMonthDay(\"" + filter.DateTo.ToString(format) + "\"))";
        case OperatorTypes.EmptyDate:
          return "IsEmpty([" + filter.FieldID + "])";
        case OperatorTypes.NotEmptyDate:
          return "Not IsEmpty([" + filter.FieldID + "])";
        default:
          throw new Exception("The specified operator (" + (object) filter.OperatorType + ") is invalid for a date field");
      }
    }

    private static string createNumericExpression(FieldFilter filter)
    {
      string str = "[#" + filter.FieldID + "]";
      switch (filter.OperatorType)
      {
        case OperatorTypes.IsNot:
        case OperatorTypes.NotEqual:
          return str + " <> " + filter.ValueFrom;
        case OperatorTypes.Equals:
          return str + " = " + filter.ValueFrom;
        case OperatorTypes.GreaterThan:
          return str + " > " + filter.ValueFrom;
        case OperatorTypes.NotGreaterThan:
          return str + " <= " + filter.ValueFrom;
        case OperatorTypes.LessThan:
          return str + " < " + filter.ValueFrom;
        case OperatorTypes.NotLessThan:
          return str + " >= " + filter.ValueFrom;
        case OperatorTypes.Between:
          return "(" + str + " >= " + filter.ValueFrom + " AND " + str + " <= " + filter.ValueTo + ")";
        case OperatorTypes.NotBetween:
          return "(" + str + " < " + filter.ValueFrom + " OR " + str + " > " + filter.ValueTo + ")";
        default:
          throw new Exception("The specified operator (" + (object) filter.OperatorType + ") is invalid for a numeric field");
      }
    }

    private static string createStringExpression(FieldFilter filter)
    {
      string str = "[" + filter.FieldID + "]";
      switch (filter.OperatorType)
      {
        case OperatorTypes.IsExact:
        case OperatorTypes.Equals:
          return str + " = \"" + filter.ValueFrom + "\"";
        case OperatorTypes.IsNot:
        case OperatorTypes.NotEqual:
          return str + " <> \"" + filter.ValueFrom + "\"";
        case OperatorTypes.StartsWith:
          return str + ".StartsWith(\"" + filter.ValueFrom + "\")";
        case OperatorTypes.DoesnotStartWith:
          return "NOT " + str + ".StartsWith(\"" + filter.ValueFrom + "\")";
        case OperatorTypes.Contains:
          return str + ".Contains(\"" + filter.ValueFrom + "\")";
        case OperatorTypes.DoesnotContain:
          return "NOT " + str + ".Contains(\"" + filter.ValueFrom + "\")";
        default:
          throw new Exception("The specified operator (" + (object) filter.OperatorType + ") is invalid for a string field");
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.panel1 = new Panel();
      this.ctlAdvSearch = new AdvancedSearchControl();
      this.dlgButtons = new DialogButtons();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.panel1.Controls.Add((Control) this.ctlAdvSearch);
      this.panel1.Dock = DockStyle.Fill;
      this.panel1.Location = new Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(687, 333);
      this.panel1.TabIndex = 0;
      this.ctlAdvSearch.AllowDatabaseFieldsOnly = false;
      this.ctlAdvSearch.AllowDynamicOperators = false;
      this.ctlAdvSearch.AllowVirtualFields = false;
      this.ctlAdvSearch.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.ctlAdvSearch.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.ctlAdvSearch.Location = new Point(10, 10);
      this.ctlAdvSearch.Name = "ctlAdvSearch";
      this.ctlAdvSearch.Size = new Size(666, 320);
      this.ctlAdvSearch.TabIndex = 0;
      this.ctlAdvSearch.Title = "Filters";
      this.dlgButtons.DialogResult = DialogResult.OK;
      this.dlgButtons.Dock = DockStyle.Bottom;
      this.dlgButtons.Location = new Point(0, 333);
      this.dlgButtons.Name = "dlgButtons";
      this.dlgButtons.Size = new Size(687, 47);
      this.dlgButtons.TabIndex = 1;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(687, 380);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.dlgButtons);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.MinimizeBox = false;
      this.Name = nameof (AdvConditionEditor);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Advanced Condition Editor";
      this.panel1.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
