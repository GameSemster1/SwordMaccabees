
public class PowerWork{
    private float power_work;
    private int speed, quantity;

    public float Power{ get; set; }
    public int Speed{ get; set; }
    public int Quantity{ get; set; }
}

public class Workers : Soldier{
    private PowerWork power_work;

    public PowerWork Power_work{ get; set; }

    public Workers(srting nation){
        _nation = nation;
		init(/*life*/30f, 1f, /*power*/5, 1, 1.5f, 1f, /*move*/1.12f, 2.8f, /*work*/3.5f, 1, 10);
		if(0 == nation){
			addedPrice(14, 10);
		}
    }
}

private void init(float hp, float armor, int power, int range, float speed_attack,
				float armor_intrusion,float walk, float running, float power_work, int speed_work, int quantity){
	Soldier.Life.Hp = hp;
	Soldier.Life.Armor = armor;
	Soldier.PowerAttack.Power = power;
	Soldier.PowerAttack.Range = range;
	Soldier.PowerAttack.Speed_attack = speed_attack;
	Soldier.PowerAttack.Armor_intrusion = armor_intrusion;
	Soldier.Movement.Walk = walk;
	Soldier.Movement.Running = running;
    Workers.PowerWork.Power = power_work;
    Workers.PowerWork.Speed = speed_work;
    Workers.PowerWork.Quantity = quantity;
}
private void addedPrice(int wheat, int water){
	Soldier.Price.Wheat = wheat;
	Soldier.Price.Water = water;
} 
