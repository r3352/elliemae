// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.TriggerEventConditionNameProvider
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using System.Collections;
using System.Collections.Specialized;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class TriggerEventConditionNameProvider : CustomEnumNameProvider
  {
    private static Hashtable nameMap = CollectionsUtil.CreateCaseInsensitiveHashtable();

    static TriggerEventConditionNameProvider()
    {
      TriggerEventConditionNameProvider.nameMap.Add((object) TriggerConditionType.ValueChange, (object) "Any change in field value");
      TriggerEventConditionNameProvider.nameMap.Add((object) TriggerConditionType.NonEmptyValue, (object) "When field is set to a non-empty value");
      TriggerEventConditionNameProvider.nameMap.Add((object) TriggerConditionType.FixedValue, (object) "When field is set to a specific value");
      TriggerEventConditionNameProvider.nameMap.Add((object) TriggerConditionType.Range, (object) "When field is set in a range of values");
      TriggerEventConditionNameProvider.nameMap.Add((object) TriggerConditionType.ValueList, (object) "When field is set to an item from a list of values");
    }

    public TriggerEventConditionNameProvider()
      : base(typeof (TriggerConditionType), TriggerEventConditionNameProvider.nameMap)
    {
    }
  }
}
