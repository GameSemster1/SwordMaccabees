

class SwordSubjects : Soldier{

	public SwordSubjects(int nation){
		if((Nation)nation == Nation.Jewish){
			base.init(/*life*/50f, 1f, /*power*/10, 1, 1f, 1f, /*move*/1.4f, 3.08f, /*price:*/4, 4, 19, 25);
			addedPrice(4, 4, 19, 25);
		}
		else if((Nation)nation == Nation.Greek || (Nation)nation == Nation.NonJew){
			base.init(/*life*/50f, 1.2f, /*power*/10, 1, 1f, 1f, /*move*/1.2f, 2.8f);
		}	
		
	}

	private void addedPrice(int wood, int iron, int wheat, int water){
		Soldier.Price.Wood = wood;
		Soldier.Price.Iron = iron;
		Soldier.Price.Wheat = wheat;
		Soldier.Price.Water = water;
	} 
	
}

