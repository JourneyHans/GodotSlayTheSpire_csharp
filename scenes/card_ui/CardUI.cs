using framework.debug;
using framework.events;
using framework.extension;
using framework.utils;
using Godot;
using Godot.Collections;

public partial class CardUI : Control {
	public const string ReparentRequested = "ReparentRequested";
	
	private Card _card;
	public int OriginalIndex;

	[Export]
	public Card Card {
		get => _card;
		set => SetCard(value);
	}

	private CharacterStats _characterStats;
	[Export]
	public CharacterStats CharacterStats {
		get => _characterStats;
		set {
			_characterStats = value;
			EventDispatcher.SafeRegEventListener(Stats.Event.StatsChanged, OnCharacterStatsChanged);
		}
	}

	public CardVisuals CardVisuals { get; private set; }
	
	public Area2D DropPointDetector { get; private set; }
	public CardStateMachine CardStateMachine { get; private set; }

	public Array<Node2D> Targets { get; } = new();
	
	public Control Parent { get; set; }
	public Tween Tween { get;private set; }
	private FinchLogger _logger;

	// 卡牌是否可以被打出的标志（法力是否足够，是否被禁手等游戏逻辑标志）
	private bool _playable = true;
	public bool Playable {
		get => _playable;
		private set {
			_playable = value;
			if (!_playable) {
				CardVisuals.Cost.AddThemeColorOverride("font_color", Colors.Red);
				CardVisuals.Icon.Modulate = new Color(1, 1, 1, 0.5f);
			}
			else {
				CardVisuals.Cost.RemoveThemeColorOverride("font_color");
				CardVisuals.Icon.Modulate = new Color(1, 1, 1);
			}
		}
	}

	// 卡牌是否禁用标志（针对卡牌被拖动时，其他卡牌应该被禁用等程序逻辑标志）
	public bool Disabled { get; set; }

	public override void _Ready() {
		_logger = new FinchLogger(this);
		CardVisuals = GetNode<CardVisuals>("CardVisuals");
		DropPointDetector = GetNode<Area2D>("DropPointDetector");
		CardStateMachine = GetNode<CardStateMachine>("CardStateMachine");
		CardStateMachine.Init(this);

		GuiInput += OnGUIInput;
		MouseEntered += OnMouseEntered;
		MouseExited += OnMouseExited;
		
		DropPointDetector.AreaEntered += OnDropPointDetectorAreaEntered;
		DropPointDetector.AreaExited += OnDropPointDetectorAreaExited;
		
		EventDispatcher.RegEventListener<CardUI>(CardState.Event.CardAimStarted, OnCardDragOrAimStarted);
		EventDispatcher.RegEventListener<CardUI>(CardState.Event.CardDragStarted, OnCardDragOrAimStarted);
		EventDispatcher.RegEventListener<CardUI>(CardState.Event.CardAimEnded, OnCardDragOrAimEnded);
		EventDispatcher.RegEventListener<CardUI>(CardState.Event.CardDragEnded, OnCardDragOrAimEnded);
	}

	protected override void Dispose(bool disposing) {
		EventDispatcher.UnRegEventListener<CardUI>(CardState.Event.CardAimStarted, OnCardDragOrAimStarted);
		EventDispatcher.UnRegEventListener<CardUI>(CardState.Event.CardDragStarted, OnCardDragOrAimStarted);
		EventDispatcher.UnRegEventListener<CardUI>(CardState.Event.CardAimEnded, OnCardDragOrAimEnded);
		EventDispatcher.UnRegEventListener<CardUI>(CardState.Event.CardDragEnded, OnCardDragOrAimEnded);
		EventDispatcher.UnRegEventListener(Stats.Event.StatsChanged, OnCharacterStatsChanged);
	}

	public override void _Input(InputEvent inputEvent) {
		CardStateMachine.OnInput(inputEvent);
	}

	private async void SetCard(Card card) {
		if (!IsNodeReady()) {
			await this.WhenReady();
		}

		_card = card;
		CardVisuals.Card = Card;
	}

	public void AnimateToPosition(Vector2 newPosition, float duration) {
		Tween = CreateTween().SetTrans(Tween.TransitionType.Circ).SetEase(Tween.EaseType.Out);
		Tween.TweenProperty(this, "global_position", newPosition, duration);
	}

	public void Play() {
		if (Card == null) {
			_logger.Error("Card is null");
			return;
		}

		Card.Play(Targets, CharacterStats);
		QueueFree();
	}

	#region Signal: CardUI

	private void OnGUIInput(InputEvent inputEvent) {
		CardStateMachine.OnGUIInput(inputEvent);
	}

	private void OnMouseEntered() {
		CardStateMachine.OnMouseEntered();
	}

	private void OnMouseExited() {
		CardStateMachine.OnMouseExited();
	}
	
	#endregion

	#region Signal: DropPointDetector

	private void OnDropPointDetectorAreaEntered(Area2D area) {
		if (!Targets.Contains(area)) {
			Targets.Add(area);
		}
	}

	private void OnDropPointDetectorAreaExited(Area2D area) {
		Targets.Remove(area);
	}

	#endregion

	private void OnCardDragOrAimStarted(CardUI cardUI) {
		if (cardUI == this) {
			return;
		}

		Disabled = true;
	}

	private void OnCardDragOrAimEnded(CardUI cardUI) {
		Disabled = false;
		Playable = CharacterStats.CanPlayCard(Card);
	}

	private void OnCharacterStatsChanged() {
		Playable = CharacterStats.CanPlayCard(Card);
	}
}