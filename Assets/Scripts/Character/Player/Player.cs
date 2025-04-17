using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.Collections;
using System.Linq;

public class Player : MonoBehaviour
{

    private List<Skill> _skills = new List<Skill>();

    private PlayerData _playerData;
    
    private float _offset = 1.2f;  

    private static Player _instance;
    public static Player Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        _instance = this;
    }

    void Start()
    {                
        _skills.Add(gameObject.AddComponent<BulletSkill>());
        _skills.Add(gameObject.AddComponent<KunaiSkill>());
        _skills.Add(gameObject.AddComponent<FireBallSkill>());
    }

    void Update()
    {
        // 나중을 위하여 . . .
    }    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LeftWall")|| other.CompareTag("RightWall"))
        {
            Vector3 newpos = transform.position;
            newpos.x = -newpos.x;

            if (newpos.x < 0)
            {
                newpos.x += _offset;
            }
            else
            {
               newpos.x -= _offset;
            }

            transform.position = newpos;
        }
        if (other.CompareTag("FrontWall")|| other.CompareTag("BackWall"))
        {
            Vector3 newpos = transform.position;
            newpos.z = -newpos.z;

            if (newpos.z < 0)
            {
                newpos.z += _offset;
            }
            else
            {
                newpos.z -= _offset;
            }

            transform.position = newpos;
        }
    }
}
