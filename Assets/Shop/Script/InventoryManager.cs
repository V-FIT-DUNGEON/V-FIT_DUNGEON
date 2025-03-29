using System;
using System.IO;
using UnityEngine;
using Kryz.CharacterStats.Examples;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance { get; private set; }

    [SerializeField] public int currentWeapon;
    [SerializeField] public int money;
    [SerializeField] public bool[] weaponsUnlocked = new bool[3] { true, false, false };

    [SerializeField] public float strength;
    [SerializeField] public float agility;
    [SerializeField] public float endurance;
    [SerializeField] public float vitality;

    private Character playerCharacter;

#if UNITY_EDITOR
    private readonly string directoryPath = $"{Application.dataPath}/SavedData/InventoryData";
#else
    private readonly string directoryPath = $"{Application.persistentDataPath}/SavedData/InventoryData";
#endif

    private string savePath;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        // Define save path
        savePath = Path.Combine(directoryPath, "PlayerStats.json");

        // Ensure directory exists
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        FindPlayerCharacter();
        Load();
    }

    private void FindPlayerCharacter()
    {
        GameObject playerObject = GameObject.Find("PlayerController");
        if (playerObject != null)
        {
            playerCharacter = playerObject.GetComponent<Character>();
            if (playerCharacter != null)
            {
                playerCharacter.OnCurrencyChanged += UpdateMoney;
            }
        }
    }

    private void UpdateMoney(float newAmount)
    {
        money = Mathf.FloorToInt(newAmount);
    }

    public void Load()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            PlayerData_Storage data = JsonUtility.FromJson<PlayerData_Storage>(json);

            money = data.money;
            currentWeapon = data.currentWeapon;
            weaponsUnlocked = data.weaponsUnlocked ?? new bool[3] { true, false, false };

            strength = data.strength;
            agility = data.agility;
            endurance = data.endurance;
            vitality = data.vitality;

            if (playerCharacter != null)
            {
                playerCharacter.Currency = money;
                playerCharacter.Strength.BaseValue = strength;
                playerCharacter.Agility.BaseValue = agility;
                playerCharacter.Endurance.BaseValue = endurance;
                playerCharacter.Vitality.BaseValue = vitality;
            }

            Debug.Log("Game Loaded Successfully!");
        }
        else
        {
            // **New Player Defaults**
            money = 100;
            currentWeapon = 0;
            weaponsUnlocked = new bool[3] { true, false, false };

            strength = 0;
            agility = 0;
            endurance = 0;
            vitality = 0;

            if (playerCharacter != null)
            {
                playerCharacter.Currency = money;
                playerCharacter.Strength.BaseValue = strength;
                playerCharacter.Agility.BaseValue = agility;
                playerCharacter.Endurance.BaseValue = endurance;
                playerCharacter.Vitality.BaseValue = vitality;
            }

            Debug.Log("No save file found. Created new player data.");
        }
    }

    public void Save()
    {
        PlayerData_Storage data = new PlayerData_Storage
        {
            money = money,
            currentWeapon = currentWeapon,
            weaponsUnlocked = weaponsUnlocked,

            strength = playerCharacter != null ? playerCharacter.Strength.BaseValue : strength,
            agility = playerCharacter != null ? playerCharacter.Agility.BaseValue : agility,
            endurance = playerCharacter != null ? playerCharacter.Endurance.BaseValue : endurance,
            vitality = playerCharacter != null ? playerCharacter.Vitality.BaseValue : vitality
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);

        Debug.Log($"Game Saved: {json}");
    }

    [Serializable]
    public class PlayerData_Storage
    {
        public int currentWeapon;
        public int money;
        public bool[] weaponsUnlocked;

        public float strength;
        public float agility;
        public float endurance;
        public float vitality;
    }
}
