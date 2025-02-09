// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Import.PointCustomFieldsImport
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.Import
{
  public class PointCustomFieldsImport
  {
    private string path = string.Empty;

    [DllImport("kernel32", EntryPoint = "GetPrivateProfileStringA", CharSet = CharSet.Ansi)]
    private static extern int GetPrivateProfileString(
      string sectionName,
      string keyName,
      string defaultValue,
      StringBuilder returnbuffer,
      int buffersize,
      string filename);

    public PointCustomFieldsImport(string path)
    {
      this.path = path;
      this.ImportFields();
    }

    private void ImportFields()
    {
      for (int index1 = 1; index1 <= 4; ++index1)
      {
        string str1 = this.path + "CUSTOM" + index1.ToString() + ".CSF";
        if (File.Exists(str1))
        {
          StringBuilder returnbuffer = new StringBuilder(256);
          string empty1 = string.Empty;
          string empty2 = string.Empty;
          ArrayList options = new ArrayList();
          for (int index2 = 1; index2 <= 25; ++index2)
          {
            string empty3;
            try
            {
              int privateProfileString = PointCustomFieldsImport.GetPrivateProfileString("Line " + index2.ToString(), "Title", "", returnbuffer, 256, str1);
              empty3 = returnbuffer.ToString();
              privateProfileString = PointCustomFieldsImport.GetPrivateProfileString("Line " + index2.ToString(), "Type", "", returnbuffer, 256, str1);
              empty2 = returnbuffer.ToString();
              if (empty2 == "Dropdown")
              {
                int num = -1;
                options.Clear();
                while (true)
                {
                  ++num;
                  privateProfileString = PointCustomFieldsImport.GetPrivateProfileString("Line " + index2.ToString(), "Dropdown" + num.ToString(), "", returnbuffer, 256, str1);
                  string str2 = returnbuffer.ToString();
                  if (!(str2 == string.Empty))
                    options.Add((object) str2);
                  else
                    break;
                }
              }
            }
            catch
            {
              empty3 = string.Empty;
            }
            if (!(empty3 == string.Empty))
              this.SetField("CUST" + ((index1 - 1) * 25 + index2).ToString("00") + "FV", empty3, empty2, options);
          }
        }
      }
    }

    private void SetField(string fieldID, string desc, string type, ArrayList options)
    {
      string[] strArray = new string[options.Count];
      for (int index = 0; index < options.Count; ++index)
        strArray[index] = (string) options[index];
      CustomFieldInfo loanCustomField = Session.ConfigurationManager.GetLoanCustomField(fieldID);
      loanCustomField.Description = desc;
      loanCustomField.Format = this.GetFieldType(type);
      loanCustomField.Options = strArray;
      Session.ConfigurationManager.UpdateLoanCustomField(loanCustomField);
    }

    private FieldFormat GetFieldType(string type)
    {
      switch (type)
      {
        case "Currency":
          return FieldFormat.DECIMAL_2;
        case "Date":
          return FieldFormat.DATE;
        case "Dropdown":
          return FieldFormat.DROPDOWNLIST;
        case "Integer":
          return FieldFormat.INTEGER;
        case "Percentage":
          return FieldFormat.DECIMAL_3;
        case "Phone":
          return FieldFormat.PHONE;
        case "SSN":
          return FieldFormat.SSN;
        case "Text":
          return FieldFormat.STRING;
        default:
          return FieldFormat.NONE;
      }
    }
  }
}
