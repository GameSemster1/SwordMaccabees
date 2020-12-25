
public abstract class Worker : Soldier{
    public Work work;

    public Worker(float hp, float armor,
        float power, float range, float rate, float intrusion,
        float walkingSpeed, float runningSpeed,
        float workPower, float workRate, float burden) : 
        base(hp, armor, power, range, rate,
                intrusion, walkingSpeed, runningSpeed){
        work.power = workPower;
        work.rate = workRate;
        work.burden = burden;
    }
}
