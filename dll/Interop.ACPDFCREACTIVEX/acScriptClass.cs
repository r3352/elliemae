// Decompiled with JetBrains decompiler
// Type: ACPDFCREACTIVEX.acScriptClass
// Assembly: Interop.ACPDFCREACTIVEX, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4DA5DA38-3850-4E97-8521-1A0336DA34F6
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\Interop.ACPDFCREACTIVEX.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace ACPDFCREACTIVEX
{
  [ClassInterface(0)]
  [Guid("020FEA15-F7F4-4591-AA59-DA77170C90C8")]
  [TypeLibType(2)]
  [ComImport]
  public class acScriptClass : IacScript, acScript
  {
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public extern acScriptClass();

    [DispId(1610678272)]
    public virtual extern short Pass { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [return: MarshalAs(UnmanagedType.Struct)]
    public virtual extern object get_Params([MarshalAs(UnmanagedType.BStr)] string Name);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void set_Params([MarshalAs(UnmanagedType.BStr)] string Name, [MarshalAs(UnmanagedType.Struct), In] object pVal);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void SetLabel([MarshalAs(UnmanagedType.BStr)] string Label);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void GotoLabel([MarshalAs(UnmanagedType.BStr)] string Name);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [return: MarshalAs(UnmanagedType.BStr)]
    public virtual extern string get_Procedure([MarshalAs(UnmanagedType.BStr)] string Name);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void set_Procedure([MarshalAs(UnmanagedType.BStr)] string Name, [MarshalAs(UnmanagedType.BStr), In] string pVal);

    [DispId(1610678280)]
    public virtual extern string LastObject { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: MarshalAs(UnmanagedType.BStr), In] set; }
  }
}
