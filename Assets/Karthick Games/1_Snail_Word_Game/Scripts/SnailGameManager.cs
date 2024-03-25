using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

public class SnailGameManager : MonoBehaviour
{
    public Color32 CLR_ButtonNormal;
    public Color32 CLR_ButtonCorrect;
    public Color32 CLR_ButtonWrong;

    private int columns = 6; // Number of columns in the grid

    [Space(10)]

    [SerializeField] private Texture2D TEX_Crosshair;

    [Space(10)]

    [SerializeField] private Image IMG_ButtonBG;

    [Space(10)]

    [SerializeField] private TextMeshProUGUI[] TXTA_Words;
    [SerializeField] private TextMeshProUGUI TXT_Word;

    [Space(10)]

    [SerializeField] private Animator[] ANIM_ToastMessages;
    [SerializeField] private GameObject cellPrefab; // Prefab for individual grid cells
    [SerializeField] private GameObject G_Fruit;
    [SerializeField] private GameObject[] GA_GridBGCategory;
    [SerializeField] private GameObject G_TransparentScreen;

    [Space(10)]

    [SerializeField] private Transform gridParent;
    [SerializeField] private Transform[] TA_New_GridCategory;



    private GameObject[,] gridCells; // 2D array to store references to grid cells
    private StringBuilder SB_WordFormed, SB_TotalWords;
    private List<GameObject> wordStack;
    private GameObject lastClickedLetter;
    [HideInInspector] public int I_GridCategory;


    private List<GameObject> cascadeGameObjectsList = new List<GameObject>();
    [SerializeField] private List<string> wordList = new List<string>();


    private int rows;
    private int gridLength;

    private float elapsedTime_Color, desiredDuration_Color = 0.5f;
    private List<string> foundWordList = new List<string>();
    private List<char> shuffledChars;



    void Start()
    {
        SB_WordFormed = new StringBuilder();
        SB_TotalWords = new StringBuilder();
        wordStack = new List<GameObject>();
        lastClickedLetter = null;

        ChangeCursor();
        CalculateGridLength();
        GridSizeCalculator(gridLength);
        GenerateGrid(cellPrefab);
        PrepareWordList();
    }


    private void CalculateGridLength()
    {
        foreach (string word in wordList)
        {
            gridLength += word.Length;
        }
    }


    private void PrepareWordList()
    {
        for (int i = 0; i < TXTA_Words.Length; i++)
        {
            TXTA_Words[i].text = wordList[i];
            SB_TotalWords.Append(wordList[i]);
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
    }


    public void GenerateGrid(GameObject cellPrefab)
    {
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
                GameObject cell = Instantiate(cellPrefab, cellPosition, Quaternion.identity);

                // Set the parent of the grid cell GameObject to this GridGenerator GameObject
                cell.transform.parent = gridParent;
                cell.transform.localScale = Vector3.one;
                cell.GetComponentInChildren<TextMeshProUGUI>().text = GetRandomLetter();

                // Set the name of the grid cell GameObject for easy identification
                // cell.name = "Cell (" + row + ", " + col + ")";
                cell.name = "" + row + col;

                // Store a reference to the grid cell GameObject in the gridCells array
                gridCells[row, col] = cell;

                // yield return new WaitForSeconds(0.05f);
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




    char NextCharacter()
    {
        // Get and remove a random character from the shuffled list
        int randomIndex = UnityEngine.Random.Range(0, shuffledChars.Count);
        char nextChar = shuffledChars[randomIndex];
        shuffledChars.RemoveAt(randomIndex);
        return nextChar;
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


    public void RemoveLetter()
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
            UpdateButtonBG();
        }
    }

    private void UpdateButtonBG()
    {
        if (!wordList.Contains(SB_WordFormed.ToString()))
        {
            if (!IMG_ButtonBG.color.Equals(CLR_ButtonWrong))
            {
                StartCoroutine(IENUM_LerpColor(IMG_ButtonBG, IMG_ButtonBG.color, CLR_ButtonWrong));
            }
        }
        else
        {
            StartCoroutine(IENUM_LerpColor(IMG_ButtonBG, IMG_ButtonBG.color, CLR_ButtonCorrect));
            IMG_ButtonBG.GetComponent<Animator>().SetTrigger("active");
        }

    }


    public void BUT_Check()
    {
        StartCoroutine(IENUM_EnableDisableTransparentScreen());
        IMG_ButtonBG.GetComponent<Animator>().SetTrigger("clicked");
        IMG_ButtonBG.GetComponent<Animator>().SetTrigger("stop");

        if (wordList.Contains(SB_WordFormed.ToString()))
        {
            foundWordList.Add(SB_WordFormed.ToString());
            //greying out the found word
            for (int i = 0; i < TXTA_Words.Length; i++)
            {
                if (TXTA_Words[i].text == SB_WordFormed.ToString())
                {
                    TXTA_Words[i].color = CLR_ButtonNormal;
                }
            }

            wordList.Remove(SB_WordFormed.ToString());

            StartCoroutine(ClearFormedWord());
            UpdateFormedWord();

            StartCoroutine(IENUM_LerpColor(IMG_ButtonBG, IMG_ButtonBG.color, CLR_ButtonNormal));
        }
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
            }

            AudioManager.Instance.PlayWrong();
        }
    }


    IEnumerator ClearFormedWord()
    {
        // clearing the word
        SB_WordFormed.Clear();
        TXT_Word.text = SB_WordFormed.ToString();

        // Cascading effect
        for (int i = 0; i < wordStack.Count; i++)
        {
            // wordStack[i].SetActive(false);
            wordStack[i].GetComponent<Animator>().enabled = true;
            wordStack[i].GetComponent<Animator>().SetTrigger("inactive");
            AudioManager.Instance.PlayCorrect();
        }

        wordStack.Clear();

        yield return null;
    }


    private void CascadeCells(GameObject from, GameObject to)
    {
        from.GetComponent<LetterController>().Move(from.transform.position, to.transform.position);
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






}
