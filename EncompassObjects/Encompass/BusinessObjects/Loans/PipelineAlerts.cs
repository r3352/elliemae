// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.PipelineAlerts
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.DataEngine;
using EllieMae.Encompass.Client;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>Summary description for PipelineAlerts.</summary>
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

    /// <summary>Gets the number of alerts in the collection.</summary>
    public int Count => this.alerts.Count;

    /// <summary>
    /// Gets a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.PipelineAlert" /> from the collection based on index.
    /// </summary>
    public PipelineAlert this[int index] => (PipelineAlert) this.alerts[index];

    /// <summary>Provides an enumerator for the collection.</summary>
    /// <returns>An object implementing IEnumerator for iterating over the collection.
    /// </returns>
    public IEnumerator GetEnumerator() => this.alerts.GetEnumerator();

    /// <summary>
    /// Provides a text description of all of the alerts on the loan.
    /// </summary>
    /// <returns>Returns a text description of each alert, seperated by
    /// a CR/LF pair.</returns>
    public override string ToString() => this.pinfo.AlertMsg;
  }
}
