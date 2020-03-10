using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameHandler : MonoBehaviour
{
    public static GameHandler Instance;
    public GameObject PreviousCard;
    public Animator PreviousCardAnimator;
    public GameObject CardParent;
    public int CardRepetationNo;
    public List<GameObject> CardTypes;
    public List<Transform> CardPositions;
    public TextMeshProUGUI ClickCountText;
    public TextMeshProUGUI TimeSpentText;
    public bool IsCardClickable;

    private int clickCount;
    [SerializeField]
    private float timeSpan;
    private void Awake()
    {
        SetInstance();
    }


    void SetInstance()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }


    private void Start()
    {
        GenerateCards();
        IsCardClickable = true;
    }

    void GenerateCards()
    {
        for (int i = 0; i < CardRepetationNo; i++)
        {
            for (int j = 0; j < CardTypes.Count; j++)
            {
                int posIndex = Random.Range(0, CardPositions.Count);
                GameObject card = Instantiate(CardTypes[j].gameObject, CardPositions[posIndex].position,
                    Quaternion.Euler(0,180,0));
                card.transform.SetParent(CardParent.transform);
                CardPositions.RemoveAt(posIndex);
            }
        }
    }

    public void CalculateClickCount()
    {
        clickCount++;
        ClickCountText.text = "Total Clicks - " + clickCount;
    }


    public IEnumerator SetFirstCardInfo(GameObject firstCard)
    {
        PreviousCard = firstCard;
        PreviousCardAnimator = firstCard.GetComponent<Animator>();
        firstCard.GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(1.5f);
        IsCardClickable = true;
    }


    public IEnumerator CardMatchFunction(GameObject currentCard)
    {
        currentCard.GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(1.5f);
        currentCard.GetComponent<Animator>().SetTrigger(AnimationName.CardMatch);
        PreviousCard.GetComponent<Animator>().SetTrigger(AnimationName.CardMatch);
        PreviousCard = null;
        IsCardClickable = true;
    }
    
    public IEnumerator CardNotMatchFunction(Animator currentCardAnimator, GameObject currentCard)
    {
        currentCard.GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(1.5f);
        currentCardAnimator.SetBool(AnimationName.CardFlip, false);
        PreviousCardAnimator.SetBool(AnimationName.CardFlip, false);
        yield return new WaitForSeconds(1.5f);
        currentCard.GetComponent<Collider>().enabled = true;
        PreviousCard.GetComponent<Collider>().enabled = true;
        PreviousCard = null;
        IsCardClickable = true;
    }


    private void Update()
    {
        CalculateSpendTime();
    }


    public void CalculateSpendTime()
    {
        timeSpan = timeSpan + Time.deltaTime;
        TimeSpentText.text = "Time Span - " + (int)timeSpan;
    }


    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
    
}
