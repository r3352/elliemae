// Decompiled with JetBrains decompiler
// Type: ACPDFCREACTIVEX._IPDFCreactiveXEvents
// Assembly: Interop.ACPDFCREACTIVEX, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4DA5DA38-3850-4E97-8521-1A0336DA34F6
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\Interop.ACPDFCREACTIVEX.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace ACPDFCREACTIVEX
{
  [Guid("5E014D76-60FD-4436-8B2F-D1D5A0784552")]
  [TypeLibType(4096)]
  [InterfaceType(2)]
  [ComImport]
  public interface _IPDFCreactiveXEvents
  {
    [DispId(1)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void BeforeDelete([In, Out] ref int Continue);

    [DispId(2)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void PrintPage(int PageNumber, [In, Out] ref int Continue);

    [DispId(3)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SavePage(int PageNumber, [In, Out] ref int Continue);

    [DispId(4)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void ClickHyperlink([MarshalAs(UnmanagedType.BStr)] string Object, [MarshalAs(UnmanagedType.BStr)] string Hyperlink, [In, Out] ref int Continue);

    [DispId(5)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void Refresh();

    [DispId(6)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SelectedObjectChange();

    [DispId(7)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void ObjectTextChange([MarshalAs(UnmanagedType.IDispatch), In] object pObject);

    [DispId(8)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void ContextSensitiveMenu([In, Out] ref int Continue);

    [DispId(9)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void MouseDown([MarshalAs(UnmanagedType.Interface), In] acObject pObject, [In] int xPos, [In] int yPos, [In, Out] ref int Continue);

    [DispId(10)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void MouseMove([MarshalAs(UnmanagedType.Interface), In] acObject pObject, [In] int xPos, [In] int yPos);

    [DispId(11)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void MouseUp([MarshalAs(UnmanagedType.Interface), In] acObject pObject, [In] int xPos, [In] int yPos);

    [DispId(12)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void NewObject([MarshalAs(UnmanagedType.Interface), In] acObject pObject, [In, Out] ref int Continue);

    [DispId(13)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void ActivateObject([MarshalAs(UnmanagedType.IDispatch), In] object pObject, [In, Out] ref int Continue);

    [DispId(14)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void LoadPage(int PageNumber, [In, Out] ref int Continue);

    [DispId(15)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void EvaluateExpression([MarshalAs(UnmanagedType.IDispatch), In] object pObject, [MarshalAs(UnmanagedType.BStr), In, Out] ref string result, [In, Out] ref int Continue);

    [DispId(16)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void ProcessingProgress([In] int TotalSteps, [In] int CurrentStep, [In, Out] ref int Continue);
  }
}
