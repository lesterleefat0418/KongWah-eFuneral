using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;

public class SelectReligion : MonoBehaviour
{
    public Page pages;
    public CanvasGroup adminLogin;
    public InputField adminPasswordField;
    [HideInInspector]
    public bool showAdminLogin = false;
    public GameObject adminBtn, logoutBtn;
    // Start is called before the first frame update
    void Start()
    {
        if (LoaderConfig.Instance == null)
        {
            SceneManager.LoadScene(0);
            return;
        }

        this.pages.init(null, LoaderConfig.Instance.selectReligionSceneLastPageId);
        if (this.adminBtn != null) this.adminBtn.SetActive(LoaderConfig.Instance != null ? !LoaderConfig.Instance.configData.isLogined : false);
        if (this.logoutBtn != null) this.logoutBtn.SetActive(LoaderConfig.Instance != null ? LoaderConfig.Instance.configData.isLogined : false);
        LoaderConfig.Instance.skipToHuabaoStage = false;
        if (this.adminPasswordField != null) { 
            EventTrigger eventTrigger = adminPasswordField.GetComponent<EventTrigger>();
            EventTrigger.Entry clickEntry = new EventTrigger.Entry();
            clickEntry.eventID = EventTriggerType.PointerClick;
            UnityAction<BaseEventData> clickAction = new UnityAction<BaseEventData>(ShowKeyboard);
            clickEntry.callback.AddListener(clickAction);
            eventTrigger.triggers.Add(clickEntry);
        }
    }


    private void ShowKeyboard(BaseEventData eventData)
    {
        if(VirtualKeyboard.Instance == null)
            return;

        VirtualKeyboard.Instance.ShowOnScreenKeyboard();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setLang(int langId)
    {
        LoaderConfig.Instance.SelectedLanguageId = langId;
        Debug.Log("selected language: " + langId);
    }

    public void changePage(int toPage)
    {
        this.pages.setPage(toPage);
    }

    public void selectReligion(int id)
    {
        LoaderConfig.Instance.SelectedReligionId = id;
        Debug.Log("selected regligion: " + id);
        SceneManager.LoadScene(2);
    }

    public void homeBtn()
    {
        Debug.Log("back to home!");
        LoaderConfig.Instance.SelectedLanguageId = 0;
        LoaderConfig.Instance.SelectedReligionId = 0;
        SceneManager.LoadScene(1);
    }

    public void skipToHuabaoStageBtn()
    {
        Debug.Log("skip to Huabao!");
        LoaderConfig.Instance.SelectedReligionId = 0;
        LoaderConfig.Instance.skipToHuabaoStage = true;
        SceneManager.LoadScene(2);
    }

    public void controlAdminLogin(bool isLogined)
    {
        this.showAdminLogin = isLogined;
        SetUI.Run(this.adminLogin, isLogined, 0f);
    }

    public void loginBtn()
    {
        if (this.adminPasswordField != null && LoaderConfig.Instance != null)
        {
            if (!string.IsNullOrEmpty(this.adminPasswordField.text))
            {
                if (this.adminPasswordField.text == LoaderConfig.Instance.configData.adminPassword)
                {
                    LoaderConfig.Instance.configData.isLogined = true;
                    Debug.Log("Admin logined");
                    this.controlAdminLogin(false);
                    this.adminPasswordField.text = "";
                    VirtualKeyboard.Instance.HideOnScreenKeyboard();
                    if (this.adminBtn != null) this.adminBtn.SetActive(false);
                    if (this.logoutBtn != null) this.logoutBtn.SetActive(true);
                }
                else
                {
                    LoaderConfig.Instance.configData.isLogined = false;
                    Debug.Log("Wrong Password");
                    this.adminPasswordField.text = "";
                }
            }
        }
    }

    public void logout()
    {
        if (LoaderConfig.Instance != null)
        {
            LoaderConfig.Instance.configData.isLogined = false;
            if (this.adminBtn != null) this.adminBtn.SetActive(true);
            if (this.logoutBtn != null) this.logoutBtn.SetActive(false);
        }
    }

}