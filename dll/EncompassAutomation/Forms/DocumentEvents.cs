// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.DocumentEvents
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

using EllieMae.Encompass.Forms.Design;
using mshtml;
using System;
using System.Collections;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  internal class DocumentEvents : HTMLDocumentEvents2, HTMLTextContainerEvents2, IDisposable
  {
    private Form parentForm;
    private IHTMLElement mouseEventElement;
    private ArrayList mouseEventSelectedControls = new ArrayList();
    private Slider currentSlider;
    private DragSelector dragSelector;
    private ContainerControl moveSource;
    private ContainerControl moveTarget;
    private Hashtable moveContainers;
    private ContainerControl highlightedControl;
    private bool forceComRelease;
    private IConnectionPoint docEventsConnPt;
    private IConnectionPoint textEventsConnPt;
    private int docEventsCookie;
    private int textEventsCookie;

    public DocumentEvents(Form parentForm)
    {
      this.parentForm = parentForm;
      Guid guid1 = typeof (HTMLDocumentEvents2).GUID;
      ((IConnectionPointContainer) parentForm.GetHTMLDocument()).FindConnectionPoint(ref guid1, out this.docEventsConnPt);
      this.docEventsConnPt.Advise((object) this, out this.docEventsCookie);
      Guid guid2 = typeof (HTMLTextContainerEvents2).GUID;
      ((IConnectionPointContainer) ((DispHTMLDocument) parentForm.GetHTMLDocument()).body).FindConnectionPoint(ref guid2, out this.textEventsConnPt);
      this.textEventsConnPt.Advise((object) this, out this.textEventsCookie);
    }

    public void ondataavailable(IHTMLEventObj pEvtObj)
    {
    }

    public bool onbeforedeactivate(IHTMLEventObj pEvtObj) => true;

    public bool onstop(IHTMLEventObj pEvtObj) => true;

    public void onrowsinserted(IHTMLEventObj pEvtObj)
    {
    }

    public bool onselectstart(IHTMLEventObj pEvtObj) => !this.parentForm.EditingEnabled;

    public bool onkeypress(IHTMLEventObj pEvtObj) => !this.parentForm.EditingEnabled;

    public bool onhelp(IHTMLEventObj pEvtObj) => false;

    public void onpropertychange(IHTMLEventObj pEvtObj)
    {
    }

    public void oncellchange(IHTMLEventObj pEvtObj)
    {
    }

    public bool oncontextmenu(IHTMLEventObj pEvtObj)
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

    public bool ondblclick(IHTMLEventObj pEvtObj)
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

    public void onfocusin(IHTMLEventObj pEvtObj)
    {
    }

    public void ondatasetcomplete(IHTMLEventObj pEvtObj)
    {
    }

    public void onkeyup(IHTMLEventObj pEvtObj)
    {
      if (this.highlightedControl == null)
        return;
      this.highlightedControl.RemoveHighlight();
      this.highlightedControl = (ContainerControl) null;
    }

    public void onfocusout(IHTMLEventObj pEvtObj)
    {
    }

    public void onbeforeeditfocus(IHTMLEventObj pEvtObj)
    {
      if (!this.parentForm.EditingEnabled)
        return;
      pEvtObj.cancelBubble = true;
      pEvtObj.returnValue = (object) false;
    }

    public bool ondragstart(IHTMLEventObj pEvtObj) => false;

    public bool oncontrolselect(IHTMLEventObj pEvtObj) => true;

    public void onactivate(IHTMLEventObj pEvtObj)
    {
    }

    public bool onbeforeactivate(IHTMLEventObj pEvtObj)
    {
      if (this.parentForm.EditingEnabled)
        pEvtObj.cancelBubble = true;
      return true;
    }

    public void onkeydown(IHTMLEventObj pEvtObj)
    {
      if (!this.parentForm.EditingEnabled)
        return;
      if (pEvtObj.keyCode == 46)
      {
        this.mouseEventElement = (IHTMLElement) null;
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

    private Keys getWindowsKeyCode(IHTMLEventObj pEvtObj)
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

    public bool onrowexit(IHTMLEventObj pEvtObj) => true;

    public bool onbeforeupdate(IHTMLEventObj pEvtObj) => true;

    public void onrowsdelete(IHTMLEventObj pEvtObj)
    {
    }

    public void onreadystatechange(IHTMLEventObj pEvtObj)
    {
    }

    public void onmousemove(IHTMLEventObj pEvtObj)
    {
      if (pEvtObj.button != 1 || !pEvtObj.shiftKey || this.dragSelector == null)
        return;
      this.dragSelector.DragTo(this.parentForm.VisibleToAbsolutePosition(new Point(pEvtObj.clientX, pEvtObj.clientY)));
      pEvtObj.cancelBubble = true;
    }

    public void onrowenter(IHTMLEventObj pEvtObj)
    {
    }

    public void onafterupdate(IHTMLEventObj pEvtObj)
    {
    }

    public void ondeactivate(IHTMLEventObj pEvtObj)
    {
    }

    public void onselectionchange(IHTMLEventObj pEvtObj)
    {
      if (!this.parentForm.EditingEnabled)
        return;
      this.parentForm.OnSelectionChanged(new FormEventArgs(this.parentForm, pEvtObj));
      pEvtObj.cancelBubble = true;
      if (this.mouseEventElement == null || Control.IsDetached(this.mouseEventElement))
        return;
      this.parentForm.SelectedControls.SetReferenceControl(this.parentForm.FindControlForElement(this.mouseEventElement));
    }

    public void ondatasetchanged(IHTMLEventObj pEvtObj)
    {
    }

    public void onmouseover(IHTMLEventObj pEvtObj)
    {
    }

    public bool onmousewheel(IHTMLEventObj pEvtObj) => true;

    public bool onerrorupdate(IHTMLEventObj pEvtObj) => true;

    public void onmouseout(IHTMLEventObj pEvtObj)
    {
    }

    public void onmousedown(IHTMLEventObj pEvtObj)
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

    public void onmouseup(IHTMLEventObj pEvtObj)
    {
      this.stopDragSelection();
      pEvtObj.cancelBubble = true;
    }

    public bool onclick(IHTMLEventObj pEvtObj)
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
      this.mouseEventElement = (IHTMLElement) null;
      return true;
    }

    public void onchange(IHTMLEventObj pEvtObj)
    {
      if (!this.parentForm.EditingEnabled && this.parentForm.FindControlForElement(pEvtObj.srcElement) is ISupportsChangeEvent controlForElement)
        controlForElement.InvokeChange();
      pEvtObj.cancelBubble = true;
    }

    public bool onbeforepaste(IHTMLEventObj pEvtObj) => false;

    public bool oncut(IHTMLEventObj pEvtObj)
    {
      if (!this.parentForm.EditingEnabled)
        return true;
      if (this.parentForm.SelectedControls.AllowCutCopyDelete())
        CopyBuffer.Current = this.parentForm.SelectedControls.Cut();
      this.mouseEventElement = (IHTMLElement) null;
      pEvtObj.returnValue = (object) false;
      pEvtObj.cancelBubble = true;
      return false;
    }

    public void onselect(IHTMLEventObj pEvtObj)
    {
    }

    public void onlayoutcomplete(IHTMLEventObj pEvtObj)
    {
    }

    public void ondragend(IHTMLEventObj pEvtObj)
    {
    }

    public bool ondrag(IHTMLEventObj pEvtObj) => false;

    public void onfilterchange(IHTMLEventObj pEvtObj)
    {
    }

    public bool onresizestart(IHTMLEventObj pEvtObj) => true;

    public bool onmovestart(IHTMLEventObj pEvtObj)
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

    public bool ondrop(IHTMLEventObj pEvtObj) => false;

    public void onlosecapture(IHTMLEventObj pEvtObj)
    {
    }

    public void onresizeend(IHTMLEventObj pEvtObj)
    {
      Control controlForElement = this.parentForm.FindControlForElement(pEvtObj.srcElement);
      if (controlForElement == null)
        return;
      Size size = controlForElement.AdjustSize(controlForElement.Size);
      if (size != controlForElement.Size)
        controlForElement.Size = size;
      this.parentForm.OnResize(new FormEventArgs(this.parentForm, pEvtObj));
    }

    public void onblur(IHTMLEventObj pEvtObj)
    {
    }

    public void onmove(IHTMLEventObj pEvtObj)
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

    public bool ondragenter(IHTMLEventObj pEvtObj) => false;

    public bool onpaste(IHTMLEventObj pEvtObj)
    {
      if (!this.parentForm.EditingEnabled)
        return true;
      pEvtObj.returnValue = (object) false;
      return false;
    }

    public bool oncopy(IHTMLEventObj pEvtObj)
    {
      if (!this.parentForm.EditingEnabled)
        return true;
      CopyBuffer.Current = this.parentForm.SelectedControls.Copy();
      pEvtObj.returnValue = (object) true;
      pEvtObj.cancelBubble = true;
      return true;
    }

    public void onscroll(IHTMLEventObj pEvtObj)
    {
    }

    public bool onbeforecut(IHTMLEventObj pEvtObj) => false;

    public void onresize(IHTMLEventObj pEvtObj)
    {
    }

    public void onpage(IHTMLEventObj pEvtObj)
    {
    }

    public void onmouseleave(IHTMLEventObj pEvtObj)
    {
    }

    public void onmouseenter(IHTMLEventObj pEvtObj)
    {
    }

    public void onfocus(IHTMLEventObj pEvtObj)
    {
    }

    public bool onbeforecopy(IHTMLEventObj pEvtObj) => false;

    public bool ondragover(IHTMLEventObj pEvtObj) => false;

    public void ondragleave(IHTMLEventObj pEvtObj)
    {
    }

    public void onmoveend(IHTMLEventObj pEvtObj)
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
        this.docEventsConnPt = (IConnectionPoint) null;
      }
      if (this.textEventsConnPt == null)
        return;
      this.textEventsConnPt.Unadvise(this.textEventsCookie);
      if (this.forceComRelease)
        Marshal.ReleaseComObject((object) this.textEventsConnPt);
      this.textEventsConnPt = (IConnectionPoint) null;
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

    private void pasteControls(IHTMLEventObj pEvtObj)
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
