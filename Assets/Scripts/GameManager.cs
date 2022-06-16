using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private int pelletAmount;
    private static int score;
    private static int current_level = 1;
    private static int lifes = 3;

    private List<GameObject> ghostList = new List<GameObject>();

    public GameObject pacmanObj;

    //FrightenedTimer
    private float fTimer = 5f;
    private float curFTimer = 0f;

    //ChaseTimer
    private float cTimer = 20f;
    private float curCTimer = 0f;

    //ScatterTimer
    private float sTimer = 7f;
    private float curSTimer = 0f;

    //Bools
    private bool chase;
    private bool scatter;
    public bool frigthened;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Debug.Log("score: " + score + " current level: " + current_level);
        scatter = true;
        ghostList.AddRange(GameObject.FindGameObjectsWithTag("Ghost"));
        pacmanObj = GameObject.FindGameObjectWithTag("Player");

        UIManager.instance.UpdateUI();
    }

    private void Update()
    {
        Timing();
    }
    /// <summary>
    /// Adds pellets
    /// </summary>
    public void AddPellet()
    {
        pelletAmount++;
    }

    public void ReducePellet(int amount)
    {
        pelletAmount--;
        score += amount;
        UIManager.instance.UpdateUI();

        if (pelletAmount <= 0)
        {
            WinCondition();
            Debug.Log("You've won the game");
        }

        for (int i = 0; i < ghostList.Count; i++)
        {
            PathFinding pGhost = ghostList[i].GetComponent<PathFinding>();
            if (score >= pGhost.pointsToCollect && !pGhost.released)
            {
                pGhost.state = PathFinding.GhostStates.CHASE;
                pGhost.released = true;
            }
        }
    }

    private void WinCondition()
    {
        current_level++;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void Timing()
    {
        UpdateStates();
        if (chase)
        {
            curCTimer = curCTimer + Time.deltaTime;
            if (curCTimer >= cTimer)
            {
                curCTimer = 0;
                chase = false;
                scatter = true;
            }
        }

        if (scatter)
        {
            curSTimer = curSTimer + Time.deltaTime;
            if (curSTimer >= sTimer)
            {
                curSTimer = 0;
                scatter = false;
                chase = true;
            }
        }

        if (frigthened)
        {
            if (curCTimer != 0 || curSTimer != 0)
            {
                scatter = false;
                chase = false;
                curCTimer = 0;
                curSTimer = 0;
            }

            curFTimer = curFTimer + Time.deltaTime;
            if (curFTimer >= fTimer)
            {
                curFTimer = 0;
                scatter = false;
                chase = true;
                frigthened = false;
            }
        }
    }

    void UpdateStates()
    {
        for (int i = 0; i < ghostList.Count; i++)
        {
            PathFinding pGhost = ghostList[i].GetComponent<PathFinding>();
            if (pGhost.state == PathFinding.GhostStates.CHASE && scatter)
            {
                pGhost.state = PathFinding.GhostStates.SCATTER;
            }
            else if (pGhost.state == PathFinding.GhostStates.SCATTER && chase)
            {
                pGhost.state = PathFinding.GhostStates.CHASE;
            }
            else if (frigthened && pGhost.state != PathFinding.GhostStates.HOME &&
                     pGhost.state != PathFinding.GhostStates.GOT_EATEN)
            {
                pGhost.state = PathFinding.GhostStates.FRIGHTENED;
            }
            else if (pGhost.state == PathFinding.GhostStates.FRIGHTENED)
            {
                pGhost.state = PathFinding.GhostStates.CHASE;
            }
        }
    }

    public void LoseLife()
    {
        lifes--;
        UIManager.instance.UpdateUI();

        Debug.Log("Remaining life of pacman : " + lifes);
        if (lifes <= 0)
        {
            ScoreHolder.level = current_level;
            ScoreHolder.score = score;

            SceneManager.LoadScene("GameOver");
            return;
        }

        foreach (GameObject ghost in ghostList)
        {
            ghost.GetComponent<PathFinding>().Reset();
        }
        pacmanObj.GetComponent<PacMan>().Reset();
    }

    public void AddScore(int amount)
    {
        score += amount;
    }

    public int ReadScore()
    {
        return score;
    }

    public int ReadLevel()
    {
        return current_level;
    }

    public int ReadLife()
    {
        return lifes;
    }
}