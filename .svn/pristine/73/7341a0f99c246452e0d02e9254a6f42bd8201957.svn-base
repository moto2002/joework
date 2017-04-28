using ShiHuanJue.Event;

public class GameManager
{
    private static GameManager m_inst;
    public static GameManager Inst
    {
        get
        {
            if (m_inst == null)
                m_inst = new GameManager();
            return m_inst;
        }
    }

    public GameManager()
    {
        ResourcesManager resourcesManager = ResourcesManager.Instance;
        NetworkManager networkManager = NetworkManager.Instance;
    }

    public void GameStart()
    {
        SceneLoadManager.Instance.SceneSwitch(new SceneMainTown());
    }

    public void GameUpdate()
    {
        EventDispatcher.TriggerEvent(EventsConst.FrameWork.Update);
    }

    public void GameDestroy()
    {

    }

    public void GameQuit()
    {

    }
}
