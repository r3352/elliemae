﻿// Decompiled with JetBrains decompiler
// Type: ProtoBuf.NetObjectCache
// Assembly: protobuf-net, Version=2.4.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 4CB0BA27-6305-4C8A-8F23-3BBBCE6B7DCC
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.xml

using ProtoBuf.Meta;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

#nullable disable
namespace ProtoBuf
{
  internal sealed class NetObjectCache
  {
    internal const int Root = 0;
    private MutableList underlyingList;
    private object rootObject;
    private int trapStartIndex;
    private Dictionary<string, int> stringKeys;
    private Dictionary<object, int> objectKeys;

    private MutableList List => this.underlyingList ?? (this.underlyingList = new MutableList());

    internal object GetKeyedObject(int key)
    {
      if (key-- == 0)
        return this.rootObject != null ? this.rootObject : throw new ProtoException("No root object assigned");
      BasicList list = (BasicList) this.List;
      if (key < 0 || key >= list.Count)
        throw new ProtoException("Internal error; a missing key occurred");
      return list[key] ?? throw new ProtoException("A deferred key does not have a value yet");
    }

    internal void SetKeyedObject(int key, object value)
    {
      if (key-- == 0)
      {
        if (value == null)
          throw new ArgumentNullException(nameof (value));
        this.rootObject = this.rootObject == null || this.rootObject == value ? value : throw new ProtoException("The root object cannot be reassigned");
      }
      else
      {
        MutableList list = this.List;
        if (key < list.Count)
        {
          object obj = list[key];
          if (obj == null)
            list[key] = value;
          else if (obj != value)
            throw new ProtoException("Reference-tracked objects cannot change reference");
        }
        else if (key != list.Add(value))
          throw new ProtoException("Internal error; a key mismatch occurred");
      }
    }

    internal int AddObjectKey(object value, out bool existing)
    {
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      if (value == this.rootObject)
      {
        existing = true;
        return 0;
      }
      string key = value as string;
      BasicList list = (BasicList) this.List;
      int num;
      if (key == null)
      {
        if (this.objectKeys == null)
        {
          this.objectKeys = new Dictionary<object, int>((IEqualityComparer<object>) NetObjectCache.ReferenceComparer.Default);
          num = -1;
        }
        else if (!this.objectKeys.TryGetValue(value, out num))
          num = -1;
      }
      else if (this.stringKeys == null)
      {
        this.stringKeys = new Dictionary<string, int>();
        num = -1;
      }
      else if (!this.stringKeys.TryGetValue(key, out num))
        num = -1;
      if (!(existing = num >= 0))
      {
        num = list.Add(value);
        if (key == null)
          this.objectKeys.Add(value, num);
        else
          this.stringKeys.Add(key, num);
      }
      return num + 1;
    }

    internal void RegisterTrappedObject(object value)
    {
      if (this.rootObject == null)
      {
        this.rootObject = value;
      }
      else
      {
        if (this.underlyingList == null)
          return;
        for (int trapStartIndex = this.trapStartIndex; trapStartIndex < this.underlyingList.Count; ++trapStartIndex)
        {
          this.trapStartIndex = trapStartIndex + 1;
          if (this.underlyingList[trapStartIndex] == null)
          {
            this.underlyingList[trapStartIndex] = value;
            break;
          }
        }
      }
    }

    internal void Clear()
    {
      this.trapStartIndex = 0;
      this.rootObject = (object) null;
      if (this.underlyingList != null)
        this.underlyingList.Clear();
      if (this.stringKeys != null)
        this.stringKeys.Clear();
      if (this.objectKeys == null)
        return;
      this.objectKeys.Clear();
    }

    private sealed class ReferenceComparer : IEqualityComparer<object>
    {
      public static readonly NetObjectCache.ReferenceComparer Default = new NetObjectCache.ReferenceComparer();

      private ReferenceComparer()
      {
      }

      bool IEqualityComparer<object>.Equals(object x, object y) => x == y;

      int IEqualityComparer<object>.GetHashCode(object obj) => RuntimeHelpers.GetHashCode(obj);
    }
  }
}
