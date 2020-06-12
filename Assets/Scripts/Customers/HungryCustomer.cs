using UnityEngine;

[System.Serializable]
public class HungryCustomer : Customer
{

    public float DefaultSpeed { get; set; }
    public int HungerValue { get; set; }
    public float TipValue { get; set; }

    //[SerializeField] Sprite one_Hunger;
    //[SerializeField] Sprite two_Hunger;
    //[SerializeField] Sprite three_hunger;

    public HungryCustomer(float movementSpeed, int hungerValue)
    {
        this.MovementSpeed = movementSpeed;
        this.HungerValue = hungerValue;
        DefaultSpeed = movementSpeed;
        //this.Sprite = UpdateSprite();

        SetSpotlightColor();
        SetTipValue();
    }
    //Sets the spotlight color of the customer in relation to hunger level. 
    private void SetSpotlightColor()
    {
        switch (HungerValue)
        {
            case 1:
                spotLightColor = Color.yellow;
                break;
            case 2:
                spotLightColor = Color.magenta;
                break;
            case 3:
                spotLightColor = Color.red;
                break;
            default:
                spotLightColor = Color.white;
                break;
        }
    }
    //Sets the tip value of the customer when fed to completion is what the amount they will pay. 
    private void SetTipValue()
    {
        TipValue = Random.Range(1.5f, 4f) * HungerValue;
    }

    public int UpdateHungerValue()
    {
        HungerValue--;
        return HungerValue;
    }

    ////Sets and updates the sprite of the customer.  
    //public Sprite UpdateSprite()
    //{
    //    switch (HungerValue)
    //    {
    //        case 1:
    //            Sprite = one_Hunger;
    //            return Sprite;
    //        case 2:
    //            Sprite = two_Hunger;
    //            return Sprite;
    //        case 3:
    //            Sprite = three_hunger;
    //            return Sprite;
    //        default:
    //            return Sprite;
    //    }
    //}

}