// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Client.SessionBoundObject
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Client
{
  /// <summary>
  /// Provides a base class for all objects which are tied to a given Session.
  /// </summary>
  [ComVisible(false)]
  public abstract class SessionBoundObject
  {
    private Session session;

    /// <summary>SessionBoundObject</summary>
    /// <param name="session"></param>
    protected internal SessionBoundObject(Session session) => this.session = session;

    /// <summary>
    /// Gets the Session object to which the current object is attached.
    /// </summary>
    public Session Session => this.session;
  }
}
