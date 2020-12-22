
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

	public Workers(int nation){
		base(/*life*/30f, 1f, /*power*/5, 1, 1.5f, 1f, /*move*/1.12f, 2.8f, /*work*/3.5f, 1, 10);
		if((Nation)nation == Nation.Jewish){
			addedPrice(14, 10);
		}
	}
	private void addedPrice(int wheat, int water){
		Soldier.Price.Wheat = wheat;
		Soldier.Price.Water = water;
	} 
}


