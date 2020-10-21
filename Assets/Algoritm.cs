using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Algoritm : MonoBehaviour
{
    [SerializeField]
    private GameObject Your_Move;
    [SerializeField]
    private GameObject AI_Move;
    [SerializeField]
    private GameObject back_card;
    [SerializeField]
    private Sprite[] Cards;
    [SerializeField]
    private Transform t_arrow;

    [SerializeField]
    private GameObject whoIsWinner;

    [SerializeField]
    private TMP_Text am_cards_player;
    [SerializeField]
    private TMP_Text am_cards_AI;

    private List<int> taliaGracza = new List<int>();
    private List<int> taliaAI = new List<int>();

    private List<int> whileDraw = new List<int>();

    void Start()
    {        
        int counter = 0;
        bool isThere = false;

        while(counter < 27)
        {            
            int temp = Random.Range(0, 53);
            isThere = false;

            for (int i = 0; i < taliaGracza.Count; i++)
            {
                if(temp == taliaGracza[i])
                {
                    isThere = true;
                    break;
                }
            }

            if (!isThere || counter==0)
            {
                taliaGracza.Add(temp);
                counter++;
            }            
        }

        bool isThere2 = false;
        
        for(int i = 0; i < Cards.Length; i++)
        {
            isThere2 = false;

            for (int j = 0; j < taliaGracza.Count; j++)
            {
                if(i == taliaGracza[j])
                {
                    isThere2 = true;
                    break;
                }
            }
            if (!isThere2)
            {
                taliaAI.Add(i);
            }
        }        
    }

    public void MakeMove()
    {        
        StartCoroutine(waitForOpponent());
    }

    private IEnumerator waitForOpponent()
    {
        GameObject[] objCards = GameObject.FindGameObjectsWithTag("card");
        for(int i = 0; i < objCards.Length; i++)
        {
            Destroy(objCards[i]);
        }

        whoIsWinner.GetComponent<TextMeshProUGUI>().enabled = false;

        yield return new WaitForSeconds(0.5f);

        t_arrow.Rotate(new Vector3(0f, 0f, 180f));
        int next_card = taliaGracza[0];
        int pPick = next_card+1;

        GameObject obj = Instantiate(Your_Move);
        obj.GetComponent<SpriteRenderer>().sprite = Cards[next_card];        

        yield return new WaitForSeconds(1);

        t_arrow.Rotate(new Vector3(0f, 0f, 180f));
        next_card = taliaAI[0];
        int aiPick = next_card+1;        

        GameObject obj2 = Instantiate(AI_Move);
        obj2.GetComponent<SpriteRenderer>().sprite = Cards[next_card];

        yield return new WaitForSeconds(1);

        checkWhoWin(pPick, aiPick);
    }

    void Update()
    {
        am_cards_player.text = "" + taliaGracza.Count + "";
        am_cards_AI.text = "" + taliaAI.Count + "";
    }

    private void checkWhoWin(int pP, int aiP)
    {
        int cardTierP = check_card(pP);
        int cardTierAI = check_card(aiP);

        print("P " + pP + " AI " + aiP);
        if(cardTierP > cardTierAI)
        {
            whoIsWinner.transform.localPosition = new Vector3(-130f, 95f, 0f);
            whoIsWinner.GetComponent<TextMeshProUGUI>().enabled = true;

            taliaGracza.Add(taliaAI[0]);
            taliaAI.Remove(taliaAI[0]);

            int temp = taliaGracza[0];
            taliaGracza.Remove(taliaGracza[0]);
            taliaGracza.Add(temp);
        }
        else if(cardTierP < cardTierAI)
        {
            whoIsWinner.transform.localPosition = new Vector3(130f, 95f, 0f);
            whoIsWinner.GetComponent<TextMeshProUGUI>().enabled = true;

            taliaAI.Add(taliaGracza[0]);
            taliaGracza.Remove(taliaGracza[0]); //przekazanie karty z talii gracza do AI

            int temp = taliaAI[0];
            taliaAI.Remove(taliaAI[0]); //karta idzie na koniec
            taliaAI.Add(temp);            
        }
        else if (cardTierP == cardTierAI)
        {
            whileDraw.Add(pP - 1);
            whileDraw.Add(aiP - 1);
            whileDraw.Add(taliaGracza[1]);
            whileDraw.Add(taliaAI[1]);
            whileDraw.Add(taliaGracza[2]);
            whileDraw.Add(taliaAI[2]);

            int p = check_card(taliaGracza[2]);
            int ai = check_card(taliaAI[2]);

            GameObject back = Instantiate(back_card);
            back.transform.position = new Vector3(-2f, 0f, 0f);
            GameObject back2 = Instantiate(back_card);

            GameObject obj = Instantiate(Your_Move);
            obj.GetComponent<SpriteRenderer>().sprite = Cards[taliaGracza[2]];

            GameObject obj2 = Instantiate(AI_Move);
            obj2.GetComponent<SpriteRenderer>().sprite = Cards[taliaAI[2]];

            if(p > ai)
            {
                whoIsWinner.transform.localPosition = new Vector3(-130f, 95f, 0f);
                whoIsWinner.GetComponent<TextMeshProUGUI>().enabled = true;

                for(int i = 0; i < whileDraw.Count; i++)
                {
                    taliaGracza.Add(whileDraw[i]);
                }
                taliaAI.RemoveRange(0, 3);
                whileDraw.Clear();
            }
            if(ai > p)
            {
                whoIsWinner.transform.localPosition = new Vector3(130f, 95f, 0f);
                whoIsWinner.GetComponent<TextMeshProUGUI>().enabled = true;

                for (int i = 0; i < whileDraw.Count; i++)
                {
                    taliaAI.Add(whileDraw[i]);
                }
                taliaGracza.RemoveRange(0, 3);
                whileDraw.Clear();
            }
        }
    }

    private int check_card(int p)
    {
        int cardTierP = 0;        
        /*
         14 Joker
         13 As
         12 Król
         11 Królowa
         10 Jopek
        */
        if (p == 53 || p == 54)
        {
            cardTierP = 14;
        }        

        if (p >= 1 && p <= 4)
        {
            cardTierP = 13;
        }

        if (p >= 5 && p <= 8)
        {
            cardTierP = 12;
        }

        if (p >= 9 && p <= 12)
        {
            cardTierP = 11;
        }        

        if (p >= 13 && p <= 16)
        {
            cardTierP = 10;
        }        

        if (p >= 17 && p <= 20)
        {
            cardTierP = 9;
        }        

        if (p >= 21 && p <= 24)
        {
            cardTierP = 8;
        }        

        if (p >= 25 && p <= 28)
        {
            cardTierP = 7;
        }        

        if (p >= 29 && p <= 32)
        {
            cardTierP = 6;
        }        

        if (p >= 33 && p <= 36)
        {
            cardTierP = 5;
        }

        if (p >= 37 && p <= 40)
        {
            cardTierP = 4;
        }        

        if (p >= 41 && p <= 44)
        {
            cardTierP = 3;
        }        

        if (p >= 45 && p <= 48)
        {
            cardTierP = 2;
        }        

        if (p >= 49 && p <= 52)
        {
            cardTierP = 1;
        }

        return cardTierP;
    }
}
