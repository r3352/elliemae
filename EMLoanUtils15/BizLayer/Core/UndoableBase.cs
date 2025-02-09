// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.BizLayer.Core.UndoableBase
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer.BizLayer;
using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;

#nullable disable
namespace EllieMae.EMLite.BizLayer.Core
{
  [Serializable]
  public abstract class UndoableBase : BindableBase
  {
    [EllieMae.EMLite.BizLayer.NotUndoable]
    private Stack _stateStack = new Stack();

    protected int EditLevel => this._stateStack.Count;

    protected internal void CopyState()
    {
      Type type = this.GetType();
      Hashtable graph = new Hashtable();
      do
      {
        foreach (FieldInfo field in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
        {
          if (field.DeclaringType == type && !this.NotUndoableField(field))
          {
            object obj = field.GetValue((object) this);
            if (field.FieldType.IsSubclassOf(typeof (BusinessCollectionBase)))
              ((BusinessCollectionBase) obj)?.CopyState();
            else if (field.FieldType.IsSubclassOf(typeof (BusinessBase)))
              ((UndoableBase) obj)?.CopyState();
            else if (field.FieldType.IsSubclassOf(typeof (BusinessInfoBase)))
            {
              ((BusinessInfoBase) obj)?.CopyState();
            }
            else
            {
              string key = field.DeclaringType.Name + "." + field.Name;
              graph.Add((object) key, obj);
            }
          }
        }
        type = type.BaseType;
      }
      while (type != typeof (UndoableBase));
      MemoryStream serializationStream = new MemoryStream();
      new BinaryFormatter().Serialize((Stream) serializationStream, (object) graph);
      this._stateStack.Push((object) serializationStream.ToArray());
    }

    protected internal void UndoChanges()
    {
      if (this.EditLevel <= 0)
        return;
      MemoryStream serializationStream = new MemoryStream((byte[]) this._stateStack.Pop());
      serializationStream.Position = 0L;
      Hashtable hashtable = (Hashtable) new BinaryFormatter().Deserialize((Stream) serializationStream);
      Type type = this.GetType();
      do
      {
        foreach (FieldInfo field in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
        {
          if (field.DeclaringType == type && !this.NotUndoableField(field))
          {
            object obj = field.GetValue((object) this);
            if (field.FieldType.IsSubclassOf(typeof (BusinessCollectionBase)))
              ((BusinessCollectionBase) obj)?.UndoChanges();
            else if (field.FieldType.IsSubclassOf(typeof (BusinessBase)))
              ((UndoableBase) obj)?.UndoChanges();
            else if (field.FieldType.IsSubclassOf(typeof (BusinessInfoBase)))
            {
              ((BusinessInfoBase) obj)?.UndoChanges();
            }
            else
            {
              string key = field.DeclaringType.Name + "." + field.Name;
              field.SetValue((object) this, hashtable[(object) key]);
            }
          }
        }
        type = type.BaseType;
      }
      while (type != typeof (UndoableBase));
    }

    protected internal void AcceptChanges()
    {
      if (this.EditLevel <= 0)
        return;
      this._stateStack.Pop();
      Type type = this.GetType();
      do
      {
        foreach (FieldInfo field in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
        {
          if (field.DeclaringType == type && !this.NotUndoableField(field))
          {
            object obj = field.GetValue((object) this);
            if (field.FieldType.IsSubclassOf(typeof (BusinessCollectionBase)))
              ((BusinessCollectionBase) obj)?.AcceptChanges();
            else if (field.FieldType.IsSubclassOf(typeof (BusinessBase)))
              ((UndoableBase) obj)?.AcceptChanges();
            else if (field.FieldType.IsSubclassOf(typeof (BusinessInfoBase)) && obj != null)
              ((BusinessInfoBase) obj).AcceptChanges();
          }
        }
        type = type.BaseType;
      }
      while (type != typeof (UndoableBase));
    }

    private bool NotUndoableField(FieldInfo field)
    {
      return Attribute.IsDefined((MemberInfo) field, typeof (EllieMae.EMLite.BizLayer.NotUndoableAttribute));
    }
  }
}
