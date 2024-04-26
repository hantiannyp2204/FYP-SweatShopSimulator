using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro; // Import the TMPro namespace
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Content.Interaction;



public class NewController : MonoBehaviour
{
    [Header("References")]
    public MacineFab macine;
    public FabricatorXRKnob xRKnob;
    public FabricatorCrafting crafting;
    public FabricatorVrCollider fabricatorCollider;
    public GameObject Anchor;
    public PowerForFab power;
    public XRKnob rotationSpeedKnob;
    public XRKnob trueRangeKnob;


    [Header("Game Variables")]
    public float speed = 50;
    public int trueRange = 50;
    Vector3 rotationPoint = Vector3.zero;
    float temp;
    int maxWinD;
    int minWinD;
    public bool hold = false;
    public bool didwin = false;
    public int Lnum = 1;
    public float CCL;
    bool gameEnded = false;

    [Header("Display texts")]
    [SerializeField] TMP_Text minT, maxT, currentText, winORloseText, levelText;
    public Image againPanel, nextPanel, endPanel, levelPanelBack;
    public float LevelCurrentPower;
    
    void Start()
    {
        
        gameEnded = false;
        Cursor.lockState = CursorLockMode.None;
        Lnum = PlayerPrefs.GetInt("Level num", 1);

        trueRange -= (Lnum - 1) * 10;
        speed *= 1 + ((float)(Lnum - 1) / 2);
        levelText.text = "speed = " + speed + "    L E V E L " + Lnum + "    range = " + trueRange;

        //BackColor();
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
        WinCheck(xRKnob.m_Value);
    }

    void Update()
    {
        if (power != null)
        {
            power.CheckIfGotPower();
        }
        if (macine.HasGameStarted == true)
        {
            power._CurrentPower -= 1 * Time.deltaTime;
            float newccPower = power._CurrentPower;
            power.UpdatePowerBar(power._PowerForFab, newccPower);
        }
        


        if (gameEnded && Input.GetKeyDown(KeyCode.L) && winORloseText.text == "WIN")
        {
            ChangeLevel();
        }
        StartRotate();
        if (hold == false)
        {
            EndRotate();
        }


        //Update your true range logic here
       //temp = Mathf.Round(transform.rotation.eulerAngles.z);
       currentText.text = "" + xRKnob.m_Value;
       // if (hold)
       // {
       //     transform.RotateAround(Anchor.transform.position, Anchor.transform.forward, speed * Time.deltaTime);
       // }

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
    public void WinCheck(float WinCheck)
    {
        if (WinCheck <= maxWinD && WinCheck >= minWinD)
        {
            winORloseText.text = "WIN";
            gameEnded = true; // Set gameEnded to true when player wins
            if (trueRange > 10)
            {
                Debug.Log("WINERSIA");
                macine._WinORLose.SetActive(true);          
            } 
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
    }

    

    public void Next()
    {
        Lnum += 1;
        PlayerPrefs.SetInt("Level num", Lnum);
        SceneManager.LoadScene("Minigame");
    }

    void ChangeLevel()
    {
        hold = true;
        temp = 0; // Reset the current number to zero
        macine._WinORLose.SetActive(false);
        gameEnded = false; // Reset gameEnded to false

        if (Lnum < 2)
        {
            Lnum += 1;
            UpdateLevelParameters();
            SetRange();
        }
        else //Win
        {
            crafting.SpawnOBJ(crafting.item2Spawn);
            ResetEverything();
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

    public void ResetEverything()
    {
        // Reset everythings
        crafting.DestroyOBJ();
        crafting.ClearLists();
        macine._WinORLose.SetActive(false);
        macine._TextHolder.SetActive(false);
        macine._NextButton.SetActive(false);
        macine._StartButton.SetActive(false);
        macine.HasGameStarted = false;
        hold = false;
        temp = 0;
        Lnum = 1;
        trueRange = 50;
        speed = 50;
        
    }
}
