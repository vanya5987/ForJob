class Weapon
{
    public int Damage { get; private set; }
    public int Bullets { get; private set; }

    private int _perShoot = 1;

    public Weapon(int damage, int bullets)
    {
        Damage = damage;
        Bullets = bullets;
    }

    public void Fire(Player player)
    {
        if (player == null)
        {
            new NullReferenceException(nameof(player));
        }

        player.TakeDamage(Damage);
        Bullets -= _perShoot;
    }
}

class Player
{
    public int CurrentHealth { get; private set; }

    private int _minHealth = 0;

    public Player(int currentHealth)
    {
        CurrentHealth = currentHealth;
    }

    public void TakeDamage(int damage)
    {
        if (CurrentHealth - damage >= _minHealth)
        {
            CurrentHealth -= damage;
        }
        else
        {
            CurrentHealth = 0;
            Console.WriteLine("Игрок убит");
        }
    }
}

class Bot
{
    public Weapon Weapon;

    private int _minHealth = 0;

    public Bot(int minHealth)
    {
        _minHealth = minHealth;
    }

    public void OnSeePlayer(Player player)
    {
        if (player == null)
        {
            new NullReferenceException(nameof(player));
        }

        if (player.CurrentHealth > _minHealth)
        {
            Weapon.Fire(player);
        }
    }
}
