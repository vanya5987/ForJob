//Магические числа нужно всегда заменять на константы...

class Weapon
{
    private int _bullets;

    public bool CanShoot() => _bullets > 0;

    public void Shoot() => _bullets -= 1;
}