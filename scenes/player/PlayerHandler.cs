/*
 * Player turn order:
 * 1. StartOfTurn Relics
 * 2. StartOfTurn Statuses
 * 3. Draw Hand
 * 4. End Turn
 * 5. EndOfTurn Relics
 * 6. EndOfTurn Statuses
 * 7. Discard Hand
 */

using framework.events;
using framework.extension;
using Godot;

public partial class PlayerHandler : Node {
    private const float HandDrawInterval = 0.25f;
    private const float HandDiscardInterval = 0.25f;
    
    [Export] public RelicHandler Relics { get; set; }
    [Export] public Player Player { get; private set; }
    [Export] public Hand Hand { get; private set; }

    public CharacterStats CharacterStats { get; private set; }

    public override void _Ready() {
        EventDispatcher.RegEventListener<Card>(Card.Event.CardPlayed, OnCardPlayed);
    }

    protected override void Dispose(bool disposing) {
        EventDispatcher.UnRegEventListener<Card>(Card.Event.CardPlayed, OnCardPlayed);
    }

    public void StartBattle(CharacterStats characterStats) {
        CharacterStats = characterStats;
        CharacterStats.DrawPile = characterStats.Deck.Duplicate<CardPile>(true);
        CharacterStats.DrawPile.Shuffle();
        CharacterStats.Discard = new CardPile();
        Relics.RelicsActivated += OnRelicsRelicsActivated;
        Player.StatusHandler.StatusesApplied += OnStatusesApplied;
        StartTurn();
    }

    public void StartTurn() {
        CharacterStats.Block = 0;
        CharacterStats.ResetMana();
        Relics.ActivateRelicsByType(Relic.EType.StartOfTurn);
    }

    public void EndTurn() {
        Hand.DisableHand();
        Relics.ActivateRelicsByType(Relic.EType.EndOfTurn);
    }

    private void DrawCards(int amount) {
        Tween tween = CreateTween();
        for (int i = 0; i < amount; i++) {
            tween.TweenCallback(Callable.From(DrawCard));
            tween.TweenInterval(HandDrawInterval);
        }

        tween.Finished += () => { EventDispatcher.TriggerEvent(Event.PlayerHandDrawn); };
    }

    private void DrawCard() {
        ReshuffleDeckFromDiscard();
        Hand.AddCard(CharacterStats.DrawPile.DrawCard());
        ReshuffleDeckFromDiscard();
    }

    private void ReshuffleDeckFromDiscard() {
        if (!CharacterStats.DrawPile.IsEmpty) {
            return;
        }

        while (!CharacterStats.Discard.IsEmpty) {
            CharacterStats.DrawPile.AddCard(CharacterStats.Discard.DrawCard());
        }

        CharacterStats.DrawPile.Shuffle();
    }

    private void DiscardCards() {
        if (Hand.GetChildren().IsNullOrEmpty()) {
            EventDispatcher.TriggerEvent(Event.PlayerHandDiscarded);
            return;
        }

        Tween tween = CreateTween();
        foreach (Node child in Hand.GetChildren()) {
            var cardUI = (CardUI)child;
            tween.TweenCallback(Callable.From(() => { CharacterStats.Discard.AddCard(cardUI.Card); }));
            tween.TweenCallback(Callable.From(() => { Hand.DiscardCard(cardUI); }));
            tween.TweenInterval(HandDiscardInterval);
        }

        tween.Finished += () => {
            EventDispatcher.TriggerEvent(Event.PlayerHandDiscarded);
        };
    }

    private void OnCardPlayed(Card card) {
        if (card.Exhausts || card.Type == Card.EType.Power) {
            // 消耗卡
            return;
        }

        CharacterStats.Discard.AddCard(card);
    }

    private void OnRelicsRelicsActivated(Relic.EType type) {
        switch (type) {
            case Relic.EType.StartOfTurn:
                Player.StatusHandler.ApplyStatusesByType(Status.EType.StartOfTurn);
                break;
            case Relic.EType.EndOfTurn:
                Player.StatusHandler.ApplyStatusesByType(Status.EType.EndOfTurn);
                break;
        }
    }

    private void OnStatusesApplied(Status.EType type) {
        switch (type) {
            case Status.EType.StartOfTurn:
                DrawCards(CharacterStats.CardsPerTurn);
                break;
            case Status.EType.EndOfTurn:
                DiscardCards();
                break;
        }
    }
}

// event
public partial class PlayerHandler {
    public static class Event {
        /// <summary>
        /// 玩家抽卡结束
        /// </summary>
        public const string PlayerHandDrawn = "PlayerHandDrawn";
        
        /// <summary>
        /// 玩家弃牌
        /// </summary>
        public const string PlayerHandDiscarded = "PlayerHandDiscarded";
        
        /// <summary>
        /// 回合结束
        /// </summary>
        public const string PlayerTurnEnded = "PlayerTurnEnded";
    }
}