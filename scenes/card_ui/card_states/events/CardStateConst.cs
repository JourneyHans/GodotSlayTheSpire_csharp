/// <summary>
/// 卡牌状态相关事件
/// </summary>
public partial class CardState {
    public static class Event {
        /// <summary>
        /// 卡牌拖拽开始
        /// 参数1：CardUI
        /// </summary>
        public const string CardDragStarted = "CardDragStarted";
        
        /// <summary>
        /// 卡牌拖拽结束
        /// 参数1：CardUI
        /// </summary>
        public const string CardDragEnded = "CardDragEnded";
    
        /// <summary>
        /// 卡牌瞄准事件开始
        /// 参数1：CardUI
        /// </summary>
        public const string CardAimStarted = "CardAimStarted";
    
        /// <summary>
        /// 卡牌瞄准事件结束
        /// 参数1：CardUI
        /// </summary>
        public const string CardAimEnded = "CardAimEnded";
    }
}