using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//���νű���ק��Canvas�£�Ȼ������UI�����н����������Σ����ʱ��Ϊ0������ʾ��ť�������ť�˳���Ϸ
public class UI_QuitApplication : MonoBehaviour
{

    
    public Button gameOverButton;



    void Start()
    {
        gameOverButton = transform.Find("GameQuitButton").GetComponent<Button>();
        gameOverButton.onClick.AddListener(Gameover);

    }

    public void Gameover()
    {
        Debug.Log("�˳�����ɹ�");
        Application.Quit();

        Resources.UnloadUnusedAssets();
    }
}
