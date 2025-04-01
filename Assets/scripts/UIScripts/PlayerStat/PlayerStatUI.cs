using System.IO;
using TMPro;
using UnityEngine;
using Newtonsoft.Json;

public class PlayerStatUI : MonoBehaviour
{
    [Header("Steup UI")]
    [SerializeField] private GameObject playerStatUI;
    [SerializeField] private GameObject playerOverallUI;

    private FileHandler _fileHandler =new FileHandler();
    private User userData;

    [Header("Player Stat UI")]
    [SerializeField] private TextMeshProUGUI strengthText;
    [SerializeField] private TextMeshProUGUI enduranceText;
    [SerializeField] private TextMeshProUGUI vitalityText;
    [SerializeField] private TextMeshProUGUI agilityText;
    [SerializeField] private TextMeshProUGUI currencyText;

    [Header("Player Overall UI")]
    [SerializeField] private TextMeshProUGUI pushupText;
    [SerializeField] private TextMeshProUGUI squatText;

    [SerializeField] string playerStatJsonData;
    [SerializeField] string userFilePath;
    [SerializeField] UserData userDatas;
    // Start is called before the first frame update
    void OnEnable()
    {
        userFilePath = _fileHandler.GetFilePath("PlayerAttributeStats");
        if(File.Exists(userFilePath))
        {
            playerStatJsonData = _fileHandler.LoadData("PlayerAttributeStats");
            userData = JsonConvert.DeserializeObject<User>(playerStatJsonData);
            userDatas = userData.UserDatas["User"];

            //set player stat UI
            strengthText.text = userDatas.GetStat("Strength").ToString();
            enduranceText.text = userDatas.GetStat("Endurance").ToString();
            agilityText.text = userDatas.GetStat("Agility").ToString();
            vitalityText.text = userDatas.GetStat("Vitality").ToString();
            currencyText.text = userDatas.GetStat("Currency").ToString();

            //set player overall UI
            pushupText.text = userDatas.GetOverallExercise("Pushup").ToString();
            squatText.text = userDatas.GetOverallExercise("Squat").ToString();

        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnStatUpdate(bool isActive)
    {
        if (isActive)
        {
            strengthText.text = userDatas.GetStat("Strength").ToString();
            enduranceText.text = userDatas.GetStat("Endurance").ToString();
            agilityText.text = userDatas.GetStat("Agility").ToString();
            vitalityText.text = userDatas.GetStat("Vitality").ToString();
            currencyText.text = userDatas.GetStat("Currency").ToString();
            
        }
        else
        {
            //playerStatUI.SetActive(false);
        }
    }

}
