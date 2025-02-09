// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Cursors.Cursor
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

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
  /// <summary>
  /// Provides an interface for collections of objects stored on the Encompass Server
  /// to allow for quick random access without having to pull all of the data to the
  /// client.
  /// </summary>
  /// <remarks>A Cursor represents a list of objects which is held on the Encompass
  /// Server and which the client can then access by directly fetching individual items
  /// or even subsets. This provides a great deal of optimization to operations that would
  /// otherwise require large amounts of data to be transferred to the client.
  /// <p>Every method invocation on a cursor requires a round trip to the Encompass Server,
  /// so you should make every attempt to retrieve the data in as efficient a manner as
  /// possible. For example, you should use the GetItems() method to retrieve
  /// a set of contiguous elements from the cursor instead of invoking the GetItem()
  /// method multiple times, which would be far more expensive.</p>
  /// <p>Because a cursor consumes resources on the server, you should invoke the Cursor's
  /// <see cref="M:EllieMae.Encompass.Cursors.Cursor.Close" /> method when you are done using it. This will release the server
  /// resources immediately. Failure to invoke Close() (or, equivalently, Dispose()) will
  /// cause a resource leak on the server until the <see cref="T:EllieMae.Encompass.Client.Session" /> is closed, at
  /// which point any open cursors for the session are released.</p>
  /// </remarks>
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

    /// <summary>
    /// Finalizer to ensure server side cursor objects are released after client cursors go away
    /// </summary>
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

    /// <summary>Returns the number of items in the cursor.</summary>
    public int Count
    {
      get
      {
        this.ensureValid();
        return this.count;
      }
    }

    /// <summary>
    /// Retrieves the item from the cursor at the specified index.
    /// </summary>
    /// <param name="index">Index of the item to be retrieved (with 0 as the first
    /// index).</param>
    /// <returns>Returns the specified object.</returns>
    public object GetItem(int index)
    {
      this.ensureValid();
      return this.ConvertToItemType(this.cursor.GetItem(index, false));
    }

    /// <summary>
    /// Retrieves a subset of the cursor items starting at a specified index.
    /// </summary>
    /// <param name="startIndex">The index at which to start the subset.</param>
    /// <param name="count">The number of items to retrieve</param>
    /// <returns>Returns an list containing the elements specified.</returns>
    public ListBase GetItems(int startIndex, int count)
    {
      this.ensureValid();
      return this.ConvertToItemList(this.cursor.GetItems(startIndex, count, false));
    }

    /// <summary>
    /// Closes the cursor, releasing any resources allocated by it on the Encompass
    /// Server.
    /// </summary>
    public void Close()
    {
      if (this.cursor == null)
        return;
      this.cursor.Dispose();
      this.cursor = (ICursor) null;
    }

    /// <summary>Provides a fast, efficient enumerator for the cursor.</summary>
    /// <returns>
    /// <p>The enumerator returned by this method is optimized for iterating over the
    /// contents of a Cursor. It pulls the cursor data down in chunks instead of
    /// fetching the items individually in order to minimize round trips to the server
    /// while also preventing large chunks of data from having to be passed back and
    /// forth in any one single transaction.</p>
    /// <p>From the application perspective, the enumerator will return objects
    /// of the underlying type of the Cursor. For example, interating over a
    /// <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.PipelineCursor" />
    /// will return a sequence of <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.PipelineData" />
    /// object.</p>
    /// </returns>
    public IEnumerator GetEnumerator() => (IEnumerator) new CursorEnumerator(this);

    /// <summary>
    /// Implementation of the IDisposable interface's Dispose() method, which invokes the
    /// <see cref="M:EllieMae.Encompass.Cursors.Cursor.Close" /> method if the cursor is not already closed.
    /// </summary>
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
