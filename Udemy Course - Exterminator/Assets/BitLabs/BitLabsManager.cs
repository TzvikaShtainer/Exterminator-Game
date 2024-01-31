using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BitLabsManager : MonoBehaviour
{
    public static BitLabsManager instance;
    [SerializeField] string bitLabToken; //293551bc-c68b-4637-925d-34394f415fcd
    private string userID;
    // Start is called before the first frame update
    void Awake()
    {
        userID = SystemInfo.deviceUniqueIdentifier;

        if (instance != null)
        {
            return;
        }
        else
        {
            instance = this;
        }
      
    }

    private void Start()
    {
        BitLabs.Init(bitLabToken, userID);
        
        // Set the name of the GameObject you would like to receive the feedback
        //BitLabs.SetRewardCallback(this.gameObject.name);

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowBitLabOffers()
    {
        BitLabs.LaunchOfferWall();
    }

    public void RewardCallback(string payout)
    { 
        string payoutWithoutComma = payout.Replace(",", "");
        var amount = 0;
        int.TryParse(payoutWithoutComma, out amount);
        var currentCoin = PlayerPrefs.GetInt("Coin");
        currentCoin += amount;
        PlayerPrefs.SetInt("Coin", currentCoin);
        //PlayfabManager.Instance.AddCurrency(currentCoin);
        Debug.Log("BitLabs Unity onReward: " + payout);
    }
}
