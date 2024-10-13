using Godot;

public partial class Battle : Node2D {
    [Export] public CharacterStats CharacterStats { get; private set; }
    [Export] public AudioStream Music { get; private set; }
    
    private PlayerHandler _playerHandler;
    private EnemyHandler _enemyHandler;
    private BattleUI _battleUI;
    private Player _player;

    public override void _Ready() {
        _playerHandler = GetNode<PlayerHandler>("PlayerHandler");
        _enemyHandler = GetNode<EnemyHandler>("EnemyHandler");
        _battleUI = GetNode<BattleUI>("BattleUI");
        _player = GetNode<Player>("Player");
        
        CharacterStats newStat = CharacterStats.CreateInstance();
        _battleUI.CharacterStats = newStat;
        _player.Stats = newStat;
        
        EventDispatcher.RegEventListener(EnemyHandler.Event.EnemyTurnEnded, OnEnemyTurnEnded);
        EventDispatcher.RegEventListener(PlayerHandler.Event.PlayerTurnEnded, _playerHandler.EndTurn);
        EventDispatcher.RegEventListener(PlayerHandler.Event.PlayerHandDiscarded, _enemyHandler.StartTurn);
        
        EventDispatcher.RegEventListener(Player.Event.PlayerDied, OnPlayerDied);
        _enemyHandler.ChildOrderChanged += OnEnemiesChildOrderChanged;

        StartBattle(newStat);
    }

    protected override void Dispose(bool disposing) {
        EventDispatcher.UnRegEventListener(EnemyHandler.Event.EnemyTurnEnded, OnEnemyTurnEnded);
        EventDispatcher.UnRegEventListener(PlayerHandler.Event.PlayerTurnEnded, _playerHandler.EndTurn);
        EventDispatcher.UnRegEventListener(PlayerHandler.Event.PlayerHandDiscarded, _enemyHandler.StartTurn);
        EventDispatcher.UnRegEventListener(Player.Event.PlayerDied, OnPlayerDied);
    }

    private void StartBattle(CharacterStats stats) {
        GetTree().Paused = false;
        AudioPlayer.PlayMusic(Music, true);
        _playerHandler.StartBattle(stats);
        _enemyHandler.ResetEnemyActions();
    }

    private void OnEnemyTurnEnded() {
        _playerHandler.StartTurn();
        _enemyHandler.ResetEnemyActions();
    }

    private void OnEnemiesChildOrderChanged() {
        if (_enemyHandler.GetChildCount() == 0) {
            EventDispatcher.TriggerEvent(BattleOverPanel.Event.BattleOverScreenRequested, "Victorious",
                BattleOverPanel.Type.Win);
        }
    }

    private void OnPlayerDied() {
        EventDispatcher.TriggerEvent(BattleOverPanel.Event.BattleOverScreenRequested, "Game Over!",
            BattleOverPanel.Type.Lose);
    }
}