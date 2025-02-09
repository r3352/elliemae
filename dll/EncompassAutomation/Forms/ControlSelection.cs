// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.ControlSelection
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
  public class ControlSelection : IEnumerable
  {
    private Form parentForm;
    private Control referenceControl;

    internal ControlSelection(Form parentForm) => this.parentForm = parentForm;

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

    public IEnumerator GetEnumerator() => this.getSelectedControls().GetEnumerator();

    public int Count
    {
      get
      {
        IHTMLControlRange currentControlRange = this.getCurrentControlRange();
        return currentControlRange == null ? 0 : currentControlRange.length;
      }
    }

    public bool Contains(Control control)
    {
      foreach (object selectedControl in this.getSelectedControls())
      {
        if (selectedControl.Equals((object) control))
          return true;
      }
      return false;
    }

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

    public void Clear()
    {
      this.createControlRange().select();
      this.referenceControl = (Control) null;
    }

    public void Add(Control control)
    {
      this.AddRange((IEnumerable) new Control[1]{ control });
      this.referenceControl = control;
    }

    public void AddRange(IEnumerable controls)
    {
      IHTMLControlRange ihtmlControlRange = this.getCurrentControlRange() ?? this.createControlRange();
      IHTMLControlRange2 ihtmlControlRange2 = (IHTMLControlRange2) ihtmlControlRange;
      foreach (Control control in controls)
        ihtmlControlRange2.addElement(control.HTMLElement);
      ihtmlControlRange.select();
      this.referenceControl = (Control) null;
    }

    public void AddRange(IEnumerable controls, bool recurse)
    {
      IHTMLControlRange range = this.getCurrentControlRange() ?? this.createControlRange();
      this.addControlsToRange(controls, range, true);
      range.select();
      this.referenceControl = (Control) null;
    }

    internal void ReplaceRange(IEnumerable controls)
    {
      this.createControlRange().select();
      IHTMLControlRange ihtmlControlRange = this.getCurrentControlRange() ?? this.createControlRange();
      IHTMLControlRange2 ihtmlControlRange2 = (IHTMLControlRange2) ihtmlControlRange;
      foreach (Control control in controls)
        ihtmlControlRange2.addElement(control.HTMLElement);
      ihtmlControlRange.select();
    }

    public void SetReferenceControl(Control c) => this.referenceControl = c;

    public Control[] ToControlArray()
    {
      ArrayList arrayList = new ArrayList();
      foreach (Control control in this)
        arrayList.Add((object) control);
      return (Control[]) arrayList.ToArray(typeof (Control));
    }

    private IHTMLControlRange createControlRange()
    {
      return (IHTMLControlRange) ((DispHTMLBody) (((DispHTMLDocument) this.parentForm.GetHTMLDocument()).body as HTMLBody)).createControlRange();
    }

    private IHTMLControlRange getCurrentControlRange()
    {
      try
      {
        HTMLDocument htmlDocument = this.parentForm.GetHTMLDocument();
        return ((DispHTMLDocument) htmlDocument).selection == null || ((DispHTMLDocument) htmlDocument).selection.type != "Control" ? (IHTMLControlRange) null : (IHTMLControlRange) ((DispHTMLDocument) htmlDocument).selection.createRange();
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

    public void BringToFront()
    {
      foreach (Control selectedControl in this.getSelectedControls())
        selectedControl.BringToFront();
    }

    public void SendToBack()
    {
      foreach (Control selectedControl in this.getSelectedControls())
        selectedControl.SendToBack();
    }

    public CopyBuffer Copy()
    {
      this.parentForm.EnsureEditing();
      return new CopyBuffer(this.getSelectedControls(false));
    }

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

    public void MoveTo(Location location)
    {
      this.parentForm.EnsureEditing();
      foreach (Control selectedControl in this.getSelectedControls())
      {
        if (selectedControl.AllowPositioning)
          selectedControl.MoveTo(location);
      }
    }

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

    public void Resize(int deltaWidth, int deltaHeight)
    {
      this.parentForm.EnsureEditing();
      foreach (Control selectedControl in this.getSelectedControls())
      {
        Size size = selectedControl.Size;
        selectedControl.Size = new Size(size.Width + deltaWidth, size.Height + deltaHeight);
      }
    }

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

    private void addControlsToRange(IEnumerable controls, IHTMLControlRange range, bool recurse)
    {
      IHTMLControlRange2 ihtmlControlRange2 = (IHTMLControlRange2) range;
      foreach (Control control in controls)
      {
        ihtmlControlRange2.addElement(control.HTMLElement);
        if (recurse && control is ContainerControl)
          this.addControlsToRange((IEnumerable) ((ContainerControl) control).Controls, range, true);
      }
    }
  }
}
