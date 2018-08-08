using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Stats_Script : MonoBehaviour {

    private Text survive_Time;
    private Text snake_Length;
    private Text damage_To_Snake;
    private Text skill_Count;



    // Use this for initialization
    void Start () {
        survive_Time = transform.Find("Survive_Time").GetComponent<Text>();
        snake_Length = transform.Find("Snake_Length").GetComponent<Text>();
        damage_To_Snake = transform.Find("Damage_To_Snake").GetComponent<Text>();
        skill_Count = transform.Find("Skill_Count").GetComponent<Text>();


    }

    // Update is called once per frame
    void Update () {
        skill_Count.text = Movement.skill_Count.ToString();
        survive_Time.text = Mathf.Round(Movement.survive_Time).ToString();
        snake_Length.text = Pathfinding.snake_Length.ToString();
        damage_To_Snake.text = Pathfinding.damage_To_Snake.ToString();

    }
}
