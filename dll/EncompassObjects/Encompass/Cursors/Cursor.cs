// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Cursors.Cursor
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.Collections;
using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Cursors
{
  [ComVisible(false)]
  public abstract class Cursor : SessionBoundObject, IEnumerable, IDisposable
  {
    private ICursor cursor;
    private int count;

    internal Cursor(Session session, ICursor cursor)
      : base(session)
    {
      this.cursor = cursor;
      this.count = cursor.GetItemCount();
    }

    ~Cursor()
    {
      try
      {
        if (!this.Session.IsConnected)
          return;
        this.Close();
      }
      catch (Exception ex)
      {
        try
        {
          new EventLog()
          {
            Log = "Application",
            Source = "Encompass SDK"
          }.WriteEntry("Encompass SDK Cursor.Finalize() failed:" + Environment.NewLine + Environment.NewLine + ex.StackTrace, EventLogEntryType.Warning);
        }
        catch
        {
        }
      }
      finally
      {
        // ISSUE: explicit finalizer call
        base.Finalize();
      }
    }

    public int Count
    {
      get
      {
        this.ensureValid();
        return this.count;
      }
    }

    public object GetItem(int index)
    {
      this.ensureValid();
      return this.ConvertToItemType(this.cursor.GetItem(index, false));
    }

    public ListBase GetItems(int startIndex, int count)
    {
      this.ensureValid();
      return this.ConvertToItemList(this.cursor.GetItems(startIndex, count, false));
    }

    public void Close()
    {
      if (this.cursor == null)
        return;
      ((IDisposable) this.cursor).Dispose();
      this.cursor = (ICursor) null;
    }

    public IEnumerator GetEnumerator() => (IEnumerator) new CursorEnumerator(this);

    void IDisposable.Dispose()
    {
      this.Close();
      GC.SuppressFinalize((object) this);
    }

    private void ensureValid()
    {
      if (this.cursor == null)
        throw new ObjectDisposedException(this.GetType().Name);
    }

    internal abstract object ConvertToItemType(object item);

    internal abstract ListBase ConvertToItemList(object[] items);

    internal ICursor Unwrap() => this.cursor;
  }
}
