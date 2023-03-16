using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SceneManagement;

public class Racing_Main : MonoBehaviour
{
    public static Racing_Main Instance;
    public bool B_production;

    [Header("Screens and UI elements")]
    public GameObject G_Game;
    public GameObject G_Demo;
    public GameObject G_Transition;
    public GameObject G_coverPage;
    public GameObject G_instructionPage;
    public TextMeshProUGUI TEXM_instruction;
    public Text TEX_points;
    public Text TEX_questionCount;
    public TextMeshProUGUI TM_pointFx;
    bool B_CloseDemo;

    [Header("Objects")]
    public GameObject G_Question;
    public GameObject G_Options;
    public GameObject[] GA_Options;
    GameObject G_Highlight;
    float timeCount;
    bool B_CanClick, B_Race_Completed;
    public GameObject G_Player, G_Enemy;
    public ParticleSystem[] P_Win;


    [Header("Values")]
    public string STR_currentQuestionAnswer;
    public string STR_currentSelectedAnswer;
    public int I_currentQuestionCount; // question number current
    public string STR_currentQuestionID;
    public int I_Points;
    public int I_wrongAnsCount;
    public int I_Counter, I_Dummmy;
    public string[] STRA_AnsList;
  //  public int I_GoalTry;
    /*public float F_Timer;
    bool B_CanCount, B_Callonce;*/
    public int I_Player, I_Enemy;

    [Header("URL")]
    public string URL;
    public string SendValueURL;

    [Header("Audios")]
    public AudioSource AS_collecting;
    public AudioSource AS_oops;
    public AudioSource AS_crtans;
    public AudioSource AS_Wrong3;
    public AudioSource AS_Horn;

    [Header("DB")]
    public List<string> STRL_difficulty;
    public string STR_difficulty;
    public List<int> IL_numbers;
    public int I_correctPoints;
    public int I_wrongPoints;
    public List<string> STRL_instruction;
    public string STR_instruction;
    public string STR_video_link;
    public List<string> STRL_options;
    public List<string> STRL_questions;
    public List<string> STRL_answers;
    public List<string> STRL_quesitonAudios;
    public List<string> STRL_optionAudios;
    public List<string> STRL_instructionAudio;
    public List<string> STRL_questionID;
    public string STR_customizationKey;
    //Dummy values only for helicopter game
    public List<string> STRL_BG_img_link;
    public List<string> STRL_avatar_Color;
    public List<string> STRL_Panel_Img_link;
    public List<string> STRL_cover_img_link;

    [Header("GAME DATA")]
    public List<string> STRL_gameData;
    public string STR_Data;

    [Header("LEVEL COMPLETE")]
    public GameObject G_levelComplete;

    [Header("AUDIO ASSIGN")]
    public AudioClip[] ACA__questionClips;
    public AudioClip[] ACA_optionClips;
    public AudioClip[] ACA_instructionClips;
    // Start is called before the first frame update
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;

        if (B_production)
        {
            URL = "https://dlearners.in/template_and_games/Game_template_api-s/game_template_1.php"; // PRODUCTION FETCH DATA
            SendValueURL = "https://dlearners.in/template_and_games/Game_template_api-s/save_child_questions.php"; // PRODUCTION SEND DATA

        }
        else
        {
             URL = "http://103.117.180.121:8000/test/Game_template_api-s/game_template_1.php"; // UAT FETCH DATA
             SendValueURL = "http://103.117.180.121:8000/test/Game_template_api-s/save_child_questions.php"; // UAT SEND DATA

           // URL = "http://20.120.84.12/Test/template_and_games/Game_template_api-s/game_template_1.php"; // UAT FETCH DATA
           //  SendValueURL = "http://20.120.84.12/Test/template_and_games/Game_template_api-s/save_child_questions.php"; // UAT SEND DATA
        }

    }
    void Start()
    {
        B_CloseDemo = true;
        G_Game.SetActive(false);
        G_Transition.SetActive(false);
        G_levelComplete.SetActive(false);
        B_Race_Completed = false;
        G_instructionPage.SetActive(false);

        TEX_points.text = I_Points.ToString();
        STRL_questions = new List<string>();
        STRL_answers = new List<string>();
        STRL_options = new List<string>();
        Invoke("THI_gameData", 1f);
      //  B_CanCount = false;
        I_currentQuestionCount = -1;
        I_Dummmy = 0;
        I_Counter = 0;
    }
   
    

    public void THI_Check()
    {
        if (B_CanClick)
        {
            // GameObject G_Selected = ;
            STR_currentSelectedAnswer = EventSystem.current.currentSelectedGameObject.name;


            if (STR_currentSelectedAnswer == STR_currentQuestionAnswer)
            {
                B_CanClick = false;
               
                THI_Correct();
            }
            else { THI_Wrong(); }
        }

    }
    void THI_gameData()
    {
        //  THI_getPreviewData();
        if (MainController.instance.mode == "live")
        {
            StartCoroutine(EN_getValues()); // live game in portal
        }
        if (MainController.instance.mode == "preview")
        {
            // preview data in html game generator

            Debug.Log("PREVIEW MODE RAKESH");
            THI_getPreviewData();
        }
    }
    private void Update()
    {
        if (!G_Demo.activeInHierarchy && B_CloseDemo)
        {
            B_CloseDemo = false;
            DemoOver();
        }

       /* if(B_CanCount)
        {
            F_Timer = F_Timer + 1 * Time.deltaTime;
            if ((int)F_Timer % 10 == 0)
            {
                if(B_Callonce)
                {
                    B_Callonce = false;
                    Debug.Log("Time came = " + F_Timer);
                    THI_QueAudio();
                }
               
            }
            else
            {
                B_Callonce = true;
            }
        }*/
        
       
    }

    public void DemoOver()
    {
        G_Game.SetActive(true);
      //  B_CanCount = true;
        THI_Transition();
    }
    void THI_Transition()
    {

        G_Question.SetActive(false);
        G_Transition.SetActive(true);
       

        Invoke(nameof(THI_NextQuestion), 2f);
    }

    void THI_QueAudio()
    {
        Debug.Log("Play Q audio");
        G_Question.transform.GetChild(0).GetComponent<AudioSource>().Play();
    }
    public void THI_ShowQuestion()
    {
        B_CanClick = true;
        G_Question.SetActive(true);
        THI_QueAudio();

        if (I_currentQuestionCount == 0)
        {
           //G_Player.GetComponent<WayPoint_Follow>().F_speed = 1f;
           // G_Enemy.GetComponent<WayPoint_Follow>().F_speed = 1f;

            G_Player.GetComponent<WayPoint_Follow>().enabled = true;
            G_Enemy.GetComponent<WayPoint_Follow>().enabled = true;
        }
    }


    public void THI_NextQuestion()
    {
        G_Transition.SetActive(false);
        G_Player.GetComponent<WayPoint_Follow>().B_GameStarted = true;
        G_Enemy.GetComponent<WayPoint_Follow>().B_GameStarted = true;
        if (I_currentQuestionCount < STRL_questions.Count - 1)
        {
           
            I_currentQuestionCount++;


            STRA_AnsList = null;
            STR_currentQuestionID = STRL_questionID[I_currentQuestionCount];
            int currentquesCount = I_currentQuestionCount + 1;
            TEX_questionCount.text = currentquesCount + "/" + STRL_questions.Count;
            STR_currentQuestionAnswer = STRL_answers[I_currentQuestionCount];
            G_Question.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = STRL_questions[I_currentQuestionCount];
            G_Question.transform.GetChild(0).GetComponent<AudioSource>().clip = ACA__questionClips[I_currentQuestionCount];

            I_Dummmy = I_Counter + IL_numbers[3];

            for (int i = 0; i < G_Options.transform.childCount; i++)
            {
                G_Options.transform.GetChild(i).name = STRL_options[i + I_Counter];
                G_Options.transform.GetChild(i).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = STRL_options[i + I_Counter];
                G_Options.transform.GetChild(i).transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
                G_Options.transform.GetChild(i).GetComponent<AudioSource>().clip = ACA_optionClips[i + I_Counter];
            }
            StopCoroutine(Highlight());
            THI_ShowQuestion();
            

            I_Counter = I_Counter + IL_numbers[3];

            I_wrongAnsCount = 0;
            
        }
        else
        {
            if(B_Race_Completed)
            {
                THI_Levelcompleted();
            }
           // 
            // Invoke(nameof(THI_Levelcompleted), 3f);
        }
    }



    void THI_Levelcompleted()
    {
        G_Player.GetComponent<AudioSource>().Stop();
        G_Enemy.GetComponent<AudioSource>().Stop();
        MainController.instance.I_TotalPoints = I_Points;
        G_levelComplete.SetActive(true);
        StartCoroutine(IN_sendDataToDB());
    }


    public void THI_Correct()
    {
        AS_crtans.Play();
        I_Points += I_correctPoints;
        TEX_points.text = I_Points.ToString();
        THI_pointFxOn(true);
        G_Player.GetComponent<WayPoint_Follow>().F_speed = 2f;
        
        if(I_Player==I_Enemy)
        {
            G_Enemy.GetComponent<WayPoint_Follow>().F_speed -= 0.1f;
        }
       
        G_Player.transform.GetChild(1).transform.GetChild(0).GetComponent<TrailRenderer>().startColor = Color.cyan;
        G_Player.transform.GetChild(1).transform.GetChild(0).GetComponent<TrailRenderer>().enabled = true;
        Invoke(nameof(ReduceSpeed), 3f);

        // Release bird animation
        THI_TrackGameData("1");
        if (I_currentQuestionCount < STRL_questions.Count - 1)
        {
            Invoke(nameof(ReduceSpeed), 3f);
            THI_NextQuestion();
        }
        else
        {
            G_Question.SetActive(false);
            G_Player.transform.GetChild(1).GetComponent<Bike_Rotate>().B_FinishRace = true;
            G_Enemy.transform.GetChild(1).GetComponent<Bike_Rotate>().B_FinishRace = true;

            if (!B_Race_Completed)
            {
                G_Player.GetComponent<WayPoint_Follow>().F_speed = 3f;
                G_Enemy.GetComponent<WayPoint_Follow>().F_speed = 1f;
            }
        }
           


    }

    void ReduceSpeed()
    {
        if (I_currentQuestionCount < STRL_questions.Count - 1)
        {
            G_Player.GetComponent<WayPoint_Follow>().F_speed = 1.5f;
        }
        G_Player.transform.GetChild(1).transform.GetChild(0).GetComponent<TrailRenderer>().enabled = false;
    }

    public void Race_Complete(string name)
    {
        B_Race_Completed = true;
       

        if (name == "Player" && I_Enemy < I_Player)
        {
            AS_Horn.Play();
            for (int i = 0; i < P_Win.Length; i++)
            {
                P_Win[i].Play();
            }
            
            Debug.Log("Winner");
            G_Enemy.GetComponent<WayPoint_Follow>().F_speed = 0;
            G_Player.GetComponent<WayPoint_Follow>().F_speed = 0;
            Invoke(nameof(THI_Levelcompleted), 3f);
        }
        else
        if(name == "Enemy" && I_Enemy > I_Player && I_Player != I_Enemy)
        {
            AS_Wrong3.Play();
            Debug.Log("You lost the race");
            G_Enemy.GetComponent<WayPoint_Follow>().F_speed = 0;
            G_Player.GetComponent<WayPoint_Follow>().F_speed = 0;
            Invoke(nameof(THI_Levelcompleted), 3f);
        }

      //  Invoke(nameof(THI_Levelcompleted), 3f);
    }

    IEnumerator Highlight()
    {
        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(1f);
            G_Highlight.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.green;
            yield return new WaitForSeconds(1f);
            G_Highlight.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.black;
            yield return new WaitForSeconds(1f);
            G_Highlight.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
        }
    }

    void THI_WrongEffect()
    {
        if (I_wrongAnsCount == 3)
        {

            if (STR_difficulty == "assistive")
            {
                for (int i = 0; i < G_Options.transform.childCount; i++)
                {
                    if (G_Options.transform.GetChild(i).name == STR_currentQuestionAnswer)
                    {
                        G_Highlight = G_Options.transform.GetChild(i).gameObject;
                        StartCoroutine(Highlight());
                    }
                }


                Invoke(nameof(THI_NextQuestion), 5f);
                //Show answer and move to next question
            }
            if (STR_difficulty == "intuitive")
            {
                for (int i = 0; i < G_Options.transform.childCount; i++)
                {

                    if (G_Options.transform.GetChild(i).name == STR_currentQuestionAnswer)
                    {
                        G_Highlight = G_Options.transform.GetChild(i).gameObject;
                        StartCoroutine(Highlight());
                    }
                }


                //Show answer and after click next question
            }

        }
        else
        if (I_wrongAnsCount == 2)
        {
            if (STR_difficulty == "independent")
            {
                Invoke(nameof(THI_NextQuestion), 2f);
            }
            //next question
        }

        //  B_Fishspawn = true;
        // StartCoroutine(SpawnFish());
        // STR_currentSelectedAnswer = "";
        // B_Correct = false;
    }
    public void THI_Collect_Out(bool plus)
    {

        if (plus)
        {
            AS_collecting.Play();
            TM_pointFx.text = "+" + 2 + " points";
            I_Points += 2;
        }
        else
        {
            AS_oops.Play();
            if (I_Points > 10)
            {
                TM_pointFx.text = "-" + 10 + " point";
                I_Points -= 10;
            }
            else
            {
                if (I_Points > 0)
                {
                    I_Points = 0;
                }
            }
        }
        TEX_points.text = I_Points.ToString();
        Invoke("THI_pointFxOff", 1f);
    }
    public void THI_Wrong()
    {
        Debug.Log("Wrong ans");

        AS_oops.Play();
        THI_pointFxOn(false);
        THI_TrackGameData("0");
        I_wrongAnsCount++;


        if (I_wrongAnsCount == 5)
        {
            Debug.Log("Restart or use coins");
        }
        //REDO the same question

        // wrong bird animation
        THI_WrongEffect();

        if (I_Points > I_wrongPoints)
        {
            I_Points -= I_wrongPoints;
        }
        else
        {
            if (I_Points > 0)
            {
                I_Points = 0;
            }
        }
        TEX_points.text = I_Points.ToString();
    }
    public void THI_pointFxOn(bool plus)
    {
        if (plus)
        {
            if (I_correctPoints != 1)
            {
                TM_pointFx.text = "+" + I_correctPoints + " points";
            }
            else
            {
                TM_pointFx.text = "+" + I_correctPoints + " point";
            }
        }
        else
        {
            if (I_Points > 0)
            {
                if (I_wrongPoints != 0)
                {
                    if (I_wrongPoints != 1)
                    {
                        TM_pointFx.text = "-" + I_wrongPoints + " points";
                    }
                    else
                    {
                        TM_pointFx.text = "-" + I_wrongPoints + " point";
                    }
                }
            }
        }
        Invoke("THI_pointFxOff", 1f);
    }
    public void THI_pointFxOff()
    {
        TM_pointFx.text = "";
    }
    public IEnumerator IN_CoverImage()
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(STRL_cover_img_link[0]);
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture2D downloadedTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            if (STRL_cover_img_link != null)
            {
                G_coverPage.GetComponent<Image>().sprite = Sprite.Create(downloadedTexture, new Rect(0.0f, 0.0f, downloadedTexture.width, downloadedTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
            }
        }

        //SPRA_Options

    }

    public IEnumerator EN_getValues()
    {
        WWWForm form = new WWWForm();
        form.AddField("game_id", MainController.instance.STR_GameID);
        // Debug.Log("GAME ID : " + MainController.instance.STR_GameID);
        UnityWebRequest www = UnityWebRequest.Post(URL, form);
        yield return www.SendWebRequest();
        if (www.isHttpError || www.isNetworkError)
        {
            Debug.Log(www.error);
        }
        else
        {
            List<string> STRL_Passagedetails = new List<string>();
            MyJSON json = new MyJSON();
            //json.Helitemp(www.downloadHandler.text);
            json.Temp_type_2(www.downloadHandler.text, STRL_difficulty, IL_numbers, STRL_questions, STRL_answers, STRL_options, STRL_questionID, STRL_instruction, STRL_quesitonAudios, STRL_optionAudios,
            STRL_instructionAudio, STRL_cover_img_link, STRL_Passagedetails);
            //        Debug.Log("GAME DATA : " + www.downloadHandler.text);

            STR_difficulty = STRL_difficulty[0];

            STR_instruction = STRL_instruction[0];
            MainController.instance.I_correctPoints = I_correctPoints = IL_numbers[1];
            I_wrongPoints = IL_numbers[2];
            MainController.instance.I_TotalQuestions = STRL_questions.Count;

            for (int i = 0; i < GA_Options.Length; i++)
            {
                GA_Options[i].SetActive(false);
            }
            if (IL_numbers[3] == 2)
            {
                G_Options = GA_Options[0];
            }
            if (IL_numbers[3] == 3)
            {
                G_Options = GA_Options[1];
            }
           
            G_Options.SetActive(true);

            StartCoroutine(EN_getAudioClips());
            StartCoroutine(IN_CoverImage());

        }
    }
    public IEnumerator EN_getAudioClips()
    {
        ACA__questionClips = new AudioClip[STRL_quesitonAudios.Count];
        ACA_optionClips = new AudioClip[STRL_optionAudios.Count];
        ACA_instructionClips = new AudioClip[STRL_instructionAudio.Count];

        for (int i = 0; i < STRL_quesitonAudios.Count; i++)
        {
            UnityWebRequest www1 = UnityWebRequestMultimedia.GetAudioClip(STRL_quesitonAudios[i], AudioType.MPEG);
            yield return www1.SendWebRequest();
            if (www1.result == UnityWebRequest.Result.ConnectionError || www1.isHttpError || www1.isNetworkError)
            {
                Debug.Log(www1.error);
            }
            else
            {
                ACA__questionClips[i] = DownloadHandlerAudioClip.GetContent(www1);
            }
        }

        for (int i = 0; i < STRL_optionAudios.Count; i++)
        {
            UnityWebRequest www2 = UnityWebRequestMultimedia.GetAudioClip(STRL_optionAudios[i], AudioType.MPEG);
            yield return www2.SendWebRequest();
            if (www2.result == UnityWebRequest.Result.ConnectionError || www2.isHttpError || www2.isNetworkError)
            {
                Debug.Log(www2.error);
            }
            else
            {
                ACA_optionClips[i] = DownloadHandlerAudioClip.GetContent(www2);
            }
        }


        for (int i = 0; i < STRL_instructionAudio.Count; i++)
        {
            UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(STRL_instructionAudio[i], AudioType.MPEG);
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.ConnectionError || www.isHttpError || www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {

                ACA_instructionClips[i] = DownloadHandlerAudioClip.GetContent(www);
                Debug.Log("audio clips fetched instruction");

            }
        }
        THI_assignAudioClips();
    }

    void THI_assignAudioClips()
    {
        if (ACA_instructionClips.Length > 0)
        {
            TEXM_instruction.gameObject.AddComponent<AudioSource>();
            TEXM_instruction.gameObject.GetComponent<AudioSource>().playOnAwake = false;
            TEXM_instruction.gameObject.GetComponent<AudioSource>().clip = ACA_instructionClips[0];
            TEXM_instruction.gameObject.AddComponent<Button>();
            TEXM_instruction.gameObject.GetComponent<Button>().onClick.AddListener(THI_playAudio);
        }

        // DemoOver();//remove later
        // THI_Transition();
    }
    void THI_playAudio()
    {
        EventSystem.current.currentSelectedGameObject.GetComponent<AudioSource>().Play();
        Debug.Log("player clicked. so playing audio");
    }
    public void THI_getPreviewData()
    {
        List<string> STRL_Passagedetails = new List<string>();
        MyJSON json = new MyJSON();
        //  json.Helitemp(MainController.instance.STR_previewJsonAPI);
        json.Temp_type_2(MainController.instance.STR_previewJsonAPI, STRL_difficulty, IL_numbers, STRL_questions, STRL_answers, STRL_options, STRL_questionID, STRL_instruction, STRL_quesitonAudios, STRL_optionAudios,
            STRL_instructionAudio, STRL_cover_img_link, STRL_Passagedetails);

        STR_difficulty = STRL_difficulty[0];
        STR_instruction = STRL_instruction[0];
        MainController.instance.I_correctPoints = I_correctPoints = IL_numbers[1];
        I_wrongPoints = IL_numbers[2];
        MainController.instance.I_TotalQuestions = STRL_questions.Count;

        for (int i = 0; i < GA_Options.Length; i++)
        {
            GA_Options[i].SetActive(false);
        }
        if (IL_numbers[3] == 2)
        {
            G_Options = GA_Options[0];
        }
        if (IL_numbers[3] == 3)
        {
            G_Options = GA_Options[1];
        }
        if (IL_numbers[3] == 4)
        {
            G_Options = GA_Options[2];
        }
        if (IL_numbers[3] == 5)
        {
            G_Options = GA_Options[3];
        }
        G_Options.SetActive(true);
        StartCoroutine(EN_getAudioClips());
        StartCoroutine(IN_CoverImage());


        // THI_createOptions();
    }
    public void THI_TrackGameData(string analysis)
    {
        DBmanager TrainSortingDB = new DBmanager();
        TrainSortingDB.question_id = STR_currentQuestionID;
        TrainSortingDB.answer = STR_currentSelectedAnswer;
        TrainSortingDB.analysis = analysis;
        string toJson = JsonUtility.ToJson(TrainSortingDB);
        STRL_gameData.Add(toJson);
        STR_Data = string.Join(",", STRL_gameData);
    }

    public IEnumerator IN_sendDataToDB()
    {
        WWWForm form = new WWWForm();
        form.AddField("child_id", MainController.instance.STR_childID);
        form.AddField("game_id", MainController.instance.STR_GameID);
        form.AddField("game_details", "[" + STR_Data + "]");


        Debug.Log("child id : " + MainController.instance.STR_childID);
        Debug.Log("game_id  : " + MainController.instance.STR_GameID);
        Debug.Log("game_details: " + "[" + STR_Data + "]");

        UnityWebRequest www = UnityWebRequest.Post(SendValueURL, form);
        yield return www.SendWebRequest();
        if (www.isHttpError || www.isNetworkError)
        {
            Debug.Log("Sending data to DB failed : " + www.error);
        }
        else
        {
            MyJSON json = new MyJSON();
            json.THI_onGameComplete(www.downloadHandler.text);

            Debug.Log("Sending data to DB success : " + www.downloadHandler.text);
        }
    }
    public void BUT_playAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BUT_instructionPage()
    {
        StopAllCoroutines();
        Time.timeScale = 0;
        G_instructionPage.SetActive(true);
        TEXM_instruction.text = STR_instruction;
        TEXM_instruction.gameObject.AddComponent<AudioSource>().Play();
    }

    public void BUT_closeInstruction()
    {
        Time.timeScale = 1;
        G_instructionPage.SetActive(false);
    }
}
