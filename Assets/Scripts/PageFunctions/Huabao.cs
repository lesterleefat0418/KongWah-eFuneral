using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Huabao : MonoBehaviour
{
    public static Huabao Instance =null;
    public bool allowAutoBurn = false;
    public float count = 0f;
    public float delayTime = 3f;
    public Drag[] burnItemsSkip, burnItems;
    public int burnId = 0;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        this.resetAutoBurnTime();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.allowAutoBurn)
        {
            if(this.burnId < this.dragItems.Length)
            {
                if (this.count > 0f)
                {
                    this.count -= Time.deltaTime;
                }
                else
                {
                    if (this.dragItems[this.burnId].enabled)
                        this.dragItems[this.burnId].fireBurn();

                    this.resetAutoBurnTime();
                    this.burnId += 1;
                }
            }
            else
            {
                Debug.Log("Finished auto burn");
                this.allowAutoBurn = false;
            }          
        }
    }

    public void resetAutoBurnTime()
    {
        this.count = this.delayTime;
    }

    Drag[] dragItems
    {
        get
        {
            return LoaderConfig.Instance.skipToHuabaoStage ? this.burnItemsSkip : burnItems;
        }
    }
}