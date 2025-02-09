// Decompiled with JetBrains decompiler
// Type: Elli.Common.WorkflowState.StateMachine`2
// Assembly: Elli.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 5A516607-8D77-4351-85BB-54751A6A69B4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Common.dll

using Elli.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Elli.Common.WorkflowState
{
  public class StateMachine<TState, TTrigger>
  {
    private readonly IDictionary<TState, StateMachine<TState, TTrigger>.StateRepresentation> _stateConfiguration = (IDictionary<TState, StateMachine<TState, TTrigger>.StateRepresentation>) new Dictionary<TState, StateMachine<TState, TTrigger>.StateRepresentation>();
    private readonly IDictionary<TTrigger, StateMachine<TState, TTrigger>.TriggerWithParameters> _triggerConfiguration = (IDictionary<TTrigger, StateMachine<TState, TTrigger>.TriggerWithParameters>) new Dictionary<TTrigger, StateMachine<TState, TTrigger>.TriggerWithParameters>();
    private readonly Func<TState> _stateAccessor;
    private readonly Action<TState> _stateMutator;
    private Action<TState, TTrigger> _unhandledTriggerAction = new Action<TState, TTrigger>(StateMachine<TState, TTrigger>.DefaultUnhandledTriggerAction);

    private event Action<StateMachine<TState, TTrigger>.Transition> _onTransitioned;

    public StateMachine(Func<TState> stateAccessor, Action<TState> stateMutator)
    {
      this._stateAccessor = Enforce.ArgumentNotNull<Func<TState>>(stateAccessor, nameof (stateAccessor));
      this._stateMutator = Enforce.ArgumentNotNull<Action<TState>>(stateMutator, nameof (stateMutator));
    }

    public StateMachine(TState initialState)
    {
      StateMachine<TState, TTrigger>.StateReference reference = new StateMachine<TState, TTrigger>.StateReference()
      {
        State = initialState
      };
      this._stateAccessor = (Func<TState>) (() => reference.State);
      this._stateMutator = (Action<TState>) (s => reference.State = s);
    }

    public TState State
    {
      get => this._stateAccessor();
      private set => this._stateMutator(value);
    }

    public IEnumerable<TTrigger> PermittedTriggers => this.CurrentRepresentation.PermittedTriggers;

    private StateMachine<TState, TTrigger>.StateRepresentation CurrentRepresentation
    {
      get => this.GetRepresentation(this.State);
    }

    private StateMachine<TState, TTrigger>.StateRepresentation GetRepresentation(TState state)
    {
      StateMachine<TState, TTrigger>.StateRepresentation representation;
      if (!this._stateConfiguration.TryGetValue(state, out representation))
      {
        representation = new StateMachine<TState, TTrigger>.StateRepresentation(state);
        this._stateConfiguration.Add(state, representation);
      }
      return representation;
    }

    public bool OnActionErrorResumeNext { get; set; }

    public StateMachine<TState, TTrigger>.StateConfiguration Configure(TState state)
    {
      return new StateMachine<TState, TTrigger>.StateConfiguration(this.GetRepresentation(state), new Func<TState, StateMachine<TState, TTrigger>.StateRepresentation>(this.GetRepresentation));
    }

    public TState[] GetConfiguredStates()
    {
      List<TState> stateList = new List<TState>();
      if (this._stateConfiguration == null)
        return stateList.ToArray();
      stateList.AddRange((IEnumerable<TState>) this._stateConfiguration.Keys);
      return stateList.ToArray();
    }

    public void Fire(TTrigger trigger) => this.InternalFire(trigger);

    public void Fire<TArg0>(
      StateMachine<TState, TTrigger>.TriggerWithParameters<TArg0> trigger,
      TArg0 arg0)
    {
      Enforce.ArgumentNotNull<StateMachine<TState, TTrigger>.TriggerWithParameters<TArg0>>(trigger, nameof (trigger));
      this.InternalFire(trigger.Trigger, (object) arg0);
    }

    public void Fire<TArg0, TArg1>(
      StateMachine<TState, TTrigger>.TriggerWithParameters<TArg0, TArg1> trigger,
      TArg0 arg0,
      TArg1 arg1)
    {
      Enforce.ArgumentNotNull<StateMachine<TState, TTrigger>.TriggerWithParameters<TArg0, TArg1>>(trigger, nameof (trigger));
      this.InternalFire(trigger.Trigger, (object) arg0, (object) arg1);
    }

    public void Fire<TArg0, TArg1, TArg2>(
      StateMachine<TState, TTrigger>.TriggerWithParameters<TArg0, TArg1, TArg2> trigger,
      TArg0 arg0,
      TArg1 arg1,
      TArg2 arg2)
    {
      Enforce.ArgumentNotNull<StateMachine<TState, TTrigger>.TriggerWithParameters<TArg0, TArg1, TArg2>>(trigger, nameof (trigger));
      this.InternalFire(trigger.Trigger, (object) arg0, (object) arg1, (object) arg2);
    }

    private void InternalFire(TTrigger trigger, params object[] args)
    {
      StateMachine<TState, TTrigger>.TriggerWithParameters triggerWithParameters;
      if (this._triggerConfiguration.TryGetValue(trigger, out triggerWithParameters))
        triggerWithParameters.ValidateParameters(args);
      StateMachine<TState, TTrigger>.TriggerBehavior handler;
      if (!this.CurrentRepresentation.TryFindHandler(trigger, out handler))
      {
        this._unhandledTriggerAction(this.CurrentRepresentation.UnderlyingState, trigger);
      }
      else
      {
        TState state = this.State;
        TState destination;
        if (!handler.ResultsInTransitionFrom(state, args, out destination))
          return;
        StateMachine<TState, TTrigger>.Transition transition = new StateMachine<TState, TTrigger>.Transition(state, destination, trigger, this.OnActionErrorResumeNext);
        this.CurrentRepresentation.Exit(transition);
        this.State = transition.Destination;
        Action<StateMachine<TState, TTrigger>.Transition> onTransitioned = this._onTransitioned;
        if (onTransitioned != null)
          onTransitioned(transition);
        this.CurrentRepresentation.Enter(transition, args);
      }
    }

    public void OnUnhandledTrigger(Action<TState, TTrigger> unhandledTriggerAction)
    {
      this._unhandledTriggerAction = unhandledTriggerAction != null ? unhandledTriggerAction : throw new ArgumentNullException(nameof (unhandledTriggerAction));
    }

    public bool IsInState(TState state) => this.CurrentRepresentation.IsIncludedIn(state);

    public bool CanFire(TTrigger trigger) => this.CurrentRepresentation.CanHandle(trigger);

    public override string ToString()
    {
      return string.Format("StateMachine {{ State = {0}, PermittedTriggers = {{ {1} }}}}", (object) this.State, (object) string.Join(", ", this.PermittedTriggers.Select<TTrigger, string>((Func<TTrigger, string>) (t => t.ToString())).ToArray<string>()));
    }

    public StateMachine<TState, TTrigger>.TriggerWithParameters<TArg0> SetTriggerParameters<TArg0>(
      TTrigger trigger)
    {
      StateMachine<TState, TTrigger>.TriggerWithParameters<TArg0> trigger1 = new StateMachine<TState, TTrigger>.TriggerWithParameters<TArg0>(trigger);
      this.SaveTriggerConfiguration((StateMachine<TState, TTrigger>.TriggerWithParameters) trigger1);
      return trigger1;
    }

    public StateMachine<TState, TTrigger>.TriggerWithParameters<TArg0, TArg1> SetTriggerParameters<TArg0, TArg1>(
      TTrigger trigger)
    {
      StateMachine<TState, TTrigger>.TriggerWithParameters<TArg0, TArg1> trigger1 = new StateMachine<TState, TTrigger>.TriggerWithParameters<TArg0, TArg1>(trigger);
      this.SaveTriggerConfiguration((StateMachine<TState, TTrigger>.TriggerWithParameters) trigger1);
      return trigger1;
    }

    public StateMachine<TState, TTrigger>.TriggerWithParameters<TArg0, TArg1, TArg2> SetTriggerParameters<TArg0, TArg1, TArg2>(
      TTrigger trigger)
    {
      StateMachine<TState, TTrigger>.TriggerWithParameters<TArg0, TArg1, TArg2> trigger1 = new StateMachine<TState, TTrigger>.TriggerWithParameters<TArg0, TArg1, TArg2>(trigger);
      this.SaveTriggerConfiguration((StateMachine<TState, TTrigger>.TriggerWithParameters) trigger1);
      return trigger1;
    }

    private void SaveTriggerConfiguration(
      StateMachine<TState, TTrigger>.TriggerWithParameters trigger)
    {
      if (this._triggerConfiguration.ContainsKey(trigger.Trigger))
        throw new InvalidOperationException(string.Format("Parameters for the trigger '{0}' have already been configured.", (object) trigger));
      this._triggerConfiguration.Add(trigger.Trigger, trigger);
    }

    private static void DefaultUnhandledTriggerAction(TState state, TTrigger trigger)
    {
      throw new InvalidOperationException(string.Format("No valid leaving transitions are permitted from state '{1}' for trigger '{0}'. Consider ignoring the trigger.", (object) trigger, (object) state));
    }

    public void OnTransitioned(
      Action<StateMachine<TState, TTrigger>.Transition> onTransitionAction)
    {
      if (onTransitionAction == null)
        throw new ArgumentNullException(nameof (onTransitionAction));
      this._onTransitioned += onTransitionAction;
    }

    internal class DynamicTriggerBehavior : StateMachine<TState, TTrigger>.TriggerBehavior
    {
      private readonly Func<object[], TState> _destination;

      public DynamicTriggerBehavior(
        TTrigger trigger,
        Func<object[], TState> destination,
        Func<bool> guard)
        : base(trigger, guard)
      {
        this._destination = Enforce.ArgumentNotNull<Func<object[], TState>>(destination, nameof (destination));
      }

      public override bool ResultsInTransitionFrom(
        TState source,
        object[] args,
        out TState destination)
      {
        destination = this._destination(args);
        return true;
      }
    }

    internal class IgnoredTriggerBehavior(TTrigger trigger, Func<bool> guard) : 
      StateMachine<TState, TTrigger>.TriggerBehavior(trigger, guard)
    {
      public override bool ResultsInTransitionFrom(
        TState source,
        object[] args,
        out TState destination)
      {
        destination = default (TState);
        return false;
      }
    }

    public class StateConfiguration
    {
      private readonly StateMachine<TState, TTrigger>.StateRepresentation _representation;
      private readonly Func<TState, StateMachine<TState, TTrigger>.StateRepresentation> _lookup;
      private static readonly Func<bool> NoGuard = (Func<bool>) (() => true);

      internal StateConfiguration(
        StateMachine<TState, TTrigger>.StateRepresentation representation,
        Func<TState, StateMachine<TState, TTrigger>.StateRepresentation> lookup)
      {
        this._representation = Enforce.ArgumentNotNull<StateMachine<TState, TTrigger>.StateRepresentation>(representation, nameof (representation));
        this._lookup = Enforce.ArgumentNotNull<Func<TState, StateMachine<TState, TTrigger>.StateRepresentation>>(lookup, nameof (lookup));
      }

      public StateMachine<TState, TTrigger>.StateConfiguration Permit(
        TTrigger trigger,
        TState destinationState)
      {
        this.EnforceNotIdentityTransition(destinationState);
        return this.InternalPermit(trigger, destinationState);
      }

      public StateMachine<TState, TTrigger>.StateConfiguration PermitIf(
        TTrigger trigger,
        TState destinationState,
        Func<bool> guard)
      {
        this.EnforceNotIdentityTransition(destinationState);
        return this.InternalPermitIf(trigger, destinationState, guard);
      }

      public StateMachine<TState, TTrigger>.StateConfiguration PermitReentry(TTrigger trigger)
      {
        return this.InternalPermit(trigger, this._representation.UnderlyingState);
      }

      public StateMachine<TState, TTrigger>.StateConfiguration PermitReentryIf(
        TTrigger trigger,
        Func<bool> guard)
      {
        return this.InternalPermitIf(trigger, this._representation.UnderlyingState, guard);
      }

      public StateMachine<TState, TTrigger>.StateConfiguration Ignore(TTrigger trigger)
      {
        return this.IgnoreIf(trigger, StateMachine<TState, TTrigger>.StateConfiguration.NoGuard);
      }

      public StateMachine<TState, TTrigger>.StateConfiguration IgnoreIf(
        TTrigger trigger,
        Func<bool> guard)
      {
        Enforce.ArgumentNotNull<Func<bool>>(guard, nameof (guard));
        this._representation.AddTriggerBehavior((StateMachine<TState, TTrigger>.TriggerBehavior) new StateMachine<TState, TTrigger>.IgnoredTriggerBehavior(trigger, guard));
        return this;
      }

      public StateMachine<TState, TTrigger>.StateConfiguration OnEntry(Action entryAction)
      {
        Enforce.ArgumentNotNull<Action>(entryAction, nameof (entryAction));
        return this.OnEntry((Action<StateMachine<TState, TTrigger>.Transition>) (t => entryAction()));
      }

      public StateMachine<TState, TTrigger>.StateConfiguration OnEntry(
        Action<StateMachine<TState, TTrigger>.Transition> entryAction)
      {
        Enforce.ArgumentNotNull<Action<StateMachine<TState, TTrigger>.Transition>>(entryAction, nameof (entryAction));
        this._representation.AddEntryAction((Action<StateMachine<TState, TTrigger>.Transition, object[]>) ((t, args) => entryAction(t)));
        return this;
      }

      public StateMachine<TState, TTrigger>.StateConfiguration OnEntryFrom(
        TTrigger trigger,
        Action entryAction)
      {
        Enforce.ArgumentNotNull<Action>(entryAction, nameof (entryAction));
        return this.OnEntryFrom(trigger, (Action<StateMachine<TState, TTrigger>.Transition>) (t => entryAction()));
      }

      public StateMachine<TState, TTrigger>.StateConfiguration OnEntryFrom(
        TTrigger trigger,
        Action<StateMachine<TState, TTrigger>.Transition> entryAction)
      {
        Enforce.ArgumentNotNull<Action<StateMachine<TState, TTrigger>.Transition>>(entryAction, nameof (entryAction));
        this._representation.AddEntryAction(trigger, (Action<StateMachine<TState, TTrigger>.Transition, object[]>) ((t, args) => entryAction(t)));
        return this;
      }

      public StateMachine<TState, TTrigger>.StateConfiguration OnEntryFrom<TArg0>(
        StateMachine<TState, TTrigger>.TriggerWithParameters<TArg0> trigger,
        Action<TArg0> entryAction)
      {
        Enforce.ArgumentNotNull<Action<TArg0>>(entryAction, nameof (entryAction));
        return this.OnEntryFrom<TArg0>(trigger, (Action<TArg0, StateMachine<TState, TTrigger>.Transition>) ((a0, t) => entryAction(a0)));
      }

      public StateMachine<TState, TTrigger>.StateConfiguration OnEntryFrom<TArg0>(
        StateMachine<TState, TTrigger>.TriggerWithParameters<TArg0> trigger,
        Action<TArg0, StateMachine<TState, TTrigger>.Transition> entryAction)
      {
        Enforce.ArgumentNotNull<Action<TArg0, StateMachine<TState, TTrigger>.Transition>>(entryAction, nameof (entryAction));
        Enforce.ArgumentNotNull<StateMachine<TState, TTrigger>.TriggerWithParameters<TArg0>>(trigger, nameof (trigger));
        this._representation.AddEntryAction(trigger.Trigger, (Action<StateMachine<TState, TTrigger>.Transition, object[]>) ((t, args) => entryAction(ParameterConversion.Unpack<TArg0>(args, 0), t)));
        return this;
      }

      public StateMachine<TState, TTrigger>.StateConfiguration OnEntryFrom<TArg0, TArg1>(
        StateMachine<TState, TTrigger>.TriggerWithParameters<TArg0, TArg1> trigger,
        Action<TArg0, TArg1> entryAction)
      {
        Enforce.ArgumentNotNull<Action<TArg0, TArg1>>(entryAction, nameof (entryAction));
        return this.OnEntryFrom<TArg0, TArg1>(trigger, (Action<TArg0, TArg1, StateMachine<TState, TTrigger>.Transition>) ((a0, a1, t) => entryAction(a0, a1)));
      }

      public StateMachine<TState, TTrigger>.StateConfiguration OnEntryFrom<TArg0, TArg1>(
        StateMachine<TState, TTrigger>.TriggerWithParameters<TArg0, TArg1> trigger,
        Action<TArg0, TArg1, StateMachine<TState, TTrigger>.Transition> entryAction)
      {
        Enforce.ArgumentNotNull<Action<TArg0, TArg1, StateMachine<TState, TTrigger>.Transition>>(entryAction, nameof (entryAction));
        Enforce.ArgumentNotNull<StateMachine<TState, TTrigger>.TriggerWithParameters<TArg0, TArg1>>(trigger, nameof (trigger));
        this._representation.AddEntryAction(trigger.Trigger, (Action<StateMachine<TState, TTrigger>.Transition, object[]>) ((t, args) => entryAction(ParameterConversion.Unpack<TArg0>(args, 0), ParameterConversion.Unpack<TArg1>(args, 1), t)));
        return this;
      }

      public StateMachine<TState, TTrigger>.StateConfiguration OnEntryFrom<TArg0, TArg1, TArg2>(
        StateMachine<TState, TTrigger>.TriggerWithParameters<TArg0, TArg1, TArg2> trigger,
        Action<TArg0, TArg1, TArg2> entryAction)
      {
        Enforce.ArgumentNotNull<Action<TArg0, TArg1, TArg2>>(entryAction, nameof (entryAction));
        return this.OnEntryFrom<TArg0, TArg1, TArg2>(trigger, (Action<TArg0, TArg1, TArg2, StateMachine<TState, TTrigger>.Transition>) ((a0, a1, a2, t) => entryAction(a0, a1, a2)));
      }

      public StateMachine<TState, TTrigger>.StateConfiguration OnEntryFrom<TArg0, TArg1, TArg2>(
        StateMachine<TState, TTrigger>.TriggerWithParameters<TArg0, TArg1, TArg2> trigger,
        Action<TArg0, TArg1, TArg2, StateMachine<TState, TTrigger>.Transition> entryAction)
      {
        Enforce.ArgumentNotNull<Action<TArg0, TArg1, TArg2, StateMachine<TState, TTrigger>.Transition>>(entryAction, nameof (entryAction));
        Enforce.ArgumentNotNull<StateMachine<TState, TTrigger>.TriggerWithParameters<TArg0, TArg1, TArg2>>(trigger, nameof (trigger));
        this._representation.AddEntryAction(trigger.Trigger, (Action<StateMachine<TState, TTrigger>.Transition, object[]>) ((t, args) => entryAction(ParameterConversion.Unpack<TArg0>(args, 0), ParameterConversion.Unpack<TArg1>(args, 1), ParameterConversion.Unpack<TArg2>(args, 2), t)));
        return this;
      }

      public StateMachine<TState, TTrigger>.StateConfiguration OnExit(Action exitAction)
      {
        Enforce.ArgumentNotNull<Action>(exitAction, nameof (exitAction));
        return this.OnExit((Action<StateMachine<TState, TTrigger>.Transition>) (t => exitAction()));
      }

      public StateMachine<TState, TTrigger>.StateConfiguration OnExit(
        Action<StateMachine<TState, TTrigger>.Transition> exitAction)
      {
        Enforce.ArgumentNotNull<Action<StateMachine<TState, TTrigger>.Transition>>(exitAction, nameof (exitAction));
        this._representation.AddExitAction(exitAction);
        return this;
      }

      public StateMachine<TState, TTrigger>.StateConfiguration SubstateOf(TState superstate)
      {
        StateMachine<TState, TTrigger>.StateRepresentation stateRepresentation = this._lookup(superstate);
        this._representation.Superstate = stateRepresentation;
        stateRepresentation.AddSubstate(this._representation);
        return this;
      }

      public StateMachine<TState, TTrigger>.StateConfiguration PermitDynamic(
        TTrigger trigger,
        Func<TState> destinationStateSelector)
      {
        return this.PermitDynamicIf(trigger, destinationStateSelector, StateMachine<TState, TTrigger>.StateConfiguration.NoGuard);
      }

      public StateMachine<TState, TTrigger>.StateConfiguration PermitDynamic<TArg0>(
        StateMachine<TState, TTrigger>.TriggerWithParameters<TArg0> trigger,
        Func<TArg0, TState> destinationStateSelector)
      {
        return this.PermitDynamicIf<TArg0>(trigger, destinationStateSelector, StateMachine<TState, TTrigger>.StateConfiguration.NoGuard);
      }

      public StateMachine<TState, TTrigger>.StateConfiguration PermitDynamic<TArg0, TArg1>(
        StateMachine<TState, TTrigger>.TriggerWithParameters<TArg0, TArg1> trigger,
        Func<TArg0, TArg1, TState> destinationStateSelector)
      {
        return this.PermitDynamicIf<TArg0, TArg1>(trigger, destinationStateSelector, StateMachine<TState, TTrigger>.StateConfiguration.NoGuard);
      }

      public StateMachine<TState, TTrigger>.StateConfiguration PermitDynamic<TArg0, TArg1, TArg2>(
        StateMachine<TState, TTrigger>.TriggerWithParameters<TArg0, TArg1, TArg2> trigger,
        Func<TArg0, TArg1, TArg2, TState> destinationStateSelector)
      {
        return this.PermitDynamicIf<TArg0, TArg1, TArg2>(trigger, destinationStateSelector, StateMachine<TState, TTrigger>.StateConfiguration.NoGuard);
      }

      public StateMachine<TState, TTrigger>.StateConfiguration PermitDynamicIf(
        TTrigger trigger,
        Func<TState> destinationStateSelector,
        Func<bool> guard)
      {
        Enforce.ArgumentNotNull<Func<TState>>(destinationStateSelector, nameof (destinationStateSelector));
        return this.InternalPermitDynamicIf(trigger, (Func<object[], TState>) (args => destinationStateSelector()), guard);
      }

      public StateMachine<TState, TTrigger>.StateConfiguration PermitDynamicIf<TArg0>(
        StateMachine<TState, TTrigger>.TriggerWithParameters<TArg0> trigger,
        Func<TArg0, TState> destinationStateSelector,
        Func<bool> guard)
      {
        Enforce.ArgumentNotNull<StateMachine<TState, TTrigger>.TriggerWithParameters<TArg0>>(trigger, nameof (trigger));
        Enforce.ArgumentNotNull<Func<TArg0, TState>>(destinationStateSelector, nameof (destinationStateSelector));
        return this.InternalPermitDynamicIf(trigger.Trigger, (Func<object[], TState>) (args => destinationStateSelector(ParameterConversion.Unpack<TArg0>(args, 0))), guard);
      }

      public StateMachine<TState, TTrigger>.StateConfiguration PermitDynamicIf<TArg0, TArg1>(
        StateMachine<TState, TTrigger>.TriggerWithParameters<TArg0, TArg1> trigger,
        Func<TArg0, TArg1, TState> destinationStateSelector,
        Func<bool> guard)
      {
        Enforce.ArgumentNotNull<StateMachine<TState, TTrigger>.TriggerWithParameters<TArg0, TArg1>>(trigger, nameof (trigger));
        Enforce.ArgumentNotNull<Func<TArg0, TArg1, TState>>(destinationStateSelector, nameof (destinationStateSelector));
        return this.InternalPermitDynamicIf(trigger.Trigger, (Func<object[], TState>) (args => destinationStateSelector(ParameterConversion.Unpack<TArg0>(args, 0), ParameterConversion.Unpack<TArg1>(args, 1))), guard);
      }

      public StateMachine<TState, TTrigger>.StateConfiguration PermitDynamicIf<TArg0, TArg1, TArg2>(
        StateMachine<TState, TTrigger>.TriggerWithParameters<TArg0, TArg1, TArg2> trigger,
        Func<TArg0, TArg1, TArg2, TState> destinationStateSelector,
        Func<bool> guard)
      {
        Enforce.ArgumentNotNull<StateMachine<TState, TTrigger>.TriggerWithParameters<TArg0, TArg1, TArg2>>(trigger, nameof (trigger));
        Enforce.ArgumentNotNull<Func<TArg0, TArg1, TArg2, TState>>(destinationStateSelector, nameof (destinationStateSelector));
        return this.InternalPermitDynamicIf(trigger.Trigger, (Func<object[], TState>) (args => destinationStateSelector(ParameterConversion.Unpack<TArg0>(args, 0), ParameterConversion.Unpack<TArg1>(args, 1), ParameterConversion.Unpack<TArg2>(args, 2))), guard);
      }

      private void EnforceNotIdentityTransition(TState destination)
      {
        if (destination.Equals((object) this._representation.UnderlyingState))
          throw new ArgumentException("Permit() (and PermitIf()) require that the destination state is not equal to the source state. To accept a trigger without changing state, use either Ignore() or PermitReentry().");
      }

      private StateMachine<TState, TTrigger>.StateConfiguration InternalPermit(
        TTrigger trigger,
        TState destinationState)
      {
        return this.InternalPermitIf(trigger, destinationState, (Func<bool>) (() => true));
      }

      private StateMachine<TState, TTrigger>.StateConfiguration InternalPermitIf(
        TTrigger trigger,
        TState destinationState,
        Func<bool> guard)
      {
        Enforce.ArgumentNotNull<Func<bool>>(guard, nameof (guard));
        this._representation.AddTriggerBehavior((StateMachine<TState, TTrigger>.TriggerBehavior) new StateMachine<TState, TTrigger>.TransitioningTriggerBehavior(trigger, destinationState, guard));
        return this;
      }

      private StateMachine<TState, TTrigger>.StateConfiguration InternalPermitDynamic(
        TTrigger trigger,
        Func<object[], TState> destinationStateSelector)
      {
        return this.InternalPermitDynamicIf(trigger, destinationStateSelector, StateMachine<TState, TTrigger>.StateConfiguration.NoGuard);
      }

      private StateMachine<TState, TTrigger>.StateConfiguration InternalPermitDynamicIf(
        TTrigger trigger,
        Func<object[], TState> destinationStateSelector,
        Func<bool> guard)
      {
        Enforce.ArgumentNotNull<Func<object[], TState>>(destinationStateSelector, nameof (destinationStateSelector));
        Enforce.ArgumentNotNull<Func<bool>>(guard, nameof (guard));
        this._representation.AddTriggerBehavior((StateMachine<TState, TTrigger>.TriggerBehavior) new StateMachine<TState, TTrigger>.DynamicTriggerBehavior(trigger, destinationStateSelector, guard));
        return this;
      }
    }

    internal class StateReference
    {
      public TState State { get; set; }
    }

    internal class StateRepresentation
    {
      private readonly TState _state;
      private readonly IDictionary<TTrigger, ICollection<StateMachine<TState, TTrigger>.TriggerBehavior>> _triggerBehaviors = (IDictionary<TTrigger, ICollection<StateMachine<TState, TTrigger>.TriggerBehavior>>) new Dictionary<TTrigger, ICollection<StateMachine<TState, TTrigger>.TriggerBehavior>>();
      private readonly ICollection<Action<StateMachine<TState, TTrigger>.Transition, object[]>> _entryActions = (ICollection<Action<StateMachine<TState, TTrigger>.Transition, object[]>>) new List<Action<StateMachine<TState, TTrigger>.Transition, object[]>>();
      private readonly ICollection<Action<StateMachine<TState, TTrigger>.Transition>> _exitActions = (ICollection<Action<StateMachine<TState, TTrigger>.Transition>>) new List<Action<StateMachine<TState, TTrigger>.Transition>>();
      private StateMachine<TState, TTrigger>.StateRepresentation _superstate;
      private readonly ICollection<StateMachine<TState, TTrigger>.StateRepresentation> _substates = (ICollection<StateMachine<TState, TTrigger>.StateRepresentation>) new List<StateMachine<TState, TTrigger>.StateRepresentation>();

      public StateRepresentation(TState state) => this._state = state;

      public bool CanHandle(TTrigger trigger)
      {
        return this.TryFindHandler(trigger, out StateMachine<TState, TTrigger>.TriggerBehavior _);
      }

      public bool TryFindHandler(
        TTrigger trigger,
        out StateMachine<TState, TTrigger>.TriggerBehavior handler)
      {
        if (this.TryFindLocalHandler(trigger, out handler))
          return true;
        return this.Superstate != null && this.Superstate.TryFindHandler(trigger, out handler);
      }

      private bool TryFindLocalHandler(
        TTrigger trigger,
        out StateMachine<TState, TTrigger>.TriggerBehavior handler)
      {
        ICollection<StateMachine<TState, TTrigger>.TriggerBehavior> source;
        if (!this._triggerBehaviors.TryGetValue(trigger, out source))
        {
          handler = (StateMachine<TState, TTrigger>.TriggerBehavior) null;
          return false;
        }
        StateMachine<TState, TTrigger>.TriggerBehavior[] array = source.Where<StateMachine<TState, TTrigger>.TriggerBehavior>((Func<StateMachine<TState, TTrigger>.TriggerBehavior, bool>) (at => at.IsGuardConditionMet)).ToArray<StateMachine<TState, TTrigger>.TriggerBehavior>();
        handler = ((IEnumerable<StateMachine<TState, TTrigger>.TriggerBehavior>) array).Count<StateMachine<TState, TTrigger>.TriggerBehavior>() <= 1 ? ((IEnumerable<StateMachine<TState, TTrigger>.TriggerBehavior>) array).FirstOrDefault<StateMachine<TState, TTrigger>.TriggerBehavior>() : throw new InvalidOperationException(string.Format("Multiple permitted exit transitions are configured from state '{1}' for trigger '{0}'. Guard clauses must be mutually exclusive.", (object) trigger, (object) this._state));
        return handler != null;
      }

      public void AddEntryAction(
        TTrigger trigger,
        Action<StateMachine<TState, TTrigger>.Transition, object[]> action)
      {
        Enforce.ArgumentNotNull<Action<StateMachine<TState, TTrigger>.Transition, object[]>>(action, nameof (action));
        this._entryActions.Add((Action<StateMachine<TState, TTrigger>.Transition, object[]>) ((t, args) =>
        {
          if (!t.Trigger.Equals((object) trigger))
            return;
          action(t, args);
        }));
      }

      public void AddEntryAction(
        Action<StateMachine<TState, TTrigger>.Transition, object[]> action)
      {
        this._entryActions.Add(Enforce.ArgumentNotNull<Action<StateMachine<TState, TTrigger>.Transition, object[]>>(action, nameof (action)));
      }

      public void AddExitAction(
        Action<StateMachine<TState, TTrigger>.Transition> action)
      {
        this._exitActions.Add(Enforce.ArgumentNotNull<Action<StateMachine<TState, TTrigger>.Transition>>(action, nameof (action)));
      }

      public void Enter(
        StateMachine<TState, TTrigger>.Transition transition,
        params object[] entryArgs)
      {
        Enforce.ArgumentNotNull<StateMachine<TState, TTrigger>.Transition>(transition, nameof (transition));
        if (transition.IsReentry)
        {
          this.ExecuteEntryActions(transition, entryArgs);
        }
        else
        {
          if (this.Includes(transition.Source))
            return;
          if (this._superstate != null)
            this._superstate.Enter(transition, entryArgs);
          this.ExecuteEntryActions(transition, entryArgs);
        }
      }

      public void Exit(
        StateMachine<TState, TTrigger>.Transition transition)
      {
        Enforce.ArgumentNotNull<StateMachine<TState, TTrigger>.Transition>(transition, nameof (transition));
        if (transition.IsReentry)
        {
          this.ExecuteExitActions(transition);
        }
        else
        {
          if (this.Includes(transition.Destination))
            return;
          this.ExecuteExitActions(transition);
          if (this._superstate == null)
            return;
          this._superstate.Exit(transition);
        }
      }

      private void ExecuteEntryActions(
        StateMachine<TState, TTrigger>.Transition transition,
        object[] entryArgs)
      {
        Enforce.ArgumentNotNull<StateMachine<TState, TTrigger>.Transition>(transition, nameof (transition));
        Enforce.ArgumentNotNull<object[]>(entryArgs, nameof (entryArgs));
        foreach (Action<StateMachine<TState, TTrigger>.Transition, object[]> entryAction in (IEnumerable<Action<StateMachine<TState, TTrigger>.Transition, object[]>>) this._entryActions)
        {
          try
          {
            entryAction(transition, entryArgs);
          }
          catch (Exception ex)
          {
            if (ex is IInsuppressible)
              throw;
            else if (!transition.OnActionErrorResumeNext)
              throw;
          }
        }
      }

      private void ExecuteExitActions(
        StateMachine<TState, TTrigger>.Transition transition)
      {
        Enforce.ArgumentNotNull<StateMachine<TState, TTrigger>.Transition>(transition, nameof (transition));
        foreach (Action<StateMachine<TState, TTrigger>.Transition> exitAction in (IEnumerable<Action<StateMachine<TState, TTrigger>.Transition>>) this._exitActions)
        {
          try
          {
            exitAction(transition);
          }
          catch
          {
            if (!transition.OnActionErrorResumeNext)
              throw;
          }
        }
      }

      public void AddTriggerBehavior(
        StateMachine<TState, TTrigger>.TriggerBehavior triggerBehavior)
      {
        ICollection<StateMachine<TState, TTrigger>.TriggerBehavior> triggerBehaviors;
        if (!this._triggerBehaviors.TryGetValue(triggerBehavior.Trigger, out triggerBehaviors))
        {
          triggerBehaviors = (ICollection<StateMachine<TState, TTrigger>.TriggerBehavior>) new List<StateMachine<TState, TTrigger>.TriggerBehavior>();
          this._triggerBehaviors.Add(triggerBehavior.Trigger, triggerBehaviors);
        }
        triggerBehaviors.Add(triggerBehavior);
      }

      public StateMachine<TState, TTrigger>.StateRepresentation Superstate
      {
        get => this._superstate;
        set => this._superstate = value;
      }

      public TState UnderlyingState => this._state;

      public void AddSubstate(
        StateMachine<TState, TTrigger>.StateRepresentation substate)
      {
        Enforce.ArgumentNotNull<StateMachine<TState, TTrigger>.StateRepresentation>(substate, nameof (substate));
        this._substates.Add(substate);
      }

      public bool Includes(TState state)
      {
        return this._state.Equals((object) state) || this._substates.Any<StateMachine<TState, TTrigger>.StateRepresentation>((Func<StateMachine<TState, TTrigger>.StateRepresentation, bool>) (s => s.Includes(state)));
      }

      public bool IsIncludedIn(TState state)
      {
        if (this._state.Equals((object) state))
          return true;
        return this._superstate != null && this._superstate.IsIncludedIn(state);
      }

      public IEnumerable<TTrigger> PermittedTriggers
      {
        get
        {
          IEnumerable<TTrigger> triggers = this._triggerBehaviors.Where<KeyValuePair<TTrigger, ICollection<StateMachine<TState, TTrigger>.TriggerBehavior>>>((Func<KeyValuePair<TTrigger, ICollection<StateMachine<TState, TTrigger>.TriggerBehavior>>, bool>) (t => t.Value.Any<StateMachine<TState, TTrigger>.TriggerBehavior>((Func<StateMachine<TState, TTrigger>.TriggerBehavior, bool>) (a => a.IsGuardConditionMet)))).Select<KeyValuePair<TTrigger, ICollection<StateMachine<TState, TTrigger>.TriggerBehavior>>, TTrigger>((Func<KeyValuePair<TTrigger, ICollection<StateMachine<TState, TTrigger>.TriggerBehavior>>, TTrigger>) (t => t.Key));
          if (this.Superstate != null)
            triggers = triggers.Union<TTrigger>(this.Superstate.PermittedTriggers);
          return (IEnumerable<TTrigger>) triggers.ToArray<TTrigger>();
        }
      }
    }

    public class Transition
    {
      private readonly TState _source;
      private readonly TState _destination;
      private readonly TTrigger _trigger;
      private readonly bool _onActionErrorResumeNext;

      public Transition(TState source, TState destination, TTrigger trigger)
        : this(source, destination, trigger, false)
      {
      }

      public Transition(
        TState source,
        TState destination,
        TTrigger trigger,
        bool onActionErrorResumeNext)
      {
        this._source = source;
        this._destination = destination;
        this._trigger = trigger;
        this._onActionErrorResumeNext = onActionErrorResumeNext;
      }

      public TState Source => this._source;

      public TState Destination => this._destination;

      public TTrigger Trigger => this._trigger;

      public bool IsReentry => this.Source.Equals((object) this.Destination);

      public bool OnActionErrorResumeNext => this._onActionErrorResumeNext;
    }

    internal class TransitioningTriggerBehavior : StateMachine<TState, TTrigger>.TriggerBehavior
    {
      private readonly TState _destination;

      public TransitioningTriggerBehavior(TTrigger trigger, TState destination, Func<bool> guard)
        : base(trigger, guard)
      {
        this._destination = destination;
      }

      public override bool ResultsInTransitionFrom(
        TState source,
        object[] args,
        out TState destination)
      {
        destination = this._destination;
        return true;
      }
    }

    internal abstract class TriggerBehavior
    {
      private readonly TTrigger _trigger;
      private readonly Func<bool> _guard;

      protected TriggerBehavior(TTrigger trigger, Func<bool> guard)
      {
        this._trigger = trigger;
        this._guard = guard;
      }

      public TTrigger Trigger => this._trigger;

      public bool IsGuardConditionMet => this._guard();

      public abstract bool ResultsInTransitionFrom(
        TState source,
        object[] args,
        out TState destination);
    }

    public abstract class TriggerWithParameters
    {
      private readonly TTrigger _underlyingTrigger;
      private readonly Type[] _argumentTypes;

      public TriggerWithParameters(TTrigger underlyingTrigger, params Type[] argumentTypes)
      {
        Enforce.ArgumentNotNull<Type[]>(argumentTypes, nameof (argumentTypes));
        this._underlyingTrigger = underlyingTrigger;
        this._argumentTypes = argumentTypes;
      }

      public TTrigger Trigger => this._underlyingTrigger;

      public void ValidateParameters(object[] args)
      {
        Enforce.ArgumentNotNull<object[]>(args, nameof (args));
        ParameterConversion.Validate(args, this._argumentTypes);
      }
    }

    public class TriggerWithParameters<TArg0>(TTrigger underlyingTrigger) : 
      StateMachine<TState, TTrigger>.TriggerWithParameters(underlyingTrigger, typeof (TArg0))
    {
    }

    public class TriggerWithParameters<TArg0, TArg1>(TTrigger underlyingTrigger) : 
      StateMachine<TState, TTrigger>.TriggerWithParameters(underlyingTrigger, typeof (TArg0), typeof (TArg1))
    {
    }

    public class TriggerWithParameters<TArg0, TArg1, TArg2>(TTrigger underlyingTrigger) : 
      StateMachine<TState, TTrigger>.TriggerWithParameters(underlyingTrigger, typeof (TArg0), typeof (TArg1), typeof (TArg2))
    {
    }
  }
}
