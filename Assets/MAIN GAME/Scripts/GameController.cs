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
using AdvancedDissolve_Example;
using AmazingAssets.AdvancedDissolve;

public class GameController : MonoBehaviour
{
    [Header("Variable")]
    public static GameController instance;
    public int maxLevel;
    public bool isStartGame = false;
    int maxPlusEffect = 0;
    bool isVibrate = false;
    Rigidbody rigid;
    float h, v;
    public float speed;
    Vector3 dir;
    bool isHold = false;
    public List<Rigidbody> pixels = new List<Rigidbody>();
    public float forceFactor;
    public static int totalPixel;
    public float rebuildHeight;
    public List<GameObject> listMeshs = new List<GameObject>();
    float meshHeight;
    float count;
    bool isBuild = false;

    [Header("UI")]
    public GameObject winPanel;
    public GameObject losePanel;
    public Text currentLevelText;
    public Text nextLevelText;
    int currentLevel;
    public Slider levelProgress;
    public Text scoreText;
    public static int score;
    public Canvas canvas;
    public GameObject startGameMenu;
    public InputField levelInput;
    public GameObject nextButton;
    public Text title;
    public Text winMenu_title;
    public Text winMenu_score;

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
    public AdvancedDissolveGeometricCutoutController controllerCutOut;

    private void OnEnable()
    {
        //PlayerPrefs.DeleteAll();
        //DOTween.SetTweensCapacity(5000, 5000);
        Application.targetFrameRate = 60;
        instance = this;
        rigid = GetComponent<Rigidbody>();
        StartCoroutine(delayRefreshInstancer());
        StartCoroutine(delayStart());
    }

    IEnumerator delayStart()
    {
        yield return new WaitForSeconds(0.01f);
        //AnalyticsManager.instance.CallEvent(AnalyticsManager.EventType.StartEvent);

        currentLevel = PlayerPrefs.GetInt("currentLevel");
        currentLevelText.text = currentLevel.ToString();
        nextLevelText.text = (currentLevel + 1).ToString();
        score = 0;
        scoreText.text = score.ToString();
        startGameMenu.SetActive(true);
        title.DOColor(new Color32(255,255,255,0), 3);
        levelProgress.maxValue = totalPixel;
        levelProgress.value = 0;
    }

    private void FixedUpdate()
    {
        if (isStartGame)
        {
            Control();
            for (int i = 0; i < pixels.Count; i++)
            {
                if (pixels[i] != null && pixels[i].GetComponent<Tile>().isMagnet)
                {
                    Vector3 magnetField = magnetPoint.position - pixels[i].position;
                    var dis = Vector3.Distance(magnetPoint.position, pixels[i].position);
                    var disX = Mathf.Abs(magnetPoint.position.x - pixels[i].position.x);
                    if (disX <= 1.5f)
                    {
                        //if (dis <= 5 && !pixels[i].GetComponent<Tile>().isMagnet)
                        //{
                        //    pixels[i].GetComponent<Tile>().isMagnet = true;
                        //    pixels[i].transform.parent = transform;
                        //}
                        pixels[i].AddForce(magnetField * forceFactor * 2 * Time.fixedDeltaTime);

                        //if (pixels[i].transform.childCount > 0)
                        //{
                        //    var disBomb = Vector3.Distance(pixels[i].transform.position, transform.position);
                        //    if (disBomb < 5)
                        //    {
                        //        Lose();
                        //        var prefab = PoolManager.instance.GetObject(PoolManager.NameObject.pixelExplode);
                        //        if (prefab != null)
                        //        {
                        //            prefab.SetActive(true);
                        //            prefab.transform.position = pixels[i].gameObject.transform.position;
                        //            prefab.GetComponent<ParticleSystem>().Play();
                        //        }
                        //        Destroy(pixels[i].gameObject);
                        //        transform.GetChild(0).gameObject.SetActive(false);
                        //        transform.GetChild(1).gameObject.SetActive(false);
                        //        for (int j = 0; j < pixels.Count; j++)
                        //        {
                        //            if (pixels[j] != null)
                        //            {
                        //                AddRemoveInstances.instance.RemoveInstances(pixels[j].GetComponent<GPUInstancerPrefab>());
                        //            }
                        //        }
                        //        break;
                        //    }
                        //}
                    }
                    else
                    {
                        magnetField = new Vector3(magnetField.x, 0, magnetField.z);
                        pixels[i].AddForce(magnetField * forceFactor / disX * Time.fixedDeltaTime);
                    }
                }
                else
                {
                    break;
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
        if(Input.GetMouseButtonDown(0))
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
            //Debug.Log(h + " " + v);
            dir = new Vector3(h, 0, v);
            if (dir != Vector3.zero)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), 15 * Time.deltaTime);
                //transform.rotation = Quaternion.LookRotation(dir);
            }
            transform.Translate(Vector3.forward * Time.deltaTime * (speed + Mathf.Abs(h) * 2 + Mathf.Abs(v) * 2));
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(transform.position.x, 20, transform.position.z - 12), Time.deltaTime * 30);
            //funnel.transform.position = Vector3.MoveTowards(funnel.transform.position, new Vector3(funnel.transform.position.x + h, funnel.transform.position.y, funnel.transform.position.z + v), speed * Time.deltaTime);
        }
        //transform.position = new Vector3(Mathf.Clamp(transform.position.x, -11f, 11f), transform.position.y, Mathf.Clamp(transform.position.z, -4f, 49));

        if (Input.GetMouseButtonUp(0))
        {
            isHold = false;
        }
    }

    public void PlusEffectMethod()
    {
        if (maxPlusEffect < 10)
        {
            Vector3 posSpawn = scoreText.transform.position;
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
        plusVar.transform.DOMoveX(scoreText.transform.position.x, 0.5f);
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
        isStartGame = true;
        isHold = true;
    }

    IEnumerator Win()
    {
        var bomb = GameObject.FindGameObjectsWithTag("Hole");
        var hole = GameObject.FindGameObjectsWithTag("Wall");
        var wall = GameObject.FindGameObjectsWithTag("Wall");
        if(bomb.Length > 0)
        {
            foreach(var item in bomb)
            {
                Destroy(item);
            }
        }
        if (hole.Length > 0)
        {
            foreach (var item in hole)
            {
                Destroy(item);
            }
        }
        if (wall.Length > 0)
        {
            foreach (var item in wall)
            {
                Destroy(item);
            }
        }
        controllerCutOut.xyzPivotPointTransform.position = new Vector3(0,200,0);
        yield return new WaitForSeconds(0.01f);
        if (isStartGame)
        {
            //AnalyticsManager.instance.CallEvent(AnalyticsManager.EventType.EndEvent);
            isStartGame = false;
            losePanel.SetActive(false);
            conffetiSpawn = Instantiate(conffeti);
            winMenu_title.text = "LEVEL " + currentLevel.ToString();
            currentLevel++;
            if (currentLevel > maxLevel)
            {
                currentLevel = 0;
            }
            PlayerPrefs.SetInt("currentLevel", currentLevel);
            winMenu_score.text = score.ToString();
            yield return new WaitForSeconds(0.1f);
            winBG.SetActive(true);
            winBG.GetComponent<MeshRenderer>().material.DOFade(1, 1);
            winPanel.SetActive(true);
            scoreText.gameObject.SetActive(false);
            levelProgress.gameObject.SetActive(false);
            //MapFlowEffect.instance.RunMapFlowEffect();
        }
    }

    IEnumerator delayRefreshInstancer()
    {
        yield return new WaitForSeconds(0.01f);
        AddRemoveInstances.instance.Setup();
    }

    public void Lose()
    {
        if (isStartGame)
        {
            //AnalyticsManager.instance.CallEvent(AnalyticsManager.EventType.EndEvent);
            isStartGame = false;
            StartCoroutine(delayLose());
        }
    }

    IEnumerator delayLose()
    {
        yield return new WaitForSeconds(1);
        losePanel.SetActive(true);
    }

    public void LoadScene()
    {
        //MapFlowEffect.instance.isStop = true;
        winPanel.SetActive(false);
        losePanel.SetActive(false);
        var temp = conffetiSpawn;
        Destroy(temp);
        SceneManager.LoadScene(0);
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
                PlayerPrefs.SetInt("currentLevel", level);
                SceneManager.LoadScene(0);
            }
        }
    }

    public void ButtonNextLevel()
    {
        title.DOKill();
        isStartGame = true;
        currentLevel++;
        if (currentLevel > maxLevel)
        {
            currentLevel = 0;
        }
        PlayerPrefs.SetInt("currentLevel", currentLevel);
        SceneManager.LoadScene(0);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pixel") && !other.GetComponent<Tile>().isCheck && isStartGame && !other.GetComponent<Tile>().isMagnet)
        {
            if (pixels.Count < 400 || other.transform.childCount > 0)
            {
                if (!UnityEngine.iOS.Device.generation.ToString().Contains("5") && !isVibrate)
                {
                    isVibrate = true;
                    StartCoroutine(delayVibrate());
                    MMVibrationManager.Haptic(HapticTypes.LightImpact);
                }
                other.GetComponent<Tile>().isCheck = true;
                other.transform.parent = null;
                other.GetComponent<Rigidbody>().isKinematic = false;
                if (other.transform.childCount == 0)
                {
                    other.GetComponent<BoxCollider>().isTrigger = false;
                    if (other != null)
                    {
                        //other.transform.DOKill();
                        //Sequence mySequence = DOTween.Sequence();
                        //mySequence.Append(other.transform.DOMoveY(10f, 0));
                        //mySequence.Append(other.transform.DOScale(0.5f, 0.5f));
                        //mySequence.AppendInterval(0.5f);
                        //mySequence.Append(other.transform.DOMoveY(10f, 0.5f));
                        //mySequence.AppendCallback(() => {
                        //    // Add your script here
                        //});
                        //mySequence.Append(transform.DORotate(new Vector3(0, 180, 0), 1));

                        other.transform.DOKill();
                        other.GetComponent<Tile>().isMagnet = true;
                        other.transform.parent = transform;
                        other.transform.DOLocalMove(new Vector3(0, 5, 0), 0);
                        //other.transform.parent = null;
                    }
                    pixels.Add(other.GetComponent<Rigidbody>());
                }
            }
        }
    }

    IEnumerator delayBuild(float time)
    {
        yield return new WaitForSeconds(time);
        isBuild = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Pixel") && other.GetComponent<Tile>().isCheck && isStartGame /*&& !other.GetComponent<Tile>().isMagnet*/ && !isBuild)
        {
            other.GetComponent<Tile>().isCheck = false;
            other.GetComponent<Tile>().isMagnet = false;
            other.transform.parent = null;
            if (other.transform.childCount == 0)
            {
                if (other != null)
                {
                    other.transform.DOKill();
                    other.transform.DOMoveY(0.5f, 0.5f);
                }
            }
            pixels.Remove(other.GetComponent<Rigidbody>());
        }
    }

    void delayRemoveMethod(Transform target, int num)
    {
        StartCoroutine(delayRemove(target, num));
    }

    IEnumerator delayRemove(Transform target, int num)
    {
        yield return new WaitForSeconds(0.001f * num);
        if (target != null)
        {
            target.transform.DOKill();        }
        if (!UnityEngine.iOS.Device.generation.ToString().Contains("5") && !isVibrate)
        {
            isVibrate = true;
            StartCoroutine(delayVibrate());
            MMVibrationManager.Haptic(HapticTypes.LightImpact);
        }
        yield return new WaitForSeconds(0.21f);
        if (target != null)
        {
            controllerCutOut.xyzPivotPointTransform.position = new Vector3(0, controllerCutOut.xyzPivotPointTransform.position.y + rebuildHeight / totalPixel, 0);
            count++;
            levelProgress.value = count;
            if (count >= totalPixel)
            {
                StartCoroutine(Win());
            }
            target.transform.DOKill();
            AddRemoveInstances.instance.RemoveInstances(target.GetComponent<GPUInstancerPrefab>());
        }
    }

    IEnumerator delayFixed(GameObject target)
    {
        yield return new WaitForSeconds(1.5f);
        target.transform.parent = transform;
    }
}
