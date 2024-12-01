using framework.extension;
using Godot;

public partial class RelicsControl : Control {
    private const int RelicsPerPage = 5;
    private const float TweenScrollDuration = 0.2f;

    [Export] public TextureButton LeftButton;
    [Export] public TextureButton RightButton;

    #region onready

    private HBoxContainer _relics;
    private float _offsetPerPage;

    #endregion

    private int _numOfRelics;
    private int _currentPage = 1;
    private int _maxPage;
    private Tween _tween;

    public override void _Ready() {
        _relics = GetNode<HBoxContainer>("%Relics");
        _offsetPerPage = CustomMinimumSize.X + _relics.GetSeparation();
        
        _relics.FreeAllChildren();

        LeftButton.Pressed += OnLeftButtonPressed;
        RightButton.Pressed += OnRightButtonPressed;
        _relics.ChildOrderChanged += OnRelicsChildOrderChanged;
    }

    private void Update() {
        if (!IsInstanceValid(LeftButton) || !IsInstanceValid(RightButton)) {
            return;
        }

        _numOfRelics = _relics.GetChildCount();
        _maxPage = Mathf.CeilToInt(_numOfRelics / (float)RelicsPerPage);

        LeftButton.Disabled = _currentPage <= 1;
        RightButton.Disabled = _currentPage >= _maxPage;
    }
    
    /*
     * 原作者的这里其实有bug，快速点击执行的Tween会因为上个
     * Tween没有执行完被kill后，计算的终点就不对了，圣物越多
     * 越容易触发。
     * 我这里采用了一个计算方法，保证了终点是根据_currentPage计算而来的，
     * 确保终点的一致性
     */
    private void TweenPage() {
        float xPos = (_currentPage - 1) * -_offsetPerPage;
        _tween?.Kill();
        _tween = CreateTween().SetTrans(Tween.TransitionType.Back).SetEase(Tween.EaseType.Out);
        _tween.DoMoveX(_relics, xPos, TweenScrollDuration);
    }

    private void OnLeftButtonPressed() {
        if (_currentPage <= 1) {
            return;
        }

        _currentPage--;
        Update();
        TweenPage();
    }

    private void OnRightButtonPressed() {
        if (_currentPage >= _maxPage) {
            return;
        }

        _currentPage++;
        Update();
        TweenPage();
    }

    private void OnRelicsChildOrderChanged() {
        Update();
    }
}