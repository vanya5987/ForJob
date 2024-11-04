class Player
{
    private string _name;
    private int _age;

    public Player(string name, int age)
    {
        if (age <= 0)
            throw new ArgumentOutOfRangeException(nameof(_age));

        _name = name ?? throw new ArgumentNullException(nameof(_name));
        _age = age;
    }

    class Weapon
    {
        private int _weaponDamage;
        private float _weaponCooldown;

        public Weapon(int weaponDamage, float weaponCooldown)
        {
            if (_weaponDamage <= 0)
                throw new ArgumentOutOfRangeException(nameof(_weaponDamage));

            if (_weaponCooldown <= 0)
                throw new ArgumentOutOfRangeException(nameof(_weaponCooldown));

            _weaponDamage = weaponDamage;
            _weaponCooldown = weaponCooldown;
        }

        public void Shoot()
        {
            //attack
        }

        public bool IsReloading()
        {
            throw new NotImplementedException();
        }

    }

    class PlayerMovement
    {
        private float _movementDirectionX;
        private float _movementDirectionY;
        private float _movementSpeed;

        public PlayerMovement(float movementDirectionX, float movementDirectionY, float movementSpeed)
        {
            if (_movementDirectionX <= 0)
                throw new ArgumentOutOfRangeException(nameof(_movementDirectionX));

            if (_movementDirectionY <= 0)
                throw new ArgumentOutOfRangeException(nameof(_movementDirectionY));

            if (_movementSpeed <= 0)
                throw new ArgumentOutOfRangeException(nameof(_movementSpeed));

            _movementDirectionX = movementDirectionX;
            _movementDirectionY = movementDirectionY;
            _movementSpeed = movementSpeed;
        }
        public void Move()
        {
            //Do move
        }
    }
}