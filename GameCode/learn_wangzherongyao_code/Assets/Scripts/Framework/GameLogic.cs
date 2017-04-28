namespace Assets.Scripts.Framework
{
    public class GameLogic : Singleton<GameLogic>
    {
        public override void Init()
        {

        }

        public override void UnInit()
        {

        }

        public void UpdateLogic(int nDelta)
        {
            Singleton<CTimerManager>.GetInstance().UpdateLogic(nDelta);
        }
    }
}