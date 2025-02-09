// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.FormTemplate
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  [Serializable]
  public class FormTemplate : BinaryConvertibleObject, ITemplateSetting
  {
    private const string className = "FormTemplate";
    private static string sw = Tracing.SwOutsideLoan;
    private XmlStringTable tbl = new XmlStringTable();
    private static object nobj = (object) Missing.Value;

    public FormTemplate() => this.TemplateVersion = "4.0";

    public FormTemplate(XmlSerializationInfo info)
    {
      this.tbl = (XmlStringTable) info.GetValue(nameof (tbl), typeof (XmlStringTable));
      this.migrateTemplate();
    }

    public Hashtable GetProperties()
    {
      Hashtable insensitiveHashtable = CollectionsUtil.CreateCaseInsensitiveHashtable();
      insensitiveHashtable.Add((object) "Description", (object) this.Description);
      return insensitiveHashtable;
    }

    public XmlStringTable GetExistingForms() => this.tbl;

    public string[] GetFormNames()
    {
      ArrayList arrayList = new ArrayList();
      foreach (string key in this.tbl.Keys)
      {
        if (Utils.IsInt((object) key))
        {
          int index = Utils.ParseInt((object) key);
          while (arrayList.Count <= index)
            arrayList.Add((object) "");
          arrayList[index] = (object) string.Concat(this.tbl[key]);
        }
      }
      return (string[]) arrayList.ToArray(typeof (string));
    }

    public bool Contains(string formName)
    {
      return new ArrayList((ICollection) this.GetFormNames()).Contains((object) formName);
    }

    public void ReplaceForms(string[] formNames)
    {
      ArrayList arrayList = new ArrayList();
      foreach (string key in this.tbl.Keys)
      {
        if (Utils.IsInt((object) key))
          arrayList.Add((object) key);
      }
      foreach (object obj in arrayList)
        this.tbl.Remove(string.Concat(obj));
      for (int index = 0; index < formNames.Length; ++index)
        this.tbl.Add(string.Concat((object) index), (object) formNames[index]);
    }

    public string TemplateName
    {
      get => this.GetForm("DTNAME");
      set => this.AddForm("DTNAME", value);
    }

    public string TemplateVersion
    {
      get => this.GetForm("DTVERSION");
      set => this.AddForm("DTVERSION", value);
    }

    public string Description
    {
      get => this.tbl != null ? (string) this.tbl["DTDESC"] : string.Empty;
      set
      {
        if (this.tbl == null)
          return;
        this.tbl["DTDESC"] = (object) value;
      }
    }

    public string GetForm(string id) => (string) this.tbl[id] ?? string.Empty;

    public void AddForm(string id, string val)
    {
      if (this.tbl.ContainsKey(id))
        this.tbl.Remove(id);
      this.tbl[id] = (object) val;
    }

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("tbl", (object) this.tbl);
    }

    public ITemplateSetting Duplicate()
    {
      ITemplateSetting templateSetting = (ITemplateSetting) this.Clone();
      templateSetting.TemplateName = "";
      return templateSetting;
    }

    public static explicit operator FormTemplate(BinaryObject obj)
    {
      return (FormTemplate) BinaryConvertibleObject.Parse(obj, typeof (FormTemplate));
    }

    public void CleanFormList()
    {
      if (this.tbl == null)
        return;
      this.tbl.Clear();
    }

    private void migrateTemplate()
    {
      if (this.TemplateVersion != "")
        return;
      string[] formNames = this.GetFormNames();
      Dictionary<string, string> inputFormNameMapping = LoanMigrationData.GetInputFormNameMapping();
      for (int index = 0; index < formNames.Length; ++index)
      {
        if (inputFormNameMapping.ContainsKey(formNames[index]))
          formNames[index] = inputFormNameMapping[formNames[index]];
      }
      this.ReplaceForms(formNames);
      this.TemplateVersion = "4.0";
    }

    public static string RESPAFormNameSwap(string currentFormID, string menuItemSelected)
    {
      if (menuItemSelected != "RESPA 2010 GFE and HUD-1" && menuItemSelected != "Old GFE and HUD-1" && !Utils.CheckIf2015RespaTila(menuItemSelected))
        return currentFormID;
      bool flag1 = Utils.CheckIf2015RespaTila(menuItemSelected);
      bool flag2 = menuItemSelected == "RESPA 2010 GFE and HUD-1";
      string str = currentFormID;
      switch (currentFormID.ToUpper())
      {
        case "CLOSINGDISCLOSUREPAGE1":
        case "CLOSINGDISCLOSUREPAGE2":
        case "HUD1PG2":
        case "HUD1PG2_2010":
          str = !flag1 ? (!flag2 ? "HUD1PG2" : "HUD1PG2_2010") : "CLOSINGDISCLOSUREPAGE2";
          break;
        case "CLOSINGDISCLOSUREPAGE3":
        case "HUD1PG1":
        case "HUD1PG1_2010":
          str = !flag1 ? (!flag2 ? "HUD1PG1" : "HUD1PG1_2010") : "CLOSINGDISCLOSUREPAGE3";
          break;
        case "CLOSINGDISCLOSUREPAGE4":
        case "CLOSINGDISCLOSUREPAGE5":
          str = !flag2 ? "HUD1PG1" : "HUD1PG3_2010";
          break;
        case "HUD1PG3_2010":
          str = !flag1 ? "HUD1PG1" : "CLOSINGDISCLOSUREPAGE1";
          break;
        case "LOANESTIMATEPAGE1":
        case "LOANESTIMATEPAGE2":
        case "LOANESTIMATEPAGE3":
          str = !flag2 ? "REGZGFE" : "REGZGFEHUD";
          break;
        case "REGZ50":
          str = !flag1 ? (!flag2 ? "REGZ50" : "REGZ50") : "TBA";
          break;
        case "REGZ50CLOSER":
          str = !flag1 ? (!flag2 ? "REGZ50CLOSER" : "REGZ50CLOSER") : "TBA";
          break;
        case "REGZGFE":
        case "REGZGFEHUD":
        case "REGZGFE_2010":
        case "REGZGFE_2015":
          str = !flag1 || !(currentFormID == "REGZGFEHUD") ? (!flag1 ? (!flag2 || !(currentFormID == "LOANESTIMATEPAGE1") ? (!flag2 ? "REGZGFE" : "REGZGFE_2010") : "REGZGFEHUD") : "REGZGFE_2015") : "LOANESTIMATEPAGE1";
          break;
      }
      return str;
    }
  }
}
