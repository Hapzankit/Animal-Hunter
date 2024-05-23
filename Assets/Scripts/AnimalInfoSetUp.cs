using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class AnimalInfoSetUp : MonoBehaviour
{
    [SerializeField]
    SOListAnimalStat animalStatList;

    [SerializeField] Image animalImage;
    [SerializeField] TMP_Text animalNameText, animalSpeedText, animalHeightText, animalWeightText, animalAgeText, animalFactText;

    public void SetupStatsUI(string targetAnimal)
    {

        foreach (SOAnimaStat animaStat in animalStatList.AnimaList)
        {
            if(animaStat.animalName == targetAnimal)
            {
                animalImage.sprite = animaStat.animalImage;
                animalNameText.text = animaStat.animalName;
                animalSpeedText.text = "Speed: " + animaStat.animalSpeed;
                animalHeightText.text = "Height: " + animaStat.animalHeight;
                animalWeightText.text = "Weight: " + animaStat.animalWeight;
                animalAgeText.text = "Life span: " + animaStat.animalAge;
                animalFactText.text =  animaStat.animalFacts;
            }
        }
    }
}
