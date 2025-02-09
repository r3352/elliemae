// Decompiled with JetBrains decompiler
// Type: ACPDFCREACTIVEX.IacScript
// Assembly: Interop.ACPDFCREACTIVEX, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4DA5DA38-3850-4E97-8521-1A0336DA34F6
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\Interop.ACPDFCREACTIVEX.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace ACPDFCREACTIVEX
{
  [InterfaceType(1)]
  [Guid("3DA80648-E234-4517-9BDD-14314F94A737")]
  [ComImport]
  public interface IacScript
  {
    [DispId(1610678272)]
    short Pass { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [return: MarshalAs(UnmanagedType.Struct)]
    object get_Params([MarshalAs(UnmanagedType.BStr)] string Name);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void set_Params([MarshalAs(UnmanagedType.BStr)] string Name, [MarshalAs(UnmanagedType.Struct), In] object pVal);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetLabel([MarshalAs(UnmanagedType.BStr)] string Label);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GotoLabel([MarshalAs(UnmanagedType.BStr)] string Name);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [return: MarshalAs(UnmanagedType.BStr)]
    string get_Procedure([MarshalAs(UnmanagedType.BStr)] string Name);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void set_Procedure([MarshalAs(UnmanagedType.BStr)] string Name, [MarshalAs(UnmanagedType.BStr), In] string pVal);

    [DispId(1610678280)]
    string LastObject { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: MarshalAs(UnmanagedType.BStr), In] set; }
  }
}
