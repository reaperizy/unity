using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoginScript : MonoBehaviour
{
    // SerializeField untuk membuat variabel dapat diakses dari Inspector
    [SerializeField] private InputField usernameField; // variabel untuk menyimpan InputField username
    [SerializeField] private InputField passwordField; // variabel untuk menyimpan InputField password
    [SerializeField] private Button loginButton; // variabel untuk menyimpan Button login

    private App app; // variabel untuk menyimpan instance dari script App

    void Start()
    {
        // Mendapatkan instance dari script App yang terdapat di scene
        app = FindObjectOfType<App>();

        // Debug log untuk memeriksa apakah instance telah ditemukan
        if (app == null)
        {
            Debug.LogError("Instance of App not found!");
        }

        // Menambahkan listener ke Button login agar memanggil fungsi Login saat diklik
        loginButton.onClick.AddListener(Login);
    }

    public void Login()
    {
        // Mendapatkan nilai teks dari InputField username dan password
        string email = usernameField.text;
        string password = passwordField.text;

        // Membuat objek SessionProps dan mengisinya dengan properti yang diperlukan
        SessionProps sessionProps = new SessionProps();
        // sessionProps.SomeProperty = "0";

        // Memanggil fungsi Login dari script App dengan parameter username, password, dan objek SessionProps
        app.Login(email, password, sessionProps);
    }

}
