using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

public class SnailGameManager : MonoBehaviour
{
    public string check;
    public int rows; // Number of rows in the grid
    public int columns; // Number of columns in the grid
    public TextMeshProUGUI TXT_Word;
    public GameObject cellPrefab; // Prefab for individual grid cells
    public Transform gridParent;
    public Transform gridParentNewPosition;

    private GameObject[,] gridCells; // 2D array to store references to grid cells
    private StringBuilder sb;
    private List<GameObject> wordStack;
    private GameObject lastClickedLetter;
    private bool isFirst = true;



    void Start()
    {
        GridSizeCalculator(40);
        GenerateGrid(cellPrefab);
        sb = new StringBuilder();
        wordStack = new List<GameObject>();
        lastClickedLetter = null;

    }


    public void GridSizeCalculator(int size)
    {
        if (size > 19 && size < 30)
        {
            rows = 9;
        }
        else if (size > 29 && size < 40)
        {
            rows = 10;
        }
        else if (size > 39 && size < 50)
        {
            rows = 12;
        }
        else if (size > 49 && size < 60)
        {
            rows = 13;
        }
        else if (size > 59 && size < 70)
        {
            rows = 14;
        }
        else if (size > 69 && size < 80)
        {
            rows = 15;
        }
    }


    public void GenerateGrid(GameObject cellPrefab)
    {
        // Initialize the gridCells array
        gridCells = new GameObject[rows, columns];

        // Loop through each row and column to create grid cells
        for (int row = 0; row < rows; row++)
        {
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
            }
        }

        gridParent.position = gridParentNewPosition.position;
        Invoke(nameof(RemoveGridLayoutGroup), 0.1f);
    }


    private void RemoveGridLayoutGroup()
    {
        gridParent.GetComponent<GridLayoutGroup>().enabled = false;
    }


    private string GetRandomLetter()
    {
        return ((char)Random.Range(65, 75)).ToString();
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
        }

        Check();
    }

    private void Check()
    {
        // Implement your word checking logic here
        if (sb.ToString() == check)
        {
            ClearFormedWord();
            // Add your code here to handle successful word formation
        }
    }


    private void ClearFormedWord()
    {
        GameObject aboveGameObject = null;
        string aboveCellName = "";
        int r = 0, c = 0;

        for (int i = 0; i < wordStack.Count; i++)
        {
            wordStack[i].SetActive(false);

            r = int.Parse((wordStack[i].name[0]).ToString());
            c = int.Parse((wordStack[i].name[1]).ToString());

            while (r > 0)
            {
                aboveCellName = "" + (r - 1) + c;

                foreach (Transform child in gridParent)
                {
                    if (aboveCellName == child.name)
                    {
                        child.GetComponent<LetterController>().Move(child.transform.position, wordStack[i].transform.position);
                        break;
                    }
                }

                r--;
            }


        }

        sb.Clear();
        TXT_Word.text = sb.ToString();
    }




}
