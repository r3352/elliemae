// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.LOConnectRestAPIHelper
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.RemotingServices;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json.Linq;
using RestApiProxy;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  internal static class LOConnectRestAPIHelper
  {
    public const string customToolsBasePath = "/encompass/v1/settings/extensions/customTools";
    public const string customFormsBasePath = "/schema/getAllAssets?filter=Form";

    public static string SessionId
    {
      get
      {
        return (Session.DefaultInstance.ServerIdentity != null ? Session.DefaultInstance.ServerIdentity.InstanceName : "LOCALHOST") + "_" + Session.DefaultInstance.SessionID;
      }
    }

    public static Dictionary<LoConnectCustomServiceInfo, string> GetCustomServices()
    {
      Dictionary<LoConnectCustomServiceInfo, string> customServices = new Dictionary<LoConnectCustomServiceInfo, string>();
      string customTools = LOConnectRestAPIHelper.GetCustomTools();
      if (customTools != "null")
      {
        object obj1 = (object) JArray.Parse(customTools);
        // ISSUE: reference to a compiler-generated field
        if (LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__15 == null)
        {
          // ISSUE: reference to a compiler-generated field
          LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__15 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (IEnumerable), typeof (LOConnectRestAPIHelper)));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        foreach (object obj2 in LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__15.Target((CallSite) LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__15, obj1))
        {
          // ISSUE: reference to a compiler-generated field
          if (LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__2 == null)
          {
            // ISSUE: reference to a compiler-generated field
            LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (LOConnectRestAPIHelper), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, bool> target1 = LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__2.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, bool>> p2 = LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__2;
          // ISSUE: reference to a compiler-generated field
          if (LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__1 == null)
          {
            // ISSUE: reference to a compiler-generated field
            LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (LOConnectRestAPIHelper), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, bool, object> target2 = LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__1.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, bool, object>> p1 = LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__1;
          // ISSUE: reference to a compiler-generated field
          if (LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__0 == null)
          {
            // ISSUE: reference to a compiler-generated field
            LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "isGlobal", typeof (LOConnectRestAPIHelper), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj3 = LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__0.Target((CallSite) LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__0, obj2);
          object obj4 = target2((CallSite) p1, obj3, true);
          if (target1((CallSite) p2, obj4))
          {
            // ISSUE: reference to a compiler-generated field
            if (LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__5 == null)
            {
              // ISSUE: reference to a compiler-generated field
              LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__5 = CallSite<Func<CallSite, System.Type, object, LoServiceType, LoConnectCustomServiceInfo>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, typeof (LOConnectRestAPIHelper), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, System.Type, object, LoServiceType, LoConnectCustomServiceInfo> target3 = LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__5.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, System.Type, object, LoServiceType, LoConnectCustomServiceInfo>> p5 = LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__5;
            System.Type type = typeof (LoConnectCustomServiceInfo);
            // ISSUE: reference to a compiler-generated field
            if (LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__4 == null)
            {
              // ISSUE: reference to a compiler-generated field
              LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<System.Type>) null, typeof (LOConnectRestAPIHelper), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, object> target4 = LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__4.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, object>> p4 = LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__4;
            // ISSUE: reference to a compiler-generated field
            if (LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__3 == null)
            {
              // ISSUE: reference to a compiler-generated field
              LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "customToolId", typeof (LOConnectRestAPIHelper), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj5 = LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__3.Target((CallSite) LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__3, obj2);
            object obj6 = target4((CallSite) p4, obj5);
            LoConnectCustomServiceInfo customServiceInfo1 = target3((CallSite) p5, type, obj6, LoServiceType.GlobalTool);
            // ISSUE: reference to a compiler-generated field
            if (LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__8 == null)
            {
              // ISSUE: reference to a compiler-generated field
              LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__8 = CallSite<Action<CallSite, Dictionary<LoConnectCustomServiceInfo, string>, LoConnectCustomServiceInfo, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", (IEnumerable<System.Type>) null, typeof (LOConnectRestAPIHelper), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Action<CallSite, Dictionary<LoConnectCustomServiceInfo, string>, LoConnectCustomServiceInfo, object> target5 = LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__8.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Action<CallSite, Dictionary<LoConnectCustomServiceInfo, string>, LoConnectCustomServiceInfo, object>> p8 = LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__8;
            Dictionary<LoConnectCustomServiceInfo, string> dictionary = customServices;
            LoConnectCustomServiceInfo customServiceInfo2 = customServiceInfo1;
            // ISSUE: reference to a compiler-generated field
            if (LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__7 == null)
            {
              // ISSUE: reference to a compiler-generated field
              LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<System.Type>) null, typeof (LOConnectRestAPIHelper), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, object> target6 = LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__7.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, object>> p7 = LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__7;
            // ISSUE: reference to a compiler-generated field
            if (LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__6 == null)
            {
              // ISSUE: reference to a compiler-generated field
              LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "name", typeof (LOConnectRestAPIHelper), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj7 = LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__6.Target((CallSite) LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__6, obj2);
            object obj8 = target6((CallSite) p7, obj7);
            target5((CallSite) p8, dictionary, customServiceInfo2, obj8);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__11 == null)
            {
              // ISSUE: reference to a compiler-generated field
              LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__11 = CallSite<Func<CallSite, System.Type, object, LoServiceType, LoConnectCustomServiceInfo>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, typeof (LOConnectRestAPIHelper), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, System.Type, object, LoServiceType, LoConnectCustomServiceInfo> target7 = LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__11.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, System.Type, object, LoServiceType, LoConnectCustomServiceInfo>> p11 = LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__11;
            System.Type type = typeof (LoConnectCustomServiceInfo);
            // ISSUE: reference to a compiler-generated field
            if (LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__10 == null)
            {
              // ISSUE: reference to a compiler-generated field
              LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__10 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<System.Type>) null, typeof (LOConnectRestAPIHelper), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, object> target8 = LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__10.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, object>> p10 = LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__10;
            // ISSUE: reference to a compiler-generated field
            if (LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__9 == null)
            {
              // ISSUE: reference to a compiler-generated field
              LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__9 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "customToolId", typeof (LOConnectRestAPIHelper), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj9 = LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__9.Target((CallSite) LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__9, obj2);
            object obj10 = target8((CallSite) p10, obj9);
            LoConnectCustomServiceInfo customServiceInfo3 = target7((CallSite) p11, type, obj10, LoServiceType.Tool);
            // ISSUE: reference to a compiler-generated field
            if (LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__14 == null)
            {
              // ISSUE: reference to a compiler-generated field
              LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__14 = CallSite<Action<CallSite, Dictionary<LoConnectCustomServiceInfo, string>, LoConnectCustomServiceInfo, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", (IEnumerable<System.Type>) null, typeof (LOConnectRestAPIHelper), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Action<CallSite, Dictionary<LoConnectCustomServiceInfo, string>, LoConnectCustomServiceInfo, object> target9 = LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__14.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Action<CallSite, Dictionary<LoConnectCustomServiceInfo, string>, LoConnectCustomServiceInfo, object>> p14 = LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__14;
            Dictionary<LoConnectCustomServiceInfo, string> dictionary = customServices;
            LoConnectCustomServiceInfo customServiceInfo4 = customServiceInfo3;
            // ISSUE: reference to a compiler-generated field
            if (LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__13 == null)
            {
              // ISSUE: reference to a compiler-generated field
              LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__13 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<System.Type>) null, typeof (LOConnectRestAPIHelper), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, object> target10 = LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__13.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, object>> p13 = LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__13;
            // ISSUE: reference to a compiler-generated field
            if (LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__12 == null)
            {
              // ISSUE: reference to a compiler-generated field
              LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__12 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "name", typeof (LOConnectRestAPIHelper), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj11 = LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__12.Target((CallSite) LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__12, obj2);
            object obj12 = target10((CallSite) p13, obj11);
            target9((CallSite) p14, dictionary, customServiceInfo4, obj12);
          }
        }
      }
      string customForms = LOConnectRestAPIHelper.GetCustomForms();
      if (customForms != "null")
      {
        object obj13 = (object) JArray.Parse(customForms);
        // ISSUE: reference to a compiler-generated field
        if (LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__22 == null)
        {
          // ISSUE: reference to a compiler-generated field
          LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__22 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (IEnumerable), typeof (LOConnectRestAPIHelper)));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        foreach (object obj14 in LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__22.Target((CallSite) LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__22, obj13))
        {
          // ISSUE: reference to a compiler-generated field
          if (LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__18 == null)
          {
            // ISSUE: reference to a compiler-generated field
            LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__18 = CallSite<Func<CallSite, System.Type, object, LoServiceType, LoConnectCustomServiceInfo>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, typeof (LOConnectRestAPIHelper), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, System.Type, object, LoServiceType, LoConnectCustomServiceInfo> target11 = LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__18.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, System.Type, object, LoServiceType, LoConnectCustomServiceInfo>> p18 = LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__18;
          System.Type type = typeof (LoConnectCustomServiceInfo);
          // ISSUE: reference to a compiler-generated field
          if (LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__17 == null)
          {
            // ISSUE: reference to a compiler-generated field
            LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__17 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<System.Type>) null, typeof (LOConnectRestAPIHelper), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, object> target12 = LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__17.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, object>> p17 = LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__17;
          // ISSUE: reference to a compiler-generated field
          if (LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__16 == null)
          {
            // ISSUE: reference to a compiler-generated field
            LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__16 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "id", typeof (LOConnectRestAPIHelper), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj15 = LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__16.Target((CallSite) LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__16, obj14);
          object obj16 = target12((CallSite) p17, obj15);
          LoConnectCustomServiceInfo customServiceInfo5 = target11((CallSite) p18, type, obj16, LoServiceType.Form);
          // ISSUE: reference to a compiler-generated field
          if (LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__21 == null)
          {
            // ISSUE: reference to a compiler-generated field
            LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__21 = CallSite<Action<CallSite, Dictionary<LoConnectCustomServiceInfo, string>, LoConnectCustomServiceInfo, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", (IEnumerable<System.Type>) null, typeof (LOConnectRestAPIHelper), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Action<CallSite, Dictionary<LoConnectCustomServiceInfo, string>, LoConnectCustomServiceInfo, object> target13 = LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__21.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Action<CallSite, Dictionary<LoConnectCustomServiceInfo, string>, LoConnectCustomServiceInfo, object>> p21 = LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__21;
          Dictionary<LoConnectCustomServiceInfo, string> dictionary = customServices;
          LoConnectCustomServiceInfo customServiceInfo6 = customServiceInfo5;
          // ISSUE: reference to a compiler-generated field
          if (LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__20 == null)
          {
            // ISSUE: reference to a compiler-generated field
            LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__20 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<System.Type>) null, typeof (LOConnectRestAPIHelper), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, object> target14 = LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__20.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, object>> p20 = LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__20;
          // ISSUE: reference to a compiler-generated field
          if (LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__19 == null)
          {
            // ISSUE: reference to a compiler-generated field
            LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__19 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "name", typeof (LOConnectRestAPIHelper), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj17 = LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__19.Target((CallSite) LOConnectRestAPIHelper.\u003C\u003Eo__4.\u003C\u003Ep__19, obj14);
          object obj18 = target14((CallSite) p20, obj17);
          target13((CallSite) p21, dictionary, customServiceInfo6, obj18);
        }
      }
      return customServices;
    }

    public static string GetCustomTools()
    {
      return RestApiProxyFactory.CreateOAPIGatewayRestApiProxy(LOConnectRestAPIHelper.SessionId, "application/json").GetStringAsync("/encompass/v1/settings/extensions/customTools").Result;
    }

    public static string GetCustomForms()
    {
      return RestApiProxyFactory.CreateRestApiProxy(LOConnectRestAPIHelper.SessionId, false, "application/json").GetStringAsync("/schema/getAllAssets?filter=Form").Result;
    }
  }
}
