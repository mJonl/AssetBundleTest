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
        /*
        AssetManager.Instance.LoadAssetAsync<SpriteAtlas>(GameConfigs.GetSpriteAtlasPath("ui_atlas"), (SpriteAtlas sp) => {
            Sprite p = sp.GetSprite("icon_2");
            Img.sprite = sp.GetSprite(string.Format("icon_{0}",Random.Range(0,sp.spriteCount-1)));
        });*/
        
        AssetManager.Instance.LoadAssetAsync<Sprite>(GameConfigs.GetSpritePath("icon_0"), (Sprite sp) => {
            Img.sprite = sp;
        });
        //AssetManager.Instance.LoadAssetAsync<Sprite>(GameConfigs.GetSpritePath("icon_1"), (Sprite sp) => {
        //
        ///});
        /*
        AssetManager.Instance.LoadAsset<Sprite>(GameConfigs.GetSpritePath("icon_0"));
        AssetManager.Instance.LoadAsset<Sprite>(GameConfigs.GetSpritePath("icon_1"));*/
        /*
        AssetBundle.LoadFromFile(Application.dataPath + "/StreamingAssets/win/ui/icon_0");
        AssetBundle ab = AssetBundle.LoadFromFile(Application.dataPath + "/StreamingAssets/win/atlas/ui_atlas");
        //GameObject obj = ab.LoadAsset<GameObject>("uiroot");
        SpriteAtlas sp = ab.LoadAsset<SpriteAtlas>("ui_atlas");

        Img.sprite = sp.GetSprite("icon_0");*/
    }
}
