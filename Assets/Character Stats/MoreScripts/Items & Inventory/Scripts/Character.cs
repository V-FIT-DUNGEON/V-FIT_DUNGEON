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
            }
        }

        public delegate void CurrencyChanged(float newAmount);
        public event CurrencyChanged OnCurrencyChanged;

        [SerializeField] private Inventory inventory;
        [SerializeField] private EquipmentPanel equipmentPanel;
        [SerializeField] private StatPanel statPanel;

        private void Start()
        {
            Currency = SaveManager.instance.money; // Load saved money
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
