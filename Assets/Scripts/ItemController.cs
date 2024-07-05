using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public GameObject itemController;

    public Color color =new Color(1,1,1,1);

   [SerializeField] private float outlineOffset = 3f;

   //public bool isInteract = false;

    private List<SpriteRenderer> ItemSpriteRenderer;

    //����ÿ���ɲ����������Ƿ���뽻��״̬
    public Dictionary<SpriteRenderer, bool> interactDictionary = new Dictionary<SpriteRenderer, bool>();


    public SpriteRenderer currentSpriteRenderer;

    public SpriteRenderer CurrentInteractSR;

    public MaterialPropertyBlock block;

    

    [SerializeField] private float fadeSpeed = 3.5f;


     void OnEnable()
    {
        
       
    }

    void Start()
    {
        ItemSpriteRenderer = new List<SpriteRenderer>();

        itemController=GameObject.FindWithTag("ItemController");

        block = new MaterialPropertyBlock();



        for (int i = 0; i < itemController.transform.childCount; i++)
        {
           SpriteRenderer itemRenderer = itemController.transform.GetChild(i).GetComponent<SpriteRenderer>();
           
            ItemSpriteRenderer.Add(itemRenderer);

            //���ü�ֵ��Ӧ��ʼΪfalse
            interactDictionary[itemRenderer] = false;
        }

        
    }

     void OnDisable()
    {
        
    }


    void Update()
    {
        if (currentSpriteRenderer == null || interactDictionary[currentSpriteRenderer] == null)
            return;

        //�Ƿ�����������ǰ���壬������ɫ����������ƫ�������񻯳̶ȣ�
        UpdateOutline(interactDictionary[currentSpriteRenderer], outlineOffset);


        IsInteract();
    }

    //�Ƿ�����������������
    void UpdateOutline(bool gradualchangeColor, float outlineOffset)
    {
        

        currentSpriteRenderer.GetPropertyBlock(block);

        block.SetFloat("_Outline", gradualchangeColor ? 1f : 0);
        //ʹ��ȫ�ֱ������ַ���
        block.SetFloat("_SampleOffset", outlineOffset);

        //color.a =Mathf.Clamp(color.a,0,1);

        //////��ʱȡ�����䣬��Ϊ�����ܼ���̫��
        //if (gradualchangeColor)
        //{
        //    color.a += Time.deltaTime * fadeSpeed;
        //}
        //else
        //{
        //    color.a -= Time.deltaTime * fadeSpeed;
        //}
        //���ý���Ч��Ҫ����Ϊȫ�ֱ���
        block.SetColor("_OutlineColor", color);

        currentSpriteRenderer.SetPropertyBlock(block);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        OnPlayerEnterItem(collision,true);

      
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        OnPlayerEnterItem(collision,false);
    }

   
    void OnPlayerEnterItem(Collider2D collision,bool _isInteract)
    {
        foreach (SpriteRenderer itemRenderer in ItemSpriteRenderer)
        {

            if (itemRenderer.gameObject == collision.gameObject)
            {
                
                if (collision.gameObject.layer==(LayerMask.NameToLayer("Transparency")))
                {
                   
                    isTransparecy(itemRenderer, _isInteract);
                }

                //���Ǵ������ɽ�������
                if (collision.CompareTag("Item"))
                {
                    print(itemRenderer.name);

                        //��ǰ�ɱ�����������
                        currentSpriteRenderer = itemRenderer;

                    //ֻ���յ�ǰ����ҽ���ʱ��������ܱ�����
                    if (_isInteract)
                    {
                        //��ǰ�ɽ�������
                        CurrentInteractSR = itemRenderer;

                    }
                        
                    interactDictionary[itemRenderer] = _isInteract;
                }

            }


        }

    }

    void isTransparecy(SpriteRenderer _itemRenderer,bool _isTransparent)
    {
        _itemRenderer.color = new Color(_itemRenderer.color.r, _itemRenderer.color.g, _itemRenderer.color.b, _isTransparent ? 0.65f : 1);
    }

    void IsInteract()
    {
        //��ǰ�ɽ����������ܹ�����
        if (interactDictionary[CurrentInteractSR])
        {
            if (InputManager.GetJInput())
            {
                print("���Խ���");
                //��ʾ�ı�������ı�
                //�ȴ��ı�ȫ��������ɣ��ٴε�����������ر��ı���
            }
        }
    }
}
