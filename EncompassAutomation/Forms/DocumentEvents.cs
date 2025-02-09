// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.DocumentEvents
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using EllieMae.Encompass.Forms.Design;
using mshtml;
using System;
using System.Collections;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  /// <summary>Summary description for DocumentEvents.</summary>
  internal class DocumentEvents : mshtml.HTMLDocumentEvents2, HTMLTextContainerEvents2, IDisposable
  {
    private Form parentForm;
    private mshtml.IHTMLElement mouseEventElement;
    private ArrayList mouseEventSelectedControls = new ArrayList();
    private Slider currentSlider;
    private DragSelector dragSelector;
    private ContainerControl moveSource;
    private ContainerControl moveTarget;
    private Hashtable moveContainers;
    private ContainerControl highlightedControl;
    private bool forceComRelease;
    private System.Runtime.InteropServices.ComTypes.IConnectionPoint docEventsConnPt;
    private System.Runtime.InteropServices.ComTypes.IConnectionPoint textEventsConnPt;
    private int docEventsCookie;
    private int textEventsCookie;

    public DocumentEvents(Form parentForm)
    {
      this.parentForm = parentForm;
      Guid guid1 = typeof (mshtml.HTMLDocumentEvents2).GUID;
      ((System.Runtime.InteropServices.ComTypes.IConnectionPointContainer) parentForm.GetHTMLDocument()).FindConnectionPoint(ref guid1, out this.docEventsConnPt);
      this.docEventsConnPt.Advise((object) this, out this.docEventsCookie);
      Guid guid2 = typeof (HTMLTextContainerEvents2).GUID;
      ((System.Runtime.InteropServices.ComTypes.IConnectionPointContainer) parentForm.GetHTMLDocument().body).FindConnectionPoint(ref guid2, out this.textEventsConnPt);
      this.textEventsConnPt.Advise((object) this, out this.textEventsCookie);
    }

    public void ondataavailable(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public bool onbeforedeactivate(mshtml.IHTMLEventObj pEvtObj) => true;

    public bool onstop(mshtml.IHTMLEventObj pEvtObj) => true;

    public void onrowsinserted(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public bool onselectstart(mshtml.IHTMLEventObj pEvtObj) => !this.parentForm.EditingEnabled;

    public bool onkeypress(mshtml.IHTMLEventObj pEvtObj) => !this.parentForm.EditingEnabled;

    public bool onhelp(mshtml.IHTMLEventObj pEvtObj) => false;

    public void onpropertychange(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public void oncellchange(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public bool oncontextmenu(mshtml.IHTMLEventObj pEvtObj)
    {
      if (this.parentForm.EditingEnabled)
      {
        Control controlForElement = this.parentForm.FindControlForElement(pEvtObj.srcElement);
        this.restoreSelections();
        if (controlForElement != null && !this.isControlSelected(controlForElement))
        {
          this.parentForm.SelectedControls.Clear();
          if (controlForElement != this.parentForm)
            this.parentForm.SelectedControls.Add(controlForElement);
        }
        pEvtObj.cancelBubble = true;
        this.parentForm.OnContextMenu(new FormEventArgs(this.parentForm, pEvtObj));
      }
      return false;
    }

    public bool ondblclick(mshtml.IHTMLEventObj pEvtObj)
    {
      if (this.parentForm.EditingEnabled)
      {
        if (this.parentForm.FindControlForElement(pEvtObj.srcElement) is ISupportsEvents controlForElement)
          EMEventEditor.ShowEventEditor((Control) controlForElement);
        pEvtObj.cancelBubble = true;
        pEvtObj.returnValue = (object) false;
      }
      return false;
    }

    public void onfocusin(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public void ondatasetcomplete(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public void onkeyup(mshtml.IHTMLEventObj pEvtObj)
    {
      if (this.highlightedControl == null)
        return;
      this.highlightedControl.RemoveHighlight();
      this.highlightedControl = (ContainerControl) null;
    }

    public void onfocusout(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public void onbeforeeditfocus(mshtml.IHTMLEventObj pEvtObj)
    {
      if (!this.parentForm.EditingEnabled)
        return;
      pEvtObj.cancelBubble = true;
      pEvtObj.returnValue = (object) false;
    }

    public bool ondragstart(mshtml.IHTMLEventObj pEvtObj) => false;

    public bool oncontrolselect(mshtml.IHTMLEventObj pEvtObj) => true;

    public void onactivate(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public bool onbeforeactivate(mshtml.IHTMLEventObj pEvtObj)
    {
      if (this.parentForm.EditingEnabled)
        pEvtObj.cancelBubble = true;
      return true;
    }

    public void onkeydown(mshtml.IHTMLEventObj pEvtObj)
    {
      if (!this.parentForm.EditingEnabled)
        return;
      if (pEvtObj.keyCode == 46)
      {
        this.mouseEventElement = (mshtml.IHTMLElement) null;
        this.parentForm.SelectedControls.Delete();
        this.parentForm.OnSelectionChanged(new FormEventArgs(this.parentForm, pEvtObj));
        pEvtObj.returnValue = (object) false;
      }
      else if (pEvtObj.keyCode == 65 && pEvtObj.ctrlKey)
        this.parentForm.Controls.SelectAll();
      else if (pEvtObj.keyCode == 86 && pEvtObj.ctrlKey)
      {
        this.pasteControls(pEvtObj);
        pEvtObj.returnValue = (object) false;
      }
      else if ((pEvtObj.keyCode == 189 || pEvtObj.keyCode == 109) && pEvtObj.shiftKey)
        this.highlightContainer();
      else if (pEvtObj.keyCode == 189 || pEvtObj.keyCode == 109)
        this.selectContainer();
      else if (pEvtObj.keyCode == 187 || pEvtObj.keyCode == 107)
        this.selectChildren();
      else
        pEvtObj.returnValue = !pEvtObj.ctrlKey ? (!pEvtObj.shiftKey ? (object) !this.moveSelectedControls(pEvtObj.keyCode, 5) : (object) !this.resizeSelectedControls(pEvtObj.keyCode)) : (object) !this.moveSelectedControls(pEvtObj.keyCode, 1);
      this.parentForm.OnKeyPress(new KeyEventArgs(this.getWindowsKeyCode(pEvtObj)));
      pEvtObj.cancelBubble = true;
    }

    private Keys getWindowsKeyCode(mshtml.IHTMLEventObj pEvtObj)
    {
      Keys keyCode = (Keys) pEvtObj.keyCode;
      if (pEvtObj.ctrlKey)
        keyCode |= Keys.Control;
      if (pEvtObj.shiftKey)
        keyCode |= Keys.Shift;
      if (pEvtObj.altKey)
        keyCode |= Keys.Alt;
      return keyCode;
    }

    public bool onrowexit(mshtml.IHTMLEventObj pEvtObj) => true;

    public bool onbeforeupdate(mshtml.IHTMLEventObj pEvtObj) => true;

    public void onrowsdelete(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public void onreadystatechange(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public void onmousemove(mshtml.IHTMLEventObj pEvtObj)
    {
      if (pEvtObj.button != 1 || !pEvtObj.shiftKey || this.dragSelector == null)
        return;
      this.dragSelector.DragTo(this.parentForm.VisibleToAbsolutePosition(new Point(pEvtObj.clientX, pEvtObj.clientY)));
      pEvtObj.cancelBubble = true;
    }

    public void onrowenter(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public void onafterupdate(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public void ondeactivate(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public void onselectionchange(mshtml.IHTMLEventObj pEvtObj)
    {
      if (!this.parentForm.EditingEnabled)
        return;
      this.parentForm.OnSelectionChanged(new FormEventArgs(this.parentForm, pEvtObj));
      pEvtObj.cancelBubble = true;
      if (this.mouseEventElement == null || Control.IsDetached(this.mouseEventElement))
        return;
      this.parentForm.SelectedControls.SetReferenceControl(this.parentForm.FindControlForElement(this.mouseEventElement));
    }

    public void ondatasetchanged(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public void onmouseover(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public bool onmousewheel(mshtml.IHTMLEventObj pEvtObj) => true;

    public bool onerrorupdate(mshtml.IHTMLEventObj pEvtObj) => true;

    public void onmouseout(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public void onmousedown(mshtml.IHTMLEventObj pEvtObj)
    {
      pEvtObj.cancelBubble = true;
      Control controlForElement = (Control) (this.parentForm.FindControlForElement(pEvtObj.srcElement) as DropdownBox);
      if (pEvtObj.button == 1 && pEvtObj.shiftKey && !(controlForElement is DropdownBox))
      {
        this.startDragSelection(new Point(pEvtObj.clientX, pEvtObj.clientY));
        pEvtObj.returnValue = (object) false;
      }
      else
      {
        this.mouseEventElement = pEvtObj.srcElement;
        this.mouseEventSelectedControls.Clear();
        foreach (Control selectedControl in this.parentForm.SelectedControls)
          this.mouseEventSelectedControls.Add((object) selectedControl);
      }
    }

    public void onmouseup(mshtml.IHTMLEventObj pEvtObj)
    {
      this.stopDragSelection();
      pEvtObj.cancelBubble = true;
    }

    public bool onclick(mshtml.IHTMLEventObj pEvtObj)
    {
      if (this.parentForm.EditingEnabled && pEvtObj.srcElement == this.mouseEventElement)
      {
        Control controlForElement = this.parentForm.FindControlForElement(pEvtObj.srcElement);
        if (controlForElement == null)
          this.parentForm.SelectedControls.Clear();
        else if (pEvtObj.ctrlKey)
        {
          this.restoreSelections();
          controlForElement.Select();
        }
        else
        {
          this.parentForm.SelectedControls.Clear();
          controlForElement.Select();
        }
      }
      else if (!this.parentForm.EditingEnabled && this.parentForm.FindControlForElement(pEvtObj.srcElement) is ISupportsClickEvent controlForElement1)
        controlForElement1.InvokeClick();
      pEvtObj.cancelBubble = true;
      this.mouseEventElement = (mshtml.IHTMLElement) null;
      return true;
    }

    public void onchange(mshtml.IHTMLEventObj pEvtObj)
    {
      if (!this.parentForm.EditingEnabled && this.parentForm.FindControlForElement(pEvtObj.srcElement) is ISupportsChangeEvent controlForElement)
        controlForElement.InvokeChange();
      pEvtObj.cancelBubble = true;
    }

    public bool onbeforepaste(mshtml.IHTMLEventObj pEvtObj) => false;

    public bool oncut(mshtml.IHTMLEventObj pEvtObj)
    {
      if (!this.parentForm.EditingEnabled)
        return true;
      if (this.parentForm.SelectedControls.AllowCutCopyDelete())
        CopyBuffer.Current = this.parentForm.SelectedControls.Cut();
      this.mouseEventElement = (mshtml.IHTMLElement) null;
      pEvtObj.returnValue = (object) false;
      pEvtObj.cancelBubble = true;
      return false;
    }

    public void onselect(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public void onlayoutcomplete(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public void ondragend(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public bool ondrag(mshtml.IHTMLEventObj pEvtObj) => false;

    public void onfilterchange(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public bool onresizestart(mshtml.IHTMLEventObj pEvtObj) => true;

    public bool onmovestart(mshtml.IHTMLEventObj pEvtObj)
    {
      if (this.parentForm.SelectedControls.Contains((Control) this.parentForm))
        return false;
      this.currentSlider = (Slider) null;
      this.moveSource = (ContainerControl) null;
      this.moveTarget = (ContainerControl) null;
      this.moveContainers = new Hashtable();
      if (this.parentForm.SelectedControls.Count == 1)
      {
        this.currentSlider = this.parentForm.SelectedControls[0] as Slider;
        if (this.currentSlider != null)
          this.currentSlider.PrepareForSlide();
      }
      if (this.currentSlider == null)
      {
        foreach (Control selectedControl in this.parentForm.SelectedControls)
        {
          if (selectedControl is ContainerControl)
            this.moveContainers[(object) selectedControl.ControlID] = (object) true;
          ContainerControl container = selectedControl.GetContainer();
          if (this.moveSource == null)
            this.moveSource = container;
          else if (this.moveSource != container)
          {
            this.moveSource = (ContainerControl) null;
            break;
          }
        }
      }
      pEvtObj.cancelBubble = true;
      return true;
    }

    public bool ondrop(mshtml.IHTMLEventObj pEvtObj) => false;

    public void onlosecapture(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public void onresizeend(mshtml.IHTMLEventObj pEvtObj)
    {
      Control controlForElement = this.parentForm.FindControlForElement(pEvtObj.srcElement);
      if (controlForElement == null)
        return;
      Size size = controlForElement.AdjustSize(controlForElement.Size);
      if (size != controlForElement.Size)
        controlForElement.Size = size;
      this.parentForm.OnResize(new FormEventArgs(this.parentForm, pEvtObj));
    }

    public void onblur(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public void onmove(mshtml.IHTMLEventObj pEvtObj)
    {
      if (pEvtObj.srcElement != null && this.currentSlider != null)
      {
        if (!(this.currentSlider.ControlID == pEvtObj.srcElement.id) || !this.currentSlider.Active)
          return;
        this.currentSlider.SlideControls();
      }
      else
      {
        if (pEvtObj.ctrlKey || pEvtObj.srcElement == null || this.moveSource == null)
          return;
        ContainerControl containerAtPointEx = this.parentForm.GetContainerAtPointEx(this.parentForm.VisibleToAbsolutePosition(new Point(pEvtObj.clientX, pEvtObj.clientY)), (IDictionary) this.moveContainers);
        if (containerAtPointEx == this.moveTarget)
          return;
        if (this.moveTarget != null)
          this.moveTarget.RemoveHighlight();
        this.moveTarget = (ContainerControl) null;
        if (containerAtPointEx == this.moveSource)
          return;
        this.moveTarget = containerAtPointEx;
        this.moveTarget.Highlight();
      }
    }

    public bool ondragenter(mshtml.IHTMLEventObj pEvtObj) => false;

    public bool onpaste(mshtml.IHTMLEventObj pEvtObj)
    {
      if (!this.parentForm.EditingEnabled)
        return true;
      pEvtObj.returnValue = (object) false;
      return false;
    }

    public bool oncopy(mshtml.IHTMLEventObj pEvtObj)
    {
      if (!this.parentForm.EditingEnabled)
        return true;
      CopyBuffer.Current = this.parentForm.SelectedControls.Copy();
      pEvtObj.returnValue = (object) true;
      pEvtObj.cancelBubble = true;
      return true;
    }

    public void onscroll(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public bool onbeforecut(mshtml.IHTMLEventObj pEvtObj) => false;

    public void onresize(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public void onpage(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public void onmouseleave(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public void onmouseenter(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public void onfocus(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public bool onbeforecopy(mshtml.IHTMLEventObj pEvtObj) => false;

    public bool ondragover(mshtml.IHTMLEventObj pEvtObj) => false;

    public void ondragleave(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public void onmoveend(mshtml.IHTMLEventObj pEvtObj)
    {
      this.currentSlider = (Slider) null;
      this.moveSource = (ContainerControl) null;
      if (this.moveTarget != null)
      {
        this.moveTarget.RemoveHighlight();
        this.parentForm.SelectedControls.MoveInto(this.moveTarget);
        this.moveTarget = (ContainerControl) null;
      }
      this.parentForm.OnMove(new FormEventArgs(this.parentForm, pEvtObj));
    }

    public void Dispose()
    {
      if (this.docEventsConnPt != null)
      {
        this.docEventsConnPt.Unadvise(this.docEventsCookie);
        if (this.forceComRelease)
          Marshal.ReleaseComObject((object) this.docEventsConnPt);
        this.docEventsConnPt = (System.Runtime.InteropServices.ComTypes.IConnectionPoint) null;
      }
      if (this.textEventsConnPt == null)
        return;
      this.textEventsConnPt.Unadvise(this.textEventsCookie);
      if (this.forceComRelease)
        Marshal.ReleaseComObject((object) this.textEventsConnPt);
      this.textEventsConnPt = (System.Runtime.InteropServices.ComTypes.IConnectionPoint) null;
    }

    private void restoreSelections()
    {
      if (this.parentForm.SelectedControls.Count == this.mouseEventSelectedControls.Count)
        return;
      this.parentForm.SelectedControls.ReplaceRange((System.Collections.IEnumerable) this.mouseEventSelectedControls);
    }

    private bool isControlSelected(Control ctrl)
    {
      foreach (Control selectedControl in this.parentForm.SelectedControls)
      {
        if (selectedControl == ctrl)
          return true;
      }
      return false;
    }

    private bool moveSelectedControls(int keyCode, int scale)
    {
      Point adjustmentForKeyCode = this.getAdjustmentForKeyCode(keyCode);
      if (adjustmentForKeyCode == Point.Empty)
        return false;
      this.parentForm.SelectedControls.Move(adjustmentForKeyCode.X * scale, adjustmentForKeyCode.Y * scale);
      return true;
    }

    private bool resizeSelectedControls(int keyCode)
    {
      Point adjustmentForKeyCode = this.getAdjustmentForKeyCode(keyCode);
      if (adjustmentForKeyCode == Point.Empty)
        return false;
      this.parentForm.SelectedControls.Resize(adjustmentForKeyCode.X, adjustmentForKeyCode.Y);
      return true;
    }

    private Point getAdjustmentForKeyCode(int keyCode)
    {
      switch (keyCode)
      {
        case 37:
          return new Point(-1, 0);
        case 38:
          return new Point(0, -1);
        case 39:
          return new Point(1, 0);
        case 40:
          return new Point(0, 1);
        default:
          return Point.Empty;
      }
    }

    private void startDragSelection(Point origin)
    {
      this.getCurrentDragSelector().BeginDrag(this.parentForm.VisibleToAbsolutePosition(origin));
    }

    private void stopDragSelection()
    {
      if (this.dragSelector == null)
        return;
      this.dragSelector.Delete();
      this.dragSelector = (DragSelector) null;
    }

    private DragSelector getCurrentDragSelector()
    {
      if (this.dragSelector != null)
        return this.dragSelector;
      DragSelector currentDragSelector = (DragSelector) null;
      Control[] controlsByType = this.parentForm.FindControlsByType(typeof (DragSelector));
      if (controlsByType.Length != 0)
        currentDragSelector = (DragSelector) controlsByType[0];
      if (currentDragSelector == null)
      {
        currentDragSelector = new DragSelector();
        this.parentForm.Controls.Insert((Control) currentDragSelector);
      }
      this.dragSelector = currentDragSelector;
      return currentDragSelector;
    }

    private void highlightContainer()
    {
      if (this.highlightedControl != null)
        return;
      ContainerControl containerControl = (ContainerControl) null;
      foreach (Control selectedControl in this.parentForm.SelectedControls)
      {
        if (containerControl == null)
          containerControl = selectedControl.GetContainer();
        else if (!containerControl.Equals((object) selectedControl.GetContainer()))
          return;
      }
      if (containerControl == null)
        return;
      this.highlightedControl = containerControl;
      this.highlightedControl.Highlight();
    }

    private void selectContainer()
    {
      ContainerControl containerControl = (ContainerControl) null;
      foreach (Control selectedControl in this.parentForm.SelectedControls)
      {
        if (containerControl == null)
          containerControl = selectedControl.GetContainer();
        else if (!containerControl.Equals((object) selectedControl.GetContainer()))
          return;
      }
      if (containerControl == null)
        return;
      this.parentForm.SelectedControls.Clear();
      containerControl.Select();
    }

    private void selectChildren()
    {
      ArrayList controls = new ArrayList();
      foreach (Control selectedControl in this.parentForm.SelectedControls)
      {
        if (selectedControl is ContainerControl)
          controls.AddRange((ICollection) ((ContainerControl) selectedControl).Controls);
      }
      if (controls.Count <= 0)
        return;
      this.parentForm.SelectedControls.Clear();
      this.parentForm.SelectedControls.AddRange((System.Collections.IEnumerable) controls);
    }

    private void pasteControls(mshtml.IHTMLEventObj pEvtObj)
    {
      try
      {
        if (CopyBuffer.Current == null)
          return;
        Control container = this.parentForm.FindControlForElement(pEvtObj.srcElement);
        if (!(container is ContainerControl))
          container = (Control) container.GetContainer();
        if (this.parentForm.SelectedControls.Count == 1)
        {
          Control selectedControl = this.parentForm.SelectedControls[0];
          if (selectedControl is ContainerControl)
            container = selectedControl;
        }
        CopyBuffer.Current.Paste(container as ContainerControl);
      }
      catch (Exception ex)
      {
        Console.WriteLine("Paste error: " + ex.ToString());
      }
    }
  }
}
