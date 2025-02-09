// Decompiled with JetBrains decompiler
// Type: ACPDFCREACTIVEX.acEnumAttributesClass
// Assembly: Interop.ACPDFCREACTIVEX, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4DA5DA38-3850-4E97-8521-1A0336DA34F6
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\Interop.ACPDFCREACTIVEX.dll

using stdole;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.CustomMarshalers;

#nullable disable
namespace ACPDFCREACTIVEX
{
  [ClassInterface(0)]
  [Guid("94D36997-4B46-44E1-9E73-BB523C734781")]
  [ComImport]
  public class acEnumAttributesClass : IEnumVARIANT, acEnumAttributes
  {
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    internal extern acEnumAttributesClass();

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void Next([In] uint celt, [MarshalAs(UnmanagedType.Struct), In] ref object rgvar, out uint pceltFetched);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void Skip([In] uint celt);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void Reset();

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void Clone([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof (EnumeratorToEnumVariantMarshaler))] out IEnumerator ppenum);
  }
}
