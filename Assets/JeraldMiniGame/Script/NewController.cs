using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro; // Import the TMPro namespace
using System.Collections;


public class NewController : MonoBehaviour
{
    public MacineFab macine;
    public FabricatorVrCollider fabricatorCollider;
    //public Power power;
    public float speed = 50;
    public int trueRange = 50;
    public Image againPanel, nextPanel, endPanel, levelPanelBack;
    public float LevelCurrentPower;
    public GameObject Anchor;
    //[SerializeField] TextMeshProUGUI TextPower; // Modify to use TextMeshProUGUI
    [SerializeField] TMP_Text minT, maxT, currentText, winORloseText, levelText; // Change to TMP_Text

    Vector3 rotationPoint = Vector3.zero;
    float temp;
    int maxWinD;
    int minWinD;
    public bool hold = false;
    public bool didwin = false;
    public int Lnum = 1;
    public float CCL;
    bool gameEnded = false;


    // Start is called before the first frame update
    void Start()
    {
        gameEnded = false;
        Cursor.lockState = CursorLockMode.None;
        Lnum = PlayerPrefs.GetInt("Level num", 1);

        trueRange -= (Lnum - 1) * 10;
        speed *= 1 + ((float)(Lnum - 1) / 2);
        levelText.text = "speed = " + speed + "    L E V E L " + Lnum + "    range = " + trueRange;

        BackColor();
        SetRange();
    }

    // Update is called once per frame

    public void StartRotate()
    {
        //hold = true;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            hold = true;
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            hold = false;
        }
    }
    public void EndRotate()
    {
        //hold = false;
        WinCheck();
    }

    void Update()
    {

        if (gameEnded && Input.GetKeyDown(KeyCode.L) && winORloseText.text == "WIN")
        {
            ChangeLevel();
        }
        StartRotate();
        if (hold == false)
        {
            EndRotate();
        }

        temp = Mathf.Round(transform.rotation.eulerAngles.z);
        currentText.text = "" + temp;
        if (hold)
        {
            //Anchor.transform.Rotate(Vector3.right, Time.deltaTime * speed); // Rotate around the forward axis (z-axis)
            //transform.RotateAround(rotationPoint, Vector3.forward, speed * Time.deltaTime);
            transform.RotateAround(Anchor.transform.position, Anchor.transform.forward, speed * Time.deltaTime); 
        }
        
        //if (power.currentPower <= 0)
        //{
        //    GotoLevel1();
        //}

        //if (Input.GetKeyDown(KeyCode.P))
        //{
        //    Debug.Log(power.currentPower);
        //}

        UpdatePowerText();
    }


    void BackColor()
    {
        if (levelPanelBack != null)
        {
            switch (Lnum)
            {
                case 1:
                    levelPanelBack.color = new Color32(0, 255, 50, 255);
                    break;
                case 2:
                    levelPanelBack.color = new Color32(0, 255, 228, 255);
                    break;
                case 3:
                    levelPanelBack.color = new Color32(0, 50, 255, 255);
                    break;
                case 4:
                    levelPanelBack.color = new Color32(160, 0, 255, 255);
                    break;
                case 5:
                    levelPanelBack.color = new Color32(255, 0, 130, 255);
                    break;
            }
        }
    }
    void SetRange()
    {
        int winD = Random.Range((trueRange / 2), 360 - (trueRange / 2));
        maxWinD = winD + (trueRange / 2);
        minWinD = winD - (trueRange / 2);

        minT.text = "Min = " + minWinD;
        maxT.text = "Max = " + maxWinD;
    }
    void WinCheck()
    {
        if (temp <= maxWinD && temp >= minWinD)
        {
            winORloseText.text = "WIN";
            gameEnded = true; // Set gameEnded to true when player wins
            if (trueRange > 10)
            {
                Debug.Log("WINERSIA");
                macine._WinORLose.SetActive(true);
                //StartCoroutine(DelayChangeLevel());
            }
                //nextPanel.gameObject.SetActive(true);
             
            else
                endPanel.gameObject.SetActive(true);
        }
        else
        {

            if (macine.IsGameEnded()) // Check if _Wheel is active
            {
                winORloseText.text = "LOSE";
                Debug.Log("LOSER");
                macine._WinORLose.SetActive(true);
                gameEnded = true; // Set gameEnded to true when player loses
                                  //againPanel.gameObject.SetActive(true);
            }
            //againPanel.gameObject.SetActive(true);
        }

        // Stop decreasing power
        //power.canDecreasePower = false;
        //PlayerPrefs.SetFloat("FinalPower", power.currentPower);
    }

    

    public void Next()
    {
        Lnum += 1;
        PlayerPrefs.SetInt("Level num", Lnum);
        SceneManager.LoadScene("Minigame");
    }

    public void Again()
    {
        //PlayerPrefs.SetFloat("FinalPower", power.newfinalPower -= 50);
        SceneManager.LoadScene("Minigame");
    }
    public void GotoLevel1()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("Jerald");
    }

    void UpdatePowerText()
    {
        //TextPower.text = "Power: " + power.currentPower.ToString();
    }

    void ChangeLevel()
    {
        hold = true;
        temp = 0; // Reset the current number to zero
        macine._WinORLose.SetActive(false);
        gameEnded = false; // Reset gameEnded to false

        if (Lnum < 3)
        {
            Lnum += 1;
            UpdateLevelParameters();
            SetRange();
        }
        else
        {
            Item item = fabricatorCollider.GetProduct();
            Destroy(fabricatorCollider.GetProduct().gameObject);
            foreach (ItemData a in item.Data.productContainable)
            {
                a.GetPrefab().GetComponent<Rigidbody>().isKinematic = true;
                Instantiate(a.GetPrefab(), fabricatorCollider._collider.transform.position, Quaternion.identity);

            }
            // Reset everythings
            hold = false;
            temp = 0;
            macine._WinORLose.SetActive(false);
            macine._TextHolder.SetActive(false);
            macine._NextButton.SetActive(false);
            macine._StartButton.SetActive(false);
            Lnum = 1;
            trueRange = 50;
            speed = 50;
            hold = false;
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetInt("Level num", Lnum);
            UpdateLevelParameters();
            SetRange();
        }
    }



    void UpdateLevelParameters()
    {
        trueRange -= (Lnum - 1) * 10;
        speed *= 1 + ((float)(Lnum - 1) / 2);
        levelText.text = "speed = " + speed + "    L E V E L " + Lnum + "    range = " + trueRange;
    }

    IEnumerator DelayChangeLevel()
    {
        yield return new WaitForSeconds(3); // Wait for 3 seconds
        ChangeLevel();
    }

    public void NextButtonToggle()
    {
        if (gameEnded && winORloseText.text == "WIN")
        {
            ChangeLevel();
        }
    }

    public void NextButtonToggleOFF()
    {
        return;
    }
}
