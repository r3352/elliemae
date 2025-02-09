// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.MailMergeJobHandler
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.JobService;
using EllieMae.EMLite.Serialization;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public class MailMergeJobHandler : JobHandler
  {
    private ArrayList processedIds = new ArrayList();

    protected override object ParseParameters(string jobData)
    {
      return new XmlSerializer().Deserialize(jobData, typeof (MailMergeJobParameters));
    }

    protected override void ProcessJob()
    {
      ContactUtils.MailMerge += new MailMergeEventHandler(this.onMailMerge);
      try
      {
        MailMergeJobParameters parameters = (MailMergeJobParameters) this.Parameters;
        string str = string.Concat(((XmlHashtable) this.State)["ids"]);
        if (str != "")
          this.processedIds.AddRange((ICollection) str.Split(','));
        Hashtable hashtable = new Hashtable();
        foreach (string processedId in this.processedIds)
          hashtable[(object) int.Parse(processedId)] = (object) true;
        ArrayList arrayList = new ArrayList();
        foreach (int contactId in parameters.ContactIDs)
        {
          if (!hashtable.Contains((object) contactId))
            arrayList.Add((object) contactId);
        }
        ContactUtils.DoMailMerge((int[]) arrayList.ToArray(typeof (int)), parameters.ContactType, FileSystemEntry.Parse(parameters.TemplatePath), false, true, parameters.Subject, parameters.EmailAddressOption, parameters.SenderUserID);
      }
      finally
      {
        ContactUtils.MailMerge -= new MailMergeEventHandler(this.onMailMerge);
        ((XmlHashtable) this.State)["ids"] = (object) string.Join(",", (string[]) this.processedIds.ToArray(typeof (string)));
        this.NotifyStateChanged();
      }
    }

    private void onMailMerge(MailMergeEventArgs e)
    {
      foreach (int contactId in e.ContactIDs)
        this.processedIds.Add((object) string.Concat((object) contactId));
    }
  }
}
