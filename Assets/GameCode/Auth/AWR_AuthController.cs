using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase;
using Google;
using UnityEngine.UI;

public class AWR_AuthController : MonoBehaviour
{
    FirebaseAuth auth;
    FirebaseApp app;
    string google_WebClientID = "184287527321-1lmdqo4nuku543cl5osm2k1cpl89tqj2.apps.googleusercontent.com";
    private GoogleSignInConfiguration google_config;

    [SerializeField]
    private Button SIWG_Button;

    // Start is called before the first frame update
    void Start()
    {
        google_config = new GoogleSignInConfiguration {
            WebClientId = google_WebClientID,
            RequestIdToken = true
        };
        InitializeFirebase();
        SIWG_Button.onClick.AddListener(onCLick_SignInWithGoogle);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitializeFirebase(){
        //CheckForDependencies();
        auth = FirebaseAuth.DefaultInstance;
    }

    public void onCLick_SignInWithGoogle()   
    {
      Debug.Log("Clicked To Sign In With Google");
        GoogleSignIn.Configuration = google_config;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;

#if !UNITY_EDITOR
        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(
        GoogleOnAuthFinished);
#else
        Debug.Log("NEED TO LOG THE USER INTO THE EDITOR");
#endif
    }

    void GoogleOnAuthFinished(Task<GoogleSignInUser> task){
        if (task.IsFaulted) {
        using (IEnumerator<System.Exception> enumerator =
                task.Exception.InnerExceptions.GetEnumerator()) {
          if (enumerator.MoveNext()) {
            GoogleSignIn.SignInException error =
                    (GoogleSignIn.SignInException)enumerator.Current;
            Debug.Log("Got Error: " + error.Status + " " + error.Message);
          } else {
            Debug.Log("Got Unexpected Exception?!?" + task.Exception);
          }
        }
      } else if(task.IsCanceled) {
        Debug.Log("Canceled");
      } else  {
        Debug.Log("Welcome: " + task.Result.DisplayName + "!");
        exchangeGoogleTokenForFirebaseToken(task.Result.IdToken);
      }
    }

    private void exchangeGoogleTokenForFirebaseToken(string googleIdToken){
        Credential credential = GoogleAuthProvider.GetCredential(googleIdToken, null);
        auth.SignInAndRetrieveDataWithCredentialAsync(credential).ContinueWith(task => {
        if (task.IsCanceled) {
        Debug.LogError("SignInAndRetrieveDataWithCredentialAsync was canceled.");
        return;
        }
        if (task.IsFaulted) {
            Debug.LogError("SignInAndRetrieveDataWithCredentialAsync encountered an error: " + task.Exception);
            return;
        }

        Firebase.Auth.AuthResult result = task.Result;
        Debug.LogFormat("User signed in successfully: {0} ({1})",
        result.User.DisplayName, result.User.UserId);
        });
    }

    private void CheckForDependencies(){
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
        var dependencyStatus = task.Result;
        if (dependencyStatus == Firebase.DependencyStatus.Available) {
            // Create and hold a reference to your FirebaseApp,
            // where app is a Firebase.FirebaseApp property of your application class.
            app = Firebase.FirebaseApp.DefaultInstance;

            // Set a flag here to indicate whether Firebase is ready to use by your app.
        } else {
            UnityEngine.Debug.LogError(System.String.Format(
            "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
            // Firebase Unity SDK is not safe to use here.
  }
});
    }
}
