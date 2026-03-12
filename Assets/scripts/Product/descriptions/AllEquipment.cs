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
            
        };
    }
}
