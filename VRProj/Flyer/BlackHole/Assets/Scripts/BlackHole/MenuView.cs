using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VR;

public class MenuView : MonoBehaviour
{
    private RectTransform root, title, line1, line2, line3, game, journey, movie, quit;
    public Action ShowGameEvent, ShowJourneyEvent, ShowMovieEvent, ShowQuitEvent;
    public GameObject effect_row, effect_col;
    private Text text_game, text_journey, text_moive, text_quit;

    private void Awake()
    {
        Debug.Log("add MenuView .");
        root = transform.GetComponent<RectTransform>();
        title = transform.FindChild("Title").GetComponent<RectTransform>();
        line1 = transform.FindChild("line1").GetComponent<RectTransform>();
        line2 = transform.FindChild("line2").GetComponent<RectTransform>();
        line3 = transform.FindChild("line3").GetComponent<RectTransform>();
        game = transform.FindChild("Game").GetComponent<RectTransform>();
        journey = transform.FindChild("VRJourney").GetComponent<RectTransform>();
        movie = transform.FindChild("Movie").GetComponent<RectTransform>();
        quit = transform.FindChild("Quit").GetComponent<RectTransform>();
        effect_row = transform.FindChild("effect_row").gameObject;
        effect_col = transform.FindChild("effect_col").gameObject;

        text_game = game.transform.FindChild("Text").GetComponent<Text>();
        text_journey = journey.transform.FindChild("Text").GetComponent<Text>();
        text_moive = movie.transform.FindChild("Text").GetComponent<Text>();
        text_quit = quit.transform.FindChild("Text").GetComponent<Text>();

        TimerManager.Add(1, Move);

        EventTriggerAssist.Get(game.gameObject).LeftClick += (e) =>
        {
            if (ShowGameEvent != null)
                ShowGameEvent();
        };
        EventTriggerAssist.Get(journey.gameObject).LeftClick += (e) =>
        {
            if (ShowJourneyEvent != null)
                ShowJourneyEvent();
        };
        EventTriggerAssist.Get(movie.gameObject).LeftClick += (e) =>
        {
            if (ShowMovieEvent != null)
                ShowMovieEvent();
        };
        EventTriggerAssist.Get(quit.gameObject).LeftClick += (e) =>
        {
            if (ShowQuitEvent != null)
                ShowQuitEvent();
        };
    }

    private void Move(int e)
    {
        TimerManager.Add(1f, (x) =>
        {
            effect_row.SetActive(true);

        });
        TimerManager.Add(2f, (x) =>
        {
            effect_col.SetActive(true);

            text_game.gameObject.GetComponent<TypeWordExample>().PlayTypeWordEffect();
            text_journey.gameObject.GetComponent<TypeWordExample>().PlayTypeWordEffect();
            text_moive.gameObject.GetComponent<TypeWordExample>().PlayTypeWordEffect();
            text_quit.gameObject.GetComponent<TypeWordExample>().PlayTypeWordEffect();
        });


        title.Moveto(new Vector3(-360, 336, 0), 0.3f).setEase(LeanTweenType.easeInCirc);
        line1.Moveto(new Vector3(-720, 334, 0), 0.3f).setDelay(0.2f);
        line2.Moveto(new Vector3(-300, 365, 0), 0.3f).setDelay(0.2f);
        line3.Moveto(new Vector3(-300, 317, 0), 0.3f).setDelay(0.2f);
        line1.Alpha(1, 0.3f);
        line2.Alpha(1, 0.3f);
        line3.Alpha(1, 0.3f);

        game.Moveto(new Vector3(-271, 155, 0), 0.3f).setDelay(0.3f).setEase(LeanTweenType.easeInCubic);
        journey.Moveto(new Vector3(-271, 87, 0), 0.3f).setDelay(0.6f).setEase(LeanTweenType.easeInCubic);
        movie.Moveto(new Vector3(-271, 21, 0), 0.3f).setDelay(0.9f).setEase(LeanTweenType.easeInCubic);
        quit.Moveto(new Vector3(-271, -45, 0), 0.3f).setDelay(1.2f).setEase(LeanTweenType.easeInCubic);
        game.Alpha(1, 0.3f).setDelay(0.3f);
        journey.Alpha(1, 0.3f).setDelay(0.6f);
        movie.Alpha(1, 0.3f).setDelay(1.2f).onComplete = () =>
        {
            root.Rotatey(330, 0.3f);
            root.Moveto(new Vector3(-380, 0, 0), 0.3f);
        };
    }
}
