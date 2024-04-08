using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro; // Import the TMPro namespace

public class NewController : MonoBehaviour
{
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
    bool hold;
    public int Lnum = 1;
    public float CCL;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Lnum = PlayerPrefs.GetInt("Level num", 1);

        trueRange -= (Lnum - 1) * 10;
        speed *= 1 + ((float)(Lnum - 1) / 2);
        levelText.text = "speed = " + speed + "    L E V E L " + Lnum + "    range = " + trueRange;

        BackColor();
        SetRange();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            hold = true;
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            hold = false;
        }
        temp = Mathf.Round(transform.rotation.eulerAngles.z);
        currentText.text = "" + temp;

        if (hold)
        {
            //Anchor.transform.Rotate(Vector3.right, Time.deltaTime * speed); // Rotate around the forward axis (z-axis)
            //transform.RotateAround(rotationPoint, Vector3.forward, speed * Time.deltaTime);
            transform.RotateAround(Anchor.transform.position, Anchor.transform.forward, speed * Time.deltaTime);


            Debug.Log(Anchor.transform.position);

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

            if (trueRange > 10)
                nextPanel.gameObject.SetActive(true);
            else
                endPanel.gameObject.SetActive(true);
        }
        else
        {
            winORloseText.text = "LOSE";
            againPanel.gameObject.SetActive(true);
        }

        // Stop decreasing power
        //power.canDecreasePower = false;
        //PlayerPrefs.SetFloat("FinalPower", power.currentPower);
    }

    public void StartRotate()
    {
        hold = true;
    }
    public void EndRotate()
    {
        hold = false;
        WinCheck();
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
}
