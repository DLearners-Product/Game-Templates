using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;


public class SnailGameManager : MonoBehaviour
{

    public string activityName;


    [Header("==========Integration variables==========")]

    #region ---------------------------------------integration---------------------------------------


    public static SnailGameManager Instance;
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

    #endregion

    [Header("==========Game variables==========")]


    #region  ---------------------------------------user input ---------------------------------------
    public Color32 CLR_ButtonNormal;
    public Color32 CLR_ButtonCorrect;
    public Color32 CLR_ButtonWrong;

    #endregion


    #region  ---------------------------------------unity reference variables---------------------------------------

    [Space(10)]

    [SerializeField] private Texture2D TEX_Crosshair;

    [Space(10)]

    [SerializeField] private Image IMG_ButtonBG;

    [Space(10)]

    [SerializeField] private TextMeshProUGUI[] TXTA_Words;
    [SerializeField] private TextMeshProUGUI TXT_Word;

    [Space(10)]

    [SerializeField] private Animator[] ANIM_ToastMessages;
    [SerializeField] private Animator ANIM_ScoreCard;

    [Space(10)]

    [SerializeField] private GameObject cellPrefab; // Prefab for individual grid cells
    [SerializeField] private GameObject[] GA_GridBGCategory;
    [SerializeField] private GameObject G_TransparentScreen;
    [SerializeField] private GameObject G_Scroll;
    [SerializeField] private GameObject G_WinWindow;
    [SerializeField] private GameObject G_Coin;
    [SerializeField] private GameObject G_WordListParticleEffect;
    [SerializeField] private GameObject G_AudioPanel;


    [Space(10)]

    [SerializeField] private Transform gridParent;
    [SerializeField] private Transform[] TA_New_GridCategory;

    [Space(10)]


    [SerializeField] private ParticleSystem PS_TotalGridParticleEffect;
    [SerializeField] private ParticleSystem PS_ScoreCard;



    #endregion


    #region  ---------------------------------------local variables---------------------------------------
    private GameObject[,] gridCells; // 2D array to store references to grid cells
    private StringBuilder SB_WordFormed, SB_TotalWords;
    private List<GameObject> wordStack;
    private GameObject lastClickedLetter;
    [HideInInspector] public int I_GridCategory;
    private int rows;
    private int columns = 6; // Number of columns in the grid
    private int gridLength;
    private float elapsedTime_Color, desiredDuration_Color = 0.5f;
    private List<Transform> coinList = new List<Transform>();
    private List<char> shuffledChars;
    private int remainingCharsNeeded;
    private int index;
    private List<string> wordList = new List<string>();
    private List<string> foundWordList = new List<string>();


    private int I_CollectedPoints = 0;
    private int I_FormedWordIndex = -1;
    private bool B_IsAudioPanelActive = false;


    #endregion



    private string _question = "Form words by clicking on the letters";
    private string _answer;
    private int _attempts = 1;




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

        #region =======================================integration=======================================

        // G_Game.SetActive(false);
        B_CloseDemo = true;

        // G_Transition.SetActive(false);
        G_levelComplete.SetActive(false);

        G_instructionPage.SetActive(false);

        TEX_points.text = I_Points.ToString();
        STRL_questions = new List<string>();
        STRL_answers = new List<string>();
        STRL_options = new List<string>();




        Invoke("THI_gameData", 1f);

        I_currentQuestionCount = -1;

        #endregion


    }


    public void GameInit()
    {
        SB_WordFormed = new StringBuilder();
        SB_TotalWords = new StringBuilder();
        wordStack = new List<GameObject>();
        shuffledChars = new List<char>();
        lastClickedLetter = null;

        ChangeCursor();
        PrepareWordList();
        CalculateGridLength();
        GridSizeCalculator(gridLength);
        GenerateGrid(cellPrefab);
        TEXM_instruction.text = STR_instruction;

        G_WordListParticleEffect.SetActive(true);
    }



    #region =======================================gameplay logic=======================================

    private void CalculateGridLength()
    {
        foreach (string word in wordList)
        {
            gridLength += word.Length;
        }
    }


    private void PrepareWordList()
    {
        //fetching questions list to local list and also adding audio to word list
        for (int i = 0; i < TXTA_Words.Length; i++)
        {
            wordList.Add(STRL_questions[i]);
            TXTA_Words[i].GetComponent<AudioSource>().clip = ACA__questionClips[i];
        }

        //appending input words to string builder
        for (int i = 0; i < TXTA_Words.Length; i++)
        {
            //setting words text to word list
            TXTA_Words[i].text = wordList[i];

            //SB_TotalWords.Append(wordList[i]);
            SB_WordFormed.Append(wordList[i]);
        }
    }


    private void ChangeCursor()
    {
        // Cursor.SetCursor(TEX_Crosshair, new Vector2(TEX_Crosshair.width / 2, TEX_Crosshair.height / 2), CursorMode.Auto);
        Cursor.SetCursor(TEX_Crosshair, new Vector2(0, 0), CursorMode.Auto);
        // Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }


    public void GridSizeCalculator(int size)
    {
        if (size < 30)//0 - 29 characters
        {
            rows = 7;//42
            I_GridCategory = 0;
        }
        else if (size > 29 && size < 40)// 30-39 characters
        {
            rows = 9;//54
            I_GridCategory = 1;
        }
        else if (size > 39 && size < 50)//40-49 characters
        {
            rows = 10;//60
            I_GridCategory = 2;
        }
        else if (size > 49 && size < 60)//50-59 characters
        {
            rows = 12;//72
            I_GridCategory = 3;
        }
        else if (size > 59 && size < 70)//60-69 characters
        {
            rows = 14;//84
            I_GridCategory = 4;
        }
        else if (size > 69 && size < 80)//70-79 characters
        {
            rows = 15;//90
            I_GridCategory = 5;
        }

        GA_GridBGCategory[I_GridCategory].SetActive(true);

        //appending remaining characters to total words string builder
        remainingCharsNeeded = (rows * columns) - SB_WordFormed.Length;

        Debug.Log("new wordlist---------");
        for (int i = 0; i < remainingCharsNeeded; i++)
        {
            wordList.Add(GetRandomLetter());
            Debug.Log(wordList[i]);
        }



        //and shuffling i##############################################
        // shuffledChars = Shuffle(SB_TotalWords.ToString());



        //without shuffling############################################
        wordList.Sort();

        for (int i = 0; i < wordList.Count; i++)
        {
            SB_TotalWords.Append(wordList[i]);
        }

        char[] charArray = (SB_TotalWords.ToString()).ToCharArray();
        shuffledChars = new List<char>(charArray);
    }


    public void GenerateGrid(GameObject cellPrefab)
    {
        SB_WordFormed.Clear();

        // Initialize the gridCells array
        gridCells = new GameObject[rows, columns];
        gridParent.position = TA_New_GridCategory[I_GridCategory].position;

        // Loop through each row and column to create grid cells
        for (int row = 0; row < rows; row++)
        {

            List<GameObject> innerList = new List<GameObject>();

            for (int col = 0; col < columns; col++)
            {
                // Calculate the position for the current grid cell
                Vector3 cellPosition = new Vector3(col, row, 0);

                // Instantiate a new grid cell GameObject at the calculated position
                GameObject cell = Instantiate(cellPrefab, cellPosition, Quaternion.identity, gridParent);

                cell.transform.localScale = Vector3.one;

                //getting the shuffled letter
                cell.GetComponentInChildren<TextMeshProUGUI>().text = shuffledChars[index++].ToString();

                //giving the cell rowcol as name
                cell.name = "" + row + col;

                // Store a reference to the grid cell GameObject in the gridCells array
                gridCells[row, col] = cell;

                cell.GetComponent<Animator>().enabled = false;
            }
        }

        Invoke(nameof(RemoveGridLayoutGroup), 0.1f);
    }


    private void RemoveGridLayoutGroup()
    {
        gridParent.GetComponent<GridLayoutGroup>().enabled = false;
    }


    private string GetRandomLetter()
    {
        return ((char)Random.Range(97, 122)).ToString();
    }


    List<char> Shuffle(string input)
    {
        char[] charArray = input.ToCharArray();

        // Shuffle the characters
        for (int i = 0; i < charArray.Length; i++)
        {
            int randomIndex = UnityEngine.Random.Range(i, charArray.Length);
            char temp = charArray[i];
            charArray[i] = charArray[randomIndex];
            charArray[randomIndex] = temp;
        }

        return new List<char>(charArray);
    }


    public void AddLetter(GameObject letter)
    {
        // Disable interactivity for the previously clicked letter
        if (lastClickedLetter != null)
        {
            lastClickedLetter.GetComponentInChildren<Button>().interactable = false;
        }

        // Update the last clicked letter
        lastClickedLetter = letter;

        // Enable interactivity for the current letter
        letter.GetComponentInChildren<Button>().interactable = true;

        SB_WordFormed.Append(letter.GetComponentInChildren<TextMeshProUGUI>().text.ToString());
        wordStack.Add(letter);
        UpdateFormedWord();
    }

    public void RemoveLetter() => StartCoroutine(IENUM_RemoveLetter());
    IEnumerator IENUM_RemoveLetter()
    {
        // Disable interactivity for the last clicked letter
        if (lastClickedLetter != null)
        {
            lastClickedLetter.GetComponentInChildren<Button>().interactable = false;
        }

        // Remove the last letter from the stack
        SB_WordFormed.Remove(SB_WordFormed.Length - 1, 1);
        wordStack.RemoveAt(wordStack.Count - 1);

        // Update the last clicked letter to the previous one
        if (wordStack.Count > 0)
        {
            lastClickedLetter = wordStack[wordStack.Count - 1];
            lastClickedLetter.GetComponentInChildren<Button>().interactable = true;
            yield return new WaitForSeconds(0.2f); // Small delay to ensure UI updates
        }
        else
        {
            lastClickedLetter = null;
        }

        // Update the formed word and trigger cascading effect
        UpdateFormedWord();
    }


    private void UpdateFormedWord()
    {
        if (SB_WordFormed.Length == 0)
        {
            TXT_Word.text = "-";
            StartCoroutine(IENUM_LerpColor(IMG_ButtonBG, IMG_ButtonBG.color, CLR_ButtonNormal));
        }
        else
        {
            TXT_Word.text = SB_WordFormed.ToString();

            if (wordList.Contains(SB_WordFormed.ToString()))
            {
                //play formed word VO
                for (int i = 0; i < STRL_answers.Count; i++)
                {
                    if (SB_WordFormed.ToString().Equals(STRL_answers[i]))
                    {
                        I_FormedWordIndex = i;
                        break;
                    }
                }
                SnailWordGame.AudioManager.Instance.PlayWordVO(ACA__questionClips[I_FormedWordIndex]);

                StartCoroutine(IENUM_LerpColor(IMG_ButtonBG, IMG_ButtonBG.color, CLR_ButtonCorrect));
                IMG_ButtonBG.GetComponent<Animator>().SetTrigger("active");
            }
            else
            {
                StartCoroutine(IENUM_LerpColor(IMG_ButtonBG, IMG_ButtonBG.color, CLR_ButtonWrong));
                IMG_ButtonBG.GetComponent<Animator>().SetTrigger("stop");
            }
        }
    }


    public void AddCoinPos(Transform pos)
    {
        coinList.Add(pos);
    }

    public void RemoveCoinPos()
    {
        coinList.RemoveAt(coinList.Count - 1);
    }


    private void SpawnCoins()
    {
        for (int i = 0; i < coinList.Count; i++)
        {
            Instantiate(G_Coin, coinList[i].position, Quaternion.identity, gridParent);
        }

        coinList.Clear();
    }


    public void BUT_Check()
    {
        StartCoroutine(IENUM_EnableDisableTransparentScreen());
        IMG_ButtonBG.GetComponent<Animator>().SetTrigger("clicked");
        IMG_ButtonBG.GetComponent<Animator>().SetTrigger("stop");

        //*success
        if (wordList.Contains(SB_WordFormed.ToString()))
        {
            //?scoring
            OnOptionClick(activityName, _question, SB_WordFormed.ToString(), true, _attempts);
            _attempts = 1;


            Invoke(nameof(UpdateScore), 3.3f);

            PS_TotalGridParticleEffect.Play();

            //greying out the found word
            for (int i = 0; i < TXTA_Words.Length; i++)
            {
                if (TXTA_Words[i].text == SB_WordFormed.ToString())
                {
                    TXTA_Words[i].color = CLR_ButtonNormal;
                }
            }

            //spawn coins
            Invoke(nameof(SpawnCoins), 0.2f);

            foundWordList.Add(SB_WordFormed.ToString());
            wordList.Remove(SB_WordFormed.ToString());

            StartCoroutine(ClearFormedWord());
            UpdateFormedWord();

            StartCoroutine(IENUM_LerpColor(IMG_ButtonBG, IMG_ButtonBG.color, CLR_ButtonNormal));

            //all words found
            if (wordList.Count == 0 || foundWordList.Count == TXTA_Words.Length)
            {
                StartCoroutine(DestroyRemainingTiles());
            }

        }
        //!failure
        else
        {
            if (foundWordList.Contains(SB_WordFormed.ToString()))
            {
                //word already found
                ANIM_ToastMessages[0].SetTrigger("active");
            }
            else
            {
                //word is not in the list
                ANIM_ToastMessages[1].SetTrigger("active");
                // Invoke(nameof(UpdateScore), 2.5f);

                //?scoring
                OnOptionClick(activityName, _question, SB_WordFormed.ToString(), false, _attempts);
                _attempts++;
            }

            SnailWordGame.AudioManager.Instance.PlayWrong();
        }
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


    private void UpdateScore()
    {
        THI_TrackGameData("1");

        I_Points = I_Points * I_correctPoints;
        I_CollectedPoints += I_Points;
        TEX_points.text = I_CollectedPoints.ToString();
        I_Points = 0;

        ANIM_ScoreCard.SetTrigger("clicked");
        PS_ScoreCard.Play();
        // SnailWordGame.AudioManager.Instance.PlayCoinChime();
    }


    IEnumerator ClearFormedWord()
    {
        // clearing the word
        SB_WordFormed.Clear();
        TXT_Word.text = SB_WordFormed.ToString();

        // Cascading effect
        for (int i = 0; i < wordStack.Count; i++)
        {
            wordStack[i].GetComponent<Animator>().enabled = true;
            wordStack[i].GetComponent<Animator>().SetTrigger("inactive");
            SnailWordGame.AudioManager.Instance.PlayCorrect();
        }

        wordStack.Clear();
        yield return null;
    }


    IEnumerator IENUM_LerpColor(Image img, Color32 currentColor, Color32 targetColor)
    {
        //*slowly changing color for background
        while (elapsedTime_Color < desiredDuration_Color)
        {
            elapsedTime_Color += Time.deltaTime;
            float percentageComplete = elapsedTime_Color / desiredDuration_Color;

            img.color = Color.Lerp(currentColor, targetColor, percentageComplete);
            yield return null;
        }

        //resetting elapsed time back to zero
        elapsedTime_Color = 0f;
    }


    IEnumerator IENUM_EnableDisableTransparentScreen()
    {
        G_TransparentScreen.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        G_TransparentScreen.SetActive(false);
    }


    IEnumerator DestroyRemainingTiles()
    {
        yield return new WaitForSeconds(3);

        foreach (Transform child in gridParent)
        {
            if (gameObject.activeInHierarchy)
            {
                child.GetComponent<Animator>().enabled = true;
                child.GetComponent<Animator>().SetTrigger("inactive");
            }
        }
        SnailWordGame.AudioManager.Instance.PlayCorrect();

        yield return new WaitForSeconds(1.25f);
        G_Scroll.SetActive(false);
        //after destroying all tiles
        ShowGameOverPanel();
    }


    public void BUT_AudioPanel()
    {
        if (B_IsAudioPanelActive)
        {
            G_AudioPanel.SetActive(false);
            B_IsAudioPanelActive = false;
        }
        else
        {
            G_AudioPanel.SetActive(true);
            B_IsAudioPanelActive = true;
        }
    }


    private void ShowGameOverPanel()
    {
        G_WinWindow.SetActive(true);
        SnailWordGame.AudioManager.Instance.PlayYummy(3.8f);
        Invoke("THI_Levelcompleted", 4f);
    }


    #endregion



    #region =======================================integration=======================================


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
        DBmanager SnailWordGameDB = new DBmanager();
        SnailWordGameDB.question_id = STR_currentQuestionID;
        SnailWordGameDB.answer = STR_currentSelectedAnswer;
        SnailWordGameDB.analysis = analysis;
        string toJson = JsonUtility.ToJson(SnailWordGameDB);
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


    #endregion


    public void OnOptionClick(string activityName, string questionText, string selectedOption, bool isCorrect, int attempts)
    {
        ActivityDataManager.Instance.RecordAnswer(activityName, questionText, selectedOption, isCorrect, attempts);
        Debug.Log(
            "activity name : " + activityName + "\n" +
            "question : " + questionText + "\n" +
            "answer : " + selectedOption + "\n" +
            "is correct : " + isCorrect + "\n" +
            "attempts : " + attempts
        );

        ActivityDataManager.Instance.SaveToLocalWebStorage();
    }

}
