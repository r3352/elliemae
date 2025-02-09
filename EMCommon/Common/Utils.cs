// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Utils
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using EllieMae.EMLite.RemotingServices;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using System.Xml.XPath;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public static class Utils
  {
    private const string APP_NAME = "Encompass";
    public const string APPLICATION_UI_NAME = "Encompass";
    private static string sw = Tracing.SwCommon;
    private static IFormatProvider s_StandardDateFormatProvider;
    private static int s_LastTwoDigitYearMax = 0;
    public const string RESPA_TILA_2015_LE_AND_CD = "RESPA-TILA 2015 LE and CD";
    private const string TILA_RESPA_2015_LE_AND_CD = "TILA-RESPA 2015 LE and CD";
    public const string URLA_2020 = "URLA 2020";
    public const string URLA_2009 = "URLA 2009";
    private static readonly System.TimeZoneInfo PacificStandardTimeZone = System.TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
    public static readonly bool EnableReadReplicaForModule = true;
    private static readonly Dictionary<string, System.TimeZoneInfo> TimeZones = new Dictionary<string, System.TimeZoneInfo>()
    {
      {
        "HST",
        System.TimeZoneInfo.FindSystemTimeZoneById("Hawaiian Standard Time")
      },
      {
        "AKT",
        System.TimeZoneInfo.FindSystemTimeZoneById("Alaskan Standard Time")
      },
      {
        "AKST",
        System.TimeZoneInfo.FindSystemTimeZoneById("Alaskan Standard Time")
      },
      {
        "AKDT",
        System.TimeZoneInfo.FindSystemTimeZoneById("Alaskan Standard Time")
      },
      {
        "PT",
        System.TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time")
      },
      {
        "PST",
        System.TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time")
      },
      {
        "PDT",
        System.TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time")
      },
      {
        "MT",
        System.TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time")
      },
      {
        "MST",
        System.TimeZoneInfo.FindSystemTimeZoneById("US Mountain Standard Time")
      },
      {
        "MDT",
        System.TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time")
      },
      {
        "CT",
        System.TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time")
      },
      {
        "CST",
        System.TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time")
      },
      {
        "CDT",
        System.TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time")
      },
      {
        "ET",
        System.TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time")
      },
      {
        "EST",
        System.TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time")
      },
      {
        "EDT",
        System.TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time")
      }
    };
    private static readonly List<string> GenericTimeZones = new List<string>()
    {
      "HST",
      "AKT",
      "PT",
      "MST",
      "MT",
      "CT",
      "ET"
    };
    private static string result = string.Empty;
    private static Dictionary<string, string> excludeForNonPiggibackSync = new Dictionary<string, string>()
    {
      {
        "2",
        ""
      },
      {
        "136",
        ""
      },
      {
        "138",
        ""
      },
      {
        "140",
        ""
      },
      {
        "141",
        ""
      },
      {
        "142",
        ""
      },
      {
        "143",
        ""
      },
      {
        "202",
        ""
      },
      {
        "967",
        ""
      },
      {
        "968",
        ""
      },
      {
        "969",
        ""
      },
      {
        "1045",
        ""
      },
      {
        "1073",
        ""
      },
      {
        "1091",
        ""
      },
      {
        "1092",
        ""
      },
      {
        "1093",
        ""
      },
      {
        "1095",
        ""
      },
      {
        "1106",
        ""
      },
      {
        "1109",
        ""
      },
      {
        "1115",
        ""
      },
      {
        "1646",
        ""
      },
      {
        "1647",
        ""
      },
      {
        "1844",
        ""
      },
      {
        "1845",
        ""
      },
      {
        "1851",
        ""
      },
      {
        "1852",
        ""
      }
    };
    private static HashSet<string> indexRateFields = new HashSet<string>()
    {
      "688",
      "CORRESPONDENT.X104",
      "4513",
      "SERVICE.X16",
      "RE88395.X313",
      "S32DISC.X150"
    };
    private static Hashtable stateNameTbl;
    private static Hashtable stateAbbrTbl;
    public static DateTime DbMinDate = new DateTime(1753, 1, 1);
    public static DateTime DbMaxDate = new DateTime(9999, 12, 31);
    public static DateTime DbMinSmallDate = new DateTime(1900, 1, 1);
    public static DateTime DbMaxSmallDate = new DateTime(2079, 6, 6);
    public static string UTCDbMaxDate = new DateTime(9999, 12, 31, 23, 59, 59, DateTimeKind.Utc).ToString("u");
    public static string CurrentTimeZoneName;
    public static List<string> TPOEntityTypes = new List<string>()
    {
      "",
      "Individual",
      "Sole Proprietorship",
      "Partnership",
      "Corporation",
      "Limited Liability Company",
      "Other (please specify)"
    };
    public static List<string> GlobalSearchFields = new List<string>()
    {
      "Loan Number",
      "Borrower First Name",
      "Loan Name",
      "Borrower Last Name",
      "Subject Property Address"
    };
    private static List<string> defaultAllowedRAStrings = new List<string>()
    {
      "na",
      "n/a",
      "Not Applicable",
      "Not appl",
      "Not appl.",
      "incl",
      "incl.",
      "Included",
      "Exempt"
    };
    private const string Validnumbers = "0123456789";
    public static Dictionary<int, string> LenderObligatedFee_IndicatorFields = new Dictionary<int, string>()
    {
      {
        834,
        "NEWHUD2.X4780"
      },
      {
        835,
        "NEWHUD2.X4781"
      },
      {
        1115,
        "NEWHUD2.X4782"
      },
      {
        1116,
        "NEWHUD2.X4783"
      },
      {
        1209,
        "NEWHUD2.X4784"
      },
      {
        1210,
        "NEWHUD2.X4785"
      }
    };
    private static Dictionary<int, string> LenderObligatedFee_BorrowerAmountFields = new Dictionary<int, string>()
    {
      {
        834,
        "554"
      },
      {
        835,
        "NEWHUD.X657"
      },
      {
        1115,
        "NEWHUD.X1604"
      },
      {
        1116,
        "NEWHUD.X1612"
      },
      {
        1209,
        "NEWHUD.X1620"
      },
      {
        1210,
        "NEWHUD.X1627"
      }
    };

    public static IFormatProvider StandardDateFormatProvider
    {
      get
      {
        int num = DateTime.Now.Year + 30;
        if (Utils.s_LastTwoDigitYearMax != num)
        {
          Utils.s_LastTwoDigitYearMax = num;
          CultureInfo specificCulture = CultureInfo.CreateSpecificCulture("en-us");
          specificCulture.Calendar.TwoDigitYearMax = num;
          Utils.s_StandardDateFormatProvider = specificCulture.DateTimeFormat.GetFormat(typeof (DateTimeFormatInfo)) as IFormatProvider;
        }
        return Utils.s_StandardDateFormatProvider;
      }
    }

    public static string Result => Utils.result;

    public static DialogResult Dialog(IWin32Window owner, string text)
    {
      return Utils.displayUtilsDialog(owner, text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
    }

    public static DialogResult Dialog(
      IWin32Window owner,
      string text,
      MessageBoxButtons buttons,
      MessageBoxIcon icon)
    {
      return Utils.displayUtilsDialog(owner, text, buttons, icon, MessageBoxDefaultButton.Button1);
    }

    public static DialogResult Dialog(
      IWin32Window owner,
      string text,
      MessageBoxButtons buttons,
      MessageBoxIcon icon,
      MessageBoxDefaultButton defaultButton)
    {
      return Utils.displayUtilsDialog(owner, text, buttons, icon, defaultButton);
    }

    public static DialogResult Dialog(
      IWin32Window owner,
      string text,
      string[] customButtonList,
      MessageBoxIcon icon)
    {
      return Utils.displayUtilsDialog(owner, text, MessageBoxButtons.OK, icon, MessageBoxDefaultButton.Button1, true, customButtonList);
    }

    private static DialogResult displayUtilsDialog(
      IWin32Window owner,
      string text,
      MessageBoxButtons buttons,
      MessageBoxIcon icon,
      MessageBoxDefaultButton defaultButton,
      bool showCustomForm = false,
      string[] buttonList = null)
    {
      Form activeForm = Form.ActiveForm;
      if (activeForm != null && !activeForm.IsDisposed && activeForm.IsHandleCreated && activeForm.InvokeRequired)
      {
        Utils.UtilsDialogCallback method = new Utils.UtilsDialogCallback(Utils.displayUtilsDialog);
        try
        {
          return (DialogResult) activeForm.Invoke((Delegate) method, (object) owner, (object) text, (object) buttons, (object) icon, (object) defaultButton);
        }
        catch
        {
        }
      }
      if (!showCustomForm)
        return MessageBox.Show(owner, text, "Encompass", buttons, icon, defaultButton);
      CustomMessageBox customMessageBox = new CustomMessageBox(icon, text, buttonList);
      DialogResult dialogResult = customMessageBox.ShowDialog();
      if (dialogResult == DialogResult.OK)
        Utils.result = customMessageBox.Result;
      return dialogResult;
    }

    public static DialogResult ShowChekboxDialog(
      string content,
      string chkboxtext,
      string dialogcaption)
    {
      Form prompt = new Form();
      prompt.AutoSize = false;
      prompt.Size = new Size(626, 200);
      prompt.Text = dialogcaption;
      prompt.StartPosition = FormStartPosition.CenterScreen;
      Panel panel = new Panel();
      Button btnOk = new Button();
      Button button = new Button();
      CheckBox chkAcceptance = new CheckBox();
      Label label = new Label();
      btnOk.Location = new Point(411, 122);
      btnOk.Name = "btnOk";
      btnOk.Size = new Size(75, 23);
      btnOk.TabIndex = 2;
      btnOk.Text = "OK";
      btnOk.UseVisualStyleBackColor = true;
      btnOk.Click += (EventHandler) ((sender, e) =>
      {
        prompt.DialogResult = DialogResult.OK;
        prompt.Close();
      });
      btnOk.Enabled = false;
      button.Location = new Point(510, 122);
      button.Name = "btnCancel";
      button.Size = new Size(75, 23);
      button.TabIndex = 3;
      button.Text = "Cancel";
      button.UseVisualStyleBackColor = true;
      button.Click += (EventHandler) ((sender, e) =>
      {
        prompt.DialogResult = DialogResult.Cancel;
        prompt.Close();
      });
      chkAcceptance.AutoSize = true;
      chkAcceptance.Location = new Point(18, 97);
      chkAcceptance.Name = "chkAcceptance";
      chkAcceptance.Size = new Size(179, 17);
      chkAcceptance.TabIndex = 1;
      chkAcceptance.Text = chkboxtext;
      chkAcceptance.UseVisualStyleBackColor = true;
      chkAcceptance.CheckedChanged += (EventHandler) ((sender, e) => btnOk.Enabled = chkAcceptance.Checked);
      label.Location = new Point(15, 10);
      label.Name = "lblMessage";
      label.Text = content;
      label.AutoSize = true;
      label.MaximumSize = new Size(592, 0);
      panel.Controls.Add((Control) btnOk);
      panel.Controls.Add((Control) button);
      panel.Controls.Add((Control) chkAcceptance);
      panel.Controls.Add((Control) label);
      panel.Location = new Point(12, 9);
      panel.Name = "panelBase";
      panel.AutoSize = true;
      panel.Dock = DockStyle.Fill;
      prompt.Controls.Add((Control) panel);
      panel.ResumeLayout(false);
      panel.PerformLayout();
      prompt.ResumeLayout(false);
      int num = (int) prompt.ShowDialog();
      return prompt.DialogResult;
    }

    private static string ValidateDecimal(int precision, string val)
    {
      int length = val.Length;
      int num1 = 0;
      while (num1 < length)
      {
        char ch = val[num1];
        switch (ch)
        {
          case '-':
            if (num1 != 0)
            {
              val = val.Remove(num1, 1);
              --length;
              continue;
            }
            ++num1;
            continue;
          case '.':
            if (val.IndexOf('.') != val.LastIndexOf('.'))
            {
              val = val.Remove(num1, 1);
              --length;
              continue;
            }
            ++num1;
            continue;
          default:
            if (ch < '0' || ch > '9')
            {
              val = val.Remove(num1, 1);
              --length;
              continue;
            }
            ++num1;
            continue;
        }
      }
      int num2 = val.IndexOf('.');
      if (num2 >= 0 && precision > 0 && length - num2 - precision - 1 > 0)
        val = val.Substring(0, num2 + precision + 1);
      return val;
    }

    public static bool ValidateEmail(string emailAddresses)
    {
      if (string.IsNullOrEmpty(emailAddresses))
        return false;
      string[] strArray = emailAddresses.Split(';');
      EmailAddressAttribute addressAttribute = new EmailAddressAttribute();
      foreach (string str in strArray)
      {
        if (!addressAttribute.IsValid((object) str.Trim()))
          return false;
      }
      return true;
    }

    public static bool ValidatePhoneNumber(string phoneNumber)
    {
      return Regex.IsMatch(phoneNumber.Trim(), "^(1\\s*[-\\/\\.]?)?(\\((\\d{3})\\)|(\\d{3}))\\s*[-\\/\\.]?\\s*(\\d{3})\\s*[-\\/\\.]?\\s*(\\d{4})\\s*(([xX]|[eE][xX][tT])?\\s*(\\d{1,4}))?$");
    }

    public static string FormatMERS(string orgval)
    {
      string str = string.Empty;
      for (int index = 0; index < orgval.Length; ++index)
      {
        char ch = orgval[index];
        switch (ch)
        {
          case '0':
          case '1':
          case '2':
          case '3':
          case '4':
          case '5':
          case '6':
          case '7':
          case '8':
          case '9':
            str += ch.ToString();
            break;
        }
      }
      if (str.Length > 18)
        str = str.Substring(0, 18);
      return str;
    }

    public static string FormatInput(string orgval, FieldFormat dataFormat, ref bool needsUpdate)
    {
      if (orgval == null)
        return (string) null;
      string s = orgval;
      switch (dataFormat)
      {
        case FieldFormat.YN:
          if (s.Length <= 0)
          {
            s = string.Empty;
            break;
          }
          s = s.Substring(0, 1).ToUpper();
          if (s != "Y" && s != "N")
          {
            s = string.Empty;
            break;
          }
          break;
        case FieldFormat.X:
          if (s.Length <= 0)
          {
            s = string.Empty;
            break;
          }
          s = s.Substring(0, 1).ToUpper();
          if (s != "X")
          {
            s = string.Empty;
            break;
          }
          break;
        case FieldFormat.ZIPCODE:
          s = s.Replace("-", string.Empty);
          int length1 = s.Length;
          int num1 = 0;
          while (num1 < length1)
          {
            char ch = s[num1];
            if (ch > '9' || ch < '0')
            {
              s = s.Remove(num1, 1);
              --length1;
            }
            else
              ++num1;
          }
          if (length1 > 9)
            s = s.Substring(0, 9);
          if (length1 > 5)
          {
            s = s.Insert(5, "-");
            break;
          }
          break;
        case FieldFormat.STATE:
          s = s.ToUpper();
          int length2 = s.Length;
          int num2 = 0;
          while (num2 < length2)
          {
            char ch = s[num2];
            if (ch < 'A' || ch > 'Z')
            {
              s = s.Remove(num2, 1);
              --length2;
            }
            else
              ++num2;
          }
          if (length2 > 2)
          {
            s = s.Substring(0, 2);
            break;
          }
          break;
        case FieldFormat.PHONE:
          s = s.Replace("-", string.Empty).Replace(" ", string.Empty);
          int length3 = s.Length;
          int num3 = 0;
          while (num3 < length3)
          {
            if (!char.IsDigit(s[num3]))
            {
              s = s.Remove(num3, 1);
              --length3;
            }
            else
              ++num3;
          }
          if (length3 > 14)
            s = s.Substring(0, 14);
          if (length3 > 3)
          {
            s = s.Insert(3, "-");
            if (length3 > 6)
            {
              s = s.Insert(7, "-");
              if (length3 > 10)
              {
                s = s.Insert(12, " ");
                break;
              }
              break;
            }
            break;
          }
          break;
        case FieldFormat.SSN:
          s = s.Replace("-", string.Empty);
          int length4 = s.Length;
          int num4 = 0;
          while (num4 < length4)
          {
            char ch = s[num4];
            if (ch > '9' || ch < '0')
            {
              s = s.Remove(num4, 1);
              --length4;
            }
            else
              ++num4;
          }
          if (length4 > 9)
            s = s.Substring(0, 9);
          if (length4 >= 3)
          {
            s = s.Insert(3, "-");
            if (length4 >= 5)
            {
              s = s.Insert(6, "-");
              break;
            }
            break;
          }
          break;
        case FieldFormat.INTEGER:
          orgval = orgval.Replace(",", string.Empty);
          s = orgval.IndexOf('.') > -1 ? orgval.Substring(0, orgval.IndexOf('.')) : orgval;
          int length5 = s.Length;
          int num5 = 0;
          while (num5 < length5)
          {
            char ch = s[num5];
            if (ch == '-')
            {
              if (num5 != 0)
              {
                s = s.Remove(num5, 1);
                --length5;
              }
              else
                ++num5;
            }
            else if (ch < '0' || ch > '9')
            {
              s = s.Remove(num5, 1);
              --length5;
            }
            else
              ++num5;
          }
          break;
        case FieldFormat.DECIMAL_1:
          orgval = orgval.Replace(",", string.Empty);
          s = Utils.ValidateDecimal(1, orgval);
          break;
        case FieldFormat.DECIMAL_2:
          orgval = orgval.Replace(",", string.Empty);
          s = Utils.ValidateDecimal(2, orgval);
          break;
        case FieldFormat.DECIMAL_3:
          orgval = orgval.Replace(",", string.Empty);
          s = Utils.ValidateDecimal(3, orgval);
          break;
        case FieldFormat.DECIMAL_4:
          orgval = orgval.Replace(",", string.Empty);
          s = Utils.ValidateDecimal(4, orgval);
          break;
        case FieldFormat.DECIMAL_6:
          orgval = orgval.Replace(",", string.Empty);
          s = Utils.ValidateDecimal(6, orgval);
          break;
        case FieldFormat.DECIMAL_5:
          orgval = orgval.Replace(",", string.Empty);
          s = Utils.ValidateDecimal(5, orgval);
          break;
        case FieldFormat.DECIMAL_7:
          orgval = orgval.Replace(",", string.Empty);
          s = Utils.ValidateDecimal(7, orgval);
          break;
        case FieldFormat.DECIMAL:
          orgval = orgval.Replace(",", string.Empty);
          s = Utils.ValidateDecimal(0, orgval);
          break;
        case FieldFormat.DECIMAL_10:
          orgval = orgval.Replace(",", string.Empty);
          s = Utils.ValidateDecimal(10, orgval);
          break;
        case FieldFormat.DECIMAL_9:
          orgval = orgval.Replace(",", string.Empty);
          s = Utils.ValidateDecimal(9, orgval);
          break;
        case FieldFormat.DATE:
        case FieldFormat.MONTHDAY:
          int length6 = s.Length;
          if (length6 > 0)
          {
            char ch = s[length6 - 1];
            if (ch == '/')
            {
              if (length6 == 2 && s != "//")
                s = s.Insert(0, "0");
              else if (length6 == 5)
                s = s.Insert(3, "0");
              else if (length6 > 6)
                s = s.Substring(0, length6 - 1);
            }
            else if (ch < '0' || ch > '9')
              s = s.Substring(0, length6 - 1);
            s = s.Replace("/", string.Empty);
            int length7 = s.Length;
            try
            {
              int.Parse(s);
              if (length7 > 8)
                s = s.Substring(0, 8);
              if (length7 >= 2)
              {
                s = s.Insert(2, "/");
                if (length7 >= 4)
                  s = s.Insert(5, "/");
              }
              if (dataFormat == FieldFormat.MONTHDAY && s.Length > 5)
                s = s.Substring(0, 5);
              bool flag;
              do
              {
                try
                {
                  length7 = s.Length;
                  if (length7 >= 2 && length7 <= 6 && length7 != 4)
                    DateTime.Parse(s + "12/01/1980".Substring(s.Length));
                  if (length7 >= 10)
                    DateTime.Parse(s);
                  flag = false;
                }
                catch (Exception ex)
                {
                  flag = true;
                  s = s.Substring(0, length7 - 1);
                }
              }
              while (flag);
            }
            catch (Exception ex)
            {
              s = string.Empty;
            }
            if (s.StartsWith(orgval) && s != orgval && s.Substring(orgval.Length) == "/")
            {
              s = orgval;
              break;
            }
            break;
          }
          break;
      }
      needsUpdate = s != orgval;
      return s;
    }

    public static string FormatInput(
      string orgval,
      FieldFormat dataFormat,
      ref bool needsUpdate,
      int orgCursorPos,
      ref int newCursorPos)
    {
      string newVal = Utils.FormatInput(orgval, dataFormat, ref needsUpdate);
      newCursorPos = Utils.GetNewCursorPosition(orgval, newVal, orgCursorPos, new string[3]
      {
        " ",
        "-",
        ","
      });
      return newVal;
    }

    public static string FormatInput(
      string orgval,
      FieldFormat dataFormat,
      ref bool needsUpdate,
      int orgCursorPos,
      ref int newCursorPos,
      string[] ignoredStrings)
    {
      string newVal = Utils.FormatInput(orgval, dataFormat, ref needsUpdate);
      string[] ignoredStrings1 = new string[ignoredStrings.Length + 1];
      ignoredStrings.CopyTo((Array) ignoredStrings1, 1);
      ignoredStrings1[0] = string.Empty;
      newCursorPos = Utils.GetNewCursorPosition(orgval, newVal, orgCursorPos, ignoredStrings1);
      return newVal;
    }

    public static string FormatLEAndCDPercentageValue(string orgval)
    {
      if (string.IsNullOrEmpty(orgval))
        return string.Empty;
      return Utils.IsInt((object) orgval) ? Utils.RemoveEndingZeros(orgval) : Utils.ParseDecimal((object) orgval).ToString("N3");
    }

    private static string removeStrings(string orgStr, string[] tokens)
    {
      foreach (string token in tokens)
        orgStr = orgStr.Replace(token, string.Empty);
      return orgStr;
    }

    public static string FormatDateValue(string val) => Utils.FormatDateValue(val, false);

    public static string FormatDateValue(string val, bool includeTime)
    {
      if (val.Length > 0 && val.Length < 4 && val != "//")
        throw new FormatException("Date format is invalid.");
      if (val.Length == 0 || val == "//")
        return "//";
      if (val.Length == 4 && Utils.ParseInt((object) val, -1) > 0)
        val = "01/01/" + val;
      if (val.Length < 6)
        throw new FormatException("Date format is invalid.");
      if (Utils.ParseInt((object) val, -1) > 0)
        val = val.Substring(0, 2) + "/" + val.Substring(2, 2) + "/" + val.Substring(4);
      DateTime date = Utils.ParseDate((object) val, true);
      if (date.Year < 1900 || date.Year > 2199)
        throw new FormatException("Date year range must be between 1900 and 2199.");
      if (includeTime && date.Second > 0)
        return date.ToString("MM/dd/yyyy hh:mm:ss tt");
      return includeTime ? date.ToString("MM/dd/yyyy hh:mm tt") : date.ToString("MM/dd/yyyy");
    }

    public static string FormatMonthDayValue(string val)
    {
      if (val.Length > 0 && val.Length < 3 && val != "/")
        throw new FormatException("Month/Day format is invalid");
      return val.Length == 0 || val == "/" ? "/" : Utils.ParseMonthDay((object) val, true).ToString("MM/dd");
    }

    public static string ApplyFieldFormatting(string value, FieldFormat format)
    {
      return Utils.ApplyFieldFormatting(value, format, true);
    }

    public static string ApplyFieldFormatting(
      string value,
      FieldFormat format,
      bool formatEmptyValues)
    {
      try
      {
        if (!formatEmptyValues && value == "")
          return "";
        switch (format)
        {
          case FieldFormat.PHONE:
          case FieldFormat.SSN:
            bool needsUpdate = false;
            return Utils.FormatInput(value, format, ref needsUpdate);
          case FieldFormat.INTEGER:
            if (value != "")
            {
              Decimal num = Decimal.Parse(value, NumberStyles.Any, (IFormatProvider) null);
              return num == 0M && value == "0" ? "0" : num.ToString("#,#");
            }
            break;
          case FieldFormat.DECIMAL_1:
            if (value != "")
              return Decimal.Parse(value, NumberStyles.Any, (IFormatProvider) null).ToString("N1");
            break;
          case FieldFormat.DECIMAL_2:
            if (value != "")
              return Decimal.Parse(value, NumberStyles.Any, (IFormatProvider) null).ToString("N2");
            break;
          case FieldFormat.DECIMAL_3:
            if (value != "")
              return Decimal.Parse(value, NumberStyles.Any, (IFormatProvider) null).ToString("N3");
            break;
          case FieldFormat.DECIMAL_4:
            if (value != "")
              return Decimal.Parse(value, NumberStyles.Any, (IFormatProvider) null).ToString("N4");
            break;
          case FieldFormat.DECIMAL_6:
            if (value != "")
              return Decimal.Parse(value, NumberStyles.Any, (IFormatProvider) null).ToString("N6");
            break;
          case FieldFormat.DECIMAL_5:
            if (value != "")
              return Decimal.Parse(value, NumberStyles.Any, (IFormatProvider) null).ToString("N5");
            break;
          case FieldFormat.DECIMAL_7:
            if (value != "")
              return Decimal.Parse(value, NumberStyles.Any, (IFormatProvider) null).ToString("N7");
            break;
          case FieldFormat.DECIMAL:
            if (value != "")
              return Decimal.Parse(value, NumberStyles.Any, (IFormatProvider) null).ToString();
            break;
          case FieldFormat.DECIMAL_10:
            if (value != "")
              return Decimal.Parse(value, NumberStyles.Any, (IFormatProvider) null).ToString("N10");
            break;
          case FieldFormat.DATE:
            return value == "" ? "//" : Utils.FormatDateValue(value);
          case FieldFormat.MONTHDAY:
            return value == "" ? "/" : Utils.FormatMonthDayValue(value);
          case FieldFormat.DATETIME:
            return value == "" ? "//" : Utils.FormatDateValue(value, true);
        }
        return value;
      }
      catch
      {
        return "";
      }
    }

    public static object ConvertToNativeValue(string value, FieldFormat format, bool throwOnError)
    {
      switch (format)
      {
        case FieldFormat.YN:
          bool result;
          return bool.TryParse(value, out result) ? (object) Utils.ParseYN(result) : (object) value ?? (object) "";
        case FieldFormat.INTEGER:
          try
          {
            return (object) Utils.ParseInt((object) value, true);
          }
          catch
          {
            if (!throwOnError)
              return (object) 0;
            throw;
          }
        case FieldFormat.DECIMAL_1:
        case FieldFormat.DECIMAL_2:
        case FieldFormat.DECIMAL_3:
        case FieldFormat.DECIMAL_4:
        case FieldFormat.DECIMAL_6:
        case FieldFormat.DECIMAL_5:
        case FieldFormat.DECIMAL_7:
        case FieldFormat.DECIMAL:
        case FieldFormat.DECIMAL_10:
          return (object) Utils.ParseDecimal((object) value, throwOnError);
        case FieldFormat.DATE:
        case FieldFormat.DATETIME:
          return (object) Utils.ParseDate((object) value, throwOnError);
        case FieldFormat.MONTHDAY:
          return (object) Utils.ParseMonthDay((object) value, throwOnError);
        default:
          return (object) value ?? (object) "";
      }
    }

    private static string ParseYN(bool value) => !value ? "N" : "Y";

    public static object ConvertToNativeValue(
      string value,
      FieldFormat format,
      object defaultValue)
    {
      switch (format)
      {
        case FieldFormat.YN:
          bool result;
          return bool.TryParse(value, out result) ? (object) Utils.ParseYN(result) : (object) value ?? (object) "";
        case FieldFormat.INTEGER:
          return Utils.ParseIntInternal((object) value, false, defaultValue);
        case FieldFormat.DECIMAL_1:
        case FieldFormat.DECIMAL_2:
        case FieldFormat.DECIMAL_3:
        case FieldFormat.DECIMAL_4:
        case FieldFormat.DECIMAL_6:
        case FieldFormat.DECIMAL_5:
        case FieldFormat.DECIMAL_7:
        case FieldFormat.DECIMAL:
        case FieldFormat.DECIMAL_10:
          return Utils.ParseDecimalInternal((object) value, false, defaultValue);
        case FieldFormat.DATE:
        case FieldFormat.DATETIME:
          return Utils.ParseDateInternal((object) value, false, defaultValue);
        case FieldFormat.MONTHDAY:
          return Utils.ParseMonthDayInternal((object) value, false, defaultValue);
        default:
          return (object) value ?? (object) "";
      }
    }

    public static System.Type GetNativeValueType(FieldFormat format)
    {
      switch (format)
      {
        case FieldFormat.INTEGER:
          return typeof (int);
        case FieldFormat.DECIMAL_1:
        case FieldFormat.DECIMAL_2:
        case FieldFormat.DECIMAL_3:
        case FieldFormat.DECIMAL_4:
        case FieldFormat.DECIMAL_6:
        case FieldFormat.DECIMAL_5:
        case FieldFormat.DECIMAL_7:
        case FieldFormat.DECIMAL:
        case FieldFormat.DECIMAL_10:
          return typeof (Decimal);
        case FieldFormat.DATE:
        case FieldFormat.MONTHDAY:
          return typeof (DateTime);
        default:
          return typeof (string);
      }
    }

    public static bool IsEmptyValue(string value, FieldFormat format)
    {
      if ((value ?? "") == "")
        return true;
      if (format == FieldFormat.DATE)
        return value == "//";
      return format == FieldFormat.MONTHDAY && value == "/";
    }

    public static string UnformatValue(string value, FieldFormat format)
    {
      if ((value ?? "") == "")
        return "";
      value = Utils.ConvertToLoanInternalValue(value, format);
      switch (format)
      {
        case FieldFormat.ZIPCODE:
          return Utils.RemoveChars(value, "-");
        case FieldFormat.PHONE:
          return Utils.RemoveChars(value, "() -");
        case FieldFormat.SSN:
          return Utils.RemoveChars(value, "-");
        case FieldFormat.DATE:
          if (value == "//")
            return "";
          break;
        case FieldFormat.MONTHDAY:
          if (value == "/")
            return "";
          break;
      }
      return value;
    }

    public static string RemoveChars(string value, string charsToRemove)
    {
      for (int index = 0; index < charsToRemove.Length; ++index)
        value = value.Replace(charsToRemove[index].ToString() ?? "", "");
      return value;
    }

    public static string RemoveEndingZeros(string value) => Utils.RemoveEndingZeros(value, false);

    public static string RemoveEndingZeros(string value, bool roundingFirst)
    {
      if ((value ?? "") == string.Empty)
        return value;
      if (roundingFirst)
        value = string.Concat((object) Utils.ArithmeticRounding(Utils.ParseDouble((object) value), 0));
      if (value.IndexOf(".") == -1)
        return value;
      for (; value.EndsWith("0") || value.EndsWith("."); value = value.Substring(0, value.Length - 1))
      {
        if (value.EndsWith("."))
          return value.Substring(0, value.Length - 1);
      }
      return value;
    }

    public static string Remove2EndingZeros(string value, bool roundingFirst)
    {
      if ((value ?? "") == string.Empty)
        return value;
      if (roundingFirst)
        value = string.Concat((object) Utils.ArithmeticRounding(Utils.ParseDouble((object) value), 0));
      if (value.IndexOf(".") == -1)
        return value;
      for (int index = 0; (value.EndsWith("0") || value.EndsWith(".")) && index < 2; value = value.Substring(0, value.Length - 1))
      {
        if (value.EndsWith("."))
          return value.Substring(0, value.Length - 1);
        ++index;
      }
      return value;
    }

    public static string ConvertToLoanInternalValue(string value, FieldFormat format)
    {
      if (value == "")
        return "";
      switch (format)
      {
        case FieldFormat.YN:
          if (value.ToLower() == "y" || value.ToLower() == "yes")
            return "Y";
          return value.ToLower() == "n" || value.ToLower() == "no" ? "N" : value;
        case FieldFormat.PHONE:
        case FieldFormat.SSN:
          bool needsUpdate = false;
          return Utils.FormatInput(value, format, ref needsUpdate);
        case FieldFormat.INTEGER:
          return value == "0" ? value : Utils.ParseDecimal((object) value, true).ToString("#");
        case FieldFormat.DECIMAL_1:
          return Utils.ParseDecimal((object) value, true).ToString("F1");
        case FieldFormat.DECIMAL_2:
          return Utils.ParseDecimal((object) value, true).ToString("F2");
        case FieldFormat.DECIMAL_3:
          return Utils.ParseDecimal((object) value, true).ToString("F3");
        case FieldFormat.DECIMAL_4:
          return Utils.ParseDecimal((object) value, true).ToString("F4");
        case FieldFormat.DECIMAL_6:
          return Utils.ParseDecimal((object) value, true).ToString("F6");
        case FieldFormat.DECIMAL_5:
          return Utils.ParseDecimal((object) value, true).ToString("F5");
        case FieldFormat.DECIMAL_7:
          return Utils.ParseDecimal((object) value, true).ToString("F7");
        case FieldFormat.DECIMAL:
          return Utils.ParseDecimal((object) value, true).ToString();
        case FieldFormat.DECIMAL_10:
          return Utils.ParseDecimal((object) value, true).ToString("F10");
        case FieldFormat.DATE:
          return Utils.FormatDateValue(value);
        case FieldFormat.MONTHDAY:
          return Utils.FormatMonthDayValue(value);
        case FieldFormat.DATETIME:
          return Utils.DateTimeToString(Utils.ParseDate((object) value));
        default:
          return value;
      }
    }

    public static int GetNewCursorPosition(
      string orgVal,
      string newVal,
      int currentPos,
      string[] ignoredStrings)
    {
      if (currentPos == orgVal.Length || currentPos == -1)
        return newVal.Length;
      if (currentPos == 0)
        return 0;
      string str1 = Utils.removeStrings(orgVal, ignoredStrings);
      string str2 = Utils.removeStrings(newVal, ignoredStrings);
      if (str1.Length > str2.Length)
        str1 = str1.Substring(0, str2.Length);
      string empty = string.Empty;
      string str3;
      if (str1 != str2)
      {
        int length = currentPos - 1 <= orgVal.Length ? currentPos - 1 : orgVal.Length;
        if (length < 0)
          length = 0;
        str3 = Utils.removeStrings(orgVal.Substring(0, length), ignoredStrings);
      }
      else
      {
        int length = currentPos <= orgVal.Length ? currentPos : orgVal.Length;
        if (length < 0)
          length = 0;
        str3 = Utils.removeStrings(orgVal.Substring(0, length), ignoredStrings);
      }
      int num = 0;
      int newCursorPosition = newVal.Length;
      for (int index = 0; index < newVal.Length; ++index)
      {
        bool flag = false;
        string str4 = newVal[index].ToString();
        foreach (string ignoredString in ignoredStrings)
        {
          if (str4 == ignoredString)
          {
            flag = true;
            break;
          }
        }
        if (!flag && (num == str3.Length || (int) newVal[index] != (int) str3[num++]))
        {
          newCursorPosition = index;
          break;
        }
      }
      return newCursorPosition;
    }

    public static string[] GetAdditionalTerritories()
    {
      return new string[6]
      {
        "AS",
        "FM",
        "MP",
        "MH",
        "PW",
        "UM"
      };
    }

    public static string[] GetStates(bool includeAdditionalTerritories)
    {
      string[] states;
      if (includeAdditionalTerritories)
        states = new string[60]
        {
          "AL",
          "AK",
          "AZ",
          "AR",
          "CA",
          "CO",
          "CT",
          "DE",
          "DC",
          "FL",
          "GA",
          "HI",
          "ID",
          "IL",
          "IN",
          "IA",
          "KS",
          "KY",
          "LA",
          "ME",
          "MD",
          "MA",
          "MI",
          "MN",
          "MS",
          "MO",
          "MT",
          "NE",
          "NV",
          "NH",
          "NJ",
          "NM",
          "NY",
          "NC",
          "ND",
          "OH",
          "OK",
          "OR",
          "PA",
          "RI",
          "SC",
          "SD",
          "TN",
          "TX",
          "UT",
          "VT",
          "VA",
          "WA",
          "WV",
          "WI",
          "WY",
          "VI",
          "GU",
          "PR",
          "AS",
          "FM",
          "MP",
          "MH",
          "PW",
          "UM"
        };
      else
        states = new string[54]
        {
          "AL",
          "AK",
          "AZ",
          "AR",
          "CA",
          "CO",
          "CT",
          "DE",
          "DC",
          "FL",
          "GA",
          "HI",
          "ID",
          "IL",
          "IN",
          "IA",
          "KS",
          "KY",
          "LA",
          "ME",
          "MD",
          "MA",
          "MI",
          "MN",
          "MS",
          "MO",
          "MT",
          "NE",
          "NV",
          "NH",
          "NJ",
          "NM",
          "NY",
          "NC",
          "ND",
          "OH",
          "OK",
          "OR",
          "PA",
          "RI",
          "SC",
          "SD",
          "TN",
          "TX",
          "UT",
          "VT",
          "VA",
          "WA",
          "WV",
          "WI",
          "WY",
          "VI",
          "GU",
          "PR"
        };
      return states;
    }

    public static string[] GetStates() => Utils.GetStates(false);

    public static string[] GetStatesWithBlankEntry()
    {
      return new string[55]
      {
        " ",
        "AL",
        "AK",
        "AZ",
        "AR",
        "CA",
        "CO",
        "CT",
        "DE",
        "DC",
        "FL",
        "GA",
        "HI",
        "ID",
        "IL",
        "IN",
        "IA",
        "KS",
        "KY",
        "LA",
        "ME",
        "MD",
        "MA",
        "MI",
        "MN",
        "MS",
        "MO",
        "MT",
        "NE",
        "NV",
        "NH",
        "NJ",
        "NM",
        "NY",
        "NC",
        "ND",
        "OH",
        "OK",
        "OR",
        "PA",
        "RI",
        "SC",
        "SD",
        "TN",
        "TX",
        "UT",
        "VT",
        "VA",
        "WA",
        "WV",
        "WI",
        "WY",
        "VI",
        "GU",
        "PR"
      };
    }

    public static string GetFullStateName(string stateName)
    {
      if (stateName == null)
        return string.Empty;
      if (stateName.Length != 2)
        return stateName;
      return Utils.stateNameTbl.Contains((object) stateName.ToUpper()) ? (string) Utils.stateNameTbl[(object) stateName.ToUpper()] : string.Empty;
    }

    public static List<string> GetFullStateNames()
    {
      List<string> fullStateNames = new List<string>();
      foreach (object key in (IEnumerable) Utils.stateNameTbl.Keys)
        fullStateNames.Add(string.Concat(Utils.stateNameTbl[key]));
      fullStateNames.Sort();
      return fullStateNames;
    }

    public static string GetStateAbbr(string stateName)
    {
      if (stateName == null)
        return string.Empty;
      if (stateName.Length <= 2)
        return stateName;
      return Utils.stateAbbrTbl.Contains((object) stateName.ToLower()) ? (string) Utils.stateAbbrTbl[(object) stateName.ToLower()] : string.Empty;
    }

    public static string TransformSettingTimezoneToStandardTimezone(
      string timezoneSetting,
      bool isDaylightSavingTime)
    {
      string standardTimezone;
      switch (timezoneSetting)
      {
        case "(UTC -05:00) Eastern Time":
          standardTimezone = !isDaylightSavingTime ? "EST" : "EDT";
          break;
        case "(UTC -06:00) Central Time":
          standardTimezone = !isDaylightSavingTime ? "CST" : "CDT";
          break;
        case "(UTC -07:00) Arizona Time":
          standardTimezone = !isDaylightSavingTime ? "MST" : "PDT";
          break;
        case "(UTC -07:00) Mountain Time":
          standardTimezone = !isDaylightSavingTime ? "MST" : "MDT";
          break;
        case "(UTC -08:00) Pacific Time":
          standardTimezone = !isDaylightSavingTime ? "PST" : "PDT";
          break;
        case "(UTC -09:00) Alaska Time":
          standardTimezone = !isDaylightSavingTime ? "AKST" : "AKDT";
          break;
        case "(UTC -10:00) Hawaii Time":
          standardTimezone = "HST";
          break;
        default:
          standardTimezone = !isDaylightSavingTime ? "EST" : "EDT";
          break;
      }
      return standardTimezone;
    }

    public static string TransformSettingTimezoneToCommonFormatTimezone(string timezoneSetting)
    {
      string commonFormatTimezone;
      switch (timezoneSetting)
      {
        case "(UTC -05:00) Eastern Time":
          commonFormatTimezone = "ET";
          break;
        case "(UTC -06:00) Central Time":
          commonFormatTimezone = "CT";
          break;
        case "(UTC -07:00) Arizona Time":
          commonFormatTimezone = "MST";
          break;
        case "(UTC -07:00) Mountain Time":
          commonFormatTimezone = "MT";
          break;
        case "(UTC -08:00) Pacific Time":
          commonFormatTimezone = "PT";
          break;
        case "(UTC -09:00) Alaska Time":
          commonFormatTimezone = "AKT";
          break;
        case "(UTC -10:00) Hawaii Time":
          commonFormatTimezone = "HST";
          break;
        default:
          commonFormatTimezone = "ET";
          break;
      }
      return commonFormatTimezone;
    }

    public static string TransformTimezoneToStandardTimezone(
      string timezone,
      bool isDaylightSavingTime)
    {
      string standardTimezone;
      switch (timezone)
      {
        case "AKT":
          standardTimezone = !isDaylightSavingTime ? "AKST" : "AKDT";
          break;
        case "CT":
          standardTimezone = !isDaylightSavingTime ? "CST" : "CDT";
          break;
        case "ET":
          standardTimezone = !isDaylightSavingTime ? "EST" : "EDT";
          break;
        case "HST":
          standardTimezone = "HST";
          break;
        case "MST":
          standardTimezone = "MST";
          break;
        case "MT":
          standardTimezone = !isDaylightSavingTime ? "MST" : "MDT";
          break;
        case "PT":
          standardTimezone = !isDaylightSavingTime ? "PST" : "PDT";
          break;
        default:
          standardTimezone = !isDaylightSavingTime ? "EST" : "EDT";
          break;
      }
      return standardTimezone;
    }

    public static System.TimeZoneInfo GetTimeZoneInfo(string timeZoneCode)
    {
      if (string.IsNullOrEmpty(timeZoneCode))
        return (System.TimeZoneInfo) null;
      System.TimeZoneInfo timeZoneInfo;
      return Utils.TimeZones.TryGetValue(timeZoneCode, out timeZoneInfo) ? timeZoneInfo : (System.TimeZoneInfo) null;
    }

    public static string GetValidGenericTimeZone(string timeZoneCode)
    {
      return Utils.GenericTimeZones.Contains(timeZoneCode) ? timeZoneCode : (string) null;
    }

    public static string DateFormat(DateTime date, string minValueText = "")
    {
      return !(date != DateTime.MinValue) ? minValueText : date.ToString("MM/dd/yyyy");
    }

    public static DateTime ParseDate(object value) => Utils.ParseDate(value, false);

    public static DateTime ParseDate(object value, DateTime defaultValue)
    {
      return Utils.ParseDate(value, false, defaultValue);
    }

    public static DateTime ParseDate(object value, bool throwOnError)
    {
      return Utils.ParseDate(value, throwOnError, DateTime.MinValue);
    }

    public static DateTime ParseDate(object value, bool throwOnError, DateTime defaultValue)
    {
      return (DateTime) Utils.ParseDateInternal(value, throwOnError, (object) defaultValue);
    }

    private static object ParseDateInternal(object value, bool throwOnError, object defaultValue)
    {
      DateTime returnValue;
      if (Utils.TryParseDate(value, out returnValue))
        return (object) returnValue;
      if (throwOnError)
        throw new FormatException("The value '" + value + "' could not be converted to a valid date.");
      return defaultValue;
    }

    public static bool TryParseDate(
      object value,
      out DateTime returnValue,
      bool preserveDateTimeKind = false)
    {
      returnValue = DateTime.MinValue;
      try
      {
        if (value is string str)
        {
          DateTimeStyles styles = DateTimeStyles.AllowWhiteSpaces;
          if (preserveDateTimeKind)
            styles |= DateTimeStyles.RoundtripKind;
          if (!string.IsNullOrEmpty(str))
          {
            if (!string.Equals(str, "//"))
            {
              DateTime result;
              if (DateTime.TryParse(str, Utils.StandardDateFormatProvider, styles, out result))
              {
                returnValue = result;
                return true;
              }
            }
          }
        }
        else if (value != null)
        {
          returnValue = Convert.ToDateTime(value);
          return true;
        }
      }
      catch (Exception ex)
      {
      }
      return false;
    }

    public static string ReformatEuropeanDate(string originalDate)
    {
      string[] strArray;
      if (originalDate.IndexOf("/") > -1)
      {
        strArray = originalDate.Split('/');
      }
      else
      {
        if (originalDate.IndexOf("-") <= -1)
          return originalDate;
        strArray = originalDate.Split('-');
      }
      if (strArray.Length != 3)
        return originalDate;
      string str1;
      string str2;
      string str3;
      if (strArray[0].Length == 4)
      {
        str1 = strArray[0];
        if (Utils.ParseInt((object) strArray[1]) > 12)
        {
          str2 = strArray[2];
          str3 = strArray[1];
        }
        else
        {
          str2 = strArray[1];
          str3 = strArray[2];
        }
      }
      else
      {
        if (strArray[2].Length != 4)
          return originalDate;
        str1 = strArray[2];
        if (Utils.ParseInt((object) strArray[0]) > 12)
        {
          str2 = strArray[1];
          str3 = strArray[0];
        }
        else
        {
          str2 = strArray[0];
          str3 = strArray[1];
        }
      }
      return str2 + "/" + str3 + "/" + str1;
    }

    public static bool IsDate(object value)
    {
      if (Utils.IsEmptyDate(value))
        return false;
      try
      {
        Utils.ParseDate(value, true);
        return true;
      }
      catch
      {
        return false;
      }
    }

    public static bool IsEmptyDate(object value)
    {
      return value is string && (string.IsNullOrEmpty((string) value) || (string) value == "//");
    }

    public static DateTime ParseMonthDay(object value) => Utils.ParseMonthDay(value, false);

    public static DateTime ParseMonthDay(object value, DateTime defaultValue)
    {
      return Utils.ParseMonthDay(value, false, defaultValue);
    }

    public static DateTime ParseMonthDay(object value, bool throwOnError)
    {
      return Utils.ParseMonthDay(value, throwOnError, new DateTime(2000, 1, 1));
    }

    public static DateTime ParseMonthDay(object value, bool throwOnError, DateTime defaultValue)
    {
      return (DateTime) Utils.ParseMonthDayInternal(value, throwOnError, (object) defaultValue);
    }

    private static object ParseMonthDayInternal(
      object value,
      bool throwOnError,
      object defaultValue)
    {
      DateTime returnValue;
      if (Utils.TryParseMonthDay(value, out returnValue))
        return (object) returnValue;
      if (throwOnError)
        throw new FormatException("Value '" + value + "' cannot be converted to a Month/Day value");
      return defaultValue;
    }

    public static bool TryParseMonthDay(object value, out DateTime returnValue)
    {
      returnValue = new DateTime(2000, 1, 1);
      DateTime returnValue1;
      if (Utils.TryParseDate(value, out returnValue1))
      {
        returnValue = new DateTime(2000, returnValue1.Month, returnValue1.Day);
        return true;
      }
      if (!Utils.TryParseDate((object) (value.ToString() + "/2000"), out returnValue1))
        return false;
      returnValue = returnValue1;
      return true;
    }

    public static bool IsMonthDay(object value)
    {
      try
      {
        Utils.ParseMonthDay(value, true);
        return true;
      }
      catch
      {
        return false;
      }
    }

    public static Decimal ParseDecimal(
      object value,
      Decimal defaultValue,
      int roundingDecimalPlaces)
    {
      try
      {
        return Utils.ParseDecimal(value, true, roundingDecimalPlaces);
      }
      catch
      {
        return defaultValue;
      }
    }

    public static Decimal ParseDecimal(object value, Decimal defaultValue)
    {
      return Utils.ParseDecimal(value, false, defaultValue);
    }

    public static Decimal ParseDecimal(object value, bool throwOnError, int roundingDecimalPlaces)
    {
      return Utils.ArithmeticRounding(Utils.ParseDecimal(value, throwOnError), roundingDecimalPlaces);
    }

    public static Decimal ParseDecimal(object value, bool throwOnError = false, Decimal defaultValue = 0M)
    {
      return (Decimal) Utils.ParseDecimalInternal(value, throwOnError, (object) defaultValue);
    }

    private static object ParseDecimalInternal(
      object value,
      bool throwOnError,
      object defaultValue)
    {
      Decimal returnValue;
      if (Utils.TryParseDecimal(value, out returnValue))
        return (object) returnValue;
      if (throwOnError)
        throw new FormatException("The value '" + value + "' cannot be converted to a numeric value.");
      return defaultValue;
    }

    public static bool TryParseDecimal(object value, out Decimal returnValue)
    {
      if (value is Decimal num)
      {
        returnValue = num;
        return true;
      }
      try
      {
        if (value is string s)
        {
          if (!string.IsNullOrEmpty(s))
            return Decimal.TryParse(s, NumberStyles.Any, (IFormatProvider) null, out returnValue);
          returnValue = 0M;
          return false;
        }
        if (value != null)
        {
          returnValue = Convert.ToDecimal(value);
          return true;
        }
      }
      catch
      {
      }
      returnValue = 0M;
      return false;
    }

    public static bool IsDecimal(object value)
    {
      if (value is string && string.IsNullOrEmpty((string) value))
        return false;
      try
      {
        Utils.ParseDecimal(value, true);
        return true;
      }
      catch
      {
        return false;
      }
    }

    public static double ParseDouble(object value, double defaultValue)
    {
      return Utils.ParseDouble(value, false, defaultValue);
    }

    public static double ParseDouble(object value, bool throwOnError = false, double defaultValue = 0.0)
    {
      try
      {
        if (value is string)
        {
          if (!string.IsNullOrEmpty((string) value))
          {
            double result;
            if (double.TryParse((string) value, NumberStyles.Any, (IFormatProvider) null, out result))
              return result;
          }
        }
        else if (value != null)
          return Convert.ToDouble(value);
      }
      catch
      {
      }
      if (throwOnError)
        throw new FormatException("The value '" + value + "' cannot be converted to a numeric value.");
      return defaultValue;
    }

    public static bool IsDouble(object value)
    {
      if (value is string && string.IsNullOrEmpty((string) value))
        return false;
      try
      {
        Utils.ParseDouble(value, true);
        return true;
      }
      catch
      {
        return false;
      }
    }

    public static float ParseSingle(object value, float defaultValue)
    {
      return Utils.ParseSingle(value, false, defaultValue);
    }

    public static float ParseSingle(object value, bool throwOnError = false, float defaultValue = 0.0f)
    {
      try
      {
        if (value is string)
        {
          if (!string.IsNullOrEmpty((string) value))
          {
            float result;
            if (float.TryParse((string) value, NumberStyles.Any, (IFormatProvider) null, out result))
              return result;
          }
        }
        else if (value != null)
          return Convert.ToSingle(value);
      }
      catch
      {
      }
      if (throwOnError)
        throw new FormatException("The value '" + value + "' cannot be converted to a numeric value.");
      return defaultValue;
    }

    public static long ParseLong(object value, long defaultValue)
    {
      return Utils.ParseLong(value, false, defaultValue);
    }

    public static long ParseLong(object value, bool throwOnError = false, long defaultValue = -1)
    {
      try
      {
        if (value is string)
        {
          if (!string.IsNullOrEmpty((string) value))
          {
            long result;
            if (long.TryParse(value.ToString(), NumberStyles.Any, (IFormatProvider) null, out result))
              return result;
          }
        }
        else if (value != null)
          return Convert.ToInt64(value);
      }
      catch
      {
      }
      if (throwOnError)
        throw new FormatException("The value '" + value + "' cannot be converted to a numeric value.");
      return defaultValue;
    }

    public static int ParseInt(object value, int defaultValue)
    {
      return Utils.ParseInt(value, false, defaultValue);
    }

    public static int ParseInt(object value, bool throwOnError = false, int defaultValue = -1)
    {
      int returnValue;
      if (Utils.TryParseInt(value, out returnValue))
        return returnValue;
      if (throwOnError)
        throw new FormatException("The value '" + value + "' cannot be converted to an integer.");
      return defaultValue;
    }

    public static object ParseIntInternal(object value, bool throwOnError, object defaultValue)
    {
      int returnValue;
      if (Utils.TryParseInt(value, out returnValue))
        return (object) returnValue;
      if (throwOnError)
        throw new FormatException("The value '" + value + "' cannot be converted to an integer.");
      return defaultValue;
    }

    public static bool TryParseInt(object value, out int returnValue)
    {
      try
      {
        if (value is string str)
        {
          if (!string.IsNullOrEmpty(str))
          {
            if (Utils.isIntegerChar(str[0]))
            {
              Decimal result;
              if (Decimal.TryParse(value.ToString(), NumberStyles.Any, (IFormatProvider) null, out result))
              {
                returnValue = (int) result;
                return true;
              }
            }
          }
        }
        else if (value != null)
        {
          returnValue = Convert.ToInt32(value);
          return true;
        }
      }
      catch
      {
      }
      returnValue = -1;
      return false;
    }

    private static bool isIntegerChar(char c) => char.IsDigit(c) || c == '-';

    public static bool IsInt(object value)
    {
      if (value is string && (string.IsNullOrEmpty((string) value) || !Utils.isIntegerChar(((string) value)[0])))
        return false;
      try
      {
        Utils.ParseInt(value, true);
        return true;
      }
      catch
      {
        return false;
      }
    }

    public static bool ParseBoolean(object value)
    {
      try
      {
        if (value is string str)
        {
          if (string.IsNullOrWhiteSpace(str))
            return false;
          switch (str[0])
          {
            case 'N':
            case 'n':
              return false;
            case 'X':
            case 'Y':
            case 'x':
            case 'y':
              return true;
            default:
              return !str.Equals("false", StringComparison.OrdinalIgnoreCase) && str.Equals("true", StringComparison.OrdinalIgnoreCase);
          }
        }
        else if (value != null)
          return Convert.ToBoolean(value);
      }
      catch
      {
      }
      return false;
    }

    public static string ParseTime(string value, bool throwOnError)
    {
      string message = "The string '" + value + "' is not a valid time format (hh:mm am/pm).";
      try
      {
        string upper = value.ToUpper();
        string str1 = string.Empty;
        if (upper.EndsWith("PM"))
          str1 = " PM";
        else if (upper.EndsWith("AM"))
          str1 = " AM";
        string str2 = upper.Replace("AM", "").Replace("PM", "").Replace(" ", "");
        if (str2.IndexOf(":") == -1)
          str2 += ":00";
        string[] strArray = str2.Split(':');
        string str3 = string.Empty;
        for (int index = 0; index < strArray.Length; ++index)
        {
          int num = Utils.ParseInt((object) strArray[index]);
          if (num == -1)
          {
            if (throwOnError)
              throw new FormatException(message);
            return (string) null;
          }
          if (index == 0 && (num > 24 || num < 0) || index == 1 && (num > 59 || num < 0) || index == 2 && (num > 59 || num < 0))
          {
            if (throwOnError)
              throw new FormatException(message);
            return (string) null;
          }
          if (index == 0)
          {
            if (str1 == string.Empty)
              str1 = num < 12 ? " AM" : " PM";
            else if (num > 11 && str1 == " AM")
              str1 = " PM";
          }
          str3 = str3 + (str3 != string.Empty ? ":" : "") + strArray[index];
        }
        return str3 + str1;
      }
      catch (Exception ex)
      {
        if (throwOnError)
          throw new FormatException(message);
        return (string) null;
      }
    }

    public static bool IsAlphaNumeric(this string theString)
    {
      return !string.IsNullOrEmpty(theString) && new Regex("^[a-zA-Z0-9]*$").IsMatch(theString);
    }

    public static bool IsAlphaNumeric(object value)
    {
      if (value is string)
      {
        if (string.IsNullOrEmpty((string) value))
          return false;
        try
        {
          if (new Regex("^[a-zA-Z0-9]*$").IsMatch((string) value))
            return true;
        }
        catch
        {
          return false;
        }
      }
      return false;
    }

    static Utils()
    {
      Utils.stateAbbrTbl = CollectionsUtil.CreateCaseInsensitiveHashtable();
      Utils.stateAbbrTbl.Add((object) "alabama", (object) "AL");
      Utils.stateAbbrTbl.Add((object) "alaska", (object) "AK");
      Utils.stateAbbrTbl.Add((object) "arizona", (object) "AZ");
      Utils.stateAbbrTbl.Add((object) "arkansas", (object) "AR");
      Utils.stateAbbrTbl.Add((object) "california", (object) "CA");
      Utils.stateAbbrTbl.Add((object) "colorado", (object) "CO");
      Utils.stateAbbrTbl.Add((object) "connecticut", (object) "CT");
      Utils.stateAbbrTbl.Add((object) "delaware", (object) "DE");
      Utils.stateAbbrTbl.Add((object) "district of columbia", (object) "DC");
      Utils.stateAbbrTbl.Add((object) "florida", (object) "FL");
      Utils.stateAbbrTbl.Add((object) "georgia", (object) "GA");
      Utils.stateAbbrTbl.Add((object) "hawaii", (object) "HI");
      Utils.stateAbbrTbl.Add((object) "idaho", (object) "ID");
      Utils.stateAbbrTbl.Add((object) "illinois", (object) "IL");
      Utils.stateAbbrTbl.Add((object) "indiana", (object) "IN");
      Utils.stateAbbrTbl.Add((object) "iowa", (object) "IA");
      Utils.stateAbbrTbl.Add((object) "kansas", (object) "KS");
      Utils.stateAbbrTbl.Add((object) "kentucky", (object) "KY");
      Utils.stateAbbrTbl.Add((object) "louisiana", (object) "LA");
      Utils.stateAbbrTbl.Add((object) "maine", (object) "ME");
      Utils.stateAbbrTbl.Add((object) "maryland", (object) "MD");
      Utils.stateAbbrTbl.Add((object) "massachusetts", (object) "MA");
      Utils.stateAbbrTbl.Add((object) "michigan", (object) "MI");
      Utils.stateAbbrTbl.Add((object) "minnesota", (object) "MN");
      Utils.stateAbbrTbl.Add((object) "mississippi", (object) "MS");
      Utils.stateAbbrTbl.Add((object) "missouri", (object) "MO");
      Utils.stateAbbrTbl.Add((object) "montana", (object) "MT");
      Utils.stateAbbrTbl.Add((object) "nebraska", (object) "NE");
      Utils.stateAbbrTbl.Add((object) "nevada", (object) "NV");
      Utils.stateAbbrTbl.Add((object) "new hampshire", (object) "NH");
      Utils.stateAbbrTbl.Add((object) "new jersey", (object) "NJ");
      Utils.stateAbbrTbl.Add((object) "new mexico", (object) "NM");
      Utils.stateAbbrTbl.Add((object) "new york", (object) "NY");
      Utils.stateAbbrTbl.Add((object) "north carolina", (object) "NC");
      Utils.stateAbbrTbl.Add((object) "north dakota", (object) "ND");
      Utils.stateAbbrTbl.Add((object) "ohio", (object) "OH");
      Utils.stateAbbrTbl.Add((object) "oklahoma", (object) "OK");
      Utils.stateAbbrTbl.Add((object) "oregon", (object) "OR");
      Utils.stateAbbrTbl.Add((object) "pennsylvania", (object) "PA");
      Utils.stateAbbrTbl.Add((object) "rhode island", (object) "RI");
      Utils.stateAbbrTbl.Add((object) "south carolina", (object) "SC");
      Utils.stateAbbrTbl.Add((object) "south dakota", (object) "SD");
      Utils.stateAbbrTbl.Add((object) "tennessee", (object) "TN");
      Utils.stateAbbrTbl.Add((object) "texas", (object) "TX");
      Utils.stateAbbrTbl.Add((object) "utah", (object) "UT");
      Utils.stateAbbrTbl.Add((object) "vermont", (object) "VT");
      Utils.stateAbbrTbl.Add((object) "virginia", (object) "VA");
      Utils.stateAbbrTbl.Add((object) "washington", (object) "WA");
      Utils.stateAbbrTbl.Add((object) "west virginia", (object) "WV");
      Utils.stateAbbrTbl.Add((object) "wisconsin", (object) "WI");
      Utils.stateAbbrTbl.Add((object) "wyoming", (object) "WY");
      Utils.stateAbbrTbl.Add((object) "virgin islands", (object) "VI");
      Utils.stateAbbrTbl.Add((object) "guam", (object) "GU");
      Utils.stateAbbrTbl.Add((object) "puerto rico", (object) "PR");
      Utils.stateAbbrTbl.Add((object) "american samoa", (object) "AS");
      Utils.stateAbbrTbl.Add((object) "federated states of micronesia", (object) "FM");
      Utils.stateAbbrTbl.Add((object) "northern mariana islands", (object) "MP");
      Utils.stateAbbrTbl.Add((object) "republic of the marshall islands", (object) "MH");
      Utils.stateAbbrTbl.Add((object) "republic of palau", (object) "PW");
      Utils.stateAbbrTbl.Add((object) "us minor outlying islands", (object) "UM");
      Utils.stateNameTbl = CollectionsUtil.CreateCaseInsensitiveHashtable();
      Utils.stateNameTbl.Add((object) "AL", (object) "Alabama");
      Utils.stateNameTbl.Add((object) "AK", (object) "Alaska");
      Utils.stateNameTbl.Add((object) "AZ", (object) "Arizona");
      Utils.stateNameTbl.Add((object) "AR", (object) "Arkansas");
      Utils.stateNameTbl.Add((object) "CA", (object) "California");
      Utils.stateNameTbl.Add((object) "CO", (object) "Colorado");
      Utils.stateNameTbl.Add((object) "CT", (object) "Connecticut");
      Utils.stateNameTbl.Add((object) "DE", (object) "Delaware");
      Utils.stateNameTbl.Add((object) "DC", (object) "District of Columbia");
      Utils.stateNameTbl.Add((object) "FL", (object) "Florida");
      Utils.stateNameTbl.Add((object) "GA", (object) "Georgia");
      Utils.stateNameTbl.Add((object) "HI", (object) "Hawaii");
      Utils.stateNameTbl.Add((object) "ID", (object) "Idaho");
      Utils.stateNameTbl.Add((object) "IL", (object) "Illinois");
      Utils.stateNameTbl.Add((object) "IN", (object) "Indiana");
      Utils.stateNameTbl.Add((object) "IA", (object) "Iowa");
      Utils.stateNameTbl.Add((object) "KS", (object) "Kansas");
      Utils.stateNameTbl.Add((object) "KY", (object) "Kentucky");
      Utils.stateNameTbl.Add((object) "LA", (object) "Louisiana");
      Utils.stateNameTbl.Add((object) "ME", (object) "Maine");
      Utils.stateNameTbl.Add((object) "MD", (object) "Maryland");
      Utils.stateNameTbl.Add((object) "MA", (object) "Massachusetts");
      Utils.stateNameTbl.Add((object) "MI", (object) "Michigan");
      Utils.stateNameTbl.Add((object) "MN", (object) "Minnesota");
      Utils.stateNameTbl.Add((object) "MS", (object) "Mississippi");
      Utils.stateNameTbl.Add((object) "MO", (object) "Missouri");
      Utils.stateNameTbl.Add((object) "MT", (object) "Montana");
      Utils.stateNameTbl.Add((object) "NE", (object) "Nebraska");
      Utils.stateNameTbl.Add((object) "NV", (object) "Nevada");
      Utils.stateNameTbl.Add((object) "NH", (object) "New Hampshire");
      Utils.stateNameTbl.Add((object) "NJ", (object) "New Jersey");
      Utils.stateNameTbl.Add((object) "NM", (object) "New Mexico");
      Utils.stateNameTbl.Add((object) "NY", (object) "New York");
      Utils.stateNameTbl.Add((object) "NC", (object) "North Carolina");
      Utils.stateNameTbl.Add((object) "ND", (object) "North Dakota");
      Utils.stateNameTbl.Add((object) "OH", (object) "Ohio");
      Utils.stateNameTbl.Add((object) "OK", (object) "Oklahoma");
      Utils.stateNameTbl.Add((object) "OR", (object) "Oregon");
      Utils.stateNameTbl.Add((object) "PA", (object) "Pennsylvania");
      Utils.stateNameTbl.Add((object) "RI", (object) "Rhode Island");
      Utils.stateNameTbl.Add((object) "SC", (object) "South Carolina");
      Utils.stateNameTbl.Add((object) "SD", (object) "South Dakota");
      Utils.stateNameTbl.Add((object) "TN", (object) "Tennessee");
      Utils.stateNameTbl.Add((object) "TX", (object) "Texas");
      Utils.stateNameTbl.Add((object) "UT", (object) "Utah");
      Utils.stateNameTbl.Add((object) "VT", (object) "Vermont");
      Utils.stateNameTbl.Add((object) "VA", (object) "Virginia");
      Utils.stateNameTbl.Add((object) "WA", (object) "Washington");
      Utils.stateNameTbl.Add((object) "WV", (object) "West Virginia");
      Utils.stateNameTbl.Add((object) "WI", (object) "Wisconsin");
      Utils.stateNameTbl.Add((object) "WY", (object) "Wyoming");
      Utils.stateNameTbl.Add((object) "VI", (object) "Virgin Islands");
      Utils.stateNameTbl.Add((object) "GU", (object) "Guam");
      Utils.stateNameTbl.Add((object) "PR", (object) "Puerto Rico");
      Utils.stateNameTbl.Add((object) "AS", (object) "American Samoa");
      Utils.stateNameTbl.Add((object) "FM", (object) "Federated States of Micronesia");
      Utils.stateNameTbl.Add((object) "MP", (object) "Northern Mariana Islands");
      Utils.stateNameTbl.Add((object) "MH", (object) "Republic of the Marshall Islands");
      Utils.stateNameTbl.Add((object) "PW", (object) "Republic of Palau");
      Utils.stateNameTbl.Add((object) "UM", (object) "US Minor Outlying Islands");
      switch (TimeZone.CurrentTimeZone.StandardName)
      {
        case "Alaskan Standard Time":
          Utils.CurrentTimeZoneName = "AKST";
          break;
        case "Central Standard Time":
          Utils.CurrentTimeZoneName = "CST";
          break;
        case "Eastern Standard Time":
          Utils.CurrentTimeZoneName = "EST";
          break;
        case "Hawaiian Standard Time":
          Utils.CurrentTimeZoneName = "HAST";
          break;
        case "Mountain Standard Time":
          Utils.CurrentTimeZoneName = "MST";
          break;
        case "Pacific Standard Time":
          Utils.CurrentTimeZoneName = "PST";
          break;
        case "US Mountain Standard Time":
          Utils.CurrentTimeZoneName = "MST";
          break;
        default:
          Utils.CurrentTimeZoneName = TimeZone.CurrentTimeZone.StandardName;
          break;
      }
    }

    public static string CapsConvert(string org, bool capsLockOn)
    {
      if (org == string.Empty)
        return org;
      org = capsLockOn ? org.ToUpper() : Regex.Replace(org.Trim().ToLower(), "[(\\w)(-)]+", new MatchEvaluator(ZipCodeUtils.CapText));
      return org;
    }

    public static string StringWrapping(ref string val, int maxLength, int maxLine, int page)
    {
      string str1 = string.Empty;
      string empty = string.Empty;
      int num1 = 0;
      int num2 = 1;
      if (val == null || val == string.Empty)
        return string.Empty;
      do
      {
        int length = val.IndexOf(Environment.NewLine);
        bool flag = false;
        string str2;
        if (length > -1)
        {
          str2 = val.Substring(0, length);
          flag = true;
        }
        else
          str2 = val;
        if (str2.Length > maxLength)
        {
          str2 = str2.Substring(0, maxLength);
          flag = false;
          for (int index = str2.Length - 1; index >= 0; --index)
          {
            if (!(str2.Substring(index, 1) != " "))
            {
              if (index != 0)
              {
                str2 = str2.Substring(0, index);
                break;
              }
              break;
            }
          }
        }
        ++num1;
        if (num2 == page)
          str1 = !(str1 == string.Empty) ? str1 + Environment.NewLine + (str2.StartsWith(" ") ? str2.Substring(1) : str2) : str2;
        if (num1 >= maxLine && num2 < page)
        {
          ++num2;
          num1 = 0;
        }
        val = !(val == str2) ? (!flag ? val.Substring(str2.Length) : val.Substring(str2.Length + 2)) : string.Empty;
      }
      while (num1 < maxLine && val.Length > 0);
      return str1;
    }

    public static JavaScriptSerializer GetJavaScriptSerializer()
    {
      return new JavaScriptSerializer()
      {
        MaxJsonLength = 104857600
      };
    }

    public static double CalcMonthlyPayment(double rate, int term, double loanAmount)
    {
      double num1 = 0.0;
      if (term == 0 || loanAmount == 0.0 || rate == 0.0)
        return num1;
      double num2 = rate / 1200.0;
      double num3 = Math.Pow(1.0 + num2, (double) term);
      if (num3 > 0.0)
        num1 = loanAmount / ((1.0 - 1.0 / num3) / num2);
      return Math.Round(num1, 2);
    }

    public static bool FitLabelText(Graphics graphics, Label label, string text)
    {
      if ((double) label.Width >= (double) graphics.MeasureString(text, label.Font).Width)
      {
        label.Text = text;
        return true;
      }
      StringBuilder stringBuilder = new StringBuilder(text);
      do
      {
        --stringBuilder.Length;
      }
      while ((double) label.Width < (double) graphics.MeasureString(stringBuilder.ToString(), label.Font).Width);
      if (3 <= stringBuilder.Length)
      {
        stringBuilder.Length -= 3;
        stringBuilder.Append("...");
      }
      label.Text = stringBuilder.ToString();
      return false;
    }

    public static string FitToolTipText(
      Graphics graphics,
      Font font,
      float targetWidth,
      string text)
    {
      string[] strArray = text.Split(' ');
      StringBuilder stringBuilder1 = new StringBuilder();
      StringBuilder stringBuilder2 = new StringBuilder();
      int num = 0;
      while (num < strArray.Length)
      {
        float width;
        do
        {
          stringBuilder1.Append(strArray[num++] + " ");
          width = graphics.MeasureString(stringBuilder1.ToString(), font).Width;
        }
        while ((double) targetWidth >= (double) width && num < strArray.Length);
        stringBuilder2.Append(stringBuilder1.ToString() + "\n");
        stringBuilder1.Length = 0;
      }
      stringBuilder2.Length = (stringBuilder2.Length -= 2);
      return stringBuilder2.ToString();
    }

    public static double ArithmeticRounding(double val, int decimals)
    {
      double num1 = val;
      try
      {
        Decimal num2 = (Decimal) val;
        Decimal num3 = (Decimal) Math.Pow(10.0, (double) decimals);
        num1 = (double) (Decimal.Floor(Math.Abs(num2) * num3 + 0.5M) / num3 * (Decimal) Math.Sign(num2));
      }
      catch
      {
      }
      return num1;
    }

    public static int ArithmeticRoundingUp(int val, int baseFraction)
    {
      int num = val;
      try
      {
        num = val / baseFraction + Utils.ParseInt((object) (val % baseFraction > 0));
      }
      catch
      {
      }
      return num;
    }

    public static Decimal ArithmeticRounding(Decimal val, int decimals)
    {
      Decimal num1 = val;
      try
      {
        Decimal num2 = val;
        Decimal num3 = (Decimal) Math.Pow(10.0, (double) decimals);
        num1 = Decimal.Floor(Math.Abs(num2) * num3 + 0.5M) / num3 * (Decimal) Math.Sign(num2);
      }
      catch
      {
      }
      return num1;
    }

    public static bool HighlightLine(
      TextBox textBox,
      int lineIndex,
      bool highlightLeadingTrailingWhitespace)
    {
      string[] strArray = textBox.Text.Split(new string[1]
      {
        Environment.NewLine
      }, StringSplitOptions.None);
      int num = 0;
      if (lineIndex < 1 || lineIndex > strArray.Length)
        return false;
      for (int index = 1; index < lineIndex; ++index)
        num += strArray[index - 1].Length + 2;
      int length = strArray[lineIndex - 1].Length;
      if (!highlightLeadingTrailingWhitespace)
      {
        int index;
        for (index = 0; index < strArray[lineIndex - 1].Length && char.IsWhiteSpace(strArray[lineIndex - 1][index]); ++index)
        {
          ++num;
          --length;
        }
        while (length > 0 && char.IsWhiteSpace(strArray[lineIndex - 1][index + length - 1]))
          --length;
      }
      if (length <= 0 || num + length > textBox.Text.Length)
        return false;
      textBox.SelectionStart = num;
      textBox.SelectionLength = length;
      textBox.Focus();
      return true;
    }

    public static T[] JoinArrays<T>(T[] array1, T[] array2)
    {
      T[] destinationArray = new T[array1.Length + array2.Length];
      if (array1.Length != 0)
        Array.Copy((Array) array1, (Array) destinationArray, array1.Length);
      if (array2.Length != 0)
        Array.Copy((Array) array2, 0, (Array) destinationArray, array1.Length, array2.Length);
      return destinationArray;
    }

    public static int[] GetEnumValues(Array values)
    {
      int[] enumValues = new int[values.Length];
      for (int index = 0; index < values.Length; ++index)
        enumValues[index] = Convert.ToInt32(values.GetValue(index));
      return enumValues;
    }

    public static string[] SplitName(string name)
    {
      while (name.IndexOf("  ") >= 0)
        name = name.Replace("  ", " ");
      string[] strArray = name.Split(' ');
      return strArray.Length == 0 ? new string[2]{ "", "" } : (strArray.Length == 1 ? new string[2]
      {
        "",
        strArray[0]
      } : new string[2]
      {
        string.Join(" ", strArray, 0, strArray.Length - 1),
        strArray[strArray.Length - 1]
      });
    }

    public static string JoinName(string fname, string lname)
    {
      fname = (fname ?? "").Trim();
      lname = (lname ?? "").Trim();
      if (fname != "" && lname != "")
        return fname + " " + lname;
      return fname != "" ? fname : lname;
    }

    public static bool isValidLength(string value, int maxLength, int minLength = 0, bool allowNull = true)
    {
      return value == null & allowNull || (value != null || allowNull) && value.Length >= minLength && value.Length <= maxLength;
    }

    public static string CreateTimestamp(bool includeTimezoneInfo)
    {
      string timestamp = DateTime.Now.ToString("MM/dd/yyyy h:mm tt");
      if (includeTimezoneInfo)
        timestamp = timestamp + " (" + Utils.GetTimezoneAbbrev(TimeZone.CurrentTimeZone) + ")";
      return timestamp;
    }

    public static string GetTimezoneAbbrev(TimeZone timezone)
    {
      return timezone.IsDaylightSavingTime(DateTime.Now) ? (timezone.DaylightName.ToLower() == "alaskan daylight time" ? "AKDT" : Utils.ExtractCapitalLetters(timezone.DaylightName)) : (timezone.StandardName.ToLower() == "alaskan standard time" ? "AKST" : Utils.ExtractCapitalLetters(timezone.StandardName));
    }

    public static string GetTimezoneAbbrev(System.TimeZoneInfo timezone)
    {
      if (timezone == null)
        return string.Empty;
      return timezone.IsDaylightSavingTime(DateTime.Now) ? (timezone.DaylightName.ToLower() == "alaskan daylight time" ? "AKDT" : Utils.ExtractCapitalLetters(timezone.DaylightName)) : (timezone.StandardName.ToLower() == "alaskan standard time" ? "AKST" : Utils.ExtractCapitalLetters(timezone.StandardName));
    }

    public static string ExtractCapitalLetters(string text)
    {
      string capitalLetters = "";
      for (int index = 0; index < text.Length; ++index)
      {
        if (char.IsUpper(text, index))
          capitalLetters += text[index].ToString();
      }
      return capitalLetters;
    }

    public static string HexEncode(char c)
    {
      return Encoding.ASCII.GetBytes(new char[1]{ c })[0].ToString("X2");
    }

    public static string HexDecode(string text)
    {
      return Encoding.ASCII.GetString(new byte[1]
      {
        byte.Parse(text, NumberStyles.HexNumber)
      });
    }

    public static string CheckFilter(string filterString)
    {
      int length = -1;
      int index = 0;
      char[] charArray = filterString.ToCharArray();
      int num1 = 0;
      string str1 = "";
      while (index < charArray.Length)
      {
        if (charArray[index] == '(')
        {
          if (num1 == 0)
          {
            str1 = "";
            length = index;
            ++num1;
            ++index;
            continue;
          }
          ++num1;
        }
        else if (charArray[index] == ')')
        {
          --num1;
          if (num1 == 0)
          {
            int num2 = index;
            string str2 = Utils.CheckFilter(str1.Trim());
            string str3 = "";
            if (length > 0)
              str3 = filterString.Substring(0, length);
            string str4 = str3 + " " + str2 + " ";
            if (num2 < filterString.Length - 1)
              str4 += filterString.Substring(num2 + 1);
            string str5 = str4.Trim();
            charArray = str5.ToCharArray();
            filterString = str5;
            str1 = "";
            index = 0;
            continue;
          }
        }
        str1 += charArray[index].ToString();
        ++index;
      }
      bool flag1 = true;
      if (str1 == "")
        str1 = filterString.Trim();
      string str6;
      bool flag2;
      if (str1.StartsWith("true"))
      {
        str6 = str1.Length <= 5 ? "" : str1.Substring(5).Trim();
        flag2 = true;
      }
      else
      {
        str6 = str1.Length <= 6 ? "" : str1.Substring(6).Trim();
        flag2 = false;
      }
      if (str6.StartsWith("and"))
      {
        str6 = str6.Substring(4).Trim();
        flag1 = true;
      }
      else if (str6.StartsWith("or"))
      {
        str6 = str6.Substring(3).Trim();
        flag1 = false;
      }
      string str7 = "";
      foreach (char ch in str6.ToCharArray())
      {
        str7 += ch.ToString();
        if (str7.Trim() == "true")
        {
          if (!flag2 & flag1)
            flag2 = false;
          else if (!flag2 && !flag1)
            flag2 = true;
          str7 = "";
        }
        else if (str7.Trim() == "false")
        {
          if (flag2 & flag1)
            flag2 = false;
          str7 = "";
        }
        else if (str7.Trim() == "and")
        {
          flag1 = true;
          str7 = "";
        }
        else if (str7.Trim() == "or")
        {
          flag1 = false;
          str7 = "";
        }
      }
      return flag2.ToString().ToLower();
    }

    public static int GetTotalTimeSpanDays(string date1, string date2)
    {
      return Utils.GetTotalTimeSpanDays(date1, date2, false);
    }

    public static int GetTotalTimeSpanDays(string date1, string date2, bool includeBeginDate)
    {
      return Utils.GetTotalTimeSpanDays(Utils.ParseDate((object) date1), Utils.ParseDate((object) date2), includeBeginDate);
    }

    public static int GetTotalTimeSpanDays(DateTime d1, DateTime d2)
    {
      return Utils.GetTotalTimeSpanDays(d1, d2, false);
    }

    public static int GetTotalTimeSpanDays(DateTime d1, DateTime d2, bool includeBeginDate)
    {
      if (d1 == DateTime.MinValue || d2 == DateTime.MinValue)
        return 0;
      int totalDays = (int) d2.Subtract(d1).TotalDays;
      return includeBeginDate && totalDays >= 0 ? totalDays + 1 : totalDays;
    }

    public static int GetTotalTimeSpanMonths(string d1, string d2, bool date1CanGreaterdate2)
    {
      return Utils.GetTotalTimeSpanMonths(Utils.ToDate(d1), Utils.ToDate(d2), date1CanGreaterdate2);
    }

    public static int GetTotalTimeSpanMonths(DateTime d1, DateTime d2, bool date1CanGreaterdate2)
    {
      return d1 == DateTime.MinValue || d2 == DateTime.MinValue || !date1CanGreaterdate2 && d1.Date > d2.Date ? -1 : (d2.Year - d1.Year) * 12 + d2.Month - d1.Month;
    }

    public static string[] ToStringArray(Array values)
    {
      ArrayList arrayList = new ArrayList();
      foreach (object obj in values)
        arrayList.Add((object) string.Concat(obj));
      return (string[]) arrayList.ToArray(typeof (string));
    }

    public static void MoveListViewItemDown(ListView list, int selected)
    {
      if (selected + 1 >= list.Items.Count)
        return;
      list.BeginUpdate();
      ListViewItem listViewItem = list.Items[selected];
      list.Items.RemoveAt(selected);
      list.Items.Insert(selected + 1, listViewItem);
      listViewItem.Selected = true;
      list.EndUpdate();
    }

    public static void MoveListViewItemUp(ListView list, int selected)
    {
      if (selected == 0)
        return;
      list.BeginUpdate();
      ListViewItem listViewItem = list.Items[selected];
      list.Items.RemoveAt(selected);
      list.Items.Insert(selected - 1, listViewItem);
      listViewItem.Selected = true;
      list.EndUpdate();
    }

    public static DateTime ToDate(string dateValue)
    {
      DateTime result;
      if (!DateTime.TryParse(dateValue, out result))
        return DateTime.MinValue;
      if (result.Year < 1753 || result.Year > 9999)
        result = DateTime.MinValue;
      return result;
    }

    public static double ToDouble(string doubleValue)
    {
      double result;
      return double.TryParse(doubleValue, out result) && result <= double.MaxValue && result >= double.MinValue ? result : 0.0;
    }

    public static string DateTimeToString(DateTime date)
    {
      return date.Kind == DateTimeKind.Unspecified ? date.ToString("yyyy-MM-dd HH:mm:ss") : date.ToUniversalTime().ToString("u");
    }

    public static string DateTimeToUTCString(DateTime date)
    {
      return date.Kind != DateTimeKind.Unspecified ? date.ToUniversalTime().ToString("u") : throw new Exception("The specified date value must have a Local or Utc Kind");
    }

    public static DateTime ParseUTCDateTime(string dateString)
    {
      return DateTime.Parse(dateString, Utils.StandardDateFormatProvider, DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeUniversal);
    }

    public static DateTime TruncateDateTime(DateTime dateTime)
    {
      return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Kind);
    }

    public static DateTime? TruncateDateTime(DateTime? dateTime)
    {
      return dateTime.HasValue ? new DateTime?(Utils.TruncateDateTime(dateTime.Value)) : new DateTime?();
    }

    public static DateTime TruncateDate(DateTime dateTime)
    {
      return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, DateTimeKind.Unspecified);
    }

    public static DateTime? TruncateDate(DateTime? dateTime)
    {
      return dateTime.HasValue ? new DateTime?(Utils.TruncateDate(dateTime.Value)) : new DateTime?();
    }

    public static DateTime Now() => Utils.TruncateDateTime(DateTime.Now);

    public static string GetMonthSuffix(int month)
    {
      if (month < 0)
        return "";
      if (month % 100 == 11 || month % 100 == 12 || month % 100 == 13)
        return "th";
      if (month % 10 == 1)
        return "st";
      if (month % 10 == 2)
        return "nd";
      return month % 10 == 3 ? "rd" : "th";
    }

    public static long NewTpoID(List<long> existingTPOIDs)
    {
      int num1 = 0;
      int num2 = 0;
      long num3;
      do
      {
        using (CryptoRandom cryptoRandom = new CryptoRandom())
        {
          num1 = cryptoRandom.Next(10000000, 99999999);
          num2 = cryptoRandom.Next(10, 99);
        }
        num3 = long.Parse(num1.ToString() + num2.ToString());
      }
      while (existingTPOIDs != null && existingTPOIDs.Contains(num3));
      return num3;
    }

    public static T GetEnumValueFromDescription<T>(string description)
    {
      System.Type type = typeof (T);
      if (!type.IsEnum)
        throw new InvalidOperationException();
      foreach (FieldInfo field in type.GetFields())
      {
        if (Attribute.GetCustomAttribute((MemberInfo) field, typeof (DescriptionAttribute)) is DescriptionAttribute customAttribute)
        {
          if (customAttribute.Description == description)
            return (T) field.GetValue((object) null);
        }
        else if (field.Name == description)
          return (T) field.GetValue((object) null);
      }
      return default (T);
    }

    public static string FormatByteSize(long length)
    {
      if (length > 1073741824L)
        return (length / 1073741824L).ToString("0.00") + " GB";
      if (length > 1048576L)
        return (length / 1048576L).ToString("0.00") + " MB";
      return length > 1024L ? (length / 1024L).ToString("0.00") + " KB" : length.ToString() + " Bytes";
    }

    public static bool CheckIf2015RespaTila(string settingName)
    {
      return settingName == "RESPA-TILA 2015 LE and CD" || settingName == "TILA-RESPA 2015 LE and CD";
    }

    public static bool CheckIfURLA2020(string settingName)
    {
      return settingName == "URLA 2020" || settingName == "2020";
    }

    public static Hashtable LoanDefaultFileFromDocumentFolder(
      IWin32Window w,
      string filePath,
      string rootNode)
    {
      Hashtable hashtable = new Hashtable();
      try
      {
        return Utils.LoanDefaultFileFromDocumentFolder(filePath, rootNode);
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog(w, "Can't load the Default List. Error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return (Hashtable) null;
      }
    }

    public static Hashtable LoanDefaultFileFromDocumentFolder(string filePath, string rootNode)
    {
      Hashtable hashtable = new Hashtable();
      XPathDocument xpathDocument;
      try
      {
        xpathDocument = new XPathDocument(filePath);
      }
      catch
      {
        throw;
      }
      XPathNavigator navigator = xpathDocument.CreateNavigator();
      navigator.Select("/" + rootNode + "/Field");
      XPathNodeIterator xpathNodeIterator = navigator.Select("/" + rootNode + "/Field");
      while (xpathNodeIterator.MoveNext())
      {
        string attribute1 = xpathNodeIterator.Current.GetAttribute("id", string.Empty);
        string attribute2 = xpathNodeIterator.Current.GetAttribute("value", string.Empty);
        try
        {
          hashtable[(object) attribute1] = (object) attribute2;
        }
        catch
        {
        }
      }
      return hashtable;
    }

    public static List<string> LoadPiggybackDefaultSyncFields(
      IWin32Window w,
      string filePath,
      bool fromConst2PermFeature = false)
    {
      XPathDocument xPathDoc;
      try
      {
        xPathDoc = new XPathDocument(filePath);
      }
      catch (Exception ex)
      {
        if (w != null)
        {
          int num = (int) Utils.Dialog(w, "Can't load the Piggyback Default List. Error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        return (List<string>) null;
      }
      XPathNodeIterator it = Utils.LoadXPath(xPathDoc);
      if (it != null && it.Count != 0)
        return Utils.LoadFields(it, fromConst2PermFeature);
      if (w != null)
      {
        int num1 = (int) Utils.Dialog(w, "Can't load the Piggyback Default List.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      return (List<string>) null;
    }

    public static List<string> LoadPiggybackDefaultSyncFields(
      string filePath,
      out string error,
      bool fromConst2PermFeature = false)
    {
      error = string.Empty;
      filePath = SystemSettings.LocalAppDir + SystemSettings.DocDirRelPath + filePath;
      XPathDocument xPathDoc;
      try
      {
        xPathDoc = new XPathDocument(filePath);
      }
      catch (Exception ex)
      {
        error = "Can't load the Piggyback Default List. " + ex.Message;
        return (List<string>) null;
      }
      XPathNodeIterator it = Utils.LoadXPath(xPathDoc);
      if (it != null && it.Count != 0)
        return Utils.LoadFields(it, fromConst2PermFeature);
      error = "Can't load the Piggyback Default List. ";
      return (List<string>) null;
    }

    private static XPathNodeIterator LoadXPath(XPathDocument xPathDoc)
    {
      return xPathDoc.CreateNavigator().Select("/objdata/element/element");
    }

    private static List<string> LoadFields(XPathNodeIterator it, bool fromConst2PermFeature)
    {
      string empty = string.Empty;
      List<string> stringList = new List<string>();
      while (it.MoveNext())
      {
        string attribute = it.Current.GetAttribute("name", string.Empty);
        if (!(attribute == string.Empty) && (!fromConst2PermFeature || !Utils.excludeForNonPiggibackSync.ContainsKey(attribute)))
          stringList.Add(attribute);
      }
      if (fromConst2PermFeature)
      {
        string[] strArray = new string[4]
        {
          "1867",
          "3142",
          "3143",
          "TR00"
        };
        foreach (string str in strArray)
        {
          if (str == "TR00")
          {
            for (int index = 1; index <= 14; ++index)
              stringList.Add(str + index.ToString("00"));
          }
          else
            stringList.Add(str);
        }
      }
      return stringList;
    }

    public static bool HazelcastDisabledInRegistry(string instanceName)
    {
      instanceName = (instanceName ?? "").Trim();
      using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Ellie Mae\\Encompass", false))
      {
        string str1 = (string) registryKey.GetValue("DisableHazelcast");
        if (str1 != null)
        {
          string[] strArray = str1.Split(new char[2]
          {
            ',',
            ';'
          }, StringSplitOptions.None);
          if (strArray != null)
          {
            if (strArray.Length != 0)
            {
              foreach (string str2 in strArray)
              {
                if (string.Compare(instanceName, str2.Trim(), true) == 0)
                  return true;
              }
            }
          }
        }
      }
      return false;
    }

    public static bool HazelcastDisabledInSmartClient(string instanceName)
    {
      instanceName = (instanceName ?? "").Trim();
      return (SmartClientUtils.GetAttribute(instanceName, "CacheStoreSource", "DisableHazelcast") ?? "").Trim() == "1";
    }

    public static bool ValidateUrl(string url)
    {
      return !string.IsNullOrEmpty(url) && new UrlAttribute().IsValid((object) url.Trim());
    }

    public static DateTime convertUTC_To_PST(DateTime dt)
    {
      return Utils.convertUtcToTimeZone(Utils.PacificStandardTimeZone, dt);
    }

    private static DateTime convertUtcToTimeZone(System.TimeZoneInfo timeZoneInfo, DateTime dt)
    {
      try
      {
        return System.TimeZoneInfo.ConvertTimeFromUtc(dt, timeZoneInfo);
      }
      catch (Exception ex)
      {
        Tracing.Log(Utils.sw, TraceLevel.Verbose, nameof (Utils), "convertToPST Error: " + ex.Message);
        return dt;
      }
    }

    public static DateTime ConvertToTimeZone(string timeZoneCode, DateTime dt)
    {
      System.TimeZoneInfo timeZoneInfo = Utils.GetTimeZoneInfo(timeZoneCode);
      if (dt.Kind == DateTimeKind.Local && !System.TimeZoneInfo.Local.Equals(timeZoneInfo))
        dt = dt.ToUniversalTime();
      if (DateTimeKind.Utc == dt.Kind)
        dt = Utils.convertUtcToTimeZone(timeZoneInfo, dt);
      return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, DateTimeKind.Unspecified);
    }

    public static string ConvertToPST(string dt)
    {
      if (string.IsNullOrWhiteSpace(dt))
        return dt;
      try
      {
        return Utils.convertUTC_To_PST(Convert.ToDateTime(dt).ToUniversalTime()).ToString();
      }
      catch (Exception ex)
      {
        Tracing.Log(Utils.sw, TraceLevel.Verbose, nameof (Utils), "convertToPST Error: " + ex.Message);
        return dt;
      }
    }

    public static T DeepClone<T>(T obj)
    {
      using (MemoryStream serializationStream = new MemoryStream())
      {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        binaryFormatter.Serialize((Stream) serializationStream, (object) obj);
        serializationStream.Position = 0L;
        return (T) binaryFormatter.Deserialize((Stream) serializationStream);
      }
    }

    public static string FormatBorrowerNames(
      string firstName,
      string middleName,
      string lastName,
      string suffix)
    {
      return firstName.Trim() + (middleName.Trim() != "" ? " " + middleName.Trim() : "") + (lastName.Trim() != "" ? " " + lastName.Trim() : "") + (suffix.Trim() != "" ? " " + suffix.Trim() : "");
    }

    public static string EscapeDoubleQuotesForVB(string s) => s.Replace("\"", "\"\"");

    public static object GetNumeric(object value, FieldFormat format)
    {
      return format == FieldFormat.INTEGER ? (object) Utils.ParseInt(value, 0) : (object) Utils.ParseDecimal(value, 0M);
    }

    public static object GetDate(object value, FieldFormat format)
    {
      return (object) (format == FieldFormat.MONTHDAY ? Utils.ParseMonthDay(value, false) : Utils.ParseDate(value, false));
    }

    public static object ConvertToDateTime(object value, FieldFormat format, bool throwOnError = false)
    {
      if (value == null)
        return (object) null;
      DateTime result;
      if (DateTime.TryParse(value.ToString(), out result))
        return result < Utils.DbMinDate || result > Utils.DbMaxDate ? (object) null : (object) (format == FieldFormat.DATE ? result.Date : result);
      if (throwOnError)
        throw new FormatException("The value '" + value + "' cannot be converted to date type.");
      return (object) null;
    }

    public static DateTime ConvertTimeToUtc(DateTime dateTime, System.TimeZoneInfo sourceTimeZone)
    {
      return !sourceTimeZone.IsInvalidTime(dateTime) ? System.TimeZoneInfo.ConvertTimeToUtc(dateTime, sourceTimeZone) : System.TimeZoneInfo.ConvertTimeToUtc(dateTime.AddHours(1.0), sourceTimeZone);
    }

    public static string GetStringValue(object value)
    {
      string empty = string.Empty;
      if (value != null)
        empty = value.ToString();
      return empty;
    }

    public static bool IsCIFsOnlyFile(string file)
    {
      string lower = file.ToLower();
      return lower.Equals("attachments.xml") || lower.Equals("statusonline.xml") || lower.Equals("loanhistory.xml") || lower.StartsWith("dtsnapshot") || lower.StartsWith("dttemplog") || lower.StartsWith("ucd") || lower.StartsWith("lockrequestlogsnapshot") || lower.StartsWith("documenttrackinglogsnapshot") || lower.StartsWith("disclosuretrackinglog") || lower.StartsWith("custom-");
    }

    public static List<string> GetEnhanceConditionsCategoryOptions()
    {
      return new List<string>()
      {
        "Assets",
        "Credit",
        "Income",
        "Liability",
        "Miscellaneous",
        "Property"
      };
    }

    public static List<string> GetEnhanceConditionsPriorToOptions()
    {
      return new List<string>()
      {
        "Approval",
        "Docs",
        "Funding",
        "Closing",
        "Purchase"
      };
    }

    public static List<string> GetEnhanceConditionsSourceOptions()
    {
      return new List<string>()
      {
        "Investor",
        "Recorder's Office",
        "Borrowers",
        "FHA",
        "VA",
        "MI Company",
        "Other"
      };
    }

    public static List<string> GetEnhanceConditionsRecipientOptions()
    {
      return new List<string>() { "MERS", "Investor" };
    }

    public static List<string> GetEnhanceConditionsTrackingOptions()
    {
      return new List<string>()
      {
        "Received",
        "Reviewed",
        "Rejected",
        "Cleared",
        "Waived"
      };
    }

    public static List<string> GetEnhanceConditionsDefaultTrackingOptions()
    {
      return new List<string>()
      {
        "Requested",
        "Re-requested",
        "Fulfilled"
      };
    }

    public static bool ValidateRAString(string val, List<string> allowedStrings)
    {
      if (string.IsNullOrEmpty(val))
        return true;
      bool flag1 = false;
      bool flag2 = false;
      foreach (char c in val)
      {
        switch (c)
        {
          case ',':
            continue;
          case '-':
            if (flag2)
              return false;
            flag2 = true;
            continue;
          case '.':
            if (flag1)
              return false;
            flag1 = true;
            continue;
          default:
            if (!char.IsDigit(c) && c != '.')
            {
              if (allowedStrings != null)
              {
                for (int index = 0; index < allowedStrings.Count; ++index)
                {
                  if (string.Compare(val, allowedStrings[index], true) == 0)
                    return true;
                }
              }
              for (int index = 0; index < Utils.defaultAllowedRAStrings.Count; ++index)
              {
                if (string.Compare(val, Utils.defaultAllowedRAStrings[index], true) == 0)
                  return true;
              }
              return false;
            }
            continue;
        }
      }
      return (!flag2 || val.StartsWith("-") || val.EndsWith("-")) && (!flag1 || !val.StartsWith(".") && !val.EndsWith("."));
    }

    public static string FormatRAString(
      string val,
      FieldFormat fieldFormat,
      bool removeEndingZeros = false)
    {
      if (val == "" || val == null || fieldFormat != FieldFormat.RA_INTEGER && fieldFormat != FieldFormat.RA_DECIMAL_2 && fieldFormat != FieldFormat.RA_DECIMAL_3 || !val.All<char>((Func<char, bool>) (l => char.IsDigit(l) || '.' == l || '-' == l || ',' == l)))
        return val;
      string str = "";
      switch (fieldFormat)
      {
        case FieldFormat.RA_INTEGER:
          str = Utils.ParseInt((object) val, 0).ToString().Replace(",", "");
          break;
        case FieldFormat.RA_DECIMAL_2:
          str = Utils.ParseDecimal((object) val, 2M).ToString("0.00");
          break;
        case FieldFormat.RA_DECIMAL_3:
          str = Utils.ParseDecimal((object) val, 3M).ToString("0.000");
          break;
      }
      if (removeEndingZeros)
        str = Utils.RemoveEndingZeros(str);
      return str;
    }

    public static List<string> GetCountryNames(bool useNameFromCode, bool includeUnitedStates)
    {
      List<string> countryNames = new List<string>();
      if (useNameFromCode)
      {
        CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
        foreach (CultureInfo cultureInfo in cultures)
        {
          RegionInfo regionInfo = new RegionInfo(cultureInfo.LCID);
          if (!countryNames.Contains(regionInfo.EnglishName))
            countryNames.Add(regionInfo.EnglishName);
        }
        if (!includeUnitedStates && countryNames.Contains("United States"))
          countryNames.Remove("United States");
      }
      else
      {
        countryNames.AddRange((IEnumerable<string>) new string[248]
        {
          "Afghanistan",
          "Aland Islands",
          "Albania",
          "Algeria",
          "American Samoa",
          "Andorra",
          "Angola",
          "Anguilla",
          "Antarctica",
          "Antigua and Barbuda",
          "Argentina",
          "Armenia",
          "Aruba",
          "Australia",
          "Austria",
          "Azerbaijan",
          "Bahamas, The",
          "Bahrain",
          "Bangladesh",
          "Barbados",
          "Belarus",
          "Belgium",
          "Belize",
          "Benin",
          "Bermuda",
          "Bhutan",
          "Bolivia",
          "Bonaire, Sint Eustatius and Saba",
          "Bosnia and Herzegovina",
          "Botswana",
          "Bouvet Island",
          "Brazil",
          "British Indian Ocean Territory",
          "Brunei Darussalam",
          "Bulgaria",
          "Burkina Faso",
          "Burundi",
          "Cambodia",
          "Cameroon",
          "Canada",
          "Cape Verde",
          "Cayman Islands",
          "Central African Republic",
          "Chad",
          "Chile",
          "China",
          "Christmas Island",
          "Cocos (Keeling) Islands",
          "Colombia",
          "Comoros",
          "Congo",
          "Congo, The Democratic Republic of the",
          "Cook Islands",
          "Costa Rica",
          "Cote D'ivoire",
          "Croatia",
          "Cuba",
          "Curacao",
          "Cyprus",
          "Czech Republic",
          "Denmark",
          "Djibouti",
          "Dominica",
          "Dominican Republic",
          "Ecuador",
          "Egypt",
          "El Salvador",
          "Equatorial Guinea",
          "Eritrea",
          "Estonia",
          "Ethiopia",
          "Falkland Islands (Malvinas)",
          "Faroe Islands",
          "Fiji",
          "Finland",
          "France",
          "French Guiana",
          "French Polynesia",
          "French Southern Territories",
          "Gabon",
          "Gambia, The",
          "Georgia",
          "Germany",
          "Ghana",
          "Gibraltar",
          "Greece",
          "Greenland",
          "Grenada",
          "Guadeloupe",
          "Guam (US Territory)",
          "Guatemala",
          "Guernsey",
          "Guinea",
          "Guinea-Bissau",
          "Guyana",
          "Haiti",
          "Heard Island and the McDonald Islands",
          "Holy See",
          "Honduras",
          "Hong Kong",
          "Hungary",
          "Iceland",
          "India",
          "Indonesia",
          "Iraq",
          "Iran",
          "Ireland",
          "Isle of Man",
          "Israel",
          "Italy",
          "Jamaica",
          "Japan",
          "Jersey",
          "Jordan",
          "Kazakhstan",
          "Kenya",
          "Kiribati",
          "Korea, Republic of",
          "Korea, The Democratic People’s Republic of (North Korea)",
          "Kosovo",
          "Kuwait",
          "Kyrgyzstan",
          "Lao People's Democratic Republic",
          "Latvia",
          "Lebanon",
          "Lesotho",
          "Liberia",
          "Libya",
          "Liechtenstein",
          "Lithuania",
          "Luxembourg",
          "Macao",
          "Macedonia, The Former Yugoslav Republic of",
          "Madagascar",
          "Malawi",
          "Malaysia",
          "Maldives",
          "Mali",
          "Malta",
          "Marshall Islands",
          "Martinique",
          "Mauritania",
          "Mauritius",
          "Mayotte",
          "Mexico",
          "Micronesia, Federated States of",
          "Moldova, Republic of",
          "Monaco",
          "Mongolia",
          "Montenegro",
          "Montserrat",
          "Morocco",
          "Mozambique",
          "Myanmar",
          "Namibia",
          "Nauru",
          "Nepal",
          "Netherlands",
          "New Caledonia",
          "New Zealand",
          "Nicaragua",
          "Niger",
          "Nigeria",
          "Niue",
          "Norfolk Island",
          "Northern Mariana Islands, The (US Territory)",
          "Norway",
          "Oman",
          "Pakistan",
          "Palau",
          "Palestinian Territories",
          "Panama",
          "Papua New Guinea",
          "Paraguay",
          "Peru",
          "Philippines",
          "Pitcairn",
          "Poland",
          "Portugal",
          "Qatar",
          "Reunion",
          "Romania",
          "Russian Federation",
          "Rwanda",
          "Saint Barthelemy",
          "Saint Helena, Ascension and Tristan da Cunha",
          "Saint Kitts and Nevis",
          "Saint Lucia",
          "Saint Martin",
          "Saint Pierre and Miquelon",
          "Saint Vincent and the Grenadines",
          "Samoa",
          "San Marino",
          "Sao Tome and Principe",
          "Saudi Arabia",
          "Senegal",
          "Serbia",
          "Seychelles",
          "Sierra Leone",
          "Singapore",
          "Sint Maarten",
          "Slovakia",
          "Slovenia",
          "Solomon Islands",
          "Somalia",
          "South Africa",
          "South Georgia and the South Sandwich Islands",
          "South Sudan",
          "Spain",
          "Sri Lanka",
          "Sudan",
          "Suriname",
          "Svalbard and Jan Mayen",
          "Swaziland",
          "Sweden",
          "Switzerland",
          "Syria",
          "Syria (Syrian Arab Republic, The)",
          "Taiwan",
          "Tajikistan",
          "Tanzania, United Republic of",
          "Thailand",
          "Timor-leste",
          "Togo",
          "Tokelau",
          "Tonga",
          "Trinidad and Tobago",
          "Tunisia",
          "Turkey",
          "Turkmenistan",
          "Turks and Caicos Islands",
          "Tuvalu",
          "Uganda",
          "Ukraine",
          "United Arab Emirates",
          "United Kingdom",
          "United States Minor Outlying Islands",
          "Uruguay",
          "Uzbekistan",
          "Vanuatu",
          "Venezuela",
          "Vietnam",
          "Virgin Islands, British",
          "Wallis and Futuna",
          "Western Sahara",
          "Yemen",
          "Zambia",
          "Zimbabwe"
        });
        if (includeUnitedStates && !countryNames.Contains("United States"))
          countryNames.Add("United States");
      }
      countryNames.Sort();
      return countryNames;
    }

    public static DateTime ConvertDateTimeToEst(DateTime dateTime)
    {
      try
      {
        System.TimeZoneInfo systemTimeZoneById = System.TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
        System.TimeZoneInfo local = System.TimeZoneInfo.Local;
        if (systemTimeZoneById == local)
          return dateTime;
        DateTime dateTime1 = dateTime;
        if (dateTime.Kind != DateTimeKind.Utc)
          dateTime1 = System.TimeZoneInfo.ConvertTimeToUtc(dateTime, local);
        return System.TimeZoneInfo.ConvertTimeFromUtc(dateTime1, systemTimeZoneById);
      }
      catch (TimeZoneNotFoundException ex)
      {
        Tracing.Log(true, TraceLevel.Error.ToString(), "Utils.ConvertDateTimeToEst", "Unable to retrieve the Eastern Standard time zone.");
        return dateTime;
      }
      catch (InvalidTimeZoneException ex)
      {
        Tracing.Log(true, TraceLevel.Error.ToString(), "Utils.ConvertDateTimeToEst", "Unable to retrieve the Eastern Standard time zone.");
        return dateTime;
      }
      catch (Exception ex)
      {
        Tracing.Log(true, TraceLevel.Error.ToString(), "Utils.ConvertDateTimeToEst", "Unable to convert pacific time to EST");
        return dateTime;
      }
    }

    public static int GetFICONumber(string score)
    {
      string empty = string.Empty;
      return Utils.ParseInt((object) ((IEnumerable<char>) score.ToCharArray()).Where<char>((Func<char, bool>) (t => "0123456789".IndexOf(t) != -1)).Aggregate<char, string>(empty, (Func<string, char, string>) ((current, t) => current + t.ToString((IFormatProvider) CultureInfo.InvariantCulture))));
    }

    public static int FindMiddleFico(int[] scores)
    {
      for (int index1 = 0; index1 < scores.Length; ++index1)
      {
        for (int index2 = index1 + 1; index2 < scores.Length; ++index2)
        {
          if (scores[index2] > scores[index1])
          {
            int score = scores[index1];
            scores[index1] = scores[index2];
            scores[index2] = score;
          }
        }
      }
      if (scores[1] <= 0 && scores[0] <= 0)
        return 0;
      return scores[1] > 0 ? scores[1] : scores[0];
    }

    public static string[] GetStateLicenseStatus()
    {
      return new string[28]
      {
        "",
        "Transition Requested",
        "Transition Cancelled",
        "Transition Rejected",
        "Pending Incomplete",
        "Pending Review",
        "Pending Deficient",
        "Pending - Withdraw Requested",
        "Withdrawn - Application Abandoned",
        "Withdrawn - Voluntary without Licensure",
        "Denied",
        "Denied - On Appeal",
        "Approved",
        "Approved - Conditional",
        "Approved - Deficient",
        "Approved - Failed to Renew",
        "Approved - Inactive",
        "Approved - On Appeal",
        "Approved - Surrender/Cancellation Requested",
        "Revoked",
        "Revoked - On Appeal",
        "Suspended",
        "Suspended - On Appeal",
        "Temporary Cease and Desist",
        "Temporary - Expired",
        "Temporary - Failed to Renew",
        "Terminated - Ordered to Surrender",
        "Terminated - Surrendered/Cancelled"
      };
    }

    public static bool LoanFileExists(string encDatadir, string loanFolder, string loanName)
    {
      return File.Exists(Path.Combine(encDatadir, loanFolder + "\\" + loanName + "\\loan.em"));
    }

    public static HashSet<string> GetIndexRateFields() => Utils.indexRateFields;

    public static bool IsLenderObligatedFee(int lineNumber)
    {
      return Utils.LenderObligatedFee_IndicatorFields.ContainsKey(lineNumber);
    }

    public static string GetLenderObligatedIndicatorFieldID(int lineNumber)
    {
      return Utils.LenderObligatedFee_IndicatorFields[lineNumber];
    }

    public static string GetLenderObligatedBorrowerAmountFieldID(int lineNumber)
    {
      return Utils.LenderObligatedFee_BorrowerAmountFields[lineNumber];
    }

    public static double TruncateToCents(double dollarAmount)
    {
      return Math.Truncate(100.0 * dollarAmount) / 100.0;
    }

    private delegate DialogResult UtilsDialogCallback(
      IWin32Window owner,
      string text,
      MessageBoxButtons buttons,
      MessageBoxIcon icon,
      MessageBoxDefaultButton defaultButton,
      bool showCustomForm = false,
      string[] buttonList = null);
  }
}
