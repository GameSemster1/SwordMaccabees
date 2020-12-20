

class SwordSubjects : Soldier{

	public SwordSubjects(srting nation){
		_nation = nation;
		if(0 == nation){
			init(/*life*/50f, 1f, /*power*/10, 1, 1f, 1f, /*move*/1.4f, 3.08f, /*price:*/4, 4, 19, 25);
			addedPrice(4, 4, 19, 25);
		}
		else if(1 == nation || 2 == nation){
			init(/*life*/50f, 1.2f, /*power*/10, 1, 1f, 1f, /*move*/1.2f, 2.8f);
		}	
			
	}
}

private void init(float hp, float armor, int power, int range, float speed_attack,
				float armor_intrusion,float walk, float running){
	Soldier.Life.Hp = hp;
	Soldier.Life.Armor = armor;
	Soldier.PowerAttack.Power = power;
	Soldier.PowerAttack.Range = range;
	Soldier.PowerAttack.Speed_attack = speed_attack;
	Soldier.PowerAttack.Armor_intrusion = armor_intrusion;
	Soldier.Movement.Walk = walk;
	Soldier.Movement.Running = running;
}
private void addedPrice(int wood, int iron, int wheat, int water){
	Soldier.Price.Wood = wood;
	Soldier.Price.Iron = iron;
	Soldier.Price.Wheat = wheat;
	Soldier.Price.Water = water;
} 