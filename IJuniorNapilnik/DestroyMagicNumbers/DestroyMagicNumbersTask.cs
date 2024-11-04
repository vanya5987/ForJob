class Weapon
{
    private const int BulletsPerShoot = 1;

    private int _bullets;

    private bool CanShoot => _bullets > 0;

    public Weapon(int bullets)
    {
        if (_bullets > 0)
            throw new ArgumentOutOfRangeException(nameof(bullets));

        _bullets = bullets;
    }

    public void Shoot()
    {
        if (CanShoot == false)
            throw new InvalidOperationException();
        else
            _bullets -= BulletsPerShoot;
    }
}