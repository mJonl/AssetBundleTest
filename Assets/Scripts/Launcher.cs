using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FoxGame;
using FoxGame.Asset;
using UnityEngine.U2D;


public class Launcher : MonoBehaviour ,IMsgReceiver{
    public Text Content;
    public Image Img;
    public Button Btn;

    public GameObject MsgBox;

    void OnEnable() {
        MsgDispatcher.GetInstance().Subscribe(GameEvents.Msg_ShowLoadingContent, this);
        MsgDispatcher.GetInstance().Subscribe(GameEvents.Msg_DownloadProgress, this);
        MsgDispatcher.GetInstance().Subscribe(GameEvents.Msg_DownloadFinish, this);
    }

    void OnDisable() {
        MsgDispatcher.GetInstance().UnSubscribe(GameEvents.Msg_ShowLoadingContent, this);
        MsgDispatcher.GetInstance().UnSubscribe(GameEvents.Msg_DownloadProgress, this);
        MsgDispatcher.GetInstance().UnSubscribe(GameEvents.Msg_DownloadFinish, this);
    }

    // Use this for initialization
    void Start () {
        Btn.gameObject.SetActive(false);
        Img.gameObject.SetActive(false);
        MsgBox.SetActive(false);

        Btn.onClick.AddListener(onClickedBtn);

        UpdateVersionManager.Instance.CheckVersion((bool needUpdate) => {
            if (needUpdate) {
                MsgBox.SetActive(true);
            } else {
                UpdateAssetManager.Instance.CheckAsset(() => {
                    MsgDispatcher.GetInstance().Fire(GameEvents.Msg_DownloadFinish);
                });
            }
        });
    }


    public bool OnMsgHandler(string msgName, params object[] args) {
        switch (msgName) {
            case GameEvents.Msg_ShowLoadingContent:
                Content.text = (string)args[0];
                break;
            case GameEvents.Msg_DownloadProgress:
                Content.text = (string)args[0];
                break;
            case GameEvents.Msg_DownloadFinish: {
                    AssetManager.Instance.InitMode(GameConfigs.LoadAssetMode);

                    Content.text = "资源更新完成";
                    Btn.gameObject.SetActive(true);
                    Img.gameObject.SetActive(true);
                   
                }
                break;
        }
        return true;
    }
    

    void onClickedBtn() {

        Img.sprite = LoadMgr.Instance.LoadSprite("comAtlas.spr", "btn1");
        GameObject obj = LoadMgr.Instance.LoadPrefab("Cube.pre");
        Instantiate(obj);
        /*
         AssetManager.Instance.LoadAssetAsync<SpriteAtlas>(GameConfigs.GetSpriteAtlasPath("comAtlas.spr"), (SpriteAtlas sp) => {
           Sprite p = sp.GetSprite("btn2");
            Img.sprite = p;
         });

          AssetManager.Instance.LoadAssetAsync<GameObject>(GameConfigs.GetPrefabsPath("Cube.pre"), (GameObject sp) => {
             GameObject.Instantiate(sp);
          });*/

    }
}
