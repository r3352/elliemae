// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Import.PointFeeImport
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Import
{
  public class PointFeeImport
  {
    private const string className = "PointFeeImport";
    private static readonly string sw = Tracing.SwImportExport;
    private FeeListBase feeList;
    private string path = string.Empty;

    [DllImport("kernel32", EntryPoint = "GetPrivateProfileStringA", CharSet = CharSet.Ansi)]
    private static extern int GetPrivateProfileString(
      string sectionName,
      string keyName,
      string defaultValue,
      StringBuilder returnbuffer,
      int buffersize,
      string filename);

    public PointFeeImport(string path)
    {
      this.path = path;
      this.importFees();
    }

    private void importFees()
    {
      string str1 = this.path + "RATES.INI";
      if (!File.Exists(str1))
      {
        int num1 = (int) Utils.Dialog((IWin32Window) Session.MainScreen, "File '" + str1 + "' does not exists. Please verify your path and try again.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        string sectionName = string.Empty;
        for (int index1 = 1; index1 <= 3; ++index1)
        {
          try
          {
            switch (index1)
            {
              case 1:
                sectionName = "City Tax";
                this.feeList = (FeeListBase) Session.GetSystemSettings(typeof (FeeCityList));
                break;
              case 2:
                sectionName = "State Tax";
                this.feeList = (FeeListBase) Session.GetSystemSettings(typeof (FeeStateList));
                break;
              case 3:
                sectionName = "User Defined";
                this.feeList = (FeeListBase) Session.GetSystemSettings(typeof (FeeUserList));
                break;
            }
          }
          catch (Exception ex)
          {
            Tracing.Log(PointFeeImport.sw, TraceLevel.Error, nameof (PointFeeImport), "PointFeeImport: " + ex.Message + "\r\r");
            continue;
          }
          ArrayList arrayList = new ArrayList();
          int num2 = -1;
          while (true)
          {
            try
            {
              StringBuilder returnbuffer = new StringBuilder(256);
              PointFeeImport.GetPrivateProfileString(sectionName, Convert.ToString(++num2), "SENTINEL", returnbuffer, 256, str1);
              if (!(returnbuffer.ToString() == "SENTINEL"))
                arrayList.Add((object) returnbuffer.ToString());
              else
                break;
            }
            catch
            {
              string str2 = "An error occurred while reading the rates for '" + sectionName + "'. Make sure that your file '" + str1 + "' has not been corrupted.";
              Tracing.Log(PointFeeImport.sw, TraceLevel.Error, nameof (PointFeeImport), str2 + "\r\r");
            }
          }
          string calcBasedOn = string.Empty;
          string str3 = string.Empty;
          string rate = string.Empty;
          string additional = string.Empty;
          for (int index2 = 0; index2 < arrayList.Count; ++index2)
          {
            string[] strArray1 = arrayList[index2].ToString().Trim().Split('=');
            string[] strArray2 = strArray1[1].ToString().Replace("X", "~").Replace("%", "~").ToString().Split('~');
            try
            {
              calcBasedOn = !(strArray2[0].Trim() == "PP") ? "Loan Amount" : "Purchase Price";
              str3 = strArray1[0].Trim();
              rate = strArray2[1].Trim();
              additional = strArray2[2].Replace("+", "");
              additional = additional.Replace("$", "");
              additional = additional.Trim();
            }
            catch (Exception ex)
            {
              Tracing.Log(PointFeeImport.sw, TraceLevel.Error, nameof (PointFeeImport), "importFees: " + ex.Message + "\r\r");
            }
            if (this.feeList.TableNameExists(str3))
            {
              if (Utils.Dialog((IWin32Window) Session.MainScreen, "The Fee '" + str3 + "' already exist. Do you want to overwrite it?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
                this.feeList.UpdateFee(str3, str3, calcBasedOn, rate, additional);
              else
                continue;
            }
            else
              this.feeList.InsertFee(str3, calcBasedOn, rate, additional);
            switch (sectionName)
            {
              case "City Tax":
                Session.SaveSystemSettings((object) (FeeCityList) this.feeList);
                continue;
              case "State Tax":
                Session.SaveSystemSettings((object) (FeeStateList) this.feeList);
                continue;
              case "User Defined":
                Session.SaveSystemSettings((object) (FeeUserList) this.feeList);
                continue;
              default:
                continue;
            }
          }
        }
      }
    }
  }
}
