
class SpearBearers : Soldier{

    public SpearBearers(int nation){
		if((Nation)nation == Nation.Jewish){
			base.init(/*life*/50f, 1.2f, /*power*/15, 2, 1.5f, 1.2f, /*move*/1.2f, 2.7f);
			addedPrice(6, 4, 20, 26);
		}
		else if((Nation)nation == Nation.Greek || (Nation)nation == Nation.NonJew){
			base.init(/*life*/50f, 1.4f, /*power*/15, 2, 1.5f, 1.2f, /*move*/1.12f, 2.5f);
		}
    }
	
	private void addedPrice(int wood, int iron, int wheat, int water){
		Soldier.Price.Wood = wood;
		Soldier.Price.Iron = iron;
		Soldier.Price.Wheat = wheat;
		Soldier.Price.Water = water;
	} 
}