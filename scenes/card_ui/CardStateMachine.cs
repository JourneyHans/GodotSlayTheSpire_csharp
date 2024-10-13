using System.Collections.Generic;
using Godot;

public partial class CardStateMachine : Node {
	[Export] public CardState InitialState;

	private CardState _currentState;
	private Dictionary<CardState.EState, CardState> _states = new();

	public void Init(CardUI card) {
		foreach (Node child in GetChildren()) {
			if (child is CardState cardState) {
				_states.Add(cardState.State, cardState);
				cardState.TransitionRequested += OnTransitionRequested;
				cardState.CardUI = card;
			}
		}

		if (InitialState != null) {
			InitialState.Enter();
			_currentState = InitialState;
		}
	}

	public void OnInput(InputEvent inputEvent) {
		_currentState.OnInput(inputEvent);
	}

	public void OnGUIInput(InputEvent inputEvent) {
		_currentState.OnGUIInput(inputEvent);
	}

	public void OnMouseEntered() {
		_currentState.OnMouseEntered();
	}

	public void OnMouseExited() {
		_currentState.OnMouseExited();
	}

	private void OnTransitionRequested(CardState from, CardState.EState to) {
		if (from != _currentState) {
			GD.PrintErr($"当前状态不匹配, from: {from}, _currentState: {_currentState}");
			return;
		}

		if (!_states.TryGetValue(to, out CardState newState)) {
			GD.PrintErr($"不存在要切换的状态, to: {to}");
			return;
		}

		_currentState?.Exit();
		newState.Enter();
		_currentState = newState;
	}
}