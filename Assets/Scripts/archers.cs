
class Archers : Soldier{

    public Archers(srting nation){
        _nation = nation;
		if(0 == nation){
			init(/*life*/50f, 0.8f, /*power*/25, 30, 3f, 1.7f, /*move*/0f, 3.64f);
			addedPrice(15, 8, 15, 20);
		}
		else if(1 == nation){
			init(/*life*/50f, 0.9f, /*power*/25, 30, 3f, 1.7f, /*move*/0f, 3.336f);
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