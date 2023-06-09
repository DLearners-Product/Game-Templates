using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SceneManagement;

public class Snail_Word_Game_Main : MonoBehaviour
{
    public static Snail_Word_Game_Main Instance;
    public bool B_production;

    [Header("Screens and UI elements")]
    public GameObject G_Game;
    public GameObject G_Demo;
    bool B_CloseDemo;
    public GameObject G_Transition;
    public GameObject G_coverPage;
    public GameObject G_instructionPage;
    public TextMeshProUGUI TEXM_instruction;
    public Text TEX_points;
    public Text TEX_questionCount;
    public TextMeshProUGUI TM_pointFx;

    [Header("Objects")]
    public GameObject G_BG;
    bool BG_Move;
    public GameObject G_Options;
    public GameObject G_OptionsParent;
    public GameObject G_QuestionParent;
    public GameObject G_QuestionPrefab;
    GameObject G_Selected;
    public GameObject G_Rocket;
    bool B_CanClick, B_Wrong;
    public bool B_Start, B_End;
    public string STR_Word;
    int I_Ccount;
    public List<GameObject> GL_Words;
    public Sprite SPR_Correct, SPR_Selected, SPR_Normal;


    //*****************************************************************************************************************************
    public GameObject G_LeftPanelQuestions;
    public GameObject G_Grid;
    public GameObject G_TilePrefab;
    public GameObject G_DummyTilePrefab;
    public TextMeshProUGUI G_CurrWord;
    public List<TextMeshProUGUI> G_LettersList;


    [Header("Values")]
    public string STR_currentQuestionAnswer;
    public string STR_currentSelectedAnswer;
    public int I_currentQuestionCount; // question number current
    public string STR_currentQuestionID;
    public int I_Points;
    public int I_wrongAnsCount;


    //*****************************************************************************************************************************
    public int rows, cols = 6;
    public List<string> charList;
    public Stack<Word_Tile> currWordStack;
    public string currWord;


    [Header("URL")]
    public string URL;
    public string SendValueURL;

    [Header("Audios")]
    public AudioSource AS_collecting;
    public AudioSource AS_oops;
    public AudioSource AS_crtans;
    public AudioSource AS_Wrong3;

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
            // URL = "http://103.117.180.121:8000/test/Game_template_api-s/game_template_1.php"; // UAT FETCH DATA
            // SendValueURL = "http://103.117.180.121:8000/test/Game_template_api-s/save_child_questions.php"; // UAT SEND DATA

            URL = "http://20.120.84.12/Test/template_and_games/Game_template_api-s/game_template_1.php"; // UAT FETCH DATA
            SendValueURL = "http://20.120.84.12/Test/template_and_games/Game_template_api-s/save_child_questions.php"; // UAT SEND DATA
        }

    }
    void Start()
    {




        //grid generation
        //GenerateGrid();
        charList = new List<string>();
        currWordStack = new Stack<Word_Tile>();
        Invoke("ArrangeWords", 2f);



        G_Game.SetActive(false);
        B_CloseDemo = true;

        G_Transition.SetActive(false);
        G_levelComplete.SetActive(false);

        G_instructionPage.SetActive(false);

        TEX_points.text = I_Points.ToString();
        STRL_questions = new List<string>();
        STRL_answers = new List<string>();
        STRL_options = new List<string>();
        Invoke("THI_gameData", 1f);

        I_currentQuestionCount = -1;

    }
    private void Update()
    {
        if (!G_Demo.activeInHierarchy && B_CloseDemo)
        {
            B_CloseDemo = false;
            DemoOver();
        }

        if (Input.GetMouseButton(0))
        {
            Vector2 worldpoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D Hit = Physics2D.Raycast(worldpoint, Vector2.zero);

            if (Hit.collider != null)
            {
                G_Selected = Hit.collider.gameObject;

                THI_Words();
                G_Selected.GetComponent<Collider2D>().enabled = false;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            THI_End();
        }
        if (BG_Move)
        {
            G_BG.transform.Translate(Vector3.down * 2f * Time.deltaTime);
        }
    }

    public void THI_Words()
    {

        /*G_Selected.GetComponent<Image>().sprite = SPR_Selected;
        string currentletter = G_Selected.name;
        STR_Word = STR_Word + currentletter;
        // Debug.Log("Word = " + STR_Word);
        GL_Words.Add(G_Selected);
        if (STR_Word.Length == 1)
        {
            if (B_Start)
            {
                THI_Start();
            }
        }*/

    }
    public void THI_Start()
    {
        B_Start = false;
        B_End = true;
    }
    public void THI_End()
    {
        if (STR_Word != "")
        {
            B_Wrong = true;
            STR_currentSelectedAnswer = STR_Word;

            STR_Word = "";
            B_End = false;

            for (int i = 0; i < STRL_questions.Count; i++)
            {
                if (STRL_questions[i].Contains(STR_currentSelectedAnswer))
                {
                    STR_currentQuestionID = STRL_questionID[i];
                    STR_currentQuestionAnswer = STRL_answers[i];
                    // Debug.Log(STR_currentQuestionAnswer);
                }
            }

            for (int i = 0; i < STRL_answers.Count; i++)
            {
                if (STRL_answers[i] == STR_currentSelectedAnswer)
                {
                    B_CanClick = false;
                    B_Wrong = false;

                    THI_TrackGameData("1");

                    G_Rocket.GetComponent<Animator>().Play("RocketCorrect");

                    BG_Move = true;

                    G_QuestionParent.transform.GetChild(i).GetComponent<TextMeshProUGUI>().color = Color.white;
                    for (int k = 0; k < GL_Words.Count; k++)
                    {
                        GL_Words[k].GetComponent<Image>().sprite = SPR_Correct;
                    }
                    //  if (I_Ccount < 9) { I_Ccount++; } else { I_Ccount = 0; }
                    I_currentQuestionCount++;

                    TEX_questionCount.text = "" + I_currentQuestionCount + "/" + (STRL_questions.Count);

                    if (I_currentQuestionCount == STRL_questions.Count)
                    {
                        Invoke(nameof(THI_Last), 5f);
                    }

                    GL_Words = new List<GameObject>();
                    B_Start = true;

                    AS_crtans.Play();
                    I_Points += I_correctPoints;
                    TEX_points.text = I_Points.ToString();
                    THI_pointFxOn(true);
                    Invoke(nameof(THI_OffBG), 8f);
                }
                else
                {
                    AS_Wrong3.Play();
                }

            }

            WrongAttempt();
            GL_Words = new List<GameObject>();

        }


        // G_Selected = null;
    }

    void THI_Last()
    {
        G_QuestionParent.transform.parent.gameObject.SetActive(false);
        G_OptionsParent.transform.parent.gameObject.SetActive(false);
        G_Rocket.GetComponent<Animator>().Play("RocketEscape");
        Invoke(nameof(THI_Levelcompleted), 2f);
    }
    void THI_OffBG()
    {
        B_CanClick = true;
        BG_Move = false;
    }

    void WrongAttempt()
    {
        if (B_Wrong)
        {
            for (int k = 0; k < GL_Words.Count; k++)
            {
                GL_Words[k].GetComponent<Collider2D>().enabled = true;
                GL_Words[k].GetComponent<Image>().sprite = SPR_Normal;
            }
            STR_currentQuestionID = STRL_questionID[0];
            THI_TrackGameData("0");
            G_Rocket.GetComponent<Animator>().Play("RocketWrong");
        }

    }


    void THI_gameData()
    {
        // THI_getPreviewData();
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

    public void DemoOver()
    {
        G_Game.SetActive(true);
        THI_Transition();
    }
    void THI_Transition()
    {

        // G_Question.SetActive(false);
        G_Transition.SetActive(true);


        Invoke(nameof(THI_NextQuestion), 2f);
    }




    public void THI_NextQuestion()
    {


        G_Transition.SetActive(false);
        if (I_currentQuestionCount < STRL_questions.Count - 1)
        {

            I_currentQuestionCount++;



            // STR_currentQuestionID = STRL_questionID[I_currentQuestionCount];
            int currentquesCount = I_currentQuestionCount + 1;
            TEX_questionCount.text = currentquesCount + "/" + STRL_questions.Count;
            // STR_currentQuestionAnswer = STRL_answers[I_currentQuestionCount];
            //  G_Question.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = STRL_questions[I_currentQuestionCount];
            //  G_Question.transform.GetChild(0).GetComponent<AudioSource>().clip = ACA__questionClips[I_currentQuestionCount];

            for (int i = 0; i < STRL_questions.Count; i++)
            {
                GameObject Q_Dummy = Instantiate(G_QuestionPrefab);
                Q_Dummy.GetComponent<TextMeshProUGUI>().text = STRL_questions[i];
                Q_Dummy.GetComponent<AudioSource>().clip = ACA__questionClips[i];
                Q_Dummy.transform.SetParent(G_QuestionParent.transform, false);
            }


            if (IL_numbers[3] == 1)
            {
                for (int i = 0; i < STRL_options.Count; i++)
                {
                    string[] dummy = STRL_options[i].Split(',');
                    for (int j = 0; j < dummy.Length; j++)
                    {
                        GameObject G_Opt = Instantiate(G_Options);
                        G_Opt.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = dummy[j];
                        G_Opt.name = dummy[j];
                        G_Opt.transform.SetParent(G_OptionsParent.transform, false);
                    }
                    dummy = new string[0];
                }
            }
            else
            {
                for (int i = 0; i < STRL_options.Count; i++)
                {
                    GameObject G_Opt = Instantiate(G_Options);
                    G_Opt.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = STRL_options[i];
                    G_Opt.name = STRL_options[i];
                    G_Opt.transform.SetParent(G_OptionsParent.transform, false);
                }
            }

            B_CanClick = true;


            I_wrongAnsCount = 0;
        }
        else
        {
            THI_Levelcompleted();
            // Invoke(nameof(THI_Levelcompleted), 3f);
        }
    }



    void THI_Levelcompleted()
    {
        MainController.instance.I_TotalPoints = I_Points;
        G_levelComplete.SetActive(true);
        StartCoroutine(IN_sendDataToDB());
    }






    void THI_WrongEffect()
    {
        if (I_wrongAnsCount == 3)
        {

            if (STR_difficulty == "assistive")
            {


                //Show answer and move to next question
            }
            if (STR_difficulty == "intuitive")
            {

                //Show answer and after click next question
            }

        }
        else
        if (I_wrongAnsCount == 2)
        {
            if (STR_difficulty == "independent")
            {

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

    public int GetWordsCharacterCount()
    {
        int charCount = 0;

        for (int i = 0; i < G_LeftPanelQuestions.transform.childCount; i++)
        {
            foreach (char c in G_LeftPanelQuestions.transform.GetChild(i).GetComponent<TextMeshProUGUI>().text)
            {
                charCount++;
                string temp = c.ToString();
                charList.Add(temp);
            }
        }

        return charCount;
    }

    public void GenerateGrid()
    {
        int totalCharCount = GetWordsCharacterCount();

        //if (totalCharCount < 35)
        //{
        //    cols = 5;
        //}

        int count = 0;
        for (int i = 0; i < cols; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                GameObject tile;
                //Debug.Log(" i = " + i + " | j = " + j + " | count = " + count + " | charList(" + count + ") = " + charList[count]);
                if (count > totalCharCount - 1)
                {
                    tile = Instantiate(G_DummyTilePrefab);
                }
                else
                {
                    tile = Instantiate(G_TilePrefab);
                    tile.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = charList[count];
                    count++;
                }

                tile.transform.SetParent(G_Grid.transform, false);
                tile.transform.position = new Vector3(i * 1.25f, -j * 1.25f, 0f);

            }

        }

    }

    public void OnClickTile(GameObject gameObject)
    {
        /*if (isPressed)
        {
            currWordStack.Push(letter);
            currWord += letter;
        }
        else
        {
            currWordStack.Pop();
            currWord = currWord.Remove(currWord.Length - 1, 1);
        }

        G_CurrWord.text = currWord;*/




        //Word_Tile go = gameObject;
        //if (go.isPressed == false)
        //{
        //    currWordStack.Push(go.letter);
        //    go.isPressed = true;
        //    currWord += go.letter;
        //}
        //else if (currWordStack.Peek() == go.letter)
        //{
        //    currWordStack.Pop();
        //    go.isPressed = false;
        //    currWord = currWord.Remove(currWord.Length - 1, 1);
        //}


    }

    public void AddToStack(Word_Tile tile)
    {
        currWordStack.Push(tile);

        //form the word
        currWord += tile.letter;
        UpdateWord();
    }

    public void RemoveFromStack()
    {
        currWordStack.Pop();
        currWord = currWord.Remove(currWord.Length - 1, 1);
        UpdateWord();
    }

    public void UpdateWord()
    {
        G_CurrWord.text = currWord;
        WordFormationCheck();
    }

    public void WordFormationCheck()
    {
        //Debug.Log(currWord);
        if (currWord == "stop" || currWord == "take" || currWord == "them" || currWord == "then" || currWord == "thank" || currWord == "always"
            || currWord == "around" || currWord == "because" || currWord == "been" || currWord == "before")      //STRL_questions.Contains(currWord)
        {
            while (currWordStack.Count > 0)
            {
                Word_Tile tile = currWordStack.Pop();
                tile.gameObject.SetActive(false);

            }

            currWord = "";
            G_CurrWord.text = currWord;

        }

    }

    public void ArrangeWords()
    {
        Debug.Log("arranging the words");


        for (int i = 0; i < G_LettersList.Count; i++)
        {
            if (i > charList.Count - 1)
            {

            }
            else
            {
                G_LettersList[i].text = charList[i];
            }





        }
    }

}