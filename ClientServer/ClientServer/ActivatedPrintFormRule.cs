// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ActivatedPrintFormRule
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.BusinessRules;
using EllieMae.EMLite.Customization;
using EllieMae.EMLite.DataEngine;
using System;
using System.Collections;
using System.Collections.Specialized;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class ActivatedPrintFormRule
  {
    private Hashtable requiredFields;
    private Hashtable requiredCodes;
    private Hashtable badFormTable;
    private ArrayList blankFormAllowable;
    private string badFormList = string.Empty;

    public ActivatedPrintFormRule()
    {
      this.requiredFields = CollectionsUtil.CreateCaseInsensitiveHashtable();
      this.requiredCodes = CollectionsUtil.CreateCaseInsensitiveHashtable();
      this.badFormTable = (Hashtable) null;
      this.badFormList = string.Empty;
      this.blankFormAllowable = new ArrayList();
    }

    public void AddFormRequiredFields(string formID, Hashtable reqFields)
    {
      Hashtable insensitiveHashtable = CollectionsUtil.CreateCaseInsensitiveHashtable();
      if (!this.requiredFields.ContainsKey((object) formID))
        this.requiredFields.Add((object) formID, (object) insensitiveHashtable);
      Hashtable requiredField = (Hashtable) this.requiredFields[(object) formID];
      string empty = string.Empty;
      foreach (DictionaryEntry reqField1 in reqFields)
      {
        string key = reqField1.Key.ToString();
        if (key == PrintRequiredFieldsInfo.ADVANCEDCODINGID)
        {
          if (!this.requiredCodes.ContainsKey((object) formID))
            this.requiredCodes.Add((object) formID, (object) new ArrayList());
          ArrayList requiredCode = (ArrayList) this.requiredCodes[(object) formID];
          ArrayList reqField2 = (ArrayList) reqFields[(object) key];
          for (int index = 0; index < reqField2.Count; ++index)
            requiredCode.Add((object) reqField2[index].ToString());
        }
        else if (!requiredField.ContainsKey((object) key))
          requiredField.Add((object) key, (object) "");
      }
    }

    public bool CanPrintBlankForm(FormItemInfo form)
    {
      string key = this.convertFormName(form.FormName);
      if (!this.requiredFields.ContainsKey((object) key))
        return true;
      Hashtable requiredField = (Hashtable) this.requiredFields[(object) key];
      return requiredField == null || requiredField.Count == 0 || requiredField.ContainsKey((object) PrintRequiredFieldsInfo.PRINTBLANKID);
    }

    public bool CheckRequiredFields(FormItemInfo[] formItems, LoanData loan, bool isForBlankForm)
    {
      this.badFormTable = CollectionsUtil.CreateCaseInsensitiveHashtable();
      string empty = string.Empty;
      foreach (FormItemInfo formItem in formItems)
      {
        string key = this.convertFormName(formItem.FormName);
        if (this.requiredFields.ContainsKey((object) key) && !(this.CanPrintBlankForm(formItem) & isForBlankForm))
        {
          ArrayList arrayList = new ArrayList();
          Hashtable requiredField = (Hashtable) this.requiredFields[(object) key];
          if (requiredField != null && requiredField.Count != 0)
          {
            foreach (DictionaryEntry dictionaryEntry in requiredField)
            {
              string id = dictionaryEntry.Key.ToString();
              if (!(id == PrintRequiredFieldsInfo.PRINTBLANKID) && (loan.GetField(id) == string.Empty || loan.GetField(id) == "//"))
                arrayList.Add((object) id);
            }
            if (arrayList.Count > 0 && !this.badFormTable.ContainsKey((object) formItem.FormName))
            {
              if (this.badFormList != string.Empty)
                this.badFormList += "\r\n";
              this.badFormList += formItem.FormName;
              this.badFormTable.Add((object) formItem.FormName, (object) arrayList);
            }
          }
        }
      }
      return this.badFormTable.Count == 0;
    }

    public bool CheckRequiredCodes(
      FormItemInfo[] forms,
      LoanData loan,
      bool isForBlankForm,
      SessionObjects sessionObjects)
    {
      if (this.badFormTable == null)
        this.badFormTable = CollectionsUtil.CreateCaseInsensitiveHashtable();
      foreach (FormItemInfo form in forms)
      {
        if (this.requiredCodes.ContainsKey((object) form.FormName) && !(this.CanPrintBlankForm(form) & isForBlankForm))
        {
          ArrayList requiredCode = (ArrayList) this.requiredCodes[(object) form.FormName];
          if (requiredCode != null && requiredCode.Count != 0)
          {
            PrintFormRule[] rules = new PrintFormRule[requiredCode.Count];
            for (int index = 0; index < requiredCode.Count; ++index)
              rules[index] = new PrintFormRule(requiredCode[index].ToString(), (RuleCondition) PredefinedCondition.Empty);
            PrintFormRuleValidators formRuleValidators = new PrintFormRuleValidators(rules, form.FormName);
            if (!formRuleValidators.ValidateAll(new ExecutionContext(sessionObjects.UserInfo, loan, (IServerDataProvider) new CustomCodeSessionDataProvider(sessionObjects), true)))
            {
              if (!this.badFormTable.ContainsKey((object) form.FormName))
              {
                if (this.badFormList != string.Empty)
                  this.badFormList += "\r\n";
                this.badFormList += form.FormName;
                this.badFormTable.Add((object) form.FormName, (object) new ArrayList());
              }
              ((ArrayList) this.badFormTable[(object) form.FormName]).Add((object) formRuleValidators);
            }
          }
        }
      }
      return this.badFormTable.Count == 0;
    }

    private string convertFormName(string formName)
    {
      if (formName.ToUpper().StartsWith("IRS4506 - COPY REQUEST PAGE 1") && !formName.ToUpper().EndsWith("(CLASSIC)"))
        return "IRS4506 - Copy Req Page1";
      if (formName.ToUpper().StartsWith("IRS4506 - COPY REQUEST PAGE 2") && !formName.ToUpper().EndsWith("(CLASSIC)"))
        return "IRS4506 - Copy Req Page2";
      if (formName.ToUpper().StartsWith("IRS4506T - TRANS REQUEST PAGE 1") && !formName.ToUpper().EndsWith("(CLASSIC)"))
        return "IRS4506T - Trans Req Page1";
      return formName.ToUpper().StartsWith("IRS4506T - TRANS REQUEST PAGE 2") && !formName.ToUpper().EndsWith("(CLASSIC)") ? "IRS4506T - Trans Req Page2" : formName;
    }

    public string GetBadPrintFormList() => this.badFormList;

    public Hashtable GetBadPrintFormTable() => this.badFormTable;

    public int BadFormsCount => this.badFormTable == null ? 0 : this.badFormTable.Count;

    public bool IsABadForm(string formKey)
    {
      return this.badFormTable != null && this.badFormTable.ContainsKey((object) formKey);
    }
  }
}
