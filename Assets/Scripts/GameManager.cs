using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    
    [SerializeField] int winYou,winRobot;
    [SerializeField] TextMeshProUGUI txtWinYou, txtWinRobot;
    [SerializeField] Sprite[] spChoices;
    [SerializeField] Transform MyChoice, RobotChoice;
    [SerializeField] Transform buttonsChoice;
    [SerializeField] Transform pnlMessage;
    private void Start()
    {
        for (int i = 0; i < buttonsChoice.childCount; i++)
        {
            Button btn = buttonsChoice.GetChild(i).GetComponent<Button>();
            int c = i;
            btn.onClick.AddListener(delegate {SetChoice(btn.gameObject,c); });
        }
    }

    public void SetChoice(GameObject btn,int choice)
    {
        for (int i = 0; i < buttonsChoice.childCount; i++)
        {
            Transform trc = buttonsChoice.GetChild(i);
            if (trc.gameObject == btn)
                trc.GetChild(0).GetComponent<Image>().color = Color.green;
            else
                trc.GetChild(0).GetComponent<Image>().color = Color.gray;
        }
        StartCoroutine(StartRount(choice));
    }

    IEnumerator StartRount(int choice)
    {
        yield return new WaitForSeconds(1);

        pnlMessage.gameObject.SetActive(true);
        TextMeshProUGUI txtTimer = pnlMessage.Find("txtTimer").GetComponent<TextMeshProUGUI>();
        txtTimer.enabled = true;

        int timer = 3;
        float _timer = timer;
        float time = Time.time + timer;
        int fSize =(int)txtTimer.fontSize;
        while (timer > 0)
        {
            _timer = time - Time.time;
            txtTimer.fontSize = Mathf.Lerp(txtTimer.fontSize,50,Time.deltaTime);
            if (_timer <= timer - 1)
            {
                timer--;
                txtTimer.fontSize = fSize;
            }
            txtTimer.text = _timer.ToString("F0") == "0" ? "1" : timer.ToString("F0");
            yield return new WaitForEndOfFrame();
        }
        txtTimer.enabled = false;
        txtTimer.fontSize = fSize;

        for (int i = 0; i < buttonsChoice.childCount; i++)
        {
            Transform trc = buttonsChoice.GetChild(i);
            trc.GetChild(0).GetComponent<Image>().color = Color.white;
        }

        buttonsChoice.gameObject.SetActive(false);

        MyChoice.gameObject.SetActive(true);
        MyChoice.GetChild(0).GetComponent<SpriteRenderer>().sprite = spChoices[choice];

        int ChoiceRobot = Random.Range(0, 3);
        RobotChoice.gameObject.SetActive(true);
        RobotChoice.GetChild(0).GetComponent<SpriteRenderer>().sprite = spChoices[ChoiceRobot];

        MyChoice.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
        RobotChoice.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;

        ResultRound result = ResultRound.Equality;

        switch (choice)
        {
            case 0:
                {
                    switch (ChoiceRobot)
                    {
                        case 0:
                            result = ResultRound.Equality;
                            break;
                        case 1:
                            result = ResultRound.Robot;
                            break;
                        case 2:
                            result = ResultRound.You;
                            break;
                    }
                }
                break;
            case 1:
                {
                    switch (ChoiceRobot)
                    {
                        case 0:
                            result = ResultRound.You;
                            break;
                        case 1:
                            result = ResultRound.Equality;
                            break;
                        case 2:
                            result = ResultRound.Robot;
                            break;
                    }
                }
                break;
            case 2:
                {
                    switch (ChoiceRobot)
                    {
                        case 0:
                            result = ResultRound.Robot;
                            break;
                        case 1:
                            result = ResultRound.You;
                            break;
                        case 2:
                            result = ResultRound.Equality;
                            break;
                    }
                }
                break;
        }

        switch (result)
        {
            case ResultRound.You:
                {
                    winYou++;
                    MyChoice.GetChild(0).GetComponent<SpriteRenderer>().color = Color.green; ;
                    RobotChoice.GetChild(0).GetComponent<SpriteRenderer>().color = Color.gray ;

                    pnlMessage.GetChild(1).gameObject.SetActive(true);
                    pnlMessage.GetChild(1).Find("chWined").GetComponent<TextMeshProUGUI>().text = "You";
                    pnlMessage.GetChild(1).Find("chWined").GetComponent<TextMeshProUGUI>().color=Color.red;
                }
                break;
            case ResultRound.Robot:
                {
                    winRobot++;
                    MyChoice.GetChild(0).GetComponent<SpriteRenderer>().color = Color.gray;
                    RobotChoice.GetChild(0).GetComponent<SpriteRenderer>().color = Color.green;

                    pnlMessage.GetChild(1).gameObject.SetActive(true);
                    pnlMessage.GetChild(1).Find("chWined").GetComponent<TextMeshProUGUI>().text = "Robot";
                    pnlMessage.GetChild(1).Find("chWined").GetComponent<TextMeshProUGUI>().color = Color.blue;
                }
                break;
            case ResultRound.Equality:
                {
                    MyChoice.GetChild(0).GetComponent<SpriteRenderer>().color = Color.gray;
                    RobotChoice.GetChild(0).GetComponent<SpriteRenderer>().color = Color.gray;

                    pnlMessage.GetChild(2).gameObject.SetActive(true);
                }
                break;
        }

        txtWinYou.text = winYou.ToString();
        txtWinRobot.text = winRobot.ToString();

        if (winRobot == 5 || winYou == 5)
        {
            pnlMessage.GetChild(3).gameObject.SetActive(true);
        }
        else
        {
            yield return new WaitForSeconds(2);
            pnlMessage.GetChild(1).gameObject.SetActive(false);
            pnlMessage.GetChild(2).gameObject.SetActive(false);
            MyChoice.gameObject.SetActive(false);
            RobotChoice.gameObject.SetActive(false);

            buttonsChoice.gameObject.SetActive(true);
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

public enum ResultRound
{
    You,
    Robot,
    Equality
}