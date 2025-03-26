using UnityEngine;

namespace Kryz.CharacterStats.Examples
{
	public enum EquipmentType
	{
		Helmet,
		Chest,
		Gloves,
		Boots,
		Weapon1,
		Weapon2,
		Accessory1,
		Accessory2,
	}

	[CreateAssetMenu]
	public class EquippableItem : Item
	{
		public int StrengthBonus;
		public int AgilityBonus;
		public int EnduranceBonus;
		public int VitalityBonus;
		[Space]
		public float StrengthPercentBonus;
		public float AgilityPercentBonus;
		public float EndurancePercentBonus;
		public float VitalityPercentBonus;
		[Space]
		public EquipmentType EquipmentType;

		public void Equip(Character c)
		{
			if (StrengthBonus != 0)
				c.Strength.AddModifier(new StatModifier(StrengthBonus, StatModType.Flat, this));
			if (AgilityBonus != 0)
				c.Agility.AddModifier(new StatModifier(AgilityBonus, StatModType.Flat, this));
			if (EnduranceBonus != 0)
				c.Endurance.AddModifier(new StatModifier(EnduranceBonus, StatModType.Flat, this));
			if (VitalityBonus != 0)
				c.Vitality.AddModifier(new StatModifier(VitalityBonus, StatModType.Flat, this));

			if (StrengthPercentBonus != 0)
				c.Strength.AddModifier(new StatModifier(StrengthPercentBonus, StatModType.PercentMult, this));
			if (AgilityPercentBonus != 0)
				c.Agility.AddModifier(new StatModifier(AgilityPercentBonus, StatModType.PercentMult, this));
			if (EndurancePercentBonus != 0)
				c.Endurance.AddModifier(new StatModifier(EndurancePercentBonus, StatModType.PercentMult, this));
			if (VitalityPercentBonus != 0)
				c.Vitality.AddModifier(new StatModifier(VitalityPercentBonus, StatModType.PercentMult, this));
		}

		public void Unequip(Character c)
		{
			c.Strength.RemoveAllModifiersFromSource(this);
			c.Agility.RemoveAllModifiersFromSource(this);
			c.Endurance.RemoveAllModifiersFromSource(this);
			c.Vitality.RemoveAllModifiersFromSource(this);
		}
	}
}