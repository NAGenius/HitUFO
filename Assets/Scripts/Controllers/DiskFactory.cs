using System.Collections.Generic;
using UnityEngine;

public class MyException : System.Exception
{
    public MyException() { }
    public MyException(string message) : base(message) { }
}

public class Disk_Attributes : MonoBehaviour
{
    public float speedX;
    public float speedY;
    public int score;
}

public class DiskFactory : MonoBehaviour
{
    System.Random random;
    List<GameObject> used_UFO;
    List<GameObject> free_UFO;

    // Start is called before the first frame update
    void Start()
    {
        used_UFO = new List<GameObject>();
        free_UFO = new List<GameObject>();
        random = new System.Random();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject GetDisk(int round)
    {
        GameObject disk;
        if (free_UFO.Count != 0)
        {
            disk = free_UFO[0];
            free_UFO.Remove(disk);
        }
        else
        {
            disk = GameObject.Instantiate(Resources.Load("Prefabs/UFO", typeof(GameObject))) as GameObject;
            disk.AddComponent<Disk_Attributes>();
        }

        int color_UFO = random.Next(1, 4);
        int size_UFO = random.Next(1, 4);
        int speed_UFO = random.Next(1, 4);

        switch (color_UFO)
        {
            case 1:
                disk.GetComponent<Renderer>().material.color = Color.green;
                break;
            case 2:
                disk.GetComponent<Renderer>().material.color = Color.yellow;
                break;
            case 3:
                disk.GetComponent<Renderer>().material.color = Color.red;
                break;
        }
        switch (size_UFO)
        {
            case 1:
                break;
            case 2:
                disk.transform.localScale = new Vector3(1.5f, 0.1f, 1.5f);
                break;
            case 3:
                disk.transform.localScale = new Vector3(1, 0.1f, 1);
                break;
        }
        disk.transform.localEulerAngles = new Vector3(-random.Next(20, 40), 0, 0);
        Disk_Attributes atbt = disk.GetComponent<Disk_Attributes>();
        atbt.score = color_UFO + size_UFO + speed_UFO;
        atbt.speedX = (speed_UFO + round * 0.3f);
        atbt.speedY = (speed_UFO + round * 0.3f) * 2;

        int direction = random.Next(1, 4);
        switch (direction)
        {
            case 1:
                disk.transform.Translate(Camera.main.ScreenToWorldPoint(new Vector3(0, random.Next(0, Camera.main.pixelHeight / 2), 8)));
                break;
            case 2:
                disk.transform.Translate(Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, random.Next(0, Camera.main.pixelHeight / 2), 8)));
                atbt.speedX = atbt.speedX * -1;
                break;
            case 3:
                disk.transform.Translate(Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth/2, 0, 8)));
                atbt.speedX = 0;
                break;
        }
        used_UFO.Add(disk);
        disk.SetActive(true);
        return disk;
    }

    public void FreeDisk(GameObject disk)
    {
        disk.SetActive(false);
        disk.transform.position = new Vector3(0, 0, 0);
        disk.transform.localScale = new Vector3(2, 0.1f, 2);
        if (!used_UFO.Contains(disk))
        {
            throw new MyException("Error: The disk to be freed is not in used_UFO list!");
        }
        used_UFO.Remove(disk);
        free_UFO.Add(disk);
    }
}
