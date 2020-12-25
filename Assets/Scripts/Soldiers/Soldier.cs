
public abstract class Soldier{
    public Life life;
    public Attack attack { get; set; }
    public Movement movement { get; set; }
    // public Sight sight { get; set; }

    public Soldier(float hp, float armor,
        float power, float range, float rate, float intrusion,
        float walkingSpeed, float runningSpeed/*, float sight_range*/){
        // float wood, float iron, float wheat, float rock, float water){
        
        life.hp = hp; life.armor = armor;
        attack.range = range; attack.rate = rate;
        attack.power = power; attack.intrusion = intrusion;
        movement.walkingSpeed = walkingSpeed; movement.runningSpeed = runningSpeed;
        // Sight.range = sight_range; 
    }
}