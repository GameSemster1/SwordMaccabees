
class Horsemen : Soldier{

    public Horsemen(int nation){
		if((Nation)nation == Nation.Jewish){
			base(/*life*/130f, 2f, /*power*/40, 2, 1.5f, 1.5f, /*move*/4.2f, 14f);
			addedPrice(21, 15, 55, 65);
		}
		else if((Nation)nation == Nation.Greek || (Nation)nation == Nation.NonJew){
			base(/*life*/130f, 2.3f, /*power*/40, 2, 1.5f, 1.5f, /*move*/3.75f, 11.95f);
		}
    }
	
	private void addedPrice(int wood, int iron, int wheat, int water){
		Soldier.Price.Wood = wood;
		Soldier.Price.Iron = iron;
		Soldier.Price.Wheat = wheat;
		Soldier.Price.Water = water;
	} 
}