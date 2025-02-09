// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.Cursors.CursorBase
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using Elli.Server.Remoting.SessionObjects;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Server;
using System;
using System.Collections;

#nullable disable
namespace Elli.Server.Remoting.Cursors
{
  public abstract class CursorBase : SessionBoundObject, ICursor, IDisposable
  {
    private ArrayList items = new ArrayList();

    protected ArrayList Items => this.items;

    protected void AddRange(object[] cursorItems)
    {
      for (int index = 0; index < this.items.Count; ++index)
        this.items.Add(this.items[index]);
    }

    internal int Count => this.items.Count;

    public virtual int GetItemCount()
    {
      CursorBase.CursorApi(this.Session, this.GetType().Name, nameof (GetItemCount));
      return this.items.Count;
    }

    public virtual int GetItemCount(int sqlRead) => this.GetItemCount();

    public virtual object[] GetItems(int startIndex, int count, bool isExternalOrganization)
    {
      CursorBase.CursorApi(this.Session, this.GetType().Name, nameof (GetItems), (object) startIndex, (object) count);
      return this.items.GetRange(startIndex, count).ToArray();
    }

    public virtual object[] GetItems(
      int startIndex,
      int count,
      bool isExternalOrganization,
      int sqlRead)
    {
      CursorBase.CursorApi(this.Session, this.GetType().Name, nameof (GetItems), (object) startIndex, (object) count);
      return this.items.GetRange(startIndex, count).ToArray();
    }

    public virtual object[] GetItems(
      int startIndex,
      int count,
      bool isExternalOrganization,
      int sqlRead,
      bool excludeArchivedLoans)
    {
      CursorBase.CursorApi(this.Session, this.GetType().Name, nameof (GetItems), (object) startIndex, (object) count);
      return this.items.GetRange(startIndex, count).ToArray();
    }

    public virtual object GetItem(
      int index,
      bool isExternalOrganization,
      bool excludeArchivedLoans)
    {
      CursorBase.CursorApi(this.Session, this.GetType().Name, nameof (GetItem), (object) index);
      return this.items[index];
    }

    public virtual object GetItem(int index, bool isExternalOrganization)
    {
      CursorBase.CursorApi(this.Session, this.GetType().Name, nameof (GetItem), (object) index);
      return this.items[index];
    }

    public virtual object[] GetItems(int startIndex, int count)
    {
      CursorBase.CursorApi(this.Session, this.GetType().Name, nameof (GetItems), (object) startIndex, (object) count);
      return this.items.GetRange(startIndex, count).ToArray();
    }

    public virtual object GetItem(int index)
    {
      CursorBase.CursorApi(this.Session, this.GetType().Name, nameof (GetItem), (object) index);
      return this.items[index];
    }

    public override void Dispose()
    {
      CursorBase.CursorApi(this.Session, this.GetType().Name, nameof (Dispose));
      this.items = (ArrayList) null;
      base.Dispose();
    }

    internal static void CursorApi(
      ISession session,
      string className,
      string apiName,
      params object[] parms)
    {
      if (ClientContext.GetCurrent(false) == null)
        return;
      ClientContext.GetCurrent().RecordClassName(className);
      ClientContext.GetCurrent().RecordApiName(apiName);
      ClientContext.GetCurrent().RecordParms(parms);
      ClientContext.GetCurrent().RecordSession(session);
    }
  }
}
