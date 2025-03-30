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

    [SerializeField] string playerStatJsonData;
    [SerializeField] string userFilePath;
    // Start is called before the first frame update
    void Start()
    {
        userFilePath = _fileHandler.GetFilePath("PlayerAttributeStats");
        if(File.Exists(userFilePath))
        {
            playerStatJsonData = _fileHandler.LoadData(userFilePath);
            userData = JsonConvert.DeserializeObject<User>(playerStatJsonData);
            strengthText.text = userData.UserDatas["UserStat"].GetStat("Strength").ToString();
            enduranceText.text = userData.UserDatas["UserStat"].GetStat("Endurance").ToString();
            agilityText.text = userData.UserDatas["UserStat"].GetStat("Agility").ToString();
            vitalityText.text = userData.UserDatas["UserStat"].GetStat("Vitality").ToString();
            currencyText.text = userData.UserDatas["UserStat"].GetStat("Currency").ToString();
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
            strengthText.text = userData.UserDatas["UserStat"].GetStat("Strength").ToString();
            enduranceText.text = userData.UserDatas["UserStat"].GetStat("Endurance").ToString();
            agilityText.text = userData.UserDatas["UserStat"].GetStat("Agility").ToString();
            vitalityText.text = userData.UserDatas["UserStat"].GetStat("Vitality").ToString();
            currencyText.text = userData.UserDatas["UserStat"].GetStat("Currency").ToString();
            
        }
        else
        {
            playerStatUI.SetActive(false);
        }
    }

}
