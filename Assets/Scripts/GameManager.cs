using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform gameTransform;
    [SerializeField] private Transform piecePrefab;

    private List<Transform> pieces;
    private int emptyLocation;
    private int size;
    private bool shuffling = false;

    private void CreateGamePieces(float gapThickness)
    {
        float width = 1 / (float)size;
        for(int row = 0; row < size; row++)
        {
            for(int col = 0; col < size; col++)
            {
                Transform piece = Instantiate(piecePrefab, gameTransform);
                pieces.Add(piece);
                piece.localPosition = new Vector3(-1 + (2 * width * col) + width, +1 - (2 * width * row) - width, 0);
                piece.localScale = ((2 * width) - gapThickness) * Vector3.one;
                piece.name = $"{(row * size) + col}";

                if((row == size - 1) && (col == size - 1))
                {
                    emptyLocation = (size * size) - 1;
                    piece.gameObject.SetActive(false);
                }
                else
                {
                    float gap = gapThickness / 2;
                    Mesh mesh = piece.GetComponent<MeshFilter>().mesh;
                    Vector2[] uv = new Vector2[4];

                    uv[0] = new Vector2((width * col) + gap, 1 - ((width * (row + 1)) - gap));
                    uv[1] = new Vector2((width * (col + 1)) - gap, 1 - ((width * (row + 1)) - gap));
                    uv[2] = new Vector2((width * col) + gap, 1 - ((width * row) + gap));
                    uv[3] = new Vector2((width * (col + 1)) - gap, 1 - ((width * row) + gap));
                    mesh.uv = uv;
                }
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        pieces = new List<Transform>();
        size = 4;
        CreateGamePieces(0.005f);
    }
    // Update is called once per frame
    void Update()
    {
        //check for completion
        if(!shuffling && CheckCompletion())
        {
            shuffling = true;
            StartCoroutine(WaitShuffle(2f));
        }

        //on click send out ray to see if we clicked on a piece
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            for (int i = 0; i < pieces.Count; i++)
            {
                if (pieces[i] == hit.transform)
                {
                    //check each direction to see if valid move
                    //we break out on the sucess so we dont carry on and swap back again
                    if(SwapIfValid(i, -size, size)) { break; }
                    if(SwapIfValid(i, +size, size)) { break; }
                    if(SwapIfValid(i, -1, 0)) { break; }
                    if(SwapIfValid(i, +1, size - 1)) { break; }
                }
            }

        }
    }
    //colCheck is used to stop horizontal moves wrapping
    private bool SwapIfValid(int i, int offset, int colCheck)
    {
        if(((i % size) != colCheck) && ((i + offset) == emptyLocation))
        {
            //swap them in game state
            (pieces[i], pieces[i + offset]) = (pieces[i + offset], pieces[i]);
            //swap their Transforms
            (pieces[i].localPosition, pieces[i + offset].localPosition) = ((pieces[i + offset].localPosition, pieces[i].localPosition));
            //update emptylocation
            emptyLocation = i;
            return true;
        }
        return false;
    }
    //we make the pieces in order so we can use this to check completion
    private bool CheckCompletion()
    {
        for(int i = 0; i < pieces.Count; i++)
        {
            if (pieces[i].name != $"{i}")
            {
                return false;
            }
        } 
        return true;
    }
    private IEnumerator WaitShuffle(float duration)
    {
        yield return new WaitForSeconds(duration);
        Shuffle();
        shuffling = false;
    }
    private void Shuffle()
    {
        int count = 0;
        int last = 0;
        while(count < (size * size * size))
        {
            //pick a random location
            int rnd = Random.Range(0, size * size);
            //Only thing we forid is doing last move
            if(rnd == last) { continue; }
            last = emptyLocation;
            //try surrounding spaces looking for valid move
            if(SwapIfValid(rnd, -size, size))
            {
                count++;
            }else if(SwapIfValid(rnd, +size, size))
            {
                count++;
            }else if(SwapIfValid(rnd, -1, 0))
            {
                count++;
            }else if(SwapIfValid(rnd, +1, size - 1))
            {
                count++;
            }
        }
    }
}
