using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    private string targetAnimal;

    private int currentLevel;

    [SerializeField]
    private List<Animals> animalsCurrentlyLive = new List<Animals>();

    [SerializeField]
    private List<Animals> animalsPrefablist = new List<Animals>();
    
    [SerializeField]
    public List<Level> LevelList = new List<Level>();
    
    
    private Level onGoingLevel ;

    public string shootedAnimal;

    [SerializeField] GameObject gameOverScreen;

    public GameObject animalSpawnPoint;



    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void Start()
    {
        CheckForNewLevel(1);

    }


    public void LevelClearCheck()
    {
      if(shootedAnimal == targetAnimal)
      {
            onGoingLevel.isCleard = true;
            CheckForNewLevel(onGoingLevel.LevelNo + 1);
      }
    }

    private void CheckForNewLevel(int levelno)
    {
        Debug.Log("Setting New Level");
        currentLevel = levelno;
        onGoingLevel = GetNewLevel();
        
        if(onGoingLevel != null)
        {
            SetNewLevel();
        }
        else
        {
            CheckifGameOver();
        }
        

    }

    private void SetNewLevel()
    {
        targetAnimal =  onGoingLevel.AnimalToShoot;

        UIManager.instance.taskText.text = "Kill a " + targetAnimal;

        CheckIfAnimalExistToCompleteLevel();
    }
    


   private void CheckifGameOver()
   {
        if(onGoingLevel == null)
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
    //Just adding till developement purpose
    private void OnDestroy()
    {
        foreach(Level level in LevelList)
        {
            level.isCleard = false;
        }

        
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

        if(count == 0)
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
        gameOverScreen.SetActive(true);
        Time.timeScale = 0;
    }
}

