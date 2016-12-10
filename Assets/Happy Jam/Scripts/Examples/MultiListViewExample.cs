using System.Collections;
using System.Collections.Generic;
using UI.MultiListView;
using UnityEngine;
using UnityEngine.UI;

public class MultiListViewExample : MonoBehaviour
{
    public MultiListView ListView;
    public Button Button;
    public bool AddToNewSublist = false;
    public bool AddAsListOfStrings = false;
    public string text1 = "Empty";
    public string text2 = "Empty";
    public bool CreateButton = true;
    public string buttonText = "ButtonText";
    public string buttonClickText = "Button Clicked";

    void Start()
    {
        Button.onClick.AddListener(OnButtonClick);
    }
    void Update()
    {
        if (AddAsListOfStrings) CreateButton = false;
    }

    void OnButtonClick()
    {
        if (AddToNewSublist)
        {
            ListView.AddSublist();
        }

        if (AddAsListOfStrings)
            ListView.AddRow(new List<string>() { text1, text2 });
        else
        {
            List<UI.MultiListView.Data.Item> items = new List<UI.MultiListView.Data.Item>();
            items.Add(new UI.MultiListView.Data.Item(text1));
            items.Add(new UI.MultiListView.Data.Item(text2));
            if (CreateButton)
            {
                UI.MultiListView.Data.Item button = new UI.MultiListView.Data.Item(buttonText, UI.MultiListView.Data.Item.ButtonType.Event);
                ListView.eventsOnClick.AddListener(EventOnClick);
                items.Add(button);

            }

            ListView.AddRow(items);
        }
    }

    void EventOnClick()
    {
        Debug.Log(buttonClickText);
    }


}
