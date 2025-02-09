// Decompiled with JetBrains decompiler
// Type: CDIntfEx.IDIDocument
// Assembly: Interop.CDIntfEx, Version=2.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E0D8D59F-38F8-4E65-9D3A-50B747C0491E
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\Interop.CDIntfEx.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace CDIntfEx
{
  [InterfaceType(2)]
  [TypeLibType(4096)]
  [Guid("D307A698-F048-4AFC-9170-572440CD523F")]
  [ComImport]
  public interface IDIDocument
  {
    [DispId(1)]
    string Title { [DispId(1), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(1), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(2)]
    string Subject { [DispId(2), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(2), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(3)]
    string Creator { [DispId(3), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(3), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(4)]
    string Author { [DispId(4), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(4), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(5)]
    string KeyWords { [DispId(5), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(5), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(19)]
    int Linearized { [DispId(19), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(19), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(20)]
    string PageMode { [DispId(20), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(20), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(21)]
    int Rotate { [DispId(21), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(21), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(6)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    bool Open([MarshalAs(UnmanagedType.BStr)] string FileName);

    [DispId(7)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    bool Save([MarshalAs(UnmanagedType.BStr)] string FileName);

    [DispId(8)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    bool Append([MarshalAs(UnmanagedType.BStr)] string FileName);

    [DispId(9)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    bool AppendEx([MarshalAs(UnmanagedType.IDispatch)] object Document);

    [DispId(10)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    bool SetBookmark(int Page, [MarshalAs(UnmanagedType.BStr)] string Text, int Level);

    [DispId(11)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void ClearBookmarks();

    [DispId(12)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    bool SearchText(short Start, [MarshalAs(UnmanagedType.BStr)] string Text, ref int Page, ref double xPos, ref double yPos);

    [DispId(13)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    bool Merge([MarshalAs(UnmanagedType.BStr)] string FileName, int Options);

    [DispId(14)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    bool MergeEx([MarshalAs(UnmanagedType.IDispatch)] object Document, int Options);

    [DispId(15)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    bool Encrypt([MarshalAs(UnmanagedType.BStr)] string OwnerPassword, [MarshalAs(UnmanagedType.BStr)] string UserPassword, int Permissions);

    [DispId(16)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    int PageCount();

    [DispId(17)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    bool OpenEx([MarshalAs(UnmanagedType.BStr)] string FileName, [MarshalAs(UnmanagedType.BStr)] string Password);

    [DispId(18)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetFlateCompression(short Ratio);

    [DispId(22)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void Optimize(short Level);

    [DispId(23)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    bool ExportToRTF([MarshalAs(UnmanagedType.BStr)] string FileName, acRtfExportOptions RtfOption, int UseTabs);

    [DispId(24)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    bool SetLicenseKey([MarshalAs(UnmanagedType.BStr)] string Company, [MarshalAs(UnmanagedType.BStr)] string LicKey);

    [DispId(25)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    bool Print([MarshalAs(UnmanagedType.BStr)] string PrinterName, int StartPage, int EndPage, int Copies);

    [DispId(26)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    bool ExportToHTML([MarshalAs(UnmanagedType.BStr)] string FileName, acHtmlExportOptions HtmlOption);

    [DispId(27)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    bool ExportToEXCEL([MarshalAs(UnmanagedType.BStr)] string FileName, acExcelExportOptions ExcelOption);

    [DispId(28)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    bool ExportToJPEG([MarshalAs(UnmanagedType.BStr)] string FileName, acJPegExportOptions JPegOption);

    [DispId(29)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    bool Encrypt128([MarshalAs(UnmanagedType.BStr)] string OwnerPassword, [MarshalAs(UnmanagedType.BStr)] string UserPassword, int Permissions);
  }
}
