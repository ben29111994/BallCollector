using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class LevelGenerator : MonoBehaviour {

    public static LevelGenerator Instance;
    public List<Texture2D> list2DMaps = new List<Texture2D>();
    public List<Color32> listColors = new List<Color32>();
    public Texture2D map;
    public Tile tilePrefab;
    public GameObject parentObject;
    public int numOfStacks;
    public int totalBall;
    public Transform parentGamplay1;
    Transform currentParent;
    Vector3 originalPos;
    float width;

    void Start()
    {
        Instance = this;
        var currentLevel = DataManager.Instance.LevelGame;
        if(currentLevel > list2DMaps.Count)
        {
            currentLevel = 0;
            DataManager.Instance.LevelGame = currentLevel;
        }
        map = list2DMaps[currentLevel];
        originalPos = parentObject.transform.position;
        currentParent = parentObject.transform;
        GameController.totalPixel = 0;
        GenerateMap(map);
        parentObject.transform.position = originalPos;
        parentObject.transform.localScale = Vector3.one * (200 / width);
    }

    private void GenerateMap(Texture2D texture)
    {
        width = texture.width;
        float ratioX = texture.width;
        float ratioY = texture.height;
        float ratio;
        if (ratioY > ratioX)
        {
            ratio = ratioX / ratioY;
        }
        else
        {
            ratio = ratioY / ratioX;
        }
        if(ratio < 0.6f && ratio > 0.4f)
        {
            ratio = 1;
        }

        Vector3 positionTileParent = new Vector3(-((texture.width - 1) * ratio / 2), 0, -((texture.height - 1) * ratio / 2));
        currentParent.localPosition = positionTileParent;

        for (int x = 0; x < texture.width - 1; x++)
        {
            for (int y = 0; y < texture.height - 1; y++)
            {
                totalBall++;
                GenerateTile(texture, x, y, ratio);
            }
        }
    }

    private void GenerateTile(Texture2D texture, int x, int y, float ratio)
    {
        Color pixelColor = texture.GetPixel(x, y);
        if (pixelColor == new Color32(255, 255, 255, 255))
            return;
        if (!listColors.Contains(pixelColor))
        {
            if (listColors.Count < 8)
                listColors.Add(pixelColor);
            else
                return;
        }

        var level = listColors.IndexOf(pixelColor);
        Tile instance;
        instance = Instantiate(tilePrefab);
        instance.transform.SetParent(currentParent);
        Vector3 pos = new Vector3(x - texture.width / 2, 0, y) * ratio;
        //var sizeValue = Random.Range(0.5f, 1);
        //var sizeValue = (level + 1) * 3;
        //Vector3 scale = Vector3.one * ratio * sizeValue * 0.1f;
        Vector3 scale = Vector3.one * Mathf.Pow(1.5f, level) / 10;
        switch(level)
        {
            case 0:
                scale = Vector3.one * Mathf.Pow(1.5f, level) / 4;
                break;
            case 1:
                scale = Vector3.one * Mathf.Pow(1.5f, level) / 5;
                break;
            case 2:
                scale = Vector3.one * Mathf.Pow(1.5f, level) / 6;
                break;
            case 3:
                scale = Vector3.one * Mathf.Pow(1.5f, level) / 7;
                break;
            case 4:
                scale = Vector3.one * Mathf.Pow(1.5f, level) / 8;
                break;
            default:
                scale = Vector3.one * Mathf.Pow(1.5f, level) / 10;
                break;
        }

        if (pixelColor.a == 0 || pixelColor == null)
        {
            pixelColor = new Color32(169, 169, 169, 255);
            Destroy(instance.GetComponent<BoxCollider>());
            Destroy(instance.GetComponent<Rigidbody>());
        }
        else
        {
            //instance.GetComponent<Rigidbody>().drag += 5 * sizeValue;
            GameController.totalPixel++;
        }

        instance.Init(level);
        instance.SetTransfrom(pos, scale);
        instance.SetColor(pixelColor);
    }

}
