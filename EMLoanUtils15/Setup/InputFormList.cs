// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.InputFormList
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class InputFormList
  {
    private const string className = "InputFormList�";
    private static string sw = Tracing.SwOutsideLoan;
    private InputFormInfo[] forms;
    private Hashtable nameMap = CollectionsUtil.CreateCaseInsensitiveHashtable();
    private Hashtable idMap = CollectionsUtil.CreateCaseInsensitiveHashtable();
    private Hashtable categoryMap = CollectionsUtil.CreateCaseInsensitiveHashtable();
    private Hashtable accessibleForms = CollectionsUtil.CreateCaseInsensitiveHashtable();

    public InputFormList(SessionObjects sessionObjects)
      : this(sessionObjects, InputFormType.All)
    {
    }

    public InputFormList(SessionObjects sessionObjects, InputFormType formType)
      : this(sessionObjects.FormManager.GetFormInfos(formType), InputFormList.getUserAccessibleForms(sessionObjects))
    {
    }

    public InputFormList(InputFormInfo[] allForms, InputFormInfo[] userAccessibleForms)
    {
      this.forms = allForms;
      foreach (InputFormInfo form in this.forms)
      {
        this.idMap[(object) form.FormID] = (object) form;
        this.nameMap[(object) form.Name] = (object) form;
      }
      this.loadFormList(userAccessibleForms);
    }

    private static InputFormInfo[] getUserAccessibleForms(SessionObjects sessionObjects)
    {
      return sessionObjects.UserInfo.IsSuperAdministrator() ? sessionObjects.FormManager.GetAllFormInfos() : ((IInputFormsAclManager) sessionObjects.GetAclManager(AclCategory.InputForms)).GetAccessibleForms(sessionObjects.UserID, sessionObjects.UserInfo.UserPersonas);
    }

    private void loadFormList(InputFormInfo[] inputForms)
    {
      ArrayList arrayList1 = new ArrayList();
      ArrayList arrayList2 = new ArrayList();
      ArrayList arrayList3 = new ArrayList();
      ArrayList arrayList4 = new ArrayList();
      ArrayList arrayList5 = new ArrayList();
      ArrayList arrayList6 = new ArrayList();
      for (int index = 0; index < inputForms.Length; ++index)
      {
        this.accessibleForms[(object) inputForms[index].FormID] = (object) inputForms[index];
        if (inputForms[index].Type != InputFormType.Custom)
        {
          if (inputForms[index].IsDefault && (inputForms[index].Type == InputFormType.Standard || inputForms[index].Type == InputFormType.Virtual))
            arrayList2.Add((object) inputForms[index].FormID);
          if (inputForms[index].CanPickField)
            arrayList3.Add((object) inputForms[index].FormID);
          if (inputForms[index].Category == InputFormCategory.Form && (inputForms[index].Type == InputFormType.Standard || inputForms[index].Type == InputFormType.Virtual))
            arrayList1.Add((object) inputForms[index].FormID);
        }
        else
        {
          if (inputForms[index].IsDefault)
            arrayList5.Add((object) inputForms[index].FormID);
          if (inputForms[index].CanPickField)
            arrayList6.Add((object) inputForms[index].FormID);
          if (inputForms[index].Category == InputFormCategory.Form)
            arrayList4.Add((object) inputForms[index].FormID);
        }
      }
      this.categoryMap[(object) "All"] = (object) (string[]) arrayList1.ToArray(typeof (string));
      this.categoryMap[(object) "Default"] = (object) (string[]) arrayList2.ToArray(typeof (string));
      this.categoryMap[(object) "DataTemplate"] = (object) (string[]) arrayList3.ToArray(typeof (string));
      this.categoryMap[(object) ("All" + (object) InputFormType.Custom)] = (object) (string[]) arrayList4.ToArray(typeof (string));
      this.categoryMap[(object) ("Default" + (object) InputFormType.Custom)] = (object) (string[]) arrayList5.ToArray(typeof (string));
      this.categoryMap[(object) ("DataTemplate" + (object) InputFormType.Custom)] = (object) (string[]) arrayList6.ToArray(typeof (string));
    }

    public InputFormInfo[] GetDefaultFormList() => this.GetFormList("Default", true);

    public InputFormInfo[] GetFormList(string loanType) => this.GetFormList(loanType, true);

    public InputFormInfo[] GetFormList(string loanType, bool includeCustomForms)
    {
      ArrayList arrayList = new ArrayList();
      foreach (string key in (IEnumerable) this.categoryMap[(object) loanType])
      {
        if (key == InputFormInfo.Divider.FormID)
          arrayList.Add((object) InputFormInfo.Divider);
        else if (this.idMap.Contains((object) key))
          arrayList.Add(this.idMap[(object) key]);
      }
      if (includeCustomForms)
      {
        bool flag = true;
        foreach (InputFormInfo form in this.forms)
        {
          if (form.Type == InputFormType.Custom)
          {
            foreach (string str in (IEnumerable) this.categoryMap[(object) (loanType + (object) InputFormType.Custom)])
            {
              if (str == form.FormID)
              {
                if (flag && loanType != "FormTemplate" && arrayList.Count > 0)
                  arrayList.Add((object) InputFormInfo.Divider);
                arrayList.Add((object) form);
                flag = false;
                break;
              }
            }
          }
        }
      }
      List<InputFormInfo> inputFormInfoList = new List<InputFormInfo>((IEnumerable<InputFormInfo>) this.forms);
      foreach (InputFormInfo form in this.forms)
      {
        if (!arrayList.Contains((object) form))
          inputFormInfoList.Remove(form);
      }
      return inputFormInfoList.ToArray();
    }

    public bool IsAccessible(string formId) => this.accessibleForms.ContainsKey((object) formId);

    public bool IsTool(string formId)
    {
      foreach (InputFormInfo form in this.forms)
      {
        if (string.Compare(form.FormID, formId, true) == 0)
          return form.Category == InputFormCategory.Tool;
      }
      return false;
    }

    public InputFormInfo GetForm(string id) => (InputFormInfo) this.idMap[(object) id];

    public InputFormInfo GetFormByName(string name) => (InputFormInfo) this.nameMap[(object) name];
  }
}
