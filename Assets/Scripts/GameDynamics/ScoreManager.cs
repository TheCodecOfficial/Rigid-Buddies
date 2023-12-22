using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Score manager class handles the game scoring.
public class ScoreManager : MonoBehaviour
{
    public int totalPoints = 0;
    public int pointsPerCollision = 100;
    public TextMeshPro scoreText;

    // Called when a ScoringObject is hit.
    public void AddPoints(MyCollider myCollider)
    {
        if(myCollider.tag == "projectile"){
            totalPoints += pointsPerCollision;
            scoreText.text = "Score: " + totalPoints.ToString();
        }
    }
}
