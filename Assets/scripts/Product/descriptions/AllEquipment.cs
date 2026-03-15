using System.Collections.Generic;


namespace localizer.product.descriptions
{

    public class ActionsModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
    } 
    public static class ActionsDescriptions
    {
        public static ActionsModel[] _allActionsArray = new ActionsModel[]
        {
            new ActionsModel()
            {
                Name="openShelterDoorText",
                Description = "To open the Localizer door, come closer and grab the door handle with your left hand, then pull it."
            },

            new ActionsModel()
            {
                Name = "manageShelterLights",
                Description = "Once you've entered, on your left, there is a switch, press it to turn on the lights, and again to turn off."
            },
            new ActionsModel()
            {
                Name="openLZZFrontDoor",
                Description = "Now face the front of the localizer and grab the front door handle and pull to open the door."
            }
            
        };
    }
}
