using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForNextLevelScript : MonoBehaviour
{
    public static ForNextLevelScript Instance;
    public float life;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        Instance = this;
    }

    void Update()
    {
        life = CharacterMovement.instance.GetHealth();
    }
}
