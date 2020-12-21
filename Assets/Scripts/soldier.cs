
public class Life{
    private float hp, armor;
    
    public float Hp{ get; set; }
    public float Armor{ get; set; }
}

public class PowerAttack{
    private int power, range;
    private double speed_attack, armor_intrusion;

    public int Power{ get; set; }
    public int Range{ get; set; }
    public float SpeedAttack{ get; set; }
    public float ArmorIntrusion{ get; set; }
}

public class Movement{
    private float walk, running;

    public float Walk{ get; set; }
    public float Running{ get; set; }
}

public class Price{
    private int wood, iron, rock, wheat, water;

    public int Wood{ get; set; }
    public int Iron{ get; set; }
    public int Rock{ get; set; }
    public int Wheat{ get; set; }
    public int Water{ get; set; }
}

enum Nation{
    Jewish,
    Greek,
    NonJew;
}



public abstract class Soldier{
    private Life life;
    private PowerAttack power_attack;
    private Movement movement;
    private Price price;
    private Nation nation;

    public Life Life{ get; set; }
    public PowerAttack PowerAttack{ get; set; }
    public Movement Movement{ get; set; }
    public Price Price{ get; set; }
    public Nation Nation{ get; }

    public void init(float hp, float armor, int power, int range, float speed_attack,
					float armor_intrusion,float walk, float running){
		Soldier.Life.Hp = hp;
		Soldier.Life.Armor = armor;
		Soldier.PowerAttack.Power = power;
		Soldier.PowerAttack.Range = range;
		Soldier.PowerAttack.Speed_attack = speed_attack;
		Soldier.PowerAttack.ArmorIntrusion = armor_intrusion;
		Soldier.Movement.Walk = walk;
		Soldier.Movement.Running = running;
	}


}
    
