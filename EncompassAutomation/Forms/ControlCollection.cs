// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.ControlCollection
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using mshtml;
using System;
using System.Collections;
using System.Drawing;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  /// <summary>
  /// Represnets the collection of controls associated with a <see cref="T:EllieMae.Encompass.Forms.ContainerControl" />.
  /// </summary>
  public class ControlCollection : ICollection, IEnumerable
  {
    private ContainerControl parentControl;

    internal ControlCollection(ContainerControl parentControl)
    {
      this.parentControl = parentControl;
    }

    /// <summary>Adds a new control to the control container.</summary>
    /// <param name="control">The control to be added.</param>
    /// <remarks>The control can be a new control or an existing control, in which
    /// case the control will be removed from its current container and placed into
    /// this new container.</remarks>
    public void Insert(Control control)
    {
      if (control.Form != null)
        this.insertExistingControl(control);
      else
        this.insertNewControl(control);
      control.OnContainerChanged();
    }

    /// <summary>Inserts a control into the container.</summary>
    /// <param name="control">The control to be inserted.</param>
    /// <param name="position">The relative position of the control within the
    /// new container.</param>
    public void Insert(Control control, Point position)
    {
      this.Insert(control);
      control.Position = position;
    }

    /// <summary>
    /// Inserts a control into the container and places it at an absolute position.
    /// </summary>
    /// <param name="control">The control to be inserted.</param>
    /// <param name="absolutePosition">The position to place the control, in absolute
    /// form coordinates.</param>
    /// <remarks>If the coordinates provided are outside the client area of the container,
    /// the control will be placed as instructed but will still be contained in the
    /// container control).</remarks>
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

    /// <summary>Removes a control from the container and deletes it.</summary>
    /// <param name="control">The control to be removed.</param>
    /// <remarks>The removed control is no longer valid and should
    /// be considered disposed.</remarks>
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

    /// <summary>
    /// Determines if a control is contained in the control hierarchy beneath this
    /// container control.
    /// </summary>
    /// <param name="controlId">The ID of the control to test.</param>
    /// <returns>Returns <c>true</c> if the control is within the container (or any
    /// nested subcontainer), <c>false</c> otherwise.</returns>
    public bool Contains(string controlId) => this.Find(controlId) != null;

    /// <summary>Finds a nested control within the container.</summary>
    /// <param name="controlId">The ID of the control to find.</param>
    /// <returns>Returns the control if it is found in the current container or
    /// any subcontainer, <c>null</c> otherwise.</returns>
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

    /// <summary>
    /// Finds the set of controls of a specified type that are nested within the
    /// container.
    /// </summary>
    /// <param name="type">The type of controls to find.</param>
    /// <returns>Returns an array containing the controls which derive from the
    /// specified type and are contained within the container (and any nested subcontainers).
    /// </returns>
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

    /// <summary>Gets a control from the collection by index.</summary>
    public Control this[int index] => (Control) this.getControlElements()[index];

    /// <summary>Gets the number of controls in the collection.</summary>
    public int Count => this.getControlElements().Count;

    /// <summary>Selects all controls within the collection.</summary>
    /// <remarks>This method cannot be used at runtime and is meant only for use within the
    /// Encompass Form Builder.</remarks>
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
      for (int name = 0; name < children.length; ++name)
      {
        IHTMLElement element = (IHTMLElement) children.item((object) name);
        if (Control.IsControl(element))
          controlElements.Add((object) element);
      }
      return controlElements;
    }

    /// <summary>Provides an enumerator over the set of controls.</summary>
    /// <returns>An IEnumerator whcih can be used to iterate over the control collection.</returns>
    public IEnumerator GetEnumerator()
    {
      return (IEnumerator) new ControlCollection.ControlEnumerator(this.parentControl.Form, this.getControlElements().GetEnumerator());
    }

    /// <summary>
    /// Indicates if this collection is thread-safe. This propery will always return <c>false</c>.
    /// </summary>
    public bool IsSynchronized => false;

    /// <summary>Provides a synchronization object for the collection</summary>
    public object SyncRoot => (object) this;

    /// <summary>
    /// Copies the elements of this collection to the specified array starting at the given index;
    /// </summary>
    /// <param name="array">The array to add the controls to.</param>
    /// <param name="index">The starting index to which the controls will be copied.</param>
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
