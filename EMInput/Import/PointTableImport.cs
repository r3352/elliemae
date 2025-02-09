// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Import.PointTableImport
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Import
{
  public class PointTableImport
  {
    private const string className = "PointTableImport";
    private static readonly string sw = Tracing.SwImportExport;
    private TblEscrowPurList tblEscrowPurList = (TblEscrowPurList) Session.GetSystemSettings(typeof (TblEscrowPurList));
    private TblEscrowRefiList tblEscrowRefiList = (TblEscrowRefiList) Session.GetSystemSettings(typeof (TblEscrowRefiList));
    private TblTitlePurList tblTitlePurList = (TblTitlePurList) Session.GetSystemSettings(typeof (TblTitlePurList));
    private TblTitleRefiList tblTitleRefiList = (TblTitleRefiList) Session.GetSystemSettings(typeof (TblTitleRefiList));
    private string tableType = string.Empty;
    private string ext = string.Empty;
    private string path = string.Empty;
    private Hashtable fields = new Hashtable();
    private string[] names = new string[5]
    {
      "Name",
      "RoundUp",
      "Nearest",
      "Offset",
      "BasedOn"
    };

    [DllImport("kernel32", EntryPoint = "GetPrivateProfileStringA", CharSet = CharSet.Ansi)]
    private static extern int GetPrivateProfileString(
      string sectionName,
      string keyName,
      string defaultValue,
      StringBuilder returnbuffer,
      int buffersize,
      string filename);

    public PointTableImport(string path, string type)
    {
      this.path = path;
      this.tableType = type;
      this.ImportTables();
    }

    private void ImportTables()
    {
      for (int index = 1; index <= 2; ++index)
      {
        this.ext = index != 1 || !(this.tableType == "Escrow") ? (index != 2 || !(this.tableType == "Escrow") ? (index != 1 || !(this.tableType == "Title") ? (index != 2 || !(this.tableType == "Title") ? string.Empty : "*.TLR") : "*.TLP") : "*.ELR") : "*.ELP";
        foreach (string file in Directory.GetFiles(this.path, this.ext))
        {
          this.fields.Clear();
          if (this.GetTableInfo(file))
            this.SaveTable(this.GetValueInfo(file));
        }
      }
    }

    private bool GetTableInfo(string file)
    {
      for (int index = 0; index < this.names.Length; ++index)
      {
        try
        {
          StringBuilder returnbuffer = new StringBuilder(256);
          PointTableImport.GetPrivateProfileString("Table Information", this.names[index], "", returnbuffer, 256, file);
          this.fields.Add((object) this.names[index].ToString(), (object) returnbuffer.ToString());
        }
        catch
        {
          int num = (int) Utils.Dialog((IWin32Window) Session.MainScreen, "An error occurred while reading [Table Information] from file '" + file + "'. We will continue importing the next file.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return false;
        }
      }
      if (this.fields.Count != 0)
        return true;
      int num1 = (int) Utils.Dialog((IWin32Window) Session.MainScreen, "No information was found under {Table Information] for file '" + file + "'. We will continue importing the next file.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      return false;
    }

    private string GetValueInfo(string file)
    {
      int num = -1;
      string valueInfo = string.Empty;
      while (true)
      {
        try
        {
          StringBuilder returnbuffer = new StringBuilder(256);
          PointTableImport.GetPrivateProfileString("Table Values", Convert.ToString(++num), "SENTINEL", returnbuffer, 256, file);
          if (!(returnbuffer.ToString() == "SENTINEL"))
            valueInfo = valueInfo + returnbuffer.ToString().Replace(",", ":") + "|";
          else
            break;
        }
        catch
        {
        }
      }
      return valueInfo;
    }

    private void SaveTable(string rateList)
    {
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      string roundType = !(this.fields[(object) "RoundUp"].ToString() == "1") ? "Down" : "Up";
      string basedOn;
      switch (this.fields[(object) "BasedOn"].ToString())
      {
        case "0":
          basedOn = "Sales Price";
          break;
        case "1":
          basedOn = "Base Loan Amount";
          break;
        case "2":
          basedOn = "Appraisal Value";
          break;
        default:
          basedOn = string.Empty;
          break;
      }
      string name = this.fields[(object) "Name"].ToString();
      string nearest = this.fields[(object) "Nearest"].ToString();
      string offSet = this.fields[(object) "Offset"].ToString();
      switch (this.ext)
      {
        case "*.ELP":
          this.SaveEscrowPurTable(name, basedOn, roundType, nearest, offSet, rateList);
          break;
        case "*.ELR":
          this.SaveEscrowRefiTable(name, basedOn, roundType, nearest, offSet, rateList);
          break;
        case "*.TLP":
          this.SaveTitlePurTable(name, basedOn, roundType, nearest, offSet, rateList);
          break;
        case "*.TLR":
          this.SaveTitleRefiTable(name, basedOn, roundType, nearest, offSet, rateList);
          break;
      }
    }

    private void SaveEscrowPurTable(
      string name,
      string basedOn,
      string roundType,
      string nearest,
      string offSet,
      string rateList)
    {
      if (this.tblEscrowPurList.TableNameExists(name))
      {
        if (Utils.Dialog((IWin32Window) Session.MainScreen, "The Table '" + name + "' already exist. Do you want to overwrite it?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
          return;
        this.tblEscrowPurList.UpdateFee(name, name, false, basedOn, roundType, nearest, offSet, rateList, "");
      }
      else
        this.tblEscrowPurList.InsertFee(name, false, basedOn, roundType, nearest, offSet, rateList, "");
      Session.SaveSystemSettings((object) this.tblEscrowPurList);
    }

    private void SaveEscrowRefiTable(
      string name,
      string basedOn,
      string roundType,
      string nearest,
      string offSet,
      string rateList)
    {
      if (this.tblEscrowRefiList.TableNameExists(name))
      {
        if (Utils.Dialog((IWin32Window) Session.MainScreen, "The Table '" + name + "' already exist. Do you want to overwrite it?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
          return;
        this.tblEscrowRefiList.UpdateFee(name, name, false, basedOn, roundType, nearest, offSet, rateList, "");
      }
      else
        this.tblEscrowRefiList.InsertFee(name, false, basedOn, roundType, nearest, offSet, rateList, "");
      Session.SaveSystemSettings((object) this.tblEscrowRefiList);
    }

    private void SaveTitlePurTable(
      string name,
      string basedOn,
      string roundType,
      string nearest,
      string offSet,
      string rateList)
    {
      if (this.tblTitlePurList.TableNameExists(name))
      {
        if (Utils.Dialog((IWin32Window) Session.MainScreen, "The Table '" + name + "' already exist. Do you want to overwrite it?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
          return;
        this.tblTitlePurList.UpdateFee(name, name, false, basedOn, roundType, nearest, offSet, rateList, "");
      }
      else
        this.tblTitlePurList.InsertFee(name, false, basedOn, roundType, nearest, offSet, rateList, "");
      Session.SaveSystemSettings((object) this.tblTitlePurList);
    }

    private void SaveTitleRefiTable(
      string name,
      string basedOn,
      string roundType,
      string nearest,
      string offSet,
      string rateList)
    {
      if (this.tblTitleRefiList.TableNameExists(name))
      {
        if (Utils.Dialog((IWin32Window) Session.MainScreen, "The Table '" + name + "' already exist. Do you want to overwrite it?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
          return;
        this.tblTitleRefiList.UpdateFee(name, name, false, basedOn, roundType, nearest, offSet, rateList, "");
      }
      else
        this.tblTitleRefiList.InsertFee(name, false, basedOn, roundType, nearest, offSet, rateList, "");
      Session.SaveSystemSettings((object) this.tblTitleRefiList);
    }
  }
}
