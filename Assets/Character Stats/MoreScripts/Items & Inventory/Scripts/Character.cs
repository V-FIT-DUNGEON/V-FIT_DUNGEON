using UnityEngine;

namespace Kryz.CharacterStats.Examples
{
    public class Character : MonoBehaviour
    {
        public CharacterStat Strength;
        public CharacterStat Agility;
        public CharacterStat Endurance;
        public CharacterStat Vitality;

        private float currency;
        public float Currency
        {
            get => currency;
            set
            {
                currency = value;
                OnCurrencyChanged?.Invoke(currency);
                if (InventoryManager.instance != null)
                {
                    InventoryManager.instance.money = Mathf.FloorToInt(currency);
                }
            }
        }

        public delegate void CurrencyChanged(float newAmount);
        public event CurrencyChanged OnCurrencyChanged;

        [SerializeField] private Inventory inventory;
        [SerializeField] private EquipmentPanel equipmentPanel;
        [SerializeField] private StatPanel statPanel;

        private void Start()
        {
            if (InventoryManager.instance != null)
            {
                // Load saved stats
                Currency = InventoryManager.instance.money;
                Strength.BaseValue = InventoryManager.instance.strength;
                Agility.BaseValue = InventoryManager.instance.agility;
                Endurance.BaseValue = InventoryManager.instance.endurance;
                Vitality.BaseValue = InventoryManager.instance.vitality;

                Debug.Log($"Loaded Stats - Strength: {Strength.BaseValue}, Agility: {Agility.BaseValue}, Endurance: {Endurance.BaseValue}, Vitality: {Vitality.BaseValue}");
            }
            else
            {
                Debug.LogError("InventoryManager instance is NULL. Cannot load player stats.");
            }
        }

        // Property setters ensure stats are saved automatically
        public float StrengthValue
        {
            get => Strength.BaseValue;
            set
            {
                Strength.BaseValue = value;
                if (InventoryManager.instance != null)
                    InventoryManager.instance.strength = value;
            }
        }

        public float AgilityValue
        {
            get => Agility.BaseValue;
            set
            {
                Agility.BaseValue = value;
                if (InventoryManager.instance != null)
                    InventoryManager.instance.agility = value;
            }
        }

        public float EnduranceValue
        {
            get => Endurance.BaseValue;
            set
            {
                Endurance.BaseValue = value;
                if (InventoryManager.instance != null)
                    InventoryManager.instance.endurance = value;
            }
        }

        public float VitalityValue
        {
            get => Vitality.BaseValue;
            set
            {
                Vitality.BaseValue = value;
                if (InventoryManager.instance != null)
                    InventoryManager.instance.vitality = value;
            }
        }

        private void OnApplicationQuit()
        {
            if (InventoryManager.instance != null)
            {
                InventoryManager.instance.Save();
            }
        }
    }
}

		/*private void Awake()
		{
			statPanel.SetStats(Strength, Agility, Endurance, Vitality);
			statPanel.UpdateStatValues();

			inventory.OnItemRightClickedEvent += EquipFromInventory;
			equipmentPanel.OnItemRightClickedEvent += UnequipFromEquipPanel;
		}

		private void EquipFromInventory(Item item)
		{
			if (item is EquippableItem)
			{
				Equip((EquippableItem)item);
			}
		}

		private void UnequipFromEquipPanel(Item item)
		{
			if (item is EquippableItem)
			{
				Unequip((EquippableItem)item);
			}
		}

		public void Equip(EquippableItem item)
		{
			if (inventory.RemoveItem(item))
			{
				EquippableItem previousItem;
				if (equipmentPanel.AddItem(item, out previousItem))
				{
					if (previousItem != null)
					{
						inventory.AddItem(previousItem);
						previousItem.Unequip(this);
						statPanel.UpdateStatValues();
					}
					item.Equip(this);
					statPanel.UpdateStatValues();
				}
				else
				{
					inventory.AddItem(item);
				}
			}
		}

		public void Unequip(EquippableItem item)
		{
			if (!inventory.IsFull() && equipmentPanel.RemoveItem(item))
			{
				item.Unequip(this);
				statPanel.UpdateStatValues();
				inventory.AddItem(item);
			}
		}*/
