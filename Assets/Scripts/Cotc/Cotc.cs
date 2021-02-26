using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CotcSdk;

public class Cotc : MonoBehaviour
{
    // Singleton patern since juste one player
    public static Cotc instance;
    public Cloud cloud;
    public Gamer gamer;
    DomainEventLoop Loop = null;

    void Awake() {
      // Singleton instanciation
      if (instance != null) {
          Destroy(gameObject);
      } else {
        instance = this;
      }
    }

    public void init() {
      CotcGameObject _Cotc = gameObject.GetComponent<CotcGameObject>();
      _Cotc.GetCloud()
        .Catch(ex => {
            Debug.LogError(ex.ToString());
        })
        .Done(_cloud => {
          cloud = _cloud;

            // Http error request handler
            int[] RetryTimes = { 100 , 5000 };
            cloud.HttpRequestFailedHandler = (HttpRequestFailedEventArgs e) => {
                // Store retry count in UserData field (persisted among retries of a given request)
                int retryCount = e.UserData != null ? (int)e.UserData : 0;
                e.UserData = retryCount + 1;
                if (retryCount >= RetryTimes.Length)
                    e.Abort();
                else
                    e.RetryIn(RetryTimes[retryCount]);
            };

            // Authentication
            // First time
          if (!PlayerPrefs.HasKey("GamerId") || !PlayerPrefs.HasKey("GamerSecret")) {
              cloud.LoginAnonymously()
              .Catch(ex => Debug.LogError("Login failed: " + ex.ToString()))
              .Done(gamer => {
                  // Persist returned credentials for next time
                  PlayerPrefs.SetString("GamerId", gamer.GamerId);
                  PlayerPrefs.SetString("GamerSecret", gamer.GamerSecret);
                  DidLogin(gamer);
              });
          }
          else {
            cloud.Login(
                "anonymous",
                networkId: PlayerPrefs.GetString("GamerId"),
                networkSecret: PlayerPrefs.GetString("GamerSecret")
                )
              .Catch(ex => Debug.LogError("Login failed: " + ex.ToString()))
              .Done(gamer => {
                  // ... (logged in)
                  DidLogin(gamer);
            });
          }
        });
    }

    void DidLogin(Gamer newGamer) {
      gamer = newGamer;

      // Another loop was running; unless you want to keep multiple users active, stop the previous
      if (Loop != null)
          Loop.Stop();
      Loop = gamer.StartEventLoop();
      Loop.ReceivedEvent += Loop_ReceivedEvent;
    }

    void Loop_ReceivedEvent(DomainEventLoop sender, EventLoopArgs e) {
        Debug.Log("Received event of type " + e.Message.Type + ": " + e.Message.ToJson());
    }
}
