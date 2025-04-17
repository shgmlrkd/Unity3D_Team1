using UnityEngine;

public class Arrow : Weapon
{
    private void OnEnable()
    {
        _timer = 0.0f;
    }

    void Start()
    {
        
    }

    void Update()
    {
        LifeTimer();
    }
}
