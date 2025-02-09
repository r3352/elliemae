// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.Impl.SafeHandleZeroIsInvalid
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Confluent.Kafka.Impl
{
  internal abstract class SafeHandleZeroIsInvalid : SafeHandle
  {
    private string handleName;

    internal SafeHandleZeroIsInvalid(string handleName)
      : base(IntPtr.Zero, true)
    {
      this.handleName = handleName;
    }

    internal SafeHandleZeroIsInvalid(string handleName, bool ownsHandle)
      : base(IntPtr.Zero, ownsHandle)
    {
      this.handleName = handleName;
    }

    public override bool IsInvalid => this.handle == IntPtr.Zero;
  }
}
