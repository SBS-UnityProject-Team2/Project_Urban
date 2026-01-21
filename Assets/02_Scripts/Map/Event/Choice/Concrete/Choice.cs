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
    
    virtual public bool CanSelect() => true;
    abstract public void ApplyChoice(Player player);
}  