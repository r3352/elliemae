// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.BusinessRules.TriggerCache
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Classes;
using EllieMae.EMLite.Common;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.BusinessRules
{
  public class TriggerCache
  {
    private const string className = "TriggerCache�";
    private static readonly string sw = Tracing.SwDataEngine;
    private static Hashtable systemTriggers = new Hashtable();
    private static object syncRoot = new object();

    private TriggerCache()
    {
    }

    public static CompiledTriggers GetTriggers(SessionObjects sessionObjects)
    {
      string key = sessionObjects.StartupInfo.ServerInstanceName + "@" + sessionObjects.StartupInfo.ServerID;
      TriggerCache.ServerTriggers serverTriggers = (TriggerCache.ServerTriggers) null;
      lock (TriggerCache.syncRoot)
      {
        if (!TriggerCache.systemTriggers.Contains((object) key))
          TriggerCache.systemTriggers[(object) key] = (object) new TriggerCache.ServerTriggers();
        serverTriggers = (TriggerCache.ServerTriggers) TriggerCache.systemTriggers[(object) key];
      }
      return serverTriggers.GetTriggers(sessionObjects, sessionObjects.TriggersConfigInfo);
    }

    public static DateTime GetTriggersModificationTimestamp(SessionObjects sessionObjects)
    {
      string key = sessionObjects.StartupInfo.ServerInstanceName + "@" + sessionObjects.StartupInfo.ServerID;
      TriggerCache.ServerTriggers serverTriggers = (TriggerCache.ServerTriggers) null;
      lock (TriggerCache.syncRoot)
      {
        if (!TriggerCache.systemTriggers.Contains((object) key))
          return DateTime.MinValue;
        serverTriggers = (TriggerCache.ServerTriggers) TriggerCache.systemTriggers[(object) key];
      }
      return serverTriggers.LastModificationTime;
    }

    private class ServerTriggers
    {
      private CompiledTriggers triggers;
      private DateTime lastModified = DateTime.MinValue;
      private object syncRoot = new object();

      public DateTime LastModificationTime
      {
        get
        {
          lock (this.syncRoot)
            return this.lastModified;
        }
      }

      public CompiledTriggers GetTriggers(
        SessionObjects sessionObjects,
        ConfigInfoForTriggers configInfo)
      {
        lock (this.syncRoot)
        {
          if (this.triggers == null || configInfo.TriggersModificationTime > this.lastModified)
          {
            this.triggers = new CompiledTriggers(this.generateTriggers(configInfo), !sessionObjects.Interactive);
            this.lastModified = configInfo.TriggersModificationTime;
          }
          return this.triggers;
        }
      }

      private Trigger[] generateTriggers(ConfigInfoForTriggers configInfo)
      {
        ArrayList arrayList = new ArrayList();
        foreach (TriggerInfo trigger in configInfo.Triggers)
        {
          if (!trigger.Inactive)
            arrayList.AddRange((ICollection) this.generateTrigger(configInfo, trigger));
        }
        return (Trigger[]) arrayList.ToArray(typeof (Trigger));
      }

      private ArrayList generateTrigger(ConfigInfoForTriggers configInfo, TriggerInfo triggerDef)
      {
        RuleCondition ruleCondition = BizRuleTranslator.GetRuleCondition((BizRuleInfo) triggerDef);
        ArrayList trigger = new ArrayList();
        foreach (TriggerEvent triggerEvent in triggerDef.Events)
        {
          string description = triggerDef.RuleName + " - " + (object) triggerEvent.Conditions[0];
          trigger.Add((object) new TriggerImplDef(description, triggerEvent, ruleCondition, configInfo));
        }
        return trigger;
      }
    }
  }
}
