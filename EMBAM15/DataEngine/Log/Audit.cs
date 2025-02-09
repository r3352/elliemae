// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.Audit
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common.Xml.AutoMapping;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class Audit
  {
    private ECloseLog _parent;

    static Audit()
    {
      XmlAutoMapper.AddProfile<Audit>(XmlAutoMapper.NewProfile<Audit>().ForCollectionMember<Alert>((Expression<Func<Audit, IList<Alert>>>) (dt => dt.Alerts), (Action<XmlAutoMapper.Profile<Audit>.ProfileOptions<IList<Alert>, Alert>>) (opts => opts.CreateItemUsing((Func<IXmlMapperContext, Audit, Alert>) ((xmlElement, parent) => new Alert(parent._parent))))));
    }

    public Audit() => this.Alerts = (IList<Alert>) new List<Alert>();

    public string Id { get; set; }

    public DateTime AuditTime { get; set; }

    public string FileKey { get; set; }

    public IList<Alert> Alerts { get; set; }

    public Audit(ECloseLog parent)
      : this()
    {
      this._parent = parent;
    }

    public Alert[] GetAlertsBySource(string alertSource)
    {
      List<Alert> alertList = new List<Alert>();
      foreach (Alert alert in (IEnumerable<Alert>) this.Alerts)
      {
        if (alert.Source == alertSource)
          alertList.Add(alert);
      }
      return alertList.ToArray();
    }
  }
}
