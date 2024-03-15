using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Platform;
using Oculus.Platform.Models;
using System.Linq;
using TMPro;
using Firebase;
using Firebase.Database;


public class GetUser : MonoBehaviour
{
    class Rank
    {
        public string name;
        public int score;
        

        public Rank(string name, int score)
        {
            this.name = name;
            this.score = score;
        }
    }
    public DatabaseReference reference { get; set; }
    // ���̺귯���� ���� �ҷ��� FirebaseDatabase ���ð�ü�� �����ؼ� ���
    private string leaderboardData = "";
    private string playerRankData = "";
    private int rank;
    public int score;
    public string OculusUserName= "";
    private ulong userId;
    private System.Action onGetUserName;
    private System.Action onLoadFirebaseData;
    public System.Action onRankActive;
    private Dictionary<string, int> sortRank = new Dictionary<string, int>();

   // [SerializeField] GameObject cellGo;
    //[SerializeField] GameObject contentGo;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] TextMeshProUGUI playerRankText;
    [SerializeField] Canvas rankCanvas;
    private void Awake()
    {
        Core.Initialize();
        //this.score = 0;
        this.score = InfoManager.Instance.GetPlayerInfo().maxScore;
    }
    void Start()
    {
        //GameObject go = Instantiate(this.cellGo, this.contentGo.transform);//scrollview�� content�� �ڽ����� cell����
        Oculus.Platform.Entitlements.IsUserEntitledToApplication().OnComplete(EntitlementCallback);
        this.getLoggedInUser();
     
            this.onGetUserName = () => {
           
            var reference = FirebaseDatabase.DefaultInstance.RootReference;
             
            reference.Child("rank").OrderByChild("name").EqualTo(this.OculusUserName).GetValueAsync().ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    if (snapshot.Exists)
                    {//�̹� user�� ����� �����ϴ� ���
                        Debug.Log("user score exist");
                        foreach (var data in snapshot.Children)
                        {
                            IDictionary rank = (IDictionary)data.Value;
                            int databaseScore = int.Parse(rank["score"].ToString());

                            //  if (databaseScore <= this.score)
                            {//���ο� ������ ��ϵ� �������� ���� ũ�ٸ�                              
                             // Debug.Log(rank["score"]);
                                Rank rank2 = new Rank(this.OculusUserName, this.score);
                                string json = JsonUtility.ToJson(rank2);//�����͸� json���·� ��ȯ
                                string key = data.Key;
                                //root�� �ڽ�rank�� key �� �߰�
                                reference.Child("rank").Child(key).SetRawJsonValueAsync(json);//������ Ű�� �ڽ����� json �����͸� ����
                                                                                              //
                                                                                              // Debug.Log("�̸�: " + rank["name"] + ", ����: " + rank["score"]);
                            }
                        }
                    }
                    else
                    {
                        Debug.Log("user's record doesn't exist!");
                        Rank rank = new Rank(this.OculusUserName, score);
                        string json = JsonUtility.ToJson(rank);//�����͸� json���·� ��ȯ
                        string key = reference.Child("rank").Push().Key;
                        //root�� �ڽ�rank�� key �� �߰�
                        reference.Child("rank").Child(key).SetRawJsonValueAsync(json);//������ Ű�� �ڽ����� json �����͸� ����
                    }
                }

            });


        };

        this.onLoadFirebaseData = () =>
        {
           // string str = "";
            var query = FirebaseDatabase.DefaultInstance.GetReference("rank").OrderByChild("score");          
            query.GetValueAsync().ContinueWith(task =>
            {
                
                if (task.IsCompleted)
                { // ���������� �����͸� ����������
                    Debug.Log("loaded");
                    DataSnapshot snapshot = task.Result;
                    // �����͸� ����ϰ��� �Ҷ��� Snapshot ��ü �����
                    int count = (int)snapshot.ChildrenCount+1;
                    //string leaderboardData = "";
                    foreach (DataSnapshot data in snapshot.Children)
                    {
                        IDictionary rank = (IDictionary)data.Value;
                        count--;
                        Debug.LogFormat("�̸�: {0},����: {1}, count:{2}", rank["name"], rank["score"],count);
                        //  StartCoroutine(this.CoWait());
                        string username = rank["name"].ToString();
                        string score = rank["score"].ToString();
                        this.leaderboardData += "  "+count+"  �̸�: "+username+"  ����: "+score+"\n\n";

                        if (this.OculusUserName == rank["name"].ToString())
                        {
                            this.rank = count;
                            Debug.LogFormat("Player's rank is {0}", this.rank);
                            this.playerRankData += "Your Rank  :  "+count;
                            //GameObject go = Instantiate(this.cellGo, this.contentGo.transform);//scrollview�� content�� �ڽ����� cell����
                        }
                    }
                }
            });
        };
        this.onRankActive = () => {
            this.rankCanvas.gameObject.SetActive(true);
            StartCoroutine(this.CoLoadData());
        };
    }
    private IEnumerator CoLoadData()
    {
        yield return new WaitForSeconds(0f);
        this.onLoadFirebaseData();
       
    }
    private void SortRank()
    {
        var sortRank = this.sortRank.OrderByDescending(rank => rank);
        foreach(var data in sortRank)
        {
            Debug.LogFormat("Data: {0}",data.Key);
        }
        //Debug.Log()
    }
    void Update()
    {
        this.text.text = this.leaderboardData;
        this.playerRankText.text = this.playerRankData;

        
    }
    void getLoggedInUser()
    {
        Users.GetLoggedInUser().OnComplete(getUserCallback);
     


    }
    void getUserCallback(Message<User> msg)
    {
        if (!msg.IsError)
        {
             User user = msg.Data;
           
            Debug.LogFormat("UserID: {0},DisplayName : {1}",user.ID,user.DisplayName);
            this.OculusUserName = user.DisplayName;
            Users.Get(msg.Data.ID).OnComplete(message =>
            {
                if (!message.IsError)
                {
                    Oculus.Platform.Models.User user = message.GetUser();
                    this.OculusUserName = user.DisplayName;
                    Debug.LogFormat("UserID2: {0},DisplayName : {1}", user.ID, user.DisplayName);
                    this.onGetUserName();
                    //StartCoroutine(this.CoLoadData());
                }
                else
                {
                    var e = message.GetError();
                }
            });

        }
        else
        {
            Error error = msg.GetError();
        }
    }
    void EntitlementCallback(Message msg)
    {
        if (msg.IsError) // User failed entitlement check
        {
            // Implements a default behavior for an entitlement check failure -- log the failure and exit the app.
            Debug.LogError("You are NOT entitled to use this app.");
            UnityEngine.Application.Quit();
        }
        else // User passed entitlement check
        {
            // Log the succeeded entitlement check for debugging.
            Debug.Log("You are entitled to use this app.");
        }
    }
}
