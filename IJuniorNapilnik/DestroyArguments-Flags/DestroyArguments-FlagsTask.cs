private void OnEnable()
{
    _enable = true;
    _effects.StartEnableAnimation();
}

private void OnDisable()
{
    _enable = false;
    _pool.Free(this);
}