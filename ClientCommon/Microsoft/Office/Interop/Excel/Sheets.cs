// Decompiled with JetBrains decompiler
// Type: Microsoft.Office.Interop.Excel.Sheets
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace Microsoft.Office.Interop.Excel
{
  [CompilerGenerated]
  [Guid("000208D7-0000-0000-C000-000000000046")]
  [TypeIdentifier]
  [ComImport]
  public interface Sheets : IEnumerable
  {
    [SpecialName]
    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    sealed extern void _VtblGap1_18();

    [DispId(0)]
    [IndexerName("_Default")]
    object this[[MarshalAs(UnmanagedType.Struct), In] object Index] { [DispId(0), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.IDispatch)] get; }
  }
}
