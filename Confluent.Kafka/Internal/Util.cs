// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.Internal.Util
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

using System;
using System.Text;

#nullable disable
namespace Confluent.Kafka.Internal
{
  internal static class Util
  {
    internal static class Marshal
    {
      /// <summary>Interpret a zero terminated c string as UTF-8.</summary>
      public static unsafe string PtrToStringUTF8(IntPtr strPtr)
      {
        if (strPtr == IntPtr.Zero)
          return (string) null;
        byte* numPtr = (byte*) (void*) strPtr;
        while (*numPtr != (byte) 0)
          ++numPtr;
        int length = (int) (numPtr - (byte*) (void*) strPtr);
        byte[] numArray = new byte[length];
        System.Runtime.InteropServices.Marshal.Copy(strPtr, numArray, 0, length);
        return Encoding.UTF8.GetString(numArray);
      }

      public static T PtrToStructure<T>(IntPtr ptr) => (T) System.Runtime.InteropServices.Marshal.PtrToStructure(ptr, typeof (T));

      public static int SizeOf<T>() => System.Runtime.InteropServices.Marshal.SizeOf(typeof (T));

      public static IntPtr OffsetOf<T>(string fieldName) => System.Runtime.InteropServices.Marshal.OffsetOf(typeof (T), fieldName);
    }
  }
}
