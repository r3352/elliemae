// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Customization.RemotableDataSource
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using System;
using System.Runtime.Remoting;

#nullable disable
namespace EllieMae.EMLite.Customization
{
  public abstract class RemotableDataSource : MarshalByRefObject, ICloneable, IDisposable
  {
    private bool readOnly;

    protected RemotableDataSource(bool readOnly) => this.readOnly = readOnly;

    public bool ReadOnly => this.readOnly;

    protected void EnsureWritable()
    {
      if (this.ReadOnly)
        throw new InvalidOperationException("The current context does not allow data to be modified.");
    }

    public virtual void Dispose()
    {
      try
      {
        RemotingServices.Disconnect((MarshalByRefObject) this);
      }
      catch
      {
      }
    }

    public override object InitializeLifetimeService() => (object) null;

    public abstract object Clone();
  }
}
