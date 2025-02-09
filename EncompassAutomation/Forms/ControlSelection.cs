// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.ControlSelection
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
  /// Represents the set of currently selected controls in the Encompass Form Builder.
  /// </summary>
  /// <remarks>This class is not intended for use outside of the Encompass Form Builder.</remarks>
  public class ControlSelection : IEnumerable
  {
    private Form parentForm;
    private Control referenceControl;

    internal ControlSelection(Form parentForm) => this.parentForm = parentForm;

    /// <summary>Returns a selected control by index.</summary>
    public Control this[int index]
    {
      get
      {
        IHTMLControlRange currentControlRange = this.getCurrentControlRange();
        if (currentControlRange == null || currentControlRange.length < index + 1)
          throw new IndexOutOfRangeException();
        return this.parentForm.FindControlForElement(currentControlRange.item(index));
      }
    }

    /// <summary>
    /// Provides an enumerator for the set of selected controls.
    /// </summary>
    /// <returns></returns>
    public IEnumerator GetEnumerator() => this.getSelectedControls().GetEnumerator();

    /// <summary>Gets the number of selected controls.</summary>
    public int Count
    {
      get
      {
        IHTMLControlRange currentControlRange = this.getCurrentControlRange();
        return currentControlRange == null ? 0 : currentControlRange.length;
      }
    }

    /// <summary>Checks of a control is contained in the collection.</summary>
    /// <param name="control">The control to check for.</param>
    /// <returns>Returns <c>true</c> if the control is selected, <c>false</c> otherwise.</returns>
    public bool Contains(Control control)
    {
      foreach (object selectedControl in this.getSelectedControls())
      {
        if (selectedControl.Equals((object) control))
          return true;
      }
      return false;
    }

    /// <summary>
    /// Determines if the entire set of selected controls supports the Cut, Copy and Delete operations.
    /// </summary>
    /// <returns></returns>
    public bool AllowCutCopyDelete()
    {
      if (this.Count == 0)
        return false;
      foreach (Control control in this)
      {
        if (!control.AllowCutCopyDelete)
          return false;
      }
      return true;
    }

    /// <summary>
    /// Determines if the entire set of controls supports positioning on the form.
    /// </summary>
    /// <returns></returns>
    public bool AllowPositioning()
    {
      if (this.Count == 0)
        return false;
      foreach (Control control in this)
      {
        if (!control.AllowPositioning)
          return false;
      }
      return true;
    }

    /// <summary>
    /// Clears the set of selected controls, causing all controls to be unselected.
    /// </summary>
    public void Clear()
    {
      this.createControlRange().select();
      this.referenceControl = (Control) null;
    }

    /// <summary>Adds a new control to the selected control list.</summary>
    /// <param name="control">The control to be selected.</param>
    /// <remarks>All existing selected controls remain selected.</remarks>
    public void Add(Control control)
    {
      this.AddRange((IEnumerable) new Control[1]{ control });
      this.referenceControl = control;
    }

    /// <summary>
    /// Adds a set of controls to the collection of selected controls.
    /// </summary>
    /// <param name="controls">An enumerable list of controls to be selected.</param>
    /// <remarks>Invoking this method is more efficient than invoking the <see cref="M:EllieMae.Encompass.Forms.ControlSelection.Add(EllieMae.Encompass.Forms.Control)" />
    /// method for each control individually.</remarks>
    public void AddRange(IEnumerable controls)
    {
      IHTMLControlRange htmlControlRange = this.getCurrentControlRange() ?? this.createControlRange();
      IHTMLControlRange2 htmlControlRange2 = (IHTMLControlRange2) htmlControlRange;
      foreach (Control control in controls)
        htmlControlRange2.addElement(control.HTMLElement);
      htmlControlRange.select();
      this.referenceControl = (Control) null;
    }

    /// <summary>
    /// Adds a set of controls to the collection of selected controls.
    /// </summary>
    /// <param name="controls">An enumerable list of controls to be selected.</param>
    /// <param name="recurse">Indicates if the sub-controls of the current control should be selected.</param>
    /// <remarks>Invoking this method is more efficient than invoking the <see cref="M:EllieMae.Encompass.Forms.ControlSelection.Add(EllieMae.Encompass.Forms.Control)" />
    /// method for each control individually.</remarks>
    public void AddRange(IEnumerable controls, bool recurse)
    {
      IHTMLControlRange range = this.getCurrentControlRange() ?? this.createControlRange();
      this.addControlsToRange(controls, range, true);
      range.select();
      this.referenceControl = (Control) null;
    }

    /// <summary>Replaces a range of controls in the selection.</summary>
    /// <param name="controls">The new set of controls to be selected.</param>
    /// <remarks>This method deselects the current set of controls and sets a new set as selected.
    /// </remarks>
    internal void ReplaceRange(IEnumerable controls)
    {
      this.createControlRange().select();
      IHTMLControlRange htmlControlRange = this.getCurrentControlRange() ?? this.createControlRange();
      IHTMLControlRange2 htmlControlRange2 = (IHTMLControlRange2) htmlControlRange;
      foreach (Control control in controls)
        htmlControlRange2.addElement(control.HTMLElement);
      htmlControlRange.select();
    }

    /// <summary>
    /// Sets the reference control for operations that require one.
    /// </summary>
    /// <param name="c">The reference control to be used.</param>
    public void SetReferenceControl(Control c) => this.referenceControl = c;

    /// <summary>
    /// Converts the list of selected controls to a control array.
    /// </summary>
    /// <returns></returns>
    public Control[] ToControlArray()
    {
      ArrayList arrayList = new ArrayList();
      foreach (Control control in this)
        arrayList.Add((object) control);
      return (Control[]) arrayList.ToArray(typeof (Control));
    }

    private IHTMLControlRange createControlRange()
    {
      return (IHTMLControlRange) (this.parentForm.GetHTMLDocument().body as HTMLBody).createControlRange();
    }

    private IHTMLControlRange getCurrentControlRange()
    {
      try
      {
        HTMLDocument htmlDocument = this.parentForm.GetHTMLDocument();
        return htmlDocument.selection == null || htmlDocument.selection.type != "Control" ? (IHTMLControlRange) null : (IHTMLControlRange) htmlDocument.selection.createRange();
      }
      catch
      {
        return (IHTMLControlRange) null;
      }
    }

    private Control[] getSelectedControls() => this.getSelectedControls(true);

    private Control[] getSelectedControls(bool allowNestedControls)
    {
      IHTMLControlRange currentControlRange = this.getCurrentControlRange();
      if (currentControlRange == null)
        return new Control[0];
      ArrayList arrayList1 = new ArrayList();
      Hashtable hashtable = new Hashtable();
      for (int index = 0; index < currentControlRange.length; ++index)
      {
        Control controlForElement = this.parentForm.FindControlForElement(currentControlRange.item(index));
        if (controlForElement != null && !hashtable.Contains((object) controlForElement.ControlID))
        {
          arrayList1.Add((object) controlForElement);
          hashtable.Add((object) controlForElement.ControlID, (object) controlForElement);
        }
      }
      if (!allowNestedControls)
      {
        ArrayList arrayList2 = new ArrayList();
        foreach (Control control in arrayList1)
        {
          for (Control container = (Control) control.GetContainer(); container != null; container = (Control) container.GetContainer())
          {
            if (hashtable.Contains((object) container.ControlID))
            {
              arrayList2.Add((object) control);
              break;
            }
          }
        }
        foreach (Control control in arrayList2)
          arrayList1.Remove((object) control);
      }
      hashtable.Clear();
      return (Control[]) arrayList1.ToArray(typeof (Control));
    }

    /// <summary>
    /// Returns the current reference control for alignment operations.
    /// </summary>
    /// <returns></returns>
    public Control GetReferenceControl() => this.getReferenceControl(this.getSelectedControls());

    private Control getReferenceControl(Control[] selectedControls)
    {
      if (this.referenceControl == null)
        return selectedControls[selectedControls.Length - 1];
      foreach (Control selectedControl in selectedControls)
      {
        if (selectedControl == this.referenceControl)
          return selectedControl;
      }
      this.referenceControl = (Control) null;
      return selectedControls[selectedControls.Length - 1];
    }

    /// <summary>
    /// Moves all selected controls to the front of the stacking order.
    /// </summary>
    public void BringToFront()
    {
      foreach (Control selectedControl in this.getSelectedControls())
        selectedControl.BringToFront();
    }

    /// <summary>
    /// Sends all selected controls to the back of the stacking order.
    /// </summary>
    public void SendToBack()
    {
      foreach (Control selectedControl in this.getSelectedControls())
        selectedControl.SendToBack();
    }

    /// <summary>
    /// Creates a <see cref="T:EllieMae.Encompass.Forms.CopyBuffer" /> from the selected controls.
    /// </summary>
    /// <returns>The Copy method creates the buffer but does not modify the contents of the Form.
    /// </returns>
    public CopyBuffer Copy()
    {
      this.parentForm.EnsureEditing();
      return new CopyBuffer(this.getSelectedControls(false));
    }

    /// <summary>
    /// Creates a <see cref="T:EllieMae.Encompass.Forms.CopyBuffer" /> from the selected controls and deletes them from the Form.
    /// </summary>
    /// <returns>Returns a <see cref="T:EllieMae.Encompass.Forms.CopyBuffer" /> created from the selected controls.</returns>
    public CopyBuffer Cut()
    {
      this.parentForm.EnsureEditing();
      Control[] selectedControls = this.getSelectedControls(false);
      CopyBuffer copyBuffer = new CopyBuffer(selectedControls);
      for (int index = 0; index < selectedControls.Length; ++index)
      {
        if (selectedControls[index].AllowCutCopyDelete)
          selectedControls[index].Delete();
      }
      return copyBuffer;
    }

    /// <summary>Removes the selected controls from the Form.</summary>
    public void Delete()
    {
      this.parentForm.EnsureEditing();
      foreach (Control selectedControl in this.getSelectedControls(false))
      {
        if (selectedControl.AllowCutCopyDelete)
        {
          try
          {
            selectedControl.Delete();
          }
          catch
          {
          }
        }
      }
    }

    /// <summary>
    /// Aligns the selected controls with the reference control.
    /// </summary>
    /// <param name="alignment">The alignment method to use for this operation.</param>
    public void Align(Alignment alignment)
    {
      this.parentForm.EnsureEditing();
      Control[] selectedControls = this.getSelectedControls();
      Control referenceControl = this.getReferenceControl(selectedControls);
      Point position1 = referenceControl.Position;
      Size size1 = referenceControl.Size;
      for (int index = 0; index < selectedControls.Length; ++index)
      {
        if (selectedControls[index] != referenceControl)
        {
          Point position2 = selectedControls[index].Position;
          Size size2 = selectedControls[index].Size;
          switch (alignment)
          {
            case Alignment.Left:
              selectedControls[index].Position = new Point(position1.X, position2.Y);
              continue;
            case Alignment.Center:
              selectedControls[index].Position = new Point(position1.X + (size1.Width - size2.Width) / 2, position2.Y);
              continue;
            case Alignment.Right:
              selectedControls[index].Position = new Point(position1.X + (size1.Width - size2.Width), position2.Y);
              continue;
            case Alignment.Top:
              selectedControls[index].Position = new Point(position2.X, position1.Y);
              continue;
            case Alignment.Middle:
              selectedControls[index].Position = new Point(position2.X, position1.Y + (size1.Height - size2.Height) / 2);
              continue;
            case Alignment.Bottom:
              selectedControls[index].Position = new Point(position2.X, position1.Y + (size1.Height - size2.Height));
              continue;
            default:
              continue;
          }
        }
      }
    }

    /// <summary>
    /// Resizes the selected controls to the size of the reference control.
    /// </summary>
    /// <param name="method">Determine which dimension(s) are to be resized.</param>
    public void MakeSameSize(ResizeMethod method)
    {
      this.parentForm.EnsureEditing();
      Control[] selectedControls = this.getSelectedControls();
      Control referenceControl = this.getReferenceControl(selectedControls);
      Size size1 = referenceControl.Size;
      for (int index = 0; index < selectedControls.Length; ++index)
      {
        if (selectedControls[index] != referenceControl)
        {
          Size size2 = selectedControls[index].Size;
          int width = size2.Width;
          int height = size2.Height;
          if ((method & ResizeMethod.Width) != ResizeMethod.None)
            width = size1.Width;
          if ((method & ResizeMethod.Height) != ResizeMethod.None)
            height = size1.Height;
          selectedControls[index].Size = new Size(width, height);
        }
      }
    }

    /// <summary>
    /// Moves the selected controls to a specified position on the form.
    /// </summary>
    /// <param name="location">The position to move the controls.</param>
    public void MoveTo(Location location)
    {
      this.parentForm.EnsureEditing();
      foreach (Control selectedControl in this.getSelectedControls())
      {
        if (selectedControl.AllowPositioning)
          selectedControl.MoveTo(location);
      }
    }

    /// <summary>
    /// Moves the selected control by a fixed number of pixels.
    /// </summary>
    /// <param name="offsetX">The number of pixels to move horizontally.</param>
    /// <param name="offsetY">The number of pixels to move vertically.</param>
    public void Move(int offsetX, int offsetY)
    {
      this.parentForm.EnsureEditing();
      foreach (Control selectedControl in this.getSelectedControls())
      {
        Point position = selectedControl.Position;
        if (selectedControl.AllowPositioning)
          selectedControl.Position = new Point(position.X + offsetX, position.Y + offsetY);
      }
    }

    /// <summary>
    /// Resizes all selectd control by the specified number of pixels.
    /// </summary>
    /// <param name="deltaWidth">The number of pixels by which to expand or contract the width.</param>
    /// <param name="deltaHeight">The number of pixels by which to expand or contract the height.</param>
    public void Resize(int deltaWidth, int deltaHeight)
    {
      this.parentForm.EnsureEditing();
      foreach (Control selectedControl in this.getSelectedControls())
      {
        Size size = selectedControl.Size;
        selectedControl.Size = new Size(size.Width + deltaWidth, size.Height + deltaHeight);
      }
    }

    /// <summary>
    /// Moves the selected controls into the specified <see cref="T:EllieMae.Encompass.Forms.ContainerControl" />.
    /// </summary>
    /// <param name="container">The ContainerControl into which the selected controls are to be moved.</param>
    public void MoveInto(ContainerControl container)
    {
      foreach (Control levelSelectedControl in this.getTopLevelSelectedControls())
        container.Controls.Insert(levelSelectedControl);
    }

    internal Control[] getTopLevelSelectedControls()
    {
      ArrayList arrayList = new ArrayList((ICollection) this.getSelectedControls());
      Hashtable hashtable = new Hashtable();
      foreach (Control control in arrayList)
      {
        if (control is ContainerControl)
          hashtable[(object) control.ControlID] = (object) true;
      }
      for (int index = arrayList.Count - 1; index >= 0 && hashtable.Count > 0; --index)
      {
        for (ContainerControl container = ((Control) arrayList[index]).GetContainer(); container != null; container = container.GetContainer())
        {
          if (hashtable.Contains((object) container.ControlID))
          {
            arrayList.RemoveAt(index);
            break;
          }
        }
      }
      return (Control[]) arrayList.ToArray(typeof (Control));
    }

    /// <summary>Adds a set of controls into a control range</summary>
    private void addControlsToRange(IEnumerable controls, IHTMLControlRange range, bool recurse)
    {
      IHTMLControlRange2 htmlControlRange2 = (IHTMLControlRange2) range;
      foreach (Control control in controls)
      {
        htmlControlRange2.addElement(control.HTMLElement);
        if (recurse && control is ContainerControl)
          this.addControlsToRange((IEnumerable) ((ContainerControl) control).Controls, range, true);
      }
    }
  }
}
