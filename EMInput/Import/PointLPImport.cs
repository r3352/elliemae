// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Import.PointLPImport
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Import
{
  public class PointLPImport
  {
    private const string className = "PointLPImport";
    private static readonly string sw = Tracing.SwImportExport;
    private string filename;
    private Hashtable pointDict = new Hashtable();
    private Encoding encoding = Encoding.GetEncoding(1252);
    private LoanProgram lp = new LoanProgram();
    private static string[][] fields = new string[65][]
    {
      new string[4]{ "26", "LP01", "X", "Conventional" },
      new string[4]{ "27", "LP01", "X", "VA" },
      new string[4]{ "28", "LP01", "X", "FHA" },
      new string[4]
      {
        "29",
        "LP01",
        "X",
        "FarmersHomeAdministration"
      },
      new string[4]{ "1196", "LP01", "X", "Other" },
      new string[4]{ "921", "LP02", "X", "PrimaryResidence" },
      new string[4]{ "923", "LP02", "X", "SecondHome" },
      new string[4]{ "924", "LP02", "X", "Investor" },
      new string[4]{ "915", "LP03", "X", "FirstLien" },
      new string[4]{ "916", "LP03", "X", "SecondLien" },
      new string[4]{ "1190", "LP05", "X", "Purchase" },
      new string[4]
      {
        "1191",
        "LP05",
        "X",
        "ConstructionToPermanent"
      },
      new string[4]{ "1192", "LP05", "X", "ConstructionOnly" },
      new string[4]{ "1193", "LP05", "X", "Cash-Out Refinance" },
      new string[4]{ "1194", "LP05", "X", "Other" },
      new string[4]{ "550", "LP07", "X", "Fixed" },
      new string[4]
      {
        "552",
        "LP07",
        "X",
        "GraduatedPaymentMortgage"
      },
      new string[4]{ "560", "LP07", "X", "AdjustableRate" },
      new string[4]{ "562", "LP07", "X", "OtherAmortizationType" },
      new string[4]{ "12", "LP08", "", "" },
      new string[4]{ "14", "LP09", "", "" },
      new string[4]{ "13", "LP10", "", "" },
      new string[4]{ "3190", "LP11", "", "" },
      new string[4]{ "2450", "LP12", "", "" },
      new string[4]{ "2451", "LP13", "", "" },
      new string[4]{ "2452", "LP14", "", "" },
      new string[4]{ "2453", "LP15", "", "" },
      new string[4]{ "2454", "LP16", "", "" },
      new string[4]{ "2455", "LP17", "", "" },
      new string[4]{ "2456", "LP18", "", "" },
      new string[4]{ "2457", "LP19", "", "" },
      new string[4]{ "2458", "LP20", "", "" },
      new string[4]{ "2459", "LP21", "", "" },
      new string[4]{ "556", "LP24", "X", "Biweekly" },
      new string[4]{ "555", "LP25", "", "" },
      new string[4]{ "2338", "LP26", "", "" },
      new string[4]{ "2330", "LP27", "", "" },
      new string[4]{ "2324", "LP28", "", "" },
      new string[4]{ "2331", "LP29", "", "" },
      new string[4]{ "2325", "LP30", "", "" },
      new string[4]{ "2322", "LP31", "", "" },
      new string[4]{ "2329", "LP32", "", "" },
      new string[4]{ "2332", "LP33", "", "" },
      new string[4]{ "2323", "LP34", "", "" },
      new string[4]{ "2320", "LP35", "X", "Up" },
      new string[4]{ "2321", "LP35", "X", "Down" },
      new string[4]{ "2327", "LP36", "", "" },
      new string[4]{ "2335", "LP37", "", "" },
      new string[4]{ "2336", "LP38", "", "" },
      new string[4]{ "2337", "LP39", "", "" },
      new string[4]{ "2328", "LP40", "", "" },
      new string[4]{ "3201", "LP41", "", "" },
      new string[4]{ "3200", "LP42", "", "" },
      new string[4]{ "7403", "LP44", "", "" },
      new string[4]{ "2001", "LP45", "", "" },
      new string[4]{ "546", "LP46", "", "" },
      new string[4]{ "547", "LP47", "", "" },
      new string[4]{ "5027", "LP48", "", "" },
      new string[4]{ "544", "LP49", "", "" },
      new string[4]{ "545", "LP50", "", "" },
      new string[4]{ "2157", "LP66", "X", "Y" },
      new string[4]{ "2159", "LP74", "", "" },
      new string[4]{ "1197", "LP85", "", "" },
      new string[4]{ "561", "LP86", "", "" },
      new string[4]{ "563", "LP87", "", "" }
    };

    public int ImportFile(IWin32Window owner, string filename, string folder, bool isPublic)
    {
      TemplateSettingsType type = TemplateSettingsType.LoanProgram;
      this.filename = filename;
      if (!this.LoadFile())
        return 0;
      this.ImportFields();
      string str = this.lp.GetSimpleField("LP44").Trim();
      if (str == string.Empty)
      {
        Tracing.Log(PointLPImport.sw, TraceLevel.Error, nameof (PointLPImport), "The Loan Program Template '" + filename + "'  does not have the right format.\r\n");
        return 0;
      }
      this.lp.TemplateName = str;
      this.lp.Description = "Template Imported from Point: " + (object) DateTime.Now + "\r\nFilename: " + filename + "\r\nTemplate Name: " + str;
      if (str.Length > 200)
        str = str.Substring(0, 200).Trim();
      FileSystemEntry fileSystemEntry = new FileSystemEntry(SystemUtil.CombinePath(folder, str), FileSystemEntry.Types.File, isPublic ? (string) null : Session.UserID);
      if (Session.ConfigurationManager.TemplateSettingsObjectExists(type, fileSystemEntry))
      {
        if (Utils.Dialog(owner, "The Loan Program Template '" + this.filename + " - " + str + "' already exist. Do you want to overwrite it?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
        {
          Tracing.Log(PointLPImport.sw, TraceLevel.Error, nameof (PointLPImport), "The Loan Program  '" + this.filename + "' was not imported because a program with the same name already exists. The user chose not to overwrite the existing program.\r\n");
          return 0;
        }
      }
      Session.ConfigurationManager.SaveTemplateSettings(type, fileSystemEntry, (BinaryObject) (BinaryConvertibleObject) this.lp);
      return 1;
    }

    private void ImportFields()
    {
      string empty = string.Empty;
      for (int index = 0; index < PointLPImport.fields.Length; ++index)
      {
        string field = this.GetField(PointLPImport.fields[index][0]);
        if (!(field == string.Empty))
        {
          if (field == PointLPImport.fields[index][2])
            this.SetField(PointLPImport.fields[index][1], PointLPImport.fields[index][3]);
          else
            this.SetField(PointLPImport.fields[index][1], field);
        }
      }
      this.SetField("LP67", (this.GetField("2201") + " " + this.GetField("2202") + " " + this.GetField("2205") + " " + this.GetField("2206")).Trim());
    }

    private void SetField(string field, string val)
    {
      if (val == string.Empty)
        return;
      this.lp.SetCurrentField(field, val);
    }

    private string GetField(string id)
    {
      string field = string.Empty;
      if (this.pointDict.ContainsKey((object) id))
      {
        field = this.pointDict[(object) id].ToString().Trim();
        if (field == null || field == "")
          field = string.Empty;
      }
      return field;
    }

    private bool LoadFile()
    {
      FileStream fileStream;
      try
      {
        fileStream = File.Open(this.filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
      }
      catch (Exception ex)
      {
        Tracing.Log(PointLPImport.sw, TraceLevel.Error, nameof (PointLPImport), "Unable to open file  '" + this.filename + "' Make sure your file is a Loan Program Template or it is not corrupted. Exception:" + ex.Message + "\r\n");
        return false;
      }
      int length = (int) fileStream.Length;
      byte[] buffer = new byte[length];
      fileStream.Read(buffer, 0, length);
      fileStream.Close();
      BinaryReader binaryReader = new BinaryReader((Stream) new MemoryStream(buffer));
      this.pointDict.Clear();
      while (true)
      {
        try
        {
          string key = ((ushort) binaryReader.ReadInt16()).ToString();
          int count = (int) binaryReader.ReadByte();
          if (count == (int) byte.MaxValue)
            count = (int) binaryReader.ReadInt16();
          string str = this.encoding.GetString(binaryReader.ReadBytes(count));
          this.pointDict[(object) key] = (object) str;
        }
        catch (EndOfStreamException ex)
        {
          binaryReader.Close();
          break;
        }
        catch (Exception ex)
        {
          Tracing.Log(PointLPImport.sw, TraceLevel.Error, nameof (PointLPImport), "Problem loading the Loan Program Template '" + this.filename + "'  Make sure your file is a Loan Program Template or it is not corrupted Exception: " + ex.Message + "\r\n");
          return false;
        }
      }
      return true;
    }
  }
}
