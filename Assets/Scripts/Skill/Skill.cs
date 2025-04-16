using UnityEngine;

public class Skill : MonoBehaviour
{
    protected WeaponData _data;

    protected int _level = 0;
    protected WaitForSeconds _fireInterval;

    public void LevelUp()
    {
        _level++;

        if (_level > 5)
            _level = 5;
    }
}
