// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Import.PointSDSImport
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
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Import
{
  public class PointSDSImport
  {
    private const string className = "PointImportRESPA";
    private static readonly string sw = Tracing.SwImportExport;
    private Hashtable fields = new Hashtable();
    private string file = string.Empty;
    private static string[][] mapping = new string[2][]
    {
      new string[4]{ "May Assign", "RESPA.X1", "X", "Y" },
      new string[4]{ "Do Not Service", "RESPA.X6", "X", "Y" }
    };

    [DllImport("kernel32", EntryPoint = "GetPrivateProfileStringA", CharSet = CharSet.Ansi)]
    private static extern int GetPrivateProfileString(
      string sectionName,
      string keyName,
      string defaultValue,
      StringBuilder returnbuffer,
      int buffersize,
      string filename);

    public PointSDSImport(string path)
    {
      this.file = path + "SDS.INI";
      this.ImportRESPA();
    }

    private void ImportRESPA()
    {
      if (!this.LoadFile())
        return;
      LoanDefaultRESPA loanDefaultRespa = new LoanDefaultRESPA(Session.SessionObjects);
      for (int index = 0; index < PointSDSImport.mapping.Length; ++index)
        loanDefaultRespa.SetField(PointSDSImport.mapping[index][1], "");
      for (int index = 0; index < PointSDSImport.mapping.Length; ++index)
      {
        string key = PointSDSImport.mapping[index][0];
        if (this.fields.ContainsKey((object) key))
        {
          string val = this.fields[(object) key].ToString();
          if (PointSDSImport.mapping[index][2] == val)
            loanDefaultRespa.SetField(PointSDSImport.mapping[index][1], PointSDSImport.mapping[index][3]);
          else if (!EncompassFields.IsNumeric(PointSDSImport.mapping[index][1]) || Utils.IsDecimal((object) val))
            loanDefaultRespa.SetField(PointSDSImport.mapping[index][1], val);
        }
      }
      loanDefaultRespa.CommitChanges(Session.SessionObjects);
    }

    private bool LoadFile()
    {
      if (!File.Exists(this.file))
      {
        int num = (int) Utils.Dialog((IWin32Window) Session.MainScreen, "The file '" + this.file + "' does not exists. Please verify your path and try again.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      string[] strArray1 = new string[2]
      {
        "May Assign",
        "Do Not Service"
      };
      string sectionName = string.Empty;
      string[] strArray2 = (string[]) null;
      for (int index1 = 1; index1 <= 1; ++index1)
      {
        if (index1 == 1)
        {
          sectionName = "Estimate of Loan";
          strArray2 = strArray1;
        }
        for (int index2 = 0; index2 < strArray2.Length; ++index2)
        {
          try
          {
            StringBuilder returnbuffer = new StringBuilder(256);
            PointSDSImport.GetPrivateProfileString(sectionName, strArray2[index2], "", returnbuffer, 256, this.file);
            if (sectionName == "Record of Transfer")
            {
              if (strArray2[index2] == "Does")
                strArray2[index2] = "Does2";
              if (strArray2[index2] == "Does Not")
                strArray2[index2] = "Does Not2";
            }
            if (returnbuffer.ToString() != string.Empty)
              this.fields.Add((object) strArray2[index2], (object) returnbuffer.ToString());
          }
          catch
          {
            int num = (int) Utils.Dialog((IWin32Window) Session.MainScreen, "An error occurred while reading SDS info from file '" + this.file + "'. Please make sure you have the right path or that the file is not corrupted.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return false;
          }
        }
      }
      if (this.fields.Count != 0)
        return true;
      int num1 = (int) Utils.Dialog((IWin32Window) Session.MainScreen, "No SDS information was found. Please make sure that your file '" + this.file + "' is not been corrupted.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      return false;
    }
  }
}
