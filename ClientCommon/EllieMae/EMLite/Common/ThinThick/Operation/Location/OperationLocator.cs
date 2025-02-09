// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.ThinThick.Operation.Location.OperationLocator
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Common.ThinThick.Operation.Location
{
  public class OperationLocator : IOperationLocator
  {
    private static object _lock = new object();
    private static OperationLocator _instance;
    private Dictionary<Type, Type> _operations = new Dictionary<Type, Type>();

    public Dictionary<Type, Type> Operations => this._operations;

    public static OperationLocator GetInstance()
    {
      lock (OperationLocator._lock)
      {
        if (OperationLocator._instance == null)
          OperationLocator._instance = new OperationLocator();
      }
      return OperationLocator._instance;
    }

    public static void SetInstance(OperationLocator instance)
    {
      OperationLocator._instance = instance;
    }

    public static bool HasInstance => OperationLocator._instance != null;

    public void Register(Type interfaceType, Type operationType)
    {
      OperationLocator.GetInstance().Operations[interfaceType] = operationType;
    }

    public T Resolve<T>()
    {
      return (T) Activator.CreateInstance(OperationLocator.GetInstance().Operations[typeof (T)]);
    }

    public void Reset() => OperationLocator.GetInstance().Operations.Clear();
  }
}
