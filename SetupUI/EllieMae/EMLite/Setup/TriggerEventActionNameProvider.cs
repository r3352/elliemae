// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.TriggerEventActionNameProvider
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using Elli.ElliEnum.Triggers;
using EllieMae.EMLite.Common;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class TriggerEventActionNameProvider : CustomEnumNameProvider
  {
    private static Hashtable nameMap = CollectionsUtil.CreateCaseInsensitiveHashtable();

    static TriggerEventActionNameProvider()
    {
      TriggerEventActionNameProvider.nameMap.Add((object) TriggerActionType.Copy, (object) "Copy new value into one or more fields");
      TriggerEventActionNameProvider.nameMap.Add((object) TriggerActionType.Assign, (object) "Update the value of one or more fields");
      TriggerEventActionNameProvider.nameMap.Add((object) TriggerActionType.CompleteTasks, (object) "Mark one or more tasks as completed");
      TriggerEventActionNameProvider.nameMap.Add((object) TriggerActionType.Email, (object) "Send an email to one or more Encompass users");
      TriggerEventActionNameProvider.nameMap.Add((object) TriggerActionType.AdvancedCode, (object) "Run advanced code");
      TriggerEventActionNameProvider.nameMap.Add((object) TriggerActionType.LoanMove, (object) "Move loan to");
      TriggerEventActionNameProvider.nameMap.Add((object) TriggerActionType.ApplyLoanTemplate, (object) "Apply loan template");
      TriggerEventActionNameProvider.nameMap.Add((object) TriggerActionType.AddSpecialFeatureCode, (object) "Add Special Feature Code");
    }

    public TriggerEventActionNameProvider()
      : base(typeof (TriggerActionType), TriggerEventActionNameProvider.nameMap)
    {
    }

    public List<string> GetActionTypes()
    {
      List<string> actionTypes = new List<string>();
      foreach (DictionaryEntry name in TriggerEventActionNameProvider.nameMap)
        actionTypes.Add(name.Value.ToString());
      actionTypes.Reverse();
      return actionTypes;
    }
  }
}
