// Decompiled with JetBrains decompiler
// Type: ACPDFCREACTIVEX._IPDFCreactiveXEvents_Event
// Assembly: Interop.ACPDFCREACTIVEX, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4DA5DA38-3850-4E97-8521-1A0336DA34F6
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\Interop.ACPDFCREACTIVEX.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace ACPDFCREACTIVEX
{
  [ComEventInterface(typeof (_IPDFCreactiveXEvents), typeof (_IPDFCreactiveXEvents_EventProvider))]
  [ComVisible(false)]
  [TypeLibType(16)]
  public interface _IPDFCreactiveXEvents_Event
  {
    event _IPDFCreactiveXEvents_BeforeDeleteEventHandler BeforeDelete;

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void add_BeforeDelete(
      [In] _IPDFCreactiveXEvents_BeforeDeleteEventHandler obj0);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void remove_BeforeDelete(
      [In] _IPDFCreactiveXEvents_BeforeDeleteEventHandler obj0);

    event _IPDFCreactiveXEvents_PrintPageEventHandler PrintPage;

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void add_PrintPage([In] _IPDFCreactiveXEvents_PrintPageEventHandler obj0);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void remove_PrintPage([In] _IPDFCreactiveXEvents_PrintPageEventHandler obj0);

    event _IPDFCreactiveXEvents_SavePageEventHandler SavePage;

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void add_SavePage([In] _IPDFCreactiveXEvents_SavePageEventHandler obj0);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void remove_SavePage([In] _IPDFCreactiveXEvents_SavePageEventHandler obj0);

    event _IPDFCreactiveXEvents_ClickHyperlinkEventHandler ClickHyperlink;

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void add_ClickHyperlink(
      [In] _IPDFCreactiveXEvents_ClickHyperlinkEventHandler obj0);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void remove_ClickHyperlink(
      [In] _IPDFCreactiveXEvents_ClickHyperlinkEventHandler obj0);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void add_Refresh([In] _IPDFCreactiveXEvents_RefreshEventHandler obj0);

    event _IPDFCreactiveXEvents_RefreshEventHandler Refresh;

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void remove_Refresh([In] _IPDFCreactiveXEvents_RefreshEventHandler obj0);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void add_SelectedObjectChange(
      [In] _IPDFCreactiveXEvents_SelectedObjectChangeEventHandler obj0);

    event _IPDFCreactiveXEvents_SelectedObjectChangeEventHandler SelectedObjectChange;

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void remove_SelectedObjectChange(
      [In] _IPDFCreactiveXEvents_SelectedObjectChangeEventHandler obj0);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void add_ObjectTextChange(
      [In] _IPDFCreactiveXEvents_ObjectTextChangeEventHandler obj0);

    event _IPDFCreactiveXEvents_ObjectTextChangeEventHandler ObjectTextChange;

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void remove_ObjectTextChange(
      [In] _IPDFCreactiveXEvents_ObjectTextChangeEventHandler obj0);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void add_ContextSensitiveMenu(
      [In] _IPDFCreactiveXEvents_ContextSensitiveMenuEventHandler obj0);

    event _IPDFCreactiveXEvents_ContextSensitiveMenuEventHandler ContextSensitiveMenu;

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void remove_ContextSensitiveMenu(
      [In] _IPDFCreactiveXEvents_ContextSensitiveMenuEventHandler obj0);

    event _IPDFCreactiveXEvents_MouseDownEventHandler MouseDown;

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void add_MouseDown([In] _IPDFCreactiveXEvents_MouseDownEventHandler obj0);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void remove_MouseDown([In] _IPDFCreactiveXEvents_MouseDownEventHandler obj0);

    event _IPDFCreactiveXEvents_MouseMoveEventHandler MouseMove;

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void add_MouseMove([In] _IPDFCreactiveXEvents_MouseMoveEventHandler obj0);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void remove_MouseMove([In] _IPDFCreactiveXEvents_MouseMoveEventHandler obj0);

    event _IPDFCreactiveXEvents_MouseUpEventHandler MouseUp;

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void add_MouseUp([In] _IPDFCreactiveXEvents_MouseUpEventHandler obj0);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void remove_MouseUp([In] _IPDFCreactiveXEvents_MouseUpEventHandler obj0);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void add_NewObject([In] _IPDFCreactiveXEvents_NewObjectEventHandler obj0);

    event _IPDFCreactiveXEvents_NewObjectEventHandler NewObject;

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void remove_NewObject([In] _IPDFCreactiveXEvents_NewObjectEventHandler obj0);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void add_ActivateObject(
      [In] _IPDFCreactiveXEvents_ActivateObjectEventHandler obj0);

    event _IPDFCreactiveXEvents_ActivateObjectEventHandler ActivateObject;

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void remove_ActivateObject(
      [In] _IPDFCreactiveXEvents_ActivateObjectEventHandler obj0);

    event _IPDFCreactiveXEvents_LoadPageEventHandler LoadPage;

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void add_LoadPage([In] _IPDFCreactiveXEvents_LoadPageEventHandler obj0);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void remove_LoadPage([In] _IPDFCreactiveXEvents_LoadPageEventHandler obj0);

    event _IPDFCreactiveXEvents_EvaluateExpressionEventHandler EvaluateExpression;

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void add_EvaluateExpression(
      [In] _IPDFCreactiveXEvents_EvaluateExpressionEventHandler obj0);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void remove_EvaluateExpression(
      [In] _IPDFCreactiveXEvents_EvaluateExpressionEventHandler obj0);

    event _IPDFCreactiveXEvents_ProcessingProgressEventHandler ProcessingProgress;

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void add_ProcessingProgress(
      [In] _IPDFCreactiveXEvents_ProcessingProgressEventHandler obj0);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void remove_ProcessingProgress(
      [In] _IPDFCreactiveXEvents_ProcessingProgressEventHandler obj0);
  }
}
