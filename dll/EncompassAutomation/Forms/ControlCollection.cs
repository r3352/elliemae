// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.ControlCollection
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

using mshtml;
using System;
using System.Collections;
using System.Drawing;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  public class ControlCollection : ICollection, IEnumerable
  {
    private ContainerControl parentControl;

    internal ControlCollection(ContainerControl parentControl)
    {
      this.parentControl = parentControl;
    }

    public void Insert(Control control)
    {
      if (control.Form != null)
        this.insertExistingControl(control);
      else
        this.insertNewControl(control);
      control.OnContainerChanged();
    }

    public void Insert(Control control, Point position)
    {
      this.Insert(control);
      control.Position = position;
    }

    public void InsertAbsolute(Control control, Point absolutePosition)
    {
      this.Insert(control);
      control.AbsolutePosition = absolutePosition;
      Point position = control.Position;
      if (position.X < 0 && position.Y < 0)
        control.Position = new Point(0, 0);
      else if (position.X < 0)
      {
        control.Position = new Point(0, position.Y);
      }
      else
      {
        if (position.Y >= 0)
          return;
        control.Position = new Point(position.X, 0);
      }
    }

    internal void Remove(Control control)
    {
      if (control.GetContainer() != this.parentControl)
        throw new ArgumentException("The specified control does not belong to this container");
      if (control is ContainerControl)
      {
        foreach (Control control1 in ((ContainerControl) control).Controls)
          control1.Delete();
      }
      ((IHTMLDOMNode) this.parentControl.ContainerElement).removeChild(control.HTMLElement as IHTMLDOMNode);
      this.parentControl.Form.ControlCache.Remove(control.ControlID);
      this.parentControl.Form.OnControlDeleted(control);
      this.parentControl.NotifyPropertyChange();
    }

    public bool Contains(string controlId) => this.Find(controlId) != null;

    public Control Find(string controlId)
    {
      if (controlId == this.parentControl.ControlID)
        return (Control) this.parentControl;
      foreach (Control control1 in this)
      {
        if (control1.ControlID == controlId)
          return control1;
        if (control1 is ContainerControl containerControl)
        {
          Control control2 = containerControl.Controls.Find(controlId);
          if (control2 != null)
            return control2;
        }
      }
      return (Control) null;
    }

    public Control[] FindByType(Type type)
    {
      ArrayList arrayList = new ArrayList();
      foreach (Control control in this)
      {
        if (type.IsAssignableFrom(control.GetType()))
          arrayList.Add((object) control);
        if (control is ContainerControl containerControl)
          arrayList.AddRange((ICollection) containerControl.Controls.FindByType(type));
      }
      return (Control[]) arrayList.ToArray(typeof (Control));
    }

    public Control this[int index] => (Control) this.getControlElements()[index];

    public int Count => this.getControlElements().Count;

    public void SelectAll()
    {
      this.parentControl.EnsureEditing();
      ControlSelection selectedControls = this.parentControl.Form.SelectedControls;
      selectedControls.Clear();
      selectedControls.AddRange((IEnumerable) this, true);
    }

    private ArrayList getControlElements()
    {
      ArrayList controlElements = new ArrayList();
      IHTMLElementCollection children = (IHTMLElementCollection) this.parentControl.ContainerElement.children;
      for (int index = 0; index < children.length; ++index)
      {
        IHTMLElement element = (IHTMLElement) children.item((object) index, (object) null);
        if (Control.IsControl(element))
          controlElements.Add((object) element);
      }
      return controlElements;
    }

    public IEnumerator GetEnumerator()
    {
      return (IEnumerator) new ControlCollection.ControlEnumerator(this.parentControl.Form, this.getControlElements().GetEnumerator());
    }

    public bool IsSynchronized => false;

    public object SyncRoot => (object) this;

    public void CopyTo(Array array, int index)
    {
      foreach (Control control in this)
        array.SetValue((object) control, index++);
    }

    private void insertExistingControl(Control control)
    {
      Point absolutePosition = control.AbsolutePosition;
      ((IHTMLDOMNode) this.parentControl.ContainerElement).appendChild((IHTMLDOMNode) control.HTMLElement);
      control.AbsolutePosition = absolutePosition;
      if (!(control is RuntimeControl))
        return;
      ((RuntimeControl) control).ApplyInteractiveState();
    }

    private void insertNewControl(Control control)
    {
      if (control.ControlID == null)
        control.ControlID = this.parentControl.Form.NewControlID(control.GetType().Name);
      else if (this.parentControl.Form.ControlExists(control.ControlID))
        throw new ArgumentException("A control with the specified ID already exists in the form");
      IHTMLElement containerElement = this.parentControl.ContainerElement;
      containerElement.innerHTML = containerElement.innerHTML + control.RenderHTML() + Environment.NewLine;
      control.AttachToElement(this.parentControl.Form, Control.FindChildByControlID(this.parentControl.ContainerElement, control.ControlID, false));
      control.PrepareForDisplay();
      this.parentControl.Form.OnControlAdded(control);
      this.parentControl.NotifyPropertyChange();
    }

    private class ControlEnumerator : IEnumerator
    {
      private Form form;
      private IEnumerator elementEnumerator;
      private Control current;

      public ControlEnumerator(Form form, IEnumerator elementEnumerator)
      {
        this.form = form;
        this.elementEnumerator = elementEnumerator;
      }

      public void Reset() => this.elementEnumerator.Reset();

      public object Current => (object) this.current;

      public bool MoveNext()
      {
        for (this.current = (Control) null; this.current == null; this.current = this.form.ElementToControl((IHTMLElement) this.elementEnumerator.Current))
        {
          if (!this.elementEnumerator.MoveNext())
            return false;
        }
        return true;
      }
    }
  }
}
