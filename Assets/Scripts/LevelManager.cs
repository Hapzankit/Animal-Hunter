using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Cinemachine.DocumentationSortingAttribute;
public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public string targetAnimal;

    private int currentLevelNo;


    public List<Animals> animalsCurrentlyLive = new List<Animals>();

    [SerializeField]
    private List<Animals> animalsPrefablist = new List<Animals>();

    [SerializeField]
    public List<Level> LevelList = new List<Level>();


    private Level onGoingLevel;

    public string shootedAnimal;



    public GameObject animalSpawnPoint;

    public AnimalInfoSetUp animalInfoSetUp;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        SetFirstLevel();
        CheckIfAnimalExistToCompleteLevel();

    }


    public void SetFirstLevel()
    {
        currentLevelNo = 1;
        onGoingLevel = GetNewLevel();
        targetAnimal = onGoingLevel.AnimalToShoot;
    }


    public void LevelClearCheck()
    {
        if (shootedAnimal == targetAnimal)
        {
            onGoingLevel.isCleard = true;
            UIManager.instance.levelClearText.text = "You Shot a " + shootedAnimal;
            CheckForNewLevel(onGoingLevel.LevelNo + 1);

        }
    }

    private void CheckForNewLevel(int levelno)
    {

        currentLevelNo = levelno;
        onGoingLevel = GetNewLevel();

        if (onGoingLevel != null)
        {
            SetNewLevel();
            Debug.Log("Setting New Level");
        }
        else
        {
            CheckifGameOver();
            Debug.Log("Showing Game Over Screen");

        }


    }

    private void SetNewLevel()
    {
        targetAnimal = onGoingLevel.AnimalToShoot;

        //SetUP animal Stat UI
        animalInfoSetUp.SetupStatsUI(targetAnimal);

        UIManager.instance.SetLevelClearScreen("Level" + onGoingLevel.LevelNo.ToString());

    }



    private void CheckifGameOver()
    {
        if (onGoingLevel == null)
        {
            Debug.Log("game over");
            StartCoroutine(GameOver());
        }

    }

    private Level GetNewLevel()
    {
        foreach (Level level in LevelList)
        {
            if (!level.isCleard)
            {
                return level;
            }
        }

        Debug.Log("All levels are cleared");
        return null;
    }


    private void CheckIfAnimalExistToCompleteLevel()
    {
        int count = 0;
        foreach (var animal in animalsCurrentlyLive)
        {
            if (animal.gameObject.CompareTag(targetAnimal))
            {
                count++;
                return;
            }
        }

        if (count == 0)
        {
            Animals animalPrefab = GetAnimalToSpawn();

            Instantiate(animalPrefab, animalSpawnPoint.transform.position, Quaternion.identity);
        }
    }


    private Animals GetAnimalToSpawn()
    {
        foreach (Animals animal in animalsPrefablist)
        {
            if (animal.animalName == targetAnimal)
            {
                return animal;
            }
        }

        Debug.Log("Target animal " + targetAnimal + " is not in animalPrefabList ");
        return null;
    }

    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(5f);
        UIManager.instance.GameWonScreen();
        Debug.Log("Game is over");
      
    }

    //Just adding till developement purpose
    private void OnDestroy()
    {
        foreach (Level level in LevelList)
        {
            level.isCleard = false;
            Debug.Log("Resetting level State");
        }
    }

}

