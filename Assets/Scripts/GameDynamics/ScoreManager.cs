using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public int totalPoints = 0;
    public int pointsPerCollision = 100;
    // Start is called before the first frame update
    public TextMeshPro scoreText;

    public void AddPoints(MyCollider myCollider)
    {
        
        if(myCollider.tag == "projectile"){
            totalPoints += pointsPerCollision;
            scoreText.text = "Score: \n" + totalPoints.ToString();
        }

    }

    
}
