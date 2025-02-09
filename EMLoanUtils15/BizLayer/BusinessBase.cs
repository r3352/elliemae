// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.BizLayer.BusinessBase
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.BizLayer.Core;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

#nullable disable
namespace EllieMae.EMLite.BizLayer
{
  [Serializable]
  public abstract class BusinessBase : UndoableBase, IEditableObject, ICloneable, IDataErrorInfo
  {
    private bool _isNew = true;
    private bool _isDeleted;
    private bool _isDirty = true;
    [NotUndoable]
    [NonSerialized]
    private BusinessCollectionBase _parent;
    [NotUndoable]
    private bool _bindingEdit;
    private bool _neverCommitted = true;
    [NotUndoable]
    private bool _isChild;
    private int _editLevelAdded;
    private BrokenRules _brokenRules = new BrokenRules();

    public bool IsNew => this._isNew;

    public bool IsDeleted => this._isDeleted;

    public virtual bool IsDirty => this._isDirty;

    protected virtual void MarkNew()
    {
      this._isNew = true;
      this._isDeleted = false;
      this.MarkDirty();
    }

    protected virtual void MarkOld()
    {
      this._isNew = false;
      this.MarkClean();
    }

    protected void MarkDeleted()
    {
      this._isDeleted = true;
      this.MarkDirty();
    }

    public void MarkDirty()
    {
      this._isDirty = true;
      this.OnIsDirtyChanged();
    }

    private void MarkClean()
    {
      this._isDirty = false;
      this.OnIsDirtyChanged();
    }

    public virtual bool IsSavable => this.IsDirty && this.IsValid;

    internal void SetParent(BusinessCollectionBase parent)
    {
      if (!this.IsChild)
        return;
      this._parent = parent;
    }

    void IEditableObject.BeginEdit()
    {
      if (this._bindingEdit)
        return;
      this.BeginEdit();
    }

    void IEditableObject.CancelEdit()
    {
      if (!this._bindingEdit)
        return;
      this.CancelEdit();
      if (!this.IsNew || !this._neverCommitted || this.EditLevel > this.EditLevelAdded || this._parent == null)
        return;
      this._parent.RemoveChild(this);
    }

    void IEditableObject.EndEdit()
    {
      if (!this._bindingEdit)
        return;
      this.ApplyEdit();
    }

    public void BeginEdit()
    {
      this._bindingEdit = true;
      this.CopyState();
    }

    public void CancelEdit()
    {
      this._bindingEdit = false;
      this.UndoChanges();
      this.AddBusinessRules();
      this.OnIsDirtyChanged();
    }

    public void ApplyEdit()
    {
      this._bindingEdit = false;
      this._neverCommitted = false;
      this.AcceptChanges();
    }

    protected internal bool IsChild => this._isChild;

    protected void MarkAsChild() => this._isChild = true;

    public void Delete()
    {
      if (this.IsChild)
        throw new NotSupportedException("Can not directly mark a child object for deletion - use its parent collection");
      this.MarkDeleted();
    }

    internal void DeleteChild()
    {
      if (!this.IsChild)
        throw new NotSupportedException("Invalid for root objects - use Delete instead");
      this.MarkDeleted();
    }

    internal int EditLevelAdded
    {
      get => this._editLevelAdded;
      set => this._editLevelAdded = value;
    }

    public object Clone()
    {
      MemoryStream serializationStream = new MemoryStream();
      BinaryFormatter binaryFormatter = new BinaryFormatter();
      binaryFormatter.Serialize((Stream) serializationStream, (object) this);
      serializationStream.Position = 0L;
      return binaryFormatter.Deserialize((Stream) serializationStream);
    }

    protected internal virtual void AddBusinessRules()
    {
    }

    public virtual bool IsValid => this._brokenRules.IsValid;

    public virtual BrokenRules.RulesCollection GetBrokenRulesCollection()
    {
      return this._brokenRules.BrokenRulesCollection;
    }

    [XmlIgnore]
    public BrokenRules.RulesCollection BrokenRulesCollection
    {
      get => this._brokenRules.BrokenRulesCollection;
    }

    public virtual string GetBrokenRulesString() => this._brokenRules.ToString();

    public string BrokenRulesString => this._brokenRules.ToString();

    protected BrokenRules BrokenRules => this._brokenRules;

    public virtual BusinessBase Save()
    {
      if (this.IsChild)
        throw new NotSupportedException("Can not directly save a child object");
      if (this.EditLevel > 0)
        throw new ApplicationException("Object is still being edited and can not be saved");
      if (!this.IsValid)
        throw new ValidationException("Object is not valid and can not be saved");
      return this;
    }

    string IDataErrorInfo.Error
    {
      get
      {
        if (this.IsValid)
          return string.Empty;
        return this.BrokenRules.BrokenRulesCollection.Count == 1 ? this.BrokenRules.BrokenRulesCollection[0].Description : this.BrokenRules.ToString();
      }
    }

    string IDataErrorInfo.this[string columnName]
    {
      get
      {
        return !this.IsValid ? this.BrokenRules.BrokenRulesCollection.RuleForProperty(columnName).Description : string.Empty;
      }
    }
  }
}
