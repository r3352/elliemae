// Decompiled with JetBrains decompiler
// Type: ACPDFCREACTIVEX.acPluginMenuClass
// Assembly: Interop.ACPDFCREACTIVEX, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4DA5DA38-3850-4E97-8521-1A0336DA34F6
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\Interop.ACPDFCREACTIVEX.dll

using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.CustomMarshalers;

#nullable disable
namespace ACPDFCREACTIVEX
{
  [TypeLibType(2)]
  [Guid("B4F4009D-5F48-45E5-BA00-6F1F33F107DD")]
  [ClassInterface(0)]
  [ComImport]
  public class acPluginMenuClass : IacPluginMenu, acPluginMenu, IEnumerable
  {
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public extern acPluginMenuClass();

    [DispId(1)]
    public virtual extern string MenuTitle { [DispId(1), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(1), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: MarshalAs(UnmanagedType.BStr), In] set; }

    [DispId(2)]
    public virtual extern int MenuID { [DispId(2), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(2), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(3)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void AddChildMenuItem([MarshalAs(UnmanagedType.Interface), In] acPluginMenu Child);

    [DispId(-4)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof (EnumeratorToEnumVariantMarshaler))]
    public virtual extern IEnumerator GetEnumerator();
  }
}
