// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.SessionObjects.SpecialFeatureCodeManager
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using Elli.ElliEnum.Triggers;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.Server;
using EllieMae.EMLite.Server.ServerObjects;
using EllieMae.EMLite.Server.ServerObjects.Bpm;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Elli.Server.Remoting.SessionObjects
{
  public class SpecialFeatureCodeManager : SessionBoundObject, ISpecialFeatureCodeManager
  {
    private const string className = "SpecialFeatureCodeManager";

    private SpecialFeatureCodeDefinitionAccessor Accessor { get; set; }

    public SpecialFeatureCodeManager Initialize(ISession session)
    {
      this.InitializeInternal(session, nameof (SpecialFeatureCodeManager).ToLower());
      this.Accessor = new SpecialFeatureCodeDefinitionAccessor();
      return this;
    }

    public virtual bool Activate(SpecialFeatureCodeDefinition toActivate)
    {
      this.onApiCalled(nameof (SpecialFeatureCodeManager), nameof (Activate), new object[1]
      {
        (object) toActivate
      });
      try
      {
        return this.Accessor.SetStatus(toActivate);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (SpecialFeatureCodeManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual bool Add(SpecialFeatureCodeDefinition toAdd)
    {
      this.onApiCalled(nameof (SpecialFeatureCodeManager), nameof (Add), new object[1]
      {
        (object) toAdd
      });
      try
      {
        return this.Accessor.Create(toAdd);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (SpecialFeatureCodeManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual bool Deactivate(SpecialFeatureCodeDefinition toDeactivate)
    {
      this.onApiCalled(nameof (SpecialFeatureCodeManager), nameof (Deactivate), new object[1]
      {
        (object) toDeactivate
      });
      try
      {
        return this.Accessor.SetStatus(toDeactivate, SpecialFeatureCodeDefinitionStatus.None);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (SpecialFeatureCodeManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual bool Delete(SpecialFeatureCodeDefinition toDelete)
    {
      this.onApiCalled(nameof (SpecialFeatureCodeManager), nameof (Delete), new object[1]
      {
        (object) toDelete
      });
      try
      {
        return this.Accessor.Delete(toDelete);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (SpecialFeatureCodeManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual IList<SpecialFeatureCodeDefinition> GetAll()
    {
      this.onApiCalled(nameof (SpecialFeatureCodeManager), nameof (GetAll), Array.Empty<object>());
      try
      {
        return this.Accessor.GetAll();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (SpecialFeatureCodeManager), ex, this.Session.SessionInfo);
        return (IList<SpecialFeatureCodeDefinition>) null;
      }
    }

    public virtual IList<SpecialFeatureCodeDefinition> GetActive()
    {
      this.onApiCalled(nameof (SpecialFeatureCodeManager), nameof (GetActive), Array.Empty<object>());
      try
      {
        return this.Accessor.GetAll(true);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (SpecialFeatureCodeManager), ex, this.Session.SessionInfo);
        return (IList<SpecialFeatureCodeDefinition>) null;
      }
    }

    public virtual bool Update(SpecialFeatureCodeDefinition toUpdate)
    {
      this.onApiCalled(nameof (SpecialFeatureCodeManager), nameof (Update), new object[1]
      {
        (object) toUpdate
      });
      try
      {
        return this.Accessor.Update(toUpdate);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (SpecialFeatureCodeManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual bool IsUsedinFieldTriggerRule(string sfcId)
    {
      this.onApiCalled(nameof (SpecialFeatureCodeManager), "IsReferredinFieldTriggerRule", new object[1]
      {
        (object) sfcId
      });
      try
      {
        bool flag = false;
        if (((IEnumerable<TriggerInfo>) BpmDbAccessor.GetAccessor(BizRuleType.Triggers).GetRules()).SelectMany<TriggerInfo, TriggerEvent>((Func<TriggerInfo, IEnumerable<TriggerEvent>>) (t => (IEnumerable<TriggerEvent>) t.Events)).Where<TriggerEvent>((Func<TriggerEvent, bool>) (a => a.Action.ActionType == TriggerActionType.AddSpecialFeatureCode)).Select<TriggerEvent, TriggerSpecialFeatureCodesAction>((Func<TriggerEvent, TriggerSpecialFeatureCodesAction>) (e => (TriggerSpecialFeatureCodesAction) e.Action)).ToList<TriggerSpecialFeatureCodesAction>().SelectMany<TriggerSpecialFeatureCodesAction, KeyValuePair<string, string>>((Func<TriggerSpecialFeatureCodesAction, IEnumerable<KeyValuePair<string, string>>>) (a => (IEnumerable<KeyValuePair<string, string>>) a.SpecialFeatureCodes)).Where<KeyValuePair<string, string>>((Func<KeyValuePair<string, string>, bool>) (sfc => sfc.Key == sfcId)).Any<KeyValuePair<string, string>>())
          flag = true;
        return flag;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (SpecialFeatureCodeManager), ex, this.Session.SessionInfo);
        return false;
      }
    }
  }
}
