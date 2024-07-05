using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectController : MonoBehaviour
{


    public GameObject controller;
    public List<GameObject> interactGameObject;



    private bool isPressedState = false;

    private bool isColorState = false;

    private bool isSpringState = false;

    void Start()
    {

        controller = GameObject.Find("GameObjectController");


        for (int i = 0; i < controller.transform.childCount; i++)
        {
            interactGameObject.Add(controller.transform.GetChild(i).gameObject);
            print(1);
        }

       //foreach进行初始设置
        foreach (GameObject obj in interactGameObject)
        {
            if (obj.name == "OpenDoor")
            {
                obj.SetActive(false);
            }
        }
    }

     void Update()
    {
       
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        CheckInteract(collision);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Spring")
        {
            

           

            
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Spring")
        {
           

           

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        CheckInteract(collision);
    }

 
    void CheckInteract(Collider2D collision)
    {
        
            if (collision.gameObject.tag=="InteractButton")
            {
                isPressedState = !isPressedState;

                collision.GetComponent<Animator>().SetBool("isPressed", isPressedState);

           
                foreach (GameObject obj in interactGameObject)
                {
                    if(obj.name== "Door")
                    {
                        obj.SetActive(isPressedState?false:true);
                    }

                    if (obj.name == "OpenDoor")
                    {
                        obj.SetActive(isPressedState ? true : false);
                    obj.GetComponent<Collider2D>().isTrigger = isPressedState;
                    }
                }
            
            }

        if (collision.gameObject.tag == "Transparency")
        {
            isColorState =!isColorState;

            collision.GetComponent<SpriteRenderer>().color=new Color(1,1,1,isColorState?0.65f:1);
        }

       
        
    }

}
