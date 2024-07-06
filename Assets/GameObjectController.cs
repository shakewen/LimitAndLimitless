using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class GameObjectController : MonoBehaviour
{



    public GameObject controller;

    //�ƶ���ľ��
    public GameObject Floor;

    //�ƶ��ĺ���ǽ��
    public GameObject CombineWall;

    public Transform FloorStartPoint;

    public Transform FloorEndPoint;

    public Transform WallStartPoint;

    public Transform WallEndPoint;

    public List<GameObject> interactGameObject;



    private bool isPressedState = false;

    private bool isColorState = false;

    private bool isMoveFloor = false;

    private bool isRobing = false;

    private bool isMoveWall = false;


    [SerializeField] Sprite[] sprites;


    void Start()
    {

        controller = GameObject.Find("BookShelf");

        if (Floor != null)
        {
            Floor = controller.transform.Find("Floor").gameObject;
            FloorStartPoint = Floor.transform.GetChild(1);
            FloorEndPoint = Floor.transform.GetChild(0);
            FloorStartPoint.parent = null;
            FloorEndPoint.parent = null;
        }

        if (CombineWall != null)
        {
            CombineWall =controller.transform.Find("CombineWall").gameObject;
            WallStartPoint= CombineWall.transform.GetChild(1);
            WallEndPoint = CombineWall.transform.GetChild(0);
            WallStartPoint.parent = null;
            WallEndPoint.parent = null;
        }

        for (int i = 0; i < controller.transform.childCount; i++)
        {
            interactGameObject.Add(controller.transform.GetChild(i).gameObject);
           
        }

       //foreach���г�ʼ����
        foreach (GameObject obj in interactGameObject)
        {
            if (obj.CompareTag("InteractButton"))
            {
                if (obj.name == "InteractButton_Red")
                {
                    obj.transform.Find("OpenDoor").gameObject.SetActive(false);
                }

            }

            if (obj.CompareTag("RodItem"))
            {
                if (obj.name == "�������")
                {
                    obj.transform.GetChild(0).gameObject.SetActive(false);
                }
            }


        }
    }

     void FixedUpdate()
    {

        if (isPressedState)
        {
            if (Floor != null)
            {
                if (isMoveFloor)
                {
                    Floor.transform.position = Vector2.MoveTowards(Floor.transform.position, FloorEndPoint.position, Time.deltaTime * 5f);
                }
                else
                {
                    Floor.transform.position = Vector2.MoveTowards(Floor.transform.position, FloorStartPoint.position, Time.deltaTime * 5f);
                }
            }
        }

        
            if (CombineWall != null)
            {
               if (isMoveWall)
               {
                 if (CombineWall.transform.position != WallEndPoint.position)
                 {
                    CombineWall.transform.position = Vector2.MoveTowards(CombineWall.transform.position, WallEndPoint.position, Time.deltaTime * 5f);
                 }
                    
               }
               else
               {
                 if(CombineWall.transform.position != WallStartPoint.position)
                 {
                    CombineWall.transform.position = Vector2.MoveTowards(CombineWall.transform.position, WallStartPoint.position, Time.deltaTime * 5f);
                 }
                   
               }
            }
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        CheckInteract(collision);

        if (collision.gameObject.tag == "RodItem")
        {
            isRobing = !isRobing;
            //�л�ͼƬ
            collision.GetComponent<SpriteRenderer>().sprite = isRobing? sprites[1] : sprites[0];
            
            if (collision.name == "���˺�")
            {
                //�������������ǽ�����һ������
                collision.transform.GetChild(0).gameObject.SetActive(isRobing? true : false);

            }

            if (collision.name == "������")
            {
                //�������Ž�ȶ���
                collision.transform.GetChild(0).gameObject.SetActive(isRobing ? true : false);
            }

            if (collision.name == "���˻�")
            {
                //�������˺������ʧ���ٴ����˰��ӳ���
                collision.transform.GetChild(0).gameObject.SetActive(isRobing ? true : false);
            }

            if (collision.name == "������")
            {
                //ʹ��ǽ�����Һ���
                isMoveWall=!isMoveWall;
            }


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

           
                    if (collision.name == "InteractButton_Red")
                    {
                        collision.transform.Find("OpenDoor").gameObject.SetActive(isPressedState?true : false);
                        collision.transform.Find("Door").gameObject.SetActive(isPressedState? false : true);
                    }

                    if (collision.name == "InteractButton_Blue")
                    {
                        collision.transform.Find("floor").gameObject.SetActive(isPressedState? false : true);
                    }

                    if(collision.name == "InteractButton_Green")
                    {
                        
                       isMoveFloor = true;

                        
                    }

                    if(collision.name == "InteractButton_Orange")
                    {
                        isMoveFloor = false;
                        
                    }
                

                    
               
            
            }

        if (collision.gameObject.tag == "Transparency")
        {
            isColorState =!isColorState;

            collision.GetComponent<SpriteRenderer>().color=new Color(1,1,1,isColorState?0.65f:1);
        }
       

       
        
    }


    
}
