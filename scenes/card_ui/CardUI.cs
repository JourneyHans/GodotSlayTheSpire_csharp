using framework.debug;
using framework.extension;
using framework.utils;
using Godot;
using Godot.Collections;

public partial class CardUI : Control {
	public const string ReparentRequested = "ReparentRequested";
	
	private Card _card;
	public int OriginalIndex;

	public StyleBoxFlat BaseStyleBox =
		SimpleLoader.LoadResource<StyleBoxFlat>("res://scenes/card_ui/card_base_style_box");

	public StyleBoxFlat HoverStyleBox =
		SimpleLoader.LoadResource<StyleBoxFlat>("res://scenes/card_ui/card_hover_style_box");

	public StyleBoxFlat DraggingStyleBox =
		SimpleLoader.LoadResource<StyleBoxFlat>("res://scenes/card_ui/card_dragging_style_box");

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
			EventDispatcher.UnRegEventListener(Stats.Event.StatsChanged, OnCharacterStatsChanged);
			EventDispatcher.RegEventListener(Stats.Event.StatsChanged, OnCharacterStatsChanged);
		}
	}

	private Panel _panel;
	public Label Cost { get; private set; }
	public TextureRect Icon { get; private set; }
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
				Cost.AddThemeColorOverride("font_color", Colors.Red);
				Icon.Modulate = new Color(1, 1, 1, 0.5f);
			}
			else {
				Cost.RemoveThemeColorOverride("font_color");
				Icon.Modulate = new Color(1, 1, 1);
			}
		}
	}

	// 卡牌是否禁用标志（针对卡牌被拖动时，其他卡牌应该被禁用等程序逻辑标志）
	public bool Disabled { get; set; }

	public override void _Ready() {
		_logger = new FinchLogger(this);
		_panel = GetNode<Panel>("Panel");
		Cost = GetNode<Label>("Cost");
		Icon = GetNode<TextureRect>("Icon");
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
		Cost.Text = _card.Cost.ToString();
		Icon.Texture = _card.Icon;
	}

	public void SetPanelStyleBox(StyleBoxFlat theme) {
		_panel.Set("theme_override_styles/panel", theme);
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