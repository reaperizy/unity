using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoginScript : MonoBehaviour {

    // Input fields for username and password
    public InputField usernameField;
    public InputField passwordField;

    // Button for login
    public Button loginButton;

    // Script for connecting to the API
    public App appScript;

    // Method to be called when the login button is clicked
    public void Login() {
        // Get the username and password from the input fields
        string username = usernameField.text;
        string password = passwordField.text;

        // Check if the username and password are not empty
        if (username != "" && password != "") {
            // Call the app script to send the login request to the API
            appScript.Login(username, password);
        }
        else {
            // Display an error message if the username or password is empty
            Debug.Log("Please enter your username and password.");
        }
    }

    // Start is called before the first frame update
    void Start() {
        // Get the app script component from the game object
        appScript = GetComponent<App>();

        // Add a listener to the login button to call the Login method when clicked
        loginButton.onClick.AddListener(Login);
    }
}
