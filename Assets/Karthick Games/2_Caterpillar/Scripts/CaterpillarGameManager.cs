using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using SimpleJSON;

namespace CaterpillarSortingGame
{

    public class CaterpillarGameManager : MonoBehaviour
    {

        [Header("==========Integration==========")]


        #region ---------------------------------------integration---------------------------------------


        public static CaterpillarGameManager Instance;
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
        bool B_CanClick, B_Wrong;
        public bool B_Start, B_End;
        public string STR_Word;
        int I_Ccount;
        public List<GameObject> GL_Words;
        public Sprite SPR_Correct, SPR_Selected, SPR_Normal;


        //*****************************************************************************************************************************
        public GameObject G_LeftPanelQuestions;

        [Header("Values")]
        public string STR_currentQuestionAnswer;
        public string STR_currentSelectedAnswer;
        public int I_currentQuestionCount; // question number current
        public string STR_currentQuestionID;
        public int I_Points;
        public int I_wrongAnsCount;

        //*****************************************************************************************************************************


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


        public List<string> STRL_Passagedetails = new List<string>();
        public string STR_Mode;
        public int I_AudioClipIndex;

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

                Debug.Log("PREVIEW MODE");
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



                STR_currentQuestionID = STRL_questionID[I_currentQuestionCount];
                int currentquesCount = I_currentQuestionCount + 1;
                TEX_questionCount.text = currentquesCount + "/" + STRL_questions.Count;
                STR_currentQuestionAnswer = STRL_answers[I_currentQuestionCount];
                //   G_Question.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = STRL_questions[I_currentQuestionCount];
                //   G_Question.transform.GetChild(0).GetComponent<AudioSource>().clip = ACA__questionClips[I_currentQuestionCount];

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
            MainController.instance.I_TotalPoints = I_CollectedPoints;
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

                MyJSON json = new MyJSON();
                //json.Helitemp(www.downloadHandler.text);
                json.Temp_type_2(www.downloadHandler.text, STRL_difficulty, IL_numbers, STRL_questions, STRL_answers, STRL_options, STRL_questionID, STRL_instruction, STRL_quesitonAudios, STRL_optionAudios,
                STRL_instructionAudio, STRL_cover_img_link, STRL_Passagedetails);
                //        Debug.Log("GAME DATA : " + www.downloadHandler.text);

                JSONNode parseJSON = JSON.Parse(www.downloadHandler.text);

                Debug.Log("999999999999999999999999999999999999999999999999999");
                Debug.Log(parseJSON["additional_keys"]);
                Debug.Log(parseJSON["additional_keys"]["Sorting mode"]);
                Debug.Log(parseJSON["additional_keys"]["Sorting mode"]["content_data"]);

                STR_Mode = parseJSON["additional_keys"]["Sorting mode"]["content_data"];
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

            // STR_Mode = STRL_Passagedetails[];
            STR_difficulty = STRL_difficulty[0];
            STR_instruction = STRL_instruction[0];
            MainController.instance.I_correctPoints = I_correctPoints = IL_numbers[1];
            I_wrongPoints = IL_numbers[2];
            MainController.instance.I_TotalQuestions = STRL_questions.Count;

            //manually parsing the additional key "Sorting mode"
            JSONNode parseJSON = JSON.Parse(MainController.instance.STR_previewJsonAPI);

            /*             Debug.Log("999999999999999999999999999999999999999999999999999");
                        Debug.Log(parseJSON["additional_keys"]);
                        Debug.Log(parseJSON["additional_keys"]["Sorting mode"]);
                        Debug.Log(parseJSON["additional_keys"]["Sorting mode"]["content_data"]); */

            STR_Mode = parseJSON["additional_keys"]["Sorting mode"]["content_data"];


            StartCoroutine(EN_getAudioClips());
            StartCoroutine(IN_CoverImage());


            // THI_createOptions();
        }


        public void THI_TrackGameData(string analysis)
        {
            DBmanager CaterpillarGameDB = new DBmanager();
            CaterpillarGameDB.question_id = STR_currentQuestionID;
            CaterpillarGameDB.answer = STR_currentSelectedAnswer;
            CaterpillarGameDB.analysis = analysis;
            string toJson = JsonUtility.ToJson(CaterpillarGameDB);
            STRL_gameData.Add(toJson);
            STR_Data = string.Join(",", STRL_gameData);

            Debug.Log("strl_gamedata = " + toJson);
            Debug.Log("str_data = " + STR_Data);
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
            // Time.timeScale = 0;
            G_instructionPage.SetActive(true);

            TEXM_instruction.text = STR_instruction;
            TEXM_instruction.gameObject.AddComponent<AudioSource>().Play();
        }

        public void BUT_closeInstruction()
        {
            // Time.timeScale = 1;
            G_instructionPage.SetActive(false);

        }

        //!end of integration
        //################################################################
        #endregion



        [Space(10)]

        [Header("==========Game==========")]


        #region  ---------------------------------------unity reference variables---------------------------------------

        [SerializeField] private GameObject G_Leaf;
        [SerializeField] private GameObject G_QandAPrefab;
        [SerializeField] private GameObject G_TransparentScreen;
        [SerializeField] private GameObject G_AscInstruction;
        [SerializeField] private GameObject G_DescInstruction;


        [SerializeField] private Transform T_QandAParent;


        [HideInInspector] public int I_FirstIndex = 0;
        [HideInInspector] public int I_LastIndex = 5;


        private GameObject _InstantiatedQandA;


        //!end of unity reference variables
        //################################################################
        #endregion



        #region  ---------------------------------------local variables---------------------------------------


        [HideInInspector] public int I_CurrentIndex;
        private int I_CollectedPoints = 0;


        //!end of local variables
        //################################################################
        #endregion



        #region =======================================gameplay logic=======================================


        void Start()
        {

            #region =======================================integration=======================================

            // G_Game.SetActive(false);
            B_CloseDemo = true;

            G_Transition.SetActive(false);
            G_levelComplete.SetActive(false);
            G_instructionPage.SetActive(false);

            TEX_points.text = I_Points.ToString();

            Invoke("THI_gameData", 1f);

            I_currentQuestionCount = -1;

            #endregion

            I_CurrentIndex = -1;
            I_AudioClipIndex = 0;
        }


        public void GameInit()
        {
            StartCoroutine(IENUM_GameInit());
        }


        IEnumerator IENUM_GameInit()
        {
            G_Leaf.SetActive(true);

            yield return new WaitForSeconds(3f);

            ShowNextQuestion();
        }


        public void ShowNextQuestion()
        {
            STR_currentSelectedAnswer = "";

            I_CurrentIndex++;

            if (I_CurrentIndex == STRL_questions.Count)
            {
                THI_Levelcompleted();
                return;
            }



            STR_currentQuestionID = STRL_questionID[I_CurrentIndex];
            int currentquesCount = I_CurrentIndex + 1;
            TEX_questionCount.text = I_CurrentIndex + "/" + STRL_questions.Count;
            STR_currentQuestionAnswer = STRL_answers[I_CurrentIndex];

            _InstantiatedQandA = Instantiate(G_QandAPrefab, G_QandAPrefab.transform.position, Quaternion.identity, T_QandAParent);
        }


        public void RemoveCurrentQuestion()
        {
            AudioManager.Instance.PlayGameMusic();
            Destroy(_InstantiatedQandA.gameObject);
            ShowNextQuestion();
        }


        public void IncrementPoints()
        {
            I_Points++;
        }


        public void DecrementPoints()
        {
            if (I_Points > 0)
            {
                I_Points--;
            }

        }


        public void UpdateScore(float delay)
        {
            THI_TrackGameData("1");

            I_Points = I_Points * I_correctPoints;
            StartCoroutine(IENUM_UpdateScoreCount(delay));
        }


        public IEnumerator IENUM_UpdateScoreCount(float delay)
        {
            yield return new WaitForSeconds(delay);

            AudioManager.Instance.PlayCoinCollect(0f);

            for (int i = I_CollectedPoints; i <= (I_CollectedPoints + I_Points); i++)
            {
                TEX_points.text = i.ToString();
                yield return new WaitForSeconds(0.35f);
            }

            I_CollectedPoints += I_Points;
            I_Points = 0;
        }



        //!end of gameplay logic
        //################################################################
        #endregion



    }





}
