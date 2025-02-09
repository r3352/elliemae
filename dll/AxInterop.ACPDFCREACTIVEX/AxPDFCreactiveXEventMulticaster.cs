// Decompiled with JetBrains decompiler
// Type: AxACPDFCREACTIVEX.AxPDFCreactiveXEventMulticaster
// Assembly: AxInterop.ACPDFCREACTIVEX, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50348D5A-A8E2-4894-AD2C-0D88350B72D8
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\AxInterop.ACPDFCREACTIVEX.dll

using ACPDFCREACTIVEX;
using System;
using System.Runtime.InteropServices;

#nullable disable
namespace AxACPDFCREACTIVEX
{
  [ClassInterface(ClassInterfaceType.None)]
  public class AxPDFCreactiveXEventMulticaster : _IPDFCreactiveXEvents
  {
    private AxPDFCreactiveX parent;

    public AxPDFCreactiveXEventMulticaster(AxPDFCreactiveX parent) => this.parent = parent;

    public virtual void BeforeDelete(ref int @continue)
    {
      _IPDFCreactiveXEvents_BeforeDeleteEvent e = new _IPDFCreactiveXEvents_BeforeDeleteEvent(@continue);
      this.parent.RaiseOnBeforeDelete((object) this.parent, e);
      @continue = e.@continue;
    }

    public virtual void PrintPage(int pageNumber, ref int @continue)
    {
      _IPDFCreactiveXEvents_PrintPageEvent e = new _IPDFCreactiveXEvents_PrintPageEvent(pageNumber, @continue);
      this.parent.RaiseOnPrintPageEvent((object) this.parent, e);
      @continue = e.@continue;
    }

    public virtual void SavePage(int pageNumber, ref int @continue)
    {
      _IPDFCreactiveXEvents_SavePageEvent e = new _IPDFCreactiveXEvents_SavePageEvent(pageNumber, @continue);
      this.parent.RaiseOnSavePageEvent((object) this.parent, e);
      @continue = e.@continue;
    }

    public virtual void ClickHyperlink(string @object, string hyperlink, ref int @continue)
    {
      _IPDFCreactiveXEvents_ClickHyperlinkEvent e = new _IPDFCreactiveXEvents_ClickHyperlinkEvent(@object, hyperlink, @continue);
      this.parent.RaiseOnClickHyperlink((object) this.parent, e);
      @continue = e.@continue;
    }

    public virtual void Refresh()
    {
      this.parent.RaiseOnRefreshEvent((object) this.parent, new EventArgs());
    }

    public virtual void SelectedObjectChange()
    {
      this.parent.RaiseOnSelectedObjectChange((object) this.parent, new EventArgs());
    }

    public virtual void ObjectTextChange(object pObject)
    {
      this.parent.RaiseOnObjectTextChange((object) this.parent, new _IPDFCreactiveXEvents_ObjectTextChangeEvent(pObject));
    }

    public virtual void ContextSensitiveMenu(ref int @continue)
    {
      _IPDFCreactiveXEvents_ContextSensitiveMenuEvent e = new _IPDFCreactiveXEvents_ContextSensitiveMenuEvent(@continue);
      this.parent.RaiseOnContextSensitiveMenu((object) this.parent, e);
      @continue = e.@continue;
    }

    public virtual void MouseDown(acObject pObject, int xPos, int yPos, ref int @continue)
    {
      _IPDFCreactiveXEvents_MouseDownEvent e = new _IPDFCreactiveXEvents_MouseDownEvent(pObject, xPos, yPos, @continue);
      this.parent.RaiseOnMouseDownEvent((object) this.parent, e);
      @continue = e.@continue;
    }

    public virtual void MouseMove(acObject pObject, int xPos, int yPos)
    {
      this.parent.RaiseOnMouseMoveEvent((object) this.parent, new _IPDFCreactiveXEvents_MouseMoveEvent(pObject, xPos, yPos));
    }

    public virtual void MouseUp(acObject pObject, int xPos, int yPos)
    {
      this.parent.RaiseOnMouseUpEvent((object) this.parent, new _IPDFCreactiveXEvents_MouseUpEvent(pObject, xPos, yPos));
    }

    public virtual void NewObject(acObject pObject, ref int @continue)
    {
      _IPDFCreactiveXEvents_NewObjectEvent e = new _IPDFCreactiveXEvents_NewObjectEvent(pObject, @continue);
      this.parent.RaiseOnNewObject((object) this.parent, e);
      @continue = e.@continue;
    }

    public virtual void ActivateObject(object pObject, ref int @continue)
    {
      _IPDFCreactiveXEvents_ActivateObjectEvent e = new _IPDFCreactiveXEvents_ActivateObjectEvent(pObject, @continue);
      this.parent.RaiseOnActivateObjectEvent((object) this.parent, e);
      @continue = e.@continue;
    }

    public virtual void LoadPage(int pageNumber, ref int @continue)
    {
      _IPDFCreactiveXEvents_LoadPageEvent e = new _IPDFCreactiveXEvents_LoadPageEvent(pageNumber, @continue);
      this.parent.RaiseOnLoadPage((object) this.parent, e);
      @continue = e.@continue;
    }

    public virtual void EvaluateExpression(object pObject, ref string result, ref int @continue)
    {
      _IPDFCreactiveXEvents_EvaluateExpressionEvent e = new _IPDFCreactiveXEvents_EvaluateExpressionEvent(pObject, result, @continue);
      this.parent.RaiseOnEvaluateExpression((object) this.parent, e);
      result = e.result;
      @continue = e.@continue;
    }

    public virtual void ProcessingProgress(int totalSteps, int currentStep, ref int @continue)
    {
      _IPDFCreactiveXEvents_ProcessingProgressEvent e = new _IPDFCreactiveXEvents_ProcessingProgressEvent(totalSteps, currentStep, @continue);
      this.parent.RaiseOnProcessingProgress((object) this.parent, e);
      @continue = e.@continue;
    }
  }
}
