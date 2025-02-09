// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.CustomAlertPanel
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class CustomAlertPanel : AlertPanelBase
  {
    public CustomAlertPanel(PipelineInfo.Alert alert) => this.SetAlert(alert);

    public override bool AllowClearAlert
    {
      get => ((CustomAlert) this.AlertConfig.Definition).AllowToClear;
    }

    protected override void PopulateTriggerFields()
    {
      base.PopulateTriggerFields();
      string conditionXml = ((CustomAlert) this.AlertConfig.Definition).ConditionXml;
      if (!((conditionXml ?? "") != ""))
        return;
      Dictionary<string, bool> dictionary = new Dictionary<string, bool>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
      FieldFilterList fieldFilterList = FieldFilterList.Parse(conditionXml);
      foreach (string triggerField in this.AlertConfig.TriggerFieldList)
        dictionary[triggerField] = true;
      foreach (FieldFilter fieldFilter in (List<FieldFilter>) fieldFilterList)
      {
        if (!dictionary.ContainsKey(fieldFilter.FieldID))
        {
          this.gvFields.Items.Add(this.CreateTriggerFieldItem(fieldFilter.FieldID));
          dictionary[fieldFilter.FieldID] = true;
        }
      }
    }

    protected override void OnClearAlert(EventArgs e)
    {
      Session.LoanData.DismissAlert(((CustomAlert) this.AlertConfig.Definition).Guid, Session.UserID, DateTime.Now);
      Session.Application.GetService<ILoanEditor>().RefreshLogPanel();
      base.OnClearAlert(e);
    }
  }
}
