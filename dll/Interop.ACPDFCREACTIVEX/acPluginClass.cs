// Decompiled with JetBrains decompiler
// Type: ACPDFCREACTIVEX.acPluginClass
// Assembly: Interop.ACPDFCREACTIVEX, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4DA5DA38-3850-4E97-8521-1A0336DA34F6
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\Interop.ACPDFCREACTIVEX.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace ACPDFCREACTIVEX
{
  [TypeLibType(2)]
  [ClassInterface(0)]
  [Guid("A9AAEA33-762A-4422-9B18-F564408BF391")]
  [ComImport]
  public class acPluginClass : IacPlugin, acPlugin
  {
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public extern acPluginClass();

    [ComAliasName("ACPDFCREACTIVEX.CommandToolConstants")]
    [DispId(1610678272)]
    public virtual extern CommandToolConstants PluginID { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: ComAliasName("ACPDFCREACTIVEX.CommandToolConstants")] get; [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: ComAliasName("ACPDFCREACTIVEX.CommandToolConstants"), In] set; }

    [DispId(1610678274)]
    public virtual extern object PDF { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: MarshalAs(UnmanagedType.IDispatch), In] set; }

    [DispId(1610678275)]
    public virtual extern object MenuItems { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.IDispatch)] get; }

    [DispId(1610678276)]
    public virtual extern string PluginName { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: MarshalAs(UnmanagedType.BStr), In] set; }

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [return: MarshalAs(UnmanagedType.Interface)]
    public virtual extern acPluginMenu NewPluginMenuItem(
      [MarshalAs(UnmanagedType.BStr), In] string Title,
      [In] int Id,
      [MarshalAs(UnmanagedType.Interface), In] acPluginMenu parentMenuItem);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void DoCommandId([In] int Id);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void DoDefaultCommandDialog();

    [DispId(1610678281)]
    public virtual extern acPlugin OuterPlugin { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: MarshalAs(UnmanagedType.Interface), In] set; }

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void DoDefaultCommand();

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void ProcessMessage(ref tagMSG pMsg);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void LoadPluginOptionsFromRegistry();

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void SavePluginOptionsToRegistry();

    [DispId(1610678286)]
    public virtual extern string OptionsRegistryKey { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: MarshalAs(UnmanagedType.BStr), In] set; }

    [DispId(1610678288)]
    public virtual extern object PluginOptions { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Struct)] get; [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: MarshalAs(UnmanagedType.Struct), In] set; }
  }
}
