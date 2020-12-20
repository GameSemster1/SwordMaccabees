
class Horsemen : Soldier{

    public Horsemen(srting nation){
        _nation = nation;
		if(0 == nation){
			init(/*life*/130f, 2f, /*power*/40, 2, 1.5f, 1.5f, /*move*/4.2f, 14f);
			addedPrice(21, 15, 55, 65);
		}
		else if(1 == nation || 2 == nation){
			init(/*life*/130f, 2.3f, /*power*/40, 2, 1.5f, 1.5f, /*move*/3.75f, 11.95f);
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