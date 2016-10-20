using UnityEngine;
using System.Collections;
using LitJson;

public class Player : MonoBehaviour {
    [SerializeField]
    private Stat health;
    [SerializeField]
    private Stat energy;

	void Start ()
    {
        Initialize();
    }
	void Update ()
    {

    }
    public void Initialize()
    {
        health.Initialize();
        energy.Initialize();

    }
    public void SetCurrentHP(float currectHP)
    {
        health.CurrentValue = currectHP;
    }

    public void SetCurrentMP(float currectMP)
    {
        energy.CurrentValue = currectMP;
    }

    public bool IsEnergyEnough(float cost)
    {
        if(energy.CurrentValue >= cost)
        {
            return true;
        }
        return false;
    }
}
