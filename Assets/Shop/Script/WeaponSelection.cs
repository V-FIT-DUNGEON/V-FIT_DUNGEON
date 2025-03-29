using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Kryz.CharacterStats.Examples;

public class WeaponSelection : MonoBehaviour
{
    [Header("Navigation Buttons")]
    [SerializeField] private Button previousButton;
    [SerializeField] private Button nextButton;

    [Header("Equip/Buy Buttons")]
    [SerializeField] private Button equipButton;
    [SerializeField] private Button buyButton;
    [SerializeField] private TMP_Text equipButtonText;
    [SerializeField] private TMP_Text priceText;

    [Header("Weapon Attributes")]
    [SerializeField] private GameObject[] weaponPrefabs; // Array of weapon prefabs
    [SerializeField] private float spawnDistance = 1.5f; // Distance in front of the object
    [SerializeField] private int[] weaponPrices;
    private int currentWeapon;
    private GameObject equippedWeapon; // Currently equipped weapon

    [Header("Sound")]
    [SerializeField] private AudioClip purchaseSound;
    private AudioSource source;

    private Character playerCharacter;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        if (source == null)
        {
            Debug.LogError("AudioSource component is missing on WeaponSelection GameObject!");
        }
        currentWeapon = InventoryManager.instance.currentWeapon;
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
        if (InventoryManager.instance.weaponsUnlocked[currentWeapon])
        {
            equipButton.gameObject.SetActive(true);
            buyButton.gameObject.SetActive(false);

            // Update button text based on whether the weapon is equipped
            equipButtonText.text = (equippedWeapon != null && equippedWeapon.name == weaponPrefabs[currentWeapon].name + "(Clone)") 
                ? "Equipped" 
                : "Equip";
        }
        else
        {
            equipButton.gameObject.SetActive(false);
            buyButton.gameObject.SetActive(true);
            priceText.text = weaponPrices[currentWeapon] + " Coins";
        }
    }

    private void Update()
    {
        if (buyButton.gameObject.activeInHierarchy)
            buyButton.interactable = (playerCharacter != null && playerCharacter.Currency >= weaponPrices[currentWeapon]);
    }

    public void ChangeWeapon(int _change)
    {
        currentWeapon += _change;
        if (currentWeapon > transform.childCount - 1)
            currentWeapon = 0;
        else if (currentWeapon < 0)
            currentWeapon = transform.childCount - 1;

        SelectWeapon(currentWeapon);
    }

    public void BuyWeapon()
    {
        if (playerCharacter != null && playerCharacter.Currency >= weaponPrices[currentWeapon])
        {
            playerCharacter.Currency -= weaponPrices[currentWeapon];
            InventoryManager.instance.weaponsUnlocked[currentWeapon] = true;
            InventoryManager.instance.Save();

            // Play sound only if AudioSource and Clip are valid
            if (source != null && purchaseSound != null)
            {
                source.PlayOneShot(purchaseSound);
            }
            else
            {
                Debug.LogWarning("Cannot play sound: AudioSource or purchase AudioClip is missing.");
            }

            UpdateUI();
        }
    }

    public void EquipWeapon()
    {
        if (!InventoryManager.instance.weaponsUnlocked[currentWeapon])
        {
            Debug.Log("Weapon not unlocked. Equip failed.");
            return;
        }

        Debug.Log("EquipWeapon function called!"); // Debugging step

        // Destroy currently equipped weapon
        if (equippedWeapon != null)
        {
            Debug.Log("Destroying previous weapon: " + equippedWeapon.name);
            Destroy(equippedWeapon);
        }

        // Spawn the new weapon in front of this object
        Vector3 spawnPosition = transform.position + transform.forward * spawnDistance;
        equippedWeapon = Instantiate(weaponPrefabs[currentWeapon], spawnPosition, transform.rotation);
        equippedWeapon.name = weaponPrefabs[currentWeapon].name; // Remove "(Clone)" from name

        Debug.Log("New weapon spawned: " + equippedWeapon.name);

        // Update UI to reflect the equipped status
        equipButtonText.text = "Equipped"; 
        Debug.Log("Equip button text changed to 'Equipped'");

        InventoryManager.instance.currentWeapon = currentWeapon;
        InventoryManager.instance.Save();
    }
}
