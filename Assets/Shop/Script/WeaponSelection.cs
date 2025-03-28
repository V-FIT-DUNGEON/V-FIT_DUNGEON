using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Kryz.CharacterStats.Examples;

public class WeaponSelection : MonoBehaviour
{
    [Header("Navigation Buttons")]
    [SerializeField] private Button previousButton;
    [SerializeField] private Button nextButton;

    [Header("Play/Buy Buttons")]
    [SerializeField] private Button play;
    [SerializeField] private Button buy;
    [SerializeField] private TMP_Text priceText;

    [Header("Weapon Attributes")]
    [SerializeField] private int[] weaponPrices;
    private int currentWeapon;

    [Header("Sound")]
    [SerializeField] private AudioClip purchase;
    private AudioSource source;

    private Character playerCharacter;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        if (source == null)
        {
            Debug.LogError("AudioSource component is missing on WeaponSelection GameObject!");
        }
        currentWeapon = SaveManager.instance.currentWeapon;
        FindPlayerCharacter();
        SelectWeapon(currentWeapon);
    }

    private void FindPlayerCharacter()
    {
        GameObject playerObject = GameObject.Find("PlayerController");
        if (playerObject != null)
        {
            playerCharacter = playerObject.GetComponent<Character>();
        }
    }

    private void SelectWeapon(int _index)
    {
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(i == _index);

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (SaveManager.instance.weaponsUnlocked[currentWeapon])
        {
            play.gameObject.SetActive(true);
            buy.gameObject.SetActive(false);
        }
        else
        {
            play.gameObject.SetActive(false);
            buy.gameObject.SetActive(true);
            priceText.text = weaponPrices[currentWeapon] + " Coins";
        }
    }

    private void Update()
    {
        if (buy.gameObject.activeInHierarchy)
            buy.interactable = (playerCharacter != null && playerCharacter.Currency >= weaponPrices[currentWeapon]);
    }

    public void ChangeWeapon(int _change)
    {
        currentWeapon += _change;
        if (currentWeapon > transform.childCount - 1)
            currentWeapon = 0;
        else if (currentWeapon < 0)
            currentWeapon = transform.childCount - 1;

        SaveManager.instance.currentWeapon = currentWeapon;
        SaveManager.instance.Save();
        SelectWeapon(currentWeapon);
    }

    public void BuyWeapon()
    {
        if (playerCharacter != null && playerCharacter.Currency >= weaponPrices[currentWeapon])
        {
            playerCharacter.Currency -= weaponPrices[currentWeapon];
            SaveManager.instance.weaponsUnlocked[currentWeapon] = true;
            SaveManager.instance.Save();

            // Play sound only if AudioSource and Clip are valid
            if (source != null && purchase != null)
            {
                source.PlayOneShot(purchase);
            }
            else
            {
                Debug.LogWarning("Cannot play sound: AudioSource or purchase AudioClip is missing.");
            }

            UpdateUI();
        }
    }
}
