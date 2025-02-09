// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.PipelineAlerts
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine;
using EllieMae.Encompass.Client;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  public class PipelineAlerts : IPipelineAlerts, IEnumerable
  {
    private PipelineInfo pinfo;
    private ArrayList alerts = new ArrayList();

    internal PipelineAlerts(Session session, PipelineInfo pinfo)
    {
      this.pinfo = pinfo;
      this.pinfo.UpdateAlerts(session.SessionObjects.CurrentUser.GetUserInfo(), session.SessionObjects.AclGroupManager.GetGroupsOfUser(session.UserID), session.SessionObjects.LoanManager.GetAlertSetupData());
      foreach (PipelineInfo.Alert alert in pinfo.Alerts)
        this.alerts.Add((object) new PipelineAlert(session, pinfo, alert));
    }

    public int Count => this.alerts.Count;

    public PipelineAlert this[int index] => (PipelineAlert) this.alerts[index];

    public IEnumerator GetEnumerator() => this.alerts.GetEnumerator();

    public override string ToString() => this.pinfo.AlertMsg;
  }
}
