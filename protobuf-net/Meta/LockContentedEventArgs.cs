// Decompiled with JetBrains decompiler
// Type: ProtoBuf.Meta.LockContentedEventArgs
// Assembly: protobuf-net, Version=2.4.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 4CB0BA27-6305-4C8A-8F23-3BBBCE6B7DCC
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.xml

using System;

#nullable disable
namespace ProtoBuf.Meta
{
  /// <summary>
  /// Contains the stack-trace of the owning code when a lock-contention scenario is detected
  /// </summary>
  public sealed class LockContentedEventArgs : EventArgs
  {
    private readonly string ownerStackTrace;

    internal LockContentedEventArgs(string ownerStackTrace)
    {
      this.ownerStackTrace = ownerStackTrace;
    }

    /// <summary>
    /// The stack-trace of the code that owned the lock when a lock-contention scenario occurred
    /// </summary>
    public string OwnerStackTrace => this.ownerStackTrace;
  }
}
