
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
    public float Speed_attack{ get; set; }
    public float Armor_intrusion{ get; set; }
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

public abstract class Soldier{
    private Life life;
    private PowerAttack power_attack;
    private Movement movement;
    private string nation;
    private Price price;

    public Life Life{ get; set; }
    public PowerAttack Power_attack{ get; set; }
    public Movement Movement{ get; set; }
    public Price Price{ get; set; }
    public string nation{ get; set; }
}
    
