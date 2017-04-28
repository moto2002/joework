using System;
using UnityEngine;

public class BlackHoleView : MonoBehaviour
{
    private enum CurShow
    {
        GameList,
        GameInfo,
        Menu,
        Quit,
        Journey,
    }
    private CurShow curShow;

    private MenuView menu;
    private GameListView gameList;
    private GameInfoView gameInfo;
    private QuitView quitView;
    private JourneyListView journeyListView;

    protected  void Awake()
    {          
        Debug.Log("BlackHoleView  Loaded add view.");
        Cursor.visible = false;
        curShow = CurShow.Menu;
        menu = transform.FindChild("Menu").gameObject.GetComponentForce<MenuView>();
        menu.ShowGameEvent = ShowGame;
        menu.ShowJourneyEvent = ShowJourney;
        menu.ShowMovieEvent = ShowMovie;
        menu.ShowQuitEvent = ShowQuit;

        gameList = transform.FindChild("GameList").gameObject.GetComponentForce<GameListView>();
        gameList.ShowGameInfo = ShowGameInfo;

        gameInfo = transform.FindChild("GameInfo").gameObject.GetComponentForce<GameInfoView>();
        gameInfo.BackToGameListEvent = ShowGame;

        quitView = transform.FindChild("QuitPanel").gameObject.GetComponentForce<QuitView>();
        journeyListView = transform.FindChild("JourneyList").gameObject.GetComponentForce<JourneyListView>();

        this.gameObject.AddComponent<StarSkyView>();
        RecordMgr recordMgr = RecordMgr.Inst;
        PlayTimeMgr playTime = PlayTimeMgr.Inst;

    }


    private void Start()
    {
       // menu.Reset(null);
    }

    void OnApplicationQuit()
    {
    }

    private void ShowGameInfo(string obj)
    {
        journeyListView.HideGameIcons(null);

        quitView.Hide();

        if (curShow != CurShow.GameInfo && curShow == CurShow.GameList)
        {
            gameList.HideGameIcons(() =>
            {
                curShow = CurShow.GameInfo;
                gameInfo.Show(obj);
            });
        }
        else
        {
            curShow = CurShow.GameInfo;
            gameInfo.Show(obj);
        }
    }

    private void ShowGame()
    {
        journeyListView.HideGameIcons(null);

        quitView.Hide();

        if (curShow != CurShow.GameList && curShow == CurShow.GameInfo)
        {
            gameInfo.Hide(() =>
            {
                curShow = CurShow.GameList;
                gameList.ShowGameIcons(null);
            });
        }
        else
        {
            curShow = CurShow.GameList;
            gameList.ShowGameIcons(null);
        }        
    }
    private void ShowJourney()
    {
        if (curShow!= CurShow.Journey)
        {
            quitView.Hide();
            gameList.HideGameIcons(null);
            gameInfo.Hide(null);
            curShow = CurShow.Journey;
            journeyListView.ShowGameIcons(null);
        }
    }
    private void ShowMovie()
    {
    }

    private void ShowQuit()
    {
       // if (curShow != CurShow.Quit)
        {
            journeyListView.HideGameIcons(null);

            gameList.HideGameIcons(() =>
            {

            });

            gameInfo.Hide(() =>
            {

            });

            curShow = CurShow.Quit;
            quitView.Show();

        }
       
    }

}