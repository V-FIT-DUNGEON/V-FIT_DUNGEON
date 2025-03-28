using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using Kryz.CharacterStats.Examples;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance { get; private set; }

    public int currentWeapon;
    public int money;
    public bool[] weaponsUnlocked = new bool[3] { true, false, false };

    private Character playerCharacter;

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
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            PlayerData_Storage data = (PlayerData_Storage)bf.Deserialize(file);

            money = data.money;
            currentWeapon = data.currentWeapon;
            weaponsUnlocked = data.weaponsUnlocked ?? new bool[3] { true, false, false };

            if (playerCharacter != null)
            {
                playerCharacter.Currency = money; // Apply loaded money
            }

            file.Close();
        }
    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");
        PlayerData_Storage data = new PlayerData_Storage
        {
            money = money,
            currentWeapon = currentWeapon,
            weaponsUnlocked = weaponsUnlocked
        };

        bf.Serialize(file, data);
        file.Close();
    }

    [Serializable]
    class PlayerData_Storage
    {
        public int currentWeapon;
        public int money;
        public bool[] weaponsUnlocked;
    }
}
