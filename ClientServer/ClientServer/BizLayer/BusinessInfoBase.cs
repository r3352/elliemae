// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.BizLayer.BusinessInfoBase
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;

#nullable disable
namespace EllieMae.EMLite.ClientServer.BizLayer
{
  [Serializable]
  public abstract class BusinessInfoBase
  {
    [NotUndoable]
    private Stack stateStack = new Stack();

    public int EditLevel => this.stateStack.Count;

    public void CopyState()
    {
      Type type = this.GetType();
      Hashtable graph = new Hashtable();
      foreach (FieldInfo field in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
      {
        if (field.DeclaringType == type && !this.NotUndoableField(field))
        {
          object obj = field.GetValue((object) this);
          string key = field.DeclaringType.Name + "." + field.Name;
          graph.Add((object) key, obj);
        }
      }
      MemoryStream serializationStream = new MemoryStream();
      new BinaryFormatter().Serialize((Stream) serializationStream, (object) graph);
      this.stateStack.Push((object) serializationStream.ToArray());
    }

    public void UndoChanges()
    {
      if (this.EditLevel <= 0)
        return;
      MemoryStream serializationStream = new MemoryStream((byte[]) this.stateStack.Pop());
      serializationStream.Position = 0L;
      Hashtable hashtable = (Hashtable) new BinaryFormatter().Deserialize((Stream) serializationStream);
      Type type = this.GetType();
      foreach (FieldInfo field in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
      {
        if (field.DeclaringType == type && !this.NotUndoableField(field))
        {
          field.GetValue((object) this);
          string key = field.DeclaringType.Name + "." + field.Name;
          field.SetValue((object) this, hashtable[(object) key]);
        }
      }
    }

    public void AcceptChanges()
    {
      if (this.EditLevel <= 0)
        return;
      this.stateStack.Pop();
    }

    private bool NotUndoableField(FieldInfo field)
    {
      return Attribute.IsDefined((MemberInfo) field, typeof (NotUndoableAttribute));
    }
  }
}
