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



    public int size;
    public string check;
    public int rows; // Number of rows in the grid
    public int columns; // Number of columns in the grid

    [Space(10)]

    [SerializeField] private Texture2D TEX_Crosshair;

    [Space(10)]

    [SerializeField] private Image IMG_ButtonBG;

    [Space(10)]

    public TextMeshProUGUI TXT_Word;

    [Space(10)]

    [SerializeField] private Animator[] ANIM_ToastMessages;
    public GameObject cellPrefab; // Prefab for individual grid cells
    public GameObject G_Fruit;
    public GameObject[] GA_GridBGCategory;

    [Space(10)]

    public Transform gridParent;
    public Transform[] TA_NewGridCategory;



    private GameObject[,] gridCells; // 2D array to store references to grid cells
    private StringBuilder sb;
    private List<GameObject> wordStack;
    private GameObject lastClickedLetter;
    private bool isFirst = true;
    private int gridCategory;



    private List<GameObject> cascadeGameObjectsList = new List<GameObject>();
    private string aboveCellName = "";
    private int r = 0, c = 0;



    private float elapsedTime_Color, desiredDuration_Color = 0.5f;


    private List<string> wordList = new List<string>();
    private List<string> foundWordList = new List<string>();



    void Start()
    {
        ChangeCursor();
        GridSizeCalculator(size);
        GenerateGrid(cellPrefab);
        sb = new StringBuilder();
        wordStack = new List<GameObject>();
        lastClickedLetter = null;
        PrepareWordList();
    }


    private void PrepareWordList()
    {
        wordList.Add("AA");
        wordList.Add("BB");
        wordList.Add("CC");
        wordList.Add("DD");
        wordList.Add("EE");
        wordList.Add("ABC");
        wordList.Add("BCA");
        wordList.Add("CAB");
    }


    private void ChangeCursor()
    {
        // Cursor.SetCursor(TEX_Crosshair, new Vector2(TEX_Crosshair.width / 2, TEX_Crosshair.height / 2), CursorMode.Auto);
        Cursor.SetCursor(TEX_Crosshair, new Vector2(0, 0), CursorMode.Auto);
        // Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }


    public void GridSizeCalculator(int size)
    {
        if (size < 30)//0 - 29
        {
            rows = 7;//42
            gridCategory = 0;
        }
        else if (size > 29 && size < 40)// 30-39
        {
            rows = 9;//54
            gridCategory = 1;
        }
        else if (size > 39 && size < 50)//40-49
        {
            rows = 10;//60
            gridCategory = 2;
        }
        else if (size > 49 && size < 60)//50-59
        {
            rows = 12;//72
            gridCategory = 3;
        }
        else if (size > 59 && size < 70)//60-69
        {
            rows = 14;//84
            gridCategory = 4;
        }
        else if (size > 69 && size < 80)//70-79
        {
            rows = 15;//90
            gridCategory = 5;
        }

        GA_GridBGCategory[gridCategory].SetActive(true);
    }


    public void GenerateGrid(GameObject cellPrefab)
    {
        // Initialize the gridCells array
        gridCells = new GameObject[rows, columns];
        gridParent.position = TA_NewGridCategory[gridCategory].position;

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
        Debug.Log(gridCells.Length);
    }


    private string GetRandomLetter()
    {
        return ((char)Random.Range(65, 70)).ToString();
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

        sb.Append(letter.GetComponentInChildren<TextMeshProUGUI>().text.ToString());
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
        sb.Remove(sb.Length - 1, 1);
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
        if (sb.Length == 0)
        {
            TXT_Word.text = "-";
        }
        else
        {
            TXT_Word.text = sb.ToString();
            UpdateButtonBG();
        }

        // Check();
    }

    private void UpdateButtonBG()
    {
        if (!wordList.Contains(sb.ToString()))
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
        // Implement your word checking logic here
        /* if (sb.ToString() == check)
        {
            StartCoroutine(ClearFormedWord());
            UpdateFormedWord();
            // Add your code here to handle successful word formation
        } */

        if (wordList.Contains(sb.ToString()))
        {
            foundWordList.Add(sb.ToString());
            wordList.Remove(sb.ToString());

            StartCoroutine(ClearFormedWord());
            UpdateFormedWord();

            StartCoroutine(IENUM_LerpColor(IMG_ButtonBG, IMG_ButtonBG.color, CLR_ButtonNormal));
            IMG_ButtonBG.GetComponent<Animator>().SetTrigger("clicked");
            IMG_ButtonBG.GetComponent<Animator>().SetTrigger("stop");
            // Add your code here to handle successful word formation
        }
        else
        {
            if (foundWordList.Contains(sb.ToString()))
            {
                //word already found
                ANIM_ToastMessages[0].SetTrigger("active");
            }
            else
            {
                //word is not in the list
                ANIM_ToastMessages[1].SetTrigger("active");
            }
        }
    }


    IEnumerator ClearFormedWord()
    {
        // clearing the word
        sb.Clear();
        TXT_Word.text = sb.ToString();

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


}
