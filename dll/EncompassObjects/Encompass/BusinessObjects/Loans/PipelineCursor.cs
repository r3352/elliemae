// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.PipelineCursor
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataEngine;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.Collections;
using EllieMae.Encompass.Cursors;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  public class PipelineCursor : Cursor, IPipelineCursor
  {
    private bool canAccessArchiveLoans;
    private bool loanSoftArchivalenabled;

    internal PipelineCursor(Session session, ICursor cursor)
      : base(session, cursor)
    {
      this.canAccessArchiveLoans = this.Session.GetUserInfo().IsAdministrator() || (bool) session.SessionObjects.StartupInfo.UserAclFeatureRights[(object) (AclFeature) 233];
      this.loanSoftArchivalenabled = this.Session.SessionObjects.StartupInfo.EnableLoanSoftArchival;
    }

    public PipelineData GetItem(int index) => (PipelineData) base.GetItem(index);

    public PipelineDataList GetItems(int startIndex, int count)
    {
      return (PipelineDataList) base.GetItems(startIndex, count);
    }

    internal override object ConvertToItemType(object item)
    {
      return (object) new PipelineData(this.Session, (PipelineInfo) item);
    }

    internal override ListBase ConvertToItemList(object[] items)
    {
      return (ListBase) PipelineData.ToList(this.Session, (PipelineInfo[]) items);
    }
  }
}
