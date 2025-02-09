// Decompiled with JetBrains decompiler
// Type: ACPDFCREACTIVEX._IPDFCreactiveXEvents_SinkHelper
// Assembly: Interop.ACPDFCREACTIVEX, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4DA5DA38-3850-4E97-8521-1A0336DA34F6
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\Interop.ACPDFCREACTIVEX.dll

using System.Runtime.InteropServices;

#nullable disable
namespace ACPDFCREACTIVEX
{
  [ClassInterface(ClassInterfaceType.None)]
  [TypeLibType(TypeLibTypeFlags.FHidden)]
  public sealed class _IPDFCreactiveXEvents_SinkHelper : _IPDFCreactiveXEvents
  {
    public _IPDFCreactiveXEvents_BeforeDeleteEventHandler m_BeforeDeleteDelegate;
    public _IPDFCreactiveXEvents_PrintPageEventHandler m_PrintPageDelegate;
    public _IPDFCreactiveXEvents_SavePageEventHandler m_SavePageDelegate;
    public _IPDFCreactiveXEvents_ClickHyperlinkEventHandler m_ClickHyperlinkDelegate;
    public _IPDFCreactiveXEvents_RefreshEventHandler m_RefreshDelegate;
    public _IPDFCreactiveXEvents_SelectedObjectChangeEventHandler m_SelectedObjectChangeDelegate;
    public _IPDFCreactiveXEvents_ObjectTextChangeEventHandler m_ObjectTextChangeDelegate;
    public _IPDFCreactiveXEvents_ContextSensitiveMenuEventHandler m_ContextSensitiveMenuDelegate;
    public _IPDFCreactiveXEvents_MouseDownEventHandler m_MouseDownDelegate;
    public _IPDFCreactiveXEvents_MouseMoveEventHandler m_MouseMoveDelegate;
    public _IPDFCreactiveXEvents_MouseUpEventHandler m_MouseUpDelegate;
    public _IPDFCreactiveXEvents_NewObjectEventHandler m_NewObjectDelegate;
    public _IPDFCreactiveXEvents_ActivateObjectEventHandler m_ActivateObjectDelegate;
    public _IPDFCreactiveXEvents_LoadPageEventHandler m_LoadPageDelegate;
    public _IPDFCreactiveXEvents_EvaluateExpressionEventHandler m_EvaluateExpressionDelegate;
    public _IPDFCreactiveXEvents_ProcessingProgressEventHandler m_ProcessingProgressDelegate;
    public int m_dwCookie;

    public virtual void BeforeDelete([In] ref int obj0)
    {
      if (this.m_BeforeDeleteDelegate == null)
        return;
      this.m_BeforeDeleteDelegate(ref obj0);
    }

    public virtual void PrintPage([In] int obj0, [In] ref int obj1)
    {
      if (this.m_PrintPageDelegate == null)
        return;
      this.m_PrintPageDelegate(obj0, ref obj1);
    }

    public virtual void SavePage([In] int obj0, [In] ref int obj1)
    {
      if (this.m_SavePageDelegate == null)
        return;
      this.m_SavePageDelegate(obj0, ref obj1);
    }

    public virtual void ClickHyperlink([In] string obj0, [In] string obj1, [In] ref int obj2)
    {
      if (this.m_ClickHyperlinkDelegate == null)
        return;
      this.m_ClickHyperlinkDelegate(obj0, obj1, ref obj2);
    }

    public virtual void Refresh()
    {
      if (this.m_RefreshDelegate == null)
        return;
      this.m_RefreshDelegate();
    }

    public virtual void SelectedObjectChange()
    {
      if (this.m_SelectedObjectChangeDelegate == null)
        return;
      this.m_SelectedObjectChangeDelegate();
    }

    public virtual void ObjectTextChange([In] object obj0)
    {
      if (this.m_ObjectTextChangeDelegate == null)
        return;
      this.m_ObjectTextChangeDelegate(obj0);
    }

    public virtual void ContextSensitiveMenu([In] ref int obj0)
    {
      if (this.m_ContextSensitiveMenuDelegate == null)
        return;
      this.m_ContextSensitiveMenuDelegate(ref obj0);
    }

    public virtual void MouseDown([In] acObject obj0, [In] int obj1, [In] int obj2, [In] ref int obj3)
    {
      if (this.m_MouseDownDelegate == null)
        return;
      this.m_MouseDownDelegate(obj0, obj1, obj2, ref obj3);
    }

    public virtual void MouseMove([In] acObject obj0, [In] int obj1, [In] int obj2)
    {
      if (this.m_MouseMoveDelegate == null)
        return;
      this.m_MouseMoveDelegate(obj0, obj1, obj2);
    }

    public virtual void MouseUp([In] acObject obj0, [In] int obj1, [In] int obj2)
    {
      if (this.m_MouseUpDelegate == null)
        return;
      this.m_MouseUpDelegate(obj0, obj1, obj2);
    }

    public virtual void NewObject([In] acObject obj0, [In] ref int obj1)
    {
      if (this.m_NewObjectDelegate == null)
        return;
      this.m_NewObjectDelegate(obj0, ref obj1);
    }

    public virtual void ActivateObject([In] object obj0, [In] ref int obj1)
    {
      if (this.m_ActivateObjectDelegate == null)
        return;
      this.m_ActivateObjectDelegate(obj0, ref obj1);
    }

    public virtual void LoadPage([In] int obj0, [In] ref int obj1)
    {
      if (this.m_LoadPageDelegate == null)
        return;
      this.m_LoadPageDelegate(obj0, ref obj1);
    }

    public virtual void EvaluateExpression([In] object obj0, [In] ref string obj1, [In] ref int obj2)
    {
      if (this.m_EvaluateExpressionDelegate == null)
        return;
      this.m_EvaluateExpressionDelegate(obj0, ref obj1, ref obj2);
    }

    public virtual void ProcessingProgress([In] int obj0, [In] int obj1, [In] ref int obj2)
    {
      if (this.m_ProcessingProgressDelegate == null)
        return;
      this.m_ProcessingProgressDelegate(obj0, obj1, ref obj2);
    }

    internal _IPDFCreactiveXEvents_SinkHelper()
    {
      this.m_dwCookie = 0;
      this.m_BeforeDeleteDelegate = (_IPDFCreactiveXEvents_BeforeDeleteEventHandler) null;
      this.m_PrintPageDelegate = (_IPDFCreactiveXEvents_PrintPageEventHandler) null;
      this.m_SavePageDelegate = (_IPDFCreactiveXEvents_SavePageEventHandler) null;
      this.m_ClickHyperlinkDelegate = (_IPDFCreactiveXEvents_ClickHyperlinkEventHandler) null;
      this.m_RefreshDelegate = (_IPDFCreactiveXEvents_RefreshEventHandler) null;
      this.m_SelectedObjectChangeDelegate = (_IPDFCreactiveXEvents_SelectedObjectChangeEventHandler) null;
      this.m_ObjectTextChangeDelegate = (_IPDFCreactiveXEvents_ObjectTextChangeEventHandler) null;
      this.m_ContextSensitiveMenuDelegate = (_IPDFCreactiveXEvents_ContextSensitiveMenuEventHandler) null;
      this.m_MouseDownDelegate = (_IPDFCreactiveXEvents_MouseDownEventHandler) null;
      this.m_MouseMoveDelegate = (_IPDFCreactiveXEvents_MouseMoveEventHandler) null;
      this.m_MouseUpDelegate = (_IPDFCreactiveXEvents_MouseUpEventHandler) null;
      this.m_NewObjectDelegate = (_IPDFCreactiveXEvents_NewObjectEventHandler) null;
      this.m_ActivateObjectDelegate = (_IPDFCreactiveXEvents_ActivateObjectEventHandler) null;
      this.m_LoadPageDelegate = (_IPDFCreactiveXEvents_LoadPageEventHandler) null;
      this.m_EvaluateExpressionDelegate = (_IPDFCreactiveXEvents_EvaluateExpressionEventHandler) null;
      this.m_ProcessingProgressDelegate = (_IPDFCreactiveXEvents_ProcessingProgressEventHandler) null;
    }
  }
}
