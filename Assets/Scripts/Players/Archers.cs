
class Archers : Soldier{

    public Archers(int nation){
		if((Nation)nation == Nation.Jewish){
			base(/*life*/50f, 0.8f, /*power*/25, 30, 3f, 1.7f, /*move*/0f, 3.64f);
			addedPrice(15, 8, 15, 20);
		}
		else if((Nation)nation == Nation.Greek){
			base(/*life*/50f, 0.9f, /*power*/25, 30, 3f, 1.7f, /*move*/0f, 3.336f);
		}
    }

	private void addedPrice(int wood, int iron, int wheat, int water){
		Soldier.Price.Wood = wood;
		Soldier.Price.Iron = iron;
		Soldier.Price.Wheat = wheat;
		Soldier.Price.Water = water;
	} 
}