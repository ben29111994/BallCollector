using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using MoreMountains.NiceVibrations;
using UnityEngine.EventSystems;
using GPUInstancer;
using System.Linq;
using AmazingAssets.AdvancedDissolve;
using TMPro;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    [Header("Variable")]
    public int maxLevel;
    public bool isPlaying = false;
    int maxPlusEffect = 0;
    bool isVibrate = false;
    float h, v;
    public float speed;
    Vector3 dir;
    bool isHold = false;
    public List<Rigidbody> pixels = new List<Rigidbody>();
    public float forceFactor;
    public static int totalPixel;
    public float rebuildHeight;
    public List<GameObject> listMeshs = new List<GameObject>();
    public float CameraOffsetY = 30, CameraOffsetZ = 20;
    public int totalBall;


    [Header("UI")]
    public GameObject winPanel;
    public Text currentLevelText;
    public Text nextLevelText;
    int currentLevel;
    public Slider levelProgress;
    public Text cointTxt;
    public static int coin;
    public Canvas canvas;
    public GameObject startGameMenu;
    public InputField levelInput;
    public GameObject nextButton;
    public Text title;
    public Text winMenu_title;
    public Text winMenu_coin;
    public TextMeshProUGUI timerTxt;
    float timer;

    [Header("Objects")]
    public GameObject funnel;
    public GameObject plusVarPrefab;
    public GameObject conffeti;
    GameObject conffetiSpawn;
    public GameObject mapReader;
    public GameObject map;
    public GameObject mapFlowEffect;
    public Transform magnetPoint;
    public GameObject winBG;
    public GameObject gemAnim;
    public GameObject gameplay2;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        //PlayerPrefs.DeleteAll();
        //DOTween.SetTweensCapacity(5000, 5000);
        Application.targetFrameRate = 60;
        CameraOffsetY = Camera.main.transform.position.y;
        StartCoroutine(delayRefreshInstancer());
        StartCoroutine(delayStart());
    }

    IEnumerator delayStart()
    {
        yield return new WaitForSeconds(0.01f);
        //AnalyticsManager.instance.CallEvent(AnalyticsManager.EventType.StartEvent);
        currentLevel = DataManager.Instance.LevelGame;
        currentLevelText.text = currentLevel.ToString();
        nextLevelText.text = (currentLevel + 1).ToString();
        coin = DataManager.Instance.Coin;
        cointTxt.text = coin.ToString();
        startGameMenu.SetActive(true);
        title.DOColor(new Color32(255,255,255,0), 3);
        levelProgress.maxValue = totalPixel;
        levelProgress.value = 0;
    }

    public void UpdateTimer(int bonusValue)
    {
        timer += bonusValue;
        timerTxt.text = ((int)timer).ToString() + "s";
    }

    private void FixedUpdate()
    {
        if (isPlaying)
        {
            timer -= Time.deltaTime;
            timerTxt.text = ((int)timer).ToString() + "s";
            if(timer <= 5)
            {
                timerTxt.color = Color.red;
                timerTxt.transform.localScale = Vector3.one * 1.5f;
            }
            if(timer <= 0)
            {
                StartCoroutine(Win());
            }
            Control();

            for (int i = 0; i < pixels.Count; i++)
            {
                if (pixels[i] != null && pixels[i].GetComponent<Tile>().isCheck)
                {
                    Vector3 magnetField = magnetPoint.position - pixels[i].position;
                    var dis = Vector3.Distance(magnetPoint.position, pixels[i].position);
                    float distance = Vector3.Distance(magnetPoint.transform.position, pixels[i].transform.position);
                    if (distance <= 2f)
                    {
                        pixels[i].AddForce(magnetField * forceFactor * Time.fixedDeltaTime);
                    }
                    else
                    {
                        if (DataManager.Instance.SizeLevel >= pixels[i].GetComponent<Tile>().ballLevel)
                            pixels[i].AddForce(magnetField * forceFactor / distance * Time.fixedDeltaTime);
                        else
                        {
                            magnetField = new Vector3(magnetField.x, 0, magnetField.z);
                            pixels[i].AddForce(magnetField * forceFactor / distance * Time.fixedDeltaTime);
                        }

                    }
                }
            }
        }
    }

    public void RemovePixel(Rigidbody target)
    {
        if (target != null)
        {
            pixels.Remove(target);
        }
    }

    private void Control()
    {
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(transform.position.x, CameraOffsetY, transform.position.z - CameraOffsetZ), Time.deltaTime * 30);

        if (Input.GetMouseButtonDown(0))
        {
            isHold = true;
        }

        if (Input.GetMouseButton(0) && isHold)
        {
#if UNITY_EDITOR
            h = Input.GetAxis("Mouse X");
            v = Input.GetAxis("Mouse Y");
#endif
#if UNITY_IOS
            if (Input.touchCount > 0)
            {
                h = Input.touches[0].deltaPosition.x / 8;
                v = Input.touches[0].deltaPosition.y / 8;
            }
#endif
            if (Mathf.Abs(h) <= 0.1f)
                h = 0;
            if (Mathf.Abs(v) <= 0.1f)
                v = 0;
            dir = new Vector3(h, 0, v);
            if (dir != Vector3.zero)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), 15 * Time.deltaTime);
            }
            transform.Translate(Vector3.forward * Time.deltaTime * (speed + Mathf.Abs(h) * 2 + Mathf.Abs(v) * 2));
        }

        if (Input.GetMouseButtonUp(0))
        {
            isHold = false;
        }
    }

    public void PlusEffectMethod()
    {
        if (maxPlusEffect < 10)
        {
            Vector3 posSpawn = cointTxt.transform.position;
            StartCoroutine(PlusEffect(posSpawn));
        }
    }

    IEnumerator PlusEffect(Vector3 pos)
    {
        maxPlusEffect++;
        if (!UnityEngine.iOS.Device.generation.ToString().Contains("5") && !isVibrate)
        {
            isVibrate = true;
            StartCoroutine(delayVibrate());
            MMVibrationManager.Haptic(HapticTypes.LightImpact);
        }
        var plusVar = Instantiate(plusVarPrefab);
        plusVar.transform.SetParent(canvas.transform);
        plusVar.transform.localScale = new Vector3(1, 1, 1);
        //plusVar.transform.position = worldToUISpace(canvas, pos);
        plusVar.transform.position = new Vector3(pos.x + Random.Range(-50,50), pos.y + Random.Range(-100, -75), pos.z);
        plusVar.GetComponent<Text>().DOColor(new Color32(255, 255, 255, 0), 1f);
        plusVar.SetActive(true);
        plusVar.transform.DOMoveY(plusVar.transform.position.y + Random.Range(50, 90), 0.5f);
        plusVar.transform.DOMoveX(cointTxt.transform.position.x, 0.5f);
        Destroy(plusVar, 0.5f);
        yield return new WaitForSeconds(0.01f);
        maxPlusEffect--;
    }

    IEnumerator delayVibrate()
    {
        yield return new WaitForSeconds(0.2f);
        isVibrate = false;
    }

    public Vector3 worldToUISpace(Canvas parentCanvas, Vector3 worldPos)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        Vector2 movePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas.transform as RectTransform, screenPos, parentCanvas.worldCamera, out movePos);
        return parentCanvas.transform.TransformPoint(movePos);
    }

    public void ButtonStartGame()
    {
        startGameMenu.SetActive(false);
        isPlaying = true;
        isHold = true;
    }

    public static int coinEarn;
    IEnumerator Win()
    {
        yield return new WaitForSeconds(0.01f);
        if (isPlaying)
        {
            //AnalyticsManager.instance.CallEvent(AnalyticsManager.EventType.EndEvent);
            isPlaying = false;
            conffetiSpawn = Instantiate(conffeti);
            //winMenu_title.text = "LEVEL " + currentLevel.ToString();
            winMenu_coin.text = coin.ToString();
            yield return new WaitForSeconds(0.1f);
            winBG.SetActive(true);
            winBG.GetComponent<MeshRenderer>().material.DOFade(1, 1);
            conffetiSpawn.transform.parent = Camera.main.transform;
            conffetiSpawn.transform.localPosition = new Vector3(winBG.transform.localPosition.x, winBG.transform.localPosition.y - 20, winBG.transform.localPosition.z);
            winPanel.SetActive(true);
            //cointTxt.gameObject.SetActive(false);
            timerTxt.gameObject.SetActive(false);
            levelProgress.gameObject.SetActive(false);
            winMenu_coin.text = (pixels.Count).ToString();
            coinEarn = pixels.Count * 10;
            gemAnim.SetActive(true);
            var bonusCoin = coin + pixels.Count * 10;
            cointTxt.DOCounter(coin, bonusCoin, 1.5f);
            DataManager.Instance.Coin = bonusCoin;
        }
    }

    IEnumerator delayRefreshInstancer()
    {
        yield return new WaitForSeconds(0.01f);
        AddRemoveInstances.instance.Setup();
    }

    public void LoadGamePlay2()
    {
        winPanel.SetActive(false);
        var temp = conffetiSpawn;
        Destroy(temp);
        //SceneManager.LoadScene(0);
        gameplay2.SetActive(true);
        bool isWin = false;
        totalBall = LevelGenerator.Instance.totalBall;
        var collectedPercentage = pixels.Count * 100 / totalBall;
        winBG.SetActive(false );
        if(collectedPercentage > 10)
        {
            isWin = true;
        }

        GamePlayII.Instance.Active_GamePlayII(pixels.Count, isWin);             
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }   

    public void OnChangeMap()
    {
        if (levelInput != null)
        {
            int level = int.Parse(levelInput.text.ToString());
            Debug.Log(level);
            if (level < maxLevel)
            {
                DataManager.Instance.LevelGame = level;
                SceneManager.LoadScene(0);
            }
        }
    }

    public void ButtonNextLevel()
    {
        title.DOKill();
        isPlaying = true;
        currentLevel++;
        if (currentLevel > maxLevel)
        {
            currentLevel = 0;
        }
        DataManager.Instance.LevelGame = currentLevel;
        SceneManager.LoadScene(0);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pixel") && !other.GetComponent<Tile>().isCheck && isPlaying && !other.GetComponent<Tile>().isMagnet)
        {
            pixels.RemoveAll(item => item == null);
            if (!UnityEngine.iOS.Device.generation.ToString().Contains("5") && !isVibrate)
                {
                    isVibrate = true;
                    StartCoroutine(delayVibrate());
                    MMVibrationManager.Haptic(HapticTypes.LightImpact);
                }
                other.GetComponent<Tile>().isCheck = true;

            pixels.Add(other.GetComponent<Rigidbody>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Pixel") && other.GetComponent<Tile>().isCheck && isPlaying)
        {
            other.GetComponent<Tile>().isCheck = false;
            other.GetComponent<Tile>().isMagnet = false;
            other.transform.parent = transform.parent;
            pixels.Remove(other.GetComponent<Rigidbody>());
        }
    }
}
