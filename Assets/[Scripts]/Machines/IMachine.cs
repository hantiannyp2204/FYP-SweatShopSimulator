using UnityEngine;


public interface IMachines
{
    public void Init()
    {
        GenerateRandomHealth();
    }
    public void GenerateRandomHealth()
    {

    }
    public void GeneratorRunning()
    {
       
    }
    public float GetCurrentHealth();
}