// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.Cursors.BorrowerCursor
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
  public class BorrowerCursor : CursorBase
  {
    private const string className = "BorrowerCursor";
    private bool summaryOnly;
    private string[] fields;
    private UserInfo userInfo;

    public BorrowerCursor Initialize(
      ISession session,
      int[] contactIds,
      string[] fields,
      bool summaryOnly)
    {
      this.InitializeInternal(session);
      this.summaryOnly = summaryOnly;
      this.fields = fields;
      this.userInfo = session.GetUserInfo();
      this.insertPrimaryKey();
      if (contactIds == null)
        return this;
      for (int index = 0; index < contactIds.Length; ++index)
        this.Items.Add((object) contactIds[index]);
      return this;
    }

    public BorrowerCursor Initialize(
      ISession session,
      int[] contactIds,
      string[] fields,
      bool summaryOnly,
      UserInfo userInfo)
    {
      this.InitializeInternal(session);
      this.summaryOnly = summaryOnly;
      this.fields = fields;
      this.userInfo = userInfo;
      this.insertPrimaryKey();
      if (contactIds == null)
        return this;
      for (int index = 0; index < contactIds.Length; ++index)
        this.Items.Add((object) contactIds[index]);
      return this;
    }

    private void insertPrimaryKey()
    {
      bool flag1 = false;
      bool flag2 = false;
      bool flag3 = false;
      if (this.fields != null)
      {
        foreach (string field in this.fields)
        {
          switch (field)
          {
            case "Contact.ContactID":
              flag1 = true;
              break;
            case "Contact.AccessLevel":
              flag2 = true;
              break;
            case "Contact.OwnerID":
              flag3 = true;
              break;
          }
        }
      }
      List<string> stringList = this.fields == null ? new List<string>() : new List<string>((IEnumerable<string>) this.fields);
      if (!flag1)
        stringList.Add("Contact.ContactID");
      if (!flag2)
        stringList.Add("Contact.AccessLevel");
      if (!flag3)
        stringList.Add("Contact.OwnerID");
      this.fields = stringList.ToArray();
    }

    public override object[] GetItems(int startIndex, int count, bool isExternalOrganization)
    {
      TraceLog.WriteApi(nameof (BorrowerCursor), nameof (GetItems), (object) startIndex, (object) count, (object) isExternalOrganization);
      int[] array = (int[]) this.Items.GetRange(startIndex, count).ToArray(typeof (int));
      return this.summaryOnly ? (object[]) BorrowerContact.GetBorrowerSummaries(this.userInfo, array, this.fields) : (object[]) BorrowerContact.GetBorrowers(array);
    }

    public override object[] GetItems(int startIndex, int count)
    {
      TraceLog.WriteApi(nameof (BorrowerCursor), nameof (GetItems), (object) startIndex, (object) count);
      int[] array = (int[]) this.Items.GetRange(startIndex, count).ToArray(typeof (int));
      return this.summaryOnly ? (object[]) BorrowerContact.GetBorrowerSummaries(this.userInfo, array, this.fields) : (object[]) BorrowerContact.GetBorrowers(array);
    }

    public override object GetItem(int index) => this.GetItems(index, 1)[0];
  }
}
