
class ElephantRider : Soldier{

    public ElephantRider(){
        _nation = 1;
		init(/*life*/160f, 2.7f, /*power*/30, 20, 3f, 1.7f, /*move*/2.5f, 5.56f)
    }

	private void init(float hp, float armor, int power, int range, float speed_attack,
					float armor_intrusion,float walk, float running){
		Soldier.Life.Hp = hp;
		Soldier.Life.Armor = armor;
		Soldier.PowerAttack.Power = power;
		Soldier.PowerAttack.Range = range;
		Soldier.PowerAttack.SpeedAttack = speed_attack;
		Soldier.PowerAttack.ArmorIntrusion = armor_intrusion;
		Soldier.Movement.Walk = walk;
		Soldier.Movement.Running = running;
	}
}