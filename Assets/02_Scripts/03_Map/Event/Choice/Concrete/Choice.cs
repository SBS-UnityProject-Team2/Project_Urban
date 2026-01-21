using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "EventChoice", menuName = "Event/EventChoice", order = 0)]
abstract public class Choice : ScriptableObject
{
    public int eventCode;
    public int choiceCode;
    public int resultCode;
    public string title;
    public string desc;
<<<<<<<< HEAD:Assets/02_Scripts/03_Map/Event/Choice/Concrete/Choice.cs
    
    virtual public bool CanSelect() => true;
    abstract public void ApplyChoice(Player player);
}  
========
}    

>>>>>>>> main:Assets/02_Scripts/03_Map/Event/Choice/Choice.cs
