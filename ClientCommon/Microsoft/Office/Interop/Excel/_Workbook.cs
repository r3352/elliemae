// Decompiled with JetBrains decompiler
// Type: Microsoft.Office.Interop.Excel._Workbook
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace Microsoft.Office.Interop.Excel
{
  [CompilerGenerated]
  [Guid("000208DA-0000-0000-C000-000000000046")]
  [TypeIdentifier]
  [ComImport]
  public interface _Workbook
  {
    [SpecialName]
    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    sealed extern void _VtblGap1_20();

    [LCIDConversion(3)]
    [DispId(277)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void Close([MarshalAs(UnmanagedType.Struct), In, Optional] object SaveChanges, [MarshalAs(UnmanagedType.Struct), In, Optional] object Filename, [MarshalAs(UnmanagedType.Struct), In, Optional] object RouteWorkbook);

    [SpecialName]
    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    sealed extern void _VtblGap2_84();

    [DispId(485)]
    Sheets Sheets { [DispId(485), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }
  }
}
