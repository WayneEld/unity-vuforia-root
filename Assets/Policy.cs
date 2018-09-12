using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Policy : MonoBehaviour {


  /* Create a Root Object to store the returned json data in */
    [System.Serializable]
    public class Quotes
    {
        public Quote[] values;
    }

    [System.Serializable]
    public class Quote
    {
        public string package_name;
        public string sum_assured;
        public int base_premium;
        public string suggested_premium;
        public string created_at;
        public string quote_package_id;
        public QuoteModule module;
    }

    [System.Serializable]
    public class QuoteModule
    {
        public string type;
        public string make;
        public string model;
    }

    [Serializable]
    public class Param
    {
        public Param(string _key, string _value)
        {
            key = _key;
            value = _value;
        }
        public string key;
        public string value;
    }

    public string api_key = "";
    public TextMesh text_mesh;
    public string result;

    private void Start()
    {
        //CreateQuote("iPhone 6S 64GB LTE");
    }

    public void CreateQuote(string gadget)
    {
        List<Param> parameters = new List<Param>();
        parameters.Add(new Param("type", "root_gadgets"));
        parameters.Add(new Param("model_name", gadget));

        StartCoroutine(CallAPICoroutine("https://sandbox.root.co.za/v1/insurance/quotes", parameters));
    }

    IEnumerator CallAPICoroutine(String url, List<Param> parameters)
    {

        string auth = api_key + ":";
        auth = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(auth));
        auth = "Basic " + auth;

        WWWForm form = new WWWForm();

        foreach (var param in parameters)
        {
            form.AddField(param.key, param.value);
        }

        UnityWebRequest www = UnityWebRequest.Post(url, form);
        www.SetRequestHeader("AUTHORIZATION", auth);
        yield return www.Send();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.downloadHandler.text);
        }
        else
        {
            Quotes json = JsonUtility.FromJson<Quotes>("{\"values\":" + www.downloadHandler.text + "}");

            //String text = "Make: " + json.values[0].module.make + "\nPremium: R" + (json.values[0].base_premium / 100);
            String text = "Premium: R" + (json.values[0].base_premium / 100);
            Debug.Log("Form upload complete!");
            Debug.Log(text);
             text_mesh.text = text;
           // result = text;


        }
        yield return true;
    }

    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Winning!!!!");
        Debug.Log("Collision detected on " + gameObject.name);
        Debug.Log("Game object " + gameObject.tag);


        if (gameObject.tag == "Player"){
            CreateQuote("iPhone 6S 64GB LTE");
        }


        if (gameObject.tag == "Respawn"){
            CreateQuote("iPhone 7 Plus 128GB LTE");
        }

    }


    void Update()
    {

       // Debug.Log("Here here herre");

        if (Input.GetMouseButtonDown(0))
        {

            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hit)
            {
                Debug.Log("Hit " + hitInfo.transform.gameObject.name);

                if (hitInfo.transform.gameObject.tag == "Player")
                {
                    Debug.Log("Male hit");

                }
                else if (hitInfo.transform.gameObject.tag == "Finish")
                {
                    Debug.Log("Female hit");

                }
                else if (hitInfo.transform.gameObject.tag == "Smoker")
                {
                    Debug.Log("Smoker hit");

                }
                else
                {
                    Debug.Log("nopz");
                }
            }
            else
            {
                Debug.Log("No hit");
            }

        }
            

    }

}
