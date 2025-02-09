// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.Cursors.BizPartnerCursor
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Server;
using System.Collections.Generic;

#nullable disable
namespace Elli.Server.Remoting.Cursors
{
  public class BizPartnerCursor : CursorBase
  {
    private const string className = "BizPartnerCursor";
    private bool summaryOnly;
    private string[] fields;
    private UserInfo userInfo;

    public BizPartnerCursor Initialize(
      ISession session,
      int[] contactIds,
      string[] fields,
      bool summaryOnly)
    {
      this.InitializeInternal(session);
      this.summaryOnly = summaryOnly;
      this.userInfo = this.Session.GetUserInfo();
      if (contactIds == null)
        return this;
      this.fields = fields;
      this.insertPrimaryKey();
      for (int index = 0; index < contactIds.Length; ++index)
        this.Items.Add((object) contactIds[index]);
      return this;
    }

    private void insertPrimaryKey()
    {
      List<string> stringList = new List<string>();
      if (this.fields != null)
        stringList.AddRange((IEnumerable<string>) this.fields);
      if (!stringList.Contains("Contact.ContactID"))
        stringList.Add("Contact.ContactID");
      if (!stringList.Contains("Contact.AccessLevel"))
        stringList.Add("Contact.AccessLevel");
      if (!stringList.Contains("Contact.FirstName"))
        stringList.Add("Contact.FirstName");
      if (!stringList.Contains("Contact.LastName"))
        stringList.Add("Contact.LastName");
      if (!stringList.Contains("Contact.WorkPhone"))
        stringList.Add("Contact.WorkPhone");
      if (!stringList.Contains("Contact.HomePhone"))
        stringList.Add("Contact.HomePhone");
      if (!stringList.Contains("Contact.MobilePhone"))
        stringList.Add("Contact.MobilePhone");
      if (!stringList.Contains("Contact.BizEmail"))
        stringList.Add("Contact.BizEmail");
      if (!stringList.Contains("Contact.IsPublic"))
        stringList.Add("Contact.IsPublic");
      this.fields = stringList.ToArray();
    }

    public override object[] GetItems(int startIndex, int count, bool isExternalOrganization)
    {
      TraceLog.WriteApi(nameof (BizPartnerCursor), nameof (GetItems), (object) startIndex, (object) count, (object) isExternalOrganization);
      int[] array = (int[]) this.Items.GetRange(startIndex, count).ToArray(typeof (int));
      return this.summaryOnly ? (object[]) BizPartnerContact.GetBizPartnerSummaries(this.userInfo, array, this.fields) : (object[]) BizPartnerContact.GetBizPartners(array);
    }

    public override object[] GetItems(int startIndex, int count)
    {
      TraceLog.WriteApi(nameof (BizPartnerCursor), nameof (GetItems), (object) startIndex, (object) count);
      int[] array = (int[]) this.Items.GetRange(startIndex, count).ToArray(typeof (int));
      return this.summaryOnly ? (object[]) BizPartnerContact.GetBizPartnerSummaries(this.userInfo, array, this.fields) : (object[]) BizPartnerContact.GetBizPartners(array);
    }

    public override object GetItem(int index) => this.GetItems(index, 1)[0];
  }
}
